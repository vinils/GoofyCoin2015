using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofyCoin2015
{
    class Program
    {
        static void Main(string[] args)
        {
            Counter.Coin = 0;
            Counter.Signature = 0;
            Counter.Transaction = 0;

            var goofy = new Goofy(256);
            var alice = new Person(256);
            var bob = new Person(256);
            var clark = new Person(256);

            //goofy transaction
            var goofyTrans = goofy.CreateCoin(alice.PublicKey);

            //alice transaction
            alice.AddTransaction(goofyTrans);
            var aliceTrans = alice.PayTo(bob.PublicKey);


            //bob transaction
            bob.AddTransaction(aliceTrans);
            var bobTrans = bob.PayTo(clark.PublicKey);

            //clark transaction
            clark.AddTransaction(bobTrans);

        }
    }
}
