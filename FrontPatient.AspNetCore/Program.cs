using System.Globalization;
using Microsoft.AspNetCore.Localization;
using FrontPatient.AspNetCore.Handlers;
using FrontPatient.AspNetCore.Models.Settings;
using FrontPatient.AspNetCore.Services.Implementations;
using FrontPatient.AspNetCore.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("fr-FR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("fr-FR");

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.


builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
        options.LogoutPath = "/Login/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthTokenHandler>();
builder.Services.AddHttpClient(builder.Configuration["MyHttpClients:GatewayClientName"]!, client =>
    {
        client.BaseAddress = new Uri("http://ocelotWebapi:8084/api/");
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
        };
    }).AddHttpMessageHandler<AuthTokenHandler>();

//Ajout du client http pour appeler le controller AuthController du projet OcelotGatewayApi permettant la gestion de l'authentification de l'utilisateur
builder.Services.AddHttpClient(builder.Configuration["MyHttpClients:AuthorizedClientName"]!, client =>
{
    client.BaseAddress = new Uri("http://ocelotWebapi:8084/api/auth/");
});
    
//Force la culture française dans l'application 
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

builder.Services.AddScoped<ILoginServices, LoginServices>();
builder.Services.AddScoped<IGenreServices, GenreServices>();
builder.Services.AddScoped<IPatientServices, PatientServices>();
builder.Services.AddScoped<IPatientNoteServices, PatientNoteServices>();
builder.Services.AddScoped<IPatientNoteSeedServices, PatientNoteSeedServices>();
builder.Services.AddScoped<IPatientRiskReportServices, PatientRiskReportServices>();

builder.Services.Configure<SeederSettings>(
    builder.Configuration.GetSection("Seeders"));

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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages();
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Patient}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();