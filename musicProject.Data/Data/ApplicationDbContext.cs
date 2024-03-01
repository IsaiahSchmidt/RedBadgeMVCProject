using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using musicProject.Data.Entities;

namespace musicProject.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users {  get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<BaseReview> BaseReviews { get; set; }
        public DbSet<AlbumReview> AlbumReviews { get; set; }
        public DbSet<TrackReview> TrackReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Artist>().HasData(
                new Artist
                {
                    Id = 1,
                    Name = "TEST",
                }
                );
            modelBuilder.Entity<Album>().HasData(
                new Album
                {
                    Id = 1,
                    Title = "TESTAlbum",
                    ArtistId = 1,
                    Genre = "rock",
                    Released = DateTime.Now
                }
                );
            modelBuilder.Entity<Track>().HasData(
                new Track
                {
                    Id = 1,
                    Title = "testTitle",
                    ArtistId = 1,
                    AlbumId = 1,
                    TrackNumber = 1,
                    Released = DateTime.Now
                },
                new Track
                {
                    Id = 2,
                    Title = "testTitle2",
                    ArtistId = 1,
                    AlbumId = 1,
                    TrackNumber = 2,
                    Released = DateTime.Now
                }
                );
            modelBuilder.Entity<AlbumReview>().HasData(
                new AlbumReview
                {
                    Id = 1,
                    Content = "bjkvlyg",
                    Rating = 5,
                    AlbumId = 1,
                }
                );
            modelBuilder.Entity<TrackReview>().HasData(
               new TrackReview
               {
                   Id = 2,
                   Content = "kjbguyvl",
                   Rating = 4,
                   TrackId = 1,
               }
               );
        }
    }
}
