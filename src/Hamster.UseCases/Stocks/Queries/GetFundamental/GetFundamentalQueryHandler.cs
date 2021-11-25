using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Hamster.UseCases.Stocks.Queries.GetFundamental
{
    public interface IRevenueCalculator
    {
        int GetRevenue(string ticker);
    }

    public class RevenueCalculator : IRevenueCalculator
    {
        public int GetRevenue(string ticker)
        {
            throw new System.NotImplementedException();
        }
    }
    
    internal class GetFundamentalQueryHandler : IRequestHandler<GetFundamentalQuery, FundamentalDto>
    {
        public GetFundamentalQueryHandler(IRevenueCalculator revenueCalculator)
        {
            
        }
        public Task<FundamentalDto> Handle(GetFundamentalQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FundamentalDto{RevenueGrowth = request.Ticker.Length});
        }
    }
}