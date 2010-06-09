/**
 * This file is part of WcfClientFactory
 * 
 *  WcfClientFactory is a tool to automatically generate a proxy client
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
using NinjaFerret.Wcf.Client.Common;

namespace NinjaFerret.Wcf.Client.Cache
{
    public class TypeCache : ITypeCache
    {
        private readonly IServiceTypeValidator serviceTypeValidator;
        private readonly Dictionary<Type, Type> cache = new Dictionary<Type, Type>();
        private readonly object lockObject = new object();

        public TypeCache(IServiceTypeValidator serviceTypeValidator)
        {
            this.serviceTypeValidator = serviceTypeValidator;
        }

        public bool Contains<TServiceType>() where TServiceType : class
        {
            serviceTypeValidator.ValidateServiceType(typeof(TServiceType));
            return cache.ContainsKey(typeof (TServiceType));
        }

        public void AddType<TServiceType>(Type client) where TServiceType : class
        {
            var serviceType = typeof(TServiceType);
            serviceTypeValidator.ValidateImplementationType(serviceType, client);
            ThrowExceptionIfAlreadyContainsServiceType<TServiceType>();
            lock(lockObject)
            {
                ThrowExceptionIfAlreadyContainsServiceType<TServiceType>();
                cache.Add(serviceType, client);
            }
        }

        public Type GetType<TServiceType>() where TServiceType : class
        {
            serviceTypeValidator.ValidateServiceType(typeof(TServiceType));
            if (Contains<TServiceType>())
            {
                return cache[typeof(TServiceType)];
            }
            throw new ServiceInterfaceTypeNotFoundException(typeof(TServiceType));
        }

        public int Count { get { return cache.Count; } }

        private void ThrowExceptionIfAlreadyContainsServiceType<TServiceType>() where TServiceType : class
        {
            var serviceType = typeof(TServiceType);
            if (Contains<TServiceType>())
            {
                throw new ServiceTypeAlreadyAddedException(serviceType);
            }
        }
    }
}