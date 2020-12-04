using System;
using System.IO;
using System.Xml.Serialization;

namespace Packages.FxEditor
{
    public class CanvasSlotCommand:CommandObjectBase
    {
        public UInt64 canvasID;
        public string channelName = "";
        
        public CanvasSlotCommand(FxCanvasSlot obj,Exporter exporter)
        {
            ObjectType = CommandTypeCanvasSlot;
            //----------------
            canvasID = exporter.GetObject(obj.canvas).ObjectID;
            channelName = obj.names[obj.channelName];
        }

        protected override void Write(Stream stream)
        {
            Write(stream,canvasID);
            Write(stream,channelName);
        }
    }
}