using System;
using System.Security.Cryptography;
using System.Text;

namespace BankingApplication.Web.Utility
{
    public static class Helper
    {
        public static string GenerateHashedPassword(string pwd)
        {
            var hashedPassword = string.Empty;

            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(pwd));
                var strResult = BitConverter.ToString(result);
                hashedPassword = strResult.Replace("-", "");
            }

            return hashedPassword;
        }

        public static string GenerateNumber(string loginName, string name)
        {
            string prefix = "11";
            return (prefix + loginName.Substring(0, 4) + name.Substring(0, 4)).ToLower();
        }
    }
}
