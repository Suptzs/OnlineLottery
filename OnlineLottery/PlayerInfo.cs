namespace OnlineLottery
{
    public class PlayerInfo : IPlayerInfo
    {
        private static int _nextId = 1;

        public PlayerInfo(IPlayerRegistrationInfo reg)
        {
            Name = reg.Name;
            Address = reg.Address;
            City = reg.City;
            PostCode = reg.PostCode;
            Country = reg.Country;
            Username = reg.Username;
            Balance = 0;
            PlayerId = _nextId++;
            Username = reg.Username;
            Password = reg.Password;
        }

        internal string Password { get; private set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string Username { get; set; }
        public decimal Balance { get; set; }
        public int PlayerId { get; set; }
        public bool IsVerified { get; set; }
    }
}