using System;
using System.Collections.Generic;
using fitlibrary;

namespace OnlineLottery.Test
{
    public class ReviewTickets : DoFixture
    {
        private readonly IDrawManager _drawManager;
        private readonly IPlayerManager _playerManager;

        public ReviewTickets()
        {
            _playerManager = new PlayerManager();
            _drawManager = new DrawManager(_playerManager);
        }

        public void DrawOnIsOpen(DateTime drawDate) => _drawManager.CreateDraw(drawDate);

        public void PlayerOpensAccountWithDollars(string username, decimal balance)
            => _playerManager.AdjustBalance(_playerManager.RegisterPlayer(new PlayerRegistrationInfo
            {
                Username = username,
                Name = username,
                Password = "XXXXXX"
            }), balance);

        public void PlayerBuysATicketWithNumbersForDrawOn(string username, int[] numbers, DateTime drawDate)
            => PlayerBuysTicketsWithNumbersForDrawOn(username, 1, numbers, drawDate);

        public void PlayerBuysTicketsWithNumbersForDrawOn(string username, int tickets, int[] numbers,
            DateTime drawDate) => _drawManager.PurchaseTicket(drawDate, _playerManager.GetPlayer(username).PlayerId, numbers, 10 * tickets);

        public IList<ITicket> PlayerListsOpenTickets(string username)
            => _drawManager.GetOpenTickets(_playerManager.GetPlayer(username).PlayerId);

        public IList<ITicket> PlayerListsTicketsForDrawOn(string username, DateTime drawDate)
            => _drawManager.GetTickets(drawDate, _playerManager.GetPlayer(username).PlayerId);

        public void SettleDrawOn(DateTime drawDate) => _drawManager.SettleDraw(drawDate, new []{1,2,3,4,5,6});
    }
}
