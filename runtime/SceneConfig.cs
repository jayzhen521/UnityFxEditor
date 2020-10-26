using System;

using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace Packages.FxEditor
{
    public class SceneConfig : MonoBehaviour
    {
        private readonly Exporter _exporter = new Exporter();
        private bool isSaved = false;
        private UIRenderer _uiRenderer=new UIRenderer();

        public bool showCanvasUI = true;
        public int frameRate = 120;
        public float duration=2;
        public string outputPath = null;
        
        
        public static Camera currentCamera=null;
        private void Start()
        {
            GlobalConfig.isPlaying = true;
            Time.captureFramerate = frameRate;
            outputPath="/Volumes/Workspace/Projects/HLVideoFx/source/PlatformsApp/testdata/fx/test.videofx";
            currentCamera=Camera.main;
            //currentCamera=SceneConfig.currentCamera;
        }


        private void Update()
        {
            return;
            if (Time.time >= duration)
            {
                if (Application.isPlaying||outputPath==null||outputPath=="")
                {
                    //if(!isSaved)_exporter.SaveToFile("/Volumes/TmpSpace/testdata/test.videofx");
                    //if (!isSaved) _exporter.SaveToFile(outputPath);
                    if (!isSaved) _exporter.SaveToFile(outputPath);
                    isSaved = true;
                    GlobalConfig.isPlaying = false;
                    EditorApplication.ExecuteMenuItem("Edit/Play");
                    
                }
            }
            else
            {
                _exporter.AddFrame();
            }
        }

        private void OnDrawGizmos()
        {
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