using System.IO;
using UnityEngine;

namespace Packages.FxEditor
{
    public class BeginCanvasCommand:CommandObjectBase
    {
        private int width = 32;
        private int height= 32;
        private Color clearColor = Color.black;

        private CanvasObject _canvasObject = null;

        public BeginCanvasCommand(FxCanvasObject obj,Exporter exporter)
        {
            ObjectType = CommandTypeBeginCanvas;
            //-------------------------
            width = obj.width;
            height = obj.height;
            clearColor = obj.backgroundColor;

            _canvasObject = exporter.GetObject(obj) as CanvasObject;
        }

        protected override void Write(Stream stream)
        {
            Write(stream,_canvasObject.ObjectID);
            Write(stream,width);
            Write(stream,height);
            Write(stream, clearColor);
        }
    }
}