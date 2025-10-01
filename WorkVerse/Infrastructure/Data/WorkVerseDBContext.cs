using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public partial class WorkVerseDBContext : DbContext
    {
        public WorkVerseDBContext(DbContextOptions<WorkVerseDBContext> options) : base(options) { }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<EmployerType> EmployerTypes { get; set; }
        public virtual DbSet<EmployerProfile> EmployerProfiles { get; set; }
        public virtual DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public virtual DbSet<BusyTime> BusyTimes { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<JobCategory> JobCategories { get; set; }
        public virtual DbSet<JobCategoryMapping> JobCategoryMappings { get; set; }
        public virtual DbSet<Domain.Entities.Application> Applications { get; set; }
        public virtual DbSet<Bookmark> Bookmarks { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<SystemInformation> SystemInformations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.HasKey(r => r.RoleId);

                entity.Property(r => r.RoleName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasIndex(r => r.RoleName).IsUnique();

                // CHECK (role_name IN ('Admin','Staff','Employer','Employee'))
                entity.HasCheckConstraint("CK_Role_Name", "RoleName IN ('Admin','Staff','Employer','Employee')");
            });

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasKey(u => u.UserId);

                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.PhoneNumber).IsUnique();

                entity.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Status).IsRequired().HasMaxLength(20);

                entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasOne(u => u.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.RoleId);


                entity.HasCheckConstraint("CK_User_Status", "Status IN ('active','suspended','pending')");
            });

            // EmployeeProfile
            modelBuilder.Entity<EmployeeProfile>(entity =>
            {
                entity.ToTable("EmployeeProfile");

                entity.HasKey(e => e.EmployeeId);

                entity.HasIndex(e => e.UserId).IsUnique();

                entity.Property(e => e.FullName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Gender).HasMaxLength(20);
                entity.Property(e => e.Mode).HasMaxLength(50);

                entity.HasOne(e => e.User)
                      .WithOne(u => u.EmployeeProfile)
                      .HasForeignKey<EmployeeProfile>(e => e.UserId);

                entity.HasCheckConstraint("CK_EmployeeProfile_Gender", "Gender IN ('Male','Female','Other')");
                entity.HasCheckConstraint("CK_EmployeeProfile_Mode", "Mode IN ('private','public')");
            });

            // BusyTime
            modelBuilder.Entity<BusyTime>(entity =>
            {
                entity.ToTable("BusyTime");

                entity.HasKey(e => e.BusyTimeId);

                entity.Property(e => e.DayOfWeek)
                      .IsRequired();

                entity.Property(e => e.StartTime)
                      .IsRequired();

                entity.Property(e => e.EndTime)
                      .IsRequired();

                entity.Property(e => e.Date)
                      .HasColumnType("date")
                      .IsRequired();

                entity.HasOne(e => e.Employee)
                      .WithMany(ep => ep.BusyTimes)   
                      .HasForeignKey(e => e.EmployeeId)
                      .HasConstraintName("FK_BusyTime_Employee");

                // Constraint check day_of_week (0–6)
                entity.HasCheckConstraint("CK_BusyTime_DayOfWeek", "[DayOfWeek] BETWEEN 0 AND 6");

                // Constraint check Start < End
                entity.HasCheckConstraint("CK_BusyTime_TimeRange", "[StartTime] < [EndTime]");
            });
            // EmployerProfile
            modelBuilder.Entity<EmployerProfile>(entity =>
            {
                entity.ToTable("EmployerProfile");

                entity.HasKey(e => e.EmployerId);

                entity.HasIndex(e => e.UserId).IsUnique();
                entity.HasIndex(e => e.ContactEmail).IsUnique();
                entity.HasIndex(e => e.ContactPhone).IsUnique();

                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.ContactEmail).IsRequired().HasMaxLength(255);
                entity.Property(e => e.ContactPhone).IsRequired().HasMaxLength(20);

                entity.HasOne(e => e.User)
                      .WithOne(u => u.EmployerProfile)
                      .HasForeignKey<EmployerProfile>(e => e.UserId);

                entity.HasOne(e => e.EmployerType)
                      .WithMany(t => t.EmployerProfiles)
                      .HasForeignKey(e => e.EmployerTypeId);
            });

            // Job
            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("Job");

                entity.HasKey(j => j.JobId);

                entity.Property(j => j.Title).IsRequired().HasMaxLength(255);
                entity.Property(j => j.Location).IsRequired().HasMaxLength(255);
                entity.Property(j => j.SalaryMin).HasColumnType("decimal(18,2)");
                entity.Property(j => j.SalaryMax).HasColumnType("decimal(18,2)");
                entity.Property(j => j.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(j => j.Status).HasMaxLength(20);
                // default job thường (không ưu tiên)
                entity.Property(j => j.IsPriority).HasDefaultValue(false);
                entity.HasOne(j => j.Employer)
                      .WithMany(e => e.Jobs)
                      .HasForeignKey(j => j.EmployerId);

                entity.HasCheckConstraint("CK_Job_SalaryRange", "SalaryMax >= SalaryMin");
            });

            //  SHIFT 
            modelBuilder.Entity<Shift>(entity =>
            {
                entity.ToTable("Shift");
                entity.HasKey(e => e.ShiftId);

                entity.Property(e => e.DayOfWeek)
                      .IsRequired();

                entity.Property(e => e.StartTime)
                      .IsRequired();

                entity.Property(e => e.EndTime)
                      .IsRequired();

                entity.HasOne(e => e.Job)
                      .WithMany(j => j.Shifts)
                      .HasForeignKey(e => e.JobId);
                    

                entity.HasCheckConstraint("CK_Shift_DayOfWeek", "DayOfWeek BETWEEN 0 AND 6");
            });

            //  JOB CATEGORY 
            modelBuilder.Entity<JobCategory>(entity =>
            {
                entity.ToTable("JobCategory");
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(e => e.CategoryName)
                      .IsUnique();
            });

            //  JOB CATEGORY MAPPING 
            modelBuilder.Entity<JobCategoryMapping>(entity =>
            {
                entity.ToTable("JobCategoryMapping");
                entity.HasKey(e => new { e.JobId, e.CategoryId });

                entity.HasOne(e => e.Job)
                      .WithMany(j => j.JobCategoryMappings)
                      .HasForeignKey(e => e.JobId);

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.JobCategoryMappings)
                      .HasForeignKey(e => e.CategoryId);
            });

            //  APPLICATION 
            modelBuilder.Entity<Domain.Entities.Application>(entity =>
            {
                entity.ToTable("Application");
                entity.HasKey(e => e.ApplicationId);

                entity.Property(e => e.Status)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(e => e.AppliedAt)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Job)
                      .WithMany(j => j.Applications)
                      .HasForeignKey(e => e.JobId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Employee)
                      .WithMany(ep => ep.Applications)
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);

            });

            // BOOKMARK 
            modelBuilder.Entity<Bookmark>(entity =>
            {
                entity.ToTable("Bookmark");
                entity.HasKey(e => e.BookmarkId);

                entity.Property(e => e.SavedAt)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Employee)
                      .WithMany(ep => ep.Bookmarks)
                      .HasForeignKey(e => e.EmployeeId);

                entity.HasOne(e => e.Job)
                      .WithMany(j => j.Bookmarks)
                      .HasForeignKey(e => e.JobId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //  REVIEW 
            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review");
                entity.HasKey(e => e.ReviewId);

                entity.Property(e => e.Rating)
                      .IsRequired();

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Employee)
                      .WithMany(ep => ep.Reviews)
                      .HasForeignKey(e => e.EmployeeId);

                entity.HasOne(e => e.Job)
                      .WithMany(j => j.Reviews)
                      .HasForeignKey(e => e.JobId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasCheckConstraint("CK_Review_Rating", "Rating BETWEEN 1 AND 5");
            });

            //  MESSAGE 
            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");
                entity.HasKey(e => e.MessageId);

                entity.Property(e => e.Content)
                      .IsRequired();

                entity.Property(e => e.SentAt)
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsRead)
                      .HasDefaultValue(false);

                entity.HasOne(e => e.Sender)
                      .WithMany(u => u.SentMessages)
                      .HasForeignKey(e => e.SenderId)
                       .OnDelete(DeleteBehavior.Restrict);



                entity.HasOne(e => e.Receiver)
                      .WithMany(u => u.ReceivedMessages)
                      .HasForeignKey(e => e.ReceiverId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // REPORT
            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");
                entity.HasKey(e => e.ReportId);

                entity.Property(e => e.TargetType)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(e => e.Status)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(e => e.ReportedAt)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Reports)
                      .HasForeignKey(e => e.UserId);

            });

            //  FEEDBACK
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");
                entity.HasKey(e => e.FeedbackId);

                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Feedbacks)
                      .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Handler)
                      .WithMany()
                      .HasForeignKey(e => e.HandledBy);
            });

            //  SUBSCRIPTION PLAN 
            modelBuilder.Entity<SubscriptionPlan>(entity =>
            {
                entity.ToTable("SubscriptionPlan");
                entity.HasKey(e => e.PlanId);

                entity.Property(e => e.PlanName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.DurationDays).IsRequired();

                entity.HasCheckConstraint("CK_SubscriptionPlan_Price", "Price >= 0");
                entity.HasCheckConstraint("CK_SubscriptionPlan_Duration", "DurationDays > 0");
            });

            //  PAYMENT 
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");
                entity.HasKey(e => e.PaymentId);

                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PaymentDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Status).IsRequired();

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Payments)
                      .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Plan)
                      .WithMany(p => p.Payments)
                      .HasForeignKey(e => e.PlanId);

                entity.HasCheckConstraint("CK_Payment_Amount", "Amount >= 0");

            });

            // USER SUBSCRIPTION 
            modelBuilder.Entity<UserSubscription>(entity =>
            {
                entity.ToTable("UserSubscription");
                entity.HasKey(e => e.UserSubscriptionId);

                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(e => e.User)
                       .WithMany(u => u.Subscriptions)
                         .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Plan)
                      .WithMany(p => p.UserSubscriptions)
                      .HasForeignKey(e => e.PlanId);
            });

            // NOTIFICATION 
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");
                entity.HasKey(e => e.NotificationId);

                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.IsRead).HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(e => e.UserId);
            });

            //  AUDIT LOG 
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("AuditLog");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.Action).IsRequired();
                entity.Property(e => e.TargetTable).IsRequired();
                entity.Property(e => e.Timestamp).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Actor)
                      .WithMany(u => u.AuditLogs)
                      .HasForeignKey(e => e.ActorId);
            });

            //  BLOG 
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("Blog");
                entity.HasKey(e => e.BlogId);

                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Slug).IsRequired();
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.ImageUrl).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.Slug).IsUnique();

                entity.HasOne(e => e.Author)
                      .WithMany(u => u.Blogs)
                      .HasForeignKey(e => e.AuthorId);
            });

            //  SYSTEM INFORMATION 
            modelBuilder.Entity<SystemInformation>(entity =>
            {
                entity.ToTable("SystemInformation");
                entity.HasKey(e => e.InfoId);

                entity.Property(e => e.PageKey).IsRequired();
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Language).IsRequired();
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.PageKey).IsUnique();
            });

            modelBuilder.Entity<StaffProfile>(entity =>
            {
                entity.ToTable("StaffProfile");

                entity.HasKey(e => e.StaffId);

                entity.Property(e => e.FullName)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.Gender)
                      .HasMaxLength(20);

                entity.Property(e => e.Address)
                      .HasMaxLength(255);

                entity.Property(e => e.IdentityCard)
                      .HasMaxLength(20);

                entity.Property(e => e.StartDate)
                      .IsRequired();

                // Quan hệ 1-1: StaffProfile <-> User
                entity.HasOne(e => e.User)
                      .WithOne(u => u.StaffProfile)   // trong User class phải có navigation
                      .HasForeignKey<StaffProfile>(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
