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