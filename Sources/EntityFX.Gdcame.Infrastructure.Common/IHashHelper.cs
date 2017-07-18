﻿namespace EntityFX.Gdcame.Infrastructure.Common
{
    public interface IHashHelper
    {
        string GetHashedString(string input);
        int GetModuloOfUserIdHash(string userId, int modulo);
        int GetServerNumberByUserId(string[] servers, string userId);
    }
}