using Grpc.AspNetCore.Server.Model;

namespace Safir.Grpc.HttpApi
{
    public class CustomHttpApiServiceMethodProvider<TService> : IServiceMethodProvider<TService>
        where TService : class
    {
        

        public void OnServiceMethodDiscovery(ServiceMethodProviderContext<TService> context)
        {
            throw new System.NotImplementedException();
        }
    }
}
