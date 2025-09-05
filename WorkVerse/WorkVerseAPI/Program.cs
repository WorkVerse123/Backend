
using Autofac.Extensions.DependencyInjection;
using Autofac;
using WorkVerseAPI.Configurations;
using Application.Mappers;

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


            //Add mapper as the DI (It will seek all asembly have in mapper)
            builder.Services.AddAutoMapper(typeof(EmployeeProfileMapper));
            builder.Services.AddAutoMapper(typeof(BusyTimeProfile));
            builder.Services.AddAutoMapper(typeof(BookmarkProfile));
            builder.Services.AddAutoMapper(typeof(ApplicationProfile));
            builder.Services.AddAutoMapper(typeof(JobProfile));
            builder.Services.AddAutoMapper(typeof(JobCategoryProfile));





            // Add services to the container.
            builder.Services.AddControllers();


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
