using EPRO.Infrastructure.Data.Models.Common;
using EPRO.Infrastructure.Data.Models.Identity;
using EPRO.Infrastructure.Data.Models.Nomenclatures;
using Microsoft.EntityFrameworkCore;

namespace EPRO.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Identity configuration

            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new ApplicationRoleConfiguration());
            builder.ApplyConfiguration(new ApplicationUserRoleConfiguration());
            builder.ApplyConfiguration(new ApplicationUserClaimConfiguration());
            builder.ApplyConfiguration(new ApplicationUserLoginConfiguration());
            builder.ApplyConfiguration(new ApplicationRoleClaimConfiguration());
            builder.ApplyConfiguration(new ApplicationUserTokenConfiguration());

            #endregion            
        }

       // public DbSet<LogOperation> LogOperation { get; set; }

        #region Common
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<MongoFile> MongoFiles { get; set; }
        public DbSet<Dismissal> Dismissals { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        #endregion       

        #region Nomenclatures
        public DbSet<ActType> ActTypes { get; set; }
        public DbSet<ApealRegion> ApealRegions { get; set; }
        public DbSet<CaseRole> CaseRoles { get; set; }
        public DbSet<CaseType> CaseTypes { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<DismissalType> DismissalTypes { get; set; }
        public DbSet<HearingType> HearingTypes { get; set; }

        #endregion
    }
}
