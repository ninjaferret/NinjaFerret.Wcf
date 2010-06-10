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
using NinjaFerret.Wcf.Client.Cache;
using NinjaFerret.Wcf.Client.CallWrapper;
using NinjaFerret.Wcf.Client.Common;
using NinjaFerret.Wcf.Client.Generator;
using NinjaFerret.Wcf.Exception;

namespace NinjaFerret.Wcf.Client
{
    public class ClientFactory
    {
        private readonly IServiceTypeValidator _serviceTypeValidator;
        private readonly ITypeCache _cache;
        private readonly ICallWrapperCache _callWrapperCache;
        private readonly IClientGenerator _clientGenerator;
        private readonly IExceptionManager _exceptionManager;

        public ClientFactory(IServiceTypeValidator serviceTypeValidator, 
            ITypeCache cache, 
            ICallWrapperCache callWrapperCache, 
            IClientGenerator clientGenerator,
            IExceptionManager exceptionManager)
        {
            _serviceTypeValidator = serviceTypeValidator;
            _clientGenerator = clientGenerator;
            _exceptionManager = exceptionManager;
            _callWrapperCache = callWrapperCache;
            _cache = cache;
        }

        public TServiceInterface GenerateClient<TServiceInterface>() where TServiceInterface : class
        {
            return GenerateClient<TServiceInterface>(DefaultEnpointName<TServiceInterface>());
        }

        public TServiceInterface GenerateClient<TServiceInterface>(string endpointName) where TServiceInterface : class
        {
            _serviceTypeValidator.ValidateServiceType(typeof(TServiceInterface));
            CreateTypeIfRequired<TServiceInterface>();
            var callWrapper = GetServiceCallerForEndpoint<TServiceInterface>(endpointName);
            var clientType = _cache.GetType<TServiceInterface>();
            return (TServiceInterface)Activator.CreateInstance(clientType, new object[] {callWrapper, _exceptionManager});
        }

        private ICallWrapper<TServiceInterface> GetServiceCallerForEndpoint<TServiceInterface>(string endpointName) where TServiceInterface : class
        {
            if (!_callWrapperCache.Contains<TServiceInterface>(endpointName))
            {
                _callWrapperCache.Add(new CallWrapper<TServiceInterface>()); 
            }
            return _callWrapperCache.Get<TServiceInterface>(endpointName);
        }

        private void CreateTypeIfRequired<TServiceInterface>() where TServiceInterface : class
        {
            if (_cache.Contains<TServiceInterface>())
            {
                return;
            }
            var client = _clientGenerator.CreateType<TServiceInterface>(_exceptionManager);
            _cache.AddType<TServiceInterface>(client);
        }

        private static string DefaultEnpointName<TServiceInterface>()
        {
            return typeof (TServiceInterface).Name; 
        }
        
    }
}


