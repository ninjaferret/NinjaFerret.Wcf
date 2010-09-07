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
namespace NinjaFerret.Wcf.Client.CallWrapper
{
    public delegate void MakeCallToTheWcfServiceDelegate<in TServiceInterface>(TServiceInterface service)
        where TServiceInterface : class;

    public interface ICallWrapper<out TServiceInterface> where TServiceInterface : class
    {
        string EndpointName { get; }
        void Call(MakeCallToTheWcfServiceDelegate<TServiceInterface> codeBlock);
    }
}