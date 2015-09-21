using System;

namespace GoofyCoin2015
{
    [Serializable()]
    public class Coin
    {
        private Int32 coinId;
        [field: NonSerializedAttribute()]
        private SignedMessage sgndCoin;

        public Coin(Signature mySignature)
        {
            coinId = Counter.Coin;
            sgndCoin = mySignature.SignMessage(this);
        }

        public Boolean isGoofyCoin()
        {
            return sgndCoin.PublicKey == Global.GoofyPk;
        }
        public Boolean isValidSignature()
        {
            return sgndCoin.isValidSignedMsg(this);
        }
    }
}
