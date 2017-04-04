using System;
using System.Linq;
using fit;
using fitlibrary;

namespace OnlineLottery.Test.PurchaseTicket
{
    public class SetUpTestEnvironment : ColumnFixture
    {
        internal static IPlayerManager PlayerManager;
        internal static IDrawManager DrawManager;

        public SetUpTestEnvironment()
        {
            PlayerManager = new PlayerManager();
            DrawManager = new DrawManager(PlayerManager);
        }

        public DateTime CreateDraw
        {
            set { DrawManager.CreateDraw(value); }
        }
    }

    public class PlayerRegisters : ColumnFixture
    {
        public class ExtendedPlayerRegistrationInfo : PlayerRegistrationInfo
        {
            public int PlayerId() => SetUpTestEnvironment.PlayerManager.RegisterPlayer(this);
        }

        private readonly ExtendedPlayerRegistrationInfo _extendedRegInfo = new ExtendedPlayerRegistrationInfo();
        public override object GetTargetObject() => _extendedRegInfo;
    }

    public class PurchaseTicket : DoFixture
    {
        private readonly IDrawManager _drawManager = SetUpTestEnvironment.DrawManager;
        private readonly IPlayerManager _playerManager = SetUpTestEnvironment.PlayerManager;

        public void PlayerDepositsDollarsWithCardAndExpiryDate(
            string username, decimal amount,
            string card, string expiry)
        {
            var pid = _playerManager.GetPlayer(username).PlayerId;
            _playerManager.DepositWithCard(pid, card, expiry, amount);
        }

        public bool PlayerHasDollars(string username, decimal amount) => _playerManager.GetPlayer(username).Balance == amount;

        public void PlayerBuysTicketsWithNumbersForDranOn(string username, int tickets, int[] numbers, DateTime drawDate)
        {
            PurchaseTickets(username, tickets, numbers, drawDate);
        }

        public void PlayerBuysATicketWithNumbersForDrawOn(string username, int[] numbers, DateTime drawDate)
        {
            PurchaseTickets(username, 1, numbers, drawDate);
        }

        private void PurchaseTickets(string username, decimal tickets, int[] numbers, DateTime drawDate)
        {
            const decimal ticketCost = 10;
            var pid = _playerManager.GetPlayer(username);
            _drawManager.PurchaseTicket(drawDate, pid.PlayerId, numbers, tickets * ticketCost);
        }

        public bool PoolValueForDrawOnIsDollars(DateTime drawDate, decimal value) => _drawManager.GetDraw(drawDate).TotalPoolSize == value;

        public bool TicketWithNumbersForDollarsIsRegisteredForPlayerForDrawOn(
            int[] numbers, decimal amount,
            string username, DateTime drawDate)
        {
            Array.Sort(numbers);
            var tickets = _drawManager.GetDraw(drawDate).Tickets;
            return tickets.Any(t => t.Holder.Username == username && t.Value == amount && t.Numbers.SequenceEqual(numbers));
        }

        public int TicketsInDrawOn(DateTime drawDate) => _drawManager.GetDraw(drawDate).Tickets.Length;

        public decimal PoolValueForDrawOnIs(DateTime drawDate) => _drawManager.GetDraw(drawDate).TotalPoolSize;

        public decimal AccountBalanceForIs(string username) => _playerManager.GetPlayer(username).Balance;
    }
}
