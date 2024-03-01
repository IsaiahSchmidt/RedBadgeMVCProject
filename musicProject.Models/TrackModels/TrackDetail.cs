using musicProject.Models.AlbumModels;
using musicProject.Models.ArtistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.TrackModels
{
    public class TrackDetail
    {
        public string Title { get; set; }
        public ArtistListItem Artist { get; set; }
        public AlbumListItem Album { get; set; }
    }
}
