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
                    Id = entity.Id,
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
                }).OrderBy(t => t.Title).ToListAsync();
            return trackList;
        }

        public async Task<TrackDetail> GetTrackAsync(string title)
        {
            var track = await _context.Tracks.Include(a => a.Artist).Include(a => a.Album)
                .FirstOrDefaultAsync(entity => entity.Title == title);
            if (track == null) { return null; }
            return new TrackDetail
            {
                Id = track.Id,
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

        public async Task<TrackDetail> GetTrackByIdAsync(int id)
        {
            var track = await _context.Tracks.Include(a => a.Album).Include(a => a.Artist)
                .FirstOrDefaultAsync(entity => entity.Id == id);
            if (track is null) return null;
            return new TrackDetail
            {
                Id = track.Id,
                Title = track.Title,
                Album = new AlbumListItem
                {
                    Title = track.Album.Title,
                    Id = track.Album.Id,
                    Artist = new ArtistListItem
                    {
                        Name = track.Artist.Name,
                        Id = track.Artist.Id
                    },
                    Genre = track.Album.Genre,
                    Released = track.Album.Released,
                }
            };
        }

        public async Task<IEnumerable<TrackListItem>> GetTracksByArtistAsync(string artistName)
        {
            List<TrackListItem> tracksByArtist = await _context.Tracks
                .Where(a => a.Artist.Name == artistName).Include(a => a.Artist).Include(a => a.Album)
                .Select(entity => new TrackListItem
                {
                    Id = entity.Id,
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

        public async Task<IEnumerable<TrackListItem>> GetTracksByAvgRatingAsync()
        {
            List<TrackListItem> tracks = new List<TrackListItem>();

            List<TrackListItem> tracksByRating = await _context.Tracks.Include(a => a.Album).Include(a => a.Artist)
                .Select(entity => new TrackListItem
                {
                    Id = entity.Id,
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
            foreach (var track in tracksByRating)
            {
                List<TrackReview> reviews = _context.TrackReviews.Where(r => r.Track.Title == track.Title).ToList();
                double avgRating = 0;
                if (reviews.Count != 0)
                    avgRating = Math.Round(reviews.Average(r => r.Rating), 2);

                tracks.Add(new TrackListItem()
                {
                    Id = track.Id,
                    Title = track.Title,
                    Album = new AlbumListItem
                    {
                        Title = track.Album.Title,
                        Genre = track.Album.Genre,
                        Released = track.Album.Released,
                        Id = track.Album.Id,
                        Artist = new ArtistListItem
                        {
                            Name = track.Album.Artist.Name,
                            Id = track.Album.Artist.Id
                        },
                    },
                    Rating = avgRating
                });
            }

            return tracks.OrderByDescending(r => r.Rating);
        }

        public async Task<IEnumerable<TrackListItem>> GetTracksByRatingAsync()
        {
            List<TrackListItem> trackListItems = await _context.Tracks.Include(a => a.Artist).Include(a => a.Album)
                    .Select(entity => new TrackListItem
                    {
                        Id = entity.Id,
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
