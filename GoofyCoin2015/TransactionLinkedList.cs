using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofyCoin2015
{
    [Serializable()]
    public class TransactionLinkedList: TransactionInfo, IEnumerable<TransactionLinkedList>
    {
        TransactionLinkedList previous;

        public TransactionLinkedList Previous
        {
            get { return previous; }
        }

        public TransactionLinkedList(TransactionLinkedList previous, TransactionInfo trans)
            :base(trans.PreviousTransSignedByMe, trans.DestinyPk)
        {
            this.previous = previous;
        }

        public TransactionLinkedList Payto(TransactionInfo trans)
        {
            return new TransactionLinkedList(this, trans);
        }

        public virtual void CheckTransaction()
        {
            if (!isOwnerTransction())
                throw new Exception("The transaction dosen't belong to the owner");

            if (!isValidSignedMsg())
                throw new Exception("The signature of the previous transaction and his pk are invalid");
        }

        public Boolean isOwnerTransction()
        {
            return isSignerPreviousTransactoin(previous.DestinyPk);
        }

        public virtual Boolean isValidSignedMsg()
        {
            return isValidSignedMsg(previous);
        }

        IEnumerator<TransactionLinkedList> IEnumerable<TransactionLinkedList>.GetEnumerator()
        {
            TransactionLinkedList trans = this;

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
