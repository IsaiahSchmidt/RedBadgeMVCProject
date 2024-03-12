using musicProject.Models.TrackModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Services.TrackServices
{
    public interface ITrackService
    {
        Task<bool> CreateTrackAsync(TrackCreate model);
        Task<TrackDetail> GetTrackAsync(string title);
        Task<List<TrackListItem>> GetAllTracksAsync();
        Task<IEnumerable<TrackListItem>> GetTracksByArtistAsync(string artistName);
        Task<IEnumerable<TrackListItem>> GetTracksByRatingAsync();
        Task<TrackDetail> GetTrackByIdAsync(int id);
        Task<IEnumerable<TrackListItem>> GetTracksByAvgRatingAsync();
    }
}
