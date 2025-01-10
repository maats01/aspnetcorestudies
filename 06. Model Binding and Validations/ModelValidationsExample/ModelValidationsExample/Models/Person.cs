using ModelValidationsExample.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace ModelValidationsExample.Models
{
    public class Person : IValidatableObject
    {
        [Required(ErrorMessage = "{0} can't be empty or null")]
        [Display(Name = "Person Name")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "{0} should be between {2} and {1} characters long")]
        public string? PersonName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }
        public string? Password { get; set; }

        [Compare("Password")]
        public string? ConfirmPassword { get; set; }

        [Range(0, 999.99, ErrorMessage = "{0} should be between ${1} and ${2}")]
        public double? Price { get; set; }

        [MinimumYearValidator(2005)]
        public DateTime? DateOfBirth { get; set; }
        public int? Age { get; set; }
        public DateTime? FromDate { get; set; }

        [DateRangeValidator("FromDate", ErrorMessage = "'From Date' should be older than 'To Date'")]
        public DateTime? ToDate { get; set; }

        public List<string>? Tags { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"Person object - Name: {PersonName}, Email: {Email}, Phone: {Phone}, Password: {Password}, ConfirmPassword: {ConfirmPassword}, Price: {Price}";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Age.HasValue == false && DateOfBirth.HasValue == false)
            {
                yield return new ValidationResult("Either date of birth or age must be supplied", new[] { nameof(Age) });
            }
        }
    }
}
