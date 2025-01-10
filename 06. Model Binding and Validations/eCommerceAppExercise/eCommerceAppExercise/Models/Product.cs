using System.ComponentModel.DataAnnotations;

namespace eCommerceAppExercise.Models
{
    public class Product
    {
        [Required(ErrorMessage = "{0} can't be blank or null")]
        [Range(1, 100, ErrorMessage = "{0} should be between a valid number (1-100)")]
        public int ProductCode { get; set; }

        [Required(ErrorMessage = "{0} can't be blank or null")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} should be between a valid number")]
        public double Price { get; set; }

        [Required(ErrorMessage = "{0} can't be blank or null")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} should be between a valid number")]
        public int Quantity { get; set; }
    }
}
