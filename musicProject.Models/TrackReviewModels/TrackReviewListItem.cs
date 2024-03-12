using musicProject.Data.Entities;
using musicProject.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.TrackReviewModels
{
    public class TrackReviewListItem
    {
        public int Id { get; set; }
        public string TrackTitle { get; set; }
        public double Rating { get; set; }
        public UserListItem User { get; set; }
    }
}
