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
        private byte[] transactionDestinyPk;

        public byte[] TransactionDestinyPk
        {
            get { return transactionDestinyPk; }
            set { transactionDestinyPk = value; }
        }

        public SignedMessage PreviousTransSignedByMe
        {
            get { return previousTransSignedByMe; }
            set { previousTransSignedByMe = value; }
        }

        public Transaction Previous
        {
            get { return previous; }
            set { previous = value; }
        }

        public Coin Coin
        {
            get { return coin; }
            set { coin = value; }
        }

        private Transaction()
        {
        }

        public Transaction(Coin coin, byte[] destinyPk)
        {
            this.Coin = coin;
            this.previous = null;
            this.previousTransSignedByMe = null;
            this.transactionDestinyPk = destinyPk;
        }

        public Transaction Payto(SignedMessage sgndTrans, byte[] destinyPk)
        {
            var trans = new Transaction();
            trans.previous = this;
            trans.previousTransSignedByMe = sgndTrans;
            trans.transactionDestinyPk = destinyPk;

            return trans;
        }

        public Boolean isFirstTransaction()
        {
            return previous == null;
        }

        public Boolean isOwnerTransction()
        {
            return previous.transactionDestinyPk == previousTransSignedByMe.PublicKey;
        }

        public Boolean isValidSignedMsg()
        {
            return previousTransSignedByMe.isValidSignedMsg(previous);
        }

        public void CheckTransaction()
        {
            foreach (var trans in this)
            {
                if (trans.isFirstTransaction())
                {
                    if (!trans.Coin.isGoofyCoin())
                        throw new Exception("This coin don't belong to Goofy");

                    if (!trans.Coin.isValidSignature())
                        throw new Exception("This coin signature is invalid");
                }
                else
                {
                    if (!trans.isOwnerTransction())
                        throw new Exception("The transaction dosen't belong to the owner");

                    if (!trans.isValidSignedMsg())
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
