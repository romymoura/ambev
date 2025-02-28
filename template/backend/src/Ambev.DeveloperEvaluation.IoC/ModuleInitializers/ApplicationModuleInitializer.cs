using Ambev.DeveloperEvaluation.Application.Cache;
using Ambev.DeveloperEvaluation.Common.Security;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class ApplicationModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        builder.Services.AddTransient(typeof(IRequestHandler<GetCacheCommand<dynamic>, CacheResult<dynamic>>), typeof(GetCacheCommandHandler<dynamic>));
        
        
        builder.Services.AddTransient(typeof(IRequestHandler<SetCacheCommand<dynamic>, CacheResult>), typeof(SetCacheCommandHandler<dynamic>));
        builder.Services.AddTransient(typeof(IRequestHandler<SetCacheCommand<List<dynamic>>, CacheResult>), typeof(SetCacheCommandHandler<List<dynamic>>));



        builder.Services.AddTransient(typeof(IRequestHandler<UpdateCacheCommand<dynamic>, CacheResult>), typeof(UpdateCacheCommandHandler<dynamic>));
        builder.Services.AddTransient(typeof(IRequestHandler<DeleteCacheCommand, CacheResult>), typeof(DeleteCacheCommandHandler));
    }
}