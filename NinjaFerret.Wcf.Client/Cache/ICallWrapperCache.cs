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
using NinjaFerret.Wcf.Client.CallWrapper;

namespace NinjaFerret.Wcf.Client.Cache
{
    public interface ICallWrapperCache
    {
        bool Contains<TServiceType>(string endpointName) where TServiceType : class;
        ICallWrapper<TServiceType> Get<TServiceType>(string endpointName) where TServiceType : class;
        void Add<TServiceType>(ICallWrapper<TServiceType> callWrapper) where TServiceType : class;
    }
}