using System.ServiceModel;

namespace NinjaFerret.Wcf.Exception
{
    public interface ITranslatableException
    {
        FaultException ToFaultException();
    }
}