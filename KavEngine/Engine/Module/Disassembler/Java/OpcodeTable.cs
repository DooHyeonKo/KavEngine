using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.Engine.Module.Disassembler.Java
{
    class OpcodeTable
    {
        public string GetOpcode(uint hex)
        {
            switch (hex)
            {
                case 0x00: return "Nop";
                case 0x01: return "aconst null";
                case 0x02: return "iconst ml";
                case 0x03: return "iconst 0";
                case 0x04: return "iconst 1";
                case 0x05: return "iconst 2";
                case 0x06: return "iconst 3";
                case 0x07: return "iconst 4";
                case 0x08: return "iconst 5";
                case 0x09: return "lconst 0"; //
                case 0x0a: return "lconst 1";
                case 0x0b: return "fconst 0";
                case 0x0c: return "fconst 1";
                case 0x0d: return "lconst 2";
                case 0x0e: return "dconst 0";
                case 0x0f: return "dconst 1";
                case 0x10: return "bipush";
                case 0x11: return "sipush";
                case 0x12: return "Ide";
                case 0x13: return "Ide w ";
                case 0x14: return "Ide2 w";
                case 0x15: return "iload";
                case 0x16: return "lload";
                case 0x17: return "fload";
                case 0x18: return "dload";
                case 0x19: return "aload";
                case 0x1a: return "iload 0";
                case 0x1b: return "iload 1";
                case 0x1c: return "iload 2";
                case 0x1d: return "iload 2";
                case 0x1e: return "iload 1";
                case 0x1f: return "iload 2";
            }

            return null;
        }
    }
}
