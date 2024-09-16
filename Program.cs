using chatapplication.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Load connection string from configuration
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");

if (string.IsNullOrEmpty(mongoConnectionString))
{
    throw new ArgumentNullException(nameof(mongoConnectionString), "MongoDB connection string is missing or empty.");
}

// Register MongoDB client with the connection string
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));

// Add SignalR service
builder.Services.AddSignalR();

// Add CORS policy allowing specific origins with ports
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOriginsWithPorts", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200"   // Allow Angular app running on port 4200
                /*"http://localhost:3000" */   // Allow React app running on port 3000
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // Allows sending credentials like cookies
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Enable CORS with the specific origins policy
app.UseCors("AllowSpecificOriginsWithPorts");

app.MapControllers();

// Map the SignalR ChatHub
app.MapHub<ChatHub>("/chathub");

app.Run();