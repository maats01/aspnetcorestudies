using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Represents a buy order to purchase the stocks
    /// </summary>
    public class BuyOrder
    {
        /// <summary>
        /// The unique id of buy order
        /// </summary>
        public Guid BuyOrderId { get; set; }

        /// <summary>
        /// The symbol of the stock
        /// </summary>

        [Required(ErrorMessage = "StockSymbol required")]
        public string? StockSymbol { get; set; }

        /// <summary>
        /// The name of the stock
        /// </summary>
        [Required(ErrorMessage = "StockName required")]
        public string? StockName { get; set; }

        /// <summary>
        /// The date and time of the order, when it was placed by the user
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
    }
}
