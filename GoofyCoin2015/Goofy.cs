using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofyCoin2015
{
    public class Goofy : Person
    {
        public Goofy(Int32 sizeKey)
            : base(sizeKey)
        {
            Global.GoofyPk = mySignature.PublicKey;
        }


        public Transaction CreateCoin(String ownerPk)
        {
            var goofyCoin = new Coin(mySignature);
            return new Transaction(goofyCoin, ownerPk);
        }
    }
}
