namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

public class SetCartRequest
{
    public dynamic? Value { get; set; } = null;
    public string? Key { get; set; }
    public TimeSpan? Expiration { get; set; }
}
