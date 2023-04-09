using Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class DEX
    {
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "DEX Scan Engine";

        private uint Magic = 0;
        private uint CheckSum = 0;
        private uint SA1 = 0;
        private uint FileSize = 0;
        private uint HeaderSize = 0;
        private uint EndianTag = 0;
        private uint LinkSize = 0;
        private uint LinkOff = 0;
        private uint MapOff = 0;
        private uint sIdsSize = 0;
        private uint sIdsOff = 0;
        private uint tIdsSize = 0;
        private uint tIdsOff = 0;
        private uint pIdsSize = 0;
        private uint pIdsOff = 0;
        private uint fIdsSize = 0;
        private uint fIdsOff = 0;
        private uint mIdsSize = 0;
        private uint mIdsOff = 0;
        private uint cDefSize = 0;
        private uint cDefOff = 0;
        private uint DataSize = 0;
        private uint DataOff = 0;
        private float Entropy = 0;

        public KavKernel.ScanResult ScanFile(byte[] Buff, string filepath)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            if (Buff[0] == 0x64 && Buff[1] == 0x65 && Buff[2] == 0x78) // DEX 파일인가?
            {
                Magic = Convert.ToUInt32(Buff.Slice(0, 8));
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
            else
            {
                mScanResult = KavKernel.ScanResult.NotInfected;
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

            if (Encoding.ASCII.GetString(Buff).Contains("AndroidManifest.xml"))
            {
                if ((int)Entropy > 6)
                {
                    VirusName = "Trojan.Android.Generic." + sIdsSize;
                    FileLocation = filepath;
                    mScanResult = KavKernel.ScanResult.Infected;
                }
            }

            return mScanResult;
        }   
    }
}
