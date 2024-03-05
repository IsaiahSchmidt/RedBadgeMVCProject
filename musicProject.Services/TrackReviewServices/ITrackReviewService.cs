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
        Task<IEnumerable<TrackReviewDetail>> GetReviewsByTrackAsync(string trackTitle);
        Task<bool> DeleteTrackReviewAsync(int id);
        Task<TrackReviewDetail> GetTrackReviewByIdAsync(int id);
    }
}
