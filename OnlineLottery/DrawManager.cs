using System;
using System.Collections.Generic;

namespace OnlineLottery
{
    public class DrawManager : IDrawManager
    {
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

            var d = _draws[drawDate];
            var player = _playerManager.GetPlayer(playerId);
            _playerManager.AdjustBalance(playerId, -value);
            d.AddTicket(new Ticket(player, drawDate, numbers, value));
        }
    }
}