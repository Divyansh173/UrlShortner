using Microsoft.EntityFrameworkCore;
using URLShortner.Entities;
using URLShortner.Services;

namespace URLShortner
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public DbSet<UrlShortner> UrlShorteners {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UrlShortner>()
                 .HasKey(c => c.ShortUrl);

            modelBuilder.Entity<UrlShortner>()
                .Property(c => c.ShortUrl)
                .HasMaxLength(UrlShortnerService.MAX_LENGTH)
                .IsRequired();

            modelBuilder.Entity<UrlShortner>()
                .Property(c => c.FullUrl)
                .IsRequired();
        }

    }
}
