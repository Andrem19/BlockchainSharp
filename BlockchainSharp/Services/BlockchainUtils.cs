using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainSharp.Services
{
    public static class BlockchainUtils
    {
        public static byte[] Hash(string data)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));
            }
        }
        public static string ByteArrayToString(this byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public static T DeserializeData<T>(Message message)
        {
            return JsonConvert.DeserializeObject<T>(message._data);
        }

    }
}
