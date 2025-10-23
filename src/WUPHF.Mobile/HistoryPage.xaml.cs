using WUPHF.Shared.Models;
using WUPHF.Shared.ViewModels;
using WUPHF.Shared.Helpers;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace WUPHF.Mobile;

public partial class HistoryPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private const string API_BASE_URL = "https://localhost:7000/api/wuphf";

    public ObservableCollection<MobileWuphfMessageViewModel> Messages { get; set; } = new();

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
                    var viewModel = new WuphfMessageViewModel(message);
                    // Convert hex color to MAUI Color for binding
                    var statusColor = Color.FromArgb(viewModel.StatusColorHex);
                    Messages.Add(new MobileWuphfMessageViewModel(viewModel, statusColor));
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

/// <summary>
/// Mobile-specific view model that extends the shared view model with MAUI Color support
/// </summary>
public class MobileWuphfMessageViewModel : WuphfMessageViewModel
{
    public MobileWuphfMessageViewModel(WuphfMessageViewModel baseViewModel, Color statusColor) 
        : base(baseViewModel.Id, baseViewModel.FromUser, baseViewModel.ToUser, 
               baseViewModel.FullMessage, baseViewModel.SentAt, baseViewModel.StatusEnum,
               baseViewModel.Channels, baseViewModel.DeliveryResults)
    {
        StatusColor = statusColor;
    }

    public Color StatusColor { get; set; }
}
