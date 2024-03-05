using Microsoft.EntityFrameworkCore;
using musicProject.Data.Data;
using musicProject.Data.Entities;
using musicProject.Models.AlbumModels;
using musicProject.Models.ArtistModels;
using musicProject.Models.TrackModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Services.AlbumServices
{
    public class AlbumService : IAlbumService
    {
        private readonly ApplicationDbContext _context;
        public AlbumService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAlbumAsync(AlbumCreate model)
        {
            var album = new Album
            {
                Title = model.Title,
                ArtistId = model.ArtistId,
                Released = model.Released,
                Genre = model.Genre
            };
            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();
            return album.Id;
        }

        public async Task<AlbumDetail> GetAlbumAsync(string title)
        {
            var album = await _context.Albums.Include(a => a.Tracks).Include(a => a.Artist)
                .FirstOrDefaultAsync(entity => entity.Title == title);
            if (album == null) { return null; }
            return new AlbumDetail
            {
                Title = album.Title,
                Artist = new ArtistListItem
                {
                    Id = album.Artist.Id,
                    Name = album.Artist.Name,
                },
                Tracks = album.Tracks.Select(t => new TrackListItem
                {
                    Id = t.Id,
                    Title = t.Title,
                }).ToList(),
                Released = album.Released,
                Genre = album.Genre
            };
        }

        public async Task<AlbumDetail> GetAlbumByIdAsync(int albumId)
        {
            var album = await _context.Albums.Include(a => a.Tracks).Include(a => a.Artist)
                .FirstOrDefaultAsync(entity => entity.Id == albumId);
            if (album == null) { return null; }
            return new AlbumDetail
            {
                Title = album.Title,
                Artist = new ArtistListItem
                {
                    Id = album.Artist.Id,
                    Name = album.Artist.Name,
                },
                Tracks = album.Tracks.Select(t => new TrackListItem
                {
                    Id = t.Id,
                    Title = t.Title,
                }).ToList(),
                Released = album.Released,
                Genre = album.Genre
            };
        }

        public async Task<IEnumerable<AlbumListItem>> GetAlbumsByArtistAsync(string artistName)
        {
            List<AlbumListItem> albumsByArtist = await _context.Albums
                .Where(a => a.Artist.Name == artistName).Include(a => a.Tracks).Include(a => a.Artist)
                .Select(entity => new AlbumListItem
                {
                    Title = entity.Title,
                    Artist = new ArtistListItem
                    {
                        Id = entity.Artist.Id,
                        Name = entity.Artist.Name,
                    },
                    Released = entity.Released,
                    Genre = entity.Genre
                }).ToListAsync();
            return albumsByArtist;
        }

        public async Task<IEnumerable<AlbumListItem>> GetAlbumsByGenreAsync(string genre)
        {
            List<AlbumListItem> albumsByGenre = await _context.Albums
                .Where(g => g.Genre == genre).Include(a => a.Tracks).Include(a => a.Artist)
                .Select(entity => new AlbumListItem
                {
                    Title = entity.Title,
                    Artist = new ArtistListItem
                    {
                        Id = entity.Artist.Id,
                        Name = entity.Artist.Name,
                    },
                    Released = entity.Released,
                    Genre = entity.Genre
                }).ToListAsync();
            return albumsByGenre;
        }

        public async Task<IEnumerable<AlbumListItem>> GetAlbumsByRatingAsync()
        {
            List<AlbumListItem> albumListItems = await _context.Albums.Include(a => a.Artist)
                .Select(entity => new AlbumListItem
                {
                    Title = entity.Title,
                    Artist = new ArtistListItem
                    {
                        Id = entity.Artist.Id,
                        Name = entity.Artist.Name,
                    },
                    Released = entity.Released,
                    Genre = entity.Genre
                }).ToListAsync();
            return albumListItems.OrderBy(review => review.Rating).ToList();
        }

        public async Task<List<AlbumListItem>> GetAllAlbumsAsync()
        {
            List<AlbumListItem> albumList = await _context.Albums.Include(a => a.Tracks).Include(a => a.Artist)
                .Select(entity => new AlbumListItem
                {
                    Title = entity.Title,
                    Artist = new ArtistListItem
                    {
                        Id = entity.Artist.Id,
                        Name = entity.Artist.Name,
                    },
                    Released = entity.Released,
                    Genre = entity.Genre
                }).ToListAsync();
            return albumList;
        }
    }
}
