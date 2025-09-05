using Application.Interfaces.IRepositories;
using Application.Interfaces.IServicies;
using Application.Servicies;
using Autofac;
using Infrastructure.Data;
using Infrastructure.Repositories;
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
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<EmployeeProfileServices>().As<IEmployeeProfileServices>().InstancePerLifetimeScope();
            builder.RegisterType<BusyTimeService>().As<IBusyTimeService>().InstancePerLifetimeScope();
            builder.RegisterType<BookmarkService>().As<IBookmarkService>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationService>().As<IApplicationService>().InstancePerLifetimeScope();
            builder.RegisterType<JobService>().As<IJobService>().InstancePerLifetimeScope();
            builder.RegisterType<ReviewService>().As<IReviewService>().InstancePerLifetimeScope();
            builder.RegisterType<JobCategoryService>().As<IJobCategoryService>().InstancePerLifetimeScope();

        }
    }
}
