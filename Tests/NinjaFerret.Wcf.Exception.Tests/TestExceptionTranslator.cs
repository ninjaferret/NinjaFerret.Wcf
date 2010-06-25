using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using NUnit.Framework;

namespace NinjaFerret.Wcf.Exception.Tests
{
    [TestFixture]
    public class TestExceptionTranslator
    {
        [Test]
        public void When_Passed_A_Non_Translatable_Exception_ToFaultException_Returns_A_Generic_Exception()
        {
            var exception = new System.Exception("This is an exception");
            var translator = new ExceptionTranslator();

            var result = translator.ToFaultException(exception);

            Assert.That(result, Is.TypeOf(typeof(FaultException)));
            Assert.That(result.GetType().IsGenericType, Is.False);
            Assert.That(result.Message, Is.EqualTo("A fault occurred calling this service."));
        }

        [Test]
        public void When_Passed_A_Translatable_Exception_ToFaultException_Returns_A_Typed_Exception()
        {
            var exception = new TestException("This is an exception");
            var translator = new ExceptionTranslator();

            var result = translator.ToFaultException(exception);

            Assert.That(result, Is.TypeOf(typeof(FaultException<TestFault>)));
        }

        [Test]
        public void When_Passed_A_Non_Typed_Fault_Exception_ToException_Returns_A_Generic_Exception()
        {
            var faultException = new FaultException("Test fault exception");
            var translator = new ExceptionTranslator();

            var result = translator.ToException(faultException);

            Assert.That(result, Is.TypeOf(typeof(System.Exception)));
        }

        [Test]
        public void When_Passed_A_Non_Translatable_Fault_ToException_Returns_A_Generic_Exception()
        {
            var faultException = new FaultException<NonTranslatableFault>(new NonTranslatableFault(), "Test fault exception");
            var translator = new ExceptionTranslator();

            var result = translator.ToException(faultException);

            Assert.That(result, Is.TypeOf(typeof(System.Exception)));
        }

        [Test]
        public void When_Passed_A_Translatable_Fault_ToException_Returns_A_Generic_Exception()
        {
            var faultException = new FaultException<TestFault>(new TestFault(), "Test fault exception");
            var translator = new ExceptionTranslator();

            var result = translator.ToException(faultException);

            Assert.That(result, Is.TypeOf(typeof(TestException)));
        }

    }

    class NonTranslatableFault
    {
        
    }

    class TestFault : TranslatableFault
    {
        public override System.Exception ToException()
        {
            return new TestException();
        }
    }

    [Serializable]
    public class TestException : System.Exception, ITranslatableException
    {

        public TestException()
        {
        }

        public TestException(string message) : base(message)
        {
        }

        public TestException(string message, System.Exception inner) : base(message, inner)
        {
        }

        protected TestException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public FaultException ToFaultException()
        {
            return new FaultException<TestFault>(new TestFault(), "An error occurred");
        }
    }
}