using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ApiTester
{
    public class EproCryptoHelper
    {
        public string ComputeHash(byte[] data, string secret)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                byte[] computedHash = hmac.ComputeHash(data);

                return ToHexString(computedHash);
            }
        }
        /// <summary>
        /// Кодира текст в шестнайсетичен код
        /// </summary>
        /// <param name="bytes">Текста за кодиране, 
        /// като масив от байтове</param>
        /// <returns>текст в шестнайсетичен код</returns>
        private string ToHexString(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
