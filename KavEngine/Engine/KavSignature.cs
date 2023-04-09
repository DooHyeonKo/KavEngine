using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Ionic.Zlib;

namespace KavEngine.Engine
{
    class KavSignature
    {
        public void LoadVDB(List<string> listVDB,string sVdbPath)
        {
            byte[] m_ReadBuffer = GetFileBuffer(sVdbPath);

            using (var Stream = new ZlibStream(new MemoryStream(m_ReadBuffer), CompressionMode.Decompress))
            {
                using (StreamReader Reader = new StreamReader(Stream))
                {
                    string line = null;

                    while ((line = Reader.ReadLine()) != null)
                    {
                        listVDB.Add(line);
                    }
                }
            }
        }

        public void LoadVDB(string VDB, string sVdbPath)
        {
            byte[] m_ReadBuffer = GetFileBuffer(sVdbPath);

            using (var Stream = new ZlibStream(new MemoryStream(m_ReadBuffer), CompressionMode.Decompress))
            {
                using (StreamReader Reader = new StreamReader(Stream))
                {
                    string line = null;

                    while ((line = Reader.ReadLine()) != null)
                    {
                        VDB += line;
                    }
                }
            }
        }

        public void GetVirusName(uint VirSignLen, string VirSignHash, List<string> VirSignature)
        {
            if (VirSignature.Count != 0)
            {
                if (VirSignature.Contains(VirSignHash))
                {
                    
                }
            }
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

        public void UnLoadVDB(List<string> listVDB)
        {
            listVDB.Clear();
        }
    }
}
