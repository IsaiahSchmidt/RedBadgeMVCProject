using musicProject.Models.AlbumReviewModels;
using musicProject.Models.ArtistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.AlbumModels
{
    public class AlbumWithReviews
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ArtistListItem Artist { get; set; }
        public List<AlbumReviewListItem> AlbumReviews { get; set; }
    }
}
