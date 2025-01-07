using Microsoft.AspNetCore.Mvc;
using eCommerceAppExercise.Models;

namespace eCommerceAppExercise.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost("/order")]
        public IActionResult Order([Bind("OrderDate,InvoicePrice,Products")] Order order)
        {
            if (!ModelState.IsValid)
            {
                List<string> errorList = ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage).ToList();
                string errors = string.Join("\n", errorList);

                return BadRequest(errors);
            }

            Random random = new Random();
            int orderNo = random.Next(1, 99999);

            return Json(new {orderNumber = orderNo});
        }
    }
}
