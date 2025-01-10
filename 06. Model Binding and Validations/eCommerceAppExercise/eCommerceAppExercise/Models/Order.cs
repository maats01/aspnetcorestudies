using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using eCommerceAppExercise.CustomValidators;

namespace eCommerceAppExercise.Models
{
    public class Order
    {
        [BindNever]
        public int? OrderNo { get; set; }

        [Required(ErrorMessage = "{0} can't be blank or null")]
        [MinimumDateValidator("2000-01-01")]
        public DateTime? OrderDate { get; set; }

        [Required(ErrorMessage = "{0} can't be blank or null")]
        [InvoicePriceValidator]
        public double? InvoicePrice { get; set; }

        [ProductListValidator]
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
