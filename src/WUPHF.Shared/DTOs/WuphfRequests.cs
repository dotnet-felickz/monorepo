using WUPHF.Shared.Models;

namespace WUPHF.Shared.DTOs;

/// <summary>
/// Request to send a WUPHF message
/// "The ultimate social networking experience!" - Ryan Howard
/// </summary>
public class SendWuphfRequest
{
    public string FromUser { get; set; } = string.Empty;
    public string ToUser { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public List<WuphfChannel> Channels { get; set; } = new();
    public bool PrintWuphf { get; set; } = true; // Always default to printing!
}

/// <summary>
/// Response after sending a WUPHF
/// </summary>
public class SendWuphfResponse
{
    public Guid MessageId { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public int ChannelsAttempted { get; set; }
    public int ChannelsSuccessful { get; set; }
    public List<string> FailedChannels { get; set; } = new();

    /// <summary>
    /// Ryan's enthusiasm level based on success rate
    /// </summary>
    public string RyanReaction => ChannelsSuccessful switch
    {
        0 => "That's what she said... wait, that doesn't work here.",
        var n when n == ChannelsAttempted => "WUPHF! We did it! I'm gonna be rich!",
        var n when n > ChannelsAttempted / 2 => "Pretty good, but we can do better!",
        _ => "This is a disaster. I need to call my lawyer."
    };
}

/// <summary>
/// Request to get WUPHF history
/// </summary>
public class GetWuphfHistoryRequest
{
    public string? UserName { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
