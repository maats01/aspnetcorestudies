﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using StocksApp.Services;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _options;


        public HomeController(FinnhubService myService, IOptions<TradingOptions> options)
        {
            _finnhubService = myService;
            _options = options;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (_options.Value.DefaultStockSymbol == null)
            {
                _options.Value.DefaultStockSymbol = "MSFT";
            }

            Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(_options.Value.DefaultStockSymbol);

            Stock stock = new Stock()
            {
                StockSymbol = _options.Value.DefaultStockSymbol,
                CurrentPrice = Convert.ToDouble(responseDictionary["c"].ToString()),
                HighestPrice = Convert.ToDouble(responseDictionary["h"].ToString()),
                LowestPrice = Convert.ToDouble(responseDictionary["l"].ToString()),
                OpenPrice = Convert.ToDouble(responseDictionary["o"].ToString()),
            };

            return View(stock);
        }
    }
}
