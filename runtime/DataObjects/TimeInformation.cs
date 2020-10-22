using System.IO;

namespace Packages.FxEditor
{
    public class TimeInformation : DataObjectBase
    {
        public override void Write(Stream stream)
        {
            Position = stream.Position;

            base.Write(stream);
        }
    }
}