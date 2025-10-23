using WUPHF.Shared.Models;
using WUPHF.Shared.Helpers;

namespace WUPHF.Shared.ViewModels;

/// <summary>
/// View model for displaying WUPHF messages in UI lists
/// Provides pre-formatted data for easier data binding in Web and Mobile applications
/// </summary>
public class WuphfMessageViewModel
{
    public WuphfMessageViewModel(WuphfMessage message)
    {
        Id = message.Id;
        FromUser = message.FromUser;
        ToUser = message.ToUser;
        
        // Truncate long messages for list display
        Message = message.Message.Length > 100 
            ? message.Message.Substring(0, 100) + "..." 
            : message.Message;
        
        FullMessage = message.Message;
        SentAt = message.SentAt;
        Status = message.Status.ToString();
        StatusEnum = message.Status;
        Channels = message.Channels;
        DeliveryResults = message.DeliveryResults;
        
        // Pre-compute UI helpers
        StatusColorHex = WuphfUIHelper.GetStatusColorHex(message.Status);
        StatusIcon = WuphfUIHelper.GetStatusIcon(message.Status);
        StatusBadgeClass = WuphfUIHelper.GetStatusBadgeClass(message.Status);
    }

    // Constructor for derived types
    protected WuphfMessageViewModel(Guid id, string fromUser, string toUser, string fullMessage, 
        DateTime sentAt, WuphfStatus statusEnum, List<WuphfChannel> channels, List<WuphfDeliveryResult> deliveryResults)
    {
        Id = id;
        FromUser = fromUser;
        ToUser = toUser;
        Message = fullMessage.Length > 100 ? fullMessage.Substring(0, 100) + "..." : fullMessage;
        FullMessage = fullMessage;
        SentAt = sentAt;
        Status = statusEnum.ToString();
        StatusEnum = statusEnum;
        Channels = channels;
        DeliveryResults = deliveryResults;
        
        StatusColorHex = WuphfUIHelper.GetStatusColorHex(statusEnum);
        StatusIcon = WuphfUIHelper.GetStatusIcon(statusEnum);
        StatusBadgeClass = WuphfUIHelper.GetStatusBadgeClass(statusEnum);
    }

    public Guid Id { get; set; }
    public string FromUser { get; set; }
    public string ToUser { get; set; }
    public string Message { get; set; }
    public string FullMessage { get; set; }
    public DateTime SentAt { get; set; }
    public string Status { get; set; }
    public WuphfStatus StatusEnum { get; set; }
    public List<WuphfChannel> Channels { get; set; }
    public List<WuphfDeliveryResult> DeliveryResults { get; set; }
    
    // Pre-computed UI properties
    public string StatusColorHex { get; set; }
    public string StatusIcon { get; set; }
    public string StatusBadgeClass { get; set; }
}
