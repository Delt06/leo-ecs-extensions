using System;

namespace Runner.Collection
{
    public class CoinsService : ICoinsService
    {
        private int _coins;

        public int Coins
        {
            get => _coins;
            private set
            {
                if (_coins == value) return;
                _coins = value;
                Changed?.Invoke();
            }
        }

        public event Action Changed;

        public void Add(int coins)
        {
            Coins += coins;
        }
    }
}