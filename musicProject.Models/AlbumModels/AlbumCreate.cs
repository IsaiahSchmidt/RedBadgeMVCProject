using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.AlbumModels
{
    public class AlbumCreate
    {
        [Required]
        [MaxLength(100), MinLength(1)]
        public string Title { get; set; }

        [Required]
        public int ArtistId { get; set; }

        [MaxLength(25), MinLength(1)]
        public string Genre { get; set; }
        public DateTime Released { get; set; }
    }
}
