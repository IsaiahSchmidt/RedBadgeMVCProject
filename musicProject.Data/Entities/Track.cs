using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Data.Entities
{
    public class Track
    {
        [Key]
        public int Id { get; set; }
        public int TrackNumber { get; set; }

        [Required]
        [MinLength(1), MaxLength(100)]
        public string Title { get; set; }

        public string Genre
        {
            get
            {
                if (Album == null)
                {
                    return "";
                }
                return Album.Genre;
            }
        }

        [Required]
        [ForeignKey(nameof(ArtistId))]
        public Artist Artist { get; set; }
        public int ArtistId { get; set; }

        [Required]
        [ForeignKey(nameof(Album))]
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        public DateTime Released { get; set; }
    }
}
