namespace Ambev.DeveloperEvaluation.NoSql.MDb;
public class MongoSettings
{
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
    public string? CollectionNameShoppingExpired { get; set; }
    public string? CollectionNameShoppingPay { get; set; }
}
