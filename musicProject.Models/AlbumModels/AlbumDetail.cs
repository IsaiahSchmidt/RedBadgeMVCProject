using musicProject.Models.ArtistModels;
using musicProject.Models.TrackModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.AlbumModels
{
    public class AlbumDetail
    {
        public string Title { get; set; }
        public ArtistListItem Artist { get; set; }
        public List<TrackListItem> Tracks { get; set; }
        public string Genre { get; set; }
        public DateTime Released { get; set; }
    }
}
