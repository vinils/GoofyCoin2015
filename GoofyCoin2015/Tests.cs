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
            var signature = new Signature(256);
            Global.GoofyPk = signature.PublicKey;

            var coin = new Coin(signature);

            //Act
            var trans = new Transaction(coin, new Signature(256).PublicKey);

            //Assert

            try
            {
                //trans.CheckTransaction();

                if (!coin.isGoofyCoin())
                    throw new Exception("This coin doenst belong to Goofy");
                if (!coin.isValidSignature())
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
            var person2 = new Person();

            Global.GoofyPk = goofy.PublicKey;

            var trans1 = goofy.CreateCoin(person1.PublicKey);

            //Action
            var sgndTrans1 = person1.SignMessage(trans1);

            var trans2 = trans1.Payto(sgndTrans1, person2.PublicKey);

            //Assert
            try
            {
                //trans2.isValidTransaction();  //previous.receiverPk == previousTransSignedByMe.PublicKey;
                trans2.CheckTransaction();

                if (!sgndTrans1.isValidSignedMsg(trans1))
                    throw new Exception("The signature of the previous transaction and his pk are invalid");
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
            var person3 = new Person();

            Global.GoofyPk = goofy.PublicKey;

            var trans1 = goofy.CreateCoin(person1.PublicKey);

            //Action
            person1.AddTransaction(trans1);

            var trans2 = person1.PayTo(person2.PublicKey);
            person2.AddTransaction(trans2);

            var trans3 = person2.PayTo(person3.PublicKey);

            //Assert
            try
            {
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
            var person1 = new Signature(256);
            var person2 = new Person();
            var person3 = new Person();

            Global.GoofyPk = goofy.PublicKey;

            var trans1 = goofy.CreateCoin(person1.PublicKey);

            //Action
            var sgndTrans1 = person1.SignMessage(trans1);
            var trans2 = trans1.Payto(sgndTrans1, person2.PublicKey);
            var trans3 = trans1.Payto(sgndTrans1, person3.PublicKey);

            //Assert
            try
            {
                trans2.CheckTransaction();
                trans3.CheckTransaction();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
