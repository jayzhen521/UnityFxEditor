using System.IO;
using System.Text;

namespace Packages.FxEditor
{
    public class FileHeader : BinaryObject
    {
        
        public string FourCC = ConstantValues.FileFourCC;
        public ushort[] Version =
        {
            ConstantValues.VersionMajor,
            ConstantValues.VersionSecondMajor,
            ConstantValues.VersionMinor,
            ConstantValues.VersionPatch
        };
        public int DataCount = 0;

        public byte[] getBytes()
        {
            byte[] data=new byte[1024];
            var ms = new MemoryStream(data);
            
            var fourCCdata = Encoding.UTF8.GetBytes(FourCC);
            Write(ms, fourCCdata, 4);
            Write(ms, Version);
            Write(ms, DataCount);
            
            return ms.ToArray();
        }
    }
}