//using MalScore.Parser.PE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalScore.Disassembler
{
    class PE
    {
        public List<string> OpcodeList = new List<string>();
        public void Analyze(string FilePath)
        {
            int Size = (int)new FileInfo(FilePath).Length;

            FileStream Stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader Reader = new BinaryReader(Stream);

            byte[] buff = Reader.ReadBytes(Size);

            foreach (var code in buff)
                OpcodeList.Add(GetOpcodeName(code));
        }

        private string GetOpcodeName(uint code)
        {
            switch (code)
            {
                case 0x00: return "ADD";
                case 0x01: return "ADD ";
                case 0x02: return "ADD";
                case 0x03: return "ADD";
                case 0x04: return "ADD";
                case 0x05: return "ADD";
                case 0x06: return "PUSH";
                case 0x07: return "POP";
                case 0x10: return "ADC";
                case 0x11: return "ADC";
                case 0x12: return "ADC";
                case 0x13: return "ADC";
                case 0x14: return "ADC";
                case 0x15: return "ADC";
                case 0x16: return "PUSH";
                case 0x17: return "POP";
                case 0x20: return "AND";
                case 0x21: return "AND";
                case 0x22: return "AND";
                case 0x23: return "AND";
                case 0x24: return "AND";
                case 0x25: return "AND";
                case 0x26: return "ES:";
                case 0x27: return "DAA";
                case 0x30: return "XOR";
                case 0x31: return "XOR ";
                case 0x32: return "XOR";
                case 0x33: return "XOR";
                case 0x34: return "XOR";
                case 0x35: return "XOR";
                case 0x36: return "SS:";
                case 0x37: return "AAA";
                case 0x40: return "INC";
                case 0x41: return "INC";
                case 0x42: return "INC";
                case 0x43: return "INC";
                case 0x44: return "INC";
                case 0x45: return "INC";
                case 0x46: return "INC";
                case 0x47: return "INC";
                case 0x50: return "PUSH";
                case 0x51: return "PUSH";
                case 0x52: return "PUSH";
                case 0x53: return "PUSH";
                case 0x54: return "PUSH";
                case 0x55: return "PUSH";
                case 0x56: return "PUSH";
                case 0x57: return "PUSH";
                case 0x70: return "JO";
                case 0x71: return "JNO ";
                case 0x72: return "JB";
                case 0x73: return "JNB";
                case 0x74: return "JZ";
                case 0x75: return "JNZ";
                case 0x76: return "JBE";
                case 0x77: return "JA";
                case 0x80: return "GRP1";
                case 0x81: return "GRP1";
                case 0x82: return "GRP1";
                case 0x83: return "GRP1";
                case 0x84: return "TEST";
                case 0x85: return "TEST";
                case 0x86: return "XCHG";
                case 0x87: return "XCHG";
                case 0x90: return "NOP";
                case 0x91: return "XCHG";
                case 0x92: return "XCHG";
                case 0x93: return "XCHG";
                case 0x94: return "XCHG";
                case 0x95: return "XCHG";
                case 0x96: return "XCHG";
                case 0x97: return "XCHG";
                case 0xA0: return "MOV";
                case 0xA1: return "MOV";
                case 0xA2: return "MOV";
                case 0xA3: return "MOV";
                case 0xA4: return "MOVSB";
                case 0xA5: return "MOVSW";
                case 0xA6: return "CMPSB";
                case 0xA7: return "CMPSW";
                case 0xB0: return "MOV";
                case 0xB1: return "MOV";
                case 0xB2: return "MOV";
                case 0xB3: return "MOV";
                case 0xB4: return "MOV";
                case 0xB5: return "MOV";
                case 0xB6: return "MOV";
                case 0xB7: return "MOV";
                case 0xC2: return "RET";
                case 0xC3: return "RET";
                case 0xC4: return "LES";
                case 0xC5: return "LDS";
                case 0xC6: return "MOV";
                case 0xC7: return "MOV";
                case 0xE0: return "LOOPNZ";
                case 0xE1: return "LOOPZ";
                case 0xE2: return "LOOP";
                case 0xE3: return "JCXZ";
                case 0xE4: return "IN";
                case 0xE5: return "IN";
                case 0xE6: return "OUT";
                case 0xE7: return "OUT";
                case 0x08: return "OR";
                case 0x09: return "OR";
                case 0x0A: return "OR";
                case 0x0B: return "OR";
                case 0x0C: return "OR";
                case 0x0D: return "OR";
                case 0x0E: return "PUSH";
                case 0x18: return "SBB";
                case 0x19: return "SBB";
                case 0x1A: return "SBB";
                case 0x1B: return "SBB";
                case 0x1C: return "SBB";
                case 0x1D: return "SBB";
                case 0x1E: return "PUSH";
                case 0x1F: return "POP";
                case 0x28: return "SUB";
                case 0x29: return "SUB";
                case 0x2A: return "SUB ";
                case 0x2B: return "SUB";
                case 0x2C: return "SUB";
                case 0x2D: return "SUB";
                case 0x2E: return "CS:";
                case 0x2F: return "DAS";
                case 0x38: return "CMP";
                case 0x39: return "CMP";
                case 0x3A: return "CMP";
                case 0x3B: return "CMP";
                case 0x3C: return "CMP";
                case 0x3D: return "CMP";
                case 0x3E: return "DS:";
                case 0x3F: return "AAS";
                case 0x48: return "DEC";
                case 0x49: return "DEC";
                case 0x4A: return "DEC";
                case 0x4B: return "DEC";
                case 0x4C: return "DEC";
                case 0x4D: return "DEC";
                case 0x4E: return "DEC";
                case 0x4F: return "DEC";
                case 0x58: return "POP";
                case 0x59: return "POP";
                case 0x5A: return "POP";
                case 0x5B: return "POP";
                case 0x5C: return "POP";
                case 0x5D: return "POP";
                case 0x5E: return "POP";
                case 0x5F: return "POP";
                case 0x78: return "JS";
                case 0x79: return "JNS";
                case 0x7A: return "JPE";
                case 0x7B: return "JPO";
                case 0x7C: return "JL";
                case 0x7D: return "JGE";
                case 0x7E: return "JLE";
                case 0x7F: return "JG";
                case 0x88: return "MOV";
                case 0x89: return "MOV";
                case 0x8A: return "MOV";
                case 0x8B: return "MOV";
                case 0x8C: return "MOV";
                case 0x8D: return "LEA";
                case 0x8E: return "MOV";
                case 0x8F: return "POP";
                case 0x98: return "CBW";
                case 0x99: return "CWD";
                case 0x9A: return "CALL";
                case 0x9B: return "WAIT";
                case 0x9C: return "PUSHF";
                case 0x9D: return "POPF";
                case 0x9E: return "SAHF";
                case 0x9F: return "LAHF";
                case 0xA8: return "TEST";
                case 0xA9: return "TEST";
                case 0xAA: return "STOSB";
                case 0xAB: return "STOSW";
                case 0xAC: return "LODSB";
                case 0xAD: return "LODSW";
                case 0xAE: return "SCASB";
                case 0xAF: return "SCASW";
                case 0xB8: return "MOV";
                case 0xB9: return "MOV";
                case 0xBA: return "MOV";
                case 0xBB: return "MOV";
                case 0xBC: return "MOV";
                case 0xBD: return "MOV";
                case 0xBE: return "MOV";
                case 0xBF: return "MOV";
                case 0xE8: return "CALL";
                case 0xE9: return "JMP";
                case 0xEA: return "JMP";
                case 0xEB: return "JMP";
                case 0xEC: return "IN";
                case 0xED: return "IN";
                case 0xEE: return "OUT";
                case 0xEF: return "OUT";
                case 0xF8: return "CLC";
                case 0xF9: return "STC";
                case 0xFA: return "CLI";
                case 0xFB: return "CLD";
                case 0xFC: return "STD";
                case 0xFD: return "GRP4";
                case 0xFE: return "GRP5";
                case 0xFF: return "POP";
            }



            return null;
        }

        public string GetOperandName(UInt16 code)
        {
            switch (code)
            {
                case 0x0000: return "AX";
                case 0x0001: return "BX";
                case 0x0002: return "CX";
                case 0x0003: return "DX";
                case 0x0004: return "BP";
                case 0x0005: return "SP";
                case 0x0006: return "SI";
                case 0x0007: return "DI";
                case 0x0008: return "CS";
                case 0x0009: return "DS";
                case 0x000A: return "SS";
                case 0x000B: return "ES";
            }


            return null;
        }


    }

}

