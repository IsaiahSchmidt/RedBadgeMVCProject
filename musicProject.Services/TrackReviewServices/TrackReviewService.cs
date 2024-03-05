using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using musicProject.Data.Data;
using musicProject.Data.Entities;
using musicProject.Models.TrackReviewModels;
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

        public async Task<IEnumerable<TrackReviewDetail>> GetReviewsByTrackAsync(string trackTitle)
        {
            List<TrackReviewDetail> reviews = new List<TrackReviewDetail>();
            List<TrackReviewListItem> reviewsByTrack = await _context.TrackReviews
                .Where(r => r.Track.Title == trackTitle).Select(entity => new TrackReviewListItem
                {
                    Rating = entity.Rating,
                    Content = entity.Content,
                    TrackId = entity.TrackId,
                }).ToListAsync();
            foreach (var review in reviewsByTrack)
            {
                TrackReviewDetail detail = new()
                {
                    Rating = review.Rating,
                    Content = review.Content,
                    UserId = review.UserId
                };
                reviews.Add(detail);
            }
            return reviews;
        }

        public async Task<IEnumerable<TrackReviewListItem>> GetUserTrackReviewsAsync()
        {
            ProcessUserInfo();
            return await _context.TrackReviews
                .Where(r => r.UserId == _userId)
                .Select(n => new TrackReviewListItem
                {
                    Id = n.Id,
                    Content = n.Content,
                    Rating = n.Rating,
                    TrackId = n.TrackId,
                }).ToListAsync();
        }

        public async Task<bool> UpdateTrackReviewAsync(TrackReviewUpdate reviewUpdate)
        {
            ProcessUserInfo();
            var review = await _context.TrackReviews.Where(r => r.UserId == _userId)
                .FirstOrDefaultAsync(n => n.Id == reviewUpdate.Id);
            if (review is null) return false;
            review.Rating = reviewUpdate.Rating;
            review.Content = reviewUpdate.Content;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TrackReviewDetail> GetTrackReviewByIdAsync(int id)
        {
            var review = await _context.TrackReviews.Where(r => r.UserId == _userId)
                .FirstOrDefaultAsync(n => n.Id == id);
            if (review is null) return null;
            return new TrackReviewDetail()
            {
                Id = review.Id,
                UserId = review.UserId,
                Rating = review.Rating,
                Content = review.Content
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
