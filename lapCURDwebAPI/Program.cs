using lapCURDwebAPI.Data;
using lapCURDwebAPI.Repository;
using lapCURDwebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // Repository registration
        builder.Services.AddScoped<repositoryUser>();
        builder.Services.AddScoped<repositoryItem>();
        builder.Services.AddScoped<repositoryUserItem>();

        // Add DbContext
        builder.Services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // Add services to the container
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add JWT Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    ValidIssuer = "yourIssuer",
                    ValidAudience = "yourAudience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                };
            });

        // Register TokenService in DI container
        builder.Services.AddSingleton<TokenService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Use CORS middleware
        app.UseCors("AllowAllOrigins");

        // Configure HTTPS Redirection (if required)
        app.UseHttpsRedirection();

        // Authentication and Authorization middleware
        app.UseAuthentication();
        app.UseAuthorization();

        // Map controllers
        app.MapControllers();

        // Apply pending migrations
        using (var scope = app.Services.CreateScope())
        {
            var _Db = scope.ServiceProvider.GetRequiredService<DataContext>();
            if (_Db != null)
            {
                if (_Db.Database.GetPendingMigrations().Any())
                {
                    _Db.Database.Migrate();
                }
            }
        }

        app.Run();
    }
}
