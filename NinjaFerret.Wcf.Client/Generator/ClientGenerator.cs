/**
 * This file is part of WcfClientFactory
 * 
 *  WcfClientFactory is a tool to automatically generate a proxy client
 *  for a specified WCF Service interface.
 *  Copyright (C) 2010  Ian Johnson
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *  
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *  
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
**/
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.ServiceModel;
using System.Threading;
using NinjaFerret.Wcf.Client.CallWrapper;
using NinjaFerret.Wcf.Client.Common.Exception;
using NinjaFerret.Wcf.Exception;

namespace NinjaFerret.Wcf.Client.Generator
{
    public class ClientGenerator : IClientGenerator
    {
        private readonly AssemblyBuilder _assemblyBuilder;
        private readonly ModuleBuilder _moduleBuilder;
        private readonly string _generatedAssemblyName;

        private const TypeAttributes ServiceClientTypeAttributes = TypeAttributes.Public |
                                                                   TypeAttributes.Class |
                                                                   TypeAttributes.AutoLayout |
                                                                   TypeAttributes.AnsiClass |
                                                                   TypeAttributes.BeforeFieldInit;

        private const MethodAttributes MethodImplementationAttributes = MethodAttributes.Public |
                                                                    MethodAttributes.Virtual |
                                                                    MethodAttributes.Final |
                                                                    MethodAttributes.HideBySig;

        private const MethodAttributes ConstructorAttributes = MethodAttributes.Public |
                                                               MethodAttributes.RTSpecialName |
                                                               MethodAttributes.SpecialName |
                                                               MethodAttributes.HideBySig;
        

        public ClientGenerator()
        {
            _generatedAssemblyName = string.Format("WcfClients{0}", Guid.NewGuid().ToString().Replace("-", "_"));
            var assemblyName = new AssemblyName(_generatedAssemblyName);
            _assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_generatedAssemblyName,string.Format("{0}.dll", _generatedAssemblyName),  true);
        }

        public void GenerateAssembly()
        {
            _assemblyBuilder.Save(string.Format("{0}.dll", _generatedAssemblyName));
        }

        public Type CreateType<TServiceInterface>(IExceptionManager exceptionManager) where TServiceInterface : class
        {
            var serviceType = typeof(TServiceInterface);
            ValidateType(serviceType);

            var returnTypeBuilder = GetTypeBuilder(serviceType);

            var callManagerField = DefineServiceCallerField<TServiceInterface>(returnTypeBuilder);
            var exceptionManagerField = returnTypeBuilder.DefineField("serviceCaller", typeof(IExceptionManager),
                                                 FieldAttributes.InitOnly | FieldAttributes.Private);
            DefineConstructor<TServiceInterface>(returnTypeBuilder, callManagerField, exceptionManagerField);
            var methods = serviceType.GetMethods().ToList();
            foreach (var method in methods)
            {
                var expression = new LambdaExpression(returnTypeBuilder, method, serviceType);
                expression.DefineAsNestedType();
                GenerateMethod<TServiceInterface>(method, returnTypeBuilder, expression, callManagerField, exceptionManagerField);
            }

            var result = returnTypeBuilder.CreateType();
            return result;
        }

        private static FieldBuilder DefineServiceCallerField<TServiceInterface>(TypeBuilder returnTypeBuilder) where TServiceInterface : class
        {
            var callerType = typeof (ICallWrapper<TServiceInterface>);
            return returnTypeBuilder.DefineField("serviceCaller", callerType,
                                                 FieldAttributes.InitOnly | FieldAttributes.Private);
        }

        private static void DefineConstructor<TServiceInterface>(TypeBuilder returnTypeBuilder, FieldInfo callerField, FieldInfo exceptionManagerField) where TServiceInterface : class
        {
            var constructorBuilder = returnTypeBuilder.DefineConstructor(ConstructorAttributes, CallingConventions.HasThis,
                                                                      new[] {typeof (ICallWrapper<TServiceInterface>), typeof(IExceptionManager)});
            var il = new MethodWrapper(constructorBuilder);
            il.LoadThis();
            il.LoadArgument(1);
            il.StoreToField(callerField);
            il.LoadThis();
            il.LoadArgument(2);
            il.StoreToField(exceptionManagerField);
            il.Return();
        }

