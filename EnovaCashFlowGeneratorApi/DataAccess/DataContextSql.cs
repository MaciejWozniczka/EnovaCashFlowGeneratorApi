using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnovaCashFlowGeneratorApi
{
    public class DataContextSql : IdentityDbContext<User>
    {
        public DbSet<ReportModel> Reports { get; set; }

        public DataContextSql(DbContextOptions<DataContextSql> options)
             : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ReportModel>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        }
    }
}