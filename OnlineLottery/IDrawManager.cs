using System;

namespace OnlineLottery
{
    public class DrawNotOpenException : ApplicationException
    {
        public DrawNotOpenException() : base("Draw is not open") { }
    }

    public interface IDrawManager
    {
        IDraw GetDraw(DateTime date);
        IDraw CreateDraw(DateTime drawDate);
        void PurchaseTicket(DateTime drawDate, int playerId, int[] numbers, decimal value);
    }
}