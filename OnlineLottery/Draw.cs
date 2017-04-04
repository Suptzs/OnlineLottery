using System;
using System.Collections.Generic;

namespace OnlineLottery
{
    public class Draw : IDraw
    {
        public Draw(DateTime drawDate)
        {
            DrawDate = drawDate;
            TotalPoolSize = 0;
            IsOpen = true;
            _tickets = new List<ITicket>();
        }

        public DateTime DrawDate { get; }
        public bool IsOpen { get; set; }
        public decimal TotalPoolSize { get; private set; }

        private readonly List<ITicket> _tickets;
        public ITicket[] Tickets => _tickets.ToArray();

        public void AddTicket(ITicket ticket)
        {
            _tickets.Add(ticket);
            TotalPoolSize += ticket.Value;
        }
    }
}