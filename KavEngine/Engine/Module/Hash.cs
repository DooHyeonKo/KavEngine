using Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class Hash
    {
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "Hash Scan Engine";

        public List<string> HashList = new List<string>();
        public List<string> Md5List = new List<string>();

        KavSignature cSignFile = new KavSignature();

        public void LoadVDB()
        {
            cSignFile.LoadVDB(HashList, KavConfig.sDailyVdbPath);
            cSignFile.LoadVDB(Md5List, KavConfig.sMainVirusDB);

            KavConfig.VirusNumber += HashList.Count;
            KavConfig.VirusNumber += Md5List.Count;
        }

        public KavKernel.ScanResult ScanFile(byte[] buffer, string filepath, int filesize)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            string sHash = KavHash.CalculateMd5Buffer(buffer).ToLower();

            int Size = filesize;

            foreach (string MainHashList in HashList)
            {
                if (MainHashList.Contains(sHash))
                {
                    string[] strData = MainHashList.Split(new string[]
                    {
                             sHash.ToString() + "," +Size.ToString() + ","
                    }, StringSplitOptions.None);

                    for (int j = 0; j < strData.Length; j++)
                    {
                        string strVirusName = strData[j].Trim();

                        if (strVirusName.Contains(","))
                        {
                            string[] sVirusName = strVirusName.Split(',');

                            VirusName = sVirusName[2];
                            FileLocation = filepath;
                        }
                        else
                        {
                            VirusName = strVirusName;
                            FileLocation = filepath;
                        }
                    }

                    mScanResult = KavKernel.ScanResult.Infected;
                }
            }


            PeFile pe = new PeFile(buffer);

            string sHash2 = KavHash.CalculateMd5Buffer(buffer);
            int Size2 = filesize;

            if (Md5List.Contains(sHash2.ToUpper()))
            {
            
                if (pe.Is32Bit)
                {
                    VirusName = $"Malware.Win32.Generic (A)";
                    FileLocation = filepath;
                    mScanResult = KavKernel.ScanResult.Infected;
                }
                else if (pe.Is64Bit)
                {
                    VirusName = $"Malware.Win64.Generic (A)";
                    FileLocation = filepath;
                    mScanResult = KavKernel.ScanResult.Infected;
                }
                else if (!pe.IsEXE)
                {
                    VirusName = $"Malware.Generic (A)";
                    FileLocation = filepath;
                    mScanResult = KavKernel.ScanResult.Infected;
                }
            }
            else if (Md5List.Contains(sHash2.ToLower()))
            {
                if (pe.Is32Bit)
                {
                    VirusName = $"Malware.Win32.Generic (B)";
                    FileLocation = filepath;
                    mScanResult = KavKernel.ScanResult.Infected;
                }
                else if (pe.Is64Bit)
                {
                    VirusName = $"Malware.Win64.Generic (B)";
                    FileLocation = filepath;
                    mScanResult = KavKernel.ScanResult.Infected;
                }
                else if (!pe.IsEXE)
                {
                    VirusName = $"Malware.Generic (B)";
                    FileLocation = filepath;
                    mScanResult = KavKernel.ScanResult.Infected;
                }
            }

            return mScanResult;
        }
    }
}
