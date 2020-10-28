using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace Packages.FxEditor
{
    public class GlobalUtility
    {
        protected static Int64 IDCounter = 0; 
        public static string GetShaderCode(string path)
        {
            
            var name = Application.dataPath+"/../Packages/FxEditor/runtime/EmbeddedResources/Shaders/" + path; // "Package.FxEditor.runtime." + path;
            var file = new FileInfo(name);
            return File.ReadAllText(name);
            // var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            // Debug.Log(stream.ToString());
            // if (stream == null) return "";
            //
            // var reader = new StreamReader(stream);
            // var text = reader.ReadToEnd();
            // return text;
        }

        public static Int64 GenerateID()
        {
            var now = DateTime.Now;
            Int64 id = (((((((now.Year * 365 + now.DayOfYear) * 24) + now.Hour) * 60) + now.Minute) * 60 + now.Second) *
                1000 + now.Millisecond);
            IDCounter++;
            return IDCounter;
        }


        public static void UpdateCanvasNodeOrder()
        {
            //set to zero
            var canvasList = UnityEngine.Object.FindObjectsOfType<FxCanvasObject>();
            foreach (var fxCanvasObject in canvasList)
            {
                fxCanvasObject.nodeOrder = 0;
            }
            
            var css = UnityEngine.Object.FindObjectsOfType<FxCanvasSlot>();
            
            
            foreach(var cs in css)
            {
                if (cs.canvas == null) continue;
                
                 int maxNode = 100;  //max depth of the nodes
                 Stack<FxCanvasObject> stack=new Stack<FxCanvasObject>();
                 stack.Push(cs.canvas);
                 
                 
                 while (stack.Count>0&&maxNode>0)
                 {
                     var canvas = stack.Pop();
                     canvas.nodeOrder++;
                     if (canvas.root == null) continue;
                     maxNode--;
                     
                     foreach (var cs2 in css)
                     {
                         if (cs2.canvas == null) continue;
                         if (cs2 == cs) continue;
                         if(cs2.gameObject.transform.IsChildOf(canvas.root.transform))stack.Push(cs2.canvas);
                     }
                 }
            }
        }

        public static Material GetObjectMaterial(GameObject obj)
        {
            var render = obj.GetComponent<Renderer>();
            
            var mat = render.sharedMaterial;
            if (mat == null) mat = render.material;
            return mat;
        }
    }
}