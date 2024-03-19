using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using musicProject.Data.Data;
using musicProject.Data.Entities;
using musicProject.Models.AlbumModels;
using musicProject.Models.ArtistModels;
using musicProject.Models.TrackModels;
using musicProject.Models.TrackReviewModels;
using musicProject.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Services.TrackReviewServices
{
    public class TrackReviewService : ITrackReviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private string _userId;
        public TrackReviewService(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }
        public async Task<bool> CreateTrackReviewAsync(TrackReviewCreate reviewCreate)
        {
            ProcessUserInfo();
            TrackReview review = new()
            {
                TrackId = reviewCreate.TrackId,
                Content = reviewCreate.Content,
                Rating = reviewCreate.Rating,
                UserId = _userId
            };
            await _context.TrackReviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTrackReviewAsync(int id)
        {
            ProcessUserInfo();
            var review = await _context.TrackReviews
                .Where(u => u.UserId == _userId)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (review == null) { return false; }
            _context.TrackReviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TrackReviewListItem>> GetReviewsByTrackAsync(string trackTitle)
        {
            List<TrackReviewDetail> reviews = new List<TrackReviewDetail>();
            List<TrackReviewListItem> reviewsByTrack = await _context.TrackReviews
                .Include(t => t.Track).Include(u => u.User)
                .Where(r => r.Track.Title == trackTitle).Select(entity => new TrackReviewListItem
                {
                    Id = entity.Id,
                    Rating = entity.Rating,
                    //Content = entity.Content,
                    TrackTitle = entity.Track.Title,
                    User = new UserListItem
                    {
                        Id = entity.User.Id,
                        UserName = entity.User.UserName
                    }
                }).ToListAsync();
            return reviewsByTrack;
        }

        public async Task<IEnumerable<TrackReviewListItem>> GetReviewsByTrackArtistAsync(string artistName)
        {
            List<TrackReviewListItem> reviewsByTrack = await _context.TrackReviews
                .Include(t => t.Track).ThenInclude(a => a.Artist).Include(u => u.User)
                .Where(r => r.Track.Artist.Name == artistName).Select(entity => new TrackReviewListItem
                {
                    Id = entity.Id,
                    Rating = entity.Rating,
                    //Content = entity.Content,
                    TrackTitle = entity.Track.Title,
                    User = new UserListItem
                    {
                        Id = entity.User.Id,
                        UserName = entity.User.UserName
                    }
                }).ToListAsync();
            return reviewsByTrack;
        }

        public async Task<IEnumerable<TrackReviewListItem>> GetUserTrackReviewsAsync()
        {
            ProcessUserInfo();
            return await _context.TrackReviews
                .Where(r => r.UserId == _userId).Include(u => u.User)
                .Include(t => t.Track).ThenInclude(a => a.Album)
                .Select(n => new TrackReviewListItem
                {
                    Id = n.Id,
                    Rating = n.Rating,
                    TrackId = n.Track.Id,
                    TrackTitle = n.Track.Title
                }).OrderByDescending(r => r.Rating).ToListAsync();
        }

        public async Task<bool> UpdateTrackReviewAsync(TrackReviewUpdate reviewUpdate)
        {
            ProcessUserInfo();
            var review = await _context.TrackReviews.Where(r => r.UserId == _userId)
                .FirstOrDefaultAsync(n => n.Id == reviewUpdate.Id);
            if (review is null) return false;
            if (review.UserId != _userId) return false;
            review.Rating = reviewUpdate.Rating;
            review.Content = reviewUpdate.Content;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TrackReviewDetail> GetTrackReviewByIdAsync(int id)
        {
            var review = await _context.TrackReviews
                .Include(u => u.User).Include(t => t.Track).ThenInclude(a => a.Album).ThenInclude(a => a.Artist)
                .FirstOrDefaultAsync(n => n.Id == id);
            if (review is null) return null;
            return new TrackReviewDetail()
            {
                Id = review.Id,
                Rating = review.Rating,
                Content = review.Content,
                Track = new TrackListItem
                {
                    Id = review.Track.Id,
                    Title = review.Track.Title,
                    Album = new AlbumListItem
                    {
                        Id = review.Track.Album.Id,
                        Artist = new ArtistListItem
                        {
                            Id = review.Track.Artist.Id,
                            Name = review.Track.Artist.Name
                        },
                        Title = review.Track.Album.Title,
                        Genre = review.Track.Album.Genre,
                        Released = review.Track.Album.Released,
                    },
                },
                User = new UserListItem
                {
                    Id = review.User.Id,
                    UserName = review.User.UserName
                }
            };
        }
        public async Task<List<TrackReviewListItem>> GetAllTrackReviewsAsync()
        {
            List<TrackReviewListItem> trackReviews = await _context.TrackReviews
                .Include(u => u.User).Include(t => t.Track)
                .Select(entity => new TrackReviewListItem
                {
                    Id = entity.Id,
                    Rating = entity.Rating,
                    TrackTitle = entity.Track.Title,
                    User = new UserListItem
                    {
                        Id = entity.User.Id,
                        UserName = entity.User.UserName
                    }
                }).ToListAsync();
            return trackReviews.OrderByDescending(r => r.Rating).ToList();
        }

        public async Task<TrackWithReviews> GetReviewsByTrackIdAsync(int trackId)
        {
            var trackWithReviews = await _context.Tracks
                .Include(a=>a.Album).Include(a=>a.Artist)
                .FirstOrDefaultAsync(t => t.Id == trackId);
            if (trackWithReviews == null) return null;
            return new TrackWithReviews()
            {
                Id = trackWithReviews.Id,
                Title = trackWithReviews.Title,
                Artist = new ArtistListItem
                {
                    Id = trackWithReviews.Artist.Id,
                    Name = trackWithReviews.Artist.Name,
                },
                Album = new AlbumListItem
                {
                    Id = trackWithReviews.Album.Id,
                    Title = trackWithReviews.Album.Title,
                    Genre = trackWithReviews.Album.Genre,
                    Released = trackWithReviews.Album.Released,
                    Artist = new ArtistListItem
                    {
                        Id = trackWithReviews.Album.Artist.Id,
                        Name = trackWithReviews.Album.Artist.Name,
                    }
                },
                TrackReviews = await _context.TrackReviews.Include(u => u.User).Include(t=>t.Track)
                .Where(t=>t.TrackId == trackId)
                .Select(entity => new TrackReviewListItem
                {
                    Id = entity.Id,
                    Rating = entity.Rating,
                    User = new UserListItem
                    {
                        Id = entity.User.Id,
                        UserName = entity.User.UserName,
                    }
                }).OrderByDescending(r => r.Rating).ToListAsync()
            };
        }
        private void ProcessUserInfo()
        {
            var claims = _contextAccessor.HttpContext!.User.Identity as ClaimsIdentity;
            var claimId = claims.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            if (claimId == null) { throw new Exception("Unable to verify user credentails."); }
            _userId = claimId;
        }

    }
}
