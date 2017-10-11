using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Auth
{
    internal static class Hash
    {
        public static string GetHash(string str)
        {
            //переводим строку в байт-массив 
            Byte[] strBytes = Encoding.Default.GetBytes(str);
            //создаем объект для получения средст шифрования 
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //вычисляем хеш-представление в байтах 
            Byte[] hashBytes = md5.ComputeHash(strBytes);
            //формируем одну цельную строку из массива 
            string hash = string.Empty;
            foreach (var item in hashBytes)
            {
                hash += string.Format("{0:x2}", item);
            }
            return hash;
        }
    }
}
