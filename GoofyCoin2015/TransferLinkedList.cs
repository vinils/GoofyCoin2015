using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofyCoin2015
{
    [Serializable()]
    public class TransferLinkedList: TransferInfo, IEnumerable<TransferLinkedList>
    {
        TransferLinkedList previous;

        public TransferLinkedList Previous
        {
            get { return previous; }
        }

        public TransferLinkedList(TransferLinkedList previous, TransferInfo trans)
            :base(trans.PreviousTransSignedByMe, trans.DestinyPk)
        {
            this.previous = previous;
        }

        public TransferLinkedList Payto(TransferInfo trans)
        {
            return new TransferLinkedList(this, trans);
        }

        protected override void CheckTransfer()
        {
            base.CheckTransfer();

            if (previous == null)
                throw new Exception("Previous transfer must be informed");
        }

        public virtual void CheckTransfers()
        {
            CheckTransfer();

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

        IEnumerator<TransferLinkedList> IEnumerable<TransferLinkedList>.GetEnumerator()
        {
            TransferLinkedList trans = this;

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
