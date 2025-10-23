using WUPHF.Shared.Models;

namespace WUPHF.Shared.Services;

/// <summary>
/// Service for validating WUPHF messages and requests
/// Centralizes all validation logic to ensure consistency across applications
/// </summary>
public interface IWuphfValidationService
{
    /// <summary>
    /// Validates a WUPHF message for sending
    /// </summary>
    ValidationResult ValidateMessage(string message, List<WuphfChannel> channels);
    
    /// <summary>
    /// Validates message length
    /// </summary>
    ValidationResult ValidateMessageLength(string message);
    
    /// <summary>
    /// Validates that at least one channel is selected
    /// </summary>
    ValidationResult ValidateChannels(List<WuphfChannel> channels);
}

/// <summary>
/// Result of a validation operation
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    
    public static ValidationResult Success() => new() { IsValid = true };
    
    public static ValidationResult Failure(string errorMessage) => new()
    {
        IsValid = false,
        ErrorMessage = errorMessage
    };
}
