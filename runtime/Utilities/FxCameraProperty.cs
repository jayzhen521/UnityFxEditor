using System;
using UnityEditor;
using UnityEngine;

namespace Packages.FxEditor
{
    public class FxCameraProperty : MonoBehaviour
    {
        [Header("屏幕机比例")]
        public int width = 9;
        public int height = 16;

        [MenuItem("FxEditor/创建物体/添加相机属性")]
        public static void OnAddCameraProperty()
        {
            Debug.Assert(Selection.objects.Length>0,"没有选择摄像机物体");
            foreach (var o in Selection.objects)
            {

                var gameObject = o as GameObject;
                if (o==null) continue;
                var cam = gameObject.GetComponent<Camera>();
                if (cam == null)
                {
                    Debug.LogWarning(string.Format("{0}物体不是摄像机物体",o.name));
                    continue;
                }

                if (gameObject.GetComponent<FxCameraProperty>() == null)
                {
                    gameObject.AddComponent<FxCameraProperty>();
                }
            }
        }


        
        private void OnDrawGizmos()
        {
            var cam = gameObject.GetComponent<Camera>();
            if (cam == null)
            {
                Debug.LogError(string.Format("{0}物体不是摄像机物体而添加了FxCameraProperty对象!!!"));
                return;
            }


            cam.aspect = (float)width / height;
        }
    }
}