using LabXand.Infrastructure.Data.EF;
using LabXand.Logging.Core;
using System.Data.Entity;

namespace LabXand.Logging.Data
{
    public class LogDataContext : DbContextBase, ILogDbContext
    {
        public LogDataContext(ILogContext logContext) : base("LaXandLogger", logContext)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<LogDataContext>());
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ApiLogEntryMap());
            modelBuilder.Configurations.Add(new ExceptionInformationMap());
            modelBuilder.Configurations.Add(new ServiceLogEntryMap());
            modelBuilder.Configurations.Add(new DetailsLogEntryMap());
            base.OnModelCreating(modelBuilder);
        }

        void ILogDbContext.Commit()
        {
            base.Commit();
        }

        public IDbSet<ApiLogEntry> ApiLogEntries { get; set; }

        public IDbSet<ExceptionInformation> ExceptionEntries { get; set; }        
    }
}
