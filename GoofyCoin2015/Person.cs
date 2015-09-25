using System.Collections.Generic;
using System.Linq;

namespace GoofyCoin2015
{
    public class Person
    {
        private static readonly int sizeKey = 256;
        private List<TransactionLinkedList> wallet = new List<TransactionLinkedList>();
        protected Signature mySignature = new Signature(sizeKey);

        public byte[] PublicKey
        {
            get { return mySignature.PublicKey; }
        }

        public Person()
        {
        }

        public void AddTransaction(TransactionLinkedList trans)
        {
            CheckTransaction(trans);
            wallet.Add(trans);
        }

        public TransactionLinkedList PayTo(byte[] publicKey)
        {
            var trans = wallet.Last();
            var sgndTrans = mySignature.SignMessage(trans);
            var transInfo = new TransactionInfo(sgndTrans, publicKey);
            var paidTransaction = trans.Payto(transInfo);
            wallet.Remove(trans);

            return paidTransaction;
        }

        private void CheckTransaction(TransactionLinkedList transaction)
        {
            foreach(var trans in transaction)
            {
                trans.CheckTransaction();
            }
        }
    }
}
