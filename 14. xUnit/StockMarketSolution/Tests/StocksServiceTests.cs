using Services;
using ServiceContracts;
using ServiceContracts.DTO;
using Xunit.Abstractions;

namespace Tests
{
    public class StocksServiceTests
    {
        private readonly IStocksService _stocksService;
        private readonly ITestOutputHelper _testOutputHelper;

        public StocksServiceTests(ITestOutputHelper testOutputHelper)
        {
            _stocksService = new StocksService();
            _testOutputHelper = testOutputHelper;
        }

        #region CreateBuyOrder

        //When requesting a buy order by passing null as BuyOrderRequest, it should throw ArgumentException
        [Fact]
        public void CreateBuyOrder_NullBuyOrderRequest()
        {
            BuyOrderRequest? buyOrderRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When requesting a buy order with a quantity number lower than 1, it should throw ArgumentException
        [Theory]
        [InlineData(0)]
        public void CreateBuyOrder_LowerThanMinQuantity(uint buyOrderQuantity)
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = buyOrderQuantity,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When requesting a buy order with a quantity number higher than 100000, it should throw ArgumentException
        [Theory]
        [InlineData(100001)]
        public void CreateBuyOrder_HigherThanMaxQuantity(uint buyOrderQuantity)
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = buyOrderQuantity,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When requesting a buy order with a price number lower than 1, it should throw ArgumentException
        [Theory]
        [InlineData(0)]
        public void CreateBuyOrder_LowerThanMinPrice(uint buyOrderPrice)
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = buyOrderPrice,
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When requesting a buy order with a price number higher than 10000, it should throw ArgumentException
        [Theory]
        [InlineData(10001)]
        public void CreateBuyOrder_HigherThanMaxPrice(uint buyOrderPrice)
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = buyOrderPrice,
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When requesting a buy order with null value as StockSymbol, it should throw ArgumentException
        [Fact]
        public void CreateBuyOrder_NullStockSymbol()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = null,
                StockName = "Microsoft",
                Price = 15,
                Quantity = 5,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When requesting a buy order with a dateAndTimeOfOrder with year lower than 2000, it should throw ArgumentException
        [Fact]
        public void CreateBuyOrder_InvalidDateAndTime()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 15,
                Quantity = 5,
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31")
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When requesting a buy order with all values correct, it should return the BuyOrderResponse with the newly generated ID
        [Fact]
        public void CreateBuyOrder_ValidDetails()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 15,
                Quantity = 5,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            BuyOrderResponse buyOrderResponse = _stocksService.CreateBuyOrder(buyOrderRequest);

            Assert.NotEqual(Guid.Empty, buyOrderResponse.BuyOrderId);
        }

        #endregion


        #region CreateSellOrder

        //When requesting a sell order by passing null as SellOrderRequest, it should return ArgumentNullException
        [Fact]
        public void CreateSellOrder_NullSellOrderRequest()
        {
            SellOrderRequest? sellOrderRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When requesting a sell order with a quantity lower than 1, it should return ArgumentException
        [Theory]
        [InlineData(0)]
        public void CreateSellOrder_LowerThanMinQuantity(uint sellOrderQuantity)
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = sellOrderQuantity,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When requesting a sell order with a quantity lower than 1, it should return ArgumentException
        [Theory]
        [InlineData(100001)]
        public void CreateSellOrder_HigherThanMaxQuantity(uint sellOrderQuantity)
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = sellOrderQuantity,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When requesting a sell order with a price lower than 1, it should return ArgumentException
        [Theory]
        [InlineData(0)]
        public void CreateSellOrder_LowerThanMinPrice(uint sellOrderPrice)
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = sellOrderPrice,
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When requesting a sell order with a price lower than 1, it should return ArgumentException
        [Theory]
        [InlineData(10001)]
        public void CreateSellOrder_HigherThanMaxPrice(uint sellOrderPrice)
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = sellOrderPrice,
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When requesting a sell order by passing null value in StockSymbol, it should return ArgumentException
        [Fact]
        public void CreateSellOrder_NullStockSymbol()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = null,
                StockName = "Microsoft",
                Price = 1,
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When requesting a sell order with a dateAndTimeOfOrder with year lower than 2000, it should throw ArgumentException
        [Fact]
        public void CreateSellOrder_InvalidDateAndTime()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31")
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When requesting a sell order with all valid values, it should return the SellOrderResponse with the newly generated ID
        [Fact]
        public void CreateSellOrder_ValidDetails()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            SellOrderResponse sellOrderResponse = _stocksService.CreateSellOrder(sellOrderRequest);

            Assert.NotEqual(Guid.Empty, sellOrderResponse.SellOrderId);
        }

