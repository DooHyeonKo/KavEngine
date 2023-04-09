using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class HTML
    {
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "HTML Scan Engine";

        private string sHtmlPattern = @"<\s*html\b|\bdoctype\b|<\s*head\b|<\s*title\b|<\s*meta\b|\bhref\b|<\s*link\b|<\s*body\b|<\s*script\b|<\s*iframe\b";
        private string IframePattern = @"\?ob_start.+?>\s*<iframe";

        public KavKernel.ScanResult ScanFile(byte[] buffer,string extension, string FilePath)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            if (extension.Contains("html") || extension.Contains("htm"))
            {
                var enc = new ASCIIEncoding();
                var header = enc.GetString(buffer);
                MatchCollection _MatchCollectionRegWrite = Regex.Matches(header, "reg.RegWrite");


                int RegWrite = _MatchCollectionRegWrite.Count;

                if (Regex.IsMatch(header, sHtmlPattern))
                {
                    if (Regex.IsMatch(header, IframePattern))
                    {
                        VirusName = "Malware-HTML/IFrame";
                        FileLocation = FilePath;
                        mScanResult = KavKernel.ScanResult.Infected;
                    }

                }

                if (header.Contains("eval("))
                {
                    if (header.Contains("unescape("))
                    {
                        VirusName = "Malware-HTML/Obfuscated";
                        FileLocation = FilePath;
                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

                if (RegWrite > 3)
                {
                    if (header.Contains("Scripting.FileSystemObject"))
                    {
                        VirusName = "Malware-HTML/Generic";
                        FileLocation = FilePath;
                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

                if (header.Contains("<script language="))
                {
                    if (header.Contains("VBScript"))
                    {
                        if (header.Contains("wso.RegWrite"))
                        {
                            if (header.Contains("Set wso = CreateObject"))
                            {
                                if (header.Contains("ChrW"))
                                {
                                    if (header.Contains("Window.ReSizeTo 0"))
                                    {
                                        if (header.Contains("-2000,-2000 : Set Office = CreateObject"))
                                        {
                                            VirusName = "Malware-HTML/Agent";
                                            FileLocation = FilePath;
                                            mScanResult = KavKernel.ScanResult.Infected;
                                        }

                                    }
                                }
                            }
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
    }
}
