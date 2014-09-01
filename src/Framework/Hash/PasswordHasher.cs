namespace Lacjam.Framework.Hash
{
    public class PasswordHasher : IPasswordHasher
    {
        public string GetHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 10);
        }

        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}