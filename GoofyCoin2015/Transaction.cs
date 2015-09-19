using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofyCoin2015
{
    public class Transaction
    {
        private Coin coin;
        private Transaction previous;
        private SignedMessage previousTransSignedByMe;
        private String receiverPk;

        private Transaction()
        {
        }

        public Transaction(Coin coin, String receiverPk)
        {
            this.coin = coin;
            this.previous = null;
            this.previousTransSignedByMe = null;
            this.receiverPk = receiverPk;
        }

        public Transaction Payto(SignedMessage sgndTrans, String receiverPk)
        {
            var trans = new Transaction();
            trans.previous = this;
            trans.previousTransSignedByMe = sgndTrans;
            trans.receiverPk = receiverPk;

            return trans;
        }

        private Boolean isFirstTransaction()
        {
            return previous == null;
        }

        private Boolean isValidTransaction()
        {
            return previous.receiverPk == previousTransSignedByMe.PublicKey;
        }

        public void CheckTransaction()
        {
            var actualTrans = this;

            if (isFirstTransaction())
            {
                if (!coin.isGoofyCoin())
                    throw new Exception("This coin don't belong to Goofy");
            }
            else
            {
                if (!actualTrans.previousTransSignedByMe.isValidSignedMsg())
                    throw new Exception("The signature of the previous transaction don't match with the public key");

                if (!actualTrans.isValidTransaction())
                    throw new Exception("The transaction dosen't belong to the owner");
            }
        }
    }
}
