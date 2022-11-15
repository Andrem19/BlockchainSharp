using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BlockchainSharp.Services;

namespace BlockchainSharp
{
    public class Wallet
    {
        private RSA _rsa;
        public Wallet()
        {
            _rsa = RSA.Create();
        }
        public byte[] Sign(string data)
        {
            byte[] dataHash = BlockchainUtils.Hash(data);
            RSAPKCS1SignatureFormatter rsaFormatter = new(_rsa);
            rsaFormatter.SetHashAlgorithm(nameof(SHA256));
            byte[] signedHash = rsaFormatter.CreateSignature(dataHash);
            return signedHash;
        }

        public static bool SignatureValid(string data, string signature, string publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                byte[] publicKeyBytes = Convert.FromBase64String(publicKey);
                rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
                RSAPKCS1SignatureDeformatter rsaDeformatter = new(rsa);
                rsaDeformatter.SetHashAlgorithm(nameof(SHA256));
                byte[] dataHash = BlockchainUtils.Hash(data);
                byte[] signatureBytes = Convert.FromBase64String(signature);
                bool res = rsaDeformatter.VerifySignature(dataHash, signatureBytes);
                return res;
            }
        }

        public string GetPublicKeyString()
        {
            byte[] publicKeyByte = _rsa.ExportSubjectPublicKeyInfo();
            string publicKeyString = Convert.ToBase64String(publicKeyByte);
            return publicKeyString;
        }
        public Transaction CreateTransaction(string receiver, decimal amount, string type)
        {
            Transaction transaction = new Transaction(GetPublicKeyString(), receiver, amount, type);
            byte[] signature = Sign(transaction.Payload());
            string signatureString = Convert.ToBase64String(signature);
            transaction.Sign(signatureString);
            return transaction;
        }
        public Block CreateBlock(List<Transaction> transactions, string lastHash, int blockCount)
        {
            var block = new Block(transactions, lastHash, GetPublicKeyString(), blockCount);
            var signature = Sign(block.Payload());
            string signatureString = Convert.ToBase64String(signature);
            block.Sign(signatureString);
            return block;
        }
    }
}
