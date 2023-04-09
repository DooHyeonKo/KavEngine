using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class Signature
    {
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "Signature Scan Engine";
        public List<string> SignList = new List<string>();
        public Dictionary<string, string> dicSignature = new Dictionary<string, string>();

        KavSignature cSignFile = new KavSignature();

        public void LoadVDB()
        {
            cSignFile.LoadVDB(SignList, KavConfig.sSignatureVdbPath);

            for (int i = 0; i < SignList.Count; i++)
            {
                string[] Signature = SignList[i].Split(',');

                if (!dicSignature.ContainsKey(Signature[0]) && !dicSignature.ContainsValue(Signature[1]))
                {
                    dicSignature.Add(Signature[0], Signature[1]); // HEX, Virus Name
                }
            }


            KavConfig.VirusNumber += SignList.Count;
        }

        public KavKernel.ScanResult ScanFile(byte[] buffer, string FilePath)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            try
            {
                if (this.IsPeHeader(buffer))
                {
                    byte[] ReadBuffer = buffer;
                    string StrBuffer = BitConverter.ToString(ReadBuffer).Replace("-", string.Empty);

                    foreach (KeyValuePair<string, string> dicItem in dicSignature)
                    {
                        if (StrBuffer.Contains(dicItem.Key))
                        {
                            VirusName = "Sig-" + dicItem.Value;
                            FileLocation = FilePath;
                            mScanResult = KavKernel.ScanResult.Infected;
                        }
                    }

                }
            }
            catch (Exception)
            {
            }

            return mScanResult;
        }

        public bool IsPeHeader(byte[] buffer)
        {
            var enc = new ASCIIEncoding();
            var header = enc.GetString(buffer);
            //%PDF−1.0
            // If you are loading it into a long, this is (0x04034b50).
            if (buffer[0] == 0x4D && buffer[1] == 0x5A)
            {
                return header.StartsWith("MZ");
            }
            return false;
        }
    }
}
