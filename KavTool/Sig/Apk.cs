using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavTool.Sig
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

        public void GetApkHeader(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                using (var mZipFile = ZipFile.OpenRead(FilePath))
                {
                    foreach (var mZipEntry in mZipFile.Entries)
                    {
                        BinaryReader m_BinaryReader = new BinaryReader(mZipEntry.Open());

                        byte[] Buff = m_BinaryReader.ReadBytes((int)mZipEntry.Length);
                        GetDexHeaderBuffer(Buff);

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
                }
            }
        }

        private void GetDexHeader(string FilePath)
        {
            byte[] Buff = null;

            FileStream Fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader Reader = new BinaryReader(Fs);

            long Size = new FileInfo(FilePath).Length;
            Buff = Reader.ReadBytes((int)Size);

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
        }


        private void GetDexHeaderBuffer(byte[] Buff)
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
        }
    }
}

