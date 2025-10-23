using Microsoft.AspNetCore.Mvc;
using WUPHF.Api.Services;
using WUPHF.Shared.DTOs;
using WUPHF.Shared.Models;
using WUPHF.Shared.Constants;
using WUPHF.Shared.Services;

namespace WUPHF.Api.Controllers;

/// <summary>
/// WUPHF API Controller - The ultimate social networking experience!
/// "Imagine you're in an accident and you need to contact someone. Instead of being like 'Help me,' you'd be like 'WUPHF me!'" - Ryan Howard
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class WuphfController : ControllerBase
{
    private readonly IWuphfService _wuphfService;
    private readonly IWuphfValidationService _validationService;
    private readonly ILogger<WuphfController> _logger;

    public WuphfController(IWuphfService wuphfService, IWuphfValidationService validationService, ILogger<WuphfController> logger)
    {
        _wuphfService = wuphfService;
        _validationService = validationService;
        _logger = logger;
    }

    /// <summary>
    /// Send a WUPHF message to all channels!
    /// "Facebook, Twitter, Google Plus, Instagram, all your other social media sites, email, text messaging, and the home phone all in one." - Ryan Howard
    /// </summary>
    [HttpPost("send")]
    public async Task<ActionResult<SendWuphfResponse>> SendWuphf([FromBody] SendWuphfRequest request)
    {
        try
        {
            _logger.LogInformation("WUPHF! Received request to send message from {FromUser} to {ToUser}",
                request.FromUser, request.ToUser);

            // Validate request using shared validation service
            var validationResult = _validationService.ValidateMessage(request.Message, request.Channels);
            if (!validationResult.IsValid)
            {
                return BadRequest(new SendWuphfResponse
                {
                    Success = false,
                    ErrorMessage = validationResult.ErrorMessage
                });
            }

            // Add printer channel if requested (Ryan's favorite feature!)
            if (request.PrintWuphf && !request.Channels.Contains(WuphfChannel.Printer))
            {
                request.Channels.Add(WuphfChannel.Printer);
            }

            var result = await _wuphfService.SendWuphfAsync(
                request.FromUser,
                request.ToUser,
                request.Message,
                request.Channels);

            var response = new SendWuphfResponse
            {
                MessageId = result.Id,
                Success = result.Status != WuphfStatus.Failed,
                ChannelsAttempted = result.Channels.Count,
                ChannelsSuccessful = result.DeliveryResults.Count(r => r.Success),
                FailedChannels = result.DeliveryResults
                    .Where(r => !r.Success)
                    .Select(r => r.Channel.ToString())
                    .ToList()
            };

            if (result.Status == WuphfStatus.Failed)
            {
                response.ErrorMessage = "All channels failed. Ryan is not pleased.";
                return StatusCode(500, response);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending WUPHF");
            return StatusCode(500, new SendWuphfResponse
            {
                Success = false,
                ErrorMessage = "Something went wrong. Ryan's probably having a meltdown."
            });
        }
    }

    /// <summary>
    /// Get WUPHF message history
    /// </summary>
    [HttpGet("history")]
    public async Task<ActionResult<List<WuphfMessage>>> GetHistory(
        [FromQuery] string? userName = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        try
        {
            var messages = await _wuphfService.GetWuphfHistoryAsync(userName, fromDate, toDate);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving WUPHF history");
            return StatusCode(500, "Error retrieving history. Ryan forgot to backup the data.");
        }
    }

    /// <summary>
    /// Get a specific WUPHF message by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<WuphfMessage>> GetWuphf(Guid id)
    {
        try
        {
            var message = await _wuphfService.GetWuphfByIdAsync(id);
            if (message == null)
            {
                return NotFound("WUPHF not found. Maybe it got lost in cyberspace?");
            }

            return Ok(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving WUPHF {MessageId}", id);
            return StatusCode(500, "Error retrieving WUPHF. Ryan's database is acting up.");
        }
    }

    /// <summary>
    /// Get Ryan Howard's motivational quotes about WUPHF
    /// </summary>
    [HttpGet("quotes")]
    public ActionResult<object> GetRyanQuotes()
    {
        return Ok(new
        {
            MainSlogan = WuphfConstants.Quotes.MainSlogan,
            Pitch = WuphfConstants.Quotes.RyanPitch,
            BusinessPlan = WuphfConstants.Quotes.BusinessPlan,
            PrinterFeature = WuphfConstants.Quotes.PrinterFeature,
            FailureQuote = WuphfConstants.Quotes.FailureQuote,
            BonusQuote = "I'm going to own the biggest social networking site in the world. Or I'll just watch 'The Office' reruns."
        });
    }

    /// <summary>
    /// Health check endpoint with Ryan's enthusiasm
    /// </summary>
    [HttpGet("health")]
    public ActionResult<object> HealthCheck()
    {
        return Ok(new
        {
            Status = "WUPHF is alive and kicking!",
            Message = "All systems operational. Ryan is pleased.",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0-ryan-approved"
        });
    }
}
