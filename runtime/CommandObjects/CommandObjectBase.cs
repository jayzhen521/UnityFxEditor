using System;
using System.IO;
using UnityEngine;

namespace Packages.FxEditor
{
    public class CommandObjectBase:BinaryObject
    {
        //-------------------------------------------------
        public const int CommandTypeDrawMesh=1;
        public const int CommandTypeChangeShader=2;
        public const int CommandTypeBeginCanvas=3;
        public const int CommandTypeEndCanvas=4;
        public const int CommandTypeImageSlot=5;
        public const int CommandTypeCanvasSlot=6;
        public const int CommandTypeParticleSystem=7;
        
        //-------------------------------------------------


        public Int64 ObjectType = 0;
        public Int64 Size = 0;
        public void WriteToStream(Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            Write(ms);

            //---processing alignment of data block.
            var count = ConstantValues.CommandDataBlockAlign - ms.Length % ConstantValues.CommandDataBlockAlign;
            var data = ms.ToArray();
            Size = data.Length + count;
            
            
            Write(stream,ObjectType);
            Write(stream,Size);
            Write(stream, data);
            for (int i = 0; i < count; i++)
            {
                stream.WriteByte(0);
            }
            //--------------------------------------
            
            ms.Close();
        }

        protected virtual void Write(Stream stream)
        {
            
        }
    }
}