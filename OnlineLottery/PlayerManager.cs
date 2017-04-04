using System.Collections.Generic;

namespace OnlineLottery
{
    public class PlayerManager : IPlayerManager
    {
        private readonly Dictionary<int, PlayerInfo> _players = new Dictionary<int, PlayerInfo>();
        private readonly Dictionary<string, PlayerInfo> _playersByUsername = new Dictionary<string, PlayerInfo>();

        public int RegisterPlayer(IPlayerRegistrationInfo p)
        {
            if(_playersByUsername.ContainsKey(p.Username)) throw new DuplicateUsernameException();

            var np = new PlayerInfo(p);
            _players.Add(np.PlayerId, np);
            _playersByUsername.Add(np.Username, np);
            return np.PlayerId;
        }

        public IPlayerInfo GetPlayer(int id)
        {
            return _players[id];
        }

        public IPlayerInfo GetPlayer(string username)
        {
            return _playersByUsername[username];
        }

        public int LogIn(string username, string password)
        {
            if(!_playersByUsername.ContainsKey(username)) throw new UnknownPlayerException();

            var p = _playersByUsername[username];
            if(p.Password == password) return p.PlayerId;
            throw new InvalidPasswordException();
        }

        public void AdjustBalance(int playerId, decimal amount)
        {
            var playerInfo = _players[playerId];
            if(PlayerHasNotEnoughFunds(playerInfo, amount))
                throw new NotEnoughFundsException();
            playerInfo.Balance += amount;
        }

        private static bool PlayerHasNotEnoughFunds(IPlayerInfo playerInfo, decimal amount)
        {
            return amount < 0 && playerInfo.Balance < -amount;
        }

        public void DepositWithCard(int playerId, string cardNumber, string expiryDate, decimal amount)
        {
            if(cardNumber.EndsWith("2")) throw new TransactionDeclinedException();

            _players[playerId].Balance += amount;
        }
    }
}