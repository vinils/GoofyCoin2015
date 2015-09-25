using System;
using System.Collections;
using System.Collections.Generic;

namespace GoofyCoin2015
{
    [Serializable()]
    public class TransferInfo
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

        public TransferInfo(SignedMessage previousTransSignedByMe, byte[] destinyPk)
        {
            this.previousTransSignedByMe = previousTransSignedByMe;
            this.destinyPk = destinyPk;
        }

        public Boolean isValidSignedMsg(TransferLinkedList previous)
        {
            return previousTransSignedByMe.isValidSignedMsg(previous);
        }

        protected virtual void CheckTransfer()
        {
            if (previousTransSignedByMe == null)
                throw new Exception("Signed Previous transaction must be informed");

            if (destinyPk == null)
                throw new Exception("Destiny public key must b informed");
        }

        protected Boolean isSignerPreviousTransactoin(byte[] ownerPk)
        {
            return previousTransSignedByMe.PublicKey == ownerPk;
        }
    }
}
