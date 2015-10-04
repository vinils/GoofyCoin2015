//-----------------------------------------------------------------------
// <copyright file="Goofy.cs" company="VLS">
//     Copyright (c) VLS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GoofyCoin2015
{
    using System;

    /// <summary>
    /// Goofy central authority which can create new coins
    /// </summary>
    public class Goofy : Person
    {
        /// <summary>
        /// Ledger root
        /// </summary>
        private TransferList ledgerRoot;

        /// <summary>
        /// Coin Id
        /// </summary>
        private int coinId = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Goofy"/> class.
        /// </summary>
        public Goofy()
        {
            Global.GoofyPk = MySignature.PublicKey;
        }

        /// <summary>
        /// Create coin
        /// </summary>
        /// <param name="destinyPk">Destiny public key</param>
        /// <returns>Transfer class</returns>
        public TransferList CreateCoin(byte[] destinyPk)
        {
            var sgndCoin = this.MySignature.SignCoin(++this.coinId);
            var coin = new Coin(this.coinId, sgndCoin);
            var info = new TransferInfo(coin, destinyPk);
            this.ledgerRoot = new TransferList(info);

            return this.ledgerRoot;
        }

        /// <summary>
        /// Check ledger transfers
        /// </summary>
        public virtual void CheckTransfers()
        {
            foreach (var trans in this.ledgerRoot)
            {
                if (!trans.IsNextNull())
                {
                    ////// check the data chain
                    //// trans.CheckNextTransfer();

                    // check the hash chain
                    trans.CheckNextTransfer();
                }
            }
        }
    }
}
