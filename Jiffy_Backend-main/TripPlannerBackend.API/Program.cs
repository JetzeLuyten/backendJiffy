using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.OpenApi.Models;
using JiffyBackend.DAL;
using JiffyBackend.DAL.Initializer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));


// Configure database context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<JiffyDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddSwaggerGen();

// Configure authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Domain"];
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "role"
    };
});


// Configure authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ServiceTypeDeleteAccess", policy =>
                            policy.RequireClaim("permissions", "delete:service"));
    options.AddPolicy("ServiceTypeGetAccess", policy =>
                      policy.RequireClaim("permissions", "getall:services"));
});

// Configure controllers and JSON options
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true; // Optional: for pretty-printing
});



// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://jiffyservices.netlify.app/") // Use the correct URL here
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Use CORS
app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use HTTPS redirection
app.UseHttpsRedirection();

// Use authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var serviceContext = scope.ServiceProvider.GetRequiredService<JiffyDbContext>();
    serviceContext.Database.EnsureDeleted();
    serviceContext.Database.EnsureCreated();
    DBInitializer.Initialize(serviceContext);
}

app.Run();
