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
using System.Reflection;
using System.Reflection.Emit;

namespace NinjaFerret.Wcf.Client.Generator
{
    public class MethodWrapper
    {
        private readonly ILGenerator _ilGenerator;

        public MethodWrapper(ILGenerator ilGenerator)
        {
            this._ilGenerator = ilGenerator;
        }

        public MethodWrapper(ConstructorBuilder constructorBuilder)
            : this(constructorBuilder.GetILGenerator())
        {
            
        }

        public MethodWrapper(MethodBuilder methodBuilder)
            : this(methodBuilder.GetILGenerator())
        {

        }

        public void LoadThis()
        {
           _ilGenerator.Emit(OpCodes.Ldarg_0); 
        }

        public void LoadArgument(byte argument)
        {
            switch (argument)
            {
                case 0:
                    _ilGenerator.Emit(OpCodes.Ldarg_0);
                    break;
                case 1:
                    _ilGenerator.Emit(OpCodes.Ldarg_1);
                    break;
                case 2:
                    _ilGenerator.Emit(OpCodes.Ldarg_2);
                    break;
                case 3:
                    _ilGenerator.Emit(OpCodes.Ldarg_3);
                    break;
                default:
                    _ilGenerator.Emit(OpCodes.Ldarg_S, argument);
                    break;

            }
        }

        public void LoadLocal(short local)
        {
            switch (local)
            {
                case 0:
                    _ilGenerator.Emit(OpCodes.Ldloc_0);
                    break;
                case 1:
                    _ilGenerator.Emit(OpCodes.Ldloc_1);
                    break;
                case 2:
                    _ilGenerator.Emit(OpCodes.Ldloc_2);
                    break;
                case 3:
                    _ilGenerator.Emit(OpCodes.Ldloc_3);
                    break;
                default:
                    _ilGenerator.Emit(OpCodes.Ldloc_S, local);
                    break;
            }
        }

        public void StoreLocal(byte local)
        {
            switch(local)
            {
                case 0:
                    _ilGenerator.Emit(OpCodes.Stloc_0);
                    break;
                case 1:
                    _ilGenerator.Emit(OpCodes.Stloc_1);
                    break;
                case 2:
                    _ilGenerator.Emit(OpCodes.Stloc_2);
                    break;
                case 3:
                    _ilGenerator.Emit(OpCodes.Stloc_3);
                    break;
                default:
                    _ilGenerator.Emit(OpCodes.Stloc_S, local);
                    break;
            }
        }

        public void StoreToField(FieldInfo callerField)
        {
            _ilGenerator.Emit(OpCodes.Stfld, callerField);
        }

        public void Return()
        {
            _ilGenerator.Emit(OpCodes.Ret);
        }

        public void DoNothing()
        {
            _ilGenerator.Emit(OpCodes.Nop);
        }

        public void DeclareLocal(Type type)
        {
            _ilGenerator.DeclareLocal(type);
        }

        public void CreateObject(ConstructorInfo constructorInfo)
        {
            _ilGenerator.Emit(OpCodes.Newobj, constructorInfo);
        }

        public void LoadField(FieldInfo fieldInfo)
        {
            _ilGenerator.Emit(OpCodes.Ldfld, fieldInfo);
        }

        public void LoadFunction(MethodInfo methodInfo)
        {
            _ilGenerator.Emit(OpCodes.Ldftn, methodInfo);
        }

        public void CallVirtualMethod(MethodInfo methodInfo)
        {
            _ilGenerator.EmitCall(OpCodes.Callvirt, methodInfo, null);
        }

        public void LoadNull()
        {
            _ilGenerator.Emit(OpCodes.Ldnull);
        }

        public void LoadString(string value)
        {
            _ilGenerator.Emit(OpCodes.Ldstr, value);
        }

        public void LoadShort(short value)
        {
            _ilGenerator.Emit(OpCodes.Ldc_I4_S, value);
        }

        public void LoadInteger(int value)
        {
            _ilGenerator.Emit(OpCodes.Ldc_I4, value);
        }

        public void LoadLong(long value)
        {
            _ilGenerator.Emit(OpCodes.Ldc_I8, value);
        }

        public void LoadFloat(float value)
        {
            _ilGenerator.Emit(OpCodes.Ldc_R4, value);
        }

        public void LoadDouble(double value)
        {
            _ilGenerator.Emit(OpCodes.Ldc_R8, value);
        }

        public void Call(MethodInfo methodInfo)
        {
            _ilGenerator.Emit(OpCodes.Call, methodInfo);
        }

        public void LoadLocalAddress(byte i)
        {
            _ilGenerator.Emit(OpCodes.Ldloca_S, i);
        }

        public void InitObject(Type type)
        {
            _ilGenerator.Emit(OpCodes.Initobj, type);
        }

        public void CatchException(Type exceptionType)
        {
            _ilGenerator.BeginCatchBlock(exceptionType);
        }

        public void Finally()
        {
            _ilGenerator.BeginFinallyBlock();
        }

        public void EndExceptionBlock(Type exceptionType)
        {
            _ilGenerator.EndExceptionBlock();
        }
    }
}