using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofyCoin2015
{
    public class Person
    {
        protected Signature mySignature;
#warning pensar melhor sobre a estrutura de armazenamento das transacoes, em vez de lista utilizar uma linked list ou implementar um arraylist
        private List<Transaction> wallet = new List<Transaction>();

        public String PublicKey
        {
            get { return mySignature.PublicKey; }
        }

        public Person(Int32 sizeKey)
        {
            mySignature = new Signature(sizeKey);
        }

        public void AddTransaction(Transaction trans)
        {
            trans.CheckTransaction();
            wallet.Add(trans);
        }

        public Transaction PayTo(String publicKey)
        {
            var trans = wallet.Last();
            var sgndTrans = mySignature.SignMessage(trans);
            var paidTransaction = trans.Payto(sgndTrans, publicKey);
            wallet.Remove(trans);

            return paidTransaction;
        }
    }
}
