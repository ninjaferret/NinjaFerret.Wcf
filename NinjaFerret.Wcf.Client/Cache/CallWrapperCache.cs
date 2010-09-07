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
using System.Collections.Generic;
using NinjaFerret.Wcf.Client.Cache.Exception;
using NinjaFerret.Wcf.Client.CallWrapper;
using NinjaFerret.Wcf.Client.Common;

namespace NinjaFerret.Wcf.Client.Cache
{
    public class CallWrapperCache : ICallWrapperCache
    {
        private readonly IServiceTypeValidator serviceTypeValidator;
        private readonly Dictionary<Type, Dictionary<string, object>> serviceCallerCache = new Dictionary<Type, Dictionary<string, object>>();
        private readonly object lockObject = new object();

        public CallWrapperCache(IServiceTypeValidator serviceTypeValidator)
        {
            this.serviceTypeValidator = serviceTypeValidator;
        }

        public bool Contains<TServiceType>(string endpointName) where TServiceType : class
        {
            var serviceType = typeof(TServiceType);
            serviceTypeValidator.ValidateServiceType(serviceType);
            if (!serviceCallerCache.ContainsKey(serviceType))
            {
                return false;
            }
            var endpoints = serviceCallerCache[serviceType];
            if (endpoints == null || endpoints.Count == 0)
            {
                return false;
            }
            if (!endpoints.ContainsKey(endpointName) || endpoints[endpointName] == null)
            {
                return false;
            }
            return true;
        }

        public void Add<TServiceType>(ICallWrapper<TServiceType> callWrapper) where TServiceType : class
        {
            var serviceType = typeof(TServiceType);
            serviceTypeValidator.ValidateServiceType(serviceType);
            ThrowErrorIfAlreadyContainsEndpointForServiceType<TServiceType>(callWrapper.EndpointName);
            lock (lockObject)
            {
                ThrowErrorIfAlreadyContainsEndpointForServiceType<TServiceType>(callWrapper.EndpointName);
                if (!serviceCallerCache.ContainsKey(typeof(TServiceType)))
                {
                    serviceCallerCache.Add(serviceType, new Dictionary<string, object>());

                }
                if (!serviceCallerCache[serviceType].ContainsKey(callWrapper.EndpointName))
                {
                    serviceCallerCache[serviceType].Add(callWrapper.EndpointName, callWrapper);
                }
            }
        }

        public ICallWrapper<TServiceType> Get<TServiceType>(string endpointName) where TServiceType : class
        {
            var serviceType = typeof(TServiceType);
            if (!Contains<TServiceType>(endpointName))
            {
                throw new EndpointNotFoundForTypeException(endpointName, serviceType.Name);
            }
            var endpoints = serviceCallerCache[serviceType];
            var serviceCaller = endpoints[endpointName] as ICallWrapper<TServiceType>;
            if (serviceCaller == null)
            {
                throw new CallWrapperIsWrongTypeException(typeof(ICallWrapper<TServiceType>),
                                                            endpoints[endpointName].GetType());
            }
            return serviceCaller;
        }

        private void ThrowErrorIfAlreadyContainsEndpointForServiceType<TServiceType>(string endpointName) where TServiceType : class
        {
            var serviceType = typeof(TServiceType);
            if (Contains<TServiceType>(endpointName))
            {
                throw new EndpointForServiceTypeAlreadyExistsException(serviceType, endpointName);
            }
        }
    }
}