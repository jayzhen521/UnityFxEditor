using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Packages.FxEditor
{
    public class TextureDeduplicates
    {
        public void ScanDirectory(string dir)
        {
            var dirInfo=new DirectoryInfo(dir);
            var files=dirInfo.GetFiles("*");
            
            //---------scan image file----------
            List<FileInfo> imageFiles=new List<FileInfo>();
            foreach (var fileInfo in files)
            {
                if(fileInfo.Extension=="jpg"||fileInfo.Extension=="png")imageFiles.Add(fileInfo);
            }
            //------------------------------------


        }
    }
}