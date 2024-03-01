using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.TrackModels
{
    public class TrackCreate
    {
        [Required]
        [MinLength(1), MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public int ArtistId { get; set; }

        [Required]
        public int AlbumId { get; set; }
        public DateTime Released { get; set; }
    }
}
