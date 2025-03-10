﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StockMarketSolution.Models;

namespace StockMarketSolution.Controllers
{
    public class StocksController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;

        public StocksController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService, IStocksService stocksService)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
            _stocksService = stocksService;
        }

        [Route("/")]
        [Route("[controller]/[action]/{stock?}")]
        [HttpGet]
        public async Task<IActionResult> Explore(string? stock, bool showAll = false)
        {
            List<Dictionary<string, string>>? stocksDictionary = await _finnhubService.GetStocks();

            List<Stock> stocks = new List<Stock>();

            if (stocksDictionary != null)
            {
                if (!showAll && _tradingOptions.Top25PopularStocks != null)
                {
                    string[]? Top25PopularStocksList = _tradingOptions.Top25PopularStocks.Split(",");

                    if (Top25PopularStocksList != null)
                    {
                        stocksDictionary = stocksDictionary
                            .Where(temp => Top25PopularStocksList.Contains(Convert.ToString(temp["symbol"])))
                            .ToList();
                    }
                }

                stocks = stocksDictionary
                    .Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) })
                    .ToList();
            }

            ViewBag.stock = stock;
            return View(stocks);
        }
    }
}
