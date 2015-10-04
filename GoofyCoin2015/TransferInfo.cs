//-----------------------------------------------------------------------
// <copyright file="TransferInfo.cs" company="VLS">
//     Copyright (c) VLS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GoofyCoin2015
{
    using System;

    /// <summary>
    /// Transfer which has previous transfer, previous transfer signed by the owner, destiny public key and can by a create coin transfer
    /// </summary>
    [Serializable]
    public abstract class TransferInfo
    {
        /// <summary>
        /// Previous transfer signed by owner
        /// </summary>
        private SignedMessage prevTransSgndByMe;

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
        /// <param name="prevTransSgndByMe">Signed hash of the previous transfer</param>
        /// <param name="destinyPk">Destiny public key</param>
        protected TransferInfo(SignedMessage prevTransSgndByMe, byte[] destinyPk)
            : this(null, prevTransSgndByMe, destinyPk)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferInfo"/> class to create a coin.
        /// </summary>
        /// <param name="coin">Coin instance</param>
        /// <param name="prevTransSgndByMe">Previous signed transfer</param>
        /// <param name="destinyPk">Destiny public key</param>
        private TransferInfo(Coin coin, SignedMessage prevTransSgndByMe, byte[] destinyPk)
        {
            this.coin = coin;
            this.prevTransSgndByMe = prevTransSgndByMe;
            this.destinyPk = destinyPk;
        }

        /// <summary>
        /// Gets Previous transfer signed by the owner
        /// </summary>
        public SignedMessage PreviousTransSignedByMe
        {
            get { return this.prevTransSgndByMe; }
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
        /// Is the previous transfer signed by me null?
        /// </summary>
        /// <returns>return true if the previous transfer signed by me null</returns>
        public virtual bool IsPrepreviousTransSignedByMeNull()
        {
            return this.prevTransSgndByMe == null;
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
                if (this.IsPrepreviousTransSignedByMeNull())
                {
                    throw new Exception("Signed previous transfer must be informed");
                }

                this.prevTransSgndByMe.Check();
            }
        }
    }
}
