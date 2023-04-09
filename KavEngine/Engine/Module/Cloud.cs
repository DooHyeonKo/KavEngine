using Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class Cloud
    {
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "Cloud Scan Engine";

        public KavKernel.ScanResult ScanFile(byte[] buffer, string filepath)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            string sCloudScanData = ScanFileFromCloud(buffer);

            if (sCloudScanData != "NotFound")
            {
                VirusName = sCloudScanData + " (Cloud)";
                FileLocation = filepath;
                //cQuarantineFile.AddQuarantineFile(FilePath, sCloudScanData + " (Cloud)");
                mScanResult = KavKernel.ScanResult.Infected;
            }

            return mScanResult;
        }

        private string GetCertificateHash(byte[] buffer)
        {
            X509Certificate2 Certificate2 = new X509Certificate2(buffer);

            string StrNameInfo = Certificate2.GetNameInfo(X509NameType.SimpleName, false);

            string sHash = KavHash.CalculateMd5Buffer(StrNameInfo).ToUpper();

            return sHash;
        }

        private bool IsCheckSignedFile(byte[] buffer)
        {
            bool IsSigned = false;

            try
            {
                X509Certificate2 Certificate2 = new X509Certificate2(buffer);
                IsSigned = true;
            }
            catch (Exception)
            {
                IsSigned = false;
            }
            return IsSigned;
        }

        private string GetWebData(string URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse respone = (HttpWebResponse)request.GetResponse();


            StreamReader reader = new StreamReader(respone.GetResponseStream(), Encoding.Default);

            string StrHtml = reader.ReadToEnd();

            reader.Close();
            respone.Close();

            return StrHtml;
        }

        byte[] GetFileBuffer(string FilePath)
        {
            byte[] buffer = null;

            FileStream Fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader Reader = new BinaryReader(Fs);

            long Size = new FileInfo(FilePath).Length;
            buffer = Reader.ReadBytes((int)Size);

            return buffer;
        }

        private string GetFileHashPE(byte[] buffer)
        {
            string data = "";

            try
            {
                byte[] buff = buffer;

                PeFile pe = new PeFile(buff);

                int SectionLengths = pe.ImageSectionHeaders.Length;

                for (int i = 0; i < SectionLengths - 1; i++)
                {
                    uint PointerToRawData = pe.ImageSectionHeaders[i].PointerToRawData;
                    uint SizeOfRawData = pe.ImageSectionHeaders[i].SizeOfRawData;
                    uint VirtualAddress = pe.ImageSectionHeaders[i].VirtualAddress;
                    uint VirtualSize = pe.ImageSectionHeaders[i].VirtualSize;
                    byte[] SectionBuffer = buff.Slice((int)PointerToRawData, (int)PointerToRawData + (int)SizeOfRawData);
                    int entropy = (int)getEntropyValue(SectionBuffer);

                    string func = "";
                    string dll = "";


                    if (pe.ImportedFunctions != null)
                    {
                        foreach (ImportFunction importFunction in pe.ImportedFunctions)
                        {
                            dll += importFunction.DLL + ",";
                            func += importFunction.Name + ",";
                        }
                    }

                    data = string.Format("{0}{1}{2}", func, dll, entropy);

                    data = KavHash.CalculateMd5Buffer(Encoding.ASCII.GetBytes(data));
                }

            }
            catch (Exception)
            {

            }

            return data;
        }

        private float getEntropyValue(byte[] c)
        {
            int[] numArray = new int[0x100];
            byte[] buffer = c;
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

            return entropy;
        }

        private string ScanFileFromCloud(byte[] buffer)
        {
            string ScanResult = "";
            string sHash = "";
            string sPeHash = "";
            PeFile pe = new PeFile(buffer);

            byte[] FileBuffer = buffer;
            int SectionLengths = pe.ImageSectionHeaders.Length;

            for (int i = 0; i < SectionLengths - 1; i++)
            {
                uint PointerToRawData = pe.ImageSectionHeaders[i].PointerToRawData;
                uint SizeOfRawData = pe.ImageSectionHeaders[i].SizeOfRawData;

                byte[] SectionBuffer = FileBuffer.Slice((int)PointerToRawData, (int)PointerToRawData + (int)SizeOfRawData);

                sHash = KavHash.CalculateMd5Buffer(SectionBuffer);

                if (sHash != "d41d8cd98f00b204e9800998ecf8427e")
                {
                    sPeHash = SizeOfRawData + sHash;
                }
            }


            string sPeMd5File = sPeHash;
            string PeUrl = KavConfig.sCloudScanURL + sPeMd5File.Replace(",", "");

            string sWebData = "";
            sWebData = GetWebData(PeUrl);

            if (sWebData != "NotFound")
            {
                ScanResult = GetVirusType(sWebData) + ".Generic";
            }
            else
            {
                ScanResult = "NotFound";

                if (IsCheckSignedFile(buffer))
                {
                    string sCertificateMd5File = GetCertificateHash(buffer);

                    sWebData = GetWebData(KavConfig.sCloudScanURL + sCertificateMd5File);

                    if (sWebData != "NotFound")
                    {
                        ScanResult = GetVirusType(sWebData) + ".Generic"; ;
                    }
                }

                string sApiHash = GetFileHashPE(buffer);

                sWebData = GetWebData(KavConfig.sCloudScanURL + sApiHash);

                if (!sWebData.Contains("NotFound"))
                {
                    if (GetVirusType(sWebData) != null)
                    {
                        ScanResult = "Heur." + GetVirusType(sWebData) + ".Generic";

                    }
                    else
                    {
                        ScanResult = "Heur" + "Generic";
                    }
                }
                else
                {
                    ScanResult = "NotFound";
                }
            }

            return ScanResult;
        }

        private string GetVirusType(string sWebData)
        {
            string ScanResult = "";

            if (sWebData.Contains("Trojan"))
            {
                ScanResult = "Trojan";
            }
            else if (sWebData.Contains("Hoax"))
            {
                ScanResult = "Hoax";
            }
            else if (sWebData.Contains("Backdoor"))
            {
                ScanResult = "Backdoor";
            }
            else if (sWebData.Contains("Trojan-Spy"))
            {
                ScanResult = "Trojan-Spy";
            }
            else if (sWebData.Contains("Trojan-Downloader"))
            {
                ScanResult = "Trojan-Downloader";
            }
            else if (sWebData.Contains("Trojan-PSW"))
            {
                ScanResult = "Trojan-PSW";
            }
            else if (sWebData.Contains("Trojan-Dropper"))
            {
                ScanResult = "Trojan-Dropper";
            }
            else if (sWebData.Contains("Trojan-DDoS"))
            {
                ScanResult = "Trojan-DDoS";
            }
            else if (sWebData.Contains("Trojan-Ransom"))
            {
                ScanResult = "Trojan-Ransom";
            }
            else if (sWebData.Contains("Exploit"))
            {
                ScanResult = "Exploit";
            }
            else if (sWebData.Contains("Email-Flooder"))
            {
                ScanResult = "Email-Flooder";
            }
            else if (sWebData.Contains("HackTool"))
            {
                ScanResult = "HackTool";
            }
            else if (sWebData.Contains("Email-Worm"))
            {
                ScanResult = "Email-Worm";
            }
            else if (sWebData.Contains("Trojan-FakeAV"))
            {
                ScanResult = "Trojan-FakeAV";
            }
            else if (sWebData.Contains("Trojan-Clicker"))
            {
                ScanResult = "Trojan-Clicker";
            }
            else if (sWebData.Contains("Worm"))
            {
                ScanResult = "Worm";
            }
            else if (sWebData.Contains("Trojan-Banker"))
            {
                ScanResult = "Trojan-Banker";
            }
            else if (sWebData.Contains("Trojan-Proxy"))
            {
                ScanResult = "Trojan-Proxy";
            }
            else if (sWebData.Contains("AdWare"))
            {
                ScanResult = "AdWare";
            }
            else if (sWebData.Contains("Virus"))
            {
                ScanResult = "Virus";
            }
            else if (sWebData.Contains("PUP"))
            {
                ScanResult = "PUP";
            }
            else if (sWebData.Contains("Malware"))
            {
                ScanResult = "Malware";
            }

            return ScanResult;
        }
    }
}
