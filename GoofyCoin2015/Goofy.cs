namespace GoofyCoin2015
{
    public class Goofy : Person
    {
        public Goofy()
        {
            Global.GoofyPk = mySignature.PublicKey;
        }


        public TransferListCreateCoin CreateCoin(byte[] ownerPk)
        {
            return new TransferListCreateCoin(ownerPk);
        }
    }
}
