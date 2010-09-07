using NinjaFerret.Wcf.Client.Cache;
using NinjaFerret.Wcf.Client.Cache.Exception;
using NinjaFerret.Wcf.Client.CallWrapper;
using NinjaFerret.Wcf.Client.Common;
using NinjaFerret.Wcf.Client.Common.Exception;
using NinjaFerret.Wcf.Client.Tests.TestInterface;
using NUnit.Framework;
using Rhino.Mocks;

namespace NinjaFerret.Wcf.Client.Tests.Cache
{
    [TestFixture]
    public class CallWrapperClassFixture
    {
        [Test]
        public void GetServiceCaller_Should_Throw_A_NotAWcfServiceExcpetion_If_Generic_Parameter_Is_Not_A_Wcf_Service_Contract()
        {
            var cache = new CallWrapperCache(new ServiceTypeValidator());

            try
            {
                cache.Get<InvalidTestInterface>("InvalidTestInterface");
                Assert.Fail("Should have errored");
            }
            catch (NotAWcfServiceExcpetion e)
            {
                Assert.That(e.ServiceType, Is.EqualTo(typeof(InvalidTestInterface)));
            }
        }

        [Test]
        public void GetServiceCaller_Should_Throw_A_NotAnInterfaceExcpetion_If_Generic_Parameter_Is_Not_A_Wcf_Service_Contract()
        {
            var cache = new CallWrapperCache(new ServiceTypeValidator());

            try
            {
                cache.Get<TestInterfaceClass>("TestInterfaceClass");
                Assert.Fail("Should have errored");
            }
            catch (NotAnInterfaceExcpetion e)
            {
                Assert.That(e.ServiceType, Is.EqualTo(typeof(TestInterfaceClass)));
            }
        }

        [Test]
        public void GetServiceCaller_Should_Add_A_Type_To_The_Cache_And_Retrieve_It()
        {
            var cache = new CallWrapperCache(new ServiceTypeValidator());

            var serviceCaller = MockRepository.GenerateStub<ICallWrapper<ITestInterface>>();
            serviceCaller.Expect(caller => caller.EndpointName).Return("ITestInterface");
            cache.Add(serviceCaller);

            Assert.That(cache.Get<ITestInterface>("ITestInterface"), Is.Not.Null);
            Assert.That(cache.Get<ITestInterface>("ITestInterface"), Is.SameAs(serviceCaller));
        }

        [Test]
        public void Should_Be_Able_To_Add_More_Than_One_Endpoint_For_A_ServiceType()
        {
            var cache = new CallWrapperCache(new ServiceTypeValidator());

            var serviceCaller = MockRepository.GenerateStub<ICallWrapper<ITestInterface>>();
            serviceCaller.Expect(caller => caller.EndpointName).Return("ITestInterface");
            var serviceCaller2 = MockRepository.GenerateStub<ICallWrapper<ITestInterface>>();
            serviceCaller2.Expect(caller => caller.EndpointName).Return("ILikeCake");

            cache.Add(serviceCaller);
            cache.Add(serviceCaller2);

            Assert.That(cache.Get<ITestInterface>("ITestInterface"), Is.Not.Null);
            Assert.That(cache.Get<ITestInterface>("ITestInterface"), Is.SameAs(serviceCaller));
            Assert.That(cache.Get<ITestInterface>("ILikeCake"), Is.Not.Null);
            Assert.That(cache.Get<ITestInterface>("ILikeCake"), Is.SameAs(serviceCaller2));
        }

        [Test]
        [ExpectedException(typeof(EndpointForServiceTypeAlreadyExistsException))]
        public void Should_Throw_An_Exception_When_Adding_Two_Callers_For_A_ServiceType_With_Same_EndpointName()
        {
            var cache = new CallWrapperCache(new ServiceTypeValidator());

            var serviceCaller = MockRepository.GenerateStub<ICallWrapper<ITestInterface>>();
            serviceCaller.Expect(caller => caller.EndpointName).Return("ITestInterface");
            var serviceCaller2 = MockRepository.GenerateStub<ICallWrapper<ITestInterface>>();
            serviceCaller2.Expect(caller => caller.EndpointName).Return("ITestInterface");

            cache.Add(serviceCaller);
            cache.Add(serviceCaller2);
        }

        [Test]
        [ExpectedException(typeof(EndpointNotFoundForTypeException))]
        public void Should_Throw_An_EndpointNotFoundForTypeException_If_No_ServiceCaller_For_That_Endpoint_Is_Found()
        {
            var cache = new CallWrapperCache(new ServiceTypeValidator());

            cache.Get<ITestInterface>("ITestInterface");
        }

        [Test]
        [ExpectedException(typeof(EndpointNotFoundForTypeException))]
        public void Should_Throw_An_EndpointNotFoundForTypeException_If_No_ServiceCaller_For_That_Endpoint_Is_Found_When_Other_Endpoints_Exist()
        {
            var cache = new CallWrapperCache(new ServiceTypeValidator());
            var serviceCaller = MockRepository.GenerateStub<ICallWrapper<ITestInterface>>();
            serviceCaller.Expect(caller => caller.EndpointName).Return("ILikeCake");

            cache.Add(serviceCaller);

            cache.Get<ITestInterface>("ITestInterface");
        }
    }
}