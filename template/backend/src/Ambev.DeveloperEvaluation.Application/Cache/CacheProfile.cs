using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Cache;

/// <summary>
/// Profile for mapping Cache
/// </summary>
public class CacheProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for cache
    /// </summary>
    public CacheProfile()
    {
        CreateMap(typeof(SetCacheCommand<>), typeof(CacheResult)).ReverseMap();
        CreateMap(typeof(SetCacheCommand<>), typeof(CacheResult<>)).ReverseMap();
        CreateMap<SetCacheCommand<dynamic>, CacheResult>().ReverseMap();
        

        CreateMap(typeof(GetCacheCommand<>), typeof(CacheResult<>)).ReverseMap();

        CreateMap(typeof(UpdateCacheCommand<>), typeof(CacheResult<>)).ReverseMap();
        CreateMap(typeof(DeleteCacheCommand<>), typeof(CacheResult<>)).ReverseMap();
    }
}
