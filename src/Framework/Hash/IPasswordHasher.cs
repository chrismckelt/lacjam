namespace Lacjam.Framework.Hash
{
    public interface IPasswordHasher
    {
        string GetHash(string password);
        bool Verify(string password, string hash);
    }
}