﻿namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}
