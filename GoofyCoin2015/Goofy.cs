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
        /// Coin Id
        /// </summary>
        private static int coinId = 0;

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
        public Transfer CreateCoin(byte[] destinyPk)
        {
            return new Transfer(++coinId, destinyPk);
        }
    }
}
