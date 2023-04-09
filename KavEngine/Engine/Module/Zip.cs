using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class Zip
    {
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "Zip Scan Engine";

        public KavKernel.ScanResult ScanFile(string FilePath)
        {
            KavKernel.ScanResult ScanResult = KavKernel.ScanResult.NotInfected;

            if (File.Exists(FilePath))
            {
                if (Path.GetExtension(FilePath).Contains(".zip"))
                {
                    using (var Zip = ZipFile.OpenRead(FilePath))
                    {
                        foreach (ZipArchiveEntry entry in Zip.Entries)
                        {
                            if (entry.FullName.Contains(".jpg.exe") && entry.FullName.Contains(".png.exe"))
                            {
                                VirusName = "Archive/Generic";
                                FileLocation = FilePath + $" ({entry.FullName})";
                                ScanResult = KavKernel.ScanResult.Infected;
                            }
                        }
                    }
                }
            }
            else
            {
                ScanResult = KavKernel.ScanResult.NotFoundPath;
            }

            return ScanResult;
        }
    }
}
