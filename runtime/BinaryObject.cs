using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Packages.FxEditor
{
    public class BinaryObject
    {
        protected void Write(Stream stream, byte[] v)
        {
            if (v == null) return;
            stream.Write(v, 0, v.Length);
        }

        protected void Write(Stream stream, byte[] v, int count)
        {
            if (v == null) return;
            stream.Write(v, 0, count);
        }

        protected void Write(Stream stream, string v)
        {
            var data = Encoding.UTF8.GetBytes(v);
            Write(stream,data.Length+1);
            stream.Write(data, 0, data.Length);
            stream.WriteByte(0);
        }

        protected void Write(Stream stream, short v)
        {
            var data = BitConverter.GetBytes(v);
            stream.Write(data, 0, data.Length);
        }

        protected void Write(Stream stream, int v)
        {
            var data = BitConverter.GetBytes(v);
            stream.Write(data, 0, data.Length);
        }
        protected void Write(Stream stream, int[] v)
        {
            if (v == null) return;
            for(int i=0;i<v.Length;i++)
                Write(stream,v[i]);
        }

        protected void Write(Stream stream, long v)
        {
            var data = BitConverter.GetBytes(v);
            stream.Write(data, 0, data.Length);
        }
        protected void Write(Stream stream, long[] v)
        {
            if (v == null) return;
            for(int i=0;i<v.Length;i++)
                Write(stream,v[i]);
        }
        

        protected void Write(Stream stream, ushort v)
        {
            var data = BitConverter.GetBytes(v);
            stream.Write(data, 0, data.Length);
        }
        protected void Write(Stream stream, ushort[] v)
        {
            if (v == null) return;
            for(int i=0;i<v.Length;i++)
                Write(stream,v[i]);
        }

        protected void Write(Stream stream, uint v)
        {
            var data = BitConverter.GetBytes(v);
            stream.Write(data, 0, data.Length);
        }
        protected void Write(Stream stream, uint[] v)
        {
            if (v == null) return;
            for(int i=0;i<v.Length;i++)
                Write(stream,v[i]);
        }

        protected void Write(Stream stream, ulong v)
        {
            var data = BitConverter.GetBytes(v);
            stream.Write(data, 0, data.Length);
        }
        protected void Write(Stream stream, ulong[] v)
        {
            if (v == null) return;
            for(int i=0;i<v.Length;i++)
                Write(stream,v[i]);
        }

        protected void Write(Stream stream, float v)
        {
            var data = BitConverter.GetBytes(v);
            stream.Write(data, 0, data.Length);
        }
        protected void Write(Stream stream, float[] v)
        {
            if (v == null) return;
            for(int i=0;i<v.Length;i++)
                Write(stream,v[i]);
        }

        protected void Write(Stream stream, Vector2 v)
        {
            Write(stream, v[0]);
            Write(stream, v[1]);
        }
        protected void Write(Stream stream, Vector2[] v)
        {
            if (v == null) return;
            for(int i=0;i<v.Length;i++)
                Write(stream,v[i]);
        }
        

        protected void Write(Stream stream, Vector3 v)
        {
            Write(stream, v[0]);
            Write(stream, v[1]);
            Write(stream, v[2]);
        }
        protected void Write(Stream stream, Vector3[] v)
        {
            if (v == null) return;
            for(int i=0;i<v.Length;i++)
                Write(stream,v[i]);
        }

        protected void Write(Stream stream, Vector4 v)
        {
            Write(stream, v[0]);
            Write(stream, v[1]);
            Write(stream, v[2]);
            Write(stream, v[3]);
        }
        protected void Write(Stream stream, Vector4[] v)
        {
            if (v == null) return;
            for(int i=0;i<v.Length;i++)
                Write(stream,v[i]);
        }
        

        protected void Write(Stream stream, Color v)
        {
            Write(stream, v.r);
            Write(stream, v.g);
            Write(stream, v.b);
            Write(stream, v.a);
        }
        protected void Write(Stream stream, Color[] v)
        {
            if (v == null) return;
            for(int i=0;i<v.Length;i++)
                Write(stream,v[i]);
        }

        protected void Write(Stream stream, Matrix4x4 v)
        {
            for(int i=0;i<16;i++)
                Write(stream,v[i]);
        }

        protected void Write(Stream stream, Matrix4x4[] v)
        {
            if (v == null) return;
            for(int i=0;i<v.Length;i++)
                Write(stream,v[i]);
        }
    }
}