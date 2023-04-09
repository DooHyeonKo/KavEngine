using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class Adware
    {
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "Adware Scan Engine";
        public List<string> AdwareList = new List<string>();

        KavSignature cSignFile = new KavSignature();

        public void LoadVDB()
        {
            cSignFile.LoadVDB(AdwareList, KavConfig.sAdwareVdbPath);
            KavConfig.VirusNumber += AdwareList.Count;

            for (int i =0;i < AdwareList.Count; i++)
            {
                KavConfig.VirusNameList.Add(AdwareList[i]);
            }

            
        }

        public KavKernel.ScanResult ScanFile( string filepath)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            if (IsCheckSignedFile(filepath))
            {
                X509Certificate certificate = X509Certificate.CreateFromSignedFile(filepath);
                X509Certificate2 Certificate2 = new X509Certificate2(certificate);

                string StrNameInfo = Certificate2.GetNameInfo(X509NameType.SimpleName, false);

                string sHash = KavHash.CalculateMd5Buffer(StrNameInfo).ToUpper();

                foreach (string MainAdwareList in AdwareList)
                {
                    if (MainAdwareList.Contains(sHash))
                    {
                        string[] strData = MainAdwareList.Split(new string[]
                        {
                                           sHash
                        }, StringSplitOptions.None);

                        for (int j = 0; j < strData.Length; j++)
                        {
                            string strVirusName = strData[j].Trim();

                            if (strVirusName.Equals(",") == false)
                            {
                                VirusName = strVirusName;
                                FileLocation = filepath;

                                //cQuarantineFile.AddQuarantineFile(FilePath, strVirusName);
                            }
                        }

                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

            }

            return mScanResult;
        }

        private bool IsCheckSignedFile(string FilePath)
        {
            bool IsSigned = false;

            try
            {
                X509Certificate certificate = X509Certificate.CreateFromSignedFile(FilePath);
                X509Certificate2 Certificate2 = new X509Certificate2(certificate);
                IsSigned = true;
            }
            catch (Exception)
            {
                IsSigned = false;
            }
            return IsSigned;
        }
    }
}
