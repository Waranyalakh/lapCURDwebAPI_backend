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

        
        
        //repository
        builder.Services.AddScoped<repositoryUser>();
        builder.Services.AddScoped<repositoryItem>();
        builder.Services.AddScoped<repositoryUserItem>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        // Add services to the container.

        //JWT
      // เพิ่มการตั้งค่า Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});


        // ลงทะเบียน TokenService ใน DI container
        builder.Services.AddSingleton<TokenService>();


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Configure the HTTP request pipeline.
        app.UseAuthentication();
        app.UseAuthorization();

        // Use CORS middleware
        app.UseCors("AllowAllOrigins");




        app.UseHttpsRedirection();//เปลี่ยนเส้นทาง ของตัว http ไป https ถ้า้

        app.UseAuthorization(); // ตรวจสอบสิธ ที่มีอยู่

        app.MapControllers(); //สร้าง endpoint  อัติโนมัตจาก route ที่มีอยู่
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
