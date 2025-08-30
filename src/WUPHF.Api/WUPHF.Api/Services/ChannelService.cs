using WUPHF.Shared.Models;

namespace WUPHF.Api.Services;

/// <summary>
/// Service for sending messages to different channels
/// </summary>
public interface IChannelService
{
    Task<WuphfDeliveryResult> SendToChannelAsync(WuphfChannel channel, string fromUser, string toUser, string message);
}

/// <summary>
/// Implementation of channel service - simulates sending to various platforms
/// "Facebook, Twitter, SMS, Email, Chat, and even prints to the nearest printer!" - Ryan Howard
/// </summary>
public class ChannelService : IChannelService
{
    private readonly ILogger<ChannelService> _logger;
    private readonly Random _random = new();

    public ChannelService(ILogger<ChannelService> logger)
    {
        _logger = logger;
    }

    public async Task<WuphfDeliveryResult> SendToChannelAsync(WuphfChannel channel, string fromUser, string toUser, string message)
    {
        _logger.LogInformation("Attempting to send WUPHF via {Channel} from {FromUser} to {ToUser}",
            channel, fromUser, toUser);

        // Simulate network delay
        await Task.Delay(_random.Next(100, 1000));

        var result = new WuphfDeliveryResult
        {
            Channel = channel,
            AttemptedAt = DateTime.UtcNow
        };

        // Simulate channel-specific behavior
        switch (channel)
        {
            case WuphfChannel.Facebook:
                result.Success = SimulateFacebookSend();
                result.ExternalId = result.Success ? $"fb_{Guid.NewGuid():N}" : null;
                if (!result.Success) result.ErrorMessage = "Facebook API rate limit exceeded";
                break;

            case WuphfChannel.Twitter:
                result.Success = SimulateTwitterSend(message);
                result.ExternalId = result.Success ? $"tw_{_random.Next(1000000, 9999999)}" : null;
                if (!result.Success) result.ErrorMessage = "Tweet too long or Twitter is down again";
                break;

            case WuphfChannel.SMS:
                result.Success = SimulateSMSSend();
                result.ExternalId = result.Success ? $"sms_{Guid.NewGuid():N}" : null;
                if (!result.Success) result.ErrorMessage = "Invalid phone number format";
                break;

            case WuphfChannel.Email:
                result.Success = true; // Email almost always works
                result.ExternalId = $"email_{Guid.NewGuid():N}";
                break;

            case WuphfChannel.Chat:
                result.Success = SimulateChatSend();
                result.ExternalId = result.Success ? $"chat_{DateTime.UtcNow.Ticks}" : null;
                if (!result.Success) result.ErrorMessage = "User is offline";
                break;

            case WuphfChannel.Printer:
                result.Success = SimulatePrinterSend();
                result.ExternalId = result.Success ? $"print_{_random.Next(1000, 9999)}" : null;
                if (!result.Success) result.ErrorMessage = "Printer out of paper or toner";
                break;

            case WuphfChannel.LinkedIn:
                result.Success = SimulateLinkedInSend();
                result.ExternalId = result.Success ? $"li_{Guid.NewGuid():N}" : null;
                if (!result.Success) result.ErrorMessage = "LinkedIn connection required";
                break;

            case WuphfChannel.Instagram:
                result.Success = SimulateInstagramSend();
                result.ExternalId = result.Success ? $"ig_{Guid.NewGuid():N}" : null;
                if (!result.Success) result.ErrorMessage = "Instagram requires visual content";
                break;

            default:
                result.Success = false;
                result.ErrorMessage = "Unknown channel";
                break;
        }

        if (result.Success)
        {
            _logger.LogInformation("WUPHF successfully sent via {Channel} with ID {ExternalId}",
                channel, result.ExternalId);
        }
        else
        {
            _logger.LogWarning("WUPHF failed to send via {Channel}: {Error}",
                channel, result.ErrorMessage);
        }

        return result;
    }

    private bool SimulateFacebookSend() => _random.Next(1, 101) <= 85; // 85% success rate
    private bool SimulateTwitterSend(string message) => message.Length <= 280 && _random.Next(1, 101) <= 80; // 80% success if not too long
    private bool SimulateSMSSend() => _random.Next(1, 101) <= 90; // 90% success rate
    private bool SimulateChatSend() => _random.Next(1, 101) <= 75; // 75% success rate
    private bool SimulatePrinterSend() => _random.Next(1, 101) <= 60; // 60% success rate (printers are unreliable!)
    private bool SimulateLinkedInSend() => _random.Next(1, 101) <= 70; // 70% success rate
    private bool SimulateInstagramSend() => _random.Next(1, 101) <= 65; // 65% success rate
}
