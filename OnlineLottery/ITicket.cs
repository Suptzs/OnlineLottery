using System;

namespace OnlineLottery
{
    public interface ITicket
    {
        int[] Numbers { get; }
        IPlayerInfo Holder { get; }
        DateTime DrawDate { get; }
        decimal Value { get; }
        bool IsOpen { get; }
        decimal Winnings { get; }
    }
}