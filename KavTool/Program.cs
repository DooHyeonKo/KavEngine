using Features;
using KavTool.Sig;
using MalScore.Disassembler;
using SSDEEP.NET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KavTool
{
    class Program
    {
        private static bool IsLinked(string folder)
        {
            if (folder.Contains("Documents and Settings") || folder.Contains("Application Data")) return false;
            return true;
        }

        private static IEnumerable<string> GetFiles(string folder, string filter, bool recursive)
        {
            string[] found = null;

            if (IsLinked(folder))
                try
                {
                    found = Directory.GetFiles(folder, filter);
                }
                catch
                {

                }

            if (found != null)
                foreach (var x in found)
                    if (File.Exists(x))
                        yield return x;

            if (!recursive) yield break;
            {
                found = null;

                if (IsLinked(folder))
                    try
                    {
                        found = Directory.GetDirectories(folder);
                    }
                    catch
                    {

                    }

                if (found == null) yield break;

                foreach (var x in found)
                    foreach (var y in GetFiles(x, filter, true))
                        if (File.Exists(y))
                            yield return y;
            }
        }

         public static double ShannonEntropy(string s)
        {
            var map = new Dictionary<char, int>();
            foreach (char c in s)
            {
                if (!map.ContainsKey(c))
                    map.Add(c, 1);
                else
                    map[c] += 1;
            }

            double result = 0.0;
            int len = s.Length;
            foreach (var item in map)
            {
                var frequency = (double)item.Value / len;
                result -= frequency * (Math.Log(frequency) / Math.Log(2));
            }

            return result;
        }

        static void Main(string[] args)
        {
            if (args[0] == null)
                Console.WriteLine("Usage: sigtool.exe [PATH]");


            if (Directory.Exists(args[0].Replace("_", " ")))
            {
                var sw = new Stopwatch();
                sw.Start();

                Parallel.ForEach(GetFiles(args[0].Replace("_", " "), "*.*", true), file =>
                {
                    try
                    {
                        var FileInfo = new FileInfo(file);
                        var FilePath = FileInfo.FullName;


                        WriteToTextLog(GetImpHash(FilePath).ToUpper());
                        Console.WriteLine(GetImpHash(FilePath).ToUpper());

                        //else if (FilePath.Contains("Backdoor."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Backdoor"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Backdoor"));
                        //}
                        //else if (FilePath.Contains("Worm."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Worm"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Worm"));
                        //}
                        //else if (FilePath.Contains("AdWare."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "AdWare"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "AdWare"));
                        //}
                        //else if (FilePath.Contains("UDS."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Malware"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Malware"));
                        //}
                        //else if (FilePath.Contains("Hoax."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Hoax"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Hoax"));
                        //}
                        //else if (FilePath.Contains("HackTool."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "HackTool"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "HackTool"));
                        //}
                        //else if (FilePath.Contains("Exploit."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Exploit"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Exploit"));
                        //}
                        //else if (FilePath.Contains("Trojan-Downloader."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Trojan-Downloader"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Trojan-Downloader"));
                        //}
                        //else if (FilePath.Contains("Trojan-Banker."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Trojan-Banker"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Trojan-Banker"));
                        //}
                        //else if (FilePath.Contains("Trojan-DDoS."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Trojan-DDoS"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Trojan-DDoS"));
                        //}
                        //else if (FilePath.Contains("Trojan-Dropper."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Trojan-Dropper"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Trojan-Dropper"));
                        //}
                        //else if (FilePath.Contains("Trojan-Ransom."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Trojan-Ransom"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Trojan-Ransom"));
                        //}
                        //else if (FilePath.Contains("Trojan-PSW."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Trojan-PSW"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Trojan-PSW"));
                        //}
                        //else if (FilePath.Contains("Net-Worm."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Net-Worm"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Net-Worm"));
                        //}
                        //else if (FilePath.Contains("Dialer."))
                        //{
                        //    WriteToTextLog(
                        //    (FilePath, "Dialer"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Dialer"));
                        //}
                        //else if (FilePath.Contains(".Downloader."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Downloader"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Downloader"));
                        //}
                        //else if (FilePath.Contains("RiskTool."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "RiskTool"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "RiskTool"));
                        //}
                        //else if (FilePath.Contains("WebToolbar."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "WebToolbar"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "WebToolbar"));
                        //}
                        //else if (FilePath.Contains("P2P-Worm."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "P2P-Worm"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "P2P-Worm"));
                        //}
                        //else if (FilePath.Contains("Packed."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Packed"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Packed"));
                        //}
                        //else if (FilePath.Contains("Rootkit."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Rootkit"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Rootkit"));
                        //}
                        //else if (FilePath.Contains("SMS-Flooder."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "SMS-Flooder"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "SMS-Flooder"));
                        //}
                        //else if (FilePath.Contains("SpamTool."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "SpamTool"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "SpamTool"));
                        //}
                        //else if (FilePath.Contains("Trojan-FakeAV."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Trojan-FakeAV"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Trojan-FakeAV"));
                        //}
                        //else if (FilePath.Contains("Trojan-GameThief."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Trojan-GameThief"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Trojan-GameThief"));
                        //}
                        //else if (FilePath.Contains("Trojan-Spy."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Trojan-Spy"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Trojan-Spy"));
                        //}
                        //else if (FilePath.Contains("Email-Worm."))
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Email-Worm"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Email-Worm"));
                        //}
                        //else
                        //{
                        //    WriteToTextLog(GetPEDataSet(FilePath, "Malware"));
                        //    Console.WriteLine(GetPEDataSet(FilePath, "Malware"));
                        //}

                        //WriteToTextLog(GetTextSectionMD5(FilePath) + Path.GetFileNameWithoutExtension(FilePath));
                        //Console.WriteLine(GetTextSectionMD5(FilePath) + Path.GetFileNameWithoutExtension(FilePath));
                    }
                    catch { }
                });
            }

        }

        static void Start(string[] args)
        {
            
        }

        private static string GetTextSectionMD5(string filename)
        {
            string sHash = "";
            var virusName = "";
            Features.PeFile pe = new Features.PeFile(filename);
            int SectionLengths = pe.ImageSectionHeaders.Length;

            for (int i = 0; i < SectionLengths - 1; i++)
            {
                uint PointerToRawData = pe.ImageSectionHeaders[i].PointerToRawData;
                uint SizeOfRawData = pe.ImageSectionHeaders[i].SizeOfRawData;
                uint VirtualAddress = pe.ImageSectionHeaders[i].VirtualAddress;
                uint VirtualSize = pe.ImageSectionHeaders[i].VirtualSize;

                byte[] SectionBuffer = pe.Buff.Slice((int)PointerToRawData, (int)PointerToRawData + (int)SizeOfRawData);

                if (!CalculateMd5Buffer(SectionBuffer).Contains("d41d8cd98f00b204e9800998ecf8427e"))
                {
                    sHash = SizeOfRawData.ToString() + CalculateMd5Buffer(SectionBuffer);
                }

                if (SizeOfRawData == 0)
                {

                }

                var newPath = Path.GetFileNameWithoutExtension(filename).Replace("HEUR.", "");
                newPath = newPath.Replace("not-a-virus.", "");
              
                try
                {
                    virusName = sHash + $"{newPath.Split('.')[0]}.{newPath.Split('.')[1]}.{newPath.Split('.')[2]}";
                }
                catch (Exception ex)
                {
                    virusName = sHash + $"{newPath.Split('.')[0]}.{newPath.Split('.')[1]}";
                }
            }

            //if (virusName.Length < 27)
            //{
            //    var newPath = Path.GetFileNameWithoutExtension(filename).Replace("HEUR.", "");
            //    newPath = newPath.Replace("not-a-virus.", "").ToLower();

            //    var newHash = new FileInfo(filename).Length.ToString() + CalculateMD5(filename);

            //    try
            //    {
            //        virusName = newHash + $"{newPath.Split('.')[0]}.{newPath.Split('.')[1]}.{newPath.Split('.')[2]}";
            //    }
            //    catch (Exception ex)
            //    {
            //        virusName = newHash + $"{newPath.Split('.')[0]}.{newPath.Split('.')[1]}";
            //    }
            //}


            return virusName;
        }

        public static string GetFormatString(int formatNumber)
        {
            string formatString = "";

            for (int i = 0; i < formatNumber; i++)
            {
                formatString += $"{i},";
            }

            return formatString;
        }

        public static string GetPEDataSet(string FilePath, int Legitimate)
        {
            Features.PeFile peFile = new Features.PeFile(FilePath);

    

            var md5 = peFile.MD5;
            var Machine = peFile.ImageNtHeaders.FileHeader.Machine;
            var SizeOfOptionalHeader = peFile.ImageNtHeaders.FileHeader.SizeOfOptionalHeader;
            var Characteristics = peFile.ImageNtHeaders.FileHeader.Characteristics;
            var MajorLinkerVersion = peFile.ImageNtHeaders.OptionalHeader.MajorLinkerVersion;
            var MinorLinkerVersion = peFile.ImageNtHeaders.OptionalHeader.MinorLinkerVersion;
            var SizeOfCode = peFile.ImageNtHeaders.OptionalHeader.SizeOfCode;
            var SizeOfInitializedData = peFile.ImageNtHeaders.OptionalHeader.SizeOfInitializedData;
            var SizeOfUninitializedData = peFile.ImageNtHeaders.OptionalHeader.SizeOfUninitializedData;
            var AddressOfEntryPoint = peFile.ImageNtHeaders.OptionalHeader.AddressOfEntryPoint;
            var BaseOfCode = peFile.ImageNtHeaders.OptionalHeader.BaseOfCode;
            var BaseOfData = peFile.ImageNtHeaders.OptionalHeader.BaseOfData;
            var ImageBase = peFile.ImageNtHeaders.OptionalHeader.ImageBase;
            var SectionAlignment = peFile.ImageNtHeaders.OptionalHeader.SectionAlignment;
            var FileAlignment = peFile.ImageNtHeaders.OptionalHeader.FileAlignment;
            var MajorOperatingSystemVersion = peFile.ImageNtHeaders.OptionalHeader.MajorOperatingSystemVersion;
            var MinorOperatingSystemVersion = peFile.ImageNtHeaders.OptionalHeader.MinorOperatingSystemVersion;
            var MajorImageVersion = peFile.ImageNtHeaders.OptionalHeader.MajorImageVersion;
            var MinorImageVersion = peFile.ImageNtHeaders.OptionalHeader.MinorImageVersion;
            var MajorSubsystemVersion = peFile.ImageNtHeaders.OptionalHeader.MajorSubsystemVersion;
            var MinorSubsystemVersion = peFile.ImageNtHeaders.OptionalHeader.MinorSubsystemVersion;
            var SizeOfImage = peFile.ImageNtHeaders.OptionalHeader.SizeOfImage;
            var SizeOfHeaders = peFile.ImageNtHeaders.OptionalHeader.SizeOfHeaders;
            var CheckSum = peFile.ImageNtHeaders.OptionalHeader.CheckSum;
            var Subsystem = peFile.ImageNtHeaders.OptionalHeader.Subsystem;
            var DllCharacteristics = peFile.ImageNtHeaders.OptionalHeader.DllCharacteristics;
            var SizeOfStackReserve = peFile.ImageNtHeaders.OptionalHeader.SizeOfStackReserve;
            var SizeOfStackCommit = peFile.ImageNtHeaders.OptionalHeader.SizeOfStackCommit;
            var SizeOfHeapReserve = peFile.ImageNtHeaders.OptionalHeader.SizeOfHeapReserve;
            var SizeOfHeapCommit = peFile.ImageNtHeaders.OptionalHeader.SizeOfHeapCommit;
            var LoaderFlags = peFile.ImageNtHeaders.OptionalHeader.LoaderFlags;
            var NumberOfRvaAndSizes = peFile.ImageNtHeaders.OptionalHeader.NumberOfRvaAndSizes;

            string peString = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32}", md5, Machine, SizeOfOptionalHeader, Characteristics, MajorLinkerVersion, MinorLinkerVersion, SizeOfCode, SizeOfInitializedData, SizeOfUninitializedData, AddressOfEntryPoint, BaseOfCode, BaseOfData, ImageBase, SectionAlignment, FileAlignment, MajorOperatingSystemVersion, MinorOperatingSystemVersion, MajorImageVersion, MinorImageVersion, MajorSubsystemVersion, MinorSubsystemVersion, SizeOfImage, SizeOfHeaders, CheckSum, Subsystem, DllCharacteristics, SizeOfStackReserve, SizeOfStackCommit, SizeOfHeapReserve, SizeOfHeapCommit, LoaderFlags, NumberOfRvaAndSizes, Legitimate.ToString());

            return peString;
        }

        public static string Opcode(string FilePath, int isMalware)
        {
            MalScore.Disassembler.PE pe = new MalScore.Disassembler.PE();
            pe.Analyze(FilePath);

            var opcodes = "";


            foreach (var str in pe.OpcodeList)
                opcodes += $"{str} ";

            var movs = Regex.Matches(opcodes, "MOV").Count;
            var adds = Regex.Matches(opcodes, "ADD").Count;
            var pushes = Regex.Matches(opcodes, "PUSH").Count;
            var pops = Regex.Matches(opcodes, "POP").Count;
            var xors = Regex.Matches(opcodes, "XOR").Count;
            var ors = Regex.Matches(opcodes, "OR").Count;
            var cmps = Regex.Matches(opcodes, "CMP").Count;
            var calls = Regex.Matches(opcodes, "CALL").Count;
            var jmps = Regex.Matches(opcodes, "JMP").Count;
            var decs = Regex.Matches(opcodes, "DEC").Count;
            var incs = Regex.Matches(opcodes, "INC").Count;
            var leas = Regex.Matches(opcodes, "LEA").Count;
            var rets = Regex.Matches(opcodes, "RET").Count;

            return $"{movs},{adds},{pushes},{pops},{xors},{xors},{ors},{cmps},{calls},{jmps},{decs},{incs},{leas},{rets},{movs},{movs},{isMalware}";
        }

        public static string GetBinaryDataSet(string FilePath, int isMalware)
        {
            var txt = "";

            txt = File.ReadAllText(FilePath);

            double Shannon = ShannonEntropy(txt);
            double defaultEntropy = GetEntropyValue(Encoding.ASCII.GetBytes(txt));
            var hash = Hasher.HashBuffer(Encoding.ASCII.GetBytes(txt), (int)new FileInfo(FilePath).Length);
            int isHex = BooleanToNumber(Regex.IsMatch(txt, @"(?:0[xX])?[0-9a-fA-F]+"));
            int isBase64 = BooleanToNumber(Regex.IsMatch(txt, @"/^(?:[a-zA-Z0-9+\/]{4})*(?:|(?:[a-zA-Z0-9+\/]{3}=)|(?:[a-zA-Z0-9+\/]{2}==)|(?:[a-zA-Z0-9+\/]{1}===))$/"));

            //byte[] data = Encoding.ASCII.GetBytes(txt);
            //var binary = string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
            
            return $"{CalculateMD5(FilePath)},{hash},{new FileInfo(FilePath).Length.ToString()},{Shannon.ToString()},{defaultEntropy.ToString()},{isHex.ToString()},{isBase64.ToString()},{isMalware.ToString()}";
            //return string.Format("{0},{1},{2},{3}", CalculateMD5(FilePath), Shannon.ToString(),defaultEntropy.ToString(), isMalware.ToString());
        }

        public static string GetImpHash(string FilePath)
        {
            return new PeFile(FilePath).ImpHash;
        }
        

        static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private static float GetEntropyValue(byte[] c)
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

        public static string GetImportAPI(string FilePath, int isMalware)
        {
            PeFile peFile = new Features.PeFile(FilePath);
            var api = "";

            var md5 = peFile.MD5;
            var Machine = peFile.ImageNtHeaders.FileHeader.Machine;
            var SizeOfOptionalHeader = peFile.ImageNtHeaders.FileHeader.SizeOfOptionalHeader;
            var Characteristics = peFile.ImageNtHeaders.FileHeader.Characteristics;
            var MajorLinkerVersion = peFile.ImageNtHeaders.OptionalHeader.MajorLinkerVersion;
            var MinorLinkerVersion = peFile.ImageNtHeaders.OptionalHeader.MinorLinkerVersion;
            var SizeOfCode = peFile.ImageNtHeaders.OptionalHeader.SizeOfCode;
            var SizeOfInitializedData = peFile.ImageNtHeaders.OptionalHeader.SizeOfInitializedData;
            var SizeOfUninitializedData = peFile.ImageNtHeaders.OptionalHeader.SizeOfUninitializedData;
            var AddressOfEntryPoint = peFile.ImageNtHeaders.OptionalHeader.AddressOfEntryPoint;
            var BaseOfCode = peFile.ImageNtHeaders.OptionalHeader.BaseOfCode;
            var BaseOfData = peFile.ImageNtHeaders.OptionalHeader.BaseOfData;
            var ImageBase = peFile.ImageNtHeaders.OptionalHeader.ImageBase;
            var SectionAlignment = peFile.ImageNtHeaders.OptionalHeader.SectionAlignment;
            var FileAlignment = peFile.ImageNtHeaders.OptionalHeader.FileAlignment;
            var MajorOperatingSystemVersion = peFile.ImageNtHeaders.OptionalHeader.MajorOperatingSystemVersion;
            var MinorOperatingSystemVersion = peFile.ImageNtHeaders.OptionalHeader.MinorOperatingSystemVersion;
            var MajorImageVersion = peFile.ImageNtHeaders.OptionalHeader.MajorImageVersion;
            var MinorImageVersion = peFile.ImageNtHeaders.OptionalHeader.MinorImageVersion;
            var MajorSubsystemVersion = peFile.ImageNtHeaders.OptionalHeader.MajorSubsystemVersion;
            var MinorSubsystemVersion = peFile.ImageNtHeaders.OptionalHeader.MinorSubsystemVersion;
            var SizeOfImage = peFile.ImageNtHeaders.OptionalHeader.SizeOfImage;
            var SizeOfHeaders = peFile.ImageNtHeaders.OptionalHeader.SizeOfHeaders;
            var CheckSum = peFile.ImageNtHeaders.OptionalHeader.CheckSum;
            var Subsystem = peFile.ImageNtHeaders.OptionalHeader.Subsystem;
            var DllCharacteristics = peFile.ImageNtHeaders.OptionalHeader.DllCharacteristics;
            var SizeOfStackReserve = peFile.ImageNtHeaders.OptionalHeader.SizeOfStackReserve;
            var SizeOfStackCommit = peFile.ImageNtHeaders.OptionalHeader.SizeOfStackCommit;
            var SizeOfHeapReserve = peFile.ImageNtHeaders.OptionalHeader.SizeOfHeapReserve;
            var SizeOfHeapCommit = peFile.ImageNtHeaders.OptionalHeader.SizeOfHeapCommit;
            var LoaderFlags = peFile.ImageNtHeaders.OptionalHeader.LoaderFlags;
            var NumberOfRvaAndSizes = peFile.ImageNtHeaders.OptionalHeader.NumberOfRvaAndSizes;

            string peString = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31}", md5, Machine, SizeOfOptionalHeader, Characteristics, MajorLinkerVersion, MinorLinkerVersion, SizeOfCode, SizeOfInitializedData, SizeOfUninitializedData, AddressOfEntryPoint, BaseOfCode, BaseOfData, ImageBase, SectionAlignment, FileAlignment, MajorOperatingSystemVersion, MinorOperatingSystemVersion, MajorImageVersion, MinorImageVersion, MajorSubsystemVersion, MinorSubsystemVersion, SizeOfImage, SizeOfHeaders, CheckSum, Subsystem, DllCharacteristics, SizeOfStackReserve, SizeOfStackCommit, SizeOfHeapReserve, SizeOfHeapCommit, LoaderFlags, NumberOfRvaAndSizes);





            if (peFile.ImportedFunctions != null)
            {
                foreach (var importFunctions in peFile.ImportedFunctions)
                {
                    api += $"[{importFunctions.DLL}].[{importFunctions.Name}]";
                }
            }
            api = $"{peString.ToString()},{api},{isMalware.ToString()}";
            return api;
        }

        public static string GetAPICall(string FilePath, bool isMalware)
        {
            var str = "";

            Features.PeFile m_PeFile = new Features.PeFile(FilePath);
            Apk mApkHeader = new Apk();
            try
            {
                mApkHeader.GetApkHeader(FilePath);
            }
            catch (Exception) { }

            if (Features.PeFile.IsPEFile(FilePath))
            {

                List<bool> m_APIList = new List<bool>();
        
                // DOS Header

                List<object> m_DosHeaderList = new List<object>();
             
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_cblp);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_cp);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_cparhdr);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_crlc);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_cs);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_csum);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_ip);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_lfanew);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_lfarlc);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_magic);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_maxalloc);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_minalloc);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_oemid);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_oeminfo);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_ovno);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_sp);
                m_DosHeaderList.Add(m_PeFile.ImageDosHeader.e_ss);

                // File Header

                List<object> m_FileHeaderList = new List<object>();

                m_FileHeaderList.Add(m_PeFile.ImageNtHeaders.FileHeader.Characteristics);
                m_FileHeaderList.Add(m_PeFile.ImageNtHeaders.FileHeader.Machine);
                m_FileHeaderList.Add(m_PeFile.ImageNtHeaders.FileHeader.NumberOfSections);
                m_FileHeaderList.Add(m_PeFile.ImageNtHeaders.FileHeader.NumberOfSymbols);
                m_FileHeaderList.Add(m_PeFile.ImageNtHeaders.FileHeader.PointerToSymbolTable);
                m_FileHeaderList.Add(m_PeFile.ImageNtHeaders.FileHeader.SizeOfOptionalHeader);
                m_FileHeaderList.Add(m_PeFile.ImageNtHeaders.FileHeader.TimeDateStamp);

                // Optional Header

                List<object> m_OptionalHeaderList = new List<object>();

                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.MajorLinkerVersion);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.MinorLinkerVersion);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.SizeOfCode);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.SizeOfInitializedData);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.SizeOfUninitializedData);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.AddressOfEntryPoint);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.BaseOfCode);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.BaseOfData);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.ImageBase);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.SectionAlignment);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.FileAlignment);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.MajorOperatingSystemVersion);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.MinorOperatingSystemVersion);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.MajorImageVersion);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.MinorImageVersion);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.MajorSubsystemVersion);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.MinorSubsystemVersion);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.SizeOfImage);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.SizeOfHeaders);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.CheckSum);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.Subsystem);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.DllCharacteristics);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.SizeOfStackReserve);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.SizeOfStackCommit);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.SizeOfHeapReserve);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.SizeOfHeapCommit);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.LoaderFlags);
                m_OptionalHeaderList.Add(m_PeFile.ImageNtHeaders.OptionalHeader.NumberOfRvaAndSizes);

                // Section Header

                List<object> m_SectionHeaderList = new List<object>();

                uint sCharacteristics1 = 0;
                uint sCharacteristics2 = 0;
                uint sCharacteristics3 = 0;
                uint sCharacteristics4 = 0;
                uint sCharacteristics5 = 0;
                uint sCharacteristics6 = 0;
                uint sCharacteristics7 = 0;
                uint sCharacteristics8 = 0;

                ushort NumberOfLinenumbers1 = 0;
                ushort NumberOfLinenumbers2 = 0;
                ushort NumberOfLinenumbers3 = 0;
                ushort NumberOfLinenumbers4 = 0;
                ushort NumberOfLinenumbers5 = 0;
                ushort NumberOfLinenumbers6 = 0;
                ushort NumberOfLinenumbers7 = 0;
                ushort NumberOfLinenumbers8 = 0;

                ushort NumberOfRelocations1 = 0;
                ushort NumberOfRelocations2 = 0;
                ushort NumberOfRelocations3 = 0;
                ushort NumberOfRelocations4 = 0;
                ushort NumberOfRelocations5 = 0;
                ushort NumberOfRelocations6 = 0;
                ushort NumberOfRelocations7 = 0;
                ushort NumberOfRelocations8 = 0;

                uint PhysicalAddress1 = 0;
                uint PhysicalAddress2 = 0;
                uint PhysicalAddress3 = 0;
                uint PhysicalAddress4 = 0;
                uint PhysicalAddress5 = 0;
                uint PhysicalAddress6 = 0;
                uint PhysicalAddress7 = 0;
                uint PhysicalAddress8 = 0;

                uint PointerToLinenumbers1 = 0;
                uint PointerToLinenumbers2 = 0;
                uint PointerToLinenumbers3 = 0;
                uint PointerToLinenumbers4 = 0;
                uint PointerToLinenumbers5 = 0;
                uint PointerToLinenumbers6 = 0;
                uint PointerToLinenumbers7 = 0;
                uint PointerToLinenumbers8 = 0;

                uint PointerToRawData1 = 0;
                uint PointerToRawData2 = 0;
                uint PointerToRawData3 = 0;
                uint PointerToRawData4 = 0;
                uint PointerToRawData5 = 0;
                uint PointerToRawData6 = 0;
                uint PointerToRawData7 = 0;
                uint PointerToRawData8 = 0;

                uint PointerToRelocations1 = 0;
                uint PointerToRelocations2 = 0;
                uint PointerToRelocations3 = 0;
                uint PointerToRelocations4 = 0;
                uint PointerToRelocations5 = 0;
                uint PointerToRelocations6 = 0;
                uint PointerToRelocations7 = 0;
                uint PointerToRelocations8 = 0;

                uint SizeOfRawData1 = 0;
                uint SizeOfRawData2 = 0;
                uint SizeOfRawData3 = 0;
                uint SizeOfRawData4 = 0;
                uint SizeOfRawData5 = 0;
                uint SizeOfRawData6 = 0;
                uint SizeOfRawData7 = 0;
                uint SizeOfRawData8 = 0;

                uint VirtualAddress1 = 0;
                uint VirtualAddress2 = 0;
                uint VirtualAddress3 = 0;
                uint VirtualAddress4 = 0;
                uint VirtualAddress5 = 0;
                uint VirtualAddress6 = 0;
                uint VirtualAddress7 = 0;
                uint VirtualAddress8 = 0;

                uint VirtualSize1 = 0;
                uint VirtualSize2 = 0;
                uint VirtualSize3 = 0;
                uint VirtualSize4 = 0;
                uint VirtualSize5 = 0;
                uint VirtualSize6 = 0;
                uint VirtualSize7 = 0;
                uint VirtualSize8 = 0;

                try
                {
                    sCharacteristics1 = m_PeFile.ImageSectionHeaders[0].Characteristics;
                    sCharacteristics2 = m_PeFile.ImageSectionHeaders[1].Characteristics;
                    sCharacteristics3 = m_PeFile.ImageSectionHeaders[2].Characteristics;
                    sCharacteristics4 = m_PeFile.ImageSectionHeaders[3].Characteristics;
                    sCharacteristics5 = m_PeFile.ImageSectionHeaders[4].Characteristics;
                    sCharacteristics6 = m_PeFile.ImageSectionHeaders[5].Characteristics;
                    sCharacteristics7 = m_PeFile.ImageSectionHeaders[6].Characteristics;
                    sCharacteristics8 = m_PeFile.ImageSectionHeaders[7].Characteristics;

                    NumberOfLinenumbers1 = m_PeFile.ImageSectionHeaders[0].NumberOfLinenumbers;
                    NumberOfLinenumbers2 = m_PeFile.ImageSectionHeaders[1].NumberOfLinenumbers;
                    NumberOfLinenumbers3 = m_PeFile.ImageSectionHeaders[2].NumberOfLinenumbers;
                    NumberOfLinenumbers4 = m_PeFile.ImageSectionHeaders[3].NumberOfLinenumbers;
                    NumberOfLinenumbers5 = m_PeFile.ImageSectionHeaders[4].NumberOfLinenumbers;
                    NumberOfLinenumbers6 = m_PeFile.ImageSectionHeaders[5].NumberOfLinenumbers;
                    NumberOfLinenumbers7 = m_PeFile.ImageSectionHeaders[6].NumberOfLinenumbers;
                    NumberOfLinenumbers8 = m_PeFile.ImageSectionHeaders[7].NumberOfLinenumbers;

                    NumberOfRelocations1 = m_PeFile.ImageSectionHeaders[0].NumberOfRelocations;
                    NumberOfRelocations2 = m_PeFile.ImageSectionHeaders[1].NumberOfRelocations;
                    NumberOfRelocations3 = m_PeFile.ImageSectionHeaders[2].NumberOfRelocations;
                    NumberOfRelocations4 = m_PeFile.ImageSectionHeaders[3].NumberOfRelocations;
                    NumberOfRelocations5 = m_PeFile.ImageSectionHeaders[4].NumberOfRelocations;
                    NumberOfRelocations6 = m_PeFile.ImageSectionHeaders[5].NumberOfRelocations;
                    NumberOfRelocations7 = m_PeFile.ImageSectionHeaders[6].NumberOfRelocations;
                    NumberOfRelocations8 = m_PeFile.ImageSectionHeaders[7].NumberOfRelocations;

                    PhysicalAddress1 = m_PeFile.ImageSectionHeaders[0].PhysicalAddress;
                    PhysicalAddress2 = m_PeFile.ImageSectionHeaders[1].PhysicalAddress;
                    PhysicalAddress3 = m_PeFile.ImageSectionHeaders[2].PhysicalAddress;
                    PhysicalAddress4 = m_PeFile.ImageSectionHeaders[3].PhysicalAddress;
                    PhysicalAddress5 = m_PeFile.ImageSectionHeaders[4].PhysicalAddress;
                    PhysicalAddress6 = m_PeFile.ImageSectionHeaders[5].PhysicalAddress;
                    PhysicalAddress7 = m_PeFile.ImageSectionHeaders[6].PhysicalAddress;
                    PhysicalAddress8 = m_PeFile.ImageSectionHeaders[7].PhysicalAddress;

                    PointerToLinenumbers1 = m_PeFile.ImageSectionHeaders[0].PointerToLinenumbers;
                    PointerToLinenumbers2 = m_PeFile.ImageSectionHeaders[1].PointerToLinenumbers;
                    PointerToLinenumbers3 = m_PeFile.ImageSectionHeaders[2].PointerToLinenumbers;
                    PointerToLinenumbers4 = m_PeFile.ImageSectionHeaders[3].PointerToLinenumbers;
                    PointerToLinenumbers5 = m_PeFile.ImageSectionHeaders[4].PointerToLinenumbers;
                    PointerToLinenumbers6 = m_PeFile.ImageSectionHeaders[5].PointerToLinenumbers;
                    PointerToLinenumbers7 = m_PeFile.ImageSectionHeaders[6].PointerToLinenumbers;
                    PointerToLinenumbers8 = m_PeFile.ImageSectionHeaders[7].PointerToLinenumbers;

                    PointerToRawData1 = m_PeFile.ImageSectionHeaders[0].PointerToRawData;
                    PointerToRawData2 = m_PeFile.ImageSectionHeaders[1].PointerToRawData;
                    PointerToRawData3 = m_PeFile.ImageSectionHeaders[2].PointerToRawData;
                    PointerToRawData4 = m_PeFile.ImageSectionHeaders[3].PointerToRawData;
                    PointerToRawData5 = m_PeFile.ImageSectionHeaders[4].PointerToRawData;
                    PointerToRawData6 = m_PeFile.ImageSectionHeaders[5].PointerToRawData;
                    PointerToRawData7 = m_PeFile.ImageSectionHeaders[6].PointerToRawData;
                    PointerToRawData8 = m_PeFile.ImageSectionHeaders[7].PointerToRawData;

                    PointerToRelocations1 = m_PeFile.ImageSectionHeaders[0].PointerToRelocations;
                    PointerToRelocations2 = m_PeFile.ImageSectionHeaders[1].PointerToRelocations;
                    PointerToRelocations3 = m_PeFile.ImageSectionHeaders[2].PointerToRelocations;
                    PointerToRelocations4 = m_PeFile.ImageSectionHeaders[3].PointerToRelocations;
                    PointerToRelocations5 = m_PeFile.ImageSectionHeaders[4].PointerToRelocations;
                    PointerToRelocations6 = m_PeFile.ImageSectionHeaders[5].PointerToRelocations;
                    PointerToRelocations7 = m_PeFile.ImageSectionHeaders[6].PointerToRelocations;
                    PointerToRelocations8 = m_PeFile.ImageSectionHeaders[7].PointerToRelocations;

                    SizeOfRawData1 = m_PeFile.ImageSectionHeaders[0].SizeOfRawData;
                    SizeOfRawData2 = m_PeFile.ImageSectionHeaders[1].SizeOfRawData;
                    SizeOfRawData3 = m_PeFile.ImageSectionHeaders[2].SizeOfRawData;
                    SizeOfRawData4 = m_PeFile.ImageSectionHeaders[3].SizeOfRawData;
                    SizeOfRawData5 = m_PeFile.ImageSectionHeaders[4].SizeOfRawData;
                    SizeOfRawData6 = m_PeFile.ImageSectionHeaders[5].SizeOfRawData;
                    SizeOfRawData7 = m_PeFile.ImageSectionHeaders[6].SizeOfRawData;
                    SizeOfRawData8 = m_PeFile.ImageSectionHeaders[7].SizeOfRawData;

                    VirtualAddress1 = m_PeFile.ImageSectionHeaders[0].VirtualAddress;
                    VirtualAddress2 = m_PeFile.ImageSectionHeaders[1].VirtualAddress;
                    VirtualAddress3 = m_PeFile.ImageSectionHeaders[2].VirtualAddress;
                    VirtualAddress4 = m_PeFile.ImageSectionHeaders[3].VirtualAddress;
                    VirtualAddress5 = m_PeFile.ImageSectionHeaders[4].VirtualAddress;
                    VirtualAddress6 = m_PeFile.ImageSectionHeaders[5].VirtualAddress;
                    VirtualAddress7 = m_PeFile.ImageSectionHeaders[6].VirtualAddress;
                    VirtualAddress8 = m_PeFile.ImageSectionHeaders[7].VirtualAddress;

                    VirtualSize1 = m_PeFile.ImageSectionHeaders[0].VirtualSize;
                    VirtualSize2 = m_PeFile.ImageSectionHeaders[1].VirtualSize;
                    VirtualSize3 = m_PeFile.ImageSectionHeaders[2].VirtualSize;
                    VirtualSize4 = m_PeFile.ImageSectionHeaders[3].VirtualSize;
                    VirtualSize5 = m_PeFile.ImageSectionHeaders[4].VirtualSize;
                    VirtualSize6 = m_PeFile.ImageSectionHeaders[5].VirtualSize;
                    VirtualSize7 = m_PeFile.ImageSectionHeaders[6].VirtualSize;
                    VirtualSize8 = m_PeFile.ImageSectionHeaders[7].VirtualSize;
                } 
                catch( Exception)
                {

                }

                m_SectionHeaderList.Add(sCharacteristics1);
                m_SectionHeaderList.Add(sCharacteristics2);
                m_SectionHeaderList.Add(sCharacteristics3);
                m_SectionHeaderList.Add(sCharacteristics4);
                m_SectionHeaderList.Add(sCharacteristics5);
                m_SectionHeaderList.Add(sCharacteristics6);
                m_SectionHeaderList.Add(sCharacteristics7);
                m_SectionHeaderList.Add(sCharacteristics8);

                m_SectionHeaderList.Add(NumberOfLinenumbers1);
                m_SectionHeaderList.Add(NumberOfLinenumbers2);
                m_SectionHeaderList.Add(NumberOfLinenumbers3);
                m_SectionHeaderList.Add(NumberOfLinenumbers4);
                m_SectionHeaderList.Add(NumberOfLinenumbers5);
                m_SectionHeaderList.Add(NumberOfLinenumbers6);
                m_SectionHeaderList.Add(NumberOfLinenumbers7);
                m_SectionHeaderList.Add(NumberOfLinenumbers8);

                m_SectionHeaderList.Add(NumberOfRelocations1);
                m_SectionHeaderList.Add(NumberOfRelocations2);
                m_SectionHeaderList.Add(NumberOfRelocations3);
                m_SectionHeaderList.Add(NumberOfRelocations4);
                m_SectionHeaderList.Add(NumberOfRelocations5);
                m_SectionHeaderList.Add(NumberOfRelocations6);
                m_SectionHeaderList.Add(NumberOfRelocations7);
                m_SectionHeaderList.Add(NumberOfRelocations8);

                m_SectionHeaderList.Add(PhysicalAddress1);
                m_SectionHeaderList.Add(PhysicalAddress2);
                m_SectionHeaderList.Add(PhysicalAddress3);
                m_SectionHeaderList.Add(PhysicalAddress4);
                m_SectionHeaderList.Add(PhysicalAddress5);
                m_SectionHeaderList.Add(PhysicalAddress6);
                m_SectionHeaderList.Add(PhysicalAddress7);
                m_SectionHeaderList.Add(PhysicalAddress8);

                m_SectionHeaderList.Add(PointerToLinenumbers1);
                m_SectionHeaderList.Add(PointerToLinenumbers2);
                m_SectionHeaderList.Add(PointerToLinenumbers3);
                m_SectionHeaderList.Add(PointerToLinenumbers4);
                m_SectionHeaderList.Add(PointerToLinenumbers5);
                m_SectionHeaderList.Add(PointerToLinenumbers6);
                m_SectionHeaderList.Add(PointerToLinenumbers7);
                m_SectionHeaderList.Add(PointerToLinenumbers8);

                m_SectionHeaderList.Add(PointerToRawData1);
                m_SectionHeaderList.Add(PointerToRawData2);
                m_SectionHeaderList.Add(PointerToRawData3);
                m_SectionHeaderList.Add(PointerToRawData4);
                m_SectionHeaderList.Add(PointerToRawData5);
                m_SectionHeaderList.Add(PointerToRawData6);
                m_SectionHeaderList.Add(PointerToRawData7);
                m_SectionHeaderList.Add(PointerToRawData8);

                m_SectionHeaderList.Add(PointerToRelocations1);
                m_SectionHeaderList.Add(PointerToRelocations2);
                m_SectionHeaderList.Add(PointerToRelocations3);
                m_SectionHeaderList.Add(PointerToRelocations4);
                m_SectionHeaderList.Add(PointerToRelocations5);
                m_SectionHeaderList.Add(PointerToRelocations6);
                m_SectionHeaderList.Add(PointerToRelocations7);
                m_SectionHeaderList.Add(PointerToRelocations8);

                m_SectionHeaderList.Add(SizeOfRawData1);
                m_SectionHeaderList.Add(SizeOfRawData2);
                m_SectionHeaderList.Add(SizeOfRawData3);
                m_SectionHeaderList.Add(SizeOfRawData4);
                m_SectionHeaderList.Add(SizeOfRawData5);
                m_SectionHeaderList.Add(SizeOfRawData6);
                m_SectionHeaderList.Add(SizeOfRawData7);
                m_SectionHeaderList.Add(SizeOfRawData8);

                m_SectionHeaderList.Add(VirtualAddress1);
                m_SectionHeaderList.Add(VirtualAddress2);
                m_SectionHeaderList.Add(VirtualAddress3);
                m_SectionHeaderList.Add(VirtualAddress4);
                m_SectionHeaderList.Add(VirtualAddress5);
                m_SectionHeaderList.Add(VirtualAddress6);
                m_SectionHeaderList.Add(VirtualAddress7);
                m_SectionHeaderList.Add(VirtualAddress8);

                m_SectionHeaderList.Add(VirtualSize1);
                m_SectionHeaderList.Add(VirtualSize2);
                m_SectionHeaderList.Add(VirtualSize3);
                m_SectionHeaderList.Add(VirtualSize4);
                m_SectionHeaderList.Add(VirtualSize5);
                m_SectionHeaderList.Add(VirtualSize6);
                m_SectionHeaderList.Add(VirtualSize7);
                m_SectionHeaderList.Add(VirtualSize8);

                // Dex Header

                List<object> m_DexHeaderList = new List<object>();
                m_DexHeaderList.Add(mApkHeader.CheckSum);
                m_DexHeaderList.Add(mApkHeader.SA1);
                m_DexHeaderList.Add(mApkHeader.FileSize);
                m_DexHeaderList.Add(mApkHeader.HeaderSize);
                m_DexHeaderList.Add(mApkHeader.EndianTag);
                m_DexHeaderList.Add(mApkHeader.LinkSize);
                m_DexHeaderList.Add(mApkHeader.LinkOff);
                m_DexHeaderList.Add(mApkHeader.MapOff);
                m_DexHeaderList.Add(mApkHeader.sIdsSize);
                m_DexHeaderList.Add(mApkHeader.sIdsOff);
                m_DexHeaderList.Add(mApkHeader.tIdsSize);
                m_DexHeaderList.Add(mApkHeader.tIdsOff);
                m_DexHeaderList.Add(mApkHeader.pIdsSize);
                m_DexHeaderList.Add(mApkHeader.pIdsOff);
                m_DexHeaderList.Add(mApkHeader.fIdsSize);
                m_DexHeaderList.Add(mApkHeader.fIdsOff);
                m_DexHeaderList.Add(mApkHeader.mIdsSize);
                m_DexHeaderList.Add(mApkHeader.mIdsOff);
                m_DexHeaderList.Add(mApkHeader.cDefSize);
                m_DexHeaderList.Add(mApkHeader.cDefOff);
                m_DexHeaderList.Add(mApkHeader.DataSize);
                m_DexHeaderList.Add(mApkHeader.DataOff);
                m_DexHeaderList.Add(mApkHeader.Entropy);

                str = "";
                m_APIList.Clear();
                str += $"{m_PeFile.MD5},";
        
                m_APIList.Add(m_PeFile.HasValidComDescriptor);
                m_APIList.Add(m_PeFile.HasValidExceptionDir);
                m_APIList.Add(m_PeFile.HasValidExportDir);
                m_APIList.Add(m_PeFile.HasValidImportDir);
                m_APIList.Add(m_PeFile.HasValidRelocDir);
                m_APIList.Add(m_PeFile.HasValidResourceDir);
                m_APIList.Add(m_PeFile.HasValidSecurityDir);
                m_APIList.Add(m_PeFile.Is32Bit);
                m_APIList.Add(m_PeFile.Is64Bit);
                m_APIList.Add(m_PeFile.IsDLL);
                m_APIList.Add(m_PeFile.IsEXE);
                m_APIList.Add(m_PeFile.IsSigned);
                m_APIList.Add(m_PeFile.IsValidPeFile);
               
                bool Accept = false;
                bool AdjustTokenPrivileges = false;
                bool AttachThreadInput = false;
                bool Bind = false;
                bool BitBlt = false;
                bool CertOpenSystemStore = false;
                bool Connect = false;
                bool ConnectNamedPipe = false;
                bool ControlService = false;
                bool CreateFile = false;
                bool CreateFileMapping = false;
                bool CreateMutex = false;
                bool CreateProcess = false;
                bool CreateRemoteThread = false;
                bool CreateService = false;
                bool CreateToolhelp32Snapshot = false;
                bool CryptAcquireContext = false;
                bool DeviceIoControl = false;
                bool EnableExecuteProtectionSupport = false;
                bool EnumProcesses = false;
                bool EnumProcessModules = false;
                bool FindFirstFile = false;
                bool FindNextFile = false;
                bool FindResource = false;
                bool FindWindow = false;
                bool FtpPutFile = false;
                bool GetAdaptersInfo = false;
                bool GetAsyncKeyState = false;
                bool GetDC = false;
                bool GetForegroundWindow = false;
                bool Gethostbyname = false;
                bool Gethostname = false;
                bool GetKeyState = false;
                bool GetModuleFileName = false;
                bool GetModuleHandle = false;
                bool GetProcAddress = false;
                bool GetStartupInfo = false;
                bool GetSystemDefaultLangId = false;
                bool GetTempPath = false;
                bool GetThreadContext = false;
                bool GetVersionEx = false;
                bool GetWindowsDirectory = false;
                bool inet_addr = false;
                bool InternetOpen = false;
                bool InternetOpenUrl = false;
                bool InternetReadFile = false;
                bool InternetWriteFile = false;
                bool CreateFileA = false;
                bool CreateFileW = false;
                bool GetLastError = false;
                bool WriteFile = false;
                bool Sleep = false;
                bool CloseHandle = false;
                bool CompareStringA = false;
                bool CreateEventA = false;
                bool CreateThread = false;
                bool DeleteCriticalSection = false;
                bool EnterCriticalSection = false;
                bool EnumCalendarInfoA = false;
                bool ExitProcess = false;
                bool FindClose = false;
                bool FindFirstFileA = false;
                bool FindResourceA = false;
                bool FormatMessageA = false;
                bool FreeLibrary = false;
                bool FreeResource = false;
                bool GetACP = false;
                bool GetCommandLineA = false;
                bool GetCPInfo = false;
                bool GetCurrentProcessId = false;
                bool GetCurrentThreadId = false;
                bool GetDateFormatA = false;
                bool GetDiskFreeSpaceA = false;
                bool GetFullPathNameA = false;
                bool GetLocaleInfoA = false;
                bool GetLocalTime = false;
                bool GetModuleFileNameA = false;
                bool GetModuleHandleA = false;
                bool GetProfileStringA = false;
                bool GetStartupInfoA = false;
                bool GetStdHandle = false;
                bool GetStringTypeExA = false;
                bool GetSystemInfo = false;
                bool GetThreadLocale = false;
                bool GetTickCount = false;
                bool GetVersion = false;
                bool GetVersionExA = false;
                bool GlobalAddAtomA = false;
                bool GlobalAlloc = false;
                bool GlobalDeleteAtom = false;
                bool GlobalFindAtomA = false;
                bool GlobalFree = false;
                bool GlobalHandle = false;
                bool GlobalLock = false;
                bool GlobalReAlloc = false;
                bool GlobalSize = false;
                bool GlobalUnlock = false;
                bool InitializeCriticalSection = false;
                bool InterlockedDecrement = false;
                bool InterlockedExchange = false;
                bool InterlockedIncrement = false;
                bool LeaveCriticalSection = false;
                bool LoadLibraryA = false;
                bool LoadLibraryExA = false;
                bool LoadResource = false;
                bool LocalAlloc = false;
                bool LocalFree = false;
                bool LockResource = false;
                bool lstrcpyA = false;
                bool lstrcpynA = false;
                bool lstrlenA = false;
                bool MulDiv = false;
                bool MultiByteToWideChar = false;
                bool RaiseException = false;
                bool ReadFile = false;
                bool ResetEvent = false;
                bool RtlUnwind = false;
                bool SetEndOfFile = false;
                bool SetErrorMode = false;
                bool SetEvent = false;
                bool SetFilePointer = false;
                bool SetThreadLocale = false;
                bool SizeofResource = false;
                bool SleepEx = false;
                bool TlsGetValue = false;
                bool TlsSetValue = false;
                bool UnhandledExceptionFilter = false;
                bool VirtualAlloc = false;
                bool VirtualFree = false;
                bool VirtualQuery = false;
                bool WaitForSingleObject = false;
                bool WideCharToMultiByte = false;
                bool RegCloseKey = false;
                bool RegQueryValueExA = false;
                bool GetCurrentProcess = false;
                bool TerminateProcess = false;
                bool SetUnhandledExceptionFilter = false;
                bool QueryPerformanceCounter = false;
                bool IsDebuggerPresent = false;
                bool DisableThreadLibraryCalls = false;

                var strAPI = "";

                if (m_PeFile.ImportedFunctions != null)
                {
                    foreach (var importFunctions in m_PeFile.ImportedFunctions)
                    {
                        strAPI += $" {importFunctions.Name} ";
                    }
                }

                Accept = strAPI.ToLower().Contains("Accept".ToLower());
                AdjustTokenPrivileges = strAPI.ToLower().Contains("AdjustTokenPrivileges".ToLower());
                AttachThreadInput = strAPI.ToLower().Contains("AttachThreadInput".ToLower());
                Bind = strAPI.ToLower().Contains("Bind".ToLower());
                BitBlt = strAPI.ToLower().Contains("BitBlt".ToLower());
                CertOpenSystemStore = strAPI.ToLower().Contains("CertOpenSystemStore".ToLower());
                Connect = strAPI.ToLower().Contains("Connect".ToLower());
                ConnectNamedPipe = strAPI.ToLower().Contains("ConnectNamedPipe".ToLower());
                ControlService = strAPI.ToLower().Contains("ControlService".ToLower());
                CreateFile = strAPI.ToLower().Contains("CreateFile".ToLower());
                CreateFileMapping = strAPI.ToLower().Contains("CreateFileMapping".ToLower());
                CreateMutex = strAPI.ToLower().Contains("CreateMutex".ToLower());
                CreateProcess = strAPI.ToLower().Contains("CreateProcess".ToLower());
                CreateRemoteThread = strAPI.ToLower().Contains("CreateRemoteThread".ToLower());
                CreateService = strAPI.ToLower().Contains("CreateService".ToLower());
                CreateToolhelp32Snapshot = strAPI.ToLower().Contains("CreateToolhelp32Snapshot".ToLower());
                CryptAcquireContext = strAPI.ToLower().Contains("CryptAcquireContext".ToLower());
                DeviceIoControl = strAPI.ToLower().Contains("DeviceIoControl".ToLower());
                EnableExecuteProtectionSupport = strAPI.ToLower().Contains("EnableExecuteProtectionSupport".ToLower());
                EnumProcesses = strAPI.ToLower().Contains("EnumProcesses".ToLower());
                EnumProcessModules = strAPI.ToLower().Contains("EnumProcessModules".ToLower());
                FindFirstFile = strAPI.ToLower().Contains("FindFirstFile".ToLower());
                FindNextFile = strAPI.ToLower().Contains("FindNextFile".ToLower());
                FindResource = strAPI.ToLower().Contains("FindResource".ToLower());
                FindWindow = strAPI.ToLower().Contains("FindWindow".ToLower());
                FtpPutFile = strAPI.ToLower().Contains("FtpPutFile".ToLower());
                GetAdaptersInfo = strAPI.ToLower().Contains("GetAdaptersInfo".ToLower());
                GetAsyncKeyState = strAPI.ToLower().Contains("GetAsyncKeyState".ToLower());
                GetDC = strAPI.ToLower().Contains("GetDC".ToLower());
                GetForegroundWindow = strAPI.ToLower().Contains("GetForegroundWindow".ToLower());
                Gethostbyname = strAPI.ToLower().Contains("Gethostbyname".ToLower());
                Gethostname = strAPI.ToLower().Contains("Gethostname".ToLower());
                GetKeyState = strAPI.ToLower().Contains("GetKeyState".ToLower());
                GetModuleFileName = strAPI.ToLower().Contains("GetModuleFileName".ToLower());
                GetModuleHandle = strAPI.ToLower().Contains("GetModuleHandle".ToLower());
                GetProcAddress = strAPI.ToLower().Contains("GetProcAddress".ToLower());
                GetStartupInfo = strAPI.ToLower().Contains("GetStartupInfo".ToLower());
                GetSystemDefaultLangId = strAPI.ToLower().Contains("GetSystemDefaultLangId".ToLower());
                GetTempPath = strAPI.ToLower().Contains("GetTempPath".ToLower());
                GetThreadContext = strAPI.ToLower().Contains("GetThreadContext".ToLower());
                GetVersionEx = strAPI.ToLower().Contains("GetVersionEx".ToLower());
                GetWindowsDirectory = strAPI.ToLower().Contains("GetWindowsDirectory".ToLower());
                inet_addr = strAPI.ToLower().Contains("inet_addr".ToLower());
                InternetOpen = strAPI.ToLower().Contains("InternetOpen".ToLower());
                InternetOpenUrl = strAPI.ToLower().Contains("InternetOpenUrl".ToLower());
                InternetReadFile = strAPI.ToLower().Contains("InternetReadFile".ToLower());
                InternetWriteFile = strAPI.ToLower().Contains("InternetWriteFile".ToLower());
                CloseHandle = strAPI.ToLower().Contains("CloseHandle".ToLower());
                CompareStringA = strAPI.ToLower().Contains("CompareStringA".ToLower());
                CreateEventA = strAPI.ToLower().Contains("CreateEventA".ToLower());
                CreateFileA = strAPI.ToLower().Contains("CreateFileA".ToLower());
                CreateThread = strAPI.ToLower().Contains("CreateThread".ToLower());
                DeleteCriticalSection = strAPI.ToLower().Contains("DeleteCriticalSection".ToLower());
                EnterCriticalSection = strAPI.ToLower().Contains("EnterCriticalSection".ToLower());
                EnumCalendarInfoA = strAPI.ToLower().Contains("EnumCalendarInfoA".ToLower());
                ExitProcess = strAPI.ToLower().Contains("ExitProcess".ToLower());
                FindClose = strAPI.ToLower().Contains("FindClose".ToLower());
                FindFirstFileA = strAPI.ToLower().Contains("FindFirstFileA".ToLower());
                FindResourceA = strAPI.ToLower().Contains("FindResourceA".ToLower());
                FormatMessageA = strAPI.ToLower().Contains("FormatMessageA".ToLower());
                FreeLibrary = strAPI.ToLower().Contains("FreeLibrary".ToLower());
                FreeResource = strAPI.ToLower().Contains("FreeResource".ToLower());
                GetACP = strAPI.ToLower().Contains("GetACP".ToLower());
                GetCommandLineA = strAPI.ToLower().Contains("GetCommandLineA".ToLower());
                GetCPInfo = strAPI.ToLower().Contains("GetCPInfo".ToLower());
                GetCurrentProcessId = strAPI.ToLower().Contains("GetCurrentProcessId".ToLower());
                GetCurrentThreadId = strAPI.ToLower().Contains("GetCurrentThreadId".ToLower());
                GetDateFormatA = strAPI.ToLower().Contains("GetDateFormatA".ToLower());
                GetDiskFreeSpaceA = strAPI.ToLower().Contains("GetDiskFreeSpaceA".ToLower());
                GetFullPathNameA = strAPI.ToLower().Contains("GetFullPathNameA".ToLower());
                GetLastError = strAPI.ToLower().Contains("GetLastError".ToLower());
                GetLocaleInfoA = strAPI.ToLower().Contains("GetLocaleInfoA".ToLower());
                GetLocalTime = strAPI.ToLower().Contains("GetLocalTime".ToLower());
                GetModuleFileNameA = strAPI.ToLower().Contains("GetModuleFileNameA".ToLower());
                GetModuleHandleA = strAPI.ToLower().Contains("GetModuleHandleA".ToLower());
                GetProcAddress = strAPI.ToLower().Contains("GetProcAddress".ToLower());
                GetProfileStringA = strAPI.ToLower().Contains("GetProfileStringA".ToLower());
                GetStartupInfoA = strAPI.ToLower().Contains("GetStartupInfoA".ToLower());
                GetStdHandle = strAPI.ToLower().Contains("GetStdHandle".ToLower());
                GetStringTypeExA = strAPI.ToLower().Contains("GetStringTypeExA".ToLower());
                GetSystemInfo = strAPI.ToLower().Contains("GetSystemInfo".ToLower());
                GetThreadLocale = strAPI.ToLower().Contains("GetThreadLocale".ToLower());
                GetTickCount = strAPI.ToLower().Contains("GetTickCount".ToLower());
                GetVersion = strAPI.ToLower().Contains("GetVersion".ToLower());
                GetVersionExA = strAPI.ToLower().Contains("GetVersionExA".ToLower());
                GlobalAddAtomA = strAPI.ToLower().Contains("GlobalAddAtomA".ToLower());
                GlobalAlloc = strAPI.ToLower().Contains("GlobalAlloc".ToLower());
                GlobalDeleteAtom = strAPI.ToLower().Contains("GlobalDeleteAtom".ToLower());
                GlobalFindAtomA = strAPI.ToLower().Contains("GlobalFindAtomA".ToLower());
                GlobalFree = strAPI.ToLower().Contains("GlobalFree".ToLower());
                GlobalHandle = strAPI.ToLower().Contains("GlobalHandle".ToLower());
                GlobalLock = strAPI.ToLower().Contains("GlobalLock".ToLower());
                GlobalReAlloc = strAPI.ToLower().Contains("GlobalReAlloc".ToLower());
                GlobalSize = strAPI.ToLower().Contains("GlobalSize".ToLower());
                GlobalUnlock = strAPI.ToLower().Contains("GlobalUnlock".ToLower());
                InitializeCriticalSection = strAPI.ToLower().Contains("InitializeCriticalSection".ToLower());
                InterlockedDecrement = strAPI.ToLower().Contains("InterlockedDecrement".ToLower());
                InterlockedExchange = strAPI.ToLower().Contains("InterlockedExchange".ToLower());
                InterlockedIncrement = strAPI.ToLower().Contains("InterlockedIncrement".ToLower());
                LeaveCriticalSection = strAPI.ToLower().Contains("LeaveCriticalSection".ToLower());
                LoadLibraryA = strAPI.ToLower().Contains("LoadLibraryA".ToLower());
                LoadLibraryExA = strAPI.ToLower().Contains("LoadLibraryExA".ToLower());
                LoadResource = strAPI.ToLower().Contains("LoadResource".ToLower());
                LocalAlloc = strAPI.ToLower().Contains("LocalAlloc".ToLower());
                LocalFree = strAPI.ToLower().Contains("LocalFree".ToLower());
                LockResource = strAPI.ToLower().Contains("LockResource".ToLower());
                lstrcpyA = strAPI.ToLower().Contains("lstrcpyA".ToLower());
                lstrcpynA = strAPI.ToLower().Contains("lstrcpynA".ToLower());
                lstrlenA = strAPI.ToLower().Contains("lstrlenA".ToLower());
                MulDiv = strAPI.ToLower().Contains("MulDiv".ToLower());
                MultiByteToWideChar = strAPI.ToLower().Contains("MultiByteToWideChar".ToLower());
                RaiseException = strAPI.ToLower().Contains("RaiseException".ToLower());
                ReadFile = strAPI.ToLower().Contains("ReadFile".ToLower());
                ResetEvent = strAPI.ToLower().Contains("ResetEvent".ToLower());
                RtlUnwind = strAPI.ToLower().Contains("RtlUnwind".ToLower());
                SetEndOfFile = strAPI.ToLower().Contains("SetEndOfFile".ToLower());
                SetErrorMode = strAPI.ToLower().Contains("SetErrorMode".ToLower());
                SetEvent = strAPI.ToLower().Contains("SetEvent".ToLower());
                SetFilePointer = strAPI.ToLower().Contains("SetFilePointer".ToLower());
                SetThreadLocale = strAPI.ToLower().Contains("SetThreadLocale".ToLower());
                SizeofResource = strAPI.ToLower().Contains("SizeofResource".ToLower());
                Sleep = strAPI.ToLower().Contains("Sleep".ToLower());
                SleepEx = strAPI.ToLower().Contains("SleepEx".ToLower());
                TlsGetValue = strAPI.ToLower().Contains("TlsGetValue".ToLower());
                TlsSetValue = strAPI.ToLower().Contains("TlsSetValue".ToLower());
                UnhandledExceptionFilter = strAPI.ToLower().Contains("UnhandledExceptionFilter".ToLower());
                VirtualAlloc = strAPI.ToLower().Contains("VirtualAlloc".ToLower());
                VirtualFree = strAPI.ToLower().Contains("VirtualFree".ToLower());
                VirtualQuery = strAPI.ToLower().Contains("VirtualQuery".ToLower());
                WaitForSingleObject = strAPI.ToLower().Contains("WaitForSingleObject".ToLower());
                WideCharToMultiByte = strAPI.ToLower().Contains("WideCharToMultiByte".ToLower());
                WriteFile = strAPI.ToLower().Contains("WriteFile".ToLower());
                RegCloseKey = strAPI.ToLower().Contains("RegCloseKey".ToLower());
                RegQueryValueExA = strAPI.ToLower().Contains("RegQueryValueExA".ToLower());
                GetCurrentProcess = strAPI.ToLower().Contains("GetCurrentProcess".ToLower());
                TerminateProcess = strAPI.ToLower().Contains("TerminateProcess".ToLower());
                SetUnhandledExceptionFilter = strAPI.ToLower().Contains("SetUnhandledExceptionFilter".ToLower());
                QueryPerformanceCounter = strAPI.ToLower().Contains("QueryPerformanceCounter".ToLower());
                IsDebuggerPresent = strAPI.ToLower().Contains("IsDebuggerPresent".ToLower());
                DisableThreadLibraryCalls = strAPI.ToLower().Contains("DisableThreadLibraryCalls".ToLower());
                CreateFileW = strAPI.ToLower().Contains("CreateFileW".ToLower());

                m_APIList.Add(Accept);
                m_APIList.Add(AdjustTokenPrivileges);
                m_APIList.Add(AttachThreadInput);
                m_APIList.Add(Bind);
                m_APIList.Add(BitBlt);
                m_APIList.Add(CertOpenSystemStore);
                m_APIList.Add(Connect);
                m_APIList.Add(ConnectNamedPipe);
                m_APIList.Add(ControlService);
                m_APIList.Add(CreateFile);
                m_APIList.Add(CreateFileMapping);
                m_APIList.Add(CreateMutex);
                m_APIList.Add(CreateProcess);
                m_APIList.Add(CreateRemoteThread);
                m_APIList.Add(CreateService);
                m_APIList.Add(CreateToolhelp32Snapshot);
                m_APIList.Add(CryptAcquireContext);
                m_APIList.Add(DeviceIoControl);
                m_APIList.Add(EnableExecuteProtectionSupport);
                m_APIList.Add(EnumProcesses);
                m_APIList.Add(EnumProcessModules);
                m_APIList.Add(FindFirstFile);
                m_APIList.Add(FindNextFile);
                m_APIList.Add(FindResource);
                m_APIList.Add(FindWindow);
                m_APIList.Add(FtpPutFile);
                m_APIList.Add(GetAdaptersInfo);
                m_APIList.Add(GetAsyncKeyState);
                m_APIList.Add(GetDC);
                m_APIList.Add(GetForegroundWindow);
                m_APIList.Add(Gethostbyname);
                m_APIList.Add(Gethostname);
                m_APIList.Add(GetKeyState);
                m_APIList.Add(GetModuleFileName);
                m_APIList.Add(GetModuleHandle);
                m_APIList.Add(GetProcAddress);
                m_APIList.Add(GetStartupInfo);
                m_APIList.Add(GetSystemDefaultLangId);
                m_APIList.Add(GetTempPath);
                m_APIList.Add(GetThreadContext);
                m_APIList.Add(GetVersionEx);
                m_APIList.Add(GetWindowsDirectory);
                m_APIList.Add(inet_addr);
                m_APIList.Add(InternetOpen);
                m_APIList.Add(InternetOpenUrl);
                m_APIList.Add(InternetReadFile);
                m_APIList.Add(InternetWriteFile);
                m_APIList.Add(CreateFileA);
                m_APIList.Add(CreateFileW);
                m_APIList.Add(GetLastError);
                m_APIList.Add(WriteFile);
                m_APIList.Add(Sleep);
                m_APIList.Add(CloseHandle);
                m_APIList.Add(CompareStringA);
                m_APIList.Add(CreateEventA);
                m_APIList.Add(CreateThread);
                m_APIList.Add(DeleteCriticalSection);
                m_APIList.Add(EnterCriticalSection);
                m_APIList.Add(EnumCalendarInfoA);
                m_APIList.Add(ExitProcess);
                m_APIList.Add(FindClose);
                m_APIList.Add(FindFirstFileA);
                m_APIList.Add(FindResourceA);
                m_APIList.Add(FormatMessageA);
                m_APIList.Add(FreeLibrary);
                m_APIList.Add(FreeResource);
                m_APIList.Add(GetACP);
                m_APIList.Add(GetCommandLineA);
                m_APIList.Add(GetCPInfo);
                m_APIList.Add(GetCurrentProcessId);
                m_APIList.Add(GetCurrentThreadId);
                m_APIList.Add(GetDateFormatA);
                m_APIList.Add(GetDiskFreeSpaceA);
                m_APIList.Add(GetFullPathNameA);
                m_APIList.Add(GetLocaleInfoA);
                m_APIList.Add(GetLocalTime);
                m_APIList.Add(GetModuleFileNameA);
                m_APIList.Add(GetModuleHandleA);
                m_APIList.Add(GetProfileStringA);
                m_APIList.Add(GetStartupInfoA);
                m_APIList.Add(GetStdHandle);
                m_APIList.Add(GetStringTypeExA);
                m_APIList.Add(GetSystemInfo);
                m_APIList.Add(GetThreadLocale);
                m_APIList.Add(GetTickCount);
                m_APIList.Add(GetVersion);
                m_APIList.Add(GetVersionExA);
                m_APIList.Add(GlobalAddAtomA);
                m_APIList.Add(GlobalAlloc);
                m_APIList.Add(GlobalDeleteAtom);
                m_APIList.Add(GlobalFindAtomA);
                m_APIList.Add(GlobalFree);
                m_APIList.Add(GlobalHandle);
                m_APIList.Add(GlobalLock);
                m_APIList.Add(GlobalReAlloc);
                m_APIList.Add(GlobalSize);
                m_APIList.Add(GlobalUnlock);
                m_APIList.Add(InitializeCriticalSection);
                m_APIList.Add(InterlockedDecrement);
                m_APIList.Add(InterlockedExchange);
                m_APIList.Add(InterlockedIncrement);
                m_APIList.Add(LeaveCriticalSection);
                m_APIList.Add(LoadLibraryA);
                m_APIList.Add(LoadLibraryExA);
                m_APIList.Add(LoadResource);
                m_APIList.Add(LocalAlloc);
                m_APIList.Add(LocalFree);
                m_APIList.Add(LockResource);
                m_APIList.Add(lstrcpyA);
                m_APIList.Add(lstrcpynA);
                m_APIList.Add(lstrlenA);
                m_APIList.Add(MulDiv);
                m_APIList.Add(MultiByteToWideChar);
                m_APIList.Add(RaiseException);
                m_APIList.Add(ReadFile);
                m_APIList.Add(ResetEvent);
                m_APIList.Add(RtlUnwind);
                m_APIList.Add(SetEndOfFile);
                m_APIList.Add(SetErrorMode);
                m_APIList.Add(SetEvent);
                m_APIList.Add(SetFilePointer);
                m_APIList.Add(SetThreadLocale);
                m_APIList.Add(SizeofResource);
                m_APIList.Add(SleepEx);
                m_APIList.Add(TlsGetValue);
                m_APIList.Add(TlsSetValue);
                m_APIList.Add(UnhandledExceptionFilter);
                m_APIList.Add(VirtualAlloc);
                m_APIList.Add(VirtualFree);
                m_APIList.Add(VirtualQuery);
                m_APIList.Add(WaitForSingleObject);
                m_APIList.Add(WideCharToMultiByte);
                m_APIList.Add(RegCloseKey);
                m_APIList.Add(RegQueryValueExA);
                m_APIList.Add(GetCurrentProcess);
                m_APIList.Add(TerminateProcess);
                m_APIList.Add(SetUnhandledExceptionFilter);
                m_APIList.Add(QueryPerformanceCounter);
                m_APIList.Add(IsDebuggerPresent);
                m_APIList.Add(DisableThreadLibraryCalls);

                foreach (var m_DosHeader in m_DosHeaderList)
                {
                    str += $"{m_DosHeader.ToString()},";
                }

                foreach (var m_FileHeader in m_FileHeaderList)
                {
                    str += $"{m_FileHeader.ToString()},";
                }

                foreach (var m_OptioanlHeader in m_OptionalHeaderList)
                {
                    str += $"{m_OptioanlHeader.ToString()},";
                }

                foreach (var m_SectionHeader in m_SectionHeaderList)
                {
                    str += $"{m_SectionHeader.ToString()},";
                }

                foreach (var m_ApkHeader in m_DexHeaderList)
                {
                    str += $"{m_ApkHeader.ToString()},";
                }

                str += $"{ShannonEntropy(File.ReadAllText(FilePath)).ToString()},";
                str += $"{GetEntropyValue(File.ReadAllBytes(FilePath)).ToString()},";

                foreach (bool m_API in m_APIList)
                {
                    if (m_API == true)
                        str += $"1,";
                    else
                        str += "0,";
                }

                var newPath = Path.GetFileNameWithoutExtension(FilePath).Replace("HEUR.", "");
                newPath.Replace("not-a-virus.", "").ToLower();

                if (isMalware)
                {
                    str += $"1";
                }
                else
                {
                    str += "0";
                }
            }

            return str;
        }

        public static string GetJSDataSet(string FilePath,int isMalware)
        {
            JS js = new JS();
            js.GetJsInfo(FilePath);
            string Item = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49},{50},{51}",
                BooleanToNumber(js.js_escape),
                BooleanToNumber(js.js_function),
                BooleanToNumber(js.js_return),
                BooleanToNumber(js.js_var),
                BooleanToNumber(js.js_for),
                BooleanToNumber(js.js_comment),
                BooleanToNumber(js.js_upperCase),
                BooleanToNumber(js.js_lowerCase),
                BooleanToNumber(js.js_while),
                BooleanToNumber(js.js_if),
                BooleanToNumber(js.js_continue),
                BooleanToNumber(js.js_catch),
                BooleanToNumber(js.js_try),
                BooleanToNumber(js.js_reverse),
                BooleanToNumber(js.js_join),
                BooleanToNumber(js.js_wscript),
                BooleanToNumber(js.js_echo),
                BooleanToNumber(js.js_quit),
                BooleanToNumber(js.js_case),
                BooleanToNumber(js.js_switch),
                BooleanToNumber(js.js_true),
                BooleanToNumber(js.js_false),
                BooleanToNumber(js.js_array),
                BooleanToNumber(js.js_new),
                BooleanToNumber(js.js_winHttpRequest),
                BooleanToNumber(js.js_fileSystemObject),
                BooleanToNumber(js.js_activeXObject),
                BooleanToNumber(js.js_responseBody),
                BooleanToNumber(js.js_Date),
                BooleanToNumber(js.js_underbar),
                BooleanToNumber(js.js_indexOf),
                BooleanToNumber(js.js_xmlHttp),
                BooleanToNumber(js.js_window),
                BooleanToNumber(js.js_location),
                BooleanToNumber(js.js_msdt),
                BooleanToNumber(js.js_fromCharCode),
                BooleanToNumber(js.js_CreateObject),
                BooleanToNumber(js.js_http),
                BooleanToNumber(js.js_eval),
                BooleanToNumber(js.js_push),
                BooleanToNumber(js.js_Offset),
                BooleanToNumber(js.js_Percent),
                BooleanToNumber(js.js_Obf),
                BooleanToNumber(js.js_char),
                BooleanToNumber(js.js_wave),
                BooleanToNumber(js.js_math),
                BooleanToNumber(js.js_click),
                BooleanToNumber(js.js_createObjectURL),
                BooleanToNumber(js.js_download),
                BooleanToNumber(js.js_base64),
                BooleanToNumber(js.js_write),
                isMalware);

            return Item;
        }

        public static int BooleanToNumber(bool value)
        {
            if (value)
                return 1;
            return 0;
        }

        private static string CalculateMd5Buffer(byte[] buffer)
        {
            string Result = "";

            if (buffer != null)
            {
                using (MD5 Md5 = MD5.Create())
                {
                    byte[] BufferData = Md5.ComputeHash(buffer);

                    StringBuilder Builder = new StringBuilder();

                    for (int i = 0; i < BufferData.Length; i++)
                    {
                        Builder.Append(BufferData[i].ToString("x2"));
                    }

                    Result = Builder.ToString();
                }
            }
            return Result;
        }

        private static void WriteToTextLog(string message)
        {
            var path = Environment.CurrentDirectory;

            var di = new DirectoryInfo(path + "\\Signatures\\" + DateTime.Now.ToString("yyyy-MMMM"));
            if (!di.Exists)
                di.Create();

            var LogFile = new FileInfo(path + "\\Signatures\\" + DateTime.Now.ToString("yyyy-MMMM") + "\\" + DateTime.Now.ToString("yyyy.MM.dd") + ".txt");
            var sw = LogFile.Exists ? LogFile.AppendText() : LogFile.CreateText();

            sw.WriteLine(message);
            sw.Flush();
            sw.Close();
        }
    }

 

    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
}

public static class Extensions
{
    public static T[] Slice<T>(this T[] source, int start, int end)
    {
        if (end < 0)
        {
            end = source.Length + end;
        }
        int len = end - start;

        T[] res = new T[len];
        for (int i = 0; i < len; i++)
        {
            res[i] = source[i + start];
        }
        return res;
    }

    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
        return source?.IndexOf(toCheck, comp) >= 0;
    }
}
