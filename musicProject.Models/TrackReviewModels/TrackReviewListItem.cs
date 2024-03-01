using musicProject.Data.Entities;
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
        public int TrackId { get; set; }
        public double Rating { get; set; }
        public string Content { get; set; }
        public string MLUserId { get; set; }
    }
}
