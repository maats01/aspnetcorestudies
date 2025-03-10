using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Interface that represents the business logic of stocks service
    /// </summary>
    public interface IStocksService
    {
        /// <summary>
        /// Inserts a new buy order into the database table called 'BuyOrders'.
        /// </summary>
        /// <param name="buyOrderRequest">BuyOrderRequest object that contains details of the buy order</param>
        /// <returns>Returns the newly generated buy order object as SellOrderResponse</returns>
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

        /// <summary>
        /// Inserts a new sell order into the database table called 'SellOrders'.
        /// </summary>
        /// <param name="sellOrderRequest">SellOrderRequest object that contains details of the sell order</param>
        /// <returns>Returns the newly generated sell order object as SellOrderResponse</returns>
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

        /// <summary>
        /// Returns a list of buy orders
        /// </summary>
        /// <returns>Returns a List of BuyOrderResponse</returns>
        Task<List<BuyOrderResponse>> GetBuyOrders();

        /// <summary>
        /// Returns a list of sell orders
        /// </summary>
        /// <returns>Returns a List of SellOrdersResponse</returns>
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
