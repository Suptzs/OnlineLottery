namespace OnlineLottery
{
    public class PlayerRegistrationInfo : IPlayerRegistrationInfo
    {
        public string Name { get; }
        public string Address { get; }
        public string City { get; }
        public string PostCode { get; }
        public string Country { get; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}