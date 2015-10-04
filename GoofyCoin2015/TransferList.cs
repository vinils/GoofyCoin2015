//-----------------------------------------------------------------------
// <copyright file="TransferList.cs" company="VLS">
//     Copyright (c) VLS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GoofyCoin2015
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Linked list transfer
    /// </summary>
    [Serializable]
    public class TransferList : TransferInfo, IEnumerable<TransferList>
    {
        /// <summary>
        /// Previous transfer
        /// </summary>
        private TransferList previous;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferList"/> class.
        /// </summary>
        /// <param name="coin">Coin instance</param>
        /// <param name="publicKey">Public key</param>
        public TransferList(Coin coin, byte[] publicKey)
            : base(coin, publicKey)
        {
            this.previous = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferList"/> class.
        /// </summary>
        /// <param name="previous">previous transfer</param>
        /// <param name="previousTransSignedByMe">signed Previous transfer</param>
        /// <param name="destinyPk">Destiny public key</param>
        protected TransferList(TransferList previous, SignedMessage previousTransSignedByMe, byte[] destinyPk)
            : base(previousTransSignedByMe, destinyPk)
        {
            this.previous = previous;
        }

        /// <summary>
        /// Gets Previous transfer
        /// </summary>
        public TransferList Previous
        {
            get { return this.previous; }
        }

        /// <summary>
        /// index of transfer
        /// </summary>
        /// <param name="trans">Search transfer</param>
        /// <returns>return the transfer if its in the chain</returns>
        public TransferList this[TransferList trans]
        {
            get
            {
                foreach (var ret in this)
                {
                    if (ret == trans)
                    {
                        return ret;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Pay to
        /// </summary>
        /// <param name="previousTransSignedByMe">signed previous transaction</param>
        /// <param name="destinyPk">destiny public key</param>
        /// <returns>the new transfer</returns>
        public TransferList PayTo(SignedMessage previousTransSignedByMe, byte[] destinyPk)
        {
            return new TransferList(this, previousTransSignedByMe, destinyPk);
        }

        /// <summary>
        /// Is previous transfer null
        /// </summary>
        /// <returns>return true if the previous transfer is null</returns>
        public virtual bool IsPreviousNull()
        {
            return this.previous == null;
        }

        /// <summary>
        /// Is the owner of this transfer
        /// </summary>
        /// <returns>return true if is the owner</returns>
        public virtual bool IsOwnerTransction()
        {
            return this.PreviousTransSignedByMe.PublicKey == this.previous.DestinyPk;
        }

        /// <summary>
        /// This transfer, the signed transfer and the public key are valid together
        /// </summary>
        /// <returns>return true if its a valid transfer</returns>
        public virtual bool IsValidSignedTransfer()
        {
            return this.PreviousTransSignedByMe.IsValidSignedTransfer(this.previous);
        }

        /// <summary>
        /// Check all the validation
        /// </summary>
        public override void Check()
        {
            base.Check();

            if (!this.IsCreateCoin())
            {
                this.CheckLastTransfer();
            }
        }

        /// <summary>
        /// Check if this transfer is valid related to the last transfer
        /// </summary>
        public virtual void CheckLastTransfer()
        {
            if (this.IsPreviousNull())
            {
                throw new Exception("Previous transaction must be informed.");
            }

            if (!this.IsOwnerTransction())
            {
                throw new Exception("Transfer dosen't belong to the owner.");
            }

            if (!this.IsValidSignedTransfer())
            {
                throw new Exception("Signature of the previous transfer and his pk are invalid.");
            }
        }

        /// <summary>
        /// IEnumerable for all transfer
        /// </summary>
        /// <returns>return transfer in the chain</returns>
        IEnumerator<TransferList> IEnumerable<TransferList>.GetEnumerator()
        {
            TransferList trans = this;

            do
            {
                yield return trans;
                trans = trans.previous;
            }
            while (trans != null);
        }

        /// <summary>
        /// Not implemented yet
        /// </summary>
        /// <returns>return exception</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
