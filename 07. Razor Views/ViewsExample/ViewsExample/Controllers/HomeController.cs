using Microsoft.AspNetCore.Mvc;
using ViewsExample.Models;

namespace ViewsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        [Route("/")]
        public IActionResult Index()
        {
            ViewData["appTitle"] = "Asp.Net Core Demo App";
            List<Person> people = new List<Person>()
            {
                new Person() {Name = "John", DateOfBirth = Convert.ToDateTime("2000-08-01"), PersonGender = Gender.Male},
                new Person() {Name = "Linda", DateOfBirth = Convert.ToDateTime("2003-10-03"), PersonGender = Gender.Female},
                new Person() {Name = "Susan", DateOfBirth = Convert.ToDateTime("2005-08-27"), PersonGender = Gender.Female}
            };

            return View(people); // Views/Home/Index.cshtml - Views/{controllerName}/{actionName}.cshtml
        }

        [Route("person-details/{name?}")]
        public IActionResult Details(string? name)
        {
            if (name == null)
            {
                return Content("Person name can't be null");
            }

            List<Person> people = new List<Person>()
            {
                new Person() {Name = "John", DateOfBirth = Convert.ToDateTime("2000-08-01"), PersonGender = Gender.Male},
                new Person() {Name = "Linda", DateOfBirth = Convert.ToDateTime("2003-10-03"), PersonGender = Gender.Female},
                new Person() {Name = "Susan", DateOfBirth = Convert.ToDateTime("2005-08-27"), PersonGender = Gender.Female}
            };

            Person person = people.Where(person => person.Name == name).FirstOrDefault();
            return View(person);
        }

        [Route("person-with-product")]
        public IActionResult PersonWithProduct()
        {
            Person person = new Person()
            {
                Name = "Sara",
                DateOfBirth = Convert.ToDateTime("2004-07-01"),
                PersonGender = Gender.Female
            };
            Product product = new Product()
            {
                ProductId = 1,
                ProductName = "Air conditioner"
            };

            PersonProductViewModel viewModel = new PersonProductViewModel()
            {
                PersonData = person,
                ProductData = product
            };

            return View(viewModel);
        }

        [Route("home/all-products")]
        public IActionResult All()
        {
            return View();
        }
    }
}
