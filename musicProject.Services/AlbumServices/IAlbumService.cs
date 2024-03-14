using musicProject.Models.AlbumModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Services.AlbumServices
{
    public interface IAlbumService
    {
        Task<int> CreateAlbumAsync(AlbumCreate model);
        Task<AlbumDetail> GetAlbumAsync(string title);
        Task<List<AlbumListItem>> GetAllAlbumsAsync();
        Task<IEnumerable<AlbumListItem>> GetAlbumsByArtistAsync(string artistName);
        Task<IEnumerable<AlbumListItem>> GetAlbumsByRatingAsync();
        Task<AlbumDetail> GetAlbumByIdAsync(int albumId);
        Task<IEnumerable<AlbumListItem>> GetAlbumsByAvgRatingAsync();

        // Unused Methods (save for later)
        //Task<IEnumerable<AlbumListItem>> GetAlbumsByGenreAsync(string genre);
    }
}
