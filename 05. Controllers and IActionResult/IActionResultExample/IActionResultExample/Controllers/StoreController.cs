using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class StoreController : Controller
    {
        [Route("store/books/{id}")]
        public IActionResult Book()
        {
            return Content($"Book Store, ID: {Request.RouteValues["id"]}");
        }
    }
}
