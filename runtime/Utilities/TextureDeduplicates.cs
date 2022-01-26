using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Packages.FxEditor
{
    public class ImageSet
    {
        public int id = 0;
        public FileInfo file = null;
    }

    public class TextureDeduplicates
    {
        bool ImageCompare(Texture2D image1, Texture2D image2)
        {
            if (image1.width != image2.width || image1.height != image2.height) return false;
            if (image1.format != image2.format) return false;

            var dataA = image1.GetRawTextureData<Color32>();
            var dataB = image2.GetRawTextureData<Color32>();

            if (dataA.Length != dataB.Length) return false;
            
            
            int count = dataA.Length;
            int diffCount = 0;
            for (int i = 0; i < count; i++)
            {
                var c1 = dataA[i];
                var c2 = dataB[i];
                if (c1.a!=c2.a||
                    c1.r!=c2.r||
                    c1.g!=c2.g||
                    c1.b!=c2.b) diffCount++;
            }

            if (diffCount <=10) return true;
            return false;
        }

        public void ScanDirectory(string dir)
        {
            var outdir = dir + "/sharedimages";
            DirectoryInfo outdirinfo = new DirectoryInfo(outdir);
            if (outdirinfo.Exists)
            {
                outdirinfo.Delete(true);
            }

            Directory.CreateDirectory(outdir);


            var dirInfo = new DirectoryInfo(dir);
            var files = dirInfo.GetFiles("*", SearchOption.AllDirectories);

            //---------scan image file----------
            List<FileInfo> imageFiles = new List<FileInfo>();
            foreach (var fileInfo in files)
            {
                if (fileInfo.Extension == ".jpg"
                 || fileInfo.Extension == ".png"
                 || fileInfo.Extension == ".pkm") imageFiles.Add(fileInfo);
            }

            //------------------------------------
            int[] setCounter = new int[1024];
            for (int i = 0; i < 1024; i++) setCounter[i] = 0;

            List<ImageSet> imageSets = new List<ImageSet>();
            int setid = 0;
            Texture2D imageA = new Texture2D(2, 2);
            Texture2D imageB = new Texture2D(2, 2);

            int allCOunt = imageFiles.Count;
            float procCount = 0.0f;
            foreach (var imageFile in imageFiles)
            {

                procCount++;
                if (EditorUtility.DisplayCancelableProgressBar("纹理处理", imageFile.FullName, procCount / allCOunt))break;
                //----------------------
                bool have = false;
                foreach (var imageSet in imageSets)
                {
                    if (imageSet.file == imageFile)
                    {
                        have = true;
                        break;
                    }
                }

                if (have) continue;
                //----------------------

                ImageConversion.LoadImage(imageA, File.ReadAllBytes(imageFile.FullName));
                foreach (var subimageFile in imageFiles)
                {
                     if (subimageFile == imageFile || subimageFile.FullName.Substring(subimageFile.FullName.Length - 4, 4) == ".pkm") continue;
                    ImageConversion.LoadImage(imageB, File.ReadAllBytes(subimageFile.FullName));
                    if (ImageCompare(imageA, imageB))
                    {
                        ImageSet imageSet = new ImageSet();
                        imageSet.id = setid;
                        imageSet.file = subimageFile;
                        setCounter[setid]++;
                        imageSets.Add(imageSet);
                    }
                }

                if (setCounter[setid] > 0)
                {
                    ImageSet imageSet = new ImageSet();
                    imageSet.id = setid;
                    imageSet.file = imageFile;
                    imageSets.Add(imageSet);
                }

                setid++;
            }
            EditorUtility.ClearProgressBar();

            
            //----------copy file--------
            for (int i = 0; i < setid; i++)
            {
                foreach (var imageSet in imageSets)
                {
                    if (imageSet.id != i) continue;
                    
                    string filepath = string.Format("{0}/{1}{2}", outdir, i, imageSet.file.Extension);
                    File.Copy(imageSet.file.FullName,filepath);
                    
                    break;
                }    
            }
            
            //-------modiftiy set file---
            var fixfiles = dirInfo.GetFiles("*.fix", SearchOption.AllDirectories);
            foreach (var fileInfo in fixfiles)
            {
                string text = File.ReadAllText(fileInfo.FullName);
                foreach (var imageSet in imageSets)
                {
                    string srctext = imageSet.file.Name;
                    string desttext = string.Format("{0}{1}", imageSet.id, imageSet.file.Extension);
                    text = text.Replace(srctext, desttext);
                }
                File.WriteAllText(fileInfo.FullName,text);
            }
            //----------delete file-----
            foreach (var imageSet in imageSets)
            {
                File.Delete(imageSet.file.FullName);
            }
            
            //------move file to out dir------------
            foreach (var imageFile in imageFiles)
            {
                
                if(!imageFile.Exists)continue;
                imageFile.MoveTo(outdir+"/"+imageFile.Name);
                
                
            }
        }
    }
}