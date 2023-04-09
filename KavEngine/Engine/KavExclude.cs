using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace KavEngine.Engine
{
    public class KavExclude
    {
        public List<string> ListExcludeFile = new List<string>();

        public void LoadExcludeFile()
        {
            string[] sExclude = File.ReadAllLines(KavConfig.sExcludeFilePath);

            foreach (string sFile in sExclude)
            {
                ListExcludeFile.Add(sFile);
            }
        }

        public void AddExcludeFile(string sFilePath)
        {
            ListExcludeFile.Add(sFilePath);

            // Save Settings

            try
            {
                foreach (string sFile in ListExcludeFile)
                {
                    StreamWriter sWriter = new StreamWriter(KavConfig.sExcludeFilePath);
                    sWriter.WriteLine(sFile);
                    sWriter.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[AddExcludeFile] " + ex.Message);
            }
        }

        public void RemoveExcludeFile(string sFilePath)
        {
            ListExcludeFile.Remove(sFilePath);

            // Save Settings

            try
            {
                foreach (string sFile in ListExcludeFile)
                {
                    StreamWriter sWriter = new StreamWriter(KavConfig.sExcludeFilePath);
                    sWriter.WriteLine(sFile);
                    sWriter.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[RemoveExcludeFile] " + ex.Message);
            }
        }
    }
}
