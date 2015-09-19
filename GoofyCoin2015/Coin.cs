using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofyCoin2015
{
    public class Coin
    {
        private Int32 coinId;
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
    }
}
