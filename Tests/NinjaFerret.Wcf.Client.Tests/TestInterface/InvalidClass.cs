using NinjaFerret.Wcf.Client.CallWrapper;

namespace NinjaFerret.Wcf.Client.Tests.TestInterface
{
    public class InvalidClass<T> where T : class
    {
        private readonly ICallWrapper<T> callWrapper;

        public InvalidClass(ICallWrapper<T> callWrapper)
        {
            this.callWrapper = callWrapper;
        }

        public ICallWrapper<T> CallWrapper
        {
            get { return callWrapper; }
        }
    }
}