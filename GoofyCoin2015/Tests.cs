using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofyCoin2015
{
    public static class Tests
    {
        public static void GoofyCreateAndTansferCoin_SouldHaveValidCoin()
        {
            //Arrange
            var goofySignature = new Signature(256);
            Global.GoofyPk = goofySignature.PublicKey;

            //Act
            var destiny = new Signature(256);
            var transList = new TransferListCreateCoin(destiny.PublicKey);

            //Assert
            try
            {
                if (!transList.isDestinyPkNotNull())
                    throw new Exception("Destiny public key must be informed.");

                //valid virtua method + all the balidations above
                transList.CheckTransfer();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void ReceivingAndMaekingTransfer_SouldHaveValidTransfer()
        { 
            //Arrange
            var goofy = new Goofy();
            var person1 = new Signature(256);
            var trans1 = goofy.CreateCoin(person1.PublicKey);

            //Action
            var sgndTrans1 = person1.SignMessage(trans1);
            var destiny = new Person();
            var transInfo = new TransferInfo(sgndTrans1, destiny.PublicKey);
            var trans2 = trans1.Payto(transInfo);

            //Assert
            try
            {
                if (!trans2.isPrepreviousTransSignedByMeNotNull())
                    throw new Exception("The signed previous transfer must be informed");

                if (!trans2.isDestinyPkNotNull())
                    throw new Exception("The destiny public key must b informed");

                //previous.receiverPk != previousTransSignedByMe.PublicKey;
                if (!trans2.isOwnerTransction())
                    throw new Exception("The transfer dosen't belong to the owner");

                //!previousTransSignedByMe.isValidSignedMsg(previous);
                if (!trans2.isValidSignedMsg())
                    throw new Exception("The previous transfer and his signature dont match");

                //checking all those validations above and the last goofytransfer
                trans2.CheckTransfer();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Attacker change a transfer in the middle of the chain and make the chain invalid
        /// </summary>
        public static void ChengeTransfer_SouldNotAffectTransferChain()
        {
            //Arrange
            var goofy = new Goofy();
            var changer = new Signature(256);
            var person1 = new Person();
            var person2 = new Person();

            var trans1 = goofy.CreateCoin(changer.PublicKey);
            var changerSgndTrans = changer.SignMessage(trans1);
            var transInfo = new TransferInfo(changerSgndTrans, person1.PublicKey);
            var changerTransfer = trans1.Payto(transInfo);

            person1.AddTransfer(changerTransfer);

            var tran3 = person1.PayTo(person2.PublicKey);

            //Act
            changerTransfer.DestinyPk = null;

            //Assert
            try
            {
                person2.AddTransfer(tran3);
            }
            catch 
            {
                Console.WriteLine("Transfer chain is broked because someone change a another transfer in the middle.");
            }
        }

        public static void ReceivingAndMaekingManyTransfer_SouldHaveValidTransferChain()
        {
            //Arrange
            var goofy = new Goofy();
            var person1 = new Person();
            var person2 = new Person();

            Global.GoofyPk = goofy.PublicKey;

            var trans1 = goofy.CreateCoin(person1.PublicKey);
            person1.AddTransfer(trans1);

            //Action
            var trans2 = person1.PayTo(person2.PublicKey);


            //Assert
            try
            {
                //testing the for loop checkTransfer
                person2.CheckTransfer(trans2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void DoubleSpendAttack_SouldHaveValidTransferChain()
        {
            //Arrange
            var goofy = new Goofy();
            var attacker = new Signature(256);

            Global.GoofyPk = goofy.PublicKey;

            var trans1 = goofy.CreateCoin(attacker.PublicKey);

            //Action
            var sgndTrans1 = attacker.SignMessage(trans1);
            var destiny1 = new Person();
            var transInfo1 = new TransferInfo(sgndTrans1, destiny1.PublicKey);
            var trans2 = trans1.Payto(transInfo1);
            var destiny2 = new Person();
            var transInfo2 = new TransferInfo(sgndTrans1, destiny2.PublicKey);
            var trans3 = trans1.Payto(transInfo2);

            //Assert
            try
            {
                if (trans2.isValidSignedMsg() && trans3.isValidSignedMsg())
                    throw new Exception("Its not allowed to double spend the same coin.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
