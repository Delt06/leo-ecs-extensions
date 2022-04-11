using System;

namespace Runner.Collection
{
    public interface ICoinsService
    {
        int Coins { get; }
        event Action Changed;
        void Add(int coins);
    }
}