using musicProject.Models.AlbumModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.AlbumReviewModels
{
    public class AlbumReviewDetail
    {
        public int Id { get; set; }
        public AlbumListItem Album { get; set; }
        public string Content { get; set; }
        public double Rating { get; set; }
        public string UserId { get; set; }
    }
}
