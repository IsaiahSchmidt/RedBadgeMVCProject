using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using musicProject.Data.Data;
using musicProject.Data.Entities;
using musicProject.Models.AlbumReviewModels;
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
                .Where(u => u.UserId == _userId)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (review == null) { return false; }
            _context.AlbumReviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AlbumReviewListItem>> GetUserAlbumReviewsAsync()
        {
            ProcessUserInfo();
            return await _context.AlbumReviews
                .Where(r => r.UserId == _userId)
                .Select(n => new AlbumReviewListItem
                {
                    Id = n.Id,
                    Content = n.Content,
                    Rating = n.Rating,
                    AlbumId = n.AlbumId
                }).ToListAsync();
        }

        public async Task<IEnumerable<AlbumReviewDetail>> GetAlbumReviewsByAlbumAsync(string albumTitle)
        {
            List<AlbumReviewDetail> reviews = new List<AlbumReviewDetail>();
            List<AlbumReviewListItem> reviewsByAlbum = await _context.AlbumReviews
                .Where(r => r.Album.Title == albumTitle).Select(entity => new AlbumReviewListItem
                {
                    Rating = entity.Rating,
                    Content = entity.Content,
                    AlbumId = entity.AlbumId,
                    UserId = entity.UserId
                }).ToListAsync();
            foreach (var review in reviewsByAlbum)
            {
                AlbumReviewDetail detail = new()
                {
                    Id = review.Id,
                    Rating = review.Rating,
                    Content = review.Content,
                    UserId = review.UserId,
                };
                reviews.Add(detail);
            }
            return reviews;
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
            var review = await _context.AlbumReviews.Where(r => r.UserId == _userId)
                .FirstOrDefaultAsync(n => n.Id == id);
            if (review is null) return null;
            return new AlbumReviewDetail()
            {
                Id = review.Id,
                Rating = review.Rating,
                Content = review.Content,
                UserId = _userId
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