        #endregion


        #region GetBuyOrders

        //By default, the result List of GetBuyOrders should be empty
        [Fact]
        public void GetBuyOrders_EmptyList()
        {
            List<BuyOrderResponse> buyOrderResponses = _stocksService.GetBuyOrders();

            Assert.Empty(buyOrderResponses);
        }

        //When you add a few buy orders, the return list of GetBuyOrders should contain the recently added orders
        [Fact]
        public void GetBuyOrders_AddFewBuyOrders()
        {
            BuyOrderRequest? buyOrderRequest1 = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = 5,
                DateAndTimeOfOrder = DateTime.Parse("2004-06-15")
            };

            BuyOrderRequest? buyOrderRequest2 = new BuyOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple",
                Price = 10,
                Quantity = 20,
                DateAndTimeOfOrder = DateTime.Parse("2004-05-30")
            };

            BuyOrderRequest? buyOrderRequest3 = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 2,
                Quantity = 12,
                DateAndTimeOfOrder = DateTime.Parse("2004-05-25")
            };

            List<BuyOrderRequest> buyOrderRequests = new List<BuyOrderRequest>()
            {
                buyOrderRequest1, buyOrderRequest2, buyOrderRequest3
            };

            List<BuyOrderResponse> buyOrderResponses = new List<BuyOrderResponse>();

            foreach (BuyOrderRequest request in buyOrderRequests)
            {
                buyOrderResponses.Add(_stocksService.CreateBuyOrder(request));
            }

            _testOutputHelper.WriteLine("Expected:");
            foreach (BuyOrderResponse buyOrder in buyOrderResponses)
            {
                _testOutputHelper.WriteLine(buyOrder.ToString());
            }

            List<BuyOrderResponse> responseFromGetOrders = _stocksService.GetBuyOrders();
            _testOutputHelper.WriteLine("Actual");
            foreach (BuyOrderResponse buyOrder in responseFromGetOrders)
            {
                _testOutputHelper.WriteLine(buyOrder.ToString());
            }

            foreach (BuyOrderResponse expected in buyOrderResponses)
            {
                Assert.Contains(expected, responseFromGetOrders);
            }
        }
        #endregion


        #region GetSellOrders

        //By default, the result List of GetSellOrders should be empty
        [Fact]
        public void GetSellOrders_EmptyList()
        {
            List<SellOrderResponse> sellOrderResponses = _stocksService.GetSellOrders();

            Assert.Empty(sellOrderResponses);
        }

        //When you add a few sell orders, the return list of GetSellOrders should contain the recently added orders
        [Fact]
        public void GetSellOrders_AddFewSellOrders()
        {
            SellOrderRequest? sellOrderRequest1 = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = 5,
                DateAndTimeOfOrder = DateTime.Parse("2004-06-15")
            };

            SellOrderRequest? sellOrderRequest2 = new SellOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple",
                Price = 10,
                Quantity = 20,
                DateAndTimeOfOrder = DateTime.Parse("2004-05-30")
            };

            SellOrderRequest? sellOrderRequest3 = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 2,
                Quantity = 12,
                DateAndTimeOfOrder = DateTime.Parse("2004-05-25")
            };

            List<SellOrderRequest> sellOrderRequests = new List<SellOrderRequest>()
            {
                sellOrderRequest1, sellOrderRequest2, sellOrderRequest3
            };

            List<SellOrderResponse> sellOrderResponses = new List<SellOrderResponse>();

            foreach (SellOrderRequest request in sellOrderRequests)
            {
                sellOrderResponses.Add(_stocksService.CreateSellOrder(request));
            }

            _testOutputHelper.WriteLine("Expected:");
            foreach (SellOrderResponse sellOrder in sellOrderResponses)
            {
                _testOutputHelper.WriteLine(sellOrder.ToString());
            }

            List<SellOrderResponse> responseFromGetOrders = _stocksService.GetSellOrders();
            _testOutputHelper.WriteLine("Actual");
            foreach (SellOrderResponse sellOrder in responseFromGetOrders)
            {
                _testOutputHelper.WriteLine(sellOrder.ToString());
            }

            foreach (SellOrderResponse expected in sellOrderResponses)
            {
                Assert.Contains(expected, responseFromGetOrders);
            }
        }
        #endregion
    }
}