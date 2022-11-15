using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainSharp
{
    public class AccountModel
    {
        public List<string> _accounts { get; set; }
        public Dictionary<string, decimal> _balances { get; set; }

        public AccountModel()
        {
            _accounts= new List<string>();
            _balances= new Dictionary<string, decimal>();
        }

        public void AddAccount(string publicKeyString)
        {
            if (!_accounts.Contains(publicKeyString))
                _accounts.Add(publicKeyString);
            _balances[publicKeyString] = 0;
        }

        public decimal GetBalance(string publicKeyString)
        {
            if(!_balances.ContainsKey(publicKeyString))
                AddAccount(publicKeyString);
            return _balances[publicKeyString];
        }
        public void UpdateBalance(string publicKeyString, decimal amount)
        {
            if(!_accounts.Contains(publicKeyString))
                AddAccount(publicKeyString);
            _balances[publicKeyString] += amount;
        }
    }
}
