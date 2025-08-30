using WUPHF.Shared.Models;

namespace WUPHF.Api.Services;

/// <summary>
/// Service interface for WUPHF operations
/// "The ultimate social networking experience!" - Ryan Howard
/// </summary>
public interface IWuphfService
{
    Task<WuphfMessage> SendWuphfAsync(string fromUser, string toUser, string message, List<WuphfChannel> channels);
    Task<List<WuphfMessage>> GetWuphfHistoryAsync(string? userName = null, DateTime? fromDate = null, DateTime? toDate = null);
    Task<WuphfMessage?> GetWuphfByIdAsync(Guid id);
}

/// <summary>
/// Main WUPHF service implementation
/// "WUPHF! The ultimate social networking experience!" - Ryan Howard
/// </summary>
public class WuphfService : IWuphfService
{
    private static readonly List<WuphfMessage> _messages = new();
    private readonly IChannelService _channelService;
    private readonly ILogger<WuphfService> _logger;

    public WuphfService(IChannelService channelService, ILogger<WuphfService> logger)
    {
        _channelService = channelService;
        _logger = logger;
    }

    public async Task<WuphfMessage> SendWuphfAsync(string fromUser, string toUser, string message, List<WuphfChannel> channels)
    {
        _logger.LogInformation("WUPHF! Sending message from {FromUser} to {ToUser} via {ChannelCount} channels",
            fromUser, toUser, channels.Count);

        var wuphfMessage = new WuphfMessage
        {
            FromUser = fromUser,
            ToUser = toUser,
            Message = message,
            Channels = channels,
            Status = WuphfStatus.Sending
        };

        _messages.Add(wuphfMessage);

        // Send to all channels concurrently - because that's the WUPHF way!
        var deliveryTasks = channels.Select(channel =>
            _channelService.SendToChannelAsync(channel, fromUser, toUser, message));

        var deliveryResults = await Task.WhenAll(deliveryTasks);
        wuphfMessage.DeliveryResults = deliveryResults.ToList();

        // Determine final status
        var successCount = deliveryResults.Count(r => r.Success);
        wuphfMessage.Status = successCount switch
        {
            0 => WuphfStatus.Failed,
            var count when count == channels.Count => WuphfStatus.Delivered,
            _ => WuphfStatus.PartiallyDelivered
        };

        _logger.LogInformation("WUPHF sent! Status: {Status}, Success rate: {SuccessRate}%",
            wuphfMessage.Status, (successCount * 100) / channels.Count);

        return wuphfMessage;
    }

    public Task<List<WuphfMessage>> GetWuphfHistoryAsync(string? userName = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = _messages.AsQueryable();

        if (!string.IsNullOrEmpty(userName))
        {
            query = query.Where(m => m.FromUser.Equals(userName, StringComparison.OrdinalIgnoreCase) ||
                                   m.ToUser.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }

        if (fromDate.HasValue)
        {
            query = query.Where(m => m.SentAt >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(m => m.SentAt <= toDate.Value);
        }

        return Task.FromResult(query.OrderByDescending(m => m.SentAt).ToList());
    }

    public Task<WuphfMessage?> GetWuphfByIdAsync(Guid id)
    {
        var message = _messages.FirstOrDefault(m => m.Id == id);
        return Task.FromResult(message);
    }
}
