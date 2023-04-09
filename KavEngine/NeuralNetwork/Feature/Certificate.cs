using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.NeuralNetwork.Feature
{
    class Certificate
    {
        public bool IsCertificate = false;
        public string Thumbprint = null;
        public string Subject = null;
        public string CertificateName = null;
        public byte[] RawData = null;
        public bool Archived = false;
        public string FriendlyName = null;
        public bool HasPrivateKey = false;
        public string Issuer = null;
        public string IssuerName = null;
        public byte[] IssuerRawData = null;
        public string OidValue = null;
        public string OidFriendlyName = null;

        public void GetCertificateFile(string FilePath)
        {
            if (IsPeHeader(FilePath))
            {
                X509Certificate Certificate = X509Certificate.CreateFromSignedFile(FilePath);
                X509Certificate2 Certificate2 = new X509Certificate2(Certificate);
                IsCertificate = IsCheckSignedFile(FilePath);
                Thumbprint = Certificate2.Thumbprint;
                Subject = Certificate2.Subject;
                RawData = Certificate2.RawData;
                Archived = Certificate2.Archived;
                FriendlyName = Certificate2.FriendlyName;
                HasPrivateKey = Certificate2.HasPrivateKey;
                Issuer = Certificate2.Issuer;
                IssuerName = Certificate2.IssuerName.Name;
                IssuerRawData = Certificate2.IssuerName.RawData;
                OidValue = Certificate2.IssuerName.Oid.Value;
                OidFriendlyName = Certificate2.IssuerName.Oid.FriendlyName;
                CertificateName = Certificate2.GetNameInfo(X509NameType.DnsFromAlternativeName, false);
            }
        }

        private bool IsCheckSignedFile(string fileName)
        {
            bool IsSigned = false;

            try
            {
                X509Certificate Certificate = X509Certificate.CreateFromSignedFile(fileName);
                X509Certificate2 Certificate2 = new X509Certificate2(Certificate);
                IsSigned = true;
            }
            catch (Exception)
            {
                IsSigned = false;
            }
            return IsSigned;
        }

        public bool IsPeHeader(string fileName)
        {
            byte[] buffer = null;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            //buffer = br.ReadBytes((int)numBytes);
            buffer = br.ReadBytes(5);
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
