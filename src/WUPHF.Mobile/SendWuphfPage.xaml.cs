using WUPHF.Shared.Models;
using WUPHF.Shared.DTOs;
using System.Text.Json;
using System.Text;

namespace WUPHF.Mobile;

public partial class SendWuphfPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private const string API_BASE_URL = "https://localhost:7000/api/wuphf";

    public SendWuphfPage()
    {
        InitializeComponent();
        _httpClient = new HttpClient();
        UpdateCharCount();
    }

    private void OnMessageTextChanged(object? sender, TextChangedEventArgs e)
    {
        UpdateCharCount();
    }

    private void UpdateCharCount()
    {
        var messageLength = MessageEditor.Text?.Length ?? 0;
        CharCountLabel.Text = $"{messageLength} / 280 characters";

        if (messageLength > 280)
        {
            CharCountLabel.TextColor = Colors.Red;
        }
        else if (messageLength > 240)
        {
            CharCountLabel.TextColor = Colors.Orange;
        }
        else
        {
            CharCountLabel.TextColor = Colors.Gray;
        }
    }

    private async void OnSendClicked(object? sender, EventArgs e)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(FromUserEntry.Text))
        {
            await DisplayAlert("Error", "Please enter your name!", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(ToUserEntry.Text))
        {
            await DisplayAlert("Error", "Please enter recipient's name!", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(MessageEditor.Text))
        {
            await DisplayAlert("Error", "Please enter a message!", "OK");
            return;
        }

        if (MessageEditor.Text.Length > 280)
        {
            await DisplayAlert("Error", "Message is too long! Maximum 280 characters.", "OK");
            return;
        }

        var selectedChannels = GetSelectedChannels();
        if (!selectedChannels.Any())
        {
            await DisplayAlert("Error", "Please select at least one channel!", "OK");
            return;
        }

        await SendWuphfMessage(selectedChannels);
    }

    private List<WuphfChannel> GetSelectedChannels()
    {
        var channels = new List<WuphfChannel>();

        if (FacebookCheck.IsChecked) channels.Add(WuphfChannel.Facebook);
        if (TwitterCheck.IsChecked) channels.Add(WuphfChannel.Twitter);
        if (SMSCheck.IsChecked) channels.Add(WuphfChannel.SMS);
        if (EmailCheck.IsChecked) channels.Add(WuphfChannel.Email);
        if (ChatCheck.IsChecked) channels.Add(WuphfChannel.Chat);
        if (PrinterCheck.IsChecked) channels.Add(WuphfChannel.Printer);
        if (LinkedInCheck.IsChecked) channels.Add(WuphfChannel.LinkedIn);
        if (InstagramCheck.IsChecked) channels.Add(WuphfChannel.Instagram);
        if (SlackCheck.IsChecked) channels.Add(WuphfChannel.Slack);

        return channels;
    }

    private async Task SendWuphfMessage(List<WuphfChannel> channels)
    {
        try
        {
            // Show loading
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            SendButton.IsEnabled = false;
            StatusLabel.IsVisible = false;

            var request = new SendWuphfRequest
            {
                FromUser = FromUserEntry.Text.Trim(),
                ToUser = ToUserEntry.Text.Trim(),
                Message = MessageEditor.Text.Trim(),
                Channels = channels,
                PrintWuphf = PrinterCheck.IsChecked
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{API_BASE_URL}/send", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<SendWuphfResponse>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null)
                {
                    StatusLabel.Text = $"🎉 WUPHF sent! {result.ChannelsSuccessful}/{result.ChannelsAttempted} channels successful!";
                    StatusLabel.TextColor = Colors.Green;
                    StatusLabel.IsVisible = true;

                    await DisplayAlert(
                        "WUPHF Sent!",
                        $"Your WUPHF was sent to {result.ChannelsSuccessful} out of {result.ChannelsAttempted} channels!\n\n" +
                        $"Ryan's Reaction: \"{result.RyanReaction}\"",
                        "Awesome!");

                    // Clear the form
                    ClearForm();
                }
            }
            else
            {
                StatusLabel.Text = "❌ Failed to send WUPHF. Ryan's servers might be down!";
                StatusLabel.TextColor = Colors.Red;
                StatusLabel.IsVisible = true;

                await DisplayAlert("Error", "Failed to send WUPHF. Please try again later!", "OK");
            }
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"❌ Error: {ex.Message}";
            StatusLabel.TextColor = Colors.Red;
            StatusLabel.IsVisible = true;

            await DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
        }
        finally
        {
            // Hide loading
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            SendButton.IsEnabled = true;
        }
    }

    private void ClearForm()
    {
        FromUserEntry.Text = string.Empty;
        ToUserEntry.Text = string.Empty;
        MessageEditor.Text = string.Empty;

        // Reset checkboxes to defaults
        FacebookCheck.IsChecked = true;
        TwitterCheck.IsChecked = true;
        EmailCheck.IsChecked = true;
        PrinterCheck.IsChecked = true;

        SMSCheck.IsChecked = false;
        ChatCheck.IsChecked = false;
        LinkedInCheck.IsChecked = false;
        InstagramCheck.IsChecked = false;
        SlackCheck.IsChecked = false;

        UpdateCharCount();
    }
}
