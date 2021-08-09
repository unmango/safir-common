using System;
using Google.Protobuf.Reflection;
using Grpc.AspNetCore.Server;
using Grpc.AspNetCore.Server.Model;
using Grpc.Core;
using Microsoft.AspNetCore.Grpc.HttpApi;
using Microsoft.Extensions.Logging;

namespace Safir.Grpc.HttpApi
{
    internal class CustomHttpApiProviderServiceBinder<TService> : ServiceBinderBase
        where TService : class
    {
        private readonly ServiceMethodProviderContext<TService> _context;
        private readonly Type _declaringType;
        private readonly ServiceDescriptor _serviceDescriptor;
        private readonly GrpcServiceOptions _globalOptions;
        private readonly GrpcServiceOptions<TService> _serviceOptions;
        private readonly IGrpcServiceActivator<TService> _serviceActivator;
        private readonly GrpcHttpApiOptions _httpApiOptions;
        private readonly ILogger _logger;
        
        public CustomHttpApiProviderServiceBinder(
            ServiceMethodProviderContext<TService> context,
            Type declaringType,
            ServiceDescriptor serviceDescriptor,
            GrpcServiceOptions globalOptions,
            GrpcServiceOptions<TService> serviceOptions,
            IServiceProvider serviceProvider,
            ILoggerFactory loggerFactory,
            IGrpcServiceActivator<TService> serviceActivator,
            GrpcHttpApiOptions httpApiOptions)
        {
            _context = context;
            _declaringType = declaringType;
            _serviceDescriptor = serviceDescriptor;
            _globalOptions = globalOptions;
            _serviceOptions = serviceOptions;
            _serviceActivator = serviceActivator;
            _httpApiOptions = httpApiOptions;
            _logger = loggerFactory.CreateLogger<CustomHttpApiProviderServiceBinder<TService>>();
        }
    }
}
