namespace OnlineLottery.Test
{
    public class PrizeDistributionForPayoutPool : fit.ColumnFixture
    {
        private readonly WinningsCalculator _wc = new WinningsCalculator();

        public int WinningCombination;
        public decimal? PayoutPool;

        public int PoolPercentage()
        {
            return _wc.GetPoolPercentage(WinningCombination);
        }

        public decimal PrizePool()
        {
            if (PayoutPool == null) PayoutPool = decimal.Parse(Args[0]);
            return _wc.GetPrizePool(WinningCombination, PayoutPool.Value);
        }
    }
}
