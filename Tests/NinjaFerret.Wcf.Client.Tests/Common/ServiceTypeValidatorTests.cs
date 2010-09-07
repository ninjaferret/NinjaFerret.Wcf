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
using NinjaFerret.Wcf.Client.Common;
using NinjaFerret.Wcf.Client.Common.Exception;
using NinjaFerret.Wcf.Client.Tests.TestInterface;
using NUnit.Framework;

namespace NinjaFerret.Wcf.Client.Tests.Common
{
    [TestFixture]
    public class ServiceTypeValidatorTests
    {
        [Test]
        public void ValidateImplementationType_Should_Throw_A_ServiceTypeMismatchException_If_Type_Does_Not_Implement_Generic_Parameter()
        {
            var clientType = typeof(ServiceTypeValidatorTests);

            var validator = new ServiceTypeValidator();

            try
            {
                validator.ValidateImplementationType(typeof(ITestInterface), clientType);
                Assert.Fail("Should have errored");
            }
            catch (ServiceTypeMismatchException e)
            {
                Assert.That(e.ImplementationType, Is.EqualTo(typeof(ServiceTypeValidatorTests)));
                Assert.That(e.ServiceType, Is.EqualTo(typeof(ITestInterface)));
            }
        }

        [Test]
        public void ValidateImplementationType_Should_Throw_A_NotAWcfServiceExcpetion_If_Generic_Parameter_Is_Not_A_Wcf_Service_Contract()
        {
            var clientType = typeof(TestInterfaceClass);

            var validator = new ServiceTypeValidator();

            try
            {
                validator.ValidateImplementationType(typeof(InvalidTestInterface), clientType);
                Assert.Fail("Should have errored");
            }
            catch (NotAWcfServiceExcpetion e)
            {
                Assert.That(e.ServiceType, Is.EqualTo(typeof(InvalidTestInterface)));
            }
        }

        [Test]
        public void ValidateImplementationType_Should_Throw_A_NotAnInterfaceExcpetion_If_Generic_Parameter_Is_Not_A_Wcf_Service_Contract()
        {
            var clientType = typeof(TestInterfaceClass);

            var validator = new ServiceTypeValidator();

            try
            {
                validator.ValidateImplementationType(typeof(TestInterfaceClass), clientType);
                Assert.Fail("Should have errored");
            }
            catch (NotAnInterfaceExcpetion e)
            {
                Assert.That(e.ServiceType, Is.EqualTo(typeof(TestInterfaceClass)));
            }
        }

        [Test]
        public void ValidateServiceType_Should_Throw_A_NotAWcfServiceExcpetion_If_Generic_Parameter_Is_Not_A_Wcf_Service_Contract()
        {
            var validator = new ServiceTypeValidator();

            try
            {
                validator.ValidateServiceType(typeof(InvalidTestInterface));
                Assert.Fail("Should have errored");
            }
            catch (NotAWcfServiceExcpetion e)
            {
                Assert.That(e.ServiceType, Is.EqualTo(typeof(InvalidTestInterface)));
            }
        }

        [Test]
        public void ValidateServiceType_Should_Throw_A_NotAnInterfaceExcpetion_If_Generic_Parameter_Is_Not_A_Wcf_Service_Contract()
        {
            var validator = new ServiceTypeValidator();

            try
            {
                validator.ValidateServiceType(typeof(TestInterfaceClass));
                Assert.Fail("Should have errored");
            }
            catch (NotAnInterfaceExcpetion e)
            {
                Assert.That(e.ServiceType, Is.EqualTo(typeof(TestInterfaceClass)));
            }
        }
    }
}