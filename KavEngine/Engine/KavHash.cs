using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KavEngine.Engine
{
    class KavHash
    {
        public static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static string CalculateSHA1(string filename)
        {
            using (var sha1 = SHA1.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = sha1.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static string CalculateSHA256(string filename)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static string CalculateSHA256Buffer(byte[] buffer)
        {
            string Result = "";

            if (buffer != null)
            {
                using (SHA256 Sha256 = SHA256.Create())
                {
                    byte[] BufferData = Sha256.ComputeHash(buffer);

                    StringBuilder Builder = new StringBuilder();

                    for (int i = 0; i < BufferData.Length; i++)
                    {
                        Builder.Append(BufferData[i].ToString("x2"));
                    }

                    Result = Builder.ToString();
                }
            }
            return Result;
        }

        public static string CalculateMd5Buffer(byte[] buffer)
        {
            string Result = "";

            if (buffer != null)
            {
                using (MD5 Md5 = MD5.Create())
                {
                    byte[] BufferData = Md5.ComputeHash(buffer);

                    StringBuilder Builder = new StringBuilder();

                    for (int i = 0; i < BufferData.Length; i++)
                    {
                        Builder.Append(BufferData[i].ToString("x2"));
                    }

                    Result = Builder.ToString();
                }
            }
            return Result;
        }

        public static string CalculateMd5Buffer(string buffer)
        {
            string Result = "";

            if (buffer != null)
            {
                using (MD5 Md5 = MD5.Create())
                {
                    byte[] BufferData = Md5.ComputeHash(Encoding.ASCII.GetBytes(buffer));

                    StringBuilder Builder = new StringBuilder();

                    for (int i = 0; i < BufferData.Length; i++)
                    {
                        Builder.Append(BufferData[i].ToString("x2"));
                    }

                    Result = Builder.ToString();
                }
            }
            return Result;
        }
    }
}
