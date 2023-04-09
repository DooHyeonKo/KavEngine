using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class Doc
    {
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "Docuemnt Scan Engine";

        public string contentTypesXML = "";


        public KavKernel.ScanResult ScanFile(string FilePath)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            byte[] Signature = GetFileSignature(FilePath);

            if (Signature[0] == 0x50 && Signature[1] == 0x4B)
            {
                using (ZipArchive zArchive = ZipFile.OpenRead(FilePath))
                {
                    foreach (ZipArchiveEntry zEntry in zArchive.Entries)
                    {
                        switch (zEntry.Name)
                        {
                            case "[Content_Types].xml":
                                {
                                    BinaryReader Reader = new BinaryReader(zEntry.Open());
                                    byte[] Buff = Reader.ReadBytes((int)zEntry.Length);
                                    contentTypesXML = Encoding.ASCII.GetString(Buff);
                                }
                                break;
                        }
                    }
                }
            }
            

            return mScanResult;
        }

        private byte[] GetFileSignature(string FilePath)
        {
            byte[] buffer = null;
            
            Stream fStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader bReader = new BinaryReader(fStream);

            buffer = bReader.ReadBytes(5);

            return buffer;
        }
    }
}
