using System;
using System.Text;

namespace Tracking.Common.Extensions
{
    public static class StringExtensions
    {
        public static string Base64Encode(this string plainText)
        {

            try
            {
                if (string.IsNullOrEmpty(plainText))
                {
                    return null;
                }

                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception e)
            {

                throw new Exception(e.ToString());
            }
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            if (string.IsNullOrWhiteSpace(base64EncodedData)) return null;
            try
            {
                string working = base64EncodedData.Replace('-', '+').Replace('_', '/'); ;
                while (working.Length % 4 != 0)
                {
                    working += '=';
                }
                byte[] base64EncodedBytes = Convert.FromBase64String(working);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}