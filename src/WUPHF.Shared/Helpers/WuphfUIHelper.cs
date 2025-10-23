using WUPHF.Shared.Models;

namespace WUPHF.Shared.Helpers;

/// <summary>
/// UI helper utilities for WUPHF display elements
/// Centralizes icon and status mapping logic used across Web and Mobile applications
/// </summary>
public static class WuphfUIHelper
{
    /// <summary>
    /// Gets the emoji icon for a WUPHF channel
    /// </summary>
    public static string GetChannelIcon(WuphfChannel channel) => channel switch
    {
        WuphfChannel.Facebook => "📘",
        WuphfChannel.Twitter => "🐦",
        WuphfChannel.SMS => "💬",
        WuphfChannel.Email => "📧",
        WuphfChannel.Chat => "💭",
        WuphfChannel.Printer => "🖨️",
        WuphfChannel.LinkedIn => "💼",
        WuphfChannel.Instagram => "📸",
        _ => "📱"
    };

    /// <summary>
    /// Gets the emoji icon for a WUPHF status
    /// </summary>
    public static string GetStatusIcon(WuphfStatus status) => status switch
    {
        WuphfStatus.Pending => "⏳",
        WuphfStatus.Sending => "🚀",
        WuphfStatus.Delivered => "✅",
        WuphfStatus.Failed => "❌",
        WuphfStatus.PartiallyDelivered => "⚠️",
        _ => "❓"
    };

    /// <summary>
    /// Gets Bootstrap CSS badge class for a WUPHF status (for web applications)
    /// </summary>
    public static string GetStatusBadgeClass(WuphfStatus status) => status switch
    {
        WuphfStatus.Delivered => "bg-success",
        WuphfStatus.Failed => "bg-danger",
        WuphfStatus.PartiallyDelivered => "bg-warning",
        WuphfStatus.Sending => "bg-info",
        WuphfStatus.Pending => "bg-secondary",
        _ => "bg-secondary"
    };

    /// <summary>
    /// Gets a color code for a WUPHF status (hex format for cross-platform use)
    /// </summary>
    public static string GetStatusColorHex(WuphfStatus status) => status switch
    {
        WuphfStatus.Delivered => "#28a745",    // Green
        WuphfStatus.Failed => "#dc3545",       // Red
        WuphfStatus.PartiallyDelivered => "#ffc107", // Orange
        WuphfStatus.Sending => "#17a2b8",      // Blue
        WuphfStatus.Pending => "#6c757d",      // Gray
        _ => "#6c757d"                         // Gray
    };
}
