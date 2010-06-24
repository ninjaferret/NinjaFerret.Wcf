using System.Runtime.Serialization;

namespace NinjaFerret.Wcf.Exception
{
    [DataContract]
    public abstract class Fault
    {
        public abstract System.Exception ToException();
    }
}