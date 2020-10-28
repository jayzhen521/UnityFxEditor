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
            var obj=new GameObject("Node");
            var cam=obj.AddComponent<Camera>();
            cam.orthographic = true;
            obj.AddComponent<FxCanvasObject>();

        }
        [MenuItem("FxEditor/创建物体/节点/效果节点")]
        public static void OnCreateFxNode()
        {
            var obj=new GameObject("Node");
            var cam=obj.AddComponent<Camera>();
            cam.orthographic = true;
            var canvasObject = obj.AddComponent<FxCanvasObject>();
            var qual = GameObject.CreatePrimitive(PrimitiveType.Quad);
            qual.transform.parent=cam.gameObject.transform;
            qual.transform.position=new Vector3(0,0,1);
            qual.transform.localScale=new Vector3(cam.orthographicSize*2,cam.orthographicSize*2,1);
            Material mat=new Material("");
            var render = qual.GetComponent<Renderer>();
            render.material = mat;
            canvasObject.root = qual;

            Selection.activeObject = obj;

        }
        [MenuItem("FxEditor/创建物体/节点/输入效果节点")]
        public static void OnCreateFxNodeInput()
        {
            var obj=new GameObject("Node");
            var cam=obj.AddComponent<Camera>();
            cam.orthographic = true;
            var canvasObject = obj.AddComponent<FxCanvasObject>();
            var qual = GameObject.CreatePrimitive(PrimitiveType.Quad);
            qual.transform.parent=cam.gameObject.transform;
            qual.transform.position=new Vector3(0,0,1);
            qual.transform.localScale=new Vector3(cam.orthographicSize*2,cam.orthographicSize*2,1);
            Material mat=new Material("");
            var render = qual.GetComponent<Renderer>();
            render.material = mat;
            canvasObject.root = qual;
            //------------------------------
            
            var slot = qual.AddComponent<FxCanvasSlot>();
            Selection.activeObject = obj;
            
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
            
            var obj=new GameObject("Timeline");
             timeline=obj.AddComponent<Timeline>();
            
        }
        
        
        [MenuItem("FxEditor/显示隐藏UI")]
        public static void OnUIToggle()
        {
            var obj = Object.FindObjectOfType<SceneConfig>();
            obj.showCanvasUI = !obj.showCanvasUI;
        }
        
        // [MenuItem("FxEditor/显示导出报告")]
        // public static void OnShowExportReport()
        // {
        //     
        // }
        //
        // [MenuItem("FxEditor/场景报告")]
        // public static void OnSceneChecker()
        // {
        //     var checker=new SceneInformation();
        //     checker.ShowModal();
        // }
        
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
            if (filepath == null) return;
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