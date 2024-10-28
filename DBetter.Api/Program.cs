using DBetter.Api;
using DBetter.Application;
using DBetter.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri("https://www.bahn.de/web/api/") });
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DBetter.Contracts.xml"));
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DBetter.Api.xml"));
    
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
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

app.AddStationEndpoints();
app.AddUserEndpoints();

app.Run();