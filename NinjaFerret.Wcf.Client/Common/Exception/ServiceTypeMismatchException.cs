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

namespace NinjaFerret.Wcf.Client.Common.Exception
{
    public class ServiceTypeMismatchException : System.Exception
    {
        public ServiceTypeMismatchException(Type serviceType, Type implementationType)
            : base(string.Format("{0} does not implement {1}", implementationType, serviceType))
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
        }

        public Type ServiceType
        {
            get; private set;
        }

        public Type ImplementationType
        {
            get; private set;
        }
    }
}