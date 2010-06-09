using System;
using System.ServiceModel;

namespace NinjaFerret.Wcf.Exception
{
    public abstract class ExceptionTranslator<TFault, TException> : IExceptionTranslator 
        where TFault : Fault
        where TException : System.Exception
    {
        public System.Exception ConvertToException(Fault fault, FaultException innerException)
        {
            var typedFault = (TFault) fault;
            if (fault == null)
                throw new ArgumentNullException("fault", "Cannot convert a null fault");
            return ConvertFaultToException(typedFault, innerException);
        }

        public abstract TException ConvertFaultToException(TFault detail, System.Exception innerException);

        public FaultException ConvertToFaultException(System.Exception exception)
        {
            var typedException = (TException) exception;
            if (exception == null)
                throw new ArgumentNullException("exception", "Cannot convert a null exception");
            return ConvertExceptionToFault(typedException);
        }

        public abstract FaultException<TFault> ConvertExceptionToFault(TException exception);

        public Type FaultType
        {
            get { return typeof(TFault); }
        }

        public Type ExceptionType
        {
            get { return typeof(TException); }
        }
    }
}