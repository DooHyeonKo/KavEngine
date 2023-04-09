using Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class Packer
    {
        private List<string> PackList = new List<string>();

        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "Packer Scan Engine";

        private KavSignature cSignFile = new KavSignature();

        public void LoadVDB()
        {
            cSignFile.LoadVDB(PackList, KavConfig.sPackerDB);
            KavConfig.VirusNumber += PackList.Count;

            for (int i = 0; i < PackList.Count; i++)
            {
                KavConfig.VirusNameList.Add(PackList[i]);
            }
        }

        public KavKernel.ScanResult ScanFile(byte[] buffer, string FilePath)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            try
            {
                PeFile pe = new PeFile(buffer);

                for (int i = 0; i < pe.ImageSectionHeaders.Length; i++)
                {
                    int SigNumber = this.PackList.Count - 1;
                    int SigCount = 0;

                    while (true)
                    {
                        string[] array2 = this.PackList[SigCount].Split(new char[]
                        {
                               ','
                        });

                        if (GetString(pe.ImageSectionHeaders[i].Name).Contains(array2[0]))
                        {
                            mScanResult = KavKernel.ScanResult.Infected;
                            VirusName = array2[1];
                            FileLocation = FilePath;
                        }
                        SigCount++;
                    }
                }
            }
            catch (Exception)
            {

            }

            return mScanResult;
        }

        private string GetString(byte[] buff)
        {
            return Encoding.ASCII.GetString(buff);
        }

    }
}
