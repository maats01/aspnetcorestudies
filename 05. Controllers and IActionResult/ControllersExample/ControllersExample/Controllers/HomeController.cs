using Microsoft.AspNetCore.Mvc;
using ControllersExample.Models;

namespace ControllersExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        [Route("/")]
        public ContentResult Index()
        {
            //return new ContentResult() { Content = "Hi from index", ContentType = "text/plain"};
            //return Content("Hi from index", "text/plain");

            return Content("<h1>Welcome!</h1> <h2>Hello from Index</h2>", "text/html");
        }

        [Route("person")]
        public JsonResult Person()
        {
            Person person = new Person() { 
                Id = Guid.NewGuid(),
                FirstName = "James",
                LastName = "Smith",
                Age = 25
            };
            //return new JsonResult(person);
            return Json(person);
        }

        [Route("file-download")]
        public VirtualFileResult FileDownload()
        {
            //return new VirtualFileResult("/dog.png", "image/png");
            return File("/dog.png", "image/png");
        }

        [Route("file-download2")]
        public PhysicalFileResult FileDownload2()
        {
            //return new PhysicalFileResult(@"C:\Users\Matheus Sampaio\Desktop\Estudo\aspnetcorestudies\ControllersExample\ControllersExample\wwwroot\dog.png", "image/png");
            return PhysicalFile(@"C:\Users\Matheus Sampaio\Desktop\Estudo\aspnetcorestudies\ControllersExample\ControllersExample\wwwroot\dog.png", "image/png");
        }

        [Route("file-download3")]
        public FileContentResult FileDownload3()
        {
            byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Matheus Sampaio\Desktop\Estudo\aspnetcorestudies\ControllersExample\ControllersExample\wwwroot\dog.png");
            //return new FileContentResult(bytes, "image/png");
            return File(bytes, "image/png");
        }
    }
}
