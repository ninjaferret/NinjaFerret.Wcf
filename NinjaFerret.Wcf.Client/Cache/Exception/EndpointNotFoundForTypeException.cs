using System;

namespace NinjaFerret.Wcf.Client.Cache.Exception
{
    public class EndpointNotFoundForTypeException : ApplicationException
    {
        public EndpointNotFoundForTypeException(string endpointName, string serviceType)
            : base(string.Format("The endpoint {0} could not be located for service {1}", endpointName, serviceType))
        {
        }
    }
}