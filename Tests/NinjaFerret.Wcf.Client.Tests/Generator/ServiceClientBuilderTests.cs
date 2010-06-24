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
using NinjaFerret.Wcf.Client.CallWrapper;
using NinjaFerret.Wcf.Client.Common.Exception;
using NinjaFerret.Wcf.Client.Generator;
using NinjaFerret.Wcf.Client.Tests.TestInterface;
using NUnit.Framework;
using Rhino.Mocks;

namespace NinjaFerret.Wcf.Client.Tests.Generator
{
    [TestFixture]
    public class ClientGeneratorTests
    {
        private ITestInterface testImplementation;
        private ClientGenerator clientBuilder;
        private ICallWrapper<ITestInterface> callManager;
        private Type resultType;

        [TestFixtureSetUp]
        public void SetUp()
        {
            clientBuilder = new ClientGenerator();
            callManager = MockRepository.GenerateMock<ICallWrapper<ITestInterface>>();
            callManager.Expect(x => x.Call(null)).IgnoreArguments().Do(
                new TestCall(DoCall)).Repeat.Any();
            testImplementation = new TestInterfaceClass(callManager);
        }

        [Test]
        public void Should_Return_A_Type_For_The_Given_Interface()
        {
            var type = clientBuilder.CreateType<ITestInterface>();
            Assert.That(type.GetInterfaces().Length, Is.EqualTo(1));
            Assert.That(type.GetInterfaces()[0], Is.EqualTo(typeof(ITestInterface)));
        }

        [Test]
        public void Should_Return_A_Type_That_Can_Be_Instantiated()
        {
            var type = clientBuilder.CreateType<ITestInterface>();
            var createdObject =  Activator.CreateInstance(type, new object[] { callManager });
            Assert.That(createdObject, Is.AssignableTo(typeof(ITestInterface)));
        }

        [Test]
        public void Should_Return_A_Short_Correctly()
        {
            var callerInterface = GetCreatedObject();

            var sValue = callerInterface.ReturnShort();

            Assert.That(sValue, Is.EqualTo(111));
        }

        [Test]
        public void Should_Return_An_Int_Correctly()
        {
            var callerInterface = GetCreatedObject();

            var iValue = callerInterface.ReturnInt();

            Assert.That(iValue, Is.EqualTo(222));
        }

        [Test]
        public void Should_Return_A_Long_Correctly()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnLong();

            Assert.That(value, Is.EqualTo(333L));
        }

        [Test]
        public void Should_Return_A_UnsignedShort_Correctly()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnUnsignedShort();

