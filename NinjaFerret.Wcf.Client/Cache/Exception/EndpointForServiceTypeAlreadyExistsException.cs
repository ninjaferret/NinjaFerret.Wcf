using System;

namespace NinjaFerret.Wcf.Client.Cache.Exception
{
    public class EndpointForServiceTypeAlreadyExistsException : ApplicationException
    {
        public EndpointForServiceTypeAlreadyExistsException(Type serviceType, string endpointName)
            : base (string.Format("A service caller for endpoint {0} already exists for {1}", endpointName, serviceType))
        {
        }
    }
}