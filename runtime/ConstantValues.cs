using System;

namespace Packages.FxEditor
{
    public class ConstantValues
    {
        //public static UInt32 FileFourCC = 0x58464C48; //<-文件
        public const string FileFourCC = "HLFX";
        //-----------------version-------------
        public const ushort VersionMajor = 0;
        public const ushort VersionSecondMajor = 0;
        public const ushort VersionMinor = 0;
        public const ushort VersionPatch = 0;
        //------------------------------
        
        public const long DataBlockAlign = 32;
        public const long CommandDataBlockAlign = 8;
    }
}