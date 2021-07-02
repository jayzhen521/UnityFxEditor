using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Packages.FxEditor
{
    public class QuickExport
    {
        private SceneConfig config = null;
        private string outputDir = "";
        private string outputFile = "";
        private string message = "";
        private Camera currentCamera = null;
        private ScriptBase[] scripts = null;

        bool GetOutputFile()
        {
            //------UI---------
            if (config.outputPath == null || config.outputPath == "")
            {
                config.outputPath = EditorUtility.SaveFilePanel("", ".", "", "videofx");
            }

            if (config.outputPath == "")
            {
                Debug.LogWarning("取消导出");
                return false;
            }


            outputFile = config.outputPath;
            var info = new FileInfo(outputFile);
            outputDir = info.DirectoryName;

            return true;
        }


        void UpdateSceneByTime(float time)
        {
            //--------------
            //var scripts = Object.FindObjectsOfType<ScriptBase>();

            //-------------
            var pb = new MaterialPropertyBlock();
            var animas = Object.FindObjectsOfType<Animator>();
            foreach (var animator in animas)
            {
                var acs = AnimationUtility.GetAnimationClips(animator.gameObject);
                foreach (var animationClip in acs)
                {
                    animationClip.SampleAnimation(animator.gameObject, time);

                    var render = animator.gameObject.GetComponent<Renderer>();
                    if (render == null) continue;
                    render.GetPropertyBlock(pb);

                    var shader = render.sharedMaterial.shader;
                    for (int j = 0; j < shader.GetPropertyCount(); j++)
                    {
                        var type = shader.GetPropertyType(j);
                        switch (type)
                        {
                            case ShaderPropertyType.Color:
                                GlobalUtility.GetMaterial(render).SetColor(j, pb.GetColor(j));
                                break;
                            case ShaderPropertyType.Vector:
                                GlobalUtility.GetMaterial(render).SetVector(j, pb.GetVector(j));
                                break;
                            case ShaderPropertyType.Float:
                                GlobalUtility.GetMaterial(render).SetFloat(j, pb.GetFloat(j));
                                break;
                            case ShaderPropertyType.Range:

                                break;
                            case ShaderPropertyType.Texture:
                                GlobalUtility.GetMaterial(render).SetTexture(j, pb.GetTexture(j));
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }

            if (scripts != null)
            {
                foreach (var scriptBase in scripts)
                {
                    scriptBase.UpdateAnimation();
                }
            }
            //--------------
        }

        void BatchExportScene(BatchExportConfig btconfig)
        {
            //Hidden
            foreach (var btconfigExcludeGameObject in btconfig.ExcludeGameObjects)
            {
                btconfigExcludeGameObject.SetActive(false);
            }


            int count = btconfig.ExportItems.Count;

            var tmpItems = new List<GameObject>();
            for (var i = 0; i < count; i++)
            {
                tmpItems.Add(btconfig.ExportItems[i].exportRoot);
            }


            for (var i = 0; i < count; i++)
            {
                var item = btconfig.ExportItems[i];
                var itemObject = tmpItems[i];

                if (itemObject == null)
                {
                    Debug.LogWarning("批量导出中有没有配置物体节点的数据");
                    continue;
                }

                if (item.filename == "")
                {
                    Debug.LogWarning("批量导出中有没有配置文件名称的数据:" + item.exportRoot.name);
                    continue;
                }

                message = string.Format("批量导出中{0}/{1}", i + 1, count);
                foreach (var it in tmpItems)
                {
                    if (it == null) continue;

                    it.SetActive(false);
                }


                itemObject.SetActive(true);

                config.outputPath = outputDir + "/" + item.filename;

                var oldcam = SceneConfig.currentCamera;
                currentCamera = item.camera;
                SimpleExportScene();
                currentCamera = null;
            }


            for (var i = 0; i < count; i++)
            {
                if (tmpItems[i] == null) continue;
                ;
                tmpItems[i].SetActive(true);
                btconfig.ExportItems[i].exportRoot = tmpItems[i];
            }


            config.outputPath = outputFile;

            // EditorUtility.DisplayCancelableProgressBar("快速批量导出导出", "准备开始导出数据...", 0);
            // EditorUtility.ClearProgressBar();
        }

        void SimpleExportScene()
        {
            //----------time------
            var frameCount = Mathf.Ceil(config.duration * config.frameRate);
            var deltaTime = 1.0f / config.frameRate;
            //---------
            var oldCam = SceneConfig.currentCamera;
            config.Prepare();
            if (currentCamera != null)
                SceneConfig.currentCamera = currentCamera;

            EditorUtility.DisplayCancelableProgressBar("快速导出", "准备开始导出数据...", 0);
            try
            {
                for (var i = 0; i < frameCount; i++)
                {
                    float time = i * deltaTime;
                    GlobalUtility.time = time;

                    UpdateSceneByTime(time);

                    config.AddFrame(true);
                    //Thread.Sleep(3);
                    if (EditorUtility.DisplayCancelableProgressBar("快速导出",
                        string.Format(message + "   " + "导出进度  {0}/{1}", (i + 1), frameCount),
                        (i + 1.0f) / frameCount) == true)
                    {
                        EditorUtility.ClearProgressBar();
                        Debug.LogWarning("手动取消了导出！");
                        return;
                    }
                }

                config.SaveTotFile();
                EditorUtility.ClearProgressBar();
                UpdateSceneByTime(0.0f);
            }
            catch (Exception e)
            {
                EditorUtility.ClearProgressBar();
                Console.WriteLine(e);
                throw;
            }

            SceneConfig.currentCamera = oldCam;
        }

        public void Run()
        {
            scripts = Object.FindObjectsOfType<ScriptBase>();
            if (scripts != null)
            {
                foreach (var scriptBase in scripts)
                {
                    scriptBase.BeginExport();
                }
            }


            config = Object.FindObjectOfType<SceneConfig>();
            if (config == null)
            {
                Debug.LogError("没有找到SceneConfig场景配置对象");
                return;
            }


            if (GetOutputFile() == false) return;


            var batchExportConfig = Object.FindObjectOfType<BatchExportConfig>();
            if (batchExportConfig != null)
            {
                BatchExportScene(batchExportConfig);
            }
            else
            {
                SimpleExportScene();
            }


            if (scripts != null)
            {
                foreach (var scriptBase in scripts)
                {
                    scriptBase.EndExport();
                }
            }

            Debug.Log("快速导出完成");
        }


        [MenuItem("FxEditor/快速的导出  %e")]
        public static void OnQuickExport()
        {
            var qc = new QuickExport();
            qc.Run();
        }
    }
}