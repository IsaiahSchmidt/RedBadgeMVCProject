using musicProject.Models.TrackModels;
using musicProject.Models.TrackReviewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Services.TrackReviewServices
{
    public interface ITrackReviewService
    {
        Task<bool> CreateTrackReviewAsync(TrackReviewCreate reviewCreate);
        Task<bool> UpdateTrackReviewAsync(TrackReviewUpdate reviewUpdate);
        Task<IEnumerable<TrackReviewListItem>> GetUserTrackReviewsAsync();
        Task<IEnumerable<TrackReviewListItem>> GetReviewsByTrackAsync(string trackTitle);
        Task<IEnumerable<TrackReviewListItem>> GetReviewsByTrackArtistAsync(string artistName);
        Task<bool> DeleteTrackReviewAsync(int id);
        Task<TrackReviewDetail> GetTrackReviewByIdAsync(int id);
        Task<List<TrackReviewListItem>> GetAllTrackReviewsAsync();
        Task<TrackWithReviews> GetReviewsByTrackIdAsync(int trackId);

    }
}
