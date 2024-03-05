using Microsoft.EntityFrameworkCore;
using musicProject.Data.Data;
using musicProject.Data.Entities;
using musicProject.Models.AlbumModels;
using musicProject.Models.ArtistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Services.ArtistServices
{
    public class ArtistService : IArtistService
    {
        private readonly ApplicationDbContext _context;
        public ArtistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateArtistAsync(ArtistCreate model)
        {
            var artist = new Artist
            {
                Name = model.Name
            };
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ArtistListItem>> GetAllArtistsAsync()
        {
            List<ArtistListItem> artistList = await _context.Artists.Select(entity => new ArtistListItem
            {
                Name = entity.Name,
                Id = entity.Id
            }).ToListAsync();
            return artistList;
        }

        public async Task<ArtistDetail> GetArtistAsync(string name)
        {
            var artist = await _context.Artists.Include(a => a.Albums).FirstOrDefaultAsync(entity => entity.Name == name);
            if (artist == null) { return null; }
            return new ArtistDetail
            {
                Albums = artist.Albums.Select(a => new AlbumListItem
                {
                    Title = a.Title,
                    Genre = a.Genre,
                    Id = a.Id,
                    Released = a.Released
                }).ToList(),
                Name = artist.Name,
                Id = artist.Id
            };
        }
    }
}
