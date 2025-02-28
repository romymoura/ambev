namespace Ambev.DeveloperEvaluation.Cache;

public class RedisSettings
{
    public string? ConnectionString { get; set; }
    public string? Password { get; set; }
    public string? InstanceName { get; set; }
    public int DatabaseId { get; set; }
    public bool EnableKeyspaceNotifications { get; set; }
    public string? NotifykeyspaceEvents {  get; set; }
    public string? NotifykeyspaceEventsValue { get; set; }
}
