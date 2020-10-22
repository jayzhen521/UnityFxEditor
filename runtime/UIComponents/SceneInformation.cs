using System;
using UnityEditor;
using UnityEngine;

namespace Packages.FxEditor
{
    public class SceneInformation: EditorWindow
    {

        
        public SceneInformation()
        {
            var title=new GUIContent("场景信息");
            titleContent = title;
        }

        private void OnGUI()
        {
            
        }
    }
}