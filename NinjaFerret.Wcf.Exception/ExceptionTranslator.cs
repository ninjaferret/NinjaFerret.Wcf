/**
 * This file is part of the NinjaFerret.Wcf.Exception framework
 * 
 *  NinjaFerret.Wcf.Exception package is framework for seamlessly translates 
 *  Exceptions into Faults or vice versa across the WCF boundary. This has been
 *  designed to work with the NinjaFerret.Wcf.Client framework
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
using System.ServiceModel;

namespace NinjaFerret.Wcf.Exception
{
    public class ExceptionTranslator : IExceptionTranslator
    {
        public FaultException ToFaultException(System.Exception exception)
        {
            var typeException = exception as ITranslatableException;
            return typeException == null
                       ? new FaultException("A fault occurred calling this service.")
                       : typeException.ToFaultException();
        }

        public System.Exception ToException(FaultException faultException)
        {
            var type = faultException.GetType();
            if (!type.IsGenericType)
                return new System.Exception(faultException.Message);

            var property = type.GetProperty("Detail");
            if (property == null || !property.PropertyType.IsSubclassOf(typeof(TranslatableFault)))
                return new System.Exception(faultException.Message);

            var fault = (TranslatableFault)property.GetValue(faultException, null);
            return fault.ToException();
        }
    }
}