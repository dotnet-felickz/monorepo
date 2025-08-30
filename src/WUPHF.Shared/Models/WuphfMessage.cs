namespace WUPHF.Shared.Models;

/// <summary>
/// Represents a WUPHF message that gets sent across all channels
/// "WUPHF: The convergence of everything, everywhere, all at once!" - Ryan Howard
/// </summary>
public class WuphfMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FromUser { get; set; } = string.Empty;
    public string ToUser { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public List<WuphfChannel> Channels { get; set; } = new();
    public WuphfStatus Status { get; set; } = WuphfStatus.Pending;
    public List<WuphfDeliveryResult> DeliveryResults { get; set; } = new();
}

/// <summary>
/// Available WUPHF channels - because why send one message when you can send it everywhere?
/// </summary>
public enum WuphfChannel
{
    Facebook,
    Twitter,
    SMS,
    Email,
    Chat,
    Printer,
    LinkedIn,
    Instagram
}

/// <summary>
/// WUPHF message status
/// </summary>
public enum WuphfStatus
{
    Pending,
    Sending,
    Delivered,
    Failed,
    PartiallyDelivered
}

/// <summary>
/// Result of delivering a WUPHF to a specific channel
/// </summary>
public class WuphfDeliveryResult
{
    public WuphfChannel Channel { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;
    public string? ExternalId { get; set; } // ID from the external service
}
