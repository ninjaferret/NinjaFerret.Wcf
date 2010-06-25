using System.ServiceModel;

namespace NinjaFerret.Wcf.Exception
{
    public interface IExceptionTranslator
    {
        FaultException ToFaultException(System.Exception exception);
        System.Exception ToException(FaultException faultException);
    }
}