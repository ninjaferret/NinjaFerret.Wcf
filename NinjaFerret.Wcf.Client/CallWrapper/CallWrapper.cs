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
using System.ServiceModel;

namespace NinjaFerret.Wcf.Client.CallWrapper
{
    public class CallWrapper<TServiceInterface> : ICallWrapper<TServiceInterface> where TServiceInterface : class
    {
        private readonly ChannelFactory<TServiceInterface> channelFactory;

        public string EndpointName { get; private set; }

        public CallWrapper() : this (typeof(TServiceInterface).Name)
        {
            
        }

        public CallWrapper(string endpointName)
        {
            EndpointName = endpointName;
            channelFactory = new ChannelFactory<TServiceInterface>(endpointName);
        }

        public void Call(MakeCallToTheWcfServiceDelegate<TServiceInterface> codeBlock)
        {
            IClientChannel proxy = null;
            var closed = false;
            if (codeBlock == null)
            {
                throw new ArgumentNullException("codeBlock", "The lambda expression cannot be null");
            }
            try
            {
                proxy = (IClientChannel)channelFactory.CreateChannel();
                codeBlock((TServiceInterface)proxy);
                proxy.Close();
                closed = true;
            }
            finally
            {
                if (!closed && proxy != null)
                {
                    proxy.Abort();
                }
            }
        }

    }
}