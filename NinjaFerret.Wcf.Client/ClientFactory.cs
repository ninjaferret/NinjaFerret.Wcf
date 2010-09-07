/**
 * This file is part of NinjaFerret.Wcf.Client
 * 
 *  NinjaFerret.Wcf.Client is a framework to automatically generate a proxy client
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
    public class ClientFactory<TServiceInterface> where TServiceInterface : class
    {
        private Type _serviceType;
        private readonly object _lockObject = new object();
        private readonly ICallWrapperCache _callWrapperCache;
        private readonly IClientGenerator _clientGenerator;
        private readonly IServiceTypeValidator _serviceTypeValidator;
        private readonly IExceptionTranslator _exceptionTranslator;

        public ClientFactory(IExceptionTranslator exceptionTranslator)
        {
            _exceptionTranslator = exceptionTranslator;
            _serviceTypeValidator = new ServiceTypeValidator();
            _clientGenerator = new ClientGenerator();
            _callWrapperCache = new CallWrapperCache(_serviceTypeValidator);
        }

        public TServiceInterface Generate()
        {
            return Generate(DefaultEnpointName());
        }

        // This has to be different because overloading causes some DI frameworks (i.e. Castle) to fall over
        // and since I want to support dependency injection frameworks I have decided to give this a different
        // name rather than overloading.
        public TServiceInterface Generate(string endpointName)
        {
            _serviceTypeValidator.ValidateServiceType(typeof(TServiceInterface));
            CreateTypeIfRequired();

            var callWrapper = GetServiceCallerForEndpoint(endpointName);

            return (TServiceInterface)Activator.CreateInstance(_serviceType, new object[] {callWrapper});
        }

        private void CreateTypeIfRequired()
        {
            if (_serviceType != null)
            {
                return;
            }
            lock (_lockObject)
            {
                _serviceType = _clientGenerator.CreateType<TServiceInterface>();
            }
        }

        private ICallWrapper<TServiceInterface> GetServiceCallerForEndpoint(string endpointName)
        {
            if (!_callWrapperCache.Contains<TServiceInterface>(endpointName))
            {
                _callWrapperCache.Add(new CallWrapper<TServiceInterface>(_exceptionTranslator)); 
            }
            return _callWrapperCache.Get<TServiceInterface>(endpointName);
        }

        private static string DefaultEnpointName()
        {
            return typeof (TServiceInterface).Name; 
        }
        
    }


}


