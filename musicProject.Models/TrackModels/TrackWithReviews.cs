using musicProject.Models.AlbumModels;
using musicProject.Models.ArtistModels;
using musicProject.Models.TrackReviewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.TrackModels
{
    public class TrackWithReviews
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ArtistListItem Artist { get; set; }
        public AlbumListItem Album { get; set; }
        public List<TrackReviewListItem> TrackReviews { get; set; }
    }
}
