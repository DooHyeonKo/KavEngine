using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class Dummy
    {
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "Dummy Scan Engine";

        public KavKernel.ScanResult ScanFile(byte[] Buffer, string filepath, int filesize)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            int FileSize = filesize;
            string FileHash = KavHash.CalculateMd5Buffer(Buffer);

            if (FileSize == 53)
            {
                if (FileHash == "1006c84823363962cbc040e42b7c1230")
                {
                    VirusName = "Dummy-Test-File";
                    FileLocation = filepath;
                    //cQuarantineFile.AddQuarantineFile(FilePath, "Dummy-Test-File");
                    mScanResult = KavKernel.ScanResult.Infected;
                }


            }

            return mScanResult;
        }
    }
}
