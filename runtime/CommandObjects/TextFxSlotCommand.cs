using System.IO;
using UnityEngine;

namespace Packages.FxEditor
{
    public class TextFxSlotCommand:CommandObjectBase
    {
        private int id = 0;
        private float size = 1.0f;
        private int textAlignment = 0;
        private Matrix4x4 matrixVP = new Matrix4x4();
        private Matrix4x4 matirxObjectToWorld = new Matrix4x4();
        
        private const int AlignmentLeft = 0;
        private const int AlignmentCenter = 1;
        private const int AlignmentRight = 2;
        
        public TextFxSlotCommand(Camera cam, GameObject gameObject, Exporter exporter)
        {
            ObjectType = CommandTypeTextFxSlot;
            //----------------------------------
            var textobj = gameObject.GetComponent<TextFx>();
            id = textobj.soltID;
            size = textobj.size;
            switch (textobj.align)
            {
                case TextAlignment.Left :
                    textAlignment = AlignmentLeft;
                    break;
                case TextAlignment.Center:
                    textAlignment = AlignmentCenter;
                    break;
                case TextAlignment.Right:
                    textAlignment = AlignmentRight;
                    break;
            }
            
            var viewMatrix = cam.worldToCameraMatrix;
            var projectMatrix = cam.projectionMatrix;
            matrixVP =   projectMatrix*viewMatrix;
            matirxObjectToWorld = gameObject.transform.localToWorldMatrix;
        }

        protected override void Write(Stream stream)
        {
            Write(stream,id);
            Write(stream,size);
            Write(stream,textAlignment);
            
            Write(stream,matrixVP);
            Write(stream,matirxObjectToWorld);
        }
    }
}