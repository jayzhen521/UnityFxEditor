using System;

using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using Object = System.Object;
using UnityEditor;
using PackageInfo = UnityEditor.PackageInfo;

namespace Packages.FxEditor
{
    public class SceneConfig : MonoBehaviour
    {
        private Exporter _exporter = null;//new Exporter();
        private bool isSaved = false;
        private UIRenderer _uiRenderer=new UIRenderer();

        public bool showCanvasUI = true;
        public int frameRate = 120;
        public float duration=2;
        public string outputPath = null;
        
        
        public static Camera currentCamera=null;

        public bool forExport = false;
        
        private void Start()
        {
            //isExporting = true;
            Time.captureFramerate = frameRate;
            outputPath="/Volumes/Workspace/Projects/HLVideoFx/source/PlatformsApp/testdata/fx/test.videofx";
            currentCamera=Camera.main;
            //currentCamera=SceneConfig.currentCamera;
            //_exporter=new Exporter();
        }

        public void Prepare()
        {
            //currentCamera=Camera.main;
            //outputPath="/Volumes/Workspace/Projects/HLVideoFx/source/PlatformsApp/testdata/fx/test.videofx";
        }

        private void Update()
        {
            if (!forExport) return;
            
            
            if (Time.time >= duration)
            {
                if (Application.isPlaying||outputPath==null||outputPath=="")
                {
                    //if(!isSaved)_exporter.SaveToFile("/Volumes/TmpSpace/testdata/test.videofx");
                    //if (!isSaved) _exporter.SaveToFile(outputPath);
                    if (!isSaved) SaveTotFile();
                    isSaved = true;
                    GlobalConfig.isPlaying = false;
                    EditorApplication.ExecuteMenuItem("Edit/Play");
                    
                }
            }
            else
            {
                AddFrame();
            }
        }

        public void AddFrame()
        {
            if(_exporter==null)_exporter=new Exporter();
            
            _exporter.AddFrame();
        }

        public void SaveTotFile()
        {
            _exporter.SaveToFile(outputPath);
        }
        private void OnDrawGizmos()
        {
            
            //forExport = false;
            
            if(_uiRenderer==null)_uiRenderer=new UIRenderer();
            //------timeline ui---
            {
                var tl = FindObjectOfType<Timeline>();
                if (tl != null)
                {
                    foreach(var c in tl.clips)
                    {
                        if(c.camera==null)continue;
                        _uiRenderer.DrawCameraBound(c.camera,Color.white);
                    }
                }
            }
            //return;
            //if (Application.isPlaying) return;
            
            GlobalUtility.UpdateCanvasNodeOrder();
            if(showCanvasUI)_uiRenderer.DrawCanvasUIS();
            //_uiRenderer.DrawString(Camera.main.transform.position,"hello" );
        }
    }
}