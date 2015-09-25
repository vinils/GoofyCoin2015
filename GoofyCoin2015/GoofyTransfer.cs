using System;
using System.Collections;
using System.Collections.Generic;

namespace GoofyCoin2015
{
    [Serializable()]
    public class GoofyTransfer: TransferLinkedList
    {
        private Coin coin;

        public Coin Coin
        {
            get { return coin; }
        }

        public GoofyTransfer(Coin coin, byte[] destinyPk)
            : base(null, new TransferInfo(null, destinyPk))
        {
            this.coin = coin;
        }

        public override Boolean isOwnerTransction()
        {
            return isGoofyCoin();
        }
        public override Boolean isValidSignedMsg()
        {
            return coin.isValidSignature();
        }

        public override void CheckTransfers()
        {
            if (!isGoofyCoin())
                throw new Exception("This coin don't belong to Goofy");

            if (!isValidSignedMsg())
                throw new Exception("This coin signature is invalid");
        }

        private Boolean isGoofyCoin()
        {
            return coin.isGoofyCoin();
        }
    }
}
