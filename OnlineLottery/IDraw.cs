using System;

namespace OnlineLottery
{
    public interface IDraw
    {
        DateTime DrawDate { get; }
        bool IsOpen { get; }
        decimal TotalPoolSize { get; }
        ITicket [] Tickets { get; }
        void AddTicket(ITicket ticket);
    }
}
