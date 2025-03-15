using DBetter.Api;
using DBetter.Application;
using DBetter.Infrastructure;
using DBetter.Infrastructure.Monitoring;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.AddMonitoring();
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);

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
    options.AddPolicy("dev", policyBuilder =>
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

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("dev");   
}

app.UseAuthentication();
app.UseAuthorization();

app.AddStationEndpoints();
app.AddUserEndpoints();
app.AddAuthenticationEndpoints();
app.AddConnectionEndpoints();

app.UseMonitoring();
app.Run();