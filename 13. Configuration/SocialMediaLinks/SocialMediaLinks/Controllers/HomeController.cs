using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace SocialMediaLinks.Controllers
{
    public class HomeController : Controller
    {
        private readonly SocialMediaLinksOptions _mediaOptions;

        public HomeController(IOptions<SocialMediaLinksOptions> options)
        {
            _mediaOptions = options.Value;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View(_mediaOptions);
        }
    }
}
