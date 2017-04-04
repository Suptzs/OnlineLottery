namespace OnlineLottery
{
    public interface ITicket
    {
        int[] Numbers { get; }
        IPlayerInfo Holder { get; }
        decimal Value { get; }
    }
}