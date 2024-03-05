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

namespace musicProject.Services.TrackServices
{
    public class TrackService : ITrackService
    {
        private readonly ApplicationDbContext _context;
        public TrackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateTrackAsync(TrackCreate model)
        {
            var track = new Track
            {
                Title = model.Title,
                ArtistId = model.ArtistId,
                AlbumId = model.AlbumId,
            };
            await _context.Tracks.AddAsync(track);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TrackListItem>> GetAllTracksAsync()
        {
            List<TrackListItem> trackList = await _context.Tracks.Include(a => a.Artist).Include(a => a.Album)
                .Select(entity => new TrackListItem
                {
                    Title = entity.Title,
                    Album = new AlbumListItem
                    {
                        Title = entity.Album.Title,
                        Genre = entity.Album.Genre,
                        Released = entity.Album.Released,
                        Id = entity.Album.Id,
                        Artist = new ArtistListItem
                        {
                            Name = entity.Artist.Name,
                            Id = entity.Artist.Id
                        }
                    }
                }).ToListAsync();
            return trackList;
        }

        public async Task<TrackDetail> GetTrackAsync(string title)
        {
            var track = await _context.Tracks.Include(a => a.Artist).Include(a => a.Album)
                .FirstOrDefaultAsync(entity => entity.Title == title);
            if (track == null) { return null; }
            return new TrackDetail
            {
                Title = track.Title,
                Album = new AlbumListItem
                {
                    Title = track.Album.Title,
                    Genre = track.Album.Genre,
                    Released = track.Album.Released,
                    Id = track.Album.Id,
                    Artist = new ArtistListItem
                    {
                        Name = track.Artist.Name,
                        Id = track.Artist.Id
                    }
                }
            };
        }

        public async Task<IEnumerable<TrackListItem>> GetTracksByArtistAsync(string artistName)
        {
            List<TrackListItem> tracksByArtist = await _context.Tracks
                .Where(a => a.Artist.Name == artistName).Include(a => a.Artist).Include(a => a.Album)
                .Select(entity => new TrackListItem
                {
                    Title = entity.Title,
                    Album = new AlbumListItem
                    {
                        Title = entity.Album.Title,
                        Genre = entity.Album.Genre,
                        Released = entity.Album.Released,
                        Id = entity.Album.Id,
                        Artist = new ArtistListItem
                        {
                            Name = entity.Artist.Name,
                            Id = entity.Artist.Id
                        }
                    }
                }).ToListAsync();
            return tracksByArtist;
        }

        public async Task<IEnumerable<TrackListItem>> GetTracksByRatingAsync()
        {
            List<TrackListItem> trackListItems = await _context.Tracks.Include(a => a.Artist).Include(a => a.Album)
                    .Select(entity => new TrackListItem
                    {
                        Title = entity.Title,
                        Album = new AlbumListItem
                        {
                            Title = entity.Album.Title,
                            Genre = entity.Album.Genre,
                            Released = entity.Album.Released,
                            Id = entity.Album.Id,
                            Artist = new ArtistListItem
                            {
                                Name = entity.Artist.Name,
                                Id = entity.Artist.Id
                            }
                        }
                    }).ToListAsync();

            return trackListItems.OrderBy(review => review.Rating).ToList();
        }
    }
}
