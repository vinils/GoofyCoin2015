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
    /// Hash table linked list transfer
    /// </summary>
    public class TransferList : IEnumerable<TransferList>
    {
        /// <summary>
        /// Next transfer
        /// </summary>
        private TransferList next;

        /// <summary>
        /// Transfer info
        /// </summary>
        private TransferInfo info;

        /// <summary>
        /// Signed hash of the transfer info
        /// </summary>
        private SignedHash sgndHash;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferList"/> class.
        /// </summary>
        /// <param name="info">transfer info</param>
        public TransferList(Coin coin, byte[] destinyPk)
        {
            this.info = new TransferInfo(coin, destinyPk);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferList"/> class.
        /// </summary>
        /// <param name="next">Next transfer</param>
        /// <param name="info">Transfer info</param>
        protected TransferList(TransferList next, TransferInfo info)
        {
            this.info = info;
            this.next = next;
        }

        /// <summary>
        /// Gets Next transfer
        /// </summary>
        public TransferInfo Info
        {
            get { return this.info; }
        }

        /// <summary>
        /// index of transfer
        /// </summary>
        /// <param name="hash">hash transfer</param>
        /// <returns>return the transfer if its in the chain</returns>
        public TransferList this[TransferInfoHash hash]
        {
            get
            {
                foreach (var ret in this)
                {
                    if (ret.sgndHash.Compare(hash))
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
        /// <param name="prevSgndHash">signed previous hash transfer</param>
        /// <param name="destinyPk">destiny public key</param>
        /// <returns>the new transfer</returns>
        public TransferList PayTo(SignedHash prevSgndHash, byte[] destinyPk)
        {
            this.sgndHash = prevSgndHash;
            var info = new TransferInfo(prevSgndHash, destinyPk);
            this.next = new TransferList(null, info);

            return this.next;
        }

        /// <summary>
        /// Is next transfer null?
        /// </summary>
        /// <returns>return true if the next transfer is null</returns>
        public virtual bool IsNextNull()
        {
            return this.next == null;
        }

        /// <summary>
        /// Is signed hash null?
        /// </summary>
        /// <returns>return true if signed hash is null</returns>
        public virtual bool IsSignedHashNull()
        {
            return this.sgndHash == null;
        }

        /// <summary>
        /// Is a valid hash transfer info
        /// </summary>
        /// <returns>return true if its a valid hash transfer</returns>
        public bool IsValidHash()
        {
            return this.sgndHash.Compare(this.info);
        }

        /// <summary>
        /// is a valid signed hash and next transfer origin
        /// </summary>
        /// <returns>return true if signed hash is the same of next transfer origin</returns>
        public bool IsValidSignature()
        {
            // this.hash == next.info.previousHash
            return this.sgndHash.Compare(this.next.info.Origin);
        }

        /// <summary>
        /// Check if the hash transfer
        /// </summary>
        public virtual void CheckHash()
        {
            if (this.IsSignedHashNull())
            {
                throw new Exception("Hash transfer must be informed.");
            }

            if (!this.IsValidHash())
            {
                throw new Exception("Invalid hash");
            }
        }

        /// <summary>
        /// Check if the next transfer is valid linked to this one
        /// </summary>
        public virtual void CheckNextTransfer()
        {
            if (this.IsNextNull())
            {
                throw new Exception("Next transfer must be informed.");
            }

            if (!this.IsValidSignature())
            {
                throw new Exception("The next signed previous transfer is invalid.");
            }
        }

        /// <summary>
        /// IEnumerable for all transfer
        /// </summary>
        /// <returns>return transfer in the chain</returns>
        IEnumerator<TransferList> IEnumerable<TransferList>.GetEnumerator()
        {
            var trans = this;

            do
            {
                yield return trans;
                trans = trans.next;
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