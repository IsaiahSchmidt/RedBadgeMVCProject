using musicProject.Data.Entities;
using musicProject.Models.AlbumModels;
using musicProject.Models.User;
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
        public string AlbumTitle { get; set; }
        public double Rating { get; set; }
        public UserListItem User { get; set; }
    }
}
