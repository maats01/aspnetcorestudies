using Microsoft.AspNetCore.Mvc;
using IActionResultExample.Models;

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("bookstore")]
        public IActionResult Index(int? bookId, bool? isLoggedIn, Book book)
        {
            if (bookId == null)
            {
                return BadRequest("Book id not supplied or empty");
            }

            if (bookId <= 0)
            {
                return BadRequest("Book id cannot be less than or equal to zero");
            }
            if (bookId > 1000)
            {
                return NotFound("Book id cannot be greater than 1000");
            }

            if (isLoggedIn == false)
            {
                return Unauthorized("User must be authenticated");
            }

            return Content($"Book id: {bookId}", "text/plain");
        }
    }
}
