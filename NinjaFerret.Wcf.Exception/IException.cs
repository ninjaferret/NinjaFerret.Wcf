using System.ServiceModel;

namespace NinjaFerret.Wcf.Exception
{
    public interface IException
    {
        FaultException ToFaultException();
    }
}