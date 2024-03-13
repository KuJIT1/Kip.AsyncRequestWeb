using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Kip.AsyncReport.Model;

namespace Kip.AsyncReport.Infrastructure
{
    public class ReportTaskContext: DbContext
    {
        public ReportTaskContext (DbContextOptions<ReportTaskContext> options) : base (options)
        {
        }

        public DbSet<ReportTask> ReportTasks { get; set; }
    }

    public class ReportTaskContextDesignFactory : IDesignTimeDbContextFactory<ReportTaskContext>
    {
        public ReportTaskContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ReportTaskContext>()
                .UseSqlServer("Server=.;Initial Catalog=master;Integrated Security=true");

            return new ReportTaskContext(optionsBuilder.Options);
        }
    }
}
