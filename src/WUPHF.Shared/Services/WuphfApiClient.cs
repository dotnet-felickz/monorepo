using WUPHF.Shared.DTOs;
using WUPHF.Shared.Models;
using System.Text.Json;
using System.Text;

namespace WUPHF.Shared.Services;

/// <summary>
/// Client service for interacting with the WUPHF API
/// Centralizes HTTP communication logic for Web and Mobile applications
/// </summary>
public interface IWuphfApiClient
{
    /// <summary>
    /// Send a WUPHF message
    /// </summary>
    Task<SendWuphfResponse?> SendWuphfAsync(SendWuphfRequest request);
    
    /// <summary>
    /// Get WUPHF message history
    /// </summary>
    Task<List<WuphfMessage>?> GetHistoryAsync();
    
    /// <summary>
    /// Get a specific WUPHF message by ID
    /// </summary>
    Task<WuphfMessage?> GetWuphfByIdAsync(Guid id);
}

/// <summary>
/// Implementation of WUPHF API client
/// </summary>
public class WuphfApiClient : IWuphfApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _jsonOptions;

    public WuphfApiClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl.TrimEnd('/');
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<SendWuphfResponse?> SendWuphfAsync(SendWuphfRequest request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/wuphf/send", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<SendWuphfResponse>(responseJson, _jsonOptions);
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<WuphfMessage>?> GetHistoryAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/wuphf/history");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<WuphfMessage>>(json, _jsonOptions);
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<WuphfMessage?> GetWuphfByIdAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/wuphf/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<WuphfMessage>(json, _jsonOptions);
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }
}
