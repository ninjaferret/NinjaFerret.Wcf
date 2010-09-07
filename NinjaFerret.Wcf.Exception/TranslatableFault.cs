using System.Runtime.Serialization;

namespace NinjaFerret.Wcf.Exception
{
    [DataContract]
    public abstract class TranslatableFault
    {
        public abstract System.Exception ToException();
    }
}