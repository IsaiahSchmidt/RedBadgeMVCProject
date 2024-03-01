using musicProject.Models.AlbumModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.TrackModels
{
    public class TrackListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public AlbumListItem Album { get; set; }
        public double Rating { get; set; }
    }
}
