using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WorkVerseDBContext>
    {
        public WorkVerseDBContext CreateDbContext(string[] args)
        {
            // Tìm tới thư mục gốc của solution
            var basePath = Directory.GetCurrentDirectory();

            // Nếu đang chạy từ Infrastructure thì dịch lên trên để tìm API
            var apiPath = Path.Combine(basePath, "..", "WorkVerseAPI");

            // Tìm appsettings.json trong API
            var configPath = File.Exists(Path.Combine(basePath, "appsettings.json"))
                ? basePath
                : apiPath;

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(configPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<WorkVerseDBContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new WorkVerseDBContext(optionsBuilder.Options);

        }
    }
}
