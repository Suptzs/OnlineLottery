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
        public class ExtendedPlayerRegistrationInfo : PlayerRegistrationInfo
        {
            public int PlayerId() => SetUpTestEnvironment.PlayerManager.RegisterPlayer(this);
        }

        private readonly ExtendedPlayerRegistrationInfo _extendedRegInfo = new ExtendedPlayerRegistrationInfo();
        public override object GetTargetObject() => _extendedRegInfo;
    }

    public class CheckStoredDetailsFor : ColumnFixture
    {
        public override object GetTargetObject() => SetUpTestEnvironment.PlayerManager.GetPlayer(Symbols.GetValueAs<int>("player"));
    }

    public class CheckLogIn : ColumnFixture
    {
        public string Username = "";
        public string Password = "";

        public int LoggedInAsPlayerId() => SetUpTestEnvironment.PlayerManager.LogIn(Username, Password);
    }

    public class CheckUserIdsForUniqueness : ColumnFixture
    {
        public int Player1;
        public int Player2;
        public int Player3;

        public bool AreIdsUnique() => Player1 != Player2 && Player1 != Player3 && Player2 != Player3;
    }
}
