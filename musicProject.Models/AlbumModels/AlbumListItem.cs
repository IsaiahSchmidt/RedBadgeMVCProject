using musicProject.Models.ArtistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.AlbumModels
{
    public class AlbumListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ArtistListItem Artist { get; set; }
        public string Genre { get; set; }
        public DateTime Released { get; set; }
        public double Rating { get; set; }
    }
}
