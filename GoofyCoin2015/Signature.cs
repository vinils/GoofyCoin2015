using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofyCoin2015
{
    public class Signature
    {
        private String secretKey;
        protected String publicKey;

        public String PublicKey
        {
            get { return publicKey; }
            protected set { publicKey = value; }
        }

        public Signature(Int32 sizeKey)
        {
            secretKey = Counter.Signature.ToString();
            PublicKey = secretKey;
        }

        public SignedMessage SignMessage(Coin coin)
        {
            return SignMessage(Counter.Coin.ToString());
        }

        public SignedMessage SignMessage(Transaction transaction)
        {
            return SignMessage(Counter.Transaction.ToString());
        }

        private SignedMessage SignMessage(String message)
        {
            return new SignedMessage(this, secretKey + message);
        }
    }
}
