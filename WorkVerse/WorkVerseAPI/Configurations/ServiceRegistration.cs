using Autofac;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace WorkVerseAPI.Configurations
{
    public class ServiceRegistration : Module
    {
        private readonly IConfiguration _configuration;

        public ServiceRegistration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Register DbContext
            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<WorkVerseDBContext>();
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
                return new WorkVerseDBContext(optionsBuilder.Options);
            }).InstancePerLifetimeScope();

            // Register service

            //builder.RegisterType<JWTService>().As<IJWTService>().InstancePerLifetimeScope();
            //builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
            //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();


        }
    }
}
