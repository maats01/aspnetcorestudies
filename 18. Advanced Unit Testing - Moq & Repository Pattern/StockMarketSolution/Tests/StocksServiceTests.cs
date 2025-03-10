using Services;
using ServiceContracts;
using ServiceContracts.DTO;
using Xunit.Abstractions;
using Entities;
using Microsoft.EntityFrameworkCore;
using AutoFixture;
using RepositoryContracts;
using Moq;

namespace Tests
{
    public class StocksServiceTests
    {
        private readonly IStocksService _stocksService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;
        private readonly IStockRepository _stockRepository;
        private readonly Mock<IStockRepository> _stockRepositoryMock;

        public StocksServiceTests(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            _stockRepositoryMock = new Mock<IStockRepository>();
            _stockRepository = _stockRepositoryMock.Object;

            _stocksService = new StocksService(_stockRepository);
            _testOutputHelper = testOutputHelper;
        }

        #region CreateBuyOrder

        //When requesting a buy order by passing null as BuyOrderRequest, it should throw ArgumentException
        [Fact]
        public async Task CreateBuyOrder_NullBuyOrderRequest()
        {
            BuyOrderRequest? buyOrderRequest = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
            _stockRepositoryMock.Verify(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()), Times.Never);
        }

        //When requesting a buy order with a quantity number lower than 1, it should throw ArgumentException
        [Theory]
        [InlineData(0)]
        public async Task CreateBuyOrder_LowerThanMinQuantity(uint buyOrderQuantity)
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = buyOrderQuantity,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
            _stockRepositoryMock.Verify(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()), Times.Never);
        }

        //When requesting a buy order with a quantity number higher than 100000, it should throw ArgumentException
        [Theory]
        [InlineData(100001)]
        public async Task CreateBuyOrder_HigherThanMaxQuantity(uint buyOrderQuantity)
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = buyOrderQuantity,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
            _stockRepositoryMock.Verify(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()), Times.Never);
        }

        //When requesting a buy order with a price number lower than 1, it should throw ArgumentException
        [Theory]
        [InlineData(0)]
        public async Task CreateBuyOrder_LowerThanMinPrice(uint buyOrderPrice)
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = buyOrderPrice,
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
            _stockRepositoryMock.Verify(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()), Times.Never);
        }

        //When requesting a buy order with a price number higher than 10000, it should throw ArgumentException
        [Theory]
        [InlineData(10001)]
        public async Task CreateBuyOrder_HigherThanMaxPrice(uint buyOrderPrice)
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = buyOrderPrice,
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
            _stockRepositoryMock.Verify(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()), Times.Never);
        }

        //When requesting a buy order with null value as StockSymbol, it should throw ArgumentException
        [Fact]
        public async Task CreateBuyOrder_NullStockSymbol()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = null,
                StockName = "Microsoft",
                Price = 15,
                Quantity = 5,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
            _stockRepositoryMock.Verify(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()), Times.Never);
        }

        //When requesting a buy order with a dateAndTimeOfOrder with year lower than 2000, it should throw ArgumentException
        [Fact]
        public async Task CreateBuyOrder_InvalidDateAndTime()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 15,
                Quantity = 5,
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31")
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
            _stockRepositoryMock.Verify(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()), Times.Never);
        }

        //When requesting a buy order with all values correct, it should return the BuyOrderResponse with the newly generated ID
        [Fact]
        public async Task CreateBuyOrder_ValidDetails()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 15,
                Quantity = 5,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            BuyOrder buyOrder = buyOrderRequest.ConvertToBuyOrder();

            _stockRepositoryMock
                .Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

            Assert.NotEqual(Guid.Empty, buyOrderResponse.BuyOrderId);
            _stockRepositoryMock.Verify(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()), Times.Once);
        }

        #endregion


        #region CreateSellOrder

        //When requesting a sell order by passing null as SellOrderRequest, it should return ArgumentNullException
        [Fact]
        public async Task CreateSellOrder_NullSellOrderRequest()
        {
            SellOrderRequest? sellOrderRequest = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
            _stockRepositoryMock.Verify(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()), Times.Never);
        }

        //When requesting a sell order with a quantity lower than 1, it should return ArgumentException
        [Theory]
        [InlineData(0)]
        public async Task CreateSellOrder_LowerThanMinQuantity(uint sellOrderQuantity)
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = sellOrderQuantity,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
            _stockRepositoryMock.Verify(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()), Times.Never);
        }

        //When requesting a sell order with a quantity lower than 1, it should return ArgumentException
        [Theory]
        [InlineData(100001)]
        public async Task CreateSellOrder_HigherThanMaxQuantity(uint sellOrderQuantity)
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = sellOrderQuantity,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
            _stockRepositoryMock.Verify(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()), Times.Never);
        }

        //When requesting a sell order with a price lower than 1, it should return ArgumentException
        [Theory]
        [InlineData(0)]
        public async Task CreateSellOrder_LowerThanMinPrice(uint sellOrderPrice)
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = sellOrderPrice,
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
            _stockRepositoryMock.Verify(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()), Times.Never);
        }

        //When requesting a sell order with a price lower than 1, it should return ArgumentException
        [Theory]
        [InlineData(10001)]
        public async Task CreateSellOrder_HigherThanMaxPrice(uint sellOrderPrice)
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = sellOrderPrice,
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
            _stockRepositoryMock.Verify(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()), Times.Never);
        }

        //When requesting a sell order by passing null value in StockSymbol, it should return ArgumentException
        [Fact]
        public async Task CreateSellOrder_NullStockSymbol()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = null,
                StockName = "Microsoft",
                Price = 1,
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
            _stockRepositoryMock.Verify(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()), Times.Never);
        }

        //When requesting a sell order with a dateAndTimeOfOrder with year lower than 2000, it should throw ArgumentException
        [Fact]
        public async Task CreateSellOrder_InvalidDateAndTime()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31")
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
            _stockRepositoryMock.Verify(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()), Times.Never);
        }

        //When requesting a sell order with all valid values, it should return the SellOrderResponse with the newly generated ID
        [Fact]
        public async Task CreateSellOrder_ValidDetails()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            SellOrder sellOrder = sellOrderRequest.ConvertToSellOrder();

            _stockRepositoryMock
                .Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

            Assert.NotEqual(Guid.Empty, sellOrderResponse.SellOrderId);
            _stockRepositoryMock.Verify(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()), Times.Once);
        }

        #endregion


        #region GetBuyOrders

        //By default, the result List of GetBuyOrders should be empty
        [Fact]
        public async Task GetBuyOrders_EmptyList()
        {
            List<BuyOrder> buyOrders = new();
            _stockRepositoryMock
                .Setup(temp => temp.GetBuyOrders())
                .ReturnsAsync(buyOrders);

            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();

            Assert.Empty(buyOrderResponses);
        }

        //When you add a few buy orders, the return list of GetBuyOrders should contain the recently added orders
        [Fact]
        public async Task GetBuyOrders_AddFewBuyOrders()
        {
            BuyOrder? buyOrder1 = new BuyOrder()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = 5,
                DateAndTimeOfOrder = DateTime.Parse("2004-06-15")
            };

            BuyOrder? buyOrder2 = new BuyOrder()
            {
                StockSymbol = "AAPL",
                StockName = "Apple",
                Price = 10,
                Quantity = 20,
                DateAndTimeOfOrder = DateTime.Parse("2004-05-30")
            };

            BuyOrder? buyOrder3 = new BuyOrder()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 2,
                Quantity = 12,
                DateAndTimeOfOrder = DateTime.Parse("2004-05-25")
            };

            List<BuyOrder> buyOrders = new()
            {
                buyOrder1, buyOrder2, buyOrder3
            };

            List<BuyOrderResponse> buyOrderResponsesExpected = new()
            {
                buyOrder1.ToBuyOrderResponse(), buyOrder2.ToBuyOrderResponse(), buyOrder3.ToBuyOrderResponse()
            };

            _testOutputHelper.WriteLine("Expected:");
            foreach (BuyOrderResponse buyOrder in buyOrderResponsesExpected)
            {
                _testOutputHelper.WriteLine(buyOrder.ToString());
            }

            _stockRepositoryMock
                .Setup(temp => temp.GetBuyOrders())
                .ReturnsAsync(buyOrders);

            List<BuyOrderResponse> responseFromGetOrders = await _stocksService.GetBuyOrders();
            _testOutputHelper.WriteLine("Actual");
            foreach (BuyOrderResponse buyOrder in responseFromGetOrders)
            {
                _testOutputHelper.WriteLine(buyOrder.ToString());
            }

            foreach (BuyOrderResponse expected in buyOrderResponsesExpected)
            {
                Assert.Contains(expected, responseFromGetOrders);
            }
        }
        #endregion


        #region GetSellOrders

        //By default, the result List of GetSellOrders should be empty
        [Fact]
        public async Task GetSellOrders_EmptyList()
        {
            List<SellOrder> sellOrder = new();
            _stockRepositoryMock
                .Setup(temp => temp.GetSellOrders())
                .ReturnsAsync(sellOrder);
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();

            Assert.Empty(sellOrderResponses);
        }

        //When you add a few sell orders, the return list of GetSellOrders should contain the recently added orders
        [Fact]
        public async Task GetSellOrders_AddFewSellOrders()
        {
            SellOrder? sellOrder1 = new SellOrder()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = 5,
                DateAndTimeOfOrder = DateTime.Parse("2004-06-15")
            };

            SellOrder? sellOrder2 = new SellOrder()
            {
                StockSymbol = "AAPL",
                StockName = "Apple",
                Price = 10,
                Quantity = 20,
                DateAndTimeOfOrder = DateTime.Parse("2004-05-30")
            };

            SellOrder? sellOrder3 = new SellOrder()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 2,
                Quantity = 12,
                DateAndTimeOfOrder = DateTime.Parse("2004-05-25")
            };

            List<SellOrder> sellOrders = new()
            {
                sellOrder1, sellOrder2, sellOrder3
            };

            List<SellOrderResponse> sellOrderResponsesExpected = new()
            {
                sellOrder1.ToSellOrderResponse(), sellOrder2.ToSellOrderResponse(), sellOrder3.ToSellOrderResponse()
            };

            _testOutputHelper.WriteLine("Expected:");
            foreach (SellOrderResponse sellOrder in sellOrderResponsesExpected)
            {
                _testOutputHelper.WriteLine(sellOrder.ToString());
            }

            _stockRepositoryMock
                .Setup(temp => temp.GetSellOrders())
                .ReturnsAsync(sellOrders);

            List<SellOrderResponse> responseFromGetOrders = await _stocksService.GetSellOrders();
            _testOutputHelper.WriteLine("Actual");
            foreach (SellOrderResponse sellOrder in responseFromGetOrders)
            {
                _testOutputHelper.WriteLine(sellOrder.ToString());
            }

            foreach (SellOrderResponse expected in sellOrderResponsesExpected)
            {
                Assert.Contains(expected, responseFromGetOrders);
            }
        }
        #endregion
    }
}