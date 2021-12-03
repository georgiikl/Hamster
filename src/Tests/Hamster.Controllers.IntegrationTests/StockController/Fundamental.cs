using System.Threading;
using Autofac;
using Xunit;

using Hamster.Controllers.IntegrationTests.Arrange;

namespace Hamster.Controllers.IntegrationTests.StockController
{
    public class Fundamental
    {
        [Fact]
        public async void All_fundamental_indicators_should_not_be_null()
        {
            // Arrange
            var controller = Arranger.BuildContainer().Resolve<Controllers.StockController>();
            
            // Act
            const string ticker = "PYPL";
            var dto = await controller.Fundamental(ticker, CancellationToken.None);
            
            // Assert
            Assert.NotNull(dto);
            Assert.NotNull(dto.RevenueGrowth);
            Assert.NotNull(dto.OperatingIncomeGrowth);
            Assert.NotNull(dto.NetIncomeGrowth);
            /*Assert.NotNull(dto.GrossMargin);
            Assert.NotNull(dto.OperatingMargin);
            Assert.NotNull(dto.NetProfitMargin);
            Assert.NotNull(dto.Roe);
            Assert.NotNull(dto.Pe);
            Assert.NotNull(dto.Ps);
            Assert.NotNull(dto.EvEbitda);*/
        }
    }
}