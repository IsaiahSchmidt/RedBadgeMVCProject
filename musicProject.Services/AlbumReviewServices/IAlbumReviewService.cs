using musicProject.Models.AlbumReviewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Services.AlbumReviewServices
{
    public interface IAlbumReviewService
    {
        Task<bool> CreateAlbumReviewAsync(AlbumReviewCreate reviewCreate);
        Task<bool> UpdateAlbumReviewAsync(AlbumReviewUpdate reviewUpdate);
        Task<IEnumerable<AlbumReviewListItem>> GetUserAlbumReviewsAsync();
        Task<IEnumerable<AlbumReviewDetail>> GetAlbumReviewsByAlbumAsync(string albumTitle);
        Task<bool> DeleteAlbumReviewAsync(int id);
        Task<AlbumReviewDetail> GetAlbumReviewByIdAsync(int id);
    }
}
