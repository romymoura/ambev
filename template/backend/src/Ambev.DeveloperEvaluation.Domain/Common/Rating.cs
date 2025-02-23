namespace Ambev.DeveloperEvaluation.Domain.Common;
public class Rating : BaseEntity
{
    public Rating()
    {
        CreatedAt = DateTime.Now;
    }
    public double Rate { get; set; }
    public int Count { get; set; }
}