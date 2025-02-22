using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using Services.Helpers;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly StockMarketDbContext _db;
        public StocksService(StockMarketDbContext db)
        {
            _db = db;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ConvertToBuyOrder();
            buyOrder.BuyOrderId = Guid.NewGuid();
            
            _db.BuyOrders.Add(buyOrder);
            await _db.SaveChangesAsync();

            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ConvertToSellOrder();
            sellOrder.SellOrderId = Guid.NewGuid();
            
            _db.SellOrders.Add(sellOrder);
            await _db.SaveChangesAsync();

            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            return await _db.BuyOrders.Select(order => order.ToBuyOrderResponse()).ToListAsync();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            return await _db.SellOrders.Select(order => order.ToSellOrderResponse()).ToListAsync();
        }
    }
}
