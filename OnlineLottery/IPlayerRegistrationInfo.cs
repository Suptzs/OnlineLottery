namespace OnlineLottery
{
    public interface IPlayerRegistrationInfo
    {
        string Name { get; }
        string Address { get; }
        string City { get; }
        string PostCode { get; }
        string Country { get; }
        string Username { get; }
        string Password { get; }
    }
}
