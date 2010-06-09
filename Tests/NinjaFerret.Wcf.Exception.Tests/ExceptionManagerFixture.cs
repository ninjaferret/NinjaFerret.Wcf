using System.ServiceModel;
using NinjaFerret.Wcf.Exception.Exceptions;
using NinjaFerret.Wcf.Exception.Faults;
using NUnit.Framework;

namespace NinjaFerret.Wcf.Exception.Tests
{
    [TestFixture]
    public class ExceptionManagerFixture
    {
        private ExceptionManager _exceptionManager;

        [SetUp]
        public void SetUp()
        {
            _exceptionManager = new ExceptionManager();
        }

        [Test]
        public void Test_That_ProcessException_Returns_Basic_Exception_When_No_Handlers_Registered()
        {
            var faultException = new FaultException<Fault>(new TestFault(), new FaultReason("This is a fault"),
                                                               new FaultCode("FaultCode"));

            var result = _exceptionManager.ProcessFault(faultException);

            Assert.That(result, Is.TypeOf(typeof(TranslatorNotFoundException)));
        }

        [Test]
        public void Test_That_When_Handler_Is_Registered_The_Correct_Exception_Is_Thrown()
        {
            var faultException = new FaultException<Fault>(new TestFault(), new FaultReason("This is a fault"),
                                                               new FaultCode("FaultCode"));
            _exceptionManager.RegisterExceptionTranslator(new TestFaultExceptionTranslator());
            var result = _exceptionManager.ProcessFault(faultException);

            Assert.That(result, Is.InstanceOf(typeof(TestFaultException)));
        }

        [Test]
        public void Test_That_When_Handler_Is_Removed_The_Generic_Exception_Is_Thrown()
        {
            var faultException = new FaultException<Fault>(new TestFault(), new FaultReason("This is a fault"),
                                                               new FaultCode("FaultCode"));
            var testFaultExceptionHandler = new TestFaultExceptionTranslator();
            _exceptionManager.RegisterExceptionTranslator(testFaultExceptionHandler);
            _exceptionManager.RemoveExceptionTranslator(testFaultExceptionHandler);

            var result = _exceptionManager.ProcessFault(faultException);

            Assert.That(result, Is.InstanceOf(typeof(System.Exception)));
        }

        [Test]
        public void Test_That_When_Processing_Exception_With_No_Handler_Generic_Failure_Is_Thrown()
        {
            var exception = new TestFaultException();

            var result = _exceptionManager.ProcessException(exception);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(FaultException<TranslationFailedFault>)));
        }

        [Test]
        public void Test_That_When_Processing_Exception_With_Handler_Present_Correct_Handler_Is_Called()
        {
            var exception = new TestFaultException();
            _exceptionManager.RegisterExceptionTranslator(new TestFaultExceptionTranslator());
            var result = _exceptionManager.ProcessException(exception);

            Assert.That(result, Is.TypeOf(typeof(FaultException<TestFault>)));
        }
    }

    public class TestFault : Fault
    {
        
    }

    public class TestFaultException : System.Exception
    {
        public TestFaultException(){}
        public TestFaultException(System.Exception innerException) : base("Test Fault Exception", innerException)
        {
            
        }
    }

    public class TestFaultExceptionTranslator : ExceptionTranslator<TestFault, TestFaultException>
    {

        public override TestFaultException ConvertFaultToException(TestFault fault, System.Exception innerException)
        {
            return new TestFaultException(innerException);
        }

        public override FaultException<TestFault> ConvertExceptionToFault(TestFaultException exception)
        {
            return new FaultException<TestFault>(new TestFault());
        }
    }
}