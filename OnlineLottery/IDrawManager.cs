using System;
using System.Collections.Generic;

namespace OnlineLottery
{
    public class DrawNotOpenException : ApplicationException
    {
        public DrawNotOpenException() : base("Draw is not open") { }
    }

    public class WrongAmountOfNumbersException : ApplicationException
    {
        public WrongAmountOfNumbersException() : base("A ticket needs 6 numbers") { }
    }

    public class InvalidPurchaseException : ApplicationException
    {
        public InvalidPurchaseException() : base("Purchase declined") { }
    }

    public interface IDrawManager
    {
        IDraw GetDraw(DateTime date);
        IDraw CreateDraw(DateTime drawDate);
        void PurchaseTicket(DateTime drawDate, int playerId, int[] numbers, decimal value);
        void SettleDraw(DateTime drawDate, int[] results);
        IList<ITicket> GetOpenTickets(int playerId);
        decimal OperatorDeductionFactor { get; }
        IList<ITicket> GetTickets(DateTime drawDate, int playerId);
    }
}