using musicProject.Models.ArtistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Services.ArtistServices
{
    public interface IArtistService
    {
        Task<bool> CreateArtistAsync(ArtistCreate model);
        Task<ArtistDetail> GetArtistAsync(string name);
        Task<List<ArtistListItem>> GetAllArtistsAsync();
    }
}
