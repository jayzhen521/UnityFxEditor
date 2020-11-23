using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Packages.FxEditor
{
    public class MenuItems
    {
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
            var cam = obj.AddComponent<Camera>();
            cam.orthographic = true;
            var canvasObject = obj.AddComponent<FxCanvasObject>();
            canvasObject.bounds_color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);

            obj.name = GlobalUtility.GetUniqueName("Node");
        }

        [MenuItem("FxEditor/创建物体/节点/效果节点")]
        public static void OnCreateFxNode()
        {
            var obj = new GameObject();

            var cam = obj.AddComponent<Camera>();
            cam.orthographic = true;
            var canvasObject = obj.AddComponent<FxCanvasObject>();
            canvasObject.bounds_color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);

            var qual = GameObject.CreatePrimitive(PrimitiveType.Quad);
            qual.transform.parent = cam.gameObject.transform;
            qual.transform.position = new Vector3(0, 0, 1);
            qual.transform.localScale = new Vector3(cam.orthographicSize * 2, cam.orthographicSize * 2, 1);
            //Material mat=new Material("");
            Material mat = GlobalUtility.CreateNewMaterialForNode();
            var render = qual.GetComponent<Renderer>();
            render.material = mat;
            canvasObject.root = qual;

            Selection.activeObject = obj;
            obj.name = GlobalUtility.GetUniqueName(mat.shader.name);
        }

        [MenuItem("FxEditor/创建物体/节点/输入效果节点")]
        public static void OnCreateFxNodeInput()
        {
            var obj = new GameObject();


            var cam = obj.AddComponent<Camera>();
            cam.orthographic = true;
            var canvasObject = obj.AddComponent<FxCanvasObject>();
            canvasObject.bounds_color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);

            var qual = GameObject.CreatePrimitive(PrimitiveType.Quad);
            qual.transform.parent = cam.gameObject.transform;
            qual.transform.position = new Vector3(0, 0, 1);
            qual.transform.localScale = new Vector3(cam.orthographicSize * 2, cam.orthographicSize * 2, 1);
            //Material mat=new Material("");
            Material mat = GlobalUtility.CreateNewMaterialForNode();
            var render = qual.GetComponent<Renderer>();
            render.material = mat;
            canvasObject.root = qual;
            //------------------------------

            var slot = qual.AddComponent<FxCanvasSlot>();
            Selection.activeObject = obj;
            obj.name = GlobalUtility.GetUniqueName(mat.shader.name);
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
            Selection.activeObject = qual;
        }

        [MenuItem("FxEditor/创建物体/时间线")]
        public static void OnCreateTimeline()
        {
            var timeline = Object.FindObjectOfType<Timeline>();
            if (timeline != null)
            {
                Debug.LogError("场景中已经创建了Timeline对象");
                Selection.activeObject = timeline.gameObject;
                return;
            }

            var obj = new GameObject("Timeline");
            timeline = obj.AddComponent<Timeline>();
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
            


            
            var objs = Object.FindObjectsOfType<Animator>();
            foreach (var animator in objs)
            {
                var acs = AnimationUtility.GetAnimationClips(animator.gameObject);
                foreach (var animationClip in acs)
                {
                    animationClip.SampleAnimation(animator.gameObject, 0.0f);
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
                    var mat = r.sharedMaterial;
                    var newmat = new Material(mat.shader);
                    newmat.CopyPropertiesFromMaterial(mat);
                    r.sharedMaterial = newmat;
                    GlobalUtility.SaveNewMaterialFile(newmat);
                }
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

        [MenuItem("FxEditor/更新")]
        public static void OnUpdate()
        {
            Client.Add("https://github.com/Helin777/UnityFxEditor.git");
        }
    }
}