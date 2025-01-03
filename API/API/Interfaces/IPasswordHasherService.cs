namespace API.Interfaces;

public interface IPasswordHasherService
{
    public string Hash(string password);
    public bool Verify(string passwordHash, string inputPassword);
}