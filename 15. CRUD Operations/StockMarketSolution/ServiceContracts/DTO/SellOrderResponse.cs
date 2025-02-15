using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that represents a sell order to sell stocks - that can be used as a return type for Stock service
    /// </summary>
    public class SellOrderResponse
    {
        /// <summary>
        /// The unique ID of the buy order
        /// </summary>
        public Guid SellOrderId { get; set; }

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
        /// The total value of the sell order
        /// </summary>
        public double TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(SellOrderResponse))
                return false;

            SellOrderResponse objToCompare = (SellOrderResponse)obj;

            return (
                SellOrderId == objToCompare.SellOrderId
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
            return $"Sell Order ID: {SellOrderId}, Stock Symbol: {StockSymbol}, Stock Name: {StockName}\n" +
                $"Date and Time of Order: {DateAndTimeOfOrder.ToString()} Quantity: {Quantity} Price: {Price}\n" +
                $"Trade Amount: {TradeAmount}";
        }
    }

    public static class SellOrderExtensions
    {
        /// <summary>
        /// An extension method to convert an object of SellOrder class into SellOrderResponse class
        /// </summary>
        /// <param name="sellOrder">The SellOrder object to convert</param>
        /// <returns>The converted object of type SellOrderResponse</returns>
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse()
            {
                SellOrderId = sellOrder.SellOrderId,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
                Price = sellOrder.Price,
                TradeAmount = sellOrder.Price * sellOrder.Quantity
            };
        }
    }
}
