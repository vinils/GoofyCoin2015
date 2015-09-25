namespace GoofyCoin2015
{
    public class Goofy : Person
    {
        public Goofy()
        {
            Global.GoofyPk = mySignature.PublicKey;
        }


        public GoofyTransfer CreateCoin(byte[] ownerPk)
        {
            var goofyCoin = new Coin(mySignature);
            return new GoofyTransfer(goofyCoin, ownerPk);
        }
    }
}
