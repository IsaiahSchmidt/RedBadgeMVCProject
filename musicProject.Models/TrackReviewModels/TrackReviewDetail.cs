using musicProject.Data.Entities;
using musicProject.Models.TrackModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.TrackReviewModels
{
    internal class TrackReviewDetail
    {
        public int Id { get; set; }
        public TrackListItem Track { get; set; }
        public double Rating { get; set; }
        public string Content { get; set; } 
        public string MLUserId { get; set; }
    }
}
