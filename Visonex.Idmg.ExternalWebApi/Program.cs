using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using System.Net;
using Visonex.Idmg.ExternalWebApi.Common;
using Visonex.Idmg.ExternalWebApi.Filters;
using Visonex.Idmg.ExternalWebApi.Middlewares;
using Visonex.Idmg.ExternalWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication("BasicAuthentication")
        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

// Register the Swagger generator
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Idmg Ext API", Version = "v1" });

    // Define the security scheme for basic authentication
    c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        Description = "Basic Authorization header using the Bearer scheme."
    });

    // Apply security requirements globally for all endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Basic"
                    }
                },
                new string[] {}
            }
        });
    c.OperationFilter<HeaderOperationFilter>();
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
builder.Services.AddTransient<LoginDelegatingHandler>();

builder.Services.AddScoped<IUserValidationService, UserValidationService>();
builder.Services.AddScoped<IServiceCredentialsRepository, ServiceCredentialsRepository>();
builder.Services.AddHttpClient<IGraphApiService, GraphApiService>()
    .AddHttpMessageHandler<LoginDelegatingHandler>();

builder.Services.AddHttpClient(HttpClientName.ServiceCredentials, c =>
{
    c.BaseAddress = new Uri(builder.Configuration["AuthorizationWebApi:ServiceCredentialUri"]!);

}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler()
    {
        UseDefaultCredentials = true
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddMvc(config =>
{
    config.Filters.Add(new HeaderValidationFilterAttribute());
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
