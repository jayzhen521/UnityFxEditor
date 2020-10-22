using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Packages.FxEditor
{
    public class Exporter
    {
        //-------------------
        private readonly List<FrameObject> framesData = new List<FrameObject>();
        private readonly Dictionary<Object, DataObjectBase> objects = new Dictionary<Object, DataObjectBase>();

        public Exporter()
        {
            ShaderObject.RegisterShaders();
            TextureObject.RegisterFormat();
        }

        //-------------------
        public int AddFrame()
        {
            var frame = new FrameObject(this);
            framesData.Add(frame);
            return framesData.Count;
        }

        //----------------------------------------
        private void ScanFrameData()
        {
        }

        //----------------------------------------
        public DataObjectBase GetObject(Object obj)
        {
            
            if (objects.ContainsKey(obj))
                return objects[obj];
            
            
            if (obj is Shader)
            {
                var dataobject = new ShaderObject(obj as Shader);
                objects[obj] = dataobject;
                return dataobject;
            }

            if (obj is Mesh)
            {
                var dataobject = new MeshObject(obj as Mesh);
                objects[obj] = dataobject;
                return dataobject;
            }

            if (obj is Texture)
            {
                var dataobject = new TextureObject(obj as Texture);
                objects[obj] = dataobject;
                return dataobject;
            }

            if (obj is FxCanvasObject)
            {
                var dataobject = new CanvasObject(obj as FxCanvasObject);
                objects[obj] = dataobject;
                return dataobject;
            }

            return null;
        }


        public void SaveToFile(string path)
        {
            var ms = new MemoryStream();


            //-------------Write data of file header------------------
            {
                var header = new FileHeader();
                var count = objects.Count + framesData.Count;
                header.DataCount = count;
                var data = header.getBytes();
                ms.Write(data, 0, data.Length);
            }
            //--------------------------------------------------------
            var dataMS = new MemoryStream();

            //-------------Write data of resources------------------
            {
                foreach (var keydata in objects)
                {
                    var obj = keydata.Value;
                    obj.WriteToStream(dataMS);
                    obj.WriteHeaderData(ms);
                }
            }
            //--------------------------------------------------------

            //-------------Frames data of resources------------------
            {
                foreach (var obj in framesData)
                {
                    obj.WriteToStream(dataMS);
                    obj.WriteHeaderData(ms);
                }
            }
            //--------------------------------------------------------

            //-------------Flush the data to file------------------
            var resData = dataMS.ToArray();
            ms.Write(resData, 0, resData.Length);
            File.WriteAllBytes(path, ms.ToArray());
            Debug.Log("count:" + objects.Count);
            Debug.Log(message: "frames:" + framesData.Count);
        }

        //----------------------------------------
    }
}