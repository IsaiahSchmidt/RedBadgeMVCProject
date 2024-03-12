using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using musicProject.Data.Entities;
using musicProject.Models.TrackReviewModels;
using musicProject.Services.TrackReviewServices;
using musicProject.Services.TrackServices;

namespace musicProject.Controllers
{
    public class TrackReviewController : Controller
    {
        private ITrackReviewService _trackReviewService;
        private ITrackService _trackService;
        public TrackReviewController(ITrackReviewService trackReviewService, ITrackService trackService)
        {
            _trackReviewService = trackReviewService;
            _trackService = trackService;
        }

        [HttpGet]
        public async Task<IActionResult> NewIndex()
        {
            var reviews = await _trackService.GetTracksByAvgRatingAsync();
            return View(reviews);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["TrackId"] = new SelectList(await _trackService.GetAllTracksAsync(), "Id", "Title");

            TrackReviewCreate reviewCreate = new TrackReviewCreate();
            //var trackSelection = _trackService.GetAllTracksAsync().Result.Select(t=>new SelectListItem
            //{
            //    Text = t.Title,
            //    Value = t.Id.ToString(),
            //}).ToList();
            //reviewCreate.TrackSelection = trackSelection;
            return View(reviewCreate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrackId, Rating, Content, UserId")] TrackReviewCreate reviewCreate)
        {
            //ViewData["TrackId"] = new SelectList(await _trackService.GetAllTracksAsync(), "Id", "Title");
            if (await _trackReviewService.CreateTrackReviewAsync(reviewCreate))
                return RedirectToAction("NewIndex");
            return View(reviewCreate);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var review = await _trackReviewService.GetTrackReviewByIdAsync(id);
            if (review == null) return BadRequest();
            return View(review);
        }

        [HttpGet]
        public async Task<IActionResult> ReviewsByTrack(int id)
        {
            var reviews = await _trackReviewService.GetReviewsByTrackIdAsync(id);
            if (reviews == null) return BadRequest();
            return View(reviews);
        }

        //[HttpGet]
        //public async Task<IActionResult> ReviewsByTrack(string title)
        //{
        //    var reviews = await _trackReviewService.GetReviewsByTrackAsync(title);
        //    if (reviews == null) return BadRequest();
        //    return View(reviews);
        //}        
        
        [HttpGet]
        public async Task<IActionResult> ReviewsByTrackArtist(string name)
        {
            var reviews = await _trackReviewService.GetReviewsByTrackArtistAsync(name);
            if (reviews == null) return BadRequest();
            return View(reviews);
        }

        [HttpGet]
        public async Task<IActionResult> MyTrackReviews()
        {
            var reviews = await _trackReviewService.GetUserTrackReviewsAsync();
            if (reviews is null) return BadRequest("You haven't reviewed any tracks yet");
            return View(reviews);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var review = await _trackReviewService.GetTrackReviewByIdAsync(id);
            if (review == null) { return NotFound(); }
            TrackReviewUpdate reviewUpdate = new TrackReviewUpdate()
            {
                Id = review.Id,
                Content = review.Content,
                Rating = review.Rating
            };
            return View(reviewUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TrackReviewUpdate reviewUpdate)
        {
            if (!ModelState.IsValid) return View(ModelState);
            if (await _trackReviewService.UpdateTrackReviewAsync(reviewUpdate))
                return RedirectToAction("NewIndex");
            return View(reviewUpdate);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _trackReviewService.GetTrackReviewByIdAsync(id);
            if (review == null) return NotFound();
            //if (review.User.Id != _userId) return BadRequest("You cannot delete someone else's review");
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (await _trackReviewService.DeleteTrackReviewAsync(id)) return RedirectToAction("MyTrackReviews");
            return StatusCode(500, "Internal server error :( ");
        }
    }
}
