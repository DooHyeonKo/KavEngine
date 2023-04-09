/*
 * SDK Library
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using KavEngine.Engine;

namespace KavEngine.SDK
{
    public class KavSDK
    {


        private KavEngine.Engine.KavEngine m_Engine = new KavEngine.Engine.KavEngine();
        private KavExclude m_Exclude = new KavExclude();

        private Dictionary<string, string> dicEngine;

        public bool UseSignatureScan = false;
        public bool UseHeuristicScan = false;
        public bool UseCloudScan = false;

        public int VirusNumber = KavConfig.VirusNumber;

        public Dictionary<string,string> GetEngineInfo()
        {
            dicEngine = new Dictionary<string, string>();

            dicEngine.Add("PE Scan Engine", "v1.0");
            dicEngine.Add("Heuristic Scan Engine", "v1.0");
            dicEngine.Add("HTML Scan Engine", "v1.0");
            dicEngine.Add("LNK Scan Engine", "v1.0");
            dicEngine.Add("JavaScript Scan Engine", "v1.0");
            dicEngine.Add("RTF Scan Engine", "v1.0");
            dicEngine.Add("Adware Scan Engine", "v1.0");
            dicEngine.Add("Signature Scan Engine", "v1.0");
            dicEngine.Add("Cloud Scan Engine", "v1.0");
            dicEngine.Add("Hash Scan Engine", "v1.0");
            dicEngine.Add("Core Scan Engine", "v1.0");
            dicEngine.Add("Packer Scan Engine", "v1.0");
            dicEngine.Add("Trojan Scan Engine", "v1.0");
            dicEngine.Add("DEX Scan Engine", "v1.0");
            dicEngine.Add("Zip Scan Engine", "v1.0");

            return dicEngine;
        }

        public int GetEngineNumber()
        {
            return dicEngine.Values.Count;
        }

        public string GetQuarantineInfo()
        {
            KavQuarantine m_File = new KavQuarantine();

            if (m_File.ListQuarantineFile == null)
            {
                m_File.LoadQuarantineFile();

                foreach (string sFile in m_File.ListQuarantineFile)
                {
                    return sFile;
                }
            }
            else
            {
                return "Not found malwares";
            }

            return null;
        }

        public void SaveQuarantineInfo()
        {
            m_Engine.SaveQuarantineList();
        }

        public string GetExcludeInfo()
        {
            KavExclude m_File = new KavExclude();

            if (m_File.ListExcludeFile == null)
            {
                m_File.LoadExcludeFile();

                foreach (string sFile in m_File.ListExcludeFile)
                {
                    return sFile;
                }
            }
            else
            {
                return "Not found exculude info";
            }

            return null;
        }

        public void AddExcludeInfo(string FilePath)
        {
            m_Exclude.AddExcludeFile(FilePath);
        }

        public void RemoveExcludeInfo(string FilePath)
        {
            m_Exclude.RemoveExcludeFile(FilePath);
        }

        private string GetUpdate()
        {
            try
            {
                //m_Engine.StartUpdate();

                return "Successfuly";
            }
            catch (Exception)
            {
                return "Failed";
            }
        }

        public void InitializeScan()
        {
            m_Engine.IsCloudScan = UseCloudScan;
            m_Engine.IsSignatureScan = UseSignatureScan;
            m_Engine.IsHeuristicScan = UseHeuristicScan;
            m_Engine.LoadVDB();
        }

        public void UnLoadVDB()
        {
            m_Engine.UnLoadVDB();
        }

        public string ScanFile(string FilePath)
        {
            KavKernel.ScanResult mScanResult = m_Engine.ScanFile(FilePath);

            if (mScanResult == KavKernel.ScanResult.Infected)
            {
                return m_Engine.VirusName;
            }
            else
            {
                return "NotFound";
            }
        }
    }
}
