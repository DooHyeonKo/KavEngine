using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class PDF
    {
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "PDF Scan Engine";

        public KavKernel.ScanResult ScanFile(byte[] buffer, string FilePath, string extensions)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            if (extensions.Contains("pdf"))
            {
                bool IsPdf = IsPdfFile(buffer);

                if (IsPdf)
                {
                    var enc = new ASCIIEncoding();
                    var header = enc.GetString(buffer);

                    if (Regex.IsMatch(header, @"^s*%PDF-1.")) // Is PDF Header
                    {
                        if (Regex.IsMatch(header, @"this\.exportDataObject.+?cName:.+?nLaunch"))
                        {
                            VirusName = "Trojan-PDF/Generic";
                            FileLocation = FilePath;

                            //cQuarantineFile.AddQuarantineFile(FilePath, "PDF/Generic");

                            mScanResult = KavKernel.ScanResult.Infected;
                        }

                    }

                }
            }
            else
            {
                mScanResult = KavKernel.ScanResult.NotFoundPath;
            }

            return mScanResult;
        }

        private bool IsPdfFile(byte[] buffer)
        {
            bool IsPdf = false;

            if (buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46 && buffer[4] == 2D)
            {
                IsPdf = true;
            }

            return IsPdf;
        }
    }
}
