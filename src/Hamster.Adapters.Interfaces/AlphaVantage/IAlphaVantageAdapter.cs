using System.Threading;
using System.Threading.Tasks;

namespace Hamster.Adapters.Interfaces.AlphaVantage
{
    public interface IAlphaVantageAdapter
    {
        Task<IncomeStatementDto> GetIncomeStatementAsync(string ticker, CancellationToken cancellationToken);
    }
}