using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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

        public static bool SignatureValid(string data, byte[] signature, byte[] publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                //RSAParameters p = rsa.ExportParameters(false);
                //p.Modulus = publicKey;
                //p.Exponent = publicKey;
                //rsa.ImportParameters(p);
                rsa.ImportSubjectPublicKeyInfo(publicKey, out _);
                RSAPKCS1SignatureDeformatter rsaDeformatter = new(rsa);
                rsaDeformatter.SetHashAlgorithm(nameof(SHA256));
                byte[] dataHash = BlockchainUtils.Hash(data);

                bool res = rsaDeformatter.VerifySignature(dataHash, signature);
                return res;
            }
        }

        public byte[] GetPublicKeyBytes()
        {
            byte[] publicKey = _rsa.ExportSubjectPublicKeyInfo();
            return publicKey;
        }
        public Transaction CreateTransaction(string receiver, decimal amount, string type)
        {
            Transaction transaction = new Transaction(GetPublicKeyBytes(), receiver, amount, type);
            byte[] signature = Sign(transaction.Payload());
            transaction.Sign(signature);
            return transaction;
        }
        public Block CreateBlock(List<Transaction> transactions, string lastHash, int blockCount)
        {
            var block = new Block(transactions, lastHash, GetPublicKeyBytes(), blockCount);
            var signature = Sign(block.Payload());
            block.Sign(signature);
            return block;
        }
    }
}
