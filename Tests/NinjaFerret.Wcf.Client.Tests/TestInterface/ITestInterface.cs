/**
 * This file is part of the unit tests for WcfClientFactory
 * 
 *  WcfClientFactory is a tool to automatically generate a proxy client
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
using System.Runtime.Serialization;
using System.ServiceModel;

namespace NinjaFerret.Wcf.Client.Tests.TestInterface
{
    [ServiceContract]
    public interface ITestInterface
    {
        int ReturnInt();
        short ReturnShort();
        long ReturnLong();
        ushort ReturnUnsignedShort();
        uint ReturnUnsignedInt();
        ulong ReturnUnsignedLong();
        float ReturnFloat();
        double ReturnDouble();
        DateTime ReturnDateTime();
        string ReturnString();
        Guid ReturnGuid();
        TestEnumWithNoZero ReturnEnumWithNoZero();
        TestEnum ReturnEnum();
        TestReferenceType ReturnReferenceType();
        short ReturnShortParameter(short a);
        int ReturnIntParameter(int a);
        long ReturnLongParameter(long a);
        ushort ReturnUnsignedShortParameter(ushort a);
        uint ReturnUnsignedIntParameter(uint a);
        ulong ReturnUnsignedLongParameter(ulong a);
        float ReturnFloatParameter(float a);
        double ReturnDoubleParameter(double a);
        DateTime ReturnDateTimeParameter(DateTime a);
        string ReturnStringParameter(string a);
        Guid ReturnGuidParameter(Guid a);
        TestEnumWithNoZero ReturnEnumWithNoZeroParameter(TestEnumWithNoZero a);
        TestEnum ReturnEnumParameter(TestEnum a);
        long ReturnWithMultipleParameters(int a, long b, TestReferenceType c);
        TestReferenceType ReturnReferenceTypeParameter(TestReferenceType a);
        void ThrowError();
        string ReturnMultipleParameters(int a, int b, int c);
        string ReturnMultipleReferenceParams(TestReferenceType a, TestReferenceType b);
    }

    [DataContract]
    public class TestReferenceType
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public enum TestEnumWithNoZero
    {
        [EnumMember]
        First = 1,
        [EnumMember]
        Second = 2,
        [EnumMember]
        Third = 3
    }

    [DataContract]
    public enum TestEnum
    {
        [EnumMember]
        Zeroth = 0,
        [EnumMember]
        First = 1,
        [EnumMember]
        Second = 2,
        [EnumMember]
        Third = 3
    }
}