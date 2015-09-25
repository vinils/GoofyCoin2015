﻿using System;

namespace GoofyCoin2015
{
    public static class Counter
    {
        private static Int32 coin;
        private static Int32 signature;
        private static Int32 transfer;

        public static Int32 Coin
        {
            get { return ++coin; }
            set { coin = value; }
        }

        public static Int32 Signature
        {
            get { return ++signature; }
            set { signature = value; }
        }

        public static Int32 Transfer
        {
            get { return ++transfer; }
            set { transfer = value; }
        }
    }
}
