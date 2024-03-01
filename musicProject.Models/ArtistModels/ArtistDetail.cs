using musicProject.Models.AlbumModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.ArtistModels
{
    public class ArtistDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AlbumListItem> Albums { get; set; }
    }
}
