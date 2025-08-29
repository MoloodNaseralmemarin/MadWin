namespace MadWin.Application.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}
