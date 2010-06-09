using System;

namespace NinjaFerret.Wcf.Exception.Exceptions
{
    public class TranslatorNotFoundException : System.Exception
    {
        public TranslatorNotFoundException(string message, Type faultType, System.Exception innerException) 
            : base (string.Format("Could not locate translator for FaultException<{0}>: {1}", faultType, message), innerException)
        {}
    }
}