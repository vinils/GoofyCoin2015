using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofyCoin2015
{
    [Serializable]
    public class TransferListCreateCoin : TransferList
    {
        public TransferListCreateCoin(byte[] publicKey)
            : base(null, new TransferInfo(null, publicKey))
        {
            previous = null;
        }

        public override void CheckTransfer()
        {
            if(!isDestinyPkNotNull())
                throw new Exception("The destiny public key must b informed");
        }
   }
}