using System.ComponentModel.DataAnnotations;
using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that represents a buy order to purchase the stocks - that can be used while inserting/updating
    /// </summary>
    public class BuyOrderRequest : IValidatableObject
    {
        /// <summary>
        /// The unique symbol of the stock
        /// </summary>
        [Required(ErrorMessage = "StockSymbol required")]
        public string? StockSymbol { get; set; }

        /// <summary>
        /// The company name of the stock
        /// </summary>
        [Required(ErrorMessage = "StockName required")]
        public string? StockName { get; set; }

        /// <summary>
        /// Date and time of order, when it is placed by the user
        /// </summary>
        public DateTime DateAndTimeOfOrder { get; set; }

        /// <summary>
        /// The number of stocks (shares) to buy
        /// </summary>
        [Range(1, 100000, ErrorMessage = "Quantity should be between 1 and 100000")]
        public uint Quantity { get; set; }

        /// <summary>
        /// The price of each stock (share)
        /// </summary>
        [Range(1, 10000, ErrorMessage = "Price should be between 1 and 10000")]
        public double Price { get; set; }

        /// <summary>
        /// Converts the BuyOrderRequest into a BuyOrder object
        /// </summary>
        /// <returns>The converted BuyOrder object</returns>
        public BuyOrder ConvertToBuyOrder()
        {
            return new BuyOrder()
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity,
                Price = Price
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (DateAndTimeOfOrder < Convert.ToDateTime("2000-01-01"))
                results.Add(new ValidationResult("Date of the order should not be older than Jan 01, 2000"));

            return results;
        }
    }
}
