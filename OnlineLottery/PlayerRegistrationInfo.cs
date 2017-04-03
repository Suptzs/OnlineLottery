﻿namespace OnlineLottery
{
    public class PlayerRegistrationInfo : IPlayerRegistrationInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}