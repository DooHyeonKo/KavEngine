using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.NeuralNetwork.Feature
{
    class Apk
    {
        public uint CheckSum = 0;
        public uint SA1 = 0;
        public uint FileSize = 0;
        public uint HeaderSize = 0;
        public uint EndianTag = 0;
        public uint LinkSize = 0;
        public uint LinkOff = 0;
        public uint MapOff = 0;
        public uint sIdsSize = 0;
        public uint sIdsOff = 0;
        public uint tIdsSize = 0;
        public uint tIdsOff = 0;
        public uint pIdsSize = 0;
        public uint pIdsOff = 0;
        public uint fIdsSize = 0;
        public uint fIdsOff = 0;
        public uint mIdsSize = 0;
        public uint mIdsOff = 0;
        public uint cDefSize = 0;
        public uint cDefOff = 0;
        public uint DataSize = 0;
        public uint DataOff = 0;
        public float Entropy = 0;

        public void GetAPKInfo(string FilePath)
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    using (var _Zip = ZipFile.OpenRead(FilePath))
                    {
                        foreach (ZipArchiveEntry _Entry in _Zip.Entries)
                        {
                            if (_Entry.Name == "classes.dex")
                            {
                                BinaryReader Reader = new BinaryReader(_Entry.Open());

                                byte[] buff = Reader.ReadBytes((int)_Entry.Length);
                                GetDexHeader(buff);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void GetDexHeader(string FilePath)
        {
            byte[] Buff = null;

            FileStream Fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader Reader = new BinaryReader(Fs);

            long Size = new FileInfo(FilePath).Length;
            Buff = Reader.ReadBytes((int)Size);

            if (Buff[0] == 0x64 && Buff[1] == 0x65 && Buff[2] == 0x78) // DEX 파일인가?
            {
                GetDexHeader(Buff);
            }

            int[] numArray = new int[0x100];
            byte[] buffer = Buff;
            for (int i = 0; i < 0x100; i++)
            {
                numArray[i] = 0;
            }

            for (int j = 0; j < (buffer.Length - 1); j++)
            {
                int index = buffer[j];
                numArray[index]++;
            }
            int length = buffer.Length;
            float entropy = 0f;
            for (int k = 0; k < 0x100; k++)
            {
                if ((numArray[k] != 0) && (k != 0))
                {
                    entropy += (-float.Parse(numArray[k].ToString()) / float.Parse(length.ToString())) * float.Parse(Math.Log((double)(float.Parse(numArray[k].ToString()) / float.Parse(length.ToString())), 2.0).ToString());
                }
            }

            Entropy = entropy;
        }

        public void GetDexHeader(byte[] Buff)
        {
            if (Buff[0] == 0x64 && Buff[1] == 0x65 && Buff[2] == 0x78) // DEX 파일인가?
            {
                CheckSum = Convert.ToUInt32(Buff.Slice(8, 0xC)[0]);
                SA1 = Convert.ToUInt32(Buff.Slice(0xC, 0x20)[0]);
                FileSize = Convert.ToUInt32(Buff.Slice(0x20, 0x24)[0]);
                HeaderSize = Convert.ToUInt32(Buff.Slice(0x24, 0x28)[0]);
                EndianTag = Convert.ToUInt32(Buff.Slice(0x28, 0x2C)[0]);
                LinkSize = Convert.ToUInt32(Buff.Slice(0x2C, 0x30)[0]);
                LinkOff = Convert.ToUInt32(Buff.Slice(0x30, 0x34)[0]);
                MapOff = Convert.ToUInt32(Buff.Slice(0x34, 0x38)[0]);
                sIdsSize = Convert.ToUInt32(Buff.Slice(0x38, 0x3C)[0]); // 전체 문자열 개수
                sIdsOff = Convert.ToUInt32(Buff.Slice(0x3C, 0x40)[0]); // 전체 문자열의 시작 위치
                tIdsSize = Convert.ToUInt32(Buff.Slice(0x40, 0x44)[0]);
                tIdsOff = Convert.ToUInt32(Buff.Slice(0x44, 0x48)[0]);
                pIdsSize = Convert.ToUInt32(Buff.Slice(0x48, 0x4C)[0]);
                pIdsOff = Convert.ToUInt32(Buff.Slice(0x4C, 0x50)[0]);
                fIdsSize = Convert.ToUInt32(Buff.Slice(0x50, 0x54)[0]);
                fIdsOff = Convert.ToUInt32(Buff.Slice(0x54, 0x58)[0]);
                mIdsSize = Convert.ToUInt32(Buff.Slice(0x58, 0x5C)[0]);
                mIdsOff = Convert.ToUInt32(Buff.Slice(0x5C, 0x60)[0]);
                cDefSize = Convert.ToUInt32(Buff.Slice(0x60, 0x64)[0]);
                cDefOff = Convert.ToUInt32(Buff.Slice(0x64, 0x68)[0]);
                DataSize = Convert.ToUInt32(Buff.Slice(0x68, 0x6C)[0]);
                DataOff = Convert.ToUInt32(Buff.Slice(0x6C, 0x70)[0]);
            }

            int[] numArray = new int[0x100];
            byte[] buffer = Buff;
            for (int i = 0; i < 0x100; i++)
            {
                numArray[i] = 0;
            }

            for (int j = 0; j < (buffer.Length - 1); j++)
            {
                int index = buffer[j];
                numArray[index]++;
            }
            int length = buffer.Length;
            float entropy = 0f;
            for (int k = 0; k < 0x100; k++)
            {
                if ((numArray[k] != 0) && (k != 0))
                {
                    entropy += (-float.Parse(numArray[k].ToString()) / float.Parse(length.ToString())) * float.Parse(Math.Log((double)(float.Parse(numArray[k].ToString()) / float.Parse(length.ToString())), 2.0).ToString());
                }
            }

            Entropy = entropy;
        }

        public string CalculateMd5Buffer(byte[] buffer)
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

        public string CalculateSHA1Buffer(byte[] buffer)
        {
            string Result = "";

            if (buffer != null)
            {
                using (SHA1 Sha1 = SHA1.Create())
                {
                    byte[] BufferData = Sha1.ComputeHash(buffer);

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

        public string CalculateSHA256Buffer(byte[] buffer)
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

        public bool IsApkHeader(byte[] buffer)
        {
            if (Encoding.ASCII.GetString(buffer).Contains("AndroidManifest.xml"))
                return true;
            else
                return false;
        }

        public bool IsDexHeader(byte[] buffer)
        {
            if (buffer[0] == 0x64 && buffer[1] == 0x65 && buffer[2] == 0x78)
                return true;
            else
                return false;
        }

        public float[] GetFeatureMatrix(string FilePath)
        {
            byte[] Buff = null;

            FileStream Fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader Reader = new BinaryReader(Fs);

            long Size = new FileInfo(FilePath).Length;
            Buff = Reader.ReadBytes((int)Size);

            if (IsApkHeader(Buff))
            {
                GetAPKInfo(FilePath);
            }
            else if (IsDexHeader(Buff))
            {
                GetDexHeader(Buff);
            }

            float[] Matrix = new float[]
            {
                (float)CheckSum,
                (float)SA1,
                (float)FileSize,
                (float)HeaderSize,
                (float)EndianTag,
                (float)LinkSize,
                (float)LinkOff,
                (float)MapOff,
                (float)sIdsSize,
                (float)sIdsOff,
                (float)tIdsSize,
                (float)tIdsOff,
                (float)pIdsSize,
                (float)pIdsOff,
                (float)fIdsSize,
                (float)fIdsOff,
                (float)mIdsSize,
                (float)mIdsOff,
                (float)cDefSize,
                (float)cDefOff,
                (float)DataSize,
                (float)DataOff,
                (float)Entropy
            };

            return Matrix;
        }

    }
}
