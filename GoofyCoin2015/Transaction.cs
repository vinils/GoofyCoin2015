using System;
using System.Collections;
using System.Collections.Generic;

namespace GoofyCoin2015
{
    [Serializable()]
    public class Transaction: IEnumerable<Transaction>
    {
        private Coin coin;
        private Transaction previous;
        private SignedMessage previousTransSignedByMe;
        private byte[] receiverPk;

        private Transaction()
        {
        }

        public Transaction(Coin coin, byte[] receiverPk)
        {
            this.coin = coin;
            this.previous = null;
            this.previousTransSignedByMe = null;
            this.receiverPk = receiverPk;
        }

        public Transaction Payto(SignedMessage sgndTrans, byte[] receiverPk)
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
            foreach (var trans in this)
            {
                if (trans.isFirstTransaction())
                {
                    if (!trans.coin.isGoofyCoin())
                        throw new Exception("This coin don't belong to Goofy");

                    if (!trans.coin.isValidSignature())
                        throw new Exception("This coin signature is invalid");
                }
                else
                {
                    if (!trans.isValidTransaction())
                        throw new Exception("The transaction dosen't belong to the owner");

                    if (!trans.previousTransSignedByMe.isValidSignedMsg(trans.previous))
                        throw new Exception("The signature of the previous transaction and his pk are invalid");
                }
            }
        }

        IEnumerator<Transaction> IEnumerable<Transaction>.GetEnumerator()
        {
            Transaction trans = this;

            do
            {
                yield return trans;
                trans = trans.previous;
            } while (trans != null);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
