using musicProject.Models.AlbumReviewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.User
{
    public class UserListItem
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int albumReviewId { get; set; }
        public int trackReviewId { get; set; }
    }
}
