using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module
{
    class Script
    {
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "Script Scan Engine";

        public List<string> Signature = new List<string>();
        public List<string> VirNames = new List<string>();

        KavSignature cSignFile = new KavSignature();

        public void LoadVDB()
        {
            cSignFile.LoadVDB(Signature, KavConfig.sJavaScriptDB);

            KavConfig.VirusNumber += Signature.Count;

            Init();
        }

        public void Init()
        {
            VirNames.Add("Malware-JS/Nemucod.A");
            VirNames.Add("Malware-JS/Nemucod.B");
            VirNames.Add("Malware-JS/Nemucod.C");
            VirNames.Add("Malware-JS/Suspect");
            VirNames.Add("Malware-JS/Agent.A");
            VirNames.Add("Malware-JS/Agent.B");
            VirNames.Add("Malware-JS/Agent.C");
            VirNames.Add("Malware-JS/Agent.D");
            VirNames.Add("Malware-JS/Agent.E");
            VirNames.Add("Malware-JS/Agent.F");
            VirNames.Add("Malware-JS/Agent.FE");
            VirNames.Add("AdWare-JS/Lnkr");
            VirNames.Add("Malware-JS.Downloader.A");
            VirNames.Add("Malware-JS.Downloader.B");
            VirNames.Add("Malware-VBS/Generic.A");
            VirNames.Add("Malware-VBS/Generic.B");
            VirNames.Add("Malware-VBS/Outlook");
            VirNames.Add("Malware-JS/Obfuscate.1");
            VirNames.Add("Malware-JS/Obfuscate.2");
            VirNames.Add("Malware-JS/Obfuscate.3");
            VirNames.Add("Malware-JS/Obfuscate.4");
            VirNames.Add("Malware-JS/Locky");
            VirNames.Add("Malware-JS/Shell");
            VirNames.Add("Malware-HTML/Generic");
            VirNames.Add("Malware-JS/Generic");
            VirNames.Add("Malware-JS/GandCrab");
            VirNames.Add("Exploit-RTF/CVE-2017-0199");
            VirNames.Add("Malware-PS/Injection");
            VirNames.Add("Malware-PS/Agent");
        }

        public KavKernel.ScanResult ScanFile(byte[] buffer, string extensions, string filepath)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            if (extensions.Contains(".js") || extensions.Contains(".vbs") || extensions.Contains(".rtf") || extensions.Contains(".ps1") || extensions.Contains(".bat"))
            {
         
                var enc = new ASCIIEncoding();
                var header = enc.GetString(buffer);

                MatchCollection _MatchCollectionObf = Regex.Matches(header, "u00");
                MatchCollection _MatchCollectionNull = Regex.Matches(header, "null");
                MatchCollection _MatchCollectionFunction = Regex.Matches(header, "function");
                MatchCollection _MatchCollectionX = Regex.Matches(header, @"x");
                MatchCollection _MatchCollectionPar = Regex.Matches(header, @"par");
                MatchCollection _MatchCollectionZero = Regex.Matches(header, @"0");
                MatchCollection _MatchCollectionWave = Regex.Matches(header, "~");
                MatchCollection _MatchCollectionPush = Regex.Matches(header, "push");
                MatchCollection _MatchCollectonOffset = Regex.Matches(header, "0x");
                MatchCollection _MatchCollectionMath = Regex.Matches(header, "Math");
                MatchCollection _MatchCollectionPercent = Regex.Matches(header, "%");
                MatchCollection _MatchCollectionChr = Regex.Matches(header, "Chr");
                MatchCollection _MatchCollectionEcho = Regex.Matches(header, "echo");
                MatchCollection _MatchCollectionCase1 = Regex.Matches(header, "([0-9], [a-z])");
                MatchCollection _MatchCollectionHex = Regex.Matches(header, "[0-9a-f]+");

                int h_zero = _MatchCollectionZero.Count;
                int h_p = _MatchCollectionPar.Count;
                int h_x = _MatchCollectionX.Count;
                int h_function = _MatchCollectionFunction.Count;
                int h_null = _MatchCollectionNull.Count;
                int h_Obfus = _MatchCollectionObf.Count;
                int h_wave = _MatchCollectionWave.Count;
                int h_push = _MatchCollectionPush.Count;
                int h_offset = _MatchCollectonOffset.Count;
                int h_math = _MatchCollectionMath.Count;
                int h_percent = _MatchCollectionPercent.Count;
                int h_char = _MatchCollectionChr.Count;
                int h_echo = _MatchCollectionEcho.Count;
                int h_case1 = _MatchCollectionCase1.Count;
                int h_hex = _MatchCollectionHex.Count;

                int entropy = (int)GetEntropyValue(buffer);


                if (h_Obfus > 100)
                {
                    if (h_null > 12)
                    {
                        VirusName = "Malware-JS/Nemucod." + h_Obfus;
                        FileLocation = filepath;
                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

                if (entropy > 6)
                {
                    VirusName = "Malware-JS/Suspect." + entropy;
                    FileLocation = filepath;
                    mScanResult = KavKernel.ScanResult.Infected;
                }

                if (h_char > 20)
                {
                    if (header.Contains("Scripting.FileSystemObject"))
                    {
                        if (header.Contains("http:"))
                        {
                            VirusName = "Malware-JS/Agent." + h_char;
                            FileLocation = filepath;
                            mScanResult = KavKernel.ScanResult.Infected;
                        }
                    }
                }

                if (header.Contains("document.getElementById"))
                {
                    if (header.Contains("eval"))
                    {
                        if (header.Contains("replace"))
                        {
                            if (header.Contains("write"))
                            {
                                if (header.Contains("substr"))
                                {
                                    if (header.Contains("charCodeAt"))
                                    {
                                        if (header.Contains("fromCharCode"))
                                        {
                                            VirusName = "AdWare-JS/Lnkr";
                                            FileLocation = filepath;
                                            mScanResult = KavKernel.ScanResult.Infected;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (h_case1 > 70)
                {
                    VirusName = "Malware-JS.Downloader." + h_case1;
                    FileLocation = filepath;
                    mScanResult = KavKernel.ScanResult.Infected;
                }

                if (header.Contains("fso.OpenTextFile"))
                {
                    if (header.Contains("WScript.CreateObject"))
                    {
                        if (header.Contains("WScript.Shell"))
                        {
                            if (header.Contains("RegWrite"))
                            {
                                VirusName = "Malware-VBS/Generic";
                                FileLocation = filepath;
                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                        }
                    }
                }

                if (Regex.IsMatch(header, @"/([A-Za-z0-9+\/]{4}){3,}([A-Za-z0-9+\/]{2}==|[A-Za-z0-9+\/]{3}=)?/"))
                {
                    if (h_hex > 0)
                    {
                        VirusName = "Malware-JS/Obfuscate.1";
                        FileLocation = filepath;
                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

                if (h_function > 20)
                {
                    if (h_hex > 0)
                    {
                        VirusName = "Malware-JS/Obfuscate.2";
                        FileLocation = filepath;
                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

                if (h_echo > 20)
                {
                    VirusName = "Malware-JS/Nemucod." + h_echo;
                    FileLocation = filepath;
                    mScanResult = KavKernel.ScanResult.Infected;
                }

                if (header.Contains("Scripting.FilesystemObject"))
                {
                    if (header.Contains("outlook.application"))
                    {
                        VirusName = "Malware-VBS/Outlook";
                        FileLocation = filepath;

                        //cQuarantineFile.AddQuarantineFile(FilePath, VirusName);

                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

                if (h_Obfus > 100)
                {
                    if (h_null == 0)
                    {
                        if (header.Contains("pre_PayPal") && header.Contains("pre_originally"))
                        {
                            VirusName = "Malware-JS/Locky." + h_Obfus;
                            FileLocation = filepath;

                            //cQuarantineFile.AddQuarantineFile(FilePath, VirusName);

                            mScanResult = KavKernel.ScanResult.Infected;
                        }
                    }
                }

                if (header.Contains("ActiveXObject"))
                {
                    if (header.Contains("WScript.Shell"))
                    {
                        VirusName = "Malware-JS/Shell";
                        FileLocation = filepath;
                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

                if (header.Contains("<HTML>"))
                {
                    if (header.Contains("OpenTextFile"))
                    {
                        VirusName = "Malware-HTML/Generic";
                        FileLocation = filepath;

                        //cQuarantineFile.AddQuarantineFile(FilePath, VirusName);

                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

                if (h_function > 100)
                {
                    if (header.Contains("5557545E"))
                    {
                        VirusName = "Malware-JS/Nemucod." + h_function;
                        FileLocation = filepath;

                        //cQuarantineFile.AddQuarantineFile(FilePath, VirusName);

                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

                if (header.Contains("ActiveXObject"))
                {
                    if (header.Contains("Scripting.FileSystemObject"))
                    {
                        VirusName = "Malware-JS/Generic";
                        FileLocation = filepath;
                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

                if (header.Contains("ActiveXObject"))
                {
                    if (header.Contains("ADODB.Stream"))
                    {
                        VirusName = "Malware-JS/Agent";
                        FileLocation = filepath;
                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

                if (h_wave > 100)
                {
                    if (header.Contains("~Co~~mm~a~nd"))
                    {
                        VirusName = "Malware-JS/GandCrab." + h_wave;
                        FileLocation = filepath;

                        //cQuarantineFile.AddQuarantineFile(FilePath, VirusName);

                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }



                if (header.Contains("function"))
                {
                    if (h_push > 100)
                    {
                        VirusName = "Malware-JS/Downloader." + h_push;
                        FileLocation = filepath;

                        //cQuarantineFile.AddQuarantineFile(FilePath, VirusName);

                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

                if (h_percent > 20)
                {
                    if (h_wave > 100)
                    {
                        if (header.Contains("http"))
                        {
                            if (header.Contains("WScrip"))
                            {
                                if (header.Contains("unescape"))
                                {
                                    VirusName = "Malware-JS/Agent." + h_wave;
                                    FileLocation = filepath;

                                    //cQuarantineFile.AddQuarantineFile(FilePath, VirusName);

                                    mScanResult = KavKernel.ScanResult.Infected;
                                }
                            }
                        }
                    }
                }

                if (header.Contains("http://"))
                {
                    if (header.Contains("ADODB.Stream"))
                    {
                        if (header.Contains("WScript"))
                        {
                            VirusName = "Malware-JS/Agent.1";
                            FileLocation = filepath;

                            //cQuarantineFile.AddQuarantineFile(FilePath, VirusName);

                            mScanResult = KavKernel.ScanResult.Infected;
                        }
                    }
                }

                if (h_x > 100)
                {
                    if (header.Contains("eval"))
                    {
                        if (header.Contains("var"))
                        {
                            if (header.Contains("PTTHLMX.2LMXSM"))
                            {
                                VirusName = "Malware-JS/Agent.2";
                                FileLocation = filepath;

                                //cQuarantineFile.AddQuarantineFile(FilePath, VirusName);

                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                        }
                    }
                }

                if (header.Contains("ActiveXObject"))
                {
                    if (header.Contains("www."))
                    {
                        if (header.Contains(".com"))
                        {
                            VirusName = "Malware-JS/Agent.3";
                            FileLocation = filepath;

                            //cQuarantineFile.AddQuarantineFile(FilePath, VirusName);

                            mScanResult = KavKernel.ScanResult.Infected;
                        }
                    }
                }

                

                if (header.Contains("ActiveXObject"))
                {
                    if (header.Contains("Sleep"))
                    {
                        if (header.Contains("var"))
                        {
                            if (header.Contains("if"))
                            {
                                if (header.Contains("ResponseBody"))
                                {
                                    if (header.Contains("Math"))
                                    {
                                        VirusName = "Malware-JS/Agent.4";
                                        FileLocation = filepath;
                                        mScanResult = KavKernel.ScanResult.Infected;
                                    }
                                }
                            }
                        }
                    }
                }

                if (h_offset > 100)
                {
                    if (h_x > 100)
                    {
                        if (h_math > 10)
                        {
                            if (header.Contains("function"))
                            {
                                VirusName = "Malware-JS/Obfuscate." + h_offset;
                                FileLocation = filepath;
                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                        }
                    }
                }

                if (header.Contains(@"{\rtf1")) // Rtf 파일 인가?
                {
                    if (header.Contains("objautlink"))
                    {
                        if (h_p > 30)
                        {
                            if (h_zero > 200)
                            {
                                VirusName = "Exploit-RTF/CVE-2017-0199";
                                FileLocation = filepath;
                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                        }
                    }
                }

                if (header.Contains("powershell  -nop -w hidden -ep bypass"))
                {
                    if (header.Contains("Invoke-ReflectivePEInjection"))
                    {
                        VirusName = "Malware-PS/Injection";
                        FileLocation = filepath;
                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

                if (header.Contains("mshta vbscript:createobject"))
                {
                    if (header.Contains("wscript.shell"))
                    {
                        VirusName = "Malware-JS/Generic";
                        FileLocation = filepath;
                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }

                if (header.Contains("New-Object system.Net.WebClient;"))
                {
                    if (header.Contains("downloadString"))
                    {
                        if (header.Contains("application/x-www-form-urlencoded"))
                        {
                            if (header.Contains("ToBase64String"))
                            {
                                if (header.Contains("Get-WmiObject"))
                                {
                                    VirusName = "Malware-PS/Agent";
                                    FileLocation = filepath;

                                    //cQuarantineFile.AddQuarantineFile(FilePath, VirusName);

                                    mScanResult = KavKernel.ScanResult.Infected;
                                }
                            }
                        }
                    }
                }

                if (Regex.IsMatch(header, @"/([A-Za-z0-9+\/]{4}){3,}([A-Za-z0-9+\/]{2}==|[A-Za-z0-9+\/]{3}=)?/"))
                {
                    VirusName = "Malware/Obfuscate.3";
                    FileLocation = filepath;
                    mScanResult = KavKernel.ScanResult.Infected;
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
    }
}
