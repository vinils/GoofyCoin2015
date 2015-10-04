//-----------------------------------------------------------------------
// <copyright file="Signature.cs" company="VLS">
//     Copyright (c) VLS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GoofyCoin2015
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// ECDSA Signature class
    /// </summary>
    public class Signature
    {
        /// <summary>
        /// ECDSA instance class
        /// </summary>
        private ECDsaCng dsa;

        /// <summary>
        /// Public key
        /// </summary>
        private byte[] publicKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="Signature"/> class.
        /// </summary>
        /// <param name="sizeKey">Size of the ECDSA key</param>
        public Signature(int sizeKey)
        {
            this.dsa = new ECDsaCng(sizeKey);
            this.dsa.HashAlgorithm = Global.HashAlgorithm;
            this.publicKey = this.dsa.Key.Export(CngKeyBlobFormat.EccPublicBlob);
        }

        /// <summary>
        /// Gets or sets Public key
        /// </summary>
        public byte[] PublicKey
        {
            get { return this.publicKey; }
            protected set { this.publicKey = value; }
        }

        /// <summary>
        /// Sign a serialized coin id
        /// </summary>
        /// <param name="coinId">Coin id</param>
        /// <returns>Signed coin</returns>
        public SignedMessage SignCoin(int coinId)
        {
            var serializedObj = Global.SerializeObject(coinId);
            return this.SignMsg(serializedObj);
        }

        /// <summary>
        /// Sign the hash of the transfer info
        /// </summary>
        /// <param name="info">Transfer info</param>
        /// <returns>return the signed hash</returns>
        public SignedHash SignTransfer(TransferInfo info)
        {
            return this.SignTransfer(new TransferInfoHash(info));
        }

        /// <summary>
        /// Sign the hash of the transfer info
        /// </summary>
        /// <param name="hash">hash of the transfer info</param>
        /// <returns>return the signed hash</returns>
        protected SignedHash SignTransfer(TransferInfoHash hash)
        {
            return this.SignHash(hash.HashCode);
        }

        /// <summary>
        /// Sign a hash
        /// </summary>
        /// <param name="hash">hash information</param>
        /// <returns>return signed hash</returns>
        protected SignedHash SignHash(byte[] hash)
        {
            //// signing hash data
            ////var msgHashed = new SHA1Managed().ComputeHash(message);
            ////var sgndData = dsa.SignHash(msgHashed); 

            ////var sgndData = this.dsa.SignData(msg);
            var sgndData = this.dsa.SignHash(hash);
            return new SignedHash(hash, this.publicKey, sgndData);
        }

        /// <summary>
        /// Sign a message
        /// </summary>
        /// <param name="msg">Message instance</param>
        /// <returns>Signed message</returns>
        protected SignedMessage SignMsg(byte[] msg)
        {
            //// signing hash data
            ////var msgHashed = new SHA1Managed().ComputeHash(message);
            ////var sgndData = dsa.SignHash(msgHashed); 

            ////var sgndData = this.dsa.SignData(msg);
            var sgndData = this.dsa.SignData(msg);
            return new SignedMessage(this.publicKey, sgndData);
        }
    }
}
