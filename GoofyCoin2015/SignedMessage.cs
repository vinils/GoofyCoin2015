//-----------------------------------------------------------------------
// <copyright file="SignedMessage.cs" company="VLS">
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
    public class SignedMessage
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
        /// Initializes a new instance of the <see cref="SignedMessage"/> class.
        /// </summary>
        /// <param name="publicKey">Public key</param>
        /// <param name="sgndData">Signed data</param>
        public SignedMessage(byte[] publicKey, byte[] sgndData)
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
        /// Is signed data of the message null?
        /// </summary>
        /// <returns>return true if signed data of the message is null</returns>
        public virtual bool IsSignedDataNull()
        {
            return this.sgndData == null;
        }

        /// <summary>
        /// Is signature of the coin?
        /// </summary>
        /// <param name="coin">coin instance</param>
        /// <returns>return true if its the signature of the coin</returns>
        public virtual bool IsValidSignedMsg(Coin coin)
        {
            return this.IsValidSignedMsg(Global.SerializeObject(coin.CoinId), Global.GoofyPk);
        }

        /// <summary>
        /// check is the signed message is valid
        /// </summary>
        public virtual void Check()
        {
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
        /// Is the signed message belong to the message and public key
        /// </summary>
        /// <param name="msg">Serialized message</param>
        /// <param name="publicKey">public key</param>
        /// <returns>true if the signed message belong to the message and public key</returns>
        protected bool IsValidSignedMsg(byte[] msg, byte[] publicKey)
        {
            bool ret;

            using (var dsa = new ECDsaCng(CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob)))
            {
                dsa.HashAlgorithm = Global.HashAlgorithm;

                //// verifying hashed message
                ////bReturn = dsa.VerifyHash(dataHash, SignedMsg);
                ret = dsa.VerifyData(msg, this.SignedData);
            }

            return ret;
        }
    }
}