using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("book")]
        public IActionResult Index()
        {
            if (!Request.Query.ContainsKey("bookid"))
            {
                //Response.StatusCode = 400;
                //return Content("Book id not supplied");
                return BadRequest("Book id not supplied");
            }

            if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
            {
                //Response.StatusCode = 400;
                //return Content("Book id cannot be null or empty");
                return BadRequest("Book id cannot be null or empty");
            }

            int bookId = Convert.ToInt16(ControllerContext.HttpContext.Request.Query["bookid"]);
            if (bookId <= 0)
            {
                //Response.StatusCode = 400;
                //return Content("Book id cannot be less than or equal to zero");
                return BadRequest("Book id cannot be less than or equal to zero");
            }
            if (bookId > 1000)
            {
                //Response.StatusCode = 400;
                //return Content("Book id cannot be greater than 1000");
                return NotFound("Book id cannot be greater than 1000");
            }

            if (!Convert.ToBoolean(Request.Query["isloggedin"]))
            {
                //Response.StatusCode = 401;
                //return Content("User must be authenticated");
                return Unauthorized("User must be authenticated");
            }

            return File("/dog.png", "image/png");
        }
    }
}
