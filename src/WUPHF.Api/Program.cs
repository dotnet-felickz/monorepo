using WUPHF.Api.Services;
using WUPHF.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Register WUPHF services
builder.Services.AddScoped<IWuphfService, WuphfService>();
builder.Services.AddScoped<IChannelService, ChannelService>();
builder.Services.AddScoped<IWuphfValidationService, WuphfValidationService>();

// Add CORS for web frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("WuphfCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("WuphfCorsPolicy");
app.MapControllers();

// Add a welcome message at the root
app.MapGet("/", () => new
{
    Message = "Welcome to WUPHF! The ultimate social networking experience!",
    Quote = "Facebook, Twitter, SMS, Email, Chat, and even prints to the nearest printer! - Ryan Howard",
    ApiDocs = "/openapi/v1.json",
    Version = "1.0.0-ryan-approved"
});

app.Run();
