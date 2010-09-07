/**
 * This file is part of the unit tests for NinjaFerret.Wcf.Client
 * 
 *  NinjaFerret.Wcf.Client is a framework that automatically generates a proxy client
 *  for a specified WCF Service interface.
 *  Copyright (C) 2010  Ian Johnson
 *  
 *  NUnit:
 *  Copyright © 2002-2008 Charlie Poole
 *  Copyright © 2002-2004 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
 *  Copyright © 2000-2002 Philip A. Craig
 *  
 *  Rhino.Mocks:
 *  Copyright (c) 2005 - 2009 Ayende Rahien (ayende@ayende.com)
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
using NinjaFerret.Wcf.Client.CallWrapper;

namespace NinjaFerret.Wcf.Client.Tests.TestInterface
{
    public class TestInterfaceClass : ITestInterface, InvalidTestInterface
    {
        public TestInterfaceClass(ICallWrapper<ITestInterface> callWrapper)
        {
            
        }

        public TestInterfaceClass(ICallWrapper<InvalidTestInterface> callWrapper)
        {

        }

        public int ReturnInt()
        {
            return 222;
        }

        public short ReturnShort()
        {
            return 111;
        }

        public long ReturnLong()
        {
            return 333L;
        }

        public ushort ReturnUnsignedShort()
        {
            return 4444;
        }

        public uint ReturnUnsignedInt()
        {
            return 55555;
        }

        public ulong ReturnUnsignedLong()
        {
            return 666666;
        }

        public float ReturnFloat()
        {
            return 1.1f;
        }

        public double ReturnDouble()
        {
            return 2.2;
        }

        public DateTime ReturnDateTime()
        {
            return DateTime.Now;
        }

        public string ReturnString()
        {
            return "TestString";
        }

        public Guid ReturnGuid()
        {
            return Guid.NewGuid();
        }

        public TestEnumWithNoZero ReturnEnumWithNoZero()
        {
            return TestEnumWithNoZero.Second;
        }

        public TestEnum ReturnEnum()
        {
            return TestEnum.Third;
        }

        public Guid ReturnGuidParameter(Guid a)
        {
            return a;
        }

        public TestEnumWithNoZero ReturnEnumWithNoZeroParameter(TestEnumWithNoZero a)
        {
            return a;
        }

        public TestEnum ReturnEnumParameter(TestEnum a)
        {
            return a;
        }

        public long ReturnWithMultipleParameters(int a, long b, TestReferenceType c)
        {
            var result = a + b + c.Id;
            return result;
        }

        public TestReferenceType ReturnReferenceTypeParameter(TestReferenceType a)
        {
            return a;
        }

        public TestReferenceType ReturnReferenceType()
        {
            return new TestReferenceType{Id = 1, Name = "ReferenceTypeName"};
        }

        public short ReturnShortParameter(short a)
        {
            return a;
        }

        public int ReturnIntParameter(int a)
        {
            return a;
        }

        public long ReturnLongParameter(long a)
        {
            return a;
        }

        public ushort ReturnUnsignedShortParameter(ushort a)
        {
            return a;
        }

        public uint ReturnUnsignedIntParameter(uint a)
        {
            return a;
        }

        public ulong ReturnUnsignedLongParameter(ulong a)
        {
            return a;
        }

        public float ReturnFloatParameter(float a)
        {
            return a;
        }

        public double ReturnDoubleParameter(double a)
        {
            return a;
        }

        public DateTime ReturnDateTimeParameter(DateTime a)
        {
            return a;
        }

        public string ReturnStringParameter(string a)
        {
            return a;
        }

        public void ThrowError()
        {
            throw new System.Exception("ITestInterface");
        }

        public string ReturnMultipleParameters(int a, int b, int c)
        {
            return string.Format("{0} {1} {2}", a, b, c);
        }

        public string ReturnMultipleReferenceParams(TestReferenceType a, TestReferenceType b)
        {
            return string.Format("{0} {1}", a.Id, b.Id);
        }
    }
}