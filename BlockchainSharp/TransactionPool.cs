using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainSharp
{
    public class TransactionPool
    {
        public List<Transaction> _transactions { get; set; }

        public TransactionPool()
        {
            _transactions = new List<Transaction>();
        }

        public void AddTransaction(Transaction transaction)
        {
            _transactions.Add(transaction);
        }
        public bool TransactionExist(Transaction trans)
        {
            foreach (var transaction in _transactions)
            {
                if (transaction.Equals(trans))
                    return true;
            }
            return false;
        }
        public void RemoveFromPool(List<Transaction> transactions)
        {
            List<Transaction> newPoolTransactions = new List<Transaction>();
            foreach (var poolTransaction in _transactions)
            {
                bool insert = true;
                foreach (var transaction in transactions)
                {
                    if (poolTransaction.Equals(transaction))//!!!
                    {
                        insert = false;
                    }
                }
                if (insert)
                {
                    newPoolTransactions.Add(poolTransaction);
                }
            }
            _transactions = newPoolTransactions;
        }
    }
}
