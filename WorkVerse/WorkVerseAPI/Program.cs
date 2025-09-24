
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


            //Add mapper as the DI (It will seek all asembly have in mapper)
            builder.Services.AddAutoMapper(typeof(EmployeeProfileMapper));
            builder.Services.AddAutoMapper(typeof(BusyTimeProfile));
            builder.Services.AddAutoMapper(typeof(BookmarkProfile));
            builder.Services.AddAutoMapper(typeof(ApplicationProfile));
            builder.Services.AddAutoMapper(typeof(JobProfile));
            builder.Services.AddAutoMapper(typeof(JobCategoryProfile));
            builder.Services.AddAutoMapper(typeof(EmployerProfileMapper));
            builder.Services.AddAutoMapper(typeof(ReportProfile));
            builder.Services.AddAutoMapper(typeof(FeedbackProfile));
            builder.Services.AddAutoMapper(typeof(BlogProfile));


            // Add Authentication
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
            builder.Services.AddAuthorization();


            // Add services to the container.
            builder.Services.AddControllers();

            // API key from appsettings
            var apiKey = builder.Configuration["Google:GeminiApiKey"];

            // Đăng ký KeywordService như 1 singleton
            builder.Services.AddSingleton<AIService>(sp => new AIService(apiKey));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