            Assert.That(value, Is.EqualTo(4444));
        }

        [Test]
        public void Should_Return_A_UnsignedInt_Correctly()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnUnsignedInt();

            Assert.That(value, Is.EqualTo(55555U));
        }

        [Test]
        public void Should_Return_A_UnsignedLong_Correctly()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnUnsignedLong();

            Assert.That(value, Is.EqualTo(666666UL));
        }

        [Test]
        public void Should_Return_A_Float_Correctly()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnFloat();

            Assert.That(value, Is.EqualTo(1.1f));
        }

        [Test]
        public void Should_Return_A_Double_Correctly()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnDouble();

            Assert.That(value, Is.EqualTo(2.2));
        }

        [Test]
        public void Should_Return_Short_Parameter_Given()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnShortParameter(1);

            Assert.That(value, Is.EqualTo(1));
        }

        [Test]
        public void Should_Return_Int_Parameter_Given()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnIntParameter(12345);

            Assert.That(value, Is.EqualTo(12345));
        }

        [Test]
        public void Should_Return_Long_Parameter_Given()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnLongParameter(123456L);

            Assert.That(value, Is.EqualTo(123456L));
        }

        [Test]
        public void Should_Return_UShort_Parameter_Given()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnUnsignedShortParameter(1234);

            Assert.That(value, Is.EqualTo(1234U));
        }

        [Test]
        public void Should_Return_UInt_Parameter_Given()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnUnsignedIntParameter(12345U);

            Assert.That(value, Is.EqualTo(12345U));
        }

        [Test]
        public void Should_Return_Ulong_Parameter_Given()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnUnsignedLongParameter(12345UL);

            Assert.That(value, Is.EqualTo(12345UL));
        }

        [Test]
        public void Should_Return_Float_Parameter_Given()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnFloatParameter(12345.123f);

            Assert.That(value, Is.EqualTo(12345.123f));
        }

        [Test]
        public void Should_Return_Double_Parameter_Given()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnDoubleParameter(12345.2345);

            Assert.That(value, Is.EqualTo(12345.2345));
        }

        [Test]
        public void Should_Return_DateTime_Parameter_Given()
        {
            var callerInterface = GetCreatedObject();
            var now = DateTime.Now;

            var value = callerInterface.ReturnDateTimeParameter(now);

            Assert.That(value, Is.EqualTo(now));
        }

        [Test]
        public void Should_Return_String_Parameter_Given()
        {
            var callerInterface = GetCreatedObject();

            var value = callerInterface.ReturnStringParameter("12345");

            Assert.That(value, Is.EqualTo("12345"));
        }

        [Test]
        public void Should_Return_Guid_Parameter_Given()
        {
            var callerInterface = GetCreatedObject();
            var guid = Guid.NewGuid();

            var value = callerInterface.ReturnGuidParameter(guid);

            Assert.That(value, Is.EqualTo(guid));
        }

        [Test]
        public void Should_Return_ReferenceType_Parameter_Given()
        {
            var callerInterface = GetCreatedObject();
            var param = new TestReferenceType {Id = 3, Name = "Test"};

            var value = callerInterface.ReturnReferenceTypeParameter(param);

            Assert.That(value.Id, Is.EqualTo(3));
            Assert.That(value.Name, Is.EqualTo("Test"));
        }

        [Test]
        public void Should_Handle_Multiple_Reference_Parameters()
        {
            var callerInterface = GetCreatedObject();
            var param1 = new TestReferenceType { Id = 1, Name = "Test" };
            var param2 = new TestReferenceType { Id = 3, Name = "Test" };
            var result = callerInterface.ReturnMultipleReferenceParams(param1, param2);

            Assert.That(result, Is.EqualTo("1 3"));
        }

        [Test]
        public void Should_Handle_Multiple_Parameters_Value_Types_Just_Ints()
        {
            var callerInterface = GetCreatedObject();

            var result = callerInterface.ReturnMultipleParameters(1, 2, 3);

            Assert.That(result, Is.EqualTo("1 2 3"));
        }

        [Test]
        public void Should_Handle_Multiple_Parameters_Mixed_Value_And_Reference_Types()
        {
            var callerInterface = GetCreatedObject();
            var refType = new TestReferenceType { Id = 3 };
            const int firstParameter = 1;
            const long secondParameter = 2L;
            var value = callerInterface.ReturnWithMultipleParameters(firstParameter, secondParameter, refType);

            Assert.That(value, Is.EqualTo(6));
        }

        [Test]
        public void Should_Handle_Enum_With_No_Zero_Value_Correctly()
        {
            var callerInterface = GetCreatedObject();
            var result = callerInterface.ReturnEnumWithNoZero();
            Assert.That(result, Is.EqualTo(TestEnumWithNoZero.Second));
        }

        [Test]
        public void Should_Handle_Enum_With_Zero_Value_Correctly()
        {
            var callerInterface = GetCreatedObject();
            var result = callerInterface.ReturnEnum();
            Assert.That(result, Is.EqualTo(TestEnum.Third));
        }

        [Test]
        public void Should_Handle_Enum_With_No_Zero_Value_As_Parameter_Correctly()
        {
            var callerInterface = GetCreatedObject();
            var result = callerInterface.ReturnEnumWithNoZeroParameter(TestEnumWithNoZero.Third);
            Assert.That(result, Is.EqualTo(TestEnumWithNoZero.Third));
        }

        [Test]
        public void Should_Handle_Enum_With_Zero_Value_As_Parameter_Correctly()
        {
            var callerInterface = GetCreatedObject();
            var result = callerInterface.ReturnEnumParameter(TestEnum.First);
            Assert.That(result, Is.EqualTo(TestEnum.First));
        }

        [Test]
        [ExpectedException(typeof(ServiceTypeNotAnInterfaceException))]
        public void Should_Throw_An_Exception_If_Type_Is_Not_An_Interface()
        {
            clientBuilder.CreateType<TestInterfaceClass>();
        }

        

        [Test]
        [ExpectedException(typeof(ServiceTypeIsNotMarkedWithServiceContractAttributeException))]
        public void Should_Throw_An_Exception_If_Type_Is_Not_Marked_As_A_ServiceContract()
        {
            clientBuilder.CreateType<InvalidTestInterface>();
        }

        private delegate void TestCall(MakeCallToTheWcfServiceDelegate<ITestInterface> codeBlock);

        private void DoCall(MakeCallToTheWcfServiceDelegate<ITestInterface> codeBlock)
        {
            codeBlock(testImplementation);
        }

        private ITestInterface GetCreatedObject()
        {
            if (resultType == null)
            {
                resultType = clientBuilder.CreateType<ITestInterface>();
            }
            return (ITestInterface)Activator.CreateInstance(resultType, new object[] { callManager });
        }

        [Test]
        [Ignore]
        public void GenerateAssembly()
        {
            var builder = new ClientGenerator();
            builder.CreateType<ITestInterface>();
            builder.GenerateAssembly();
        }

    }
}