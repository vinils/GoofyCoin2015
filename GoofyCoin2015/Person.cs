using System.Collections.Generic;
using System.Linq;

namespace GoofyCoin2015
{
    public class Person
    {
        private static readonly int sizeKey = 256;
        private List<TransferList> wallet = new List<TransferList>();
        protected Signature mySignature = new Signature(sizeKey);

        public byte[] PublicKey
        {
            get { return mySignature.PublicKey; }
        }

        public Person()
        {
        }

        public void AddTransfer(TransferList trans)
        {
            CheckTransfer(trans);
            wallet.Add(trans);
        }

        public TransferList PayTo(byte[] publicKey)
        {
            var trans = wallet.Last();
            var sgndTrans = mySignature.SignMessage(trans);
            var transInfo = new TransferInfo(sgndTrans, publicKey);
            var paidTransfer = trans.Payto(transInfo);
            wallet.Remove(trans);

            return paidTransfer;
        }
        public virtual void CheckTransfer(TransferList transfer)
        {
            foreach (var trans in transfer)
            {
                trans.CheckTransfer();
            }
        }
    }
}
