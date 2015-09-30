//-----------------------------------------------------------------------
// <copyright file="Transfer.cs" company="VLS">
//     Copyright (c) VLS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GoofyCoin2015
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Transfer which has previous transfer, previous transfer signed by the owner, destiny public key and can by a create coin transfer
    /// </summary>
    [Serializable]
    public class Transfer : IEnumerable<Transfer>
    {
        /// <summary>
        /// Previous transfer
        /// </summary>
        private Transfer previous;

        /// <summary>
        /// Previous transfer signed by owner
        /// </summary>
        private SignedTransfer previousTransSignedByMe;

        /// <summary>
        /// Destiny public key
        /// </summary>
        private byte[] destinyPk;

        /// <summary>
        /// Coin id
        /// </summary>
        private int? coinId;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transfer"/> class.
        /// </summary>
        /// <param name="coinId">Coin id</param>
        /// <param name="publicKey">Public key</param>
        public Transfer(int coinId, byte[] publicKey)
            : this(null, null, publicKey)
        {
            this.coinId = coinId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transfer"/> class.
        /// </summary>
        /// <param name="previous">previous transfer</param>
        /// <param name="previousTransSignedByMe">signed Previous transfer</param>
        /// <param name="destinyPk">Destiny public key</param>
        protected Transfer(Transfer previous, SignedTransfer previousTransSignedByMe, byte[] destinyPk)
        {
            this.previous = previous;
            this.previousTransSignedByMe = previousTransSignedByMe;
            this.destinyPk = destinyPk;
        }

        /// <summary>
        /// Gets Previous transfer
        /// </summary>
        public Transfer Previous
        {
            get { return this.previous; }
        }

        /// <summary>
        /// Gets or sets transfer destiny public key
        /// </summary>
        public byte[] DestinyPk
        {
            get { return this.destinyPk; }
            set { this.destinyPk = value; }
        }

        /// <summary>
        /// Gets Previous transfer signed by the owner
        /// </summary>
        public SignedTransfer PreviousTransSignedByMe
        {
            get { return this.previousTransSignedByMe; }
        }

        /// <summary>
        /// index of transfer
        /// </summary>
        /// <param name="trans">Search transfer</param>
        /// <returns>return the same transfer if exist in the chain</returns>
        public Transfer this[Transfer trans]
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
        public Transfer PayTo(SignedTransfer previousTransSignedByMe, byte[] destinyPk)
        {
            return new Transfer(this, previousTransSignedByMe, destinyPk);
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
            return this.previousTransSignedByMe.PublicKey == this.previous.DestinyPk;
        }

        /// <summary>
        /// This transfer, the signed transfer and the public key are valid together
        /// </summary>
        /// <returns>return true if its a valid transfer</returns>
        public virtual bool IsValidSignedTransfer()
        {
            return this.previousTransSignedByMe.IsValidSignedTransfer(this.previous);
        }

        /// <summary>
        /// Is the a valid coin id?
        /// </summary>
        /// <returns>return true if its bigger than 0</returns>
        public virtual bool IsValidCoinId()
        {
            return this.coinId >= 0;
        }

        /// <summary>
        /// Is this a create coin transfer?
        /// </summary>
        /// <returns>return true if it is create coin transfer</returns>
        public virtual bool IsCreateCoin()
        {
            return this.coinId != null;
        }

        /// <summary>
        /// Is the previous transfer signed by me null?
        /// </summary>
        /// <returns>return true if the previous transfer signed by me null</returns>
        public virtual bool IsPrepreviousTransSignedByMeNull()
        {
            return this.previousTransSignedByMe == null;
        }

        /// <summary>
        /// Is destiny public key null?
        /// </summary>
        /// <returns>return true if destiny public key null</returns>
        public virtual bool IsDestinyPkNull()
        {
            return this.destinyPk == null;
        }

        /// <summary>
        /// Check all the validation
        /// </summary>
        public virtual void CheckTransfer()
        {
            if (this.IsDestinyPkNull())
            {
                throw new Exception("The destiny public key must b informed");
            }

            if (this.IsCreateCoin())
            {
                if (!this.IsValidCoinId())
                {
                    throw new Exception("Coin id must be informed.");
                }
            }
            else
            {
                this.CheckLastTransfer();
            }
        }

        /// <summary>
        /// Check if this transfer is valid related to the last transfer
        /// </summary>
        public virtual void CheckLastTransfer()
        {
            if (this.IsPrepreviousTransSignedByMeNull())
            {
                throw new Exception("Signed previous transfer must be informed");
            }

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
        IEnumerator<Transfer> IEnumerable<Transfer>.GetEnumerator()
        {
            Transfer trans = this;

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
