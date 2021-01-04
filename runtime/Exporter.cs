using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
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

            //-----------time--------------------
            {
                var obj = Object.FindObjectOfType<SceneConfig>();
                var dataobject = new TimeInformation(obj);
                objects[obj] = dataobject;
            }
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
            if (obj == null) return null;
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
                TextureObject dataobject = null;

                dataobject = new TextureObject(obj as Texture);
                //dataobject = new TextureObject(Texture2D.whiteTexture);
                objects[obj] = dataobject;

                return dataobject;
            }

            if (obj is FxCanvasObject)
            {
                var dataobject = new CanvasObject(obj as FxCanvasObject);
                objects[obj] = dataobject;
                return dataobject;
            }

            if (obj is AnimationClip)
            {
                var dataobject = new AnimationClipObject(obj as AnimationClip);
                objects[obj] = dataobject;
                return dataobject;
            }

            return null;
        }


        public void SaveToFile(string path)
        {
            var ms = new MemoryStream();
            int headersize = 0;


            //-------------Write data of file header------------------
            {
                var header = new FileHeader();
                var count = objects.Count + framesData.Count;
                header.DataCount = count;
                var data = header.getBytes();
                ms.Write(data, 0, data.Length);
                headersize = data.Length + header.DataCount * (int) DataObjectBase.DataObjectHeaderSize;
            }
            //--------------------------------------------------------
            var dataMS = new MemoryStream();

            //-------------Write data of resources------------------
            string outputDir = (new FileInfo(path)).DirectoryName;
            //string datasetfile = outputDir + "/dataset.txt";
            string datasetfile = path + ".fix";
            var dataset = new StreamWriter(datasetfile);
            {
                foreach (var keydata in objects)
                {
                    var obj = keydata.Value;
                    obj.WriteToStream(dataMS);
                    obj.WriteHeaderData(ms);

                    //-------for copy texture file----------

                    var tex = obj as TextureObject;
                    var config = Object.FindObjectOfType<SceneConfig>();
                    if (tex != null && config.isExternalTexture)
                    {
                        //     
                        //     
                        //Debug.Log("cccc:");
                        if (tex.externalTextureData != null)
                        {
                            var texData = tex.externalTextureData;
                            var tex2d = tex._texture as Texture2D;
                            //
                            // FileInfo dstinfo = new FileInfo(path);
                            // FileInfo srcinfo = new FileInfo(tex.externalTextureData.path);
                            string ext = ".jpg";
                            byte[] data = null;

                            if (tex2d.format == TextureFormat.RGBA32)
                            {
                                ext = ".png";
                                data = ImageConversion.EncodeToPNG(tex2d);
                            }
                            else
                            {
                                data = ImageConversion.EncodeToJPG(tex2d, config.jpegCompressQuality);
                            }

                            string filename = obj.ObjectID.ToString() + ext;
                            string outfile = string.Format("{0}/{1}", outputDir, filename);
                            File.WriteAllBytes(outfile, data);
                            

                            dataset.WriteLine(string.Format("{0} {1} {2} {3}",
                                filename,
                                texData.position + headersize + obj.Position,
                                texData.size,
                                texData.format));
                        }
                    }
                    //---------------------------------------
                }
            }
            dataset.Close();

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

            var info = new FileInfo(path);
            if (info.Exists) info.Delete();

            File.WriteAllBytes(path, ms.ToArray());
            Debug.Log("count:" + objects.Count);
            Debug.Log(message: "frames:" + framesData.Count);

            //testExportFile(path);

            RelayoutFile(path);
        }

        private void RelayoutFile(string path)
        {
            string fixfile = path + ".fix";
            string text = File.ReadAllText(fixfile);
            string[] lines = text.Split('\n');
            byte[] srcData = File.ReadAllBytes(path);
            byte[] dstData = new byte[srcData.Length];

            
            //compute data block
            int currentDstPos = 0;
            int srcPos = 0;
            foreach (var line in lines)
            {
                if (line == "") continue;
                string[] fs = line.Split(' ');
                

                int pos = int.Parse(fs[1]);
                int size = int.Parse(fs[2]);

                int datasize = pos - srcPos;
                Array.Copy(srcData, srcPos, 
                    dstData,currentDstPos,datasize);
                currentDstPos +=datasize;
                srcPos = pos + size;
            }

            //last data block
            {
                int datasize = srcData.Length-srcPos;
                Array.Copy(srcData, srcPos, 
                    dstData,currentDstPos,datasize);
                currentDstPos +=datasize;
            }
            byte[] finaldata = new byte[currentDstPos];
            Array.Copy(dstData,0,
                finaldata,0,currentDstPos);
            

            File.Delete(path);
            File.WriteAllBytes(path, finaldata);
        }

        // public void testExportFile(string path)
        // {
        //     string outputDir = (new FileInfo(path)).DirectoryName;
        //     string datasetfile = outputDir + "/dataset.txt";
        //     string outputfile = outputDir + "/output.videofx";
        //     File.Copy(path,outputfile,true);
        //     var outfile=new FileStream(outputfile,FileMode.Open);
        //     var dataset=new StreamReader(datasetfile);
        //     while (!dataset.EndOfStream)
        //     {
        //         string linetext = dataset.ReadLine();
        //         if (linetext == "") continue;
        //         string[] fs = linetext.Split(' ');
        //         UInt64 pos = UInt64.Parse(fs[1]);
        //         UInt64 size = UInt64.Parse(fs[2]);
        //         outfile.Seek((long) pos, SeekOrigin.Begin);
        //         for (var i = 0; i <(int) size; i++)
        //         {
        //             outfile.WriteByte((byte) (i%255));
        //         }
        //     }
        // }
        //----------------------------------------
    }
}