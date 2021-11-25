namespace Hamster.UseCases.Stocks.Queries.GetFundamental
{
    public class FundamentalDto
    {
        // Финансовые потоки
        
        /// <summary>
        /// Рост выручки, в процентах
        /// </summary>
        public int RevenueGrowth { get; set; }

        /// <summary>
        /// Рост операционной прибыли, в процентах
        /// </summary>
        public int OperatingProfitGrowth { get; set; }

        /// <summary>
        /// Рост чистой прибыли, в процентах
        /// </summary>
        public int NetProfitGrowth { get; set; }

        // Маржинальность бизнеса
        
        /// <summary>
        /// Валовая маржинальность (валовая прибыль)
        /// </summary>
        public int GrossMargin { get; set; }

        /// <summary>
        /// Операционная маржинальность
        /// </summary>
        public int OperatingMargin { get; set; }

        /// <summary>
        /// Маржинальность по чистой прибыли (коэффициент прибыльности)
        /// </summary>
        public int NetProfitMargin { get; set; }

        /// <summary>
        /// ROE (прибыль на инвестиции)
        /// </summary>
        public int Roe { get; set; }

        // Мультипликаторы
        
        /// <summary>
        /// P/E (коэффициент цена/прибыль)
        /// </summary>
        public int Pe { get; set; }

        /// <summary>
        /// P/S (коэффициент цена/объём продаж)
        /// </summary>
        public int Ps { get; set; }

        /// <summary>
        /// EV/EBITDA
        /// </summary>
        public int EvEbitda { get; set; }
    }
}