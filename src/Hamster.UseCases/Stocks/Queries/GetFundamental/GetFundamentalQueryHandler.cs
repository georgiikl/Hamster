using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Hamster.UseCases.Stocks.Queries.GetFundamental
{
    internal class GetFundamentalQueryHandler : IRequestHandler<GetFundamentalQuery, FundamentalDto>
    {
        public Task<FundamentalDto> Handle(GetFundamentalQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FundamentalDto());
        }
    }
}