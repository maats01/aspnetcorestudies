using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using StockMarketSolution.Models;
using System.Security;

namespace StockMarketSolution.Controllers
{
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;

        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptions> options, IConfiguration configuration, IStocksService stocksService)
        {
            _finnhubService = finnhubService;
            _stocksService = stocksService;
            _tradingOptions = options.Value;
            _configuration = configuration;
        }

        [Route("/")]
        [Route("[action]")]
        [Route("~/[controller]")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(_tradingOptions.DefaultStockSymbol))
                _tradingOptions.DefaultStockSymbol = "MSFT";

            Dictionary<string, object>? companyProfile = await _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);

            Dictionary<string, object>? stockPriceQuote = await _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);

            StockTrade stockTrade = new StockTrade() { StockSymbol = _tradingOptions.DefaultStockSymbol };

            if (companyProfile["name"] != null && stockPriceQuote["c"] != null)
            {
                stockTrade.Price = Convert.ToDouble(stockPriceQuote["c"].ToString());
                stockTrade.StockName = companyProfile["name"].ToString();
            }

            ViewBag.FinnhubToken = _configuration["APIKey"];
            return View(stockTrade);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(buyOrderRequest);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade()
                {
                    StockSymbol = buyOrderRequest.StockSymbol,
                    StockName = buyOrderRequest.StockName,
                    Price = buyOrderRequest.Price,
                    Quantity = buyOrderRequest.Quantity
                };

                return View("Index", stockTrade);
            }

            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest? sellOrderRequest)
        {
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            ModelState.Clear();
            TryValidateModel(sellOrderRequest);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade()
                {
                    StockName = sellOrderRequest.StockName,
                    StockSymbol = sellOrderRequest.StockSymbol,
                    Price = sellOrderRequest.Price,
                    Quantity = sellOrderRequest.Quantity
                };

                return View("Index", stockTrade);
            }

            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("[controller]/[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            Orders orders = new Orders()
            {
                BuyOrders = await _stocksService.GetBuyOrders(),
                SellOrders = await _stocksService.GetSellOrders()
            };

            return View(orders);
        }

        [Route("[controller]/[action]")]
        [HttpGet]
        public async Task<IActionResult> OrdersPDF()
        {
            List<IOrderResponse> orders = new List<IOrderResponse>();

            orders.AddRange(await _stocksService.GetSellOrders());
            orders.AddRange(await _stocksService.GetBuyOrders());
            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            ViewBag.TradingOptions = _tradingOptions;
            
            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Bottom = 20, Left = 20, Right = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}
