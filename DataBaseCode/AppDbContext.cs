using Microsoft.EntityFrameworkCore;

namespace WinFormsOpenTK
{
    public class AppDbContext : DbContext
    {
        private string dataPath;

        public AppDbContext(string path) 
        {
            dataPath = path;
        }

        public DbSet<DbModel> ModelsData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(dataPath);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbModel>()
                .HasKey(m => m.PresetName);
        }
    }
}
