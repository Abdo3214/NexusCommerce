
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NexusCommerce.BLL.ServicesExtention;
using NexusCommerce.Common;
using NexusCommerce.DAL.Data.Context;
using NexusCommerce.DAL.Data.Models;
using NexusCommerce.DAL.ServicesExtention;
using Scalar.AspNetCore;
using System.Text;

namespace NexusCommerce.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddDALServices(builder.Configuration);
            builder.Services.AddBLLServices();

            // Bind JWT Settings
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            var jwtSettingsSection = builder.Configuration.GetSection("Jwt");
            var jwtKey = jwtSettingsSection["Key"] ?? "SuperSecretKeyNexusCommerceSuperSecretKey12345!";
            var jwtIssuer = jwtSettingsSection["Issuer"] ?? "NexusCommerceAPI";
            var jwtAudience = jwtSettingsSection["Audience"] ?? "NexusCommerceClient";

            // Configure Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // Configure JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Configure Policy-Based Authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
            });

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Seed Database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    NexusCommerce.DAL.SeedDataProvider.SeedDataProvider.SeedAsync(context, userManager, roleManager).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            var filesPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(filesPath))
            {
                Directory.CreateDirectory(filesPath);
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(filesPath),
                RequestPath = "/Files"
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
