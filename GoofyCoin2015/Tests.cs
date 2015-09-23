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

            var goofyCoin = new Coin(goofySignature);

            //Act
            var destiny = new Signature(256);
            var trans = new Transaction(goofyCoin, destiny.PublicKey);

            //Assert

            try
            {
                //if(trans.Coin!=null)
                if(!trans.isFirstTransaction())
                    throw new Exception("This is the first transaction");

                //if(trans.Coin.Signature.PublicKey != Global.GoofyPk)
                if (!trans.Coin.isGoofyCoin())
                    throw new Exception("This coin doenst belong to Goofy");

                //if(trans.Coin.Signature.isValidSignedMsg(trans.Coin))
                if (!trans.Coin.isValidSignature())
                    throw new Exception("This coin signature is invalid");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void ReceivingAndMaekingTransfer_SouldHaveValidTransactionChain()
        { 
            //Arrange
            var goofy = new Goofy();
            var person1 = new Signature(256);

            Global.GoofyPk = goofy.PublicKey;

            var trans1 = goofy.CreateCoin(person1.PublicKey);

            //Action
            var sgndTrans1 = person1.SignMessage(trans1);
            var destiny = new Person();
            var trans2 = trans1.Payto(sgndTrans1, destiny.PublicKey);

            //Assert
            try
            {
                //previous.receiverPk != previousTransSignedByMe.PublicKey;
                //if(trans2.Previous.TransactionDestinyPk != trans2.PreviousTransSignedByMe.PublicKey)
                if (!trans2.isOwnerTransction())
                    throw new Exception("The transaction dosen't belong to the owner");

                //!previousTransSignedByMe.isValidSignedMsg(previous);
                //if (!trans2.PreviousTransSignedByMe.isValidSignedMsg(trans2.Previous))
                if(!trans2.isValidSignedMsg())
                    throw new Exception("The previous transaction and his signature dont match");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void ReceivingAndMaekingManyTransfer_SouldHaveValidTransactionChain()
        {
            //Arrange
            var goofy = new Goofy();
            var person1 = new Person();
            var person2 = new Person();

            Global.GoofyPk = goofy.PublicKey;

            var trans1 = goofy.CreateCoin(person1.PublicKey);

            //Action
            person1.AddTransaction(trans1);

            var trans2 = person1.PayTo(person2.PublicKey);
            person2.AddTransaction(trans2);

            var destiny = new Person();
            var trans3 = person2.PayTo(destiny.PublicKey);

            //Assert
            try
            {
                //testing the for loop
                trans3.CheckTransaction();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void DoubleSpendAttach_SouldHaveValidTransactionChain()
        {
            //Arrange
            var goofy = new Goofy();
            var attacker = new Signature(256);

            Global.GoofyPk = goofy.PublicKey;

            var trans1 = goofy.CreateCoin(attacker.PublicKey);

            //Action
            var sgndTrans1 = attacker.SignMessage(trans1);
            var destiny1 = new Person();
            var trans2 = trans1.Payto(sgndTrans1, destiny1.PublicKey);
            var destiny2 = new Person();
            var trans3 = trans1.Payto(sgndTrans1, destiny2.PublicKey);

            //Assert
            try
            {
                if (trans2.isValidSignedMsg() && trans3.isValidSignedMsg())
                    throw new Exception("its not allowed to double spend the same coin");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
