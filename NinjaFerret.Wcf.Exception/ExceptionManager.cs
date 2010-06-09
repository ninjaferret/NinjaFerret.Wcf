using System;
using System.Collections.Generic;
using System.ServiceModel;
using NinjaFerret.Wcf.Exception.Exceptions;
using NinjaFerret.Wcf.Exception.Faults;

namespace NinjaFerret.Wcf.Exception
{
    public class ExceptionManager : IExceptionManager
    {
        private readonly Dictionary<Type, IExceptionTranslator> _faultToExceptionTranslators = new Dictionary<Type, IExceptionTranslator>();
        private readonly Dictionary<Type, IExceptionTranslator> _exceptionToFaultTranslators = new Dictionary<Type, IExceptionTranslator>();

        public void RegisterExceptionTranslator(IExceptionTranslator translator)
        {
            if (_faultToExceptionTranslators.ContainsKey(translator.FaultType)) return;
            lock (_faultToExceptionTranslators)
            {
                // double check
                AddFaultToExceptionTranslator(translator);
                AddExceptionToFaultTranslator(translator);
            }
        }

        public void RemoveExceptionTranslator(IExceptionTranslator translator)
        {
            if (!_faultToExceptionTranslators.ContainsKey(translator.FaultType)) return;
            lock(_faultToExceptionTranslators)
            {
                // double check
                RemoveFaultToExceptionTranslator(translator);
                RemoveExceptionToFaultTranslator(translator);
            }
        }

        public FaultException ProcessException(System.Exception exception)
        {
            var type = exception.GetType();
            if (!_exceptionToFaultTranslators.ContainsKey(type))
                return
                    new FaultException<TranslationFailedFault>(new TranslationFailedFault
                                                                   {
                                                                       Message = exception.Message,
                                                                       TranslationFailureReason =
                                                                           "Could not find registered translator"
                                                                   });
            return _exceptionToFaultTranslators[type].ConvertToFaultException(exception);
        }

        public System.Exception ProcessFault<TFault>(FaultException<TFault> faultException) where TFault : Fault
        {
            var type = faultException.Detail.GetType();
            if (!_faultToExceptionTranslators.ContainsKey(type))
                return new TranslatorNotFoundException(faultException.Message, faultException.Detail.GetType(), faultException);
            return _faultToExceptionTranslators[type].ConvertToException(faultException.Detail, faultException);
        }


        private void AddExceptionToFaultTranslator(IExceptionTranslator translator)
        {
            if (_exceptionToFaultTranslators.ContainsKey(translator.ExceptionType)) return;
            _exceptionToFaultTranslators.Add(translator.ExceptionType, translator);
        }

        private void AddFaultToExceptionTranslator(IExceptionTranslator translator)
        {
            if (_faultToExceptionTranslators.ContainsKey(translator.FaultType)) return;
            _faultToExceptionTranslators.Add(translator.FaultType, translator);
        }

        private void RemoveFaultToExceptionTranslator(IExceptionTranslator translator)
        {
            if (!_faultToExceptionTranslators.ContainsKey(translator.FaultType)) return;
            _faultToExceptionTranslators.Remove(translator.FaultType);
        }

        private void RemoveExceptionToFaultTranslator(IExceptionTranslator translator)
        {
            if (!_exceptionToFaultTranslators.ContainsKey(translator.ExceptionType)) return;
            _exceptionToFaultTranslators.Remove(translator.ExceptionType);
        }
    }
}