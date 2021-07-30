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
        public static float time = 0.0f;    //<-临时使用渲染时间
        protected static UInt64 IDCounter = 0; 
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

        public static UInt64 GenerateID()
        {
            var now = DateTime.Now;
            UInt64 id = (UInt64) (((((((now.Year * 365 + now.DayOfYear) * 24) + now.Hour) * 60) + now.Minute) * 60 + now.Second) *
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
            
            // var mat = render.sharedMaterial;
            // if (mat == null) mat = render.material;
            return render.material;
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
            //Debug.Log(shaderName);
            
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
            Material materal = null;
            try
            {
                 
                materal = renderer.material;
                //materal = renderer.material;
                
            }
            catch (Exception e)
            {
                
                materal = renderer.material;
                

                Debug.LogError("Material error;");
            }
            
            
            return materal;
        }

        public static Bounds GetGameObjectBounds(GameObject obj)
        {
            var renders = obj.GetComponentsInChildren<Renderer>();
            
            float r = 99999.0f;
            Vector3 minPoint=new Vector3(r,r,r);
            Vector3 maxPoint=new Vector3(-r,-r,-r);
            foreach (var render in renders)
            {
                var bounds = render.bounds;
                
                
                
                var p = bounds.min;
                minPoint.x = Mathf.Min(minPoint.x, p.x);
                minPoint.y = Mathf.Min(minPoint.y, p.y);
                minPoint.z = Mathf.Min(minPoint.z, p.z);

                p = bounds.max;
                maxPoint.x = Mathf.Max(maxPoint.x, p.x);
                maxPoint.y = Mathf.Max(maxPoint.y, p.y);
                maxPoint.z = Mathf.Max(maxPoint.z, p.z);
            }

            var center = (minPoint + maxPoint) * .5f;
            var size = maxPoint - minPoint;
           
            
            return new Bounds(center,size);
        }
        
        
        public static Bounds GetSelectionBounds()
        {
            List<Renderer> renders=new List<Renderer>();
            foreach (var obj in Selection.gameObjects)
            {
                var rs = obj.GetComponentsInChildren<Renderer>();
                foreach (var renderer in rs)
                {
                    renders.Add(renderer);
                }
            }
            float r = 99999.0f;
            Vector3 minPoint=new Vector3(r,r,r);
            Vector3 maxPoint=new Vector3(-r,-r,-r);
            foreach (var render in renders)
            {
                var bounds = render.bounds;
                var p = bounds.min;
                minPoint.x = Mathf.Min(minPoint.x, p.x);
                minPoint.y = Mathf.Min(minPoint.y, p.y);
                minPoint.z = Mathf.Min(minPoint.z, p.z);

                p = bounds.max;
                maxPoint.x = Mathf.Max(maxPoint.x, p.x);
                maxPoint.y = Mathf.Max(maxPoint.y, p.y);
                maxPoint.z = Mathf.Max(maxPoint.z, p.z);
            }

            var center = (minPoint + maxPoint) * .5f;
            var size = maxPoint - minPoint;
            return new Bounds(center,size);
        }

        public static Vector3 SnapToPoints(List<Vector3> points, Vector3 p,int mode)
        {
            float dis = 999999;
            Vector3 snapPoint = p;
            foreach (var pt in points)
            {
                float d = 99999.0f;
                if (mode == 0)
                {
                    d=Vector3.Distance(pt, p);    
                }else if (mode == 1)
                {
                    d = Mathf.Abs(pt.x - p.x);
                    
                }else if (mode == 2)
                {
                    d = Mathf.Abs(pt.y - p.y);
                }
                
                if (d < dis)
                {
                    dis = d;
                    snapPoint = pt;
                }
            }
            return snapPoint;
        }

    }
}