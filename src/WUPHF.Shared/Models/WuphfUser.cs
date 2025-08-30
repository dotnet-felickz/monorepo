namespace WUPHF.Shared.Models;

/// <summary>
/// User profile for WUPHF - because everyone needs to be WUPHFed!
/// </summary>
public class WuphfUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<WuphfChannelConfig> ChannelConfigs { get; set; } = new();
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Ryan Howard's favorite quote about WUPHF
    /// </summary>
    public string? PersonalWuphfQuote { get; set; } = "I'm gonna be rich!";
}

/// <summary>
/// Configuration for each channel per user
/// </summary>
public class WuphfChannelConfig
{
    public WuphfChannel Channel { get; set; }
    public bool IsEnabled { get; set; } = true;
    public string? Handle { get; set; } // Username/handle for the channel
    public string? AccessToken { get; set; } // For OAuth channels
    public Dictionary<string, string> AdditionalSettings { get; set; } = new();
}
