//-----------------------------------------------------------------------
// <copyright file="SignedHash.cs" company="VLS">
//     Copyright (c) VLS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GoofyCoin2015
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// Signed message with the public key and the signed data
    /// </summary>
    [Serializable]
    public class SignedHash : TransferInfoHash
    {
        /// <summary>
        /// Signed data
        /// </summary>
        private byte[] sgndData;

        /// <summary>
        /// Public key
        /// </summary>
        private byte[] publicKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedHash"/> class.
        /// </summary>
        /// <param name="hash">transfer info hash</param>
        /// <param name="publicKey">Public key</param>
        /// <param name="sgndData">signed hash of transfer info</param>
        public SignedHash(byte[] hash, byte[] publicKey, byte[] sgndData)
            : base(hash)
        {
            this.publicKey = publicKey;
            this.sgndData = sgndData;
        }

        /// <summary>
        /// Gets public key
        /// </summary>
        public byte[] PublicKey
        {
            get { return this.publicKey; }
        }

        /// <summary>
        /// Gets signed data
        /// </summary>
        public byte[] SignedData
        {
            get { return this.sgndData; }
        }

        /// <summary>
        /// Is public key null?
        /// </summary>
        /// <returns>return true if the public key is null</returns>
        public virtual bool IsPublicKeyNull()
        {
            return this.publicKey == null;
        }

        /// <summary>
        /// Is signed data of the hash null?
        /// </summary>
        /// <returns>return true if signed data of the hash is null</returns>
        public virtual bool IsSignedDataNull()
        {
            return this.sgndData == null;
        }

        /// <summary>
        /// Compare the hash signatures
        /// </summary>
        /// <param name="sgndHash">signed hash</param>
        /// <returns>return true if they are the same</returns>
        public bool Compare(SignedHash sgndHash)
        {
            // this.sgndHash == sgndHash;
            return this.IsValidSignedHash(sgndHash.HashCode, sgndHash.publicKey);
        }

        /// <summary>
        /// check is the signed hash is valid
        /// </summary>
        public override void Check()
        {
            base.Check();

            if (this.IsPublicKeyNull())
            {
                throw new Exception("Signed message must to have a public key.");
            }

            if (this.IsSignedDataNull())
            {
                throw new Exception("Signed data must be informed.");
            }
        }

        /// <summary>
        /// Is the signed hash belong to the hash and public key
        /// </summary>
        /// <param name="hash">transfer info hash</param>
        /// <param name="publicKey">public key</param>
        /// <returns>true if the signed hash belong to the hash and public key</returns>
        protected bool IsValidSignedHash(byte[] hash, byte[] publicKey)
        {
            bool ret;

            using (var dsa = new ECDsaCng(CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob)))
            {
                dsa.HashAlgorithm = Global.HashAlgorithm;

                //// verifying hashed message
                ////bReturn = dsa.VerifyHash(dataHash, SignedMsg);
                ret = dsa.VerifyHash(hash, this.sgndData);
            }

            return ret;
        }
    }
}
