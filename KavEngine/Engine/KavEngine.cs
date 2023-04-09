/*
 * Scan Engine Library
 * 
 * Copyright (C) 2022 DuHyeon-Ko
 * 
 * KOR
 * 
 * 이 프로젝트는 바이러스 검사 코어 엔진 라이브러리입니다. 
 * 그리고 이 프로젝트는 오픈 소스 프로젝트이고 다른 사이트
 * 에 코드를 변경해 배포하면 안됨니다. 
 * 
 * ENG
 * 
 * This project is a virus scan core engine library.
 * And this project is an open source project
 * You must not change the code and distribute it.
 * 
 * Created by KamSecurity
*/

using Features;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO.Compression;
using System.Diagnostics;
using System.Net.Http;
using KavEngine.Engine.Module;

namespace KavEngine.Engine
{
    public enum HeuristicLevel
    {
        High,
        Normal,
        Low,
        None
    }

    class KavEngine
    {

        // HTML Pattern



        // LNK Pattern

        PE PEEngine = new PE();
        Adware AdwareEngine = new Adware();
        Dummy DummyEngine = new Dummy();
        Hash HashEngine = new Hash();
        HTML HTMLEngine = new HTML();
        LNK LNKEngine = new LNK();
        Script ScriptEngine = new Script();
        Signature SignatureEngine = new Signature();
        Zip ZipEngine = new Zip();
        Cloud CloudEngine = new Cloud();
        DEX DexEngine = new DEX();
        PDF PdfEngine = new PDF();
        Packer PackerEngine = new Packer();
        Trojan TrojanEngine = new Trojan();
        Daily DailyEngine = new Daily();

        //PEClassifier PEClassifier = new PEClassifier();

        //Heuristic HeuristicEngine = new Heuristic();

        private List<string> WhiteList = new List<string>();


        KavSignature cSignFile = new KavSignature();

        KavExclude cExcludeFile = new KavExclude();

        KavQuarantine cQuarantineFile = new KavQuarantine();

        public HeuristicLevel HeurLevel = HeuristicLevel.None;

        public string VirusName = "";

        public string FileLocation = "";

        public bool IsHeuristicScan = false;

        public bool IsCloudScan = false;

        public bool IsAIScan = false;

        public bool IsSignatureScan = false;

        public void LoadVDB()
        {

            cSignFile.LoadVDB(WhiteList, KavConfig.sWhiteListVdbPath);
            PEEngine.LoadVDB();
            SignatureEngine.LoadVDB();
            ScriptEngine.LoadVDB();
            AdwareEngine.LoadVDB();
            HashEngine.LoadVDB();
            PackerEngine.LoadVDB();
            DailyEngine.LoadVDB();
            //HeuristicEngine.LoadVDB();
            cExcludeFile.LoadExcludeFile();
            cQuarantineFile.LoadQuarantineFile();
        }

        public void UnLoadVDB()
        {
            //cSignFile.LoadVDB(WhiteList, KavConfig.sWhiteListVdbPath);
            //PEEngine.LoadVDB();
            //SignatureEngine.LoadVDB();
            //AdwareEngine.LoadVDB();
            //HashEngine.LoadVDB();
            //ScriptEngine.LoadVDB();
        }

        public void ReloadVDB()
        {
            // Reset Virus Database


            PEEngine.VirusList.Clear();
            
            HashEngine.HashList.Clear();
            AdwareEngine.AdwareList.Clear();
            SignatureEngine.SignList.Clear();
            WhiteList.Clear();
            HashEngine.Md5List.Clear();
            // Load Virus Database

            cSignFile.LoadVDB(WhiteList, KavConfig.sWhiteListVdbPath);
            PEEngine.LoadVDB();
            SignatureEngine.LoadVDB();
            AdwareEngine.LoadVDB();
            HashEngine.LoadVDB();
        }


        //public byte[] ReadFully(Stream stream, int initialLength)
        //{
        //    // If we've been passed an unhelpful initial length, just
        //    // use 32K.

        //    byte[] buffer = new byte[initialLength];
        //    long read = 0;