        private static void GenerateMethod<TServiceInterface>(MethodInfo method, TypeBuilder typeBuilder, LambdaExpression lambdaExpression, FieldInfo callManagerField, FieldBuilder exceptionManagerField) where TServiceInterface : class
        {
            var delegateType = typeof(MakeCallToTheWcfServiceDelegate<TServiceInterface>);
            var callMethodInfo = callManagerField.FieldType.GetMethod("Call", new[] { delegateType });

            var methodBuilder = GetMethodBuilder(typeBuilder, method);
            methodBuilder.InitLocals = true;
            var il = new MethodWrapper(methodBuilder);
            var successful = il.DeclareLabel();
            il.DeclareLocal(lambdaExpression.TypeBuilder);
            DefineReturnTypeLocal(method, il);

            il.CreateObject(lambdaExpression.Constructor);
            il.StoreLocal(0);

            ProcessParameters(method, il, lambdaExpression);
            
            PrepareReturnValue(method, il, lambdaExpression);
            BeginTryBlockIfNecessary(method, il);
            // Load the object onto the stack
            il.LoadThis();
            il.LoadField(callManagerField);
            // Put the lambda expression and method onto the stack
            il.LoadLocal(0);
            il.LoadFunction(lambdaExpression.Method);
            // Call the delegate constructor popping the lambda expression and method off the stack
            
            il.CreateObject(delegateType.GetConstructor(new[] { typeof(object), typeof(IntPtr) }));
            
            // There should now be the call manager and the delegate on the stack
            il.CallVirtualMethod(callMethodInfo);
            il.DoNothing();
            SaveReturnValue(method, il, lambdaExpression);
            il.GotoLabel(successful);
            CatchErrors(il, method, exceptionManagerField);
            il.MarkLabel(successful);
            ExtractReturnValue(il);
            il.Return();
            typeBuilder.DefineMethodOverride(methodBuilder, method);
            return;
        }

        

        private static void DefineReturnTypeLocal(MethodInfo methodInfo, MethodWrapper il)
        {
            if (HasReturnValue(methodInfo))
            {
                il.DeclareLocal(methodInfo.ReturnType);
            }
        }

        private static void ProcessParameters(MethodInfo method, MethodWrapper il, LambdaExpression lambdaExpression)
        {
            byte argument = 1;
            var parameters = method.GetParameters().OrderBy(parameter => parameter.Position);
            foreach (var parameter in parameters)
            {
                var field = lambdaExpression.GetFieldForParameter(parameter);
                il.LoadLocal(0);
                il.LoadArgument(argument);
                il.StoreToField(field);
                argument++;
            }
            il.DoNothing();
        }

        private static void PrepareReturnValue(MethodInfo method, MethodWrapper il, LambdaExpression lambdaExpression)
        {
            if (HasReturnValue(method))
            {

                if (method.ReturnType.IsValueType && method.ReturnType.IsPrimitive)
                {
                    il.LoadLocal(0);
                    EmitDefaultValueType(il, method.ReturnType);
                    il.StoreToField(lambdaExpression.ResultField);
                }
                else if (method.ReturnType.IsValueType && !method.ReturnType.IsPrimitive)
                {
                    il.LoadLocalAddress(1);
                    il.InitObject(method.ReturnType);
                }
                else
                {
                    il.LoadLocal(0);
                    il.LoadNull();
                    il.StoreToField(lambdaExpression.ResultField);
                }

            }
            il.DoNothing();
        }

        private static void ExtractReturnValue(MethodWrapper il)
        {
            il.LoadLocal(1);
        }

        private static void SaveReturnValue(MethodInfo method, MethodWrapper il, LambdaExpression lambdaExpression)
        {
            if (!HasReturnValue(method)) return;
            il.LoadLocal(0);
            il.LoadField(lambdaExpression.ResultField);
            il.StoreLocal(1);
            il.DoNothing();
        }

        private static void EmitDefaultValueType(MethodWrapper il, Type returnType)
        {
            // Load String - OpCodes.Ldstr;
            if (returnType.Equals(typeof(string)))
            {
                il.LoadString(string.Empty);
                return;
            }

            // Push int8 onto stack as int32 - OpCodes.Ldc_I4_S;
            if (returnType.Equals(typeof(short)))
            {
                il.LoadShort(0);
                return;
            }
            // Push int64 onto stack - OpCodes.Ldc_I8;
            if (returnType.Equals(typeof(long)))
            {
                il.LoadLong(0L);
                return;
            }
            // Push float32 onto stack - OpCodes.Ldc_R4;
            if (returnType.Equals(typeof(float)))
            {
                il.LoadFloat(0.0f);
                return;
            }
            // Push float64 onto stack - OpCodes.Ldc_R8;
            if (returnType.Equals(typeof(double)))
            {
                il.LoadDouble(0.0);
                return;
            }
            // DEFAULT Push int32 onto stack - OpCodes.Ldc_I4;
            il.LoadInteger(0);
        }

        private static void BeginTryBlockIfNecessary(MethodInfo methodInfo, MethodWrapper methodWrapper)
        {
            var attributes = methodInfo.GetCustomAttributes(typeof(FaultContractAttribute), true);
            if (attributes.Length > 0)
            {
                methodWrapper.BeginExceptionBlock();
            }
        }

        private static void CatchErrors(MethodWrapper methodWrapper, MethodInfo methodInfo, FieldInfo exceptionManagerField)
        {
            var attributes = methodInfo.GetCustomAttributes(typeof (FaultContractAttribute), true);
            var faultExceptionType = typeof(FaultException<>);
            foreach(var attribute in attributes)
            {
                var typedAttribute = attribute as FaultContractAttribute;
                if (typedAttribute == null)
                    continue;
                var exceptionType = faultExceptionType.MakeGenericType(typedAttribute.DetailType);
                
                methodWrapper.CatchException(exceptionType);
                var localBuilder = methodWrapper.DeclareLocal(exceptionType);
                methodWrapper.StoreLocal(localBuilder);
                methodWrapper.LoadThis();
                methodWrapper.LoadLocal(localBuilder);

                var methodToCall =
                    typeof (IExceptionManager).GetMethod("ProcessFault").MakeGenericMethod(typedAttribute.DetailType);

                methodWrapper.CallVirtualMethod(methodToCall);

                methodWrapper.Throw();


            }
            if (attributes.Length > 0)
            {
                methodWrapper.EndExceptionBlock();
            }
        }

        private static bool HasReturnValue(MethodInfo method)
        {
            return !method.ReturnType.Equals(typeof(void));
        }

        private static void ValidateType(Type type)
        {
            CheckIfTypeIsAnInterface(type);
            CheckIfMethodHasServiceContractAttribute(type);
        }

        private static void CheckIfTypeIsAnInterface(Type type)
        {
            if (!type.IsInterface)
            {
                throw new ServiceTypeNotAnInterfaceException(type);
            }
        }

        private static void CheckIfMethodHasServiceContractAttribute(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(ServiceContractAttribute), true);
            if (attributes.Count() == 0)
            {
                throw new ServiceTypeIsNotMarkedWithServiceContractAttributeException(type);
            }
        }

        private TypeBuilder GetTypeBuilder(Type serviceType)
        {
            var returnType = _moduleBuilder.DefineType(string.Format("{0}Client{1}", serviceType.Name, Guid.NewGuid().ToString().Replace("-", "_")),
                                                      ServiceClientTypeAttributes,
                                                      typeof(object),
                                                      new[] { serviceType });
            return returnType;
        }

        private static MethodBuilder GetMethodBuilder(TypeBuilder returnTypeBuilder, MethodInfo method)
        {
            return returnTypeBuilder.DefineMethod(method.Name,
                                                  MethodImplementationAttributes,
                                                  method.ReturnType,
                                                  method.GetParameters().Select(parameter => parameter.ParameterType).ToArray());
        }

    }
}