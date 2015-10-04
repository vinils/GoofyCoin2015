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
            var transList = new TransferList(coin, destiny.PublicKey);

            // Assert
            try
            {
                if (transList.IsDestinyPkNull())
                {
                    throw new Exception("Destiny public key must be informed.");
                }

                if (!transList.IsCreateCoin())
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
                transList.Check();
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
            var sgndTrans1 = person1.SignTransfer(trans1);
            var destiny = new Person();
            var trans2 = trans1.PayTo(sgndTrans1, destiny.PublicKey);

            // Assert
            try
            {
                if (trans2.IsDestinyPkNull())
                {
                    throw new Exception("The destiny public key must b informed");
                }

                if (trans2.IsCreateCoin())
                {
                    throw new Exception("This should not be a create coin transfer.");
                }

                if (trans2.IsPrepreviousTransSignedByMeNull())
                {
                    throw new Exception("The signed previous transfer must be informed");
                }

                // previous.receiverPk != previousTransSignedByMe.PublicKey;
                if (!trans2.IsOwnerTransction())
                {
                    throw new Exception("The transfer dosen't belong to the owner");
                }

                // !previousTransSignedByMe.isValidSignedMsg(previous);
                if (!trans2.IsValidSignedTransfer())
                {
                    throw new Exception("The previous transfer and his signature dont match");
                }

                // checking all those validations above and the last goofytransfer
                trans2.Check();
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
            var changerSgndTrans = changer.SignTransfer(trans1);
            var changerTransfer = trans1.PayTo(changerSgndTrans, person1.PublicKey);

            person1.AddTransfer(changerTransfer);

            var tran3 = person1.PayTo(person2.PublicKey);

            // Act
            changerTransfer.DestinyPk = null;

            // Assert
            try
            {
                person2.CheckTransfers(tran3);
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
                // testing the for loop checkTransfer
                person2.CheckTransfers(trans2);

                person2.AddTransfer(trans2);
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
            var sgndTrans1 = attacker.SignTransfer(trans1);
            var destiny1 = new Person();
            var trans2 = trans1.PayTo(sgndTrans1, destiny1.PublicKey);
            var destiny2 = new Person();
            var trans3 = trans1.PayTo(sgndTrans1, destiny2.PublicKey);

            // Assert
            try
            {
                if (trans2.IsValidSignedTransfer() && trans3.IsValidSignedTransfer())
                {
                    throw new Exception("Its not allowed to double spend the same coin.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
