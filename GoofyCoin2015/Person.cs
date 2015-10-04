//-----------------------------------------------------------------------
// <copyright file="Person.cs" company="VLS">
//     Copyright (c) VLS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GoofyCoin2015
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Representation of system interaction of an user or a person
    /// This user can transfer coins
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Size of the ECDSA key
        /// </summary>
        private static readonly int SizeKey = 256;

        /// <summary>
        /// Received transfers
        /// </summary>
        private List<TransferList> wallet = new List<TransferList>();

        /// <summary>
        /// ECDSA Signature with public and private key
        /// </summary>
        private Signature mySignature = new Signature(SizeKey);

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        public Person()
        {
        }

        /// <summary>
        /// Gets person public key
        /// </summary>
        public byte[] PublicKey
        {
            get { return this.mySignature.PublicKey; }
        }

        /// <summary>
        /// Gets ECDSA person signature
        /// </summary>
        protected Signature MySignature
        {
            get { return this.mySignature; }
        }

        /// <summary>
        /// Add a transfer
        /// </summary>
        /// <param name="trans">Sent transfer</param>
        public void AddTransfer(TransferList trans)
        {
            this.CheckTransfers(trans);
            this.wallet.Add(trans);
        }

        /// <summary>
        /// Pay the last transfer to some person
        /// </summary>
        /// <param name="destinyPk">destiny public key</param>
        /// <returns>sent transfer</returns>
        public TransferList PayTo(byte[] destinyPk)
        {
            var trans = this.wallet.Last();
            var sgndTrans = this.mySignature.SignTransfer(trans.Info);
            var paidTransfer = trans.PayTo(sgndTrans, destinyPk);

            this.wallet.Remove(trans);

            return paidTransfer;
        }

        /// <summary>
        /// Check if a all the transfer and previous transfers
        /// </summary>
        /// <param name="transfer">Sent transfer</param>
        public virtual void CheckTransfers(TransferList transfer)
        {
            transfer.Info.Check();
        }
    }
}
