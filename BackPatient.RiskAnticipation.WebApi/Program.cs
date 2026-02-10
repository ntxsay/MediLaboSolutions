using BackPatient.RiskAnticipation.WebApi.Handlers;
using BackPatient.RiskAnticipation.WebApi.Services.Implementations;
using BackPatient.RiskAnticipation.WebApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
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

builder.Services.AddScoped<IRiskAnticipationServices, RiskAnticipationServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();