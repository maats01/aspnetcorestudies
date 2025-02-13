using Entities;
using System.Runtime.CompilerServices;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that represents a buy order to purchase stocks - that can be used as a return type for Stock service
    /// </summary>
    public class BuyOrderResponse
    {
        /// <summary>
        /// The unique ID of the buy order
        /// </summary>
        public Guid BuyOrderId { get; set; }

        /// <summary>
        /// The unique symbol of the stock
        /// </summary>
        public string? StockSymbol { get; set; }

        /// <summary>
        /// The company name of the stock
        /// </summary>
        public string? StockName { get; set; }

        /// <summary>
        /// Date and time of order, when it is placed by the user
        /// </summary>
        public DateTime DateAndTimeOfOrder { get; set; }

        /// <summary>
        /// The number of stocks (shares) to buy
        /// </summary>
        public uint Quantity { get; set; }

        /// <summary>
        /// The price of each stock (share)
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// The total value of the buy order
        /// </summary>
        public double TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(BuyOrderResponse))
                return false;

            BuyOrderResponse objToCompare = (BuyOrderResponse)obj;

            return (
                BuyOrderId == objToCompare.BuyOrderId
                && StockSymbol == objToCompare.StockSymbol
                && StockName == objToCompare.StockName
                && DateAndTimeOfOrder == objToCompare.DateAndTimeOfOrder
                && Quantity == objToCompare.Quantity
                && Price == objToCompare.Price
                && TradeAmount == objToCompare.TradeAmount
                );
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Sell Order ID: {BuyOrderId}, Stock Symbol: {StockSymbol}, Stock Name: {StockName}\n" +
                $"Date and Time of Order: {DateAndTimeOfOrder.ToString()} Quantity: {Quantity} Price: {Price}\n" +
                $"Trade Amount: {TradeAmount}";
        }
    }

    public static class BuyOrderExtensions
    {
        /// <summary>
        /// An extension method that converts an object of BuyOrder class into BuyOrderResponse class
        /// </summary>
        /// <param name="buyOrder">The BuyOrder object type to convert</param>
        /// <returns>The converted object of the type BuyOrderResponse</returns>
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse()
            {
                BuyOrderId = buyOrder.BuyOrderId,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                Price = buyOrder.Price,
                TradeAmount = buyOrder.Quantity * buyOrder.Price
            };
        }
    }
}
