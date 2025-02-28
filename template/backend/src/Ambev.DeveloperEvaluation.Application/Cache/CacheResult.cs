namespace Ambev.DeveloperEvaluation.Application.Cache;

public class CacheResult
{
    public bool Action { get; set; }
}
public class CacheResult<T> : CacheResult
{
    public T? Value { get; set; }
}