        //    int chunk;
        //    while ((chunk = stream.Read(buffer, (int)read, buffer.Length - (int)read)) > 0)
        //    {
        //        read += chunk;

        //        // If we've reached the end of our buffer, check to see if there's
        //        // any more information
        //        if (read == buffer.Length)
        //        {
        //            int nextByte = stream.ReadByte();

        //            // End of stream? If so, we're done
        //            if (nextByte == -1)
        //            {
        //                return buffer;
        //            }

        //            // Nope. Resize the buffer, put in the byte we've just
        //            // read, and continue
        //            byte[] newBuffer = new byte[buffer.Length * 2];
        //            Array.Copy(buffer, newBuffer, buffer.Length);
        //            newBuffer[read] = (byte)nextByte;
        //            buffer = newBuffer;
        //            read++;
        //        }
        //    }
        //    // Buffer is now too big. Shrink it.
        //    byte[] ret = new byte[read];
        //    Array.Copy(buffer, ret, read);
        //    return ret;
        //}

        public KavKernel.ScanResult ScanFile(string FilePath)
        {
            KavKernel.ScanResult mScanResult = KavKernel.ScanResult.NotInfected;
            var m_FileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            byte[] buffer = new byte[m_FileStream.Length];
            m_FileStream.Read(buffer, 0, buffer.Length);


            var Extensions = Path.GetExtension(FilePath);
            int FileSize = (int)new FileInfo(FilePath).Length;


            try
            {
                if (File.Exists(FilePath))
                {
                    if (!WhiteList.Contains(KavHash.CalculateMd5Buffer(buffer).ToUpper()))
                    {
                        PEEngine.UseHeur = IsHeuristicScan;
                        if (!cExcludeFile.ListExcludeFile.Contains(FilePath))
                        {

                        }
                        else
                        {
                            mScanResult = KavKernel.ScanResult.NotFoundPath;

                        }

                        if (ScriptEngine.ScanFile(buffer, Extensions, FilePath) == KavKernel.ScanResult.Infected)
                        {
                            VirusName = ScriptEngine.VirusName;
                            FileLocation = ScriptEngine.FileLocation;
                            mScanResult = KavKernel.ScanResult.Infected;
                        }

                        if (PeFile.IsPEFile(FilePath))
                        {
                            if (PEEngine.ScanFile(buffer, FilePath) == KavKernel.ScanResult.Infected)
                            {
                                VirusName = PEEngine.VirusName;
                                FileLocation = PEEngine.FileLocation;
                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                            else if (AdwareEngine.ScanFile(FilePath) == KavKernel.ScanResult.Infected)
                            {
                                VirusName = AdwareEngine.VirusName;
                                FileLocation = AdwareEngine.FileLocation;
                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                            else if (PackerEngine.ScanFile(buffer,FilePath) == KavKernel.ScanResult.Infected)
                            {
                                VirusName = PackerEngine.VirusName;
                                FileLocation = PackerEngine.FileLocation;
                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                            else if (TrojanEngine.ScanFile(buffer, FilePath) == KavKernel.ScanResult.Infected)
                            {
                                VirusName = TrojanEngine.VirusName;
                                FileLocation = TrojanEngine.FileLocation;
                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                            else if (IsSignatureScan)
                            {
                                if (SignatureEngine.ScanFile(buffer, FilePath) == KavKernel.ScanResult.Infected)
                                {
                                    VirusName = SignatureEngine.VirusName;
                                    FileLocation = SignatureEngine.FileLocation;
                                    mScanResult = KavKernel.ScanResult.Infected;
                                }
                            }

                        }


                        if (DummyEngine.ScanFile(buffer, FilePath, FileSize) == KavKernel.ScanResult.Infected)
                        {
                            VirusName = DummyEngine.VirusName;
                            FileLocation = DummyEngine.FileLocation;
                            mScanResult = KavKernel.ScanResult.Infected;
                        }


                        if (HashEngine.ScanFile(buffer, FilePath, FileSize) == KavKernel.ScanResult.Infected)
                        {
                            VirusName = HashEngine.VirusName;
                            FileLocation = HashEngine.FileLocation;
                            mScanResult = KavKernel.ScanResult.Infected;
                        }

                        if (HTMLEngine.ScanFile(buffer, Extensions, FilePath) == KavKernel.ScanResult.Infected)
                        {
                            VirusName = HTMLEngine.VirusName;
                            FileLocation = HTMLEngine.FileLocation;
                            mScanResult = KavKernel.ScanResult.Infected;
                        }

                        if (LNKEngine.ScanFile(buffer, FilePath,Extensions) == KavKernel.ScanResult.Infected)
                        {
                            VirusName = LNKEngine.VirusName;
                            FileLocation = LNKEngine.FileLocation;
                            mScanResult = KavKernel.ScanResult.Infected;
                        }

                        if (DexEngine.ScanFile(buffer, FilePath) == KavKernel.ScanResult.Infected)
                        {
                            VirusName = DexEngine.VirusName;
                            FileLocation = DexEngine.FileLocation;
                            mScanResult = KavKernel.ScanResult.Infected;
                        }

                        if (PdfEngine.ScanFile(buffer, FilePath, Extensions) == KavKernel.ScanResult.Infected)
                        {
                            VirusName = PdfEngine.VirusName;
                            FileLocation = PdfEngine.FileLocation;
                            mScanResult = KavKernel.ScanResult.Infected;
                        }

                        if (DailyEngine.ScanFile(buffer, FilePath) == KavKernel.ScanResult.Infected)
                        {
                            VirusName = DailyEngine.VirusName;
                            FileLocation = DailyEngine.FileLocation;
                            mScanResult = KavKernel.ScanResult.Infected;
                        }

                        //if (HeuristicEngine.ScanFile(FilePath) == KavKernel.ScanResult.Infected)
                        //{
                        //    VirusName = HeuristicEngine.VirusName;
                        //    FileLocation = HeuristicEngine.FileLocation;
                        //    mScanResult = KavKernel.ScanResult.Infected;
                        //}


                        if (IsCloudScan)
                        {
                            if (CloudEngine.ScanFile(buffer, FilePath) == KavKernel.ScanResult.Infected)
                            {
                                VirusName = CloudEngine.VirusName;
                                FileLocation = CloudEngine.FileLocation;
                                mScanResult = KavKernel.ScanResult.Infected;
                            }
                        }


                    }
                    else
                    {
                        mScanResult = KavKernel.ScanResult.NotInfected;
                    }

                }
            }
            catch (Exception)
            {

            }

            return mScanResult;
        }

        public void SaveQuarantineList()
        {
            cQuarantineFile.SaveQuarantineSettings();
        }

        public KavKernel.CleanResult CureFile(string FilePath)
        {
            try
            {
                File.Delete(FilePath);
                return KavKernel.CleanResult.Success;
            }
            catch (Exception)
            {
                return KavKernel.CleanResult.Failed;
            }
        }

        private KavKernel.UpdateResult StartUpdate()
        {
            try
            {
                WebClient Client = new WebClient();

                string SigDownloadPath = Environment.CurrentDirectory;

                Client.DownloadFile("http://192.168.0.28/db/db_2.txt", SigDownloadPath + "\\Signature\\main.vdb");
                Client.DownloadFile("http://192.168.0.28/db/db_0.txt", SigDownloadPath + "\\Signature\\adware.vdb");
                Client.DownloadFile("http://192.168.0.28/db/db_3.txt", SigDownloadPath + "\\Signature\\signature.vdb");
                Client.DownloadFile("http://192.168.0.28/db/db_5.txt", SigDownloadPath + "\\Signature\\ml.zip");
                Client.DownloadFile("http://192.168.0.28/db/db_1.txt", SigDownloadPath + "\\Signature\\daily.vdb");
                Client.DownloadFile("http://192.168.0.28/db/db_4.txt", SigDownloadPath + "\\Signature\\whitelist.vdb");

                return KavKernel.UpdateResult.Success;
            }
            catch (Exception)
            {
                return KavKernel.UpdateResult.Failed;
            }
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
    }
