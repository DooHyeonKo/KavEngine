using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class Daily
    {
        public List<string> Sha256List = new List<string>();
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "Daily Scan Engine";

        KavSignature cSignFile = new KavSignature();
        public void LoadVDB()
        {
            cSignFile.LoadVDB(Sha256List, KavConfig.sSHA256VdbPath);
        }

        public KavKernel.ScanResult ScanFile(byte[] buffer, string filepath)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            var m_BufferHash = KavHash.CalculateSHA256Buffer(buffer);

            foreach (var m_VirusList in Sha256List)
            {
                var VirusData = m_VirusList.Split(':');

                if (VirusData[0] == m_BufferHash)
                {
                    VirusName = VirusData[1];
                    FileLocation = filepath;
                }
            }

            return mScanResult;
        }
    }
}
