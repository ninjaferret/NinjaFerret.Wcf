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