using System;
using System.ServiceModel;
using NinjaFerret.Wcf.Exception.Faults;

namespace NinjaFerret.Wcf.Exception.Translators
{
    public class ArgumentExceptionTranslator : ExceptionTranslator<ArgumentFault, ArgumentException>
    {
        public override ArgumentException ConvertFaultToException(ArgumentFault detail, System.Exception innerException)
        {
            return new ArgumentException(detail.Message, detail.Parameter, innerException);
        }

        public override FaultException<ArgumentFault> ConvertExceptionToFault(ArgumentException exception)
        {
            var fault = new ArgumentFault {Message = exception.Message, Parameter = exception.ParamName};
            return new FaultException<ArgumentFault>(fault, new FaultReason(fault.Message));
        }
    }
}