using System;
using System.Collections.Generic;
using System.Linq;
using fitSharp.Fit.Model;

namespace OnlineLottery
{
    public class DrawManager : IDrawManager
    {
        public decimal OperatorDeductionFactor => 0.5m;

        private readonly IPlayerManager _playerManager;
        private readonly Dictionary<DateTime, Draw> _draws = new Dictionary<DateTime, Draw>();

        public DrawManager(IPlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public IDraw GetDraw(DateTime date)
        {
            return _draws[date];
        }

        public IDraw CreateDraw(DateTime drawDate)
        {
            var draw = new Draw(drawDate);
            _draws.Add(drawDate, draw);
            return draw;
        }

        public void PurchaseTicket(DateTime drawDate, int playerId, int[] numbers, decimal value)
        {
            if (!_draws.ContainsKey(drawDate)) throw new DrawNotOpenException();
            if (numbers.Length != 6) throw new WrongAmountOfNumbersException();
            if (value < 0) throw new InvalidPurchaseException();

            var d = _draws[drawDate];
            var player = _playerManager.GetPlayer(playerId);
            _playerManager.AdjustBalance(playerId, -value);
            d.AddTicket(new Ticket(player, drawDate, numbers, value));
        }

        public IList<ITicket> GetOpenTickets(int playerId)
            => (from draw in _draws
                from ticket in draw.Value.Tickets
                where ticket.Holder.PlayerId == playerId && ticket.IsOpen
                select ticket).ToList();

        public IList<ITicket> GetTickets(DateTime drawDate, int playerId)
            => (from draw in _draws
                from ticket in draw.Value.Tickets
                where ticket.Holder.PlayerId == playerId && ticket.DrawDate == drawDate
                select ticket).ToList();

        public void SettleDraw(DateTime drawDate, int[] results)
        {
            var d = _draws[drawDate];
            d.IsOpen = false;
            var ticketCategories = SplitTicketsIntoCategories(results, d.Tickets);
            for (var commonNumbers = 0; commonNumbers <= results.Length; commonNumbers++)
                SettleTicketCategories(commonNumbers, d, ticketCategories[commonNumbers]);
        }

        private void SettleTicketCategories(int commonNumbers, IDraw draw, List<ITicket> tickets)
        {
            var prizePool = new WinningsCalculator().GetPrizePool(commonNumbers, draw.TotalPoolSize * (1 - OperatorDeductionFactor));
            var totalTicketValue = GetTotalTicketValue(tickets);
            foreach (Ticket t in tickets)
                SettleTicket(t, prizePool, totalTicketValue);
        }

        private void SettleTicket(Ticket t, decimal prizePool, decimal totalTicketValue)
        {
            t.IsOpen = false;
            if (prizePool <= 0) return;

            t.Winnings = t.Value * prizePool / totalTicketValue;
            _playerManager.AdjustBalance(t.Holder.PlayerId, t.Winnings);
        }

        private static Dictionary<int, List<ITicket>> SplitTicketsIntoCategories(int[] results, IEnumerable<ITicket> tickets)
        {
            var ticketCategories = new Dictionary<int, List<ITicket>>();

            for (var i = 0; i <= results.Length; i++)
                ticketCategories[i] = new List<ITicket>();

            foreach(var t in tickets)
                ticketCategories[CountCommonElements(t.Numbers, results)].Add(t);

            return ticketCategories;
        }

        private static int CountCommonElements(IEnumerable<int> ticketNumbers, int[] results)
            => ticketNumbers.Sum(ticketNumber => results.Count(result => result == ticketNumber));

        private static decimal GetTotalTicketValue(IEnumerable<ITicket> tickets) => tickets.Sum(t => t.Value);
    }
}