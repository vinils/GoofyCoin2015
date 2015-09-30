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
        /// Serialize and sign the transfer
        /// </summary>
        /// <param name="transfer">Transfer class</param>
        /// <returns>Signed transfer</returns>
        public SignedTransfer SignTransfer(Transfer transfer)
        {
            return this.SignTransfer((object)transfer);
        }

        /// <summary>
        /// Serialize and sign the object
        /// </summary>
        /// <param name="obj">Any object</param>
        /// <returns>Signed object</returns>
        private SignedTransfer SignTransfer(object obj)
        {
            var serializedObj = Global.SerializeObject(obj);

            //// signing hash data
            ////var msgHashed = new SHA1Managed().ComputeHash(message);
            ////var sgndData = dsa.SignHash(msgHashed); 

            var sgndData = this.dsa.SignData(serializedObj);
            return new SignedTransfer(this.publicKey, sgndData);
        }
    }
}
