namespace WUPHF.Shared.Constants;

/// <summary>
/// Ryan Howard's favorite WUPHF quotes and constants
/// </summary>
public static class WuphfConstants
{
    public static class Quotes
    {
        public const string MainSlogan = "WUPHF - Where technology meets human connection!";
        public const string RyanPitch = "Imagine you're in an accident and you need to contact someone. Instead of being like 'Help me,' you'd be like 'WUPHF me!'";
        public const string BusinessPlan = "Facebook, Twitter, Google Plus, Instagram, all your other social media sites, email, text messaging, and the home phone all in one.";
        public const string PrinterFeature = "It even prints out a WUPHF on the nearest printer!";
        public const string FailureQuote = "I thought I was going to be rich. I mean, I still might be.";
    }

    public static class Limits
    {
        public const int MaxMessageLength = 280; // Twitter-like limit
        public const int MaxChannelsPerMessage = 9;
        public const int MaxMessagesPerUserPerDay = 100;
        public const int PrinterQueueLimit = 50;
    }

    public static class Endpoints
    {
        public const string SendWuphf = "/api/wuphf/send";
        public const string GetHistory = "/api/wuphf/history";
        public const string GetUser = "/api/users/{id}";
        public const string UpdateChannels = "/api/users/{id}/channels";
    }
}
