using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainSharp
{
    public class TransactionPool
    {
        public List<Transaction> transactions = new List<Transaction>();

        public void AddTransaction(Transaction transaction)
        {
            transactions.Add(transaction);
        }
        public bool TransactionExist(Transaction trans)
        {
            foreach (var transaction in transactions)
            {
                if (transaction.Equals(trans))
                    return true;
            }
            return false;
        }
    }
}
