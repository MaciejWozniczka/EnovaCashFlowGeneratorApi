using Microsoft.EntityFrameworkCore;

namespace EnovaCashFlowGeneratorApi
{
    public class DataContextEnova : DbContext
    {
        public DbSet<Feature> Features { get; set; }
        public DbSet<DbItem> DbItems { get; set; }
        private string _connectionString;

        public DataContextEnova(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataContextEnova(DbContextOptions<DataContextEnova> options)
         : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Feature>().HasKey("Parent");
            modelBuilder.Entity<DbItem>().HasKey("ID");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}