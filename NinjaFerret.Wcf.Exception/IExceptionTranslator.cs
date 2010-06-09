using System;
using System.ServiceModel;

namespace NinjaFerret.Wcf.Exception
{

    public interface IExceptionTranslator
    {
        System.Exception ConvertToException(Fault fault, FaultException innerException);
        FaultException ConvertToFaultException(System.Exception exception);
        Type FaultType { get; }
        Type ExceptionType { get; }
    }
}
