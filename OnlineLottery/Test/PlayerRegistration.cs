using System;
using fit;

namespace OnlineLottery.Test
{
    public class SetUpTestEnvironment : Fixture
    {
        internal static IPlayerManager PlayerManager;
        public SetUpTestEnvironment()
        {
            PlayerManager = new PlayerManager();
        }
    }

    public class PlayerRegisters : ColumnFixture
    {
        public string Username = "";
        public string Password = "";

        public int PlayerId()
        {
            return SetUpTestEnvironment.PlayerManager.RegisterPlayer(new PlayerRegistrationInfo
            {
                Username = Username,
                Password = Password
            });
        }
    }

    public class CheckStoredDetails : ColumnFixture
    {
        public int PlayerId;
        public string Username => SetUpTestEnvironment.PlayerManager.GetPlayer(PlayerId).Username;

        public decimal Balance => SetUpTestEnvironment.PlayerManager.GetPlayer(PlayerId).Balance;
    }

    public class CheckLogIn : ColumnFixture
    {
        public string Username = "";
        public string Password = "";

        public bool CanLogIn()
        {
            try
            {
                SetUpTestEnvironment.PlayerManager.LogIn(Username, Password);
                return true;
            }
            catch (ApplicationException)
            {
                return false;
            }
        }
    }
}
