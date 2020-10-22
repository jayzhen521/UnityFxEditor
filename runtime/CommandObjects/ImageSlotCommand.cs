using System.IO;

namespace Packages.FxEditor
{
    public class ImageSlotCommand:CommandObjectBase
    {
        private int id = 0;
        private string channelName = "";
        public ImageSlotCommand(FxImageSlot obj)
        {
            ObjectType = CommandTypeImageSlot;
            //----------------------------------
            id = obj.slotID;
            channelName = obj.names[obj.channelName];
            
        }

        protected override void Write(Stream stream)
        {
            Write(stream,id);
            Write(stream,channelName);
            base.Write(stream);
        }
    }
}