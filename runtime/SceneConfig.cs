using System;

using UnityEditor;
using UnityEngine;

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
            //currentCamera=Camera.main;
            currentCamera=SceneConfig.currentCamera;
        }


        private void Update()
        {
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
            //return;
            //if (Application.isPlaying) return;
            
            if(_uiRenderer==null)_uiRenderer=new UIRenderer();
            
            GlobalUtility.UpdateCanvasNodeOrder();
            if(showCanvasUI)_uiRenderer.DrawCanvasUIS();
            //_uiRenderer.DrawString(Camera.main.transform.position,"hello" );
            
        }
    }
}