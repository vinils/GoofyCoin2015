using System;
using System.Collections;
using System.Collections.Generic;

namespace GoofyCoin2015
{
    [Serializable()]
    public class TransferList: TransferInfo, IEnumerable<TransferList>
    {
        protected TransferList previous;

        public TransferList Previous
        {
            get { return previous; }
        }

        protected TransferList(TransferList previous, TransferInfo transferInfo)
            : base(transferInfo.PreviousTransSignedByMe, transferInfo.DestinyPk )
        {
            this.previous = previous;
        }

        public TransferList PayTo(TransferInfo trans)
        {
            return new TransferList(this, trans);
        }

        public override void CheckTransfer()
        {
            base.CheckTransfer();
            CheckLastTransfer();
        }

        public virtual void CheckLastTransfer()
        {
            if (!isOwnerTransction())
                throw new Exception("The transfer dosen't belong to the owner");

            if (!isValidSignedMsg())
                throw new Exception("The signature of the previous transfer and his pk are invalid");
        }

        public virtual Boolean isOwnerTransction()
        {
            return isSignerPreviousTransactoin(previous.DestinyPk);
        }

        public virtual Boolean isValidSignedMsg()
        {
            return isValidSignedMsg(previous);
        }

        IEnumerator<TransferList> IEnumerable<TransferList>.GetEnumerator()
        {
            TransferList trans = this;

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
