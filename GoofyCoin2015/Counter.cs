using System;

namespace GoofyCoin2015
{
    public static class Counter
    {
        private static Int32 coin;

        public static Int32 Coin
        {
            get { return ++coin; }
            set { coin = value; }
        }
    }
}
