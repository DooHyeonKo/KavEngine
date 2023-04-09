using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KavEngine.NeuralNetwork.Feature
{
    class Script
    {
        public int js_escape = 0;
        public int js_function = 0;
        public int js_return = 0;
        public int js_var = 0;
        public int js_for = 0;
        public int js_comment = 0;
        public int js_upperCase = 0;
        public int js_lowerCase = 0;
        public int js_while = 0;
        public int js_if = 0;
        public int js_continue = 0;
        public int js_catch = 0;
        public int js_try = 0;
        public int js_reverse = 0;
        public int js_join = 0;
        public int js_wscript = 0;
        public int js_echo = 0;
        public int js_quit = 0;
        public int js_case = 0;
        public int js_switch = 0;
        public int js_true = 0;
        public int js_false = 0;
        public int js_array = 0;
        public int js_new = 0;
        public int js_winHttpRequest = 0;
        public int js_fileSystemObject = 0;
        public int js_activeXObject = 0;
        public int js_responseBody = 0;
        public int js_Date = 0;
        public int js_underbar = 0;
        public int js_indexOf = 0;
        public int js_xmlHttp = 0;
        public int js_window = 0;
        public int js_location = 0;
        public int js_msdt = 0;
        public int js_fromCharCode = 0;
        public int js_CreateObject = 0;
        public int js_http = 0;
        public int js_eval = 0;
        public int js_push = 0;
        public int js_Offset = 0;
        public int js_Percent = 0;
        public int js_Obf = 0;
        public int js_char = 0;
        public int js_wave = 0;
        public int js_math = 0;
        public int js_click = 0;
        public int js_createObjectURL = 0;
        public int js_download = 0;
        public int js_base64 = 0;
        public int js_write = 0;

        public void GetJsInfo(string FilePath)
        {
            try
            {
                byte[] buffer = null;
                FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(FilePath).Length;

                buffer = br.ReadBytes((int)fs.Length);
                var enc = new ASCIIEncoding();
                var header = enc.GetString(buffer);

                GetJsInfoText(header);
            }
            catch (Exception)
            {

            }
        }

        public void GetJsInfoText(string text)
        {
            js_escape = Regex.Matches(text, "escape").Count;
            js_function = Regex.Matches(text, "function").Count;
            js_return = Regex.Matches(text, "return").Count;
            js_var = Regex.Matches(text, "var").Count;
            js_for = Regex.Matches(text, "for").Count;
            js_comment = Regex.Matches(text, "//").Count;
            js_lowerCase = Regex.Matches(text, "[^a-z]").Count;
            js_upperCase = Regex.Matches(text, "[^A-Z]").Count;
            js_while = Regex.Matches(text, "while").Count;
            js_if = Regex.Matches(text, "if").Count;
            js_continue = Regex.Matches(text, "continue").Count;
            js_catch = Regex.Matches(text, "catch").Count;
            js_try = Regex.Matches(text, "try").Count;
            js_reverse = Regex.Matches(text, "reverse").Count;
            js_join = Regex.Matches(text, "join").Count;
            js_wscript = Regex.Matches(text.ToLower(), "wscript").Count;
            js_echo = Regex.Matches(text.ToLower(), "echo").Count;
            js_quit = Regex.Matches(text.ToLower(), "quit").Count;
            js_case = Regex.Matches(text.ToLower(), "case").Count;
            js_switch = Regex.Matches(text.ToLower(), "switch").Count;
            js_true = Regex.Matches(text.ToLower(), "true").Count;
            js_false = Regex.Matches(text.ToLower(), "false").Count;
            js_array = Regex.Matches(text.ToLower(), "array").Count;
            js_new = Regex.Matches(text.ToLower(), "new").Count;
            js_winHttpRequest = Regex.Matches(text.ToLower(), "winhttprequest").Count;
            js_fileSystemObject = Regex.Matches(text.ToLower(), "filesystemobject").Count;
            js_activeXObject = Regex.Matches(text.ToLower(), "activexobject").Count;
            js_responseBody = Regex.Matches(text.ToLower(), "responsebody").Count;
            js_Date = Regex.Matches(text.ToLower(), "date").Count;
            js_underbar = Regex.Matches(text, "_").Count;
            js_indexOf = Regex.Matches(text.ToLower(), "indexof").Count;
            js_xmlHttp = Regex.Matches(text.ToLower(), "xmlhttp").Count;
            js_window = Regex.Matches(text.ToLower(), "window").Count;
            js_location = Regex.Matches(text.ToLower(), "location").Count;
            js_msdt = Regex.Matches(text.ToLower(), "ms-msdt").Count;
            js_fromCharCode = Regex.Matches(text.ToLower(), "fromcharcode").Count;
            js_CreateObject = Regex.Matches(text.ToLower(), "createobject").Count;
            js_http = Regex.Matches(text.ToLower(), "http").Count;
            js_eval = Regex.Matches(text.ToLower(), "eval").Count;
            js_push = Regex.Matches(text.ToLower(), "push").Count;
            js_Offset = Regex.Matches(text.ToLower(), "0x").Count;
            js_Percent = Regex.Matches(text, "%").Count;
            js_Obf = Regex.Matches(text.ToLower(), "u00").Count;
            js_char = Regex.Matches(text.ToLower(), "chr").Count;
            js_wave = Regex.Matches(text.ToLower(), "~").Count;
            js_math = Regex.Matches(text.ToLower(), "math").Count;
            js_download = Regex.Matches(text, "download").Count;
            js_click = Regex.Matches(text, "click").Count;
            js_math = Regex.Matches(text, "math").Count;
            js_base64 = Regex.Matches(text, "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$").Count;
            js_write = Regex.Matches(text, "write").Count;
        }

        public float[] GetFeatureMatrix(string FilePath)
        {
            GetJsInfo(FilePath);

            float[] Matrix = new float[]
            {
                (float)js_escape,
                (float)js_function,
                (float)js_return,
                (float)js_var,
                (float)js_for,
                (float)js_comment,
                (float)js_upperCase,
                (float)js_lowerCase,
                (float)js_while,
                (float)js_if,
                (float)js_continue,
                (float)js_catch,
                (float)js_try,
                (float)js_reverse,
                (float)js_join,
                (float)js_wscript,
                (float)js_echo,
                (float)js_quit,
                (float)js_case,
                (float)js_switch,
                (float)js_true,
                (float)js_false,
                (float)js_array,
                (float)js_new,
                (float)js_winHttpRequest,
                (float)js_fileSystemObject,
                (float)js_activeXObject,
                (float)js_responseBody,
                (float)js_Date,
                (float)js_underbar,
                (float)js_indexOf,
                (float)js_xmlHttp,
                (float)js_window,
                (float)js_location,
                (float)js_msdt,
                (float)js_fromCharCode,
                (float)js_CreateObject,
                (float)js_http,
                (float)js_eval,
                (float)js_push,
                (float)js_Offset,
                (float)js_Percent,
                (float)js_Obf,
                (float)js_char,
                (float)js_wave,
                (float)js_math,
                (float)js_click,
                (float)js_createObjectURL,
                (float)js_download,
                (float)js_base64,
                (float)js_write
            };

            return Matrix;
        }
    }
}
