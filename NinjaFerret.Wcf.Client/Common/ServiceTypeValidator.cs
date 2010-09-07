/**
 * This file is part of NinjaFerret.Wcf.Client
 * 
 *  NinjaFerret.Wcf.Client is a framework that automatically generates a proxy client
 *  for a specified WCF Service interface.
 *  Copyright (C) 2010  Ian Johnson
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *  
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *  
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
**/
using System;
using System.Linq;
using System.ServiceModel;
using NinjaFerret.Wcf.Client.Common.Exception;

namespace NinjaFerret.Wcf.Client.Common
{
    public class ServiceTypeValidator : IServiceTypeValidator
    {
        public void ValidateServiceType(Type serviceType)
        {
            CheckThatTheServiceTypeIsAnInterface(serviceType);
            CheckThatTheServiceTypeIsAWcfServiceContract(serviceType);
            
        }

        public void ValidateImplementationType(Type serviceType, Type implementationType)
        {
            ValidateServiceType(serviceType);
            CheckIfImplementationImplementsTheInterface(serviceType, implementationType);
        }

        private static void CheckThatTheServiceTypeIsAnInterface(Type serviceType)
        {
            if (!serviceType.IsInterface)
            {
                throw new NotAnInterfaceExcpetion(serviceType);
            }
        }

        private static void CheckThatTheServiceTypeIsAWcfServiceContract(Type serviceType)
        {
            var attributes = serviceType.GetCustomAttributes(typeof(ServiceContractAttribute), true);
            if (attributes.Length == 0)
            {
                throw new NotAWcfServiceExcpetion(serviceType);
            }
        }

        private static void CheckIfImplementationImplementsTheInterface(Type serviceType, Type implementationType)
        {
            var interfaces = implementationType.GetInterfaces();
            if (interfaces == null || interfaces.Length == 0 || interfaces.Count(i => i.Equals(serviceType)) == 0)
            {
                throw new ServiceTypeMismatchException(serviceType, implementationType);
            }
        }
    }
}