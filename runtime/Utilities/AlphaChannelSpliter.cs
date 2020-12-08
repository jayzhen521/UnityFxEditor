using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Packages.FxEditor
{
    public class AlphaChannelSpliter : MonoBehaviour
    {
        private RenderTexture framebuffer = null;
        private Texture2D textureA = null;
        private Texture2D textureB = null;
        private Texture2D finalTexture = null;

        public GameObject rgbObject = null;
        public GameObject alphaObject = null;


        public int width = 1920;
        public int height = 1080;

        [Tooltip("输入图片序列文件所在目录")]
        public string inputDir = "";
        
        [Tooltip("图片序列文件输出目录")]
        public string outputDir = "";

        public AlphaChannelSpliter()
        {
        }

        void CreateObject()
        {
            framebuffer = new RenderTexture(width, height, 16, RenderTextureFormat.Default);
            
            textureA = new Texture2D(width, height);
            textureB = new Texture2D(width, height);
            finalTexture = new Texture2D(width, height);
        }

        private void SpliteHorizontal(FileInfo pngfile, FileInfo outfile)
        {
            var data = File.ReadAllBytes(pngfile.FullName);
            ImageConversion.LoadImage(textureA, data);
            int size = width * height;
            
            ImageConversion.LoadImage(textureB, data);
            // var cs=textureB.GetRawTextureData<Color32>();
            // for (int i = 0; i < size; i++)
            // {
            //     var c = cs[i];
            //     //c.r =255;
            //     c.g = 255;
            //     c.b =255;
            //     c.a = 255;
            //     cs[i] = c;
            // }
            // textureB.Apply();
            
            
            EditorUtility.CompressTexture(textureB,TextureFormat.Alpha8,0);
            
            rgbObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = textureA;
            alphaObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture=textureB;
            
            

            var camera = Camera.main;
            camera.targetTexture = framebuffer;
            var oldtarget = RenderTexture.active;
            RenderTexture.active = camera.targetTexture;
            camera.Render();
            finalTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            finalTexture.Apply();
            RenderTexture.active = oldtarget;

            data = ImageConversion.EncodeToPNG(finalTexture);
            
            File.WriteAllBytes(outfile.FullName, data);
        }

        public void Run()
        {
            
            var source = inputDir;
            var dest = outputDir;
            CreateObject();

            var srcDire = new DirectoryInfo(source);
            if (!srcDire.Exists)
            {
                Debug.LogError("找不到输入目录!!!");
                return;
            }
            Directory.CreateDirectory(dest);
            
            var srcFiles = srcDire.GetFiles("*.png");


            int count = srcFiles.Length;
            if (count <= 0)
            {
                Debug.LogError("输入目录中找不到任何PNG文件!!!");
                return;
            }
            float i = 0.0f;
            foreach (var fileInfo in srcFiles)
            {
                string outfile = dest + "/" + fileInfo.Name;
                SpliteHorizontal(fileInfo, new FileInfo(outfile));
                i++;
                if (EditorUtility.DisplayCancelableProgressBar("分离Alpha通道中...", fileInfo.Name, i / count)) break;
            }

            EditorUtility.ClearProgressBar();
        }
    }
}