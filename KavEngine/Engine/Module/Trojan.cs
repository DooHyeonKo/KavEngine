using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Features;

namespace KavEngine.Engine.Module
{
    class Trojan
    {
        public string VirusName = "";
        public string FileLocation = "";
        public string EngineName = "Trojan Scan Engine";

        public KavKernel.ScanResult ScanFile(byte[] buffer, string FilePath)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;

            try
            {
                if (buffer[0] == 0x4d && buffer[1] == 0x5a)
                {
                    PeFile peFile = new PeFile(buffer);

                    int c = 0; // Threat Level

                    try
                    {
                        if (peFile.ImportedFunctions != null)
                        {
                            foreach (ImportFunction importFunction in peFile.ImportedFunctions)
                            {
                                

                                if (importFunction.Name != null)
                                {
                                    if (importFunction.Name.Contains("Accept"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("AdjustTokenPrivileges"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("AttachThreadInput"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("Bind"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("BitBlt"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CertOpenSystemStore"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("Connect"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("ConnectNamedPipe"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("ControlService"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CreateFile"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CreateFileMapping"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CreateMutex"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CreateProcess"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CreateRemoteThread"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CreateService"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CreateToolhelp32Snapshot"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CryptAcquireContext"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("DeviceIoControl"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("EnableExecuteProtectionSupport"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("EnumProcesses"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("EnumProcessModules"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("FindFirstFile"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("FindNextFile"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("FindResource"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("FindWindow"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("FtpPutFile"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetAdaptersInfo"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetAsyncKeyState"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetDC"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetForegroundWindow"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("Gethostbyname"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("Gethostname"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetKeyState"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetModuleFilename"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetModuleHandle"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetProcAddress"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetStartupInfo"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetSystemDefaultLangId"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetTempPath"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetThreadContext"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetVersionEx"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetWindowsDirectory"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("inet_addr"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("InternetOpen"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("InternetOpenUrl"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("InternetReadFile"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("InternetWriteFile"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("IsNTAdmin"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("IsWoW64Process"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("LdrLoadDll"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("LoadResource"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("LsaEnumerateLogonSessions"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("MapViewOfFile"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("MapVirtualKey"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("Module32First"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("Module32Next"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("NetScheduleJobAdd"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("NetShareEnum"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("NtQueryDirectoryFile"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("NtQueryInformationProcess"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("NtSetInformationProcess"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("OpenMutex"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("OpenProcess"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("OutputDebugString"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("PeekNamedPipe"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("Process32First"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("Process32Next"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("QueueUserAPC"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("ReadProcessMemory"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("Recv"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("RegisterHotKey"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("RegOpenKey"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("ResumeThread"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("RtlCreateRegistryKey"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("RtlWriteRegistryValue"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("SamIConnect"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("SamIGetPrivateData"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("SamQueryInformationUse"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("Send"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("SetFileTime"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("SetThreadContext"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("SetWindowsHookEx"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("SfcTerminateWatcherThread"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("ShellExecute"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("StartServiceCtrlDispatcher"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("SuspendThread"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("Thread32First"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("Thread32Next"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("Toolhelp32ReadProcessMemory"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("URLDownloadToFile"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("VirtualAllocEx"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("VirtualProtectEx"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("WideCharToMultiByte"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("WinExec"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("WriteProcessMemory"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("WSAStartup"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("RegSetValueEx"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("RegCreateKeyEx"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("RegEnumKeyEx"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("RegQueryValueEx"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("RegOpenKeyEx"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("RegCloseKey"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("RegEnumValue"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetCurrentProcess"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetCurrentProessId"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CreateThread"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("SetThreadPriority"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetProcessTimes"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("ExitProcess"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetFileVersionInfo"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetFileVersionInfoSize"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetSystemMetrics"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetSystemInfo"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetNativeSystemInfo"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("IsDebuggerPresent"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("QueryPerformanceCounter"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("DeleteFile"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetFileType"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("MoveFile"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetFileAttributes"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CopyFile"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("FindFirstFileEx"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("GetFileSize"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("ReadFile"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("EnableWindow"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CryptDeriveKey"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CryptEncrypt"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CryptDecrypt"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CryptCreateHash"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("CryptHashData"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("HttpQueryInfo"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("HttpSendRequestEx"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("HttpEndRequest"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("HttpOpenRequest"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("InternetConnectA"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("InternetGetConnectedState"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("InternetSetOptionA"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("InternetWriteFile"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("InternetCrackUrlA"))
                                    {
                                        c += 5;
                                    }
                                    else if (importFunction.Name.Contains("InternetSetStatusCallbackA"))
                                    {
                                        c += 5;
                                    }
                                }
                            }
                        }
                    }
                    catch (NullReferenceException)
                    {

                    }

                    uint SizeOfRawData = 0;

                    for (int i = 0; i < peFile.ImageSectionHeaders.Length; i++)
                    {
                        uint PointerToRawData = peFile.ImageSectionHeaders[i].PointerToRawData;
                        SizeOfRawData = peFile.ImageSectionHeaders[i].SizeOfRawData;
                        uint VirtualAddress = peFile.ImageSectionHeaders[i].VirtualAddress;
                        uint VirtualSize = peFile.ImageSectionHeaders[i].VirtualSize;

                        byte[] SectionBuffer = peFile.Buff.Slice((int)PointerToRawData, (int)PointerToRawData + (int)SizeOfRawData);

                        if (peFile.ImageNtHeaders.FileHeader.NumberOfSections < 3)
                        {
                            c += 5;

                            int entropy = (int)GetEntropyValue(SectionBuffer);

                            if (entropy == 1 || entropy > 6)
                            {
                                c += 15;
                            }
                        }
                    }

                    if (!peFile.IsSigned)
                    {
                        c += 5;
                    }

                    FileVersionInfo VersionInfo = FileVersionInfo.GetVersionInfo(FilePath);

                    if (VersionInfo.FileVersion == null)
                    {
                        c += 1;
                    }
                    else if (VersionInfo.ProductVersion == null)
                    {
                        c += 1;
                    }
                    else if (VersionInfo.LegalCopyright == null)
                    {
                        c += 1;
                    }
                    else if (VersionInfo.LegalTrademarks == null)
                    {
                        c += 1;
                    }
                    else if (VersionInfo.SpecialBuild == null)
                    {
                        c += 1;
                    }
                    else if (VersionInfo.Comments == null)
                    {
                        c += 1;
                    }
                    else if (VersionInfo.CompanyName == null)
                    {
                        c += 1;
                    }
                    else if (VersionInfo.InternalName == null)
                    {
                        c += 1;
                    }
                    else if (VersionInfo.Language == null)
                    {
                        c += 1;
                    }
                    else if (VersionInfo.OriginalFilename == null)
                    {
                        c += 1;
                    }
                    else if (VersionInfo.FileDescription == null)
                    {
                        c += 1;
                    }
                    else if (VersionInfo.PrivateBuild == null)
                    {
                        c += 1;
                    }

                    if (c > 200)
                    {
                        VirusName = $"Heuristic.Generic.{peFile.MD5.Substring(0, 10).ToUpper()}";

                        FileLocation = FilePath;
                        mScanResult = KavKernel.ScanResult.Infected;
                    }
                }
            }
            catch (NullReferenceException)
            {

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
