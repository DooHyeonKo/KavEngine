using Features;
//using KavEngine.NeuralNetwork.Classifier;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class PE
    {
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "PE Scan Engine";
        public bool UseHeur = false;
        public List<string> VirusList = new List<string>();
        KavSignature cSignFile = new KavSignature();

        public void LoadVDB()
        {
            cSignFile.LoadVDB(VirusList, KavConfig.sPeVdbPath);

            KavConfig.VirusNumber += VirusList.Count;


        }

        public KavKernel.ScanResult ScanFile(byte[] buffer, string FilePath)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            if (IsPeHeader(buffer))
            {
                byte[] buff = buffer;
                string sHash = "";
                string sPeHash = "";
                PeFile pe = new PeFile(buff);
                int SectionLengths = pe.ImageSectionHeaders.Length - 1;
                float __entropy = GetEntropyValue(pe.Buff);
                for (int i = 0; i < SectionLengths; i++)
                {
                    uint PointerToRawData = pe.ImageSectionHeaders[i].PointerToRawData;
                    uint SizeOfRawData = pe.ImageSectionHeaders[i].SizeOfRawData;
                    uint VirtualAddress = pe.ImageSectionHeaders[i].VirtualAddress;
                    uint VirtualSize = pe.ImageSectionHeaders[i].VirtualSize;

                    byte[] SectionBuffer = buff.Slice((int)PointerToRawData, (int)PointerToRawData + (int)SizeOfRawData);

                    sHash = SizeOfRawData.ToString() + KavHash.CalculateMd5Buffer(SectionBuffer);

                    if (sHash != "d41d8cd98f00b204e9800998ecf8427e")
                    {
                        foreach (string MainPeList in VirusList)
                        {
                            if (MainPeList.Contains(sHash))
                            {
                                string[] strData = MainPeList.Split(new string[]
                                {
                                     sHash
                                }, StringSplitOptions.None);

                                for (int j = 0; j < strData.Length; j++)
                                {
                                    string strVirusName = strData[j].Trim();

                                     VirusName = $"{strVirusName}.Generic.{sHash.Substring(0, 10).ToString().ToUpper()}";


                                    FileLocation = FilePath;
                                }

                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                        }
                    }

                    //FileClassifier.ModelInput Input = new FileClassifier.ModelInput()
                    //{
                    //    Hash = pe.MD5,
                    //    Size_of_data = SizeOfRawData,
                    //    Virtual_address = VirtualAddress,
                    //    Virtual_size = VirtualSize,
                    //    Entropy = __entropy
                    //};

                    //var Data = FileClassifier.Predict(Input);

                    //if (Data.Score[1] > 0.8)
                    //{
                    //    VirusName = $"Malicious (Score = {Data.PredictedLabel}%)";
                    //    FileLocation = FilePath;
                    //    mScanResult = KavKernel.ScanResult.Infected;
                    //}

                    int entropy = (int)GetEntropyValue(SectionBuffer);
                    int _entropy = (int)GetEntropyValue(pe.Buff);

                    if (entropy > 6)
                    {
                        if (UseHeur)
                        {
                            if (VirtualAddress == 4096)
                            {
                                VirusName = "Packed Malware";
                                FileLocation = FilePath;
                                mScanResult = KavKernel.ScanResult.Infected;
                            } 
                        }
                    }
                }
            }

            return mScanResult;
        }

        private float GetEntropyValue(byte[] c)
        {
            int[] numArray = new int[0x100];
            byte[] buffer = c;
            for (int i = 0; i < 0x100; i++)
            {
                numArray[i] = 0;
            }

            for (int j = 0; j < (buffer.Length - 1); j++)
            {
                int index = buffer[j];
                numArray[index]++;
            }
            int length = buffer.Length;
            float entropy = 0f;
            for (int k = 0; k < 0x100; k++)
            {
                if ((numArray[k] != 0) && (k != 0))
                {
                    entropy += (-float.Parse(numArray[k].ToString()) / float.Parse(length.ToString())) * float.Parse(Math.Log((double)(float.Parse(numArray[k].ToString()) / float.Parse(length.ToString())), 2.0).ToString());
                }
            }

            return entropy;
        }

        public bool IsPeHeader(byte[] buffer)
        {
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
