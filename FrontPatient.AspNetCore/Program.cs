using System.Globalization;
using FrontPatient.AspNetCore.Services;
using Microsoft.AspNetCore.Localization;

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("fr-FR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("fr-FR");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient("GatewayClient", client =>
{
    client.BaseAddress = new Uri("http://ocelotwebapi:8084/api/");
});
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("fr-FR")
    };

    options.DefaultRequestCulture = new RequestCulture("fr-FR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddScoped<IGenreServices, GenreServices>();
builder.Services.AddScoped<IPatientServices, PatientServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Patient}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();