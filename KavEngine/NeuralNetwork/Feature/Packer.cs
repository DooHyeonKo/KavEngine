using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.NeuralNetwork.Feature
{
    class Packer
    {
        public Dictionary<string, string> dicPackersName = new Dictionary<string, string>();

        public bool IsPacker = false;
        public string PackerName = null;
        public int PackerNumber = 0;

        public void AddPacker()
        {
            dicPackersName.Add(".aspack", "Aspack packer");
            dicPackersName.Add(".adata", "Aspack packer/Armadillo packer");
            dicPackersName.Add("ASPack", "Aspack packer");
            dicPackersName.Add(".ASPack", "ASPAck Protector");
            dicPackersName.Add(".boom", "The Boomerang List Builder (config+exe xored with a single byte key 0x77)");
            dicPackersName.Add(".ccg", "CCG Packer (Chinese Packer)");
            dicPackersName.Add(".charmve", "Added by the PIN tool");
            dicPackersName.Add("BitArts", "Crunch 2.0 Packer");
            dicPackersName.Add("DAStub", "DAStub Dragon Armor protector");
            dicPackersName.Add("!EPack", "Epack packer");
            dicPackersName.Add("FSG!", "FSG packer (not a section name, but a good identifier)");
            dicPackersName.Add(".gentee", "Gentee installer");
            dicPackersName.Add("kkrunchy", "kkrunchy Packer");
            dicPackersName.Add(".mackt", "ImpRec-created section");
            dicPackersName.Add(".MaskPE", "MaskPE Packer");
            dicPackersName.Add("MEW", "MEW packer");
            dicPackersName.Add(".MPRESS1", "Mpress Packer");
            dicPackersName.Add(".MPRESS2", "Mpress Packer");
            dicPackersName.Add(".neolite", "Neolite Packer");
            dicPackersName.Add(".neolit", "Neolite Packer");
            dicPackersName.Add(".nsp1", "NsPack packer");
            dicPackersName.Add(".nsp0", "NsPack packer");
            dicPackersName.Add(".nsp2", "NsPack packer");
            dicPackersName.Add("nsp1", "NsPack packer");
            dicPackersName.Add("nsp0", "NsPack packer");
            dicPackersName.Add("nsp2", "NsPack packer");
            dicPackersName.Add(".packed", "RLPack Packer (first section)");
            dicPackersName.Add("pebundle", "PEBundle Packer");
            dicPackersName.Add("PEBundle", "PEBundle Packer");
            dicPackersName.Add("PEC2TO", "PECompact packer");
            dicPackersName.Add("PECompact2", "PECompact packer (not a section name, but a good identifier)");
            dicPackersName.Add("PEC2", "PECompact packer");
            dicPackersName.Add("pec1", "PECompact packer");
            dicPackersName.Add("pec2", "PECompact packer");
            dicPackersName.Add("PEC2MO", "PECompact packer");
            dicPackersName.Add("PELOCKnt", "PELock Protector");
            dicPackersName.Add(".perplex", "Perplex PE-Protector");
            dicPackersName.Add("PESHiELD", "PEShield Packer");
            dicPackersName.Add(".petite", "Petite Packer");
            dicPackersName.Add(".pinclie", "Added by the PIN tool");
            dicPackersName.Add("ProCrypt", "ProCrypt Packer");
            dicPackersName.Add(".RLPack", "RLPack Packer (second section)");
            dicPackersName.Add(".rmnet", "Ramnit virus marker");
            dicPackersName.Add("RCryptor", "RPCrypt Packer");
            dicPackersName.Add(".RPCrypt", "RPCrypt Packer");
            dicPackersName.Add(".seau", "SeauSFX Packer");
            dicPackersName.Add(".sforce3", "StarForce Protection");
            dicPackersName.Add(".spack", "Simple Pack (by bagie)");
            dicPackersName.Add(".svkp", "SVKP packer");
            dicPackersName.Add("Themida", "Themida Packer");
            dicPackersName.Add(".Themida", "Themida Packer");
            dicPackersName.Add(".taz", "Some version os PESpin");
            dicPackersName.Add(".tsuarch", "TSULoader");
            dicPackersName.Add(".tsustub", "TSULoader");
            dicPackersName.Add(".packed", "Unknown Packer");
            dicPackersName.Add("PEPACK!!", "Pepack");
            dicPackersName.Add(".Upack", "Upack packer");
            dicPackersName.Add(".ByDwing", "Upack Packer");
            dicPackersName.Add("UPX0", "UPX packer");
            dicPackersName.Add("UPX1", "UPX packer");
            dicPackersName.Add("UPX2", "UPX packer");
            dicPackersName.Add("UPX!", "UPX packer");
            dicPackersName.Add(".UPX0", "UPX Packer");
            dicPackersName.Add(".UPX1", "UPX Packer");
            dicPackersName.Add(".UPX2", "UPX Packer");
            dicPackersName.Add(".vmp0", "VMProtect packer");
            dicPackersName.Add(".vmp1", "VMProtect packer");
            dicPackersName.Add(".vmp2", "VMProtect packer");
            dicPackersName.Add("VProtect", "Vprotect Packer");
            dicPackersName.Add(".winapi", "Added by API Override tool");
            dicPackersName.Add("WinLicen", "WinLicense (Themida) Protector");
            dicPackersName.Add("_winzip_", "WinZip Self-Extractor");
            dicPackersName.Add(".WWPACK", "WWPACK Packer");
            dicPackersName.Add(".yP", "Y0da Protector");
            dicPackersName.Add(".y0da", "Y0da Protector");
        }

        public void GetPackerName(string FilePath)
        {
            try
            {
                Features.PeFile pe = new Features.PeFile(FilePath);

                for (int i = 0; i < pe.ImageSectionHeaders.Length; i++)
                {
                    if (dicPackersName.ContainsKey(pe.ImageSectionHeaders[i].Name.ToString()))
                    {
                        PackerNumber += 1;
                        IsPacker = true;
                        
                        foreach (KeyValuePair<string,string> keyValuePair in dicPackersName)
                        {
                            PackerName += keyValuePair.Value;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
