namespace EntityFX.Gdcame.Infrastructure.Common
{
    public interface IHashHelper
    {
        string GetHashedString(string input);
        //        int GetModuloOfUserIdHash(string userId, int modulo);
        int GetServerNumberByRendezvousHashing(string userId, string[] servers);

        int GetModulo(string userId, int modulo);
    }
}