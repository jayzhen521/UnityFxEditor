using System;
using System.IO;
using UnityEngine;

namespace Packages.FxEditor
{
    public class DataObjectBase : BinaryObject
    {
        //-----------------------------------------
        public const uint ObjectTypeTimeInformation = 1;
        public const uint ObjectTypeMesh = 2;
        public const uint ObjectTypeTexture = 3;
        public const uint ObjectTypeShader = 4;
        public const uint ObjectTypeMaterial = 5;
        public const uint ObjectTypeVideo = 6;
        public const uint ObjectTypeSound = 7;
        public const uint ObjectTypeFrame = 8;
        public const uint ObjectTypeHostInformation = 9;
        public const uint ObjectTypeAnimationClip = 10;
        //-----------------------------------------


        public UInt64 ObjectID = GlobalUtility.GenerateID();
        public Int64 ObjectType = 0;
        public Int64 Position = 0;
        public Int64 Size = 0;
        public static UInt64 DataObjectHeaderSize = 64;

        public static UInt64 ObjectSize
        {
            get { return sizeof(Int64) * 4; }
        }

        /// <summary>
        /// write object header data.
        /// </summary>
        /// <param name="stream"></param>
        public void WriteHeaderData(Stream stream)
        {
            byte[] data = new byte[DataObjectHeaderSize];
            MemoryStream ms = new MemoryStream(data);
            //---------------------------------------
            Write(ms, ObjectID);
            Write(ms, ObjectType);
            Write(ms, Position);
            Write(ms, Size);
            //---------------------------------------
            stream.Write(data, 0, data.Length);
            ms.Close();
        }

        public void WriteToStream(Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            Write(ms);

            var data = ms.ToArray();
            Position = stream.Position;
            Size = data.Length;
            
            Write(stream, data);
            
            //---processing alignment of data block.
            var count = ConstantValues.DataBlockAlign - ms.Length % ConstantValues.DataBlockAlign;
            for (int i = 0; i < count; i++)
            {
                ms.WriteByte(0);
            }
            //--------------------------------------

            
            ms.Close();
        }

        public virtual void Write(Stream stream)
        {
        }
    }
}