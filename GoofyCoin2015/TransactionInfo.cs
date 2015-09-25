using System;
using System.Collections;
using System.Collections.Generic;

namespace GoofyCoin2015
{
    [Serializable()]
    public class TransactionInfo
    {
        private SignedMessage previousTransSignedByMe;
        private byte[] destinyPk;

        public byte[] DestinyPk
        {
            get { return destinyPk; }
            set { destinyPk = value; }
        }

        public SignedMessage PreviousTransSignedByMe
        {
            get { return previousTransSignedByMe; }
        }

        public TransactionInfo(SignedMessage previousTransSignedByMe, byte[] destinyPk)
        {
            this.previousTransSignedByMe = previousTransSignedByMe;
            this.destinyPk = destinyPk;
        }

        public Boolean isValidSignedMsg(TransactionLinkedList previous)
        {
            return previousTransSignedByMe.isValidSignedMsg(previous);
        }

        public virtual void CheckTransactionInfo()
        {
            if (previousTransSignedByMe == null)
                throw new Exception("Previous transaction must be informed");

            if (destinyPk == null)
                throw new Exception("Destiny public key must b informed");
        }

        protected Boolean isSignerPreviousTransactoin(byte[] ownerPk)
        {
            return previousTransSignedByMe.PublicKey == ownerPk;
        }
    }
}
