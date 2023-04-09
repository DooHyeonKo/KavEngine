using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace KavEngine.Engine
{
   public class KavQuarantine
    {
        public List<string> ListQuarantineFile = new List<string>();

        public void LoadQuarantineFile()
        {
            string[] sExclude = File.ReadAllLines(KavConfig.sQuarantineFilePath);

            foreach (string sFile in sExclude)
            {
                ListQuarantineFile.Add(sFile);
            }
        }

        public void AddQuarantineFile(string sFilePath, string sVirusName)
        {
            ListQuarantineFile.Add(string.Format("{0},{1}", sFilePath, sVirusName));
        }

        public void SaveQuarantineSettings()
        {
            // Save Settings

            try
            {
                StreamWriter SW = new StreamWriter(KavConfig.sQuarantineFilePath);

                int nCount = ListQuarantineFile.Count;

                for (int i =0;i <nCount;i++)
                {
                    ListQuarantineFile[i] += "\r\n";
                    SW.Write(ListQuarantineFile[i]);
                }

                SW.Close();
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[AddQuarantineFile] " + ex.Message);
            }
        }

        public void RemoveQuarantineFile(string sFilePath, string sVirusName)
        {
            ListQuarantineFile.Remove(string.Format("{0},{1}", sFilePath, sVirusName));

            // Save Settings

            try
            {
                foreach (string sFile in ListQuarantineFile)
                {
                    StreamWriter sWriter = new StreamWriter(KavConfig.sQuarantineFilePath);
                    sWriter.WriteLine(sFile);
                    sWriter.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[RemoveQuarantineFile] " + ex.Message);
            }
        }

        public void MoveQuarantineFile(string sFilePath, string sVirusName)
        {
            AddQuarantineFile(sFilePath, sVirusName);

            try
            {
                string sFileName = Path.GetFileName(sFilePath);
                string sQuarantinePath = AppDomain.CurrentDomain + "\\Quarantine";

                string sPath = sQuarantinePath + "\\" + sFileName;

                File.Move(sFileName, sPath);

                File.Delete(sFilePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[MoveQuarantineFile] " + ex.Message);
            }
        }
    }
}
