using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavTool.Sig
{
    class PE
    {
        public static float[] GetFeatures(string FilePath)
        {
            Features.PeFile peFile = new Features.PeFile(FilePath);

            uint Characteristics = 0;
            uint NumberOfLinenumbers = 0;
            uint NumberOfRelocations = 0;
            uint PhysicalAddress = 0;
            uint PointerToLinenumbers = 0;
            uint PointerToRawData = 0;
            uint PointerToRelocations = 0;
            uint SizeOfRawData = 0;
            uint VirtualAddress = 0;
            uint VirtualSize = 0;

            for (int i = 0; i < peFile.ImageSectionHeaders.Length - 1; i++)
            {
                Characteristics = peFile.ImageSectionHeaders[i].Characteristics;
                NumberOfLinenumbers = peFile.ImageSectionHeaders[i].NumberOfLinenumbers;
                NumberOfRelocations = peFile.ImageSectionHeaders[i].NumberOfRelocations;
                PhysicalAddress = peFile.ImageSectionHeaders[i].PhysicalAddress;
                PointerToLinenumbers = peFile.ImageSectionHeaders[i].PointerToLinenumbers;
                PointerToRawData = peFile.ImageSectionHeaders[i].PointerToRawData;
                PointerToRelocations = peFile.ImageSectionHeaders[i].PointerToRelocations;
                SizeOfRawData = peFile.ImageSectionHeaders[i].SizeOfRawData;
                VirtualAddress = peFile.ImageSectionHeaders[i].VirtualAddress;
                VirtualSize = peFile.ImageSectionHeaders[i].VirtualSize;
            }

            ushort _NumberOfModuleForwarderRefs = 0;
            ushort _OffsetModuleName = 0;
            uint _TimeDateStamp = 0;

            if (peFile.ImageBoundImportDescriptor != null)
            {
                _NumberOfModuleForwarderRefs = peFile.ImageBoundImportDescriptor.NumberOfModuleForwarderRefs;
                _OffsetModuleName = peFile.ImageBoundImportDescriptor.OffsetModuleName;
                _TimeDateStamp = peFile.ImageBoundImportDescriptor.TimeDateStamp;
            }

            uint cb = 0;
            uint cmtSize = 0;
            uint cmtVirtualAddress = 0;
            uint EntryPointRVA = 0;
            uint EntryPointToken = 0;
            uint exportTableSize = 0;
            uint exportTableVirtualAddress = 0;
            uint Flags = 0;
            uint MajorRuntimeVersion = 0;
            uint mnhSize = 0;
            uint mnhVirtualAddress = 0;
            uint mdSize = 0;
            uint mdVirtualAddress = 0;
            uint MinorRuntimeVersion = 0;
            uint rSize = 0;
            uint rVirtualAddress = 0;
            uint nameSize = 0;
            uint nameVirtualAddress = 0;
            uint vtSize = 0;
            uint vtVirtualAddress = 0;

            if (peFile.HasValidComDescriptor)
            {
                cb = peFile.ImageComDescriptor.cb;
                cmtSize = peFile.ImageComDescriptor.CodeManagerTable.Size;
                cmtVirtualAddress = peFile.ImageComDescriptor.CodeManagerTable.VirtualAddress;
                EntryPointRVA = peFile.ImageComDescriptor.EntryPointRVA;
                EntryPointToken = peFile.ImageComDescriptor.EntryPointToken;
                exportTableSize = peFile.ImageComDescriptor.ExportAddressTableJumps.Size;
                exportTableVirtualAddress = peFile.ImageComDescriptor.ExportAddressTableJumps.VirtualAddress;
                Flags = peFile.ImageComDescriptor.Flags;
                MajorRuntimeVersion = peFile.ImageComDescriptor.MajorRuntimeVersion;
                mnhSize = peFile.ImageComDescriptor.ManagedNativeHeader.Size;
                mnhVirtualAddress = peFile.ImageComDescriptor.ManagedNativeHeader.VirtualAddress;
                mdSize = peFile.ImageComDescriptor.MetaData.Size;
                mdVirtualAddress = peFile.ImageComDescriptor.MetaData.VirtualAddress;
                MinorRuntimeVersion = peFile.ImageComDescriptor.MinorRuntimeVersion;
                rSize = peFile.ImageComDescriptor.Resources.Size;
                rVirtualAddress = peFile.ImageComDescriptor.Resources.VirtualAddress;
                nameSize = peFile.ImageComDescriptor.StrongNameSignature.Size;
                nameVirtualAddress = peFile.ImageComDescriptor.StrongNameSignature.VirtualAddress;
                vtSize = peFile.ImageComDescriptor.VTableFixups.Size;
                vtVirtualAddress = peFile.ImageComDescriptor.VTableFixups.VirtualAddress;
            }

            uint exportAddressOfFunctions = 0;
            uint exportAddressOfNameOrdinals = 0;
            uint exportAddressOfNames = 0;
            uint exportBase = 0;
            uint exportCharacteristics = 0;
            uint exportMajorVersion = 0;
            uint exportMinorVersion = 0;
            uint exportName = 0;
            uint NumberOfFunctions = 0;
            uint exportNumberOfNames = 0;
            uint exportTimeDateStamp = 0;

            if (peFile.HasValidExportDir)
            {
                exportAddressOfFunctions = peFile.ImageExportDirectory.AddressOfFunctions;
                exportAddressOfNameOrdinals = peFile.ImageExportDirectory.AddressOfNameOrdinals;
                exportAddressOfNames = peFile.ImageExportDirectory.AddressOfNames;
                exportBase = peFile.ImageExportDirectory.Base;
                exportCharacteristics = peFile.ImageExportDirectory.Characteristics;
                exportMajorVersion = peFile.ImageExportDirectory.MajorVersion;
                exportMinorVersion = peFile.ImageExportDirectory.MinorVersion;
                exportName = peFile.ImageExportDirectory.Name;
                NumberOfFunctions = peFile.ImageExportDirectory.NumberOfFunctions;
                exportNumberOfNames = peFile.ImageExportDirectory.NumberOfNames;
                exportTimeDateStamp = peFile.ImageExportDirectory.TimeDateStamp;
            }

            float entropy = GetEntropyValue(peFile.Buff);

            float[] peFeatures = new float[]
            {
                (float)peFile.FileSize,
                Convert.ToSingle(peFile.HasValidComDescriptor),
                Convert.ToSingle(peFile.HasValidExceptionDir),
                Convert.ToSingle(peFile.HasValidExportDir),
                Convert.ToSingle(peFile.HasValidImportDir),
                Convert.ToSingle(peFile.HasValidRelocDir),
                Convert.ToSingle(peFile.HasValidResourceDir),
                Convert.ToSingle(peFile.HasValidSecurityDir),
                Convert.ToSingle(peFile.Is32Bit),
                Convert.ToSingle(peFile.Is64Bit),
                Convert.ToSingle(peFile.IsDLL),
                Convert.ToSingle(peFile.IsSigned),
                Convert.ToSingle(peFile.IsValidPeFile),
                (float)peFile.ImageBoundImportDescriptor.NumberOfModuleForwarderRefs,
                (float)peFile.ImageBoundImportDescriptor.OffsetModuleName,
                (float)peFile.ImageBoundImportDescriptor.TimeDateStamp,
                (float)peFile.ImageDosHeader.e_cblp,
                (float)peFile.ImageDosHeader.e_cp,
                (float)peFile.ImageDosHeader.e_cparhdr,
                (float)peFile.ImageDosHeader.e_crlc,
                (float)peFile.ImageDosHeader.e_cs,
                (float)peFile.ImageDosHeader.e_csum,
                (float)peFile.ImageDosHeader.e_ip,
                (float)peFile.ImageDosHeader.e_lfanew,
                (float)peFile.ImageDosHeader.e_lfarlc,
                (float)peFile.ImageDosHeader.e_magic,
                (float)peFile.ImageDosHeader.e_maxalloc,
                (float)peFile.ImageDosHeader.e_minalloc,
                (float)peFile.ImageDosHeader.e_oemid,
                (float)peFile.ImageDosHeader.e_oeminfo,
                (float)peFile.ImageDosHeader.e_ovno,
                (float)peFile.ImageDosHeader.e_sp,
                (float)peFile.ImageDosHeader.e_ss,
                (float)peFile.ImageNtHeaders.Signature,
                (float)peFile.ImageNtHeaders.FileHeader.Characteristics,
                (float)peFile.ImageNtHeaders.FileHeader.Machine,
                (float)peFile.ImageNtHeaders.FileHeader.NumberOfSections,
                (float)peFile.ImageNtHeaders.FileHeader.NumberOfSymbols,
                (float)peFile.ImageNtHeaders.FileHeader.PointerToSymbolTable,
                (float)peFile.ImageNtHeaders.FileHeader.SizeOfOptionalHeader,
                (float)peFile.ImageNtHeaders.FileHeader.TimeDateStamp,
                (float)peFile.ImageNtHeaders.OptionalHeader.AddressOfEntryPoint,
                (float)peFile.ImageNtHeaders.OptionalHeader.Architecture.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.Architecture.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.BaseOfCode,
                (float)peFile.ImageNtHeaders.OptionalHeader.BaseOfData,
                (float)peFile.ImageNtHeaders.OptionalHeader.BaseRelocationTable.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.BaseRelocationTable.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.BoundImport.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.BoundImport.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.CertificateTable.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.CertificateTable.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.CheckSum,
                (float)peFile.ImageNtHeaders.OptionalHeader.CLRRuntimeHeader.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.CLRRuntimeHeader.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.Debug.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.Debug.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.DelayImportDescriptor.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.DelayImportDescriptor.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.DllCharacteristics,
                (float)peFile.ImageNtHeaders.OptionalHeader.ExceptionTable.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.ExceptionTable.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.ExportTable.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.ExportTable.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.FileAlignment,
                (float)peFile.ImageNtHeaders.OptionalHeader.GlobalPtr.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.GlobalPtr.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.IAT.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.IAT.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.ImageBase,
                (float)peFile.ImageNtHeaders.OptionalHeader.ImportTable.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.ImportTable.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.LoadConfigTable.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.LoadConfigTable.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.LoaderFlags,
                (float)peFile.ImageNtHeaders.OptionalHeader.Magic,
                (float)peFile.ImageNtHeaders.OptionalHeader.MajorImageVersion,
                (float)peFile.ImageNtHeaders.OptionalHeader.MajorLinkerVersion,
                (float)peFile.ImageNtHeaders.OptionalHeader.MajorOperatingSystemVersion,
                (float)peFile.ImageNtHeaders.OptionalHeader.MajorSubsystemVersion,
                (float)peFile.ImageNtHeaders.OptionalHeader.MinorImageVersion,
                (float)peFile.ImageNtHeaders.OptionalHeader.MinorLinkerVersion,
                (float)peFile.ImageNtHeaders.OptionalHeader.MinorOperatingSystemVersion,
                (float)peFile.ImageNtHeaders.OptionalHeader.MinorSubsystemVersion,
                (float)peFile.ImageNtHeaders.OptionalHeader.NumberOfRvaAndSizes,
                (float)peFile.ImageNtHeaders.OptionalHeader.Reserved.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.Reserved.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.ResourceTable.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.ResourceTable.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.SectionAlignment,
                (float)peFile.ImageNtHeaders.OptionalHeader.SizeOfCode,
                (float)peFile.ImageNtHeaders.OptionalHeader.SizeOfHeaders,
                (float)peFile.ImageNtHeaders.OptionalHeader.SizeOfHeapCommit,
                (float)peFile.ImageNtHeaders.OptionalHeader.SizeOfHeapReserve,
                (float)peFile.ImageNtHeaders.OptionalHeader.SizeOfImage,
                (float)peFile.ImageNtHeaders.OptionalHeader.SizeOfInitializedData,
                (float)peFile.ImageNtHeaders.OptionalHeader.SizeOfStackCommit,
                (float)peFile.ImageNtHeaders.OptionalHeader.SizeOfStackReserve,
                (float)peFile.ImageNtHeaders.OptionalHeader.SizeOfUninitializedData,
                (float)peFile.ImageNtHeaders.OptionalHeader.Subsystem,
                (float)peFile.ImageNtHeaders.OptionalHeader.TLSTable.Size,
                (float)peFile.ImageNtHeaders.OptionalHeader.TLSTable.VirtualAddress,
                (float)peFile.ImageNtHeaders.OptionalHeader.Win32VersionValue,
                (float)peFile.ImageResourceDirectory.Characteristics,
                (float)peFile.ImageResourceDirectory.MajorVersion,
                (float)peFile.ImageResourceDirectory.MinorVersion,
                (float)peFile.ImageResourceDirectory.NumberOfIdEntries,
                (float)peFile.ImageResourceDirectory.NumberOfNameEntries,
                (float)peFile.ImageResourceDirectory.TimeDateStamp,
                (float)Characteristics,
                (float)NumberOfLinenumbers,
                (float)NumberOfRelocations,
                (float)PhysicalAddress,
                (float)PointerToLinenumbers,
                (float)PointerToRawData,
                (float)PointerToRelocations,
                (float)SizeOfRawData,
                (float)VirtualAddress,
                (float)VirtualSize,
                (float)cb,
                (float)cmtSize,
                (float)cmtVirtualAddress,
                (float)EntryPointRVA,
                (float)EntryPointToken,
                (float)exportTableSize,
                (float)exportTableVirtualAddress,
                (float)Flags,
                (float)MajorRuntimeVersion,
                (float)mnhSize,
                (float)mnhVirtualAddress,
                (float)mdSize,
                (float)mdVirtualAddress,
                (float)MinorRuntimeVersion,
                (float)rSize,
                (float)rVirtualAddress,
                (float)nameSize,
                (float)nameVirtualAddress,
                (float)vtSize,
                (float)vtVirtualAddress,
                (float)exportAddressOfFunctions,
                (float)exportAddressOfNameOrdinals,
                (float)exportAddressOfNames,
                (float)exportBase,
                (float)exportCharacteristics,
                (float)exportMajorVersion,
                (float)exportMinorVersion,
                (float)exportName,
                (float)NumberOfFunctions,
                (float)exportNumberOfNames,
                (float)exportTimeDateStamp,
                entropy
            };


            return peFeatures;
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
    }
}
