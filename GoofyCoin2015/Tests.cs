//-----------------------------------------------------------------------
// <copyright file="Tests.cs" company="VLS">
//     Copyright (c) VLS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GoofyCoin2015
{
    using System;

    /// <summary>
    /// Tests scenarios
    /// </summary>
    public static class Tests
    {
        /// <summary>
        /// Goofy create and transfer a coin should have a valid transfer
        /// </summary>
        public static void GoofyCreateAndTansferCoin_ShouldHaveValidCoin()
        {
            // Arrange
            var coinId = 1;
            var goofy = new Signature(256);
            Global.GoofyPk = goofy.PublicKey;

            // Act
            var sgndCoin = goofy.SignCoin(coinId);
            var coin = new Coin(coinId, sgndCoin);
            var destiny = new Signature(256);
            var info = new TransferInfo(coin, destiny.PublicKey);
            var trans = new TransferList(info);

            // Assert
            try
            {
                if (info.IsDestinyPkNull())
                {
                    throw new Exception("Destiny public key must be informed.");
                }

                if (!info.IsCreateCoin())
                {
                    throw new Exception("This should be a create coin transfer.");
                }

                ////if (!info.coin.IsGoofyCoin())
                ////    throw new Exception("This is not a goofy coin");

                ////if (!IsValidCoinId())
                ////    throw new Exception("This is not valid coin");

                ////if (!IsValidSignature())
                ////    throw new Exception("This is not a valid signature");

                // valid virtua method + all the balidations above
                trans.Info.Check();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Person receiving and making a transfer should have a valid transfer
        /// </summary>
        public static void ReceivingAndMakingTransfer_ShouldHaveValidTransfer()
        {
            // Arrange
            var goofy = new Goofy();
            var person1 = new Signature(256);
            var trans1 = goofy.CreateCoin(person1.PublicKey);

            // Action
            var sgndTrans = person1.SignTransfer(trans1.Info);
            var destiny = new Person();
            var trans2 = trans1.PayTo(sgndTrans, destiny.PublicKey);

            // Assert
            try
            {
                // validation 1
                if (trans2.Info.IsDestinyPkNull())
                {
                    throw new Exception("The destiny public key must b informed");
                }

                // validation 2
                if (trans2.Info.IsCreateCoin())
                {
                    throw new Exception("This should not be a create coin transfer.");
                }

                // validation 3
                if (trans2.Info.IsOriginhNull())
                {
                    throw new Exception("Previous hash transfer must be informed");
                }

                // validation 4
                // validation 3,4
                ((TransferInfoHash)trans2.Info.Origin).Check();

                // validation 5
                if (trans2.Info.Origin.IsHashCodeNull())
                {
                    throw new Exception("Hash code must be informed.");
                }

                // validation 6
                if (trans2.Info.Origin.IsSignedDataNull())
                {
                    throw new Exception("Signed data must be informed");
                }

                // validation 7
                // validation 4,5,6
                trans2.Info.Origin.Check();

                // validation 8
                // validation 1,2,3,7
                // checking all those validations above and the last goofytransfer
                trans2.Info.Check();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Attacker change a transfer in the middle of the chain and make the chain invalid
        /// </summary>
        public static void ChengeTransfer_ShouldNotAffectTransferChain()
        {
            // Arrange
            var goofy = new Goofy();
            var changer = new Signature(256);
            var person1 = new Person();
            var person2 = new Person();

            var trans1 = goofy.CreateCoin(changer.PublicKey);
            var sgndTrans = changer.SignTransfer(trans1.Info);
            var changerTransfer = trans1.PayTo(sgndTrans, person1.PublicKey);

            person1.AddTransfer(changerTransfer);

            var tran3 = person1.PayTo(person2.PublicKey);

            // Act
            changerTransfer.Info.DestinyPk = null;

            // Assert
            try
            {
                goofy.CheckTransfers();
            }
            catch 
            {
                Console.WriteLine("Transfer chain is broked because someone change a another transfer in the middle.");
            }
        }

        /// <summary>
        /// Receiving and making many transfers should have a valid transfer chain
        /// </summary>
        public static void ReceivingAndMakingManyTransfers_ShouldHaveValidTransferChain()
        {
            // Arrange
            var goofy = new Goofy();
            var person1 = new Person();
            var person2 = new Person();

            var trans1 = goofy.CreateCoin(person1.PublicKey);
            person1.AddTransfer(trans1);

            // Action
            var trans2 = person1.PayTo(person2.PublicKey);

            // Assert
            try
            {
                // validation 1
                if (trans1.IsSignedHashNull())
                {
                    throw new Exception("Hash transfer must be informed.");
                }

                // validation 2
                // trans1.hash.IsValidHash(trans1.Info.Hash(), trans1.Info.destinyPk);
                // trans1.info.IsValidHash(trans1.hash);
                if (!trans1.IsValidHash())
                {
                    throw new Exception("Invalid hash");
                }

                // validation 3
                // validation 1,2
                trans1.CheckHash();

                // validation 4
                if (trans1.IsNextNull())
                {
                    throw new Exception("Next transfer must be informed.");
                }

                // validation 5
                // this.hash == this.next.info.PreviousHash
                // trans2.Info.PreviousHash.IsValidHash(trans1.hash.HashCode, trans1.hash.SignedHash.PublicKey);
                // trans1.next.info.IsValidSignedTransfer(trans1.hash);
                if (!trans1.IsValidSignature())
                {
                    throw new Exception("The next signed previous transfer is invalid.");
                }

                // validation 6
                // validation 4,5
                trans1.CheckNextTransfer();

                goofy.CheckTransfers();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Double spend attack should not have a valid transfer chain
        /// </summary>
        public static void DoubleSpendAttack_ShouldNotHaveValidTransferChain()
        {
            // Arrange
            var goofy = new Goofy();
            var attacker = new Signature(256);

            var trans1 = goofy.CreateCoin(attacker.PublicKey);

            // Action
            var sgndTrans1 = attacker.SignTransfer(trans1.Info);
            var destiny1 = new Person();
            var trans2 = trans1.PayTo(sgndTrans1, destiny1.PublicKey);
            var destiny2 = new Person();
            var trans3 = trans1.PayTo(sgndTrans1, destiny2.PublicKey);

            // Assert
            try
            {
                ////if (trans2.IsValidSignedTransfer() && trans3.IsValidSignedTransfer())
                ////{
                ////    throw new Exception("Its not allowed to double spend the same coin.");
                ////}

                trans1.CheckNextTransfer();

                throw new Exception("There is not a way to check double spend attack.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
