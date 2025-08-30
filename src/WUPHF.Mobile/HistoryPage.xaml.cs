using WUPHF.Shared.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace WUPHF.Mobile;

public partial class HistoryPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private const string API_BASE_URL = "https://localhost:7000/api/wuphf";

    public ObservableCollection<WuphfMessageViewModel> Messages { get; set; } = new();

    public HistoryPage()
    {
        InitializeComponent();
        _httpClient = new HttpClient();
        BindingContext = this;

        // Load history when page appears
        _ = LoadHistory();
    }

    private async void OnRefreshClicked(object? sender, EventArgs e)
    {
        await LoadHistory();
    }

    private async void OnSendFirstWuphfClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//SendWuphf");
    }

    private async Task LoadHistory()
    {
        try
        {
            // Show loading
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            RefreshButton.IsEnabled = false;

            // Hide all states
            EmptyStateView.IsVisible = false;
            ErrorStateView.IsVisible = false;
            MessagesCollectionView.IsVisible = false;

            var response = await _httpClient.GetAsync($"{API_BASE_URL}/history");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var messages = JsonSerializer.Deserialize<List<WuphfMessage>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<WuphfMessage>();

                // Clear and populate the collection
                Messages.Clear();
                foreach (var message in messages.Take(50)) // Limit for mobile performance
                {
                    Messages.Add(new WuphfMessageViewModel(message));
                }

                if (Messages.Any())
                {
                    MessagesCollectionView.IsVisible = true;
                }
                else
                {
                    EmptyStateView.IsVisible = true;
                }
            }
            else
            {
                ShowError("Failed to load WUPHF history. Ryan's servers might be having issues!");
            }
        }
        catch (Exception ex)
        {
            ShowError($"Error loading history: {ex.Message}");
        }
        finally
        {
            // Hide loading
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            RefreshButton.IsEnabled = true;
        }
    }

    private void ShowError(string message)
    {
        ErrorLabel.Text = message;
        ErrorStateView.IsVisible = true;
        EmptyStateView.IsVisible = false;
        MessagesCollectionView.IsVisible = false;
    }
}

// ViewModel class for easier data binding in mobile
public class WuphfMessageViewModel
{
    public WuphfMessageViewModel(WuphfMessage message)
    {
        Id = message.Id;
        FromUser = message.FromUser;
        ToUser = message.ToUser;
        Message = message.Message.Length > 100 ? message.Message.Substring(0, 100) + "..." : message.Message;
        SentAt = message.SentAt;
        Status = message.Status.ToString();
        Channels = message.Channels;

        StatusColor = message.Status switch
        {
            WuphfStatus.Delivered => Colors.Green,
            WuphfStatus.Failed => Colors.Red,
            WuphfStatus.PartiallyDelivered => Colors.Orange,
            WuphfStatus.Sending => Colors.Blue,
            WuphfStatus.Pending => Colors.Gray,
            _ => Colors.Gray
        };
    }

    public Guid Id { get; set; }
    public string FromUser { get; set; }
    public string ToUser { get; set; }
    public string Message { get; set; }
    public DateTime SentAt { get; set; }
    public string Status { get; set; }
    public List<WuphfChannel> Channels { get; set; }
    public Color StatusColor { get; set; }
}
