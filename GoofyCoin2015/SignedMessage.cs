using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofyCoin2015
{
    public class SignedMessage
    {
        private String sgndMsg;
        protected String publicKey;

        public String PublicKey
        {
            get { return publicKey; }
            private set { publicKey = value; }
        }

        public String SignedMsg
        {
            get { return sgndMsg; }
            private set { sgndMsg = value; }
        }

        public SignedMessage(Signature mySignature, String signedMsg)
        {
            PublicKey = mySignature.PublicKey;
            SignedMsg = signedMsg;
        }

        public Boolean isValidSignedMsg()
        {
            return PublicKey[0] == SignedMsg[0];
        }
    }
}
