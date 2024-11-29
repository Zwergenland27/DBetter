using System.Net;
using DBetter.Api;
using DBetter.Application;
using DBetter.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri("https://www.bahn.de/web/api/") });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });
    
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DBetter.Contracts.xml"));
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DBetter.Api.xml"));
    
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200")
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

        var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.Add("traceId", activity?.TraceId);
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.AddStationEndpoints();
app.AddUserEndpoints();
app.AddAuthenticationEndpoints();

app.Run();