//-----------------------------------------------------------------------
// <copyright file="Coin.cs" company="VLS">
//     Copyright (c) VLS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GoofyCoin2015
{
    using System;

    /// <summary>
    /// Create a new coin
    /// </summary>
    [Serializable]
    public class Coin
    {
        /// <summary>
        /// Coin id
        /// </summary>
        private int coinId;

        /// <summary>
        /// Signed coin id
        /// </summary>
        private SignedMessage sgndCoin;

        /// <summary>
        /// Initializes a new instance of the <see cref="Coin"/> class.
        /// </summary>
        /// <param name="coinId">Coin id</param>
        /// <param name="sgndCoin">signed coin</param>
        public Coin(int coinId, SignedMessage sgndCoin)
        {
            this.coinId = coinId;
            this.sgndCoin = sgndCoin;
        }

        /// <summary>
        /// Gets Coin id
        /// </summary>
        public int CoinId
        {
            get { return this.coinId; }
        }

        /// <summary>
        /// Gets Signed coin
        /// </summary>
        public SignedMessage SignedCoin
        {
            get { return this.sgndCoin; }
        }

        /// <summary>
        /// Is a goofy coin?
        /// </summary>
        /// <returns>Return true if is a goofy coin</returns>
        public bool IsGoofyCoin()
        {
            return this.SignedCoin.PublicKey == Global.GoofyPk;
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
        /// Validate if this coin has a valid signature
        /// </summary>
        /// <returns>Return true if its a valid coin signature</returns>
        public bool IsValidSignature()
        {
            return this.SignedCoin.IsValidSignedMsg(this);
        }

        /// <summary>
        /// Check the coin
        /// </summary>
        public void Check()
        {
            if (!this.IsGoofyCoin())
            {
                throw new Exception("This is not a goofy coin");
            }

            if (!this.IsValidCoinId())
            {
                throw new Exception("This is not valid coin");
            }

            if (!this.IsValidSignature())
            {
                throw new Exception("This is not a valid signature");
            }
        }
    }
}
