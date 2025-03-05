namespace Ambev.DeveloperEvaluation.Common.Security;

public class SecuritySettings
{
    public string? IssuerKey { get; set; }
    public string? TokenAudirence { get; set; }
    public string? Issuer { get; set; }
    public int TokenExp { get; set; }
}
