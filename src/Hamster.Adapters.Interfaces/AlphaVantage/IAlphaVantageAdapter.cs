using System.Threading;
using System.Threading.Tasks;

namespace Hamster.Adapters.Interfaces.AlphaVantage
{
    public interface IAlphaVantageAdapter
    {
        Task<IncomeStatementDto> GetIncomeStatement(string ticker, CancellationToken cancellationToken);
    }
}