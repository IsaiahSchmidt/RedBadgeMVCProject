using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Data.Entities
{
    public class TrackReview : BaseReview
    {
        [Required]
        [ForeignKey(nameof(Track))]
        public int TrackId { get; set; }
        public Track Track { get; set; } = null;
    }
}
