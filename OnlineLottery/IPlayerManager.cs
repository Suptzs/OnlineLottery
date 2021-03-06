﻿using System;

namespace OnlineLottery
{
    public class UnknownPlayerException : ApplicationException
    {
        public UnknownPlayerException() : base("Unknown user") { }
    }
    public class InvalidPasswordException : ApplicationException
    {
        public InvalidPasswordException() : base("Invalid password") { }
    }
    public class DuplicateUsernameException : ApplicationException
    {
        public DuplicateUsernameException() : base("Duplicate username") { }
    }
    public class NotEnoughFundsException : ApplicationException
    {
        public NotEnoughFundsException() : base("Not enough funds") { }
    }
    public class TransactionDeclinedException : ApplicationException
    {
        public TransactionDeclinedException() : base("Transaction declined") { }
    }

    public interface IPlayerManager
    {
        int RegisterPlayer(IPlayerRegistrationInfo p);
        IPlayerInfo GetPlayer(int id);
        IPlayerInfo GetPlayer(string username);
        int LogIn(string username, string password);
        void AdjustBalance(int playerId, decimal amount);
        void DepositWithCard(int playerId, string cardNumber, string expiryDate, decimal amount);
    }
}