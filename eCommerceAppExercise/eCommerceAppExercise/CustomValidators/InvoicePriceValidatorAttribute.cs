using eCommerceAppExercise.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace eCommerceAppExercise.CustomValidators
{
    public class InvoicePriceValidatorAttribute : ValidationAttribute
    {
        public string DefaultErrorMessage = "Invoice price should be equal to the total cost of all products (i.e. {0}) in the order.";
        
        public InvoicePriceValidatorAttribute() 
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                PropertyInfo? otherProperty = validationContext.ObjectType.GetProperty(nameof(Order.Products));
                if (otherProperty != null)
                {
                    List<Product> products = (List<Product>) otherProperty.GetValue(validationContext.ObjectInstance)!;

                    double totalPrice = 0;
                    foreach (Product product in products)
                    {
                        totalPrice += product.Price * product.Quantity;
                    }

                    double actualPrice = (double) value;
                    if (totalPrice > 0)
                    {
                        if (actualPrice != totalPrice)
                        {
                            return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, totalPrice), new string[] {nameof(validationContext.MemberName)});
                        }
                    }
                    else
                    {
                        return new ValidationResult("No products found to validate invoice price.", new string[] {nameof(validationContext.MemberName)});
                    }

                    return ValidationResult.Success;
                }
            }

            return null;
        }
    }
}
