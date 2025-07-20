using Microsoft.Extensions.Options;
using CELINEwebsite.Models;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using CELINEwebsite.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Configure supported cultures and default culture
var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("ar")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en"); // English as default
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    // Add culture provider to detect from cookie
    options.AddInitialRequestCultureProvider(new CookieRequestCultureProvider
    {
        CookieName = CookieRequestCultureProvider.DefaultCookieName,
        Options = options
    });
});




// √÷› Â–« ›Ì Program.cs ﬁ»· »‰«¡ «· ÿ»Ìﬁ (var app = builder.Build();)
// «·ÿ—Ìﬁ… «·’ÕÌÕ… · ”ÃÌ· EmailSettings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
//  ”ÃÌ· EmailService
builder.Services.AddTransient<EmailService>();

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Apply the localization with the default culture
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    ApplyCurrentCultureToResponseHeaders = true
};

app.UseRequestLocalization(localizationOptions);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();