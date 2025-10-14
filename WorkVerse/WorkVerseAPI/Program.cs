using Autofac.Extensions.DependencyInjection;
using Autofac;
using WorkVerseAPI.Configurations;
using Application.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Application.Servicies;
using Application.DTOs.Email;
using Infrastructure.Models;
using Microsoft.OpenApi.Models; // 👈 thêm dòng này để hỗ trợ Swagger Bearer

namespace WorkVerseAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Use Autofac as the DI container
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterModule(new ServiceRegistration(builder.Configuration));
            });

            builder.Services.AddDbContext<WorkVerseDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add AutoMapper profiles
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            // Email SMTP
            builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection("EmailSettings"));

            // PayOS config
            builder.Services.Configure<PayOSSettings>(
                builder.Configuration.GetSection("PayOS"));

            // JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("IsPremium", policy =>
                    policy.RequireClaim("IsPremium", "True"));
            });

            // Add controllers
            builder.Services.AddControllers();

            // Gemini AI key
            var apiKey = builder.Configuration["Google:GeminiApiKey"];
            builder.Services.AddSingleton<AIService>(sp => new AIService(apiKey));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowDev", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:5173",
                        "https://workverse-sage.vercel.app",    
                        "https://www.workverse-sage.vercel.app" 
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            // ✅ Swagger với Bearer Token
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WorkVerse API",
                    Version = "v1",
                    Description = "API for WorkVerse application"
                });

                // Thêm cấu hình JWT Bearer
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Nhập token vào đây (format: Bearer {your token})",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            var app = builder.Build();

            //app.UseMiddleware<Application.ExceptionHandler.GlobalExceptionHandlerMiddleware>();

            // Configure middleware pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();              // ✅ Đặt trước Authentication và Authorization

            app.UseCors("AllowDev");

            app.UseAuthentication();       // ✅ Sau UseRouting
            app.UseAuthorization();        // ✅ Sau UseAuthentication

            app.MapControllers();


            app.Run();
        }
    }
}
