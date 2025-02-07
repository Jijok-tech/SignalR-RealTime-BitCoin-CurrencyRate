using RealTimeDashboard.Hub;

var builder = WebApplication.CreateBuilder(args);

// Add necessary services
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure middleware
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}");

    endpoints.MapHub<MarketDataHub>("/marketDataHub");
    endpoints.MapHub<CurrencyHub>("/currencyHub");
});

app.Run();
