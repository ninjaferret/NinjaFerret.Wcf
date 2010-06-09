using System;

namespace NinjaFerret.Wcf.Client.Cache.Exception
{
    public class CallWrapperIsWrongTypeException : ApplicationException
    {
        public CallWrapperIsWrongTypeException(Type expectedType, Type actualType)
            : base (string.Format("Expected to return type {0} but got {1}", expectedType.FullName, actualType.FullName))
        {
        }
    }
}