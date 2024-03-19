using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using musicProject.Data.Data;
using musicProject.Data.Entities;
using musicProject.Models.AlbumModels;
using musicProject.Models.AlbumReviewModels;
using musicProject.Models.ArtistModels;
using musicProject.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Services.AlbumReviewServices
{
    public class AlbumReviewService : IAlbumReviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private string _userId;
        public AlbumReviewService(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> CreateAlbumReviewAsync(AlbumReviewCreate reviewCreate)
        {
            ProcessUserInfo();
            AlbumReview review = new()
            {
                AlbumId = reviewCreate.AlbumId,
                Content = reviewCreate.Content,
                Rating = reviewCreate.Rating,
                UserId = _userId
            };
            await _context.AlbumReviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAlbumReviewAsync(int id)
        {
            ProcessUserInfo();
            var review = await _context.AlbumReviews
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == _userId);
            if (review == null) { return false; }
            _context.AlbumReviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AlbumReviewListItem>> GetUserAlbumReviewsAsync()
        {
            ProcessUserInfo();
            return await _context.AlbumReviews.Include(r => r.User).Include(a => a.Album)
                .Where(r => r.UserId == _userId)
                .Select(n => new AlbumReviewListItem
                {
                    Id = n.Id,
                    //Content = n.Content,
                    Rating = n.Rating,
                    AlbumTitle = n.Album.Title,           
                    AlbumId = n.Album.Id,
                    User = new UserListItem()
                    {
                        Id = n.User.Id,
                        UserName = n.User.UserName
                    }
                }).OrderByDescending(r => r.Rating).ToListAsync();
        }

        public async Task<IEnumerable<AlbumReviewListItem>> GetAlbumReviewsByAlbumAsync(string albumTitle)
        {
            List<AlbumReviewDetail> reviews = new List<AlbumReviewDetail>();
            List<AlbumReviewListItem> reviewsByAlbum = await _context.AlbumReviews
                .Include(a => a.Album).Include(r => r.User)
                .Where(r => r.Album.Title == albumTitle).Select(entity => new AlbumReviewListItem
                {
                    Id = entity.Id,
                    Rating = entity.Rating,
                    //Content = entity.Content,
                    AlbumTitle = entity.Album.Title,
                    User = new UserListItem()
                    {
                        Id = entity.User.Id,
                        UserName = entity.User.UserName
                    }
                }).ToListAsync();
            return reviewsByAlbum;
        }       
        
        public async Task<IEnumerable<AlbumReviewListItem>> GetAlbumReviewsByArtistAsync(string artistName)
        {
            List<AlbumReviewDetail> reviews = new List<AlbumReviewDetail>();
            List<AlbumReviewListItem> reviewsByArtist = await _context.AlbumReviews
                .Include(a => a.Album).Include(r => r.User)
                .Where(r => r.Album.Artist.Name == artistName).Select(entity => new AlbumReviewListItem
                {
                    Id = entity.Id,
                    Rating = entity.Rating,
                    //Content = entity.Content,
                    AlbumTitle = entity.Album.Title,
                    User = new UserListItem()
                    {
                        Id = entity.User.Id,
                        UserName = entity.User.UserName
                    }
                }).ToListAsync();


            //foreach (var review in reviewsByArtist)
            //{
            //    AlbumReviewDetail detail = new()
            //    {
            //        Id = review.Id,
            //        Rating = review.Rating,
            //        Content = review.Content,
            //        User = new UserListItem()
            //        {
            //            Id = review.User.Id,
            //            UserName = review.User.UserName
            //        }
            //    };
            //    reviews.Add(detail);
            //}
            //this is unnecessary - code above already does this
            return reviewsByArtist;
        }

        public async Task<bool> UpdateAlbumReviewAsync(AlbumReviewUpdate reviewUpdate)
        {
            ProcessUserInfo();
            var review = await _context.AlbumReviews.Where(r => r.UserId == _userId)
                .FirstOrDefaultAsync(n => n.Id == reviewUpdate.Id);
            if (review is null) return false;
            review.Rating = reviewUpdate.Rating;
            review.Content = reviewUpdate.Content;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AlbumReviewDetail> GetAlbumReviewByIdAsync(int id)
        {
            var review = await _context.AlbumReviews
                .Include(r => r.User).Include(a => a.Album).ThenInclude(a => a.Artist)
                .FirstOrDefaultAsync(n => n.Id == id);
            if (review is null) return null;
            return new AlbumReviewDetail()
            {
                Album = new AlbumListItem
                {
                    Title = review.Album.Title,
                    Artist = new ArtistListItem
                    {
                        Name = review.Album.Artist.Name,
                        Id = review.Album.Artist.Id
                    },
                    Id = review.Album.Id,
                    Genre = review.Album.Genre,
                    Released = review.Album.Released,
                },
                Id = review.Id,
                Rating = review.Rating,
                Content = review.Content,
                User = new UserListItem()
                {
                    Id = review.User.Id,
                    UserName = review.User.UserName
                }
            };
        }

        public async Task<AlbumWithReviews> GetReviewsByAlbumIdAsync(int albumId)
        {
            var albumWithReviews = await _context.Albums.Include(a=>a.Artist)
                .FirstOrDefaultAsync(a=>a.Id == albumId);
            if (albumWithReviews == null) return null;
            return new AlbumWithReviews()
            {
                Id = albumWithReviews.Id,
                Title = albumWithReviews.Title,
                Artist = new ArtistListItem
                {
                    Id = albumWithReviews.Artist.Id,
                    Name = albumWithReviews.Artist.Name,
                },
                AlbumReviews = await _context.AlbumReviews.Include(u => u.User).Include(a => a.Album)
                .Where(a => a.AlbumId == albumId).Select(entity => new AlbumReviewListItem
                {
                    Id = entity.Id,
                    Rating = entity.Rating,
                    User = new UserListItem
                    {
                        Id = entity.User.Id,
                        UserName = entity.User.UserName
                    }
                }).OrderByDescending(r => r.Rating).ToListAsync()
            };
        }
        public async Task<List<AlbumReviewListItem>> GetAllAlbumReviewsAsync()
        {
            List<AlbumReviewListItem> albumReviews = await _context.AlbumReviews.Include(u => u.User).Include(a => a.Album)
                .Select(entity => new AlbumReviewListItem
                {
                    Id = entity.Id,
                    Rating = entity.Rating,
                    AlbumTitle = entity.Album.Title,
                    User = new UserListItem()
                    {
                        Id = entity.User.Id,
                        UserName = entity.User.UserName
                    }
                }).ToListAsync();
            return albumReviews.OrderByDescending(r => r.Rating).ToList();
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
