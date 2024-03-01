using musicProject.Data.Entities;
using musicProject.Models.AlbumModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.AlbumReviewModels
{
    public class AlbumReviewListItem
    {
        public int Id { get; set; }
        public int AlbumId { get; set; } 
        public double Rating { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
    }
}
