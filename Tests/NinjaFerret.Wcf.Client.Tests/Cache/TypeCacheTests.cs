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
using NinjaFerret.Wcf.Client.Cache;
using NinjaFerret.Wcf.Client.Cache.Exception;
using NinjaFerret.Wcf.Client.Common;
using NinjaFerret.Wcf.Client.Common.Exception;
using NinjaFerret.Wcf.Client.Tests.TestInterface;
using NUnit.Framework;

namespace NinjaFerret.Wcf.Client.Tests.Cache
{
    [TestFixture]
    public class TypeCacheTests
    {
        [Test]
        public void Should_Add_A_Type_To_The_Cache_And_Retrieve_It()
        {
            var implementation = typeof(TestInterfaceClass);
            var cache = new TypeCache(new ServiceTypeValidator());

            cache.AddType<ITestInterface>(implementation);
            
            Assert.That(cache.Count, Is.EqualTo(1));
            Assert.That(cache.GetType<ITestInterface>(), Is.Not.Null);
            Assert.That(cache.GetType<ITestInterface>(), Is.SameAs(implementation));
        }

        [Test]
        public void Add_Should_Throw_A_ServiceTypeMismatchException_If_Type_Does_Not_Implement_Generic_Parameter()
        {
            var clientType = typeof(InvalidClass<ITestInterface>);

            var cache = new TypeCache(new ServiceTypeValidator());

            try
            {
                cache.AddType<ITestInterface>(clientType);
                Assert.Fail("Should have errored");
            }
            catch (ServiceTypeMismatchException e)
            {
                Assert.That(e.ImplementationType, Is.EqualTo(typeof(InvalidClass<ITestInterface>)));
                Assert.That(e.ServiceType, Is.EqualTo(typeof(ITestInterface)));
            }
        }

        [Test]
        public void Add_Should_Throw_A_NotAWcfServiceExcpetion_If_Generic_Parameter_Is_Not_A_Wcf_Service_Contract()
        {
            var clientType = typeof(TestInterfaceClass);

            var cache = new TypeCache(new ServiceTypeValidator());

            try
            {
                cache.AddType<InvalidTestInterface>(clientType);
                Assert.Fail("Should have errored");
            }
            catch (NotAWcfServiceExcpetion e)
            {
                Assert.That(e.ServiceType, Is.EqualTo(typeof(InvalidTestInterface)));
            }
        }

        [Test]
        public void Add_Should_Throw_A_NotAnInterfaceExcpetion_If_Generic_Parameter_Is_Not_A_Wcf_Service_Contract()
        {
            var clientType = typeof(TestInterfaceClass);

            var cache = new TypeCache(new ServiceTypeValidator());

            try
            {
                cache.AddType<TestInterfaceClass>(clientType);
                Assert.Fail("Should have errored");
            }
            catch (NotAnInterfaceExcpetion e)
            {
                Assert.That(e.ServiceType, Is.EqualTo(typeof(TestInterfaceClass)));
            }
        }

        [Test]
        [ExpectedException(typeof(ServiceTypeAlreadyAddedException))]
        public void AddClientType_Should_Throw_A_ServiceTypeAlreadyAdded_If_Trying_To_Register_Two_Implementations_Of_Same_Interface()
        {
            var implementationType1 = typeof (TestInterfaceClass);
            var implementationType2 = typeof (TestInterfaceClass);
            var cache = new TypeCache(new ServiceTypeValidator());
            cache.AddType<ITestInterface>(implementationType1);
            cache.AddType<ITestInterface>(implementationType2);
        }

        [Test]
        public void Should_Return_False_If_Cache_Does_Not_Contain_Something_For_That_Type()
        {
            var typeCache = new TypeCache(new ServiceTypeValidator());

            var result = typeCache.Contains<ITestInterface>();

            Assert.That(result, Is.False);
        }

        [Test]
        public void Should_Return_True_If_Cache_Contains_Something_For_That_Type()
        {
            var implementation = typeof(TestInterfaceClass);
            var cache = new TypeCache(new ServiceTypeValidator());

            cache.AddType<ITestInterface>(implementation);
            var result = cache.Contains<ITestInterface>();

            Assert.That(result, Is.True);
        }


        [Test]
        public void GetServiceType_Should_Throw_A_NotAWcfServiceExcpetion_If_Generic_Parameter_Is_Not_A_Wcf_Service_Contract()
        {
            var cache = new TypeCache(new ServiceTypeValidator());

            try
            {
                cache.GetType<InvalidTestInterface>();
                Assert.Fail("Should have errored");
            }
            catch (NotAWcfServiceExcpetion e)
            {
                Assert.That(e.ServiceType, Is.EqualTo(typeof(InvalidTestInterface)));
            }
        }

        [Test]
        public void GetServiceType_Should_Throw_A_NotAnInterfaceExcpetion_If_Generic_Parameter_Is_Not_A_Wcf_Service_Contract()
        {
            var cache = new TypeCache(new ServiceTypeValidator());

            try
            {
                cache.GetType<TestInterfaceClass>();
                Assert.Fail("Should have errored");
            }
            catch (NotAnInterfaceExcpetion e)
            {
                Assert.That(e.ServiceType, Is.EqualTo(typeof(TestInterfaceClass)));
            }
        }

        [Test]
        public void Contains_Should_Throw_A_NotAWcfServiceExcpetion_If_Generic_Parameter_Is_Not_A_Wcf_Service_Contract()
        {
            var cache = new TypeCache(new ServiceTypeValidator());

            try
            {
                cache.Contains<InvalidTestInterface>();
                Assert.Fail("Should have errored");
            }
            catch (NotAWcfServiceExcpetion e)
            {
                Assert.That(e.ServiceType, Is.EqualTo(typeof(InvalidTestInterface)));
            }
        }

        [Test]
        public void Contains_Should_Throw_A_NotAnInterfaceExcpetion_If_Generic_Parameter_Is_Not_A_Wcf_Service_Contract()
        {
            var cache = new TypeCache(new ServiceTypeValidator());

            try
            {
                cache.Contains<TestInterfaceClass>();
                Assert.Fail("Should have errored");
            }
            catch (NotAnInterfaceExcpetion e)
            {
                Assert.That(e.ServiceType, Is.EqualTo(typeof(TestInterfaceClass)));
            }
        }

        

        
    }
}