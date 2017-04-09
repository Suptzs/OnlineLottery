using System;
using System.Collections.Generic;
using System.Linq;
using fit;
using fitlibrary;

namespace OnlineLottery.Test
{
    public class SettlementTest : DoFixture
    {
        private readonly IDrawManager _drawManager;
        private readonly IPlayerManager _playerManager;
        private readonly DateTime _drawDate;

        public SettlementTest()
        {
            _playerManager = new PlayerManager();
            _drawManager = new DrawManager(_playerManager);
            _drawDate = DateTime.Now;
            _drawManager.CreateDraw(_drawDate);
        }

        public Fixture AccountsBeforeTheDraw() => new CreatePlayerFixture(_playerManager);

        public Fixture TicketsInTheDraw() => new TicketPurchaseFixture(_playerManager, _drawManager, _drawDate);

        public void DrawResultsAre(int[] numbers) => _drawManager.SettleDraw(_drawDate, numbers);

        public Fixture PrizeDistribution() => new PrizePoolDistribution(_drawManager, _drawDate);

        public Fixture AccountsAfterTheDraw() => new BalanceCheckFixture(_playerManager);
    }

    internal class CreatePlayerFixture : SetUpFixture
    {
        private readonly IPlayerManager _playerManager;

        public CreatePlayerFixture(IPlayerManager pm)
        {
            _playerManager = pm;
        }

        public void PlayerBalance(string player, decimal balance)
        {
            var playerId = _playerManager.RegisterPlayer(new PlayerRegistrationInfo
            {
                Username = player,
                Name = player,
                Password = "XXXXXX"
            });
            _playerManager.AdjustBalance(playerId, balance);
        }
    }

    internal class TicketPurchaseFixture : SetUpFixture
    {
        private readonly IDrawManager _drawManager;
        private readonly DateTime _drawDate;
        private readonly IPlayerManager _playerManager;

        public TicketPurchaseFixture(IPlayerManager pm, IDrawManager dm, DateTime drawDate)
        {
            _drawManager = dm;
            _drawDate = drawDate;
            _playerManager = pm;
        }

        public void PlayerNumbersValue(string player, int[] numbers, decimal value)
        {
            _drawManager.PurchaseTicket(_drawDate, _playerManager.GetPlayer(player).PlayerId, numbers, value);
        }
    }

    internal class PrizePoolDistribution : ColumnFixture
    {
        private readonly IDrawManager _drawManager;
        private readonly DateTime _drawDate;

        public PrizePoolDistribution(IDrawManager dm, DateTime drawDate)
        {
            _drawManager = dm;
            _drawDate = drawDate;
        }

        public int WinningTickets() => _drawManager.GetDraw(_drawDate).Tickets.Count(ticket => ticket.Winnings != 0);

        public decimal[] PrizeMoney()
        {
            var prizes = _drawManager.GetDraw(_drawDate).Tickets.Where(ticket => ticket.Winnings > 0).Select(ticket => ticket.Winnings).ToArray();
            Array.Sort(prizes);
            return prizes;
        }
    }

    internal class BalanceCheckFixture : ColumnFixture
    {
        private readonly IPlayerManager _playerManager;

        public BalanceCheckFixture(IPlayerManager pm)
        {
            _playerManager = pm;
        }

        public string Player;
        public decimal Balance => _playerManager.GetPlayer(Player).Balance;
    }
}
