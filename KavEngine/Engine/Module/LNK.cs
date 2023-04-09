
using Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class LNK
    {
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "LNK Scan Engine";

        public KavKernel.ScanResult ScanFile(byte[] buffer, string FilePath, string extension)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            if (buffer[0] != 0x4d && buffer[1] != 0x5a)
            {
                // Scan LNK File

                if (extension.Contains("lnk"))
                {
                    bool IsLnkFile = false;

                    var enc = new ASCIIEncoding();
                    var header = enc.GetString(buffer).Replace(" ", ""); // 빈칸 없애기;

                    if (buffer[0] == 0x4C && buffer[1] == 0x00 && buffer[2] == 0x00 && buffer[3] == 0x00 && buffer[4] == 0x01 && buffer[5] == 0x14 && buffer[6] == 0x02 && buffer[7] == 0x00)
                    {
                        IsLnkFile = true;
                    }

                    if (IsLnkFile)
                    {
                        MatchCollection _MatchCollectionHTTP = Regex.Matches(header, "http");

                        if (header.Contains("cmd.exe") && header.Contains("powershell"))
                        {
                            if (header.Contains("http"))
                            {
                                VirusName = "LNK/Agent";
                                FileLocation = FilePath;

                                //cQuarantineFile.AddQuarantineFile(FilePath, "LNK/Agent");

                                mScanResult = KavKernel.ScanResult.Infected;
                            }


                        }

                        if (header.Contains("cmd.exe"))
                        {
                            if (header.Contains("mshta.exe"))
                            {
                                VirusName = "LNK/Agent";
                                FileLocation = FilePath;

                                //cQuarantineFile.AddQuarantineFile(FilePath, "LNK/Agent");

                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                        }

                        if (header.Contains(".DownloadString"))
                        {
                            if (header.Contains("powershell.exe"))
                            {
                                if (header.Contains("mshta.exe"))
                                {
                                    VirusName = "LNK/Agent";
                                    FileLocation = FilePath;

                                    //cQuarantineFile.AddQuarantineFile(FilePath, "LNK/Agent");

                                    mScanResult = KavKernel.ScanResult.Infected;
                                }
                            }
                        }

                        if (header.Contains("WScript.shell"))
                        {
                            if (header.Contains("cmd.exe") && header.Contains("powershell.exe"))
                            {
                                VirusName = "LNK/Generic";
                                FileLocation = FilePath;

                                //cQuarantineFile.AddQuarantineFile(FilePath, "LNK/Generic");

                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                        }

                        if (header.Contains("del/f/q"))
                        {
                            if (header.Contains("cmd.exe") && header.Contains("powershell.exe"))
                            {
                                VirusName = "LNK/Generic";
                                FileLocation = FilePath;

                                //cQuarantineFile.AddQuarantineFile(FilePath, "LNK/Generic");

                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                        }

                        if (header.Contains("-ExecutionPolicy"))
                        {
                            if (header.Contains("ByPass"))
                            {
                                VirusName = "LNK/Generic";
                                FileLocation = FilePath;

                                //cQuarantineFile.AddQuarantineFile(FilePath, "LNK/Generic");

                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                        }

                        int h_count = _MatchCollectionHTTP.Count;

                        if (h_count > 0)
                        {
                            if (header.Contains("DownloadFile") && header.Contains("DownloadString"))
                            {
                                VirusName = "LNK/Agent";
                                FileLocation = FilePath;

                                //cQuarantineFile.AddQuarantineFile(FilePath, "LNK/Agent");

                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                        }
                    }
                }
                else
                {
                    mScanResult = KavKernel.ScanResult.NotFoundPath;
                }
            }

            return mScanResult;
        }
    }
}
