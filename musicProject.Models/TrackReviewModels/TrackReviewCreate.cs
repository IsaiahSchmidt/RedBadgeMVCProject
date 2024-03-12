using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicProject.Models.TrackReviewModels
{
    public class TrackReviewCreate
    {
        [Required]
        public int TrackId { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Please input a value between 1 and 10.")]
        public double Rating { get; set; }

        [MaxLength(2000), MinLength(1)]
        public string Content { get; set; } = string.Empty;
        public string UserId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> TrackSelection { get; set; }
    }
}
