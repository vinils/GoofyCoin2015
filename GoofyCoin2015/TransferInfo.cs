//-----------------------------------------------------------------------
// <copyright file="TransferInfo.cs" company="VLS">
//     Copyright (c) VLS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GoofyCoin2015
{
    using System;

    /// <summary>
    /// Transfer which has previous transfer, previous transfer signed by the owner, destiny public key and can be a create coin transfer
    /// </summary>
    [Serializable]
    public class TransferInfo
    {
        /// <summary>
        /// Previous transfer hash signed by owner
        /// </summary>
        private SignedHash origin;

        /// <summary>
        /// Destiny public key
        /// </summary>
        private byte[] destinyPk;

        /// <summary>
        /// Coin id
        /// </summary>
        private Coin coin;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferInfo"/> class to create a coin.
        /// </summary>
        /// <param name="coin">Coin instance</param>
        /// <param name="destinyPk">Destiny public key</param>
        public TransferInfo(Coin coin, byte[] destinyPk)
            : this(coin, null, destinyPk)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferInfo"/> class.
        /// </summary>
        /// <param name="prevSgndHash">Signed hash of the previous transfer</param>
        /// <param name="destinyPk">Destiny public key</param>
        public TransferInfo(SignedHash prevSgndHash, byte[] destinyPk)
            : this(null, prevSgndHash, destinyPk)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferInfo"/> class to create a coin.
        /// </summary>
        /// <param name="coin">Coin instance</param>
        /// <param name="prevSgndHash">Previous signed hash transfer</param>
        /// <param name="destinyPk">Destiny public key</param>
        private TransferInfo(Coin coin, SignedHash prevSgndHash, byte[] destinyPk)
        {
            this.origin = prevSgndHash;
            this.destinyPk = destinyPk;
            this.coin = coin;
        }

        /// <summary>
        /// Gets Previous transfer hash signed by the owner
        /// </summary>
        public SignedHash Origin
        {
            get { return this.origin; }
        }

        /// <summary>
        /// Gets or sets transfer destiny public key
        /// </summary>
        public byte[] DestinyPk
        {
            get { return this.destinyPk; }
            set { this.destinyPk = value; }
        }

        /// <summary>
        /// Is this a create coin transfer?
        /// </summary>
        /// <returns>return true if it is create coin transfer</returns>
        public virtual bool IsCreateCoin()
        {
            return this.coin != null;
        }

        /// <summary>
        /// Is the origin null?
        /// </summary>
        /// <returns>return true if the previous hash null</returns>
        public virtual bool IsOriginhNull()
        {
            return this.origin == null;
        }

        /// <summary>
        /// Is destiny public key null?
        /// </summary>
        /// <returns>return true if destiny public key null</returns>
        public virtual bool IsDestinyPkNull()
        {
            return this.destinyPk == null;
        }

        /// <summary>
        /// Check all the validation
        /// </summary>
        public virtual void Check()
        {
            if (this.IsDestinyPkNull())
            {
                throw new Exception("The destiny public key must b informed");
            }

            if (this.IsCreateCoin())
            {
                this.coin.Check();
            }
            else
            {
                if (this.IsOriginhNull())
                {
                    throw new Exception("Previous hash transfer must be informed");
                }

                this.origin.Check();
            }
        }
    }
}
