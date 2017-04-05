using System;
using System.Linq;
using fitlibrary;

namespace OnlineLottery.Test.PurchaseTicket
{
    public class SetUpTestEnvironment : DoFixture
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

        public void NewPlayerRegistersAndDepositsDollars(string username, decimal amount)
        {
            var pid = PlayerManager.RegisterPlayer(new PlayerRegistrationInfo {Username = username});
            PlayerManager.DepositWithCard(pid, "11111111", "01/01", amount);
        }
    }

    public class PurchaseTicket : DoFixture
    {
        private readonly IDrawManager _drawManager = SetUpTestEnvironment.DrawManager;
        private readonly IPlayerManager _playerManager = SetUpTestEnvironment.PlayerManager;

        public bool PlayerHasDollars(string username, decimal amount) => _playerManager.GetPlayer(username).Balance == amount;

        public void PlayerBuysTicketsWithNumbersForDrawOn(string username, int tickets, int[] numbers, DateTime drawDate)
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
