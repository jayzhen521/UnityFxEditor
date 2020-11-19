using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.UnityLinker;
using UnityEngine;
using Object = System.Object;
using Random = System.Random;

namespace Packages.FxEditor
{
    public class GlobalUtility
    {
        protected static Int64 IDCounter = 0; 
        public static string GetShaderCode(string path)
        {
            
            var name = "Packages/com.nbox.fxeditor/runtime/EmbeddedResources/Shaders/" + path; // "Package.FxEditor.runtime." + path;
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
            return IDCounter+id;
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

        public static string GetUniqueName(string name)
        {
            var objects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            List<string > names=new List<string>();
            foreach (var gameObject in objects)
            {
                names.Add(gameObject.name);
            }

            string newname = ObjectNames.GetUniqueName(names.ToArray(), name);
            return newname;
        }

        public static Material CreateNewMaterialForNode()
        {
            string[] filters = {"shader","shader"};
            
            string file=EditorUtility.OpenFilePanelWithFilters("","Packages/com.nbox.fxeditor/shaders/",filters);
            if (file == null) return null;
            
            
            FileInfo fileinfo=new FileInfo(file);
            string shaderName = "HLFx/" + fileinfo.Name.Replace(".shader","");
            Debug.Log(shaderName);
            
            Material material = new Material(Shader.Find(shaderName));
            

            SaveNewMaterialFile(material);
            
            return material;
        }

        public static void SaveNewMaterialFile(Material material)
        {
            //--------------------------------------
            string dir = "Assets/materialsCache";
            Directory.CreateDirectory(dir);
            
            string filename = dir+"/"+Guid.NewGuid().ToString() + ".mat";
            AssetDatabase.CreateAsset(material, filename);
            //--------------------------------------
        }

        private static Random rnd = null;
        public static int[] RandomSample(int count)
        {
            rnd= new Random();
            
            int[] randomizePos = new int[count];
            randomizePos[0] = 0;
            int pos = 1;
            
            while (pos<count)
            {
                
                bool have = false;
                int v = rnd.Next(count);
                for (int i = 0; i < pos; i++)
                {
                    if (randomizePos[i] == v)
                    {
                        have = true;
                        break;
                    }
                }

                if (!have)
                {
                    randomizePos[pos] = v;
                    pos++;
                }
            }

            return randomizePos;
        }


        public static Material GetMaterial(Renderer renderer)
        {
            var materal = renderer.sharedMaterial;
            return materal;
        }
            
    }
}