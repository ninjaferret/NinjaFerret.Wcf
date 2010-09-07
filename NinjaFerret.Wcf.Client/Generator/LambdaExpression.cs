/**
 * This file is part of NinjaFerret.Wcf.Client
 * 
 *  NinjaFerret.Wcf.Client is a framework that automatically generates a proxy client
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace NinjaFerret.Wcf.Client.Generator
{
    internal class LambdaExpression
    {
        public LambdaExpression(TypeBuilder parentTypeBuilder, MethodInfo methodInfo, Type serviceInterfaceType)
        {
            _parentTypeBuilder = parentTypeBuilder;
            _methodInfo = methodInfo;
            _serviceType = serviceInterfaceType;
        }
        
        public ConstructorInfo Constructor { get; private set; }
        public MethodInfo Method { get; private set; }
        public TypeBuilder TypeBuilder { get; private set; }
        public Type Type { get; private set; }
        public FieldBuilder ResultField { get; private set; }
        public Type ReturnType { get { return _methodInfo.ReturnType; } }

        public void DefineAsNestedType()
        {
            DefineType();
            DefineConstructor();
            DefineParametersAsFields();
            DefineResultField();
            DefineMethod();
            CreateType();        }

        public FieldInfo GetFieldForParameter(ParameterInfo parameter)
        {
            return _fields.Single(field => field.Name.Equals(parameter.Name));
        }

        private void DefineType()
        {
            var typeName = string.Format(NestedClassName, _methodInfo.Name);
            TypeBuilder = _parentTypeBuilder.DefineNestedType(typeName, NestedTypeAttributes);
        }

        private void DefineConstructor()
        {
            Constructor = TypeBuilder.DefineDefaultConstructor(LambdaConstructorAttributes);
        }

        private void DefineParametersAsFields()
        {
            _fields = _methodInfo.GetParameters().Select(
                parameter =>
                    this.TypeBuilder.DefineField
                        (parameter.Name, 
                        parameter.ParameterType, 
                        ExpressionFieldAttributes)).ToList();
        }

        private void DefineResultField()
        {
            if (HasReturnType())
            {
               ResultField = TypeBuilder.DefineField("result", ReturnType, ExpressionFieldAttributes);
            }
        }

        private bool HasReturnType()
        {
            return !ReturnType.Equals(typeof(void));
        }

        private void DefineMethod()
        {
            var methodName = string.Format("<{0}>b__1", _methodInfo.Name);
            var methodBuilder = TypeBuilder.DefineMethod(methodName, MethodAttribute);
            methodBuilder.InitLocals = true;
            methodBuilder.SetParameters(new[] { _serviceType });
            methodBuilder.SetReturnType(typeof(void));
            var il = new MethodWrapper(methodBuilder);
            if (HasReturnType())
            {
                il.LoadThis();
            }
            il.LoadArgument(1);
            foreach (var parameter in _methodInfo.GetParameters())
            {
                LoadParameter(il, parameter);
            }
            il.CallVirtualMethod(_methodInfo);
            if (HasReturnType())
            {
                il.StoreToField(ResultField);
            }
            il.DoNothing();
            il.Return();
            Method = methodBuilder;
        }

        private void CreateType()
        {
            Type = TypeBuilder.CreateType();
        }

        private void LoadParameter(MethodWrapper il, ParameterInfo parameter)
        {
            var field = GetFieldForParameter(parameter);
            il.LoadThis();
            il.LoadField(field);
        }

        private TypeBuilder _parentTypeBuilder;
        private MethodInfo _methodInfo;
        private Type _serviceType;
        private List<FieldBuilder> _fields;


        private const MethodAttributes LambdaConstructorAttributes = MethodAttributes.Public |
                                                                            MethodAttributes.RTSpecialName |
                                                                            MethodAttributes.SpecialName |
                                                                            MethodAttributes.HideBySig;
        
        private const TypeAttributes NestedTypeAttributes = TypeAttributes.AutoLayout |
                                                            TypeAttributes.AnsiClass |
                                                            TypeAttributes.Sealed |
                                                            TypeAttributes.NestedPrivate |
                                                            TypeAttributes.BeforeFieldInit;

        private const string NestedClassName = "<>__DisplayClass{0}";
        private const MethodAttributes MethodAttribute = MethodAttributes.Public | MethodAttributes.HideBySig;
        private const FieldAttributes ExpressionFieldAttributes = FieldAttributes.Public;

    }
}
