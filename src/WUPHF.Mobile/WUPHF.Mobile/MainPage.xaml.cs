namespace WUPHF.Mobile;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnSendWuphfClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//SendWuphf");
    }

    private async void OnHistoryClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//History");
    }

    private async void OnAboutClicked(object? sender, EventArgs e)
    {
        await DisplayAlert(
            "💡 About Ryan Howard",
            "Ryan Howard is the visionary behind WUPHF - the ultimate social networking experience!\n\n" +
            "\"Facebook, Twitter, SMS, Email, Chat, and even prints to the nearest printer!\" - Ryan Howard\n\n" +
            "From temp at Dunder Mifflin to tech entrepreneur, Ryan's dream lives on in this app!",
            "WUPHF Yeah!");
    }
}
