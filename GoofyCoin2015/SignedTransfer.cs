//-----------------------------------------------------------------------
// <copyright file="SignedTransfer.cs" company="VLS">
//     Copyright (c) VLS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GoofyCoin2015
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// Signed transfer
    /// </summary>
    [Serializable]
    public class SignedTransfer
    {
        /// <summary>
        /// Signed transfer
        /// </summary>
        private byte[] sgndTrans;

        /// <summary>
        /// Public key
        /// </summary>
        private byte[] publicKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedTransfer"/> class.
        /// </summary>
        /// <param name="publicKey">Public key</param>
        /// <param name="signedTrans">Signed Transfer</param>
        public SignedTransfer(byte[] publicKey, byte[] signedTrans)
        {
            this.publicKey = publicKey;
            this.sgndTrans = signedTrans;
        }

        /// <summary>
        /// Gets public key
        /// </summary>
        public byte[] PublicKey
        {
            get { return this.publicKey; }
        }

        /// <summary>
        /// Gets signed transfer
        /// </summary>
        public byte[] SignedTrans
        {
            get { return this.sgndTrans; }
        }

        /// <summary>
        /// Is a valid signed transfer?
        /// </summary>
        /// <param name="transfer">transfer class</param>
        /// <returns>true if its a valid signed transfer</returns>
        public virtual bool IsValidSignedTransfer(Transfer transfer)
        {
            return this.IsValidSignedTransfer((object)transfer);
        }

        /// <summary>
        /// Is the signer of the signed transfer?
        /// </summary>
        /// <param name="publicKey">Public key</param>
        /// <returns>return true if public key is equals to the signed transfer public key</returns>
        public virtual bool IsSigner(byte[] publicKey)
        {
            return this.publicKey == publicKey;
        }

        /// <summary>
        /// Is public key null
        /// </summary>
        /// <returns>return true if the public key is null</returns>
        public virtual bool IsPublicKeyNull()
        {
            return this.publicKey == null;
        }

        /// <summary>
        /// Is signed transfer null
        /// </summary>
        /// <returns>return true if signed transfer is null</returns>
        public virtual bool IsSignedTransferNull()
        {
            return this.sgndTrans == null;
        }

        /// <summary>
        /// check is the signed transfer is valid
        /// </summary>
        public virtual void CheckSignedTransfer()
        {
            if (this.IsPublicKeyNull())
            {
                throw new Exception("Signed message must to have a public key.");
            }

            if (this.IsSignedTransferNull())
            {
                throw new Exception("Signed message must be signed");
            }
        }

        /// <summary>
        /// Is valid signed transfer
        /// </summary>
        /// <param name="obj">Object transfer</param>
        /// <returns>true if the transfer belong to this signed transfer and his public key</returns>
        private bool IsValidSignedTransfer(object obj)
        {
            bool ret;

            var serializedObj = Global.SerializeObject(obj);

            using (var dsa = new ECDsaCng(CngKey.Import(this.publicKey, CngKeyBlobFormat.EccPublicBlob)))
            {
                dsa.HashAlgorithm = Global.HashAlgorithm;

                //// verifying hashed message
                ////bReturn = dsa.VerifyHash(dataHash, SignedMsg);
                ret = dsa.VerifyData(serializedObj, this.SignedTrans);
            }

            return ret;
        }
    }
}