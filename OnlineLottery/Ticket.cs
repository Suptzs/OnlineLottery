using System;

namespace OnlineLottery
{
    public class Ticket : ITicket
    {
        public Ticket(IPlayerInfo player, DateTime drawDate, int[] numbers, decimal value)
        {
            Numbers = numbers;
            Holder = player;
            DrawDate = drawDate;
            Value = value;
            IsOpen = true;
            Winnings = 0;
        }

        public int[] Numbers { get; }
        public IPlayerInfo Holder { get; }
        public DateTime DrawDate { get; }
        public decimal Value { get; }
        public bool IsOpen { get; set; }
        public decimal Winnings { get; set; }
    }
}