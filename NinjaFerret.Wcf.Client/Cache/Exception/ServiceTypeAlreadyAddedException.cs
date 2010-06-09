using System;

namespace NinjaFerret.Wcf.Client.Cache.Exception
{
    public class ServiceTypeAlreadyAddedException : ApplicationException
    {
        public ServiceTypeAlreadyAddedException(Type serviceType)
            : base (string.Format("The service interface type {0} has already been defined", serviceType.FullName))
        {
        }
    }
}