using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Packages.FxEditor
{
    public class MenuItems
    {

        [MenuItem("FxEditor/工具/Alpha分离/配置Alpha分离")]
        public static void OnCreateAlphaSpliter()
        {
            float aspect = Camera.main.aspect;
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = Color.black;
            Camera.main.orthographic = true;
            Camera.main.transform.position=new Vector3(0,0,-10);
            float h = Camera.main.orthographicSize;
            float w = h * aspect;
            GameObject obj=new GameObject("VideoAlphaSpliter");
            var spliter = obj.AddComponent<AlphaChannelSpliter>();
            obj.transform.position = Vector3.zero;
            
            GameObject objRGB=GameObject.CreatePrimitive(PrimitiveType.Quad);
            objRGB.name = "RGB";
            objRGB.transform.parent = obj.transform;
            objRGB.transform.localScale=new Vector3(w,h*2,1);
            objRGB.transform.position=new Vector3(-w*0.5f,0,0);
            objRGB.GetComponent<MeshRenderer>().material=new Material(Shader.Find("HLFx/TextureColorMask"));
            
            GameObject objAlpha=GameObject.CreatePrimitive(PrimitiveType.Quad);
            objAlpha.name = "Alpha";
            objAlpha.transform.parent = obj.transform;
            objAlpha.transform.localScale=new Vector3(w,h*2,1);
            objAlpha.transform.position=new Vector3(w*0.5f,0,0);
            objAlpha.GetComponent<MeshRenderer>().material=new Material(Shader.Find("HLFx/Tools/AlphaToGray"));

            spliter.rgbObject = objRGB;
            spliter.alphaObject = objAlpha;
        }
        [MenuItem("FxEditor/工具/Alpha分离/水平分布")]
        public static void OnToHor()
        {
            float aspect = Camera.main.aspect;
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = Color.black;
            Camera.main.orthographic = true;
            Camera.main.transform.position=new Vector3(0,0,-10);
            
            
            float h = Camera.main.orthographicSize;
            float w = h * aspect;
            
            var spliter = Object.FindObjectOfType<AlphaChannelSpliter>();

            GameObject objRGB = spliter.rgbObject;
            objRGB.transform.localScale=new Vector3(w,h*2,1);
            objRGB.transform.position=new Vector3(-w*0.5f,0,0);
            
            
            GameObject objAlpha=spliter.alphaObject;
            objAlpha.transform.localScale=new Vector3(w,h*2,1);
            objAlpha.transform.position=new Vector3(w*0.5f,0,0);

        }
        [MenuItem("FxEditor/工具/Alpha分离/垂直分布")]
        public static void OnToVert()
        {
            float aspect = Camera.main.aspect;
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = Color.black;
            Camera.main.orthographic = true;
            Camera.main.transform.position=new Vector3(0,0,-10);
            
            
            float h = Camera.main.orthographicSize;
            float w = h * aspect;
            
            var spliter = Object.FindObjectOfType<AlphaChannelSpliter>();

            GameObject objRGB = spliter.rgbObject;
            objRGB.transform.localScale=new Vector3(w*2,h,1);
            objRGB.transform.position=new Vector3(0,-h*0.5f,0);
            
            
            GameObject objAlpha=spliter.alphaObject;
            objAlpha.transform.localScale=new Vector3(w*2,h,1);
            objAlpha.transform.position=new Vector3(0,h*0.5f,0);

        }
        
        
        [MenuItem("FxEditor/工具/Alpha分离/运行Alpha分离")]
        public static void OnAlphaChannelSplit()
        {
            var spliter=Object.FindObjectOfType<AlphaChannelSpliter>();
            if (spliter == null) return;
            
            spliter.Run();
        }
        
        
        [MenuItem("FxEditor/创建配置")]
        public static void OnCreateConfig()
        {
            var obj = Object.FindObjectOfType<SceneConfig>();
            if (obj != null)
            {
                //Message
                EditorUtility.DisplayDialog("Warning", "场景中已经创建了配置！", "OK");
                return;
            }

            var gameObject = new GameObject("SceneConfig");
            gameObject.AddComponent<SceneConfig>();
        }


        [MenuItem("FxEditor/创建物体/节点/空节点")]
        public static void OnCreateNode()
        {
            var obj = new GameObject("Node");


            var aspect = 1.0f;
            if(Camera.main!=null) aspect=Camera.main.aspect;
            
            var cam = obj.AddComponent<Camera>();
            cam.orthographic = true;
            var canvasObject = obj.AddComponent<FxCanvasObject>();
            canvasObject.bounds_color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);

            obj.name = GlobalUtility.GetUniqueName("Node");

            var objectRoot = new GameObject("noderoot");
            objectRoot.transform.parent = obj.transform;
            canvasObject.root = objectRoot;
            canvasObject.width =(int) (canvasObject.height * aspect);
        }

        [MenuItem("FxEditor/创建物体/节点/效果节点")]
        public static void OnCreateFxNode()
        {
            var obj = new GameObject();



            var aspect = 1.0f;
            if(Camera.main!=null) aspect=Camera.main.aspect;
            
            var cam = obj.AddComponent<Camera>();
            cam.orthographic = true;
            var canvasObject = obj.AddComponent<FxCanvasObject>();

            var objectRoot = new GameObject("noderoot");
            objectRoot.transform.parent = obj.transform;

            canvasObject.root = objectRoot;

            canvasObject.bounds_color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);

            var qual = GameObject.CreatePrimitive(PrimitiveType.Quad);
            qual.transform.parent = cam.gameObject.transform;
            qual.transform.position = new Vector3(0, 0, 1);
            qual.transform.localScale =
                new Vector3(cam.orthographicSize * 2*aspect + 0.01f, cam.orthographicSize * 2 + 0.01f, 1);
            canvasObject.width =(int) (canvasObject.height * aspect);
            qual.transform.parent = objectRoot.transform;
            //Material mat=new Material("");
            Material mat = GlobalUtility.CreateNewMaterialForNode();
            var render = qual.GetComponent<Renderer>();
            render.material = mat;


            Selection.activeObject = obj;
            obj.name = GlobalUtility.GetUniqueName(mat.shader.name);
        }

        [MenuItem("FxEditor/创建物体/节点/输入效果节点")]
        public static void OnCreateFxNodeInput()
        {
            var obj = new GameObject();
            var aspect = 1.0f;
            if(Camera.main!=null) aspect=Camera.main.aspect;

            var cam = obj.AddComponent<Camera>();
            cam.orthographic = true;
            var canvasObject = obj.AddComponent<FxCanvasObject>();
            var objectRoot = new GameObject("noderoot");
            objectRoot.transform.parent = obj.transform;
            canvasObject.root = objectRoot;

            canvasObject.bounds_color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);

            var qual = GameObject.CreatePrimitive(PrimitiveType.Quad);
            qual.transform.parent = cam.gameObject.transform;
            qual.transform.position = new Vector3(0, 0, 1);
            qual.transform.localScale =
                new Vector3(cam.orthographicSize * aspect*2 + 0.01f, cam.orthographicSize * 2 + 0.01f, 1);
            qual.transform.parent = objectRoot.transform;
            canvasObject.width =(int) (canvasObject.height * aspect);
            //Material mat=new Material("");
            Material mat = GlobalUtility.CreateNewMaterialForNode();

            var render = qual.GetComponent<Renderer>();
            render.material = mat;

            //------------------------------

            var slot = qual.AddComponent<FxCanvasSlot>();
            Selection.activeObject = obj;
            obj.name = GlobalUtility.GetUniqueName(mat.shader.name);
        }

        [MenuItem("FxEditor/创建物体/节点/链接节点 _c")]
        public static void OnConnectNodes()
        {
            FxCanvasSlot slot = null;
            FxCanvasObject node = null;

            
            foreach (var gameObject in Selection.gameObjects)
            {
                var slotobj = gameObject.GetComponentInChildren<FxCanvasSlot>();
                if (slotobj != null && slotobj.canvas == null)
                {
                    slot = slotobj;
                    break;
                }
            }

            if (slot == null) return;
            
            List<FxCanvasObject> cs=new List<FxCanvasObject>();
            var pc = slot.gameObject.GetComponentsInChildren<FxCanvasObject>();
            foreach (var p in pc)
            {
                cs.Add(p);
            }
            pc = slot.gameObject.GetComponentsInParent<FxCanvasObject>();
            foreach (var p in pc)
            {
                cs.Add(p);
            }
            
            //---------
            foreach (var gameObject in Selection.gameObjects)
            {
                //----------node-------------
                var nodeobj = gameObject.GetComponentInChildren<FxCanvasObject>();
                if (nodeobj != null&&cs.IndexOf(nodeobj)<0)
                {
                    node = nodeobj;
                    break;
                }
                nodeobj = gameObject.GetComponentInParent<FxCanvasObject>();
                if (nodeobj != null&&cs.IndexOf(nodeobj)<0)
                {
                    node = nodeobj;
                    break;
                }
            }

            Debug.Log("node:"+node+",slot:"+slot);
            if (slot == null || node == null)
            {
                Debug.LogWarning("选择的内容中找不到能够链接的对象！！");
            }
            else
            {
                slot.canvas = node;
                
            }
        }

        [MenuItem("FxEditor/创建物体/效果图片")]
        public static void OnCreateFxQuad()
        {
            var qual = GameObject.CreatePrimitive(PrimitiveType.Quad);

            //Material mat=new Material("");
            Material mat = GlobalUtility.CreateNewMaterialForNode();
            var render = qual.GetComponent<Renderer>();
            render.material = mat;
            qual.name = GlobalUtility.GetUniqueName(mat.shader.name);
            
            if (Selection.activeObject!=null&&Selection.activeObject is GameObject)
            {
                var obj = Selection.activeObject as GameObject;
                
                qual.transform.parent = obj.transform;
                qual.transform.localPosition = new Vector3(0, 0, 1);

                var cam = obj.GetComponent<Camera>();
                if (cam != null)
                {
                    qual.transform.localScale=new Vector3(cam.orthographicSize *2* cam.aspect + 0.01f, cam.orthographicSize*2 + 0.01f, 1);
                }
                
            }
            
            Selection.activeObject = qual;
            
        }

        [MenuItem("FxEditor/创建物体/文字")]
        public static void OnCreateTextFx()
        {
            GameObject obj = new GameObject();
            var textfx = obj.AddComponent<TextFx>();
            textfx.text = "Hello";
            textfx.material = new Material(Shader.Find("HLFx/Text/SolidColor"));

            obj.name = GlobalUtility.GetUniqueName("TextFx");
        }


        [MenuItem("FxEditor/工具/修复动画时间")]
        public static void OnFixAnimationTime()
        {
            var pb = new MaterialPropertyBlock();

            var objs = Object.FindObjectsOfType<Animator>();
            foreach (var animator in objs)
            {
                var acs = AnimationUtility.GetAnimationClips(animator.gameObject);
                foreach (var animationClip in acs)
                {
                    animationClip.SampleAnimation(animator.gameObject, 0.0f);
                }

                var render = animator.gameObject.GetComponent<Renderer>();
                if (render == null) continue;
                render.GetPropertyBlock(pb);

                var shader = render.material.shader;
                for (int i = 0; i < shader.GetPropertyCount(); i++)
                {
                    var type = shader.GetPropertyType(i);
                    switch (type)
                    {
                        case ShaderPropertyType.Color:
                            render.material.SetColor(i, pb.GetColor(i));
                            break;
                        case ShaderPropertyType.Vector:
                            render.material.SetVector(i, pb.GetVector(i));
                            break;
                        case ShaderPropertyType.Float:
                            render.material.SetFloat(i, pb.GetFloat(i));
                            break;
                        case ShaderPropertyType.Range:

                            break;
                        case ShaderPropertyType.Texture:
                            render.material.SetTexture(i, pb.GetTexture(i));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        [MenuItem("FxEditor/工具/克隆材质")]
        public static void OnCloneMaterials()
        {
            foreach (var VARIABLE in Selection.objects)
            {
                var obj = Selection.activeObject as GameObject;
                if (obj == null)
                {
                    Debug.LogError("需要选择包含材质的节点");
                    return;
                }

                var rs = obj.GetComponentsInChildren<Renderer>();
                foreach (var r in rs)
                {
                    var mat = r.material;
                    var newmat = new Material(mat.shader);
                    newmat.CopyPropertiesFromMaterial(mat);
                    r.material = newmat;
                    GlobalUtility.SaveNewMaterialFile(newmat);
                }
            }
        }

        [MenuItem("FxEditor/工具/纹理映射")]
        public static void OnTextureMapping()
        {
            var bound = GlobalUtility.GetSelectionBounds();
            foreach (var gameobject in Selection.gameObjects)
            {
                Renderer rnd = gameobject.GetComponent<Renderer>();
                if (rnd == null) continue;

                Material m = rnd.material;

                var bd = rnd.bounds;
                var v1 = bd.min - bound.min;
                v1.x /= bound.size.x;
                v1.y /= bound.size.y;

                var v2 = new Vector3(
                    bd.size.x / bound.size.x,
                    bd.size.y / bound.size.y,
                    0
                );


                // Vector3 v1=cam.WorldToViewportPoint(rnd.bounds.min);
                // Vector3 v2=cam.WorldToViewportPoint(rnd.bounds.max);

                m.mainTextureOffset = new Vector2(v1.x, v1.y);
                m.mainTextureScale = new Vector2(v2.x, v2.y);
            }
        }

        [MenuItem("FxEditor/工具/纹理去重")]
        public static void OnTextureDeduplicates()
        {
            
            string indir = "";
            

            indir=EditorUtility.OpenFolderPanel("选择纹理去重目录", "", "");
            if (indir == "") return;


            try
            {
                if (EditorUtility.DisplayDialog("警告",String.Format("这个操作可能对数据造成不可逆的破坏确认目录是:\n{0}\n吗？",indir),  "OK", "Cancel"))
                {
                    TextureDeduplicates td=new TextureDeduplicates();
                    td.ScanDirectory(indir);
                }
            }
            catch (Exception e)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError(e.Message);
                
                throw;
            }
            

            return;
            
        }
        
        [MenuItem("FxEditor/工具/材质排序")]
        public static void OnMaterialOrder()
        {
            var rs = Object.FindObjectsOfType<Renderer>();
            foreach (var r in rs)
            {
                var t = r.gameObject.transform;
                int z = (int) (t.position.z * 10);
                r.material.renderQueue = 4000 - z;
            }
        }
        
        

        [MenuItem("FxEditor/动画工具/添加动画属性")]
        public static void OnCreateAnimation()
        {
            foreach (var gameObject in Selection.gameObjects)
            {
                gameObject.AddComponent<AnimationProperty>();
            }
        }
        
        [MenuItem("FxEditor/动画工具/创建程序动画对象")]
        public static void OnCreateProceduralAnimation()
        {
            var pd = new GameObject("ProceduralAnimationRoot");
            pd.AddComponent<ProceduralAnimation>();
            foreach (var gameObject in Selection.gameObjects)
            {
                gameObject.transform.parent = pd.transform;
            }
        }
        


        [MenuItem("FxEditor/显示隐藏UI")]
        public static void OnUIToggle()
        {
            var obj = Object.FindObjectOfType<SceneConfig>();
            obj.showCanvasUI = !obj.showCanvasUI;
        }

        [MenuItem("FxEditor/导出数据")]
        public static void OnExport()
        {
            var obj = Object.FindObjectOfType<SceneConfig>();
            if (obj == null)
            {
                Debug.LogError("没有创建场景配置对象");
                return;
            }

            var filepath = EditorUtility.SaveFilePanel("", "", "", "videofx");
            //var filepath="/Volumes/Workspace/Projects/HLVideoFx/source/PlatformsApp/testdata/fx/test.videofx";
            if (filepath == null || filepath == "") return;
            obj.outputPath = filepath;
            obj.forExport = true;
            EditorApplication.ExecuteMenuItem("Edit/Play");
        }


        
        [MenuItem("MyTest/test")]
        public static void OnTest()
        {
            var fs = AssetDatabase.FindAssets("*",new []{"Assets/assettest"});

            var path = AssetDatabase.GUIDToAssetPath(fs[0]);
            var obj = AssetDatabase.LoadMainAssetAtPath(path);
            Object.Instantiate(obj);
            
            Debug.Log(obj);
            
            // foreach (var s in fs)
            // {
            //     var lbs = AssetDatabase.GetLabels(new GUID(s));
            //     foreach (var lb in lbs)
            //     {
            //         Debug.Log(lb);    
            //     }
            //     
            // }
        }
            
        //
        // [MenuItem("FxEditor/更新")]
        // public static void OnUpdate()
        // {
        //     //Client.Add("https://github.com/Helin777/UnityFxEditor.git");
        // }

        // [MenuItem("Test/MMA")]
        // public static void OnMMA()
        // {
        //     Process.Start("/Users/henry/Temp/ttt.wls");
        // }
    }
}