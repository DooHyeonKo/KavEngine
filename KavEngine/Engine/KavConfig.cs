/*
 * Config Library
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
using System.Text;

namespace KavEngine.Engine
{
    public class KavConfig
    {
        // Path

        public static string directoryName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

        // General

        public static string sExcludeFilePath = directoryName + "\\Data\\Exclude.db";

        public static string sQuarantineFilePath = directoryName + "\\Data\\Quarantine.db";

        // Time

        public static string UpdateTime = "";

        public static string LastScanTime = "";

        // Virus Database

        public static string sDailyVdbPath = directoryName + "\\Signature\\db_daily.ptn";

        public static string sPeVdbPath = directoryName + "\\Signature\\db_pe.ptn";

        public static string sAdwareVdbPath = directoryName + "\\Signature\\db_cert.ptn";

        public static string sSignatureVdbPath = directoryName + "\\Signature\\db_sig.ptn";

        public static string sWhiteListVdbPath = directoryName + "\\Signature\\db_white.ptn";

        public static string sSHA256VdbPath = directoryName + "\\Signature\\db_sha256.ptn";

        //public static string sMLModelPath = directoryName + "\\Signature\\db_ml.model";

        public static string sMainVirusDB = directoryName + "\\Signature\\db_md5.ptn";

        public static string sPeMLModelPath = directoryName + "\\Model\\MLModel.model";

        public static string sHeurDB = directoryName + "\\Signature\\db_heur.ptn";

        public static string sJavaScriptDB = directoryName + "\\Signature\\db_js.ptn";

        public static string sYaraRuleDB = directoryName + "\\Signature\\db_yara.ptn";

        public static string sPackerDB = directoryName + "\\Signature\\db_packer.ptn";

        // Config Path

        public static string LastScanTimePath = directoryName + "\\Config\\last_scan.cfg";

        public static string UpdateTimePath = directoryName + "\\Config\\update.cfg";

        public static string EngineConfig = directoryName + "\\Config\\engine.cfg";

        // Virus Info

        public static int VirusNumber = 0;
        public static List<string> VirusNameList = new List<string>();

        // Cloud Scan URL

        public static string sCloudScanURL = "http://222.118.93.106/cloud/pescan.php?hash=";

        // KMD Network URL

        public static string sKmdNetworkURL_AI = "http://192.168.18.1/v2/api/scan/ai";
        public static string sKmdNetworkURL_AV = "http://192.168.18.1/v2/api/scan/av";

    }
}
