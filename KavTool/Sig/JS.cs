using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KavTool.Sig
{
    class JS
    {
        public bool js_escape = false;
        public bool js_function = false;
        public bool js_return = false;
        public bool js_var = false;
        public bool js_for = false;
        public bool js_comment = false;
        public bool js_while = false;
        public bool js_if = false;
        public bool js_continue = false;
        public bool js_catch = false;
        public bool js_try = false;
        public bool js_reverse = false;
        public bool js_join = false;
        public bool js_wscript = false;
        public bool js_echo = false;
        public bool js_quit = false;
        public bool js_case = false;
        public bool js_switch = false;
        public bool js_true = false;
        public bool js_false = false;
        public bool js_array = false;
        public bool js_new = false;
        public bool js_winHttpRequest = false;
        public bool js_fileSystemObject = false;
        public bool js_activeXObject = false;
        public bool js_responseBody = false;
        public bool js_Date = false;
        public bool js_underbar = false;
        public bool js_indexOf = false;
        public bool js_xmlHttp = false;
        public bool js_window = false;
        public bool js_location = false;
        public bool js_msdt = false;
        public bool js_fromCharCode = false;
        public bool js_CreateObject = false;
        public bool js_http = false;
        public bool js_eval = false;
        public bool js_push = false;
        public bool js_Offset = false;
        public bool js_Percent = false;
        public bool js_Obf = false;
        public bool js_char = false;
        public bool js_wave = false;
        public bool js_math = false;
        public bool js_click = false;
        public bool js_createObjectURL = false;
        public bool js_download = false;
        public bool js_write = false;
        public bool js_base64 =false;
        public bool js_upperCase = false;
        public bool js_lowerCase = false;

        public void GetJsInfo(string FilePath)
        {
            try
            {
                var txt = "";

                StreamReader m_StreamReader = new StreamReader(FilePath);
                
                while (!m_StreamReader.EndOfStream)
                {
                    txt += m_StreamReader.ReadLine();
                }
                m_StreamReader.Close();

                GetJsInfoText(txt);
            }
            catch (Exception)
            {

            }
        }

      

       

        public void GetJsInfoText(string text)
        {
            js_escape = text.ToLower().Contains("escape");
            js_function = text.ToLower().Contains("function");
            js_return = text.ToLower().Contains("return");
            js_var = text.ToLower().Contains("var");
            js_for = text.ToLower().Contains("for");
            js_comment = text.ToLower().Contains("//");
            js_while = text.ToLower().Contains("while");
            js_if = text.ToLower().Contains("if");
            js_continue = text.ToLower().Contains("continue");
            js_catch = text.ToLower().Contains("catch");
            js_try = text.ToLower().Contains("try");
            js_reverse = text.ToLower().Contains("reverse");
            js_join = text.ToLower().Contains("join");
            js_wscript = text.ToLower().Contains("wscript");
            js_echo = text.ToLower().Contains("echo");
            js_quit = text.ToLower().Contains("quit");
            js_case = text.ToLower().Contains("case");
            js_switch = text.ToLower().Contains("switch");
            js_true = text.ToLower().Contains("true");
            js_false = text.ToLower().Contains("false");
            js_array = text.ToLower().Contains("array");
            js_new = text.ToLower().Contains("new");
            js_winHttpRequest = text.ToLower().Contains("winhttprequest");
            js_fileSystemObject = text.ToLower().Contains("filesystemobject");
            js_activeXObject = text.ToLower().Contains("activexobject");
            js_responseBody = text.ToLower().Contains("responsebody");
            js_Date = text.ToLower().Contains("date");
            js_underbar = text.ToLower().Contains("_");
            js_indexOf = text.ToLower().Contains("indexof");
            js_xmlHttp = text.ToLower().Contains("xmlhttp");
            js_window = text.ToLower().Contains("window");
            js_location = text.ToLower().Contains("location");
            js_msdt = text.ToLower().Contains("ms-msdt");
            js_fromCharCode = text.ToLower().Contains("fromcharcode");
            js_CreateObject = text.ToLower().Contains("createobject");
            js_http = text.ToLower().Contains("http");
            js_eval = text.ToLower().Contains("eval");
            js_push = text.ToLower().Contains("push");
            js_Offset = text.ToLower().Contains("0x");
            js_Percent = text.ToLower().Contains("%");
            js_Obf = text.ToLower().Contains("u00");
            js_char = text.ToLower().Contains("chr");
            js_wave = text.ToLower().Contains("~");
            js_math = text.ToLower().Contains("math");
            js_download = text.ToLower().Contains("download");
            js_click = text.ToLower().Contains("click");
            js_math = text.ToLower().Contains("math");
            js_write = text.ToLower().Contains("write");
            js_lowerCase = Regex.IsMatch(text, "[^a-z]");
            js_upperCase = Regex.IsMatch(text, "[^A-Z]");
            js_base64 = Regex.IsMatch(text, "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$");
        }
    }
}
