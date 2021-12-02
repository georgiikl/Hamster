namespace Hamster.Adapters.Interfaces.AlphaVantage
{
    public class IncomeStatementDto
    {
        public string Symbol { get; set; }
        public IncomeStatementItemDto[] AnnualReports { get; set; }
        public IncomeStatementItemDto[] QuarterlyReports { get; set; }
    }
}