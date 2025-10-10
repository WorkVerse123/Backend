using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Application.Interfaces.IServicies;
using Application.Services;
using Application.Servicies;
using Autofac;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalSystem.Application.Services;
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
            builder.RegisterType<EmployerProfileService>().As<IEmployerProfileService>().InstancePerLifetimeScope();
            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
            builder.RegisterType<JWTService>().As<IJWTService>().InstancePerLifetimeScope();
            builder.RegisterType<ReportService>().As<IReportService>().InstancePerLifetimeScope();
            builder.RegisterType<FeedbackService>().As<IFeedbackService>().InstancePerLifetimeScope();
            builder.RegisterType<BlogService>().As<IBlogService>().InstancePerLifetimeScope();
            builder.RegisterType<StaffProfileService>().As<IStaffProfileService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<AIService>().As<IAIService>().InstancePerLifetimeScope();
			builder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();
            builder.RegisterType<EmployerTypeService>().As<IEmployerTypeSevice>().InstancePerLifetimeScope();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentService>().As<IPaymentService>().InstancePerLifetimeScope();
            builder.RegisterType<PayOSService>().As<IThirdPaymentService>().InstancePerLifetimeScope();
            builder.RegisterType<UserSubscriptionService>().As<IUserSubscriptionService>().InstancePerLifetimeScope();
            builder.RegisterType<AdminService>().As<IAdminService>().InstancePerLifetimeScope();
        }
	}
}
