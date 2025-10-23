using WUPHF.Shared.Constants;
using WUPHF.Shared.Models;

namespace WUPHF.Shared.Services;

/// <summary>
/// Implementation of WUPHF validation service
/// "Quality control is important, even for WUPHF!" - Ryan Howard (probably)
/// </summary>
public class WuphfValidationService : IWuphfValidationService
{
    public ValidationResult ValidateMessage(string message, List<WuphfChannel> channels)
    {
        // Validate message content
        if (string.IsNullOrWhiteSpace(message))
        {
            return ValidationResult.Failure("Message cannot be empty! Ryan says: 'You can't WUPHF nothing!'");
        }

        // Validate message length
        var lengthResult = ValidateMessageLength(message);
        if (!lengthResult.IsValid)
        {
            return lengthResult;
        }

        // Validate channels
        var channelsResult = ValidateChannels(channels);
        if (!channelsResult.IsValid)
        {
            return channelsResult;
        }

        return ValidationResult.Success();
    }

    public ValidationResult ValidateMessageLength(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return ValidationResult.Success(); // Empty check should be done separately
        }

        if (message.Length > WuphfConstants.Limits.MaxMessageLength)
        {
            return ValidationResult.Failure(
                $"Message too long! Max length is {WuphfConstants.Limits.MaxMessageLength} characters.");
        }

        return ValidationResult.Success();
    }

    public ValidationResult ValidateChannels(List<WuphfChannel> channels)
    {
        if (channels == null || !channels.Any())
        {
            return ValidationResult.Failure("No channels selected! The whole point of WUPHF is to send everywhere!");
        }

        if (channels.Count > WuphfConstants.Limits.MaxChannelsPerMessage)
        {
            return ValidationResult.Failure(
                $"Too many channels! Maximum is {WuphfConstants.Limits.MaxChannelsPerMessage}.");
        }

        return ValidationResult.Success();
    }
}
