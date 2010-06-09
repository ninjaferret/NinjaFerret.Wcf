using System.ServiceModel;

namespace NinjaFerret.Wcf.Exception
{
    public interface IExceptionManager
    {
        void RegisterExceptionTranslator(IExceptionTranslator translator);

        void RemoveExceptionTranslator(IExceptionTranslator translator);

        System.Exception ProcessFault<TFault>(FaultException<TFault> faultException) where TFault : Fault;

        FaultException ProcessException(System.Exception exception);
    }
}