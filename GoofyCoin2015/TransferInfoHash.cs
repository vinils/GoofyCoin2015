//-----------------------------------------------------------------------
// <copyright file="TransferInfoHash.cs" company="VLS">
//     Copyright (c) VLS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GoofyCoin2015
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// Hash of the transfer info
    /// </summary>
    [Serializable]
    public class TransferInfoHash
    {
        /// <summary>
        /// hash code of the transfer info
        /// </summary>
        private byte[] hashCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferInfoHash"/> class.
        /// </summary>
        /// <param name="info">Transfer info</param>
        public TransferInfoHash(TransferInfo info)
        {
            var serializedInfo = Global.SerializeObject(info);
            this.hashCode = new SHA1Managed().ComputeHash(serializedInfo);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferInfoHash"/> class.
        /// </summary>
        /// <param name="hash">Hash transfer info</param>
        protected TransferInfoHash(byte[] hash)
        {
            this.hashCode = hash;
        }

        /// <summary>
        /// Gets hash code
        /// </summary>
        public byte[] HashCode
        {
            get { return this.hashCode; }
        }

        /// <summary>
        /// Is hash code null?
        /// </summary>
        /// <returns>return null if hash code is null</returns>
        public bool IsHashCodeNull()
        {
            return this.hashCode == null;
        }

        /// <summary>
        /// Compare the hash code
        /// </summary>
        /// <param name="info">Transfer info</param>
        /// <returns>return true if transfer info has the same hash</returns>
        public bool Compare(TransferInfo info)
        {
            var hash = new TransferInfoHash(info);
            return this.Compare(hash);
        }

        /// <summary>
        /// Compare the hash code
        /// </summary>
        /// <param name="hash">hash transfer info</param>
        /// <returns>return true if the hash is the same of the hash transfer info</returns>
        public bool Compare(TransferInfoHash hash)
        {
            for (int x = 0; x < this.hashCode.Length; x++)
            {
                if (this.hashCode[x] != hash.hashCode[x])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check the hash transfer info
        /// </summary>
        public virtual void Check()
        {
            if (this.IsHashCodeNull())
            {
                throw new Exception("Hash code must be informed.");
            }
        }
    }
}
