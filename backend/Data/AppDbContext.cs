using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tablo isimlerinin Tekil olması 
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Subcategory>().ToTable("Subcategory");

            // Category Id -> Subcategory Category_id FK Maplemesi
            modelBuilder.Entity<Subcategory>()
                .HasOne(c => c.Category)
                .WithMany(s => s.Subcategories)
                .HasForeignKey(s => s.Category_id);
        }
    }
}
