namespace GoofyCoin2015
{
    public class Goofy : Person
    {
        public Goofy()
        {
            Global.GoofyPk = mySignature.PublicKey;
        }


        public Transaction CreateCoin(byte[] ownerPk)
        {
            var goofyCoin = new Coin(mySignature);
            return new Transaction(goofyCoin, ownerPk);
        }
    }
}
