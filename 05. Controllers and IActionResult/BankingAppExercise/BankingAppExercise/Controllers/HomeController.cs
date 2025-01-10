using Microsoft.AspNetCore.Mvc;

namespace BankingAppExercise.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Content("Welcome to the Best Bank!", "text/plain");
        }

        [Route("account-details")]
        public IActionResult Details()
        {
            return Json(new { 
                accountNumber = 1001,
                accountHolderName = "Jonas",
                currentBalance = 5000
            });
        }

        [Route("account-statement")]
        public IActionResult Statement()
        {
            return File("/list.pdf", "application/pdf");
        }

        [Route("get-current-balance/{accountNumber?}")]
        public IActionResult GetBalance()
        {
            if (!Request.RouteValues.ContainsKey("accountNumber"))
            {
                return NotFound("Account number should be supplied");
            }

            if (Convert.ToInt32(Request.RouteValues["accountNumber"]) != 1001)
            {
                return BadRequest("Account number should be equal 1001");
            }

            return Content("5000", "text/plain");
        }
    }
}
