using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Utils
{
    class FileUtils
    {

        public static byte[] GetFileBuffer(string FilePath)
        {
            byte[] buffer = null;

            FileStream Fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader Reader = new BinaryReader(Fs);

            long Size = new FileInfo(FilePath).Length;
            buffer = Reader.ReadBytes((int)Size);

            return buffer;
        }
    }
}
