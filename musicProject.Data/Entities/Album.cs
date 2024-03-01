using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Data.Entities
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100), MinLength(1)]
        public string Title { get; set; }

        [Required]
        public int ArtistId { get; set; }

        [ForeignKey(nameof(ArtistId))]
        public Artist Artist { get; set; }

        [MaxLength(25), MinLength(1)]
        public string Genre { get; set; }
        public DateTime Released { get; set; }
        public List<Track> Tracks { get; set; } = new List<Track>();
    }
}
