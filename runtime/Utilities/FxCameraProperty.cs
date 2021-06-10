using System;
using UnityEditor;
using UnityEngine;

namespace Packages.FxEditor
{
    public class FxCameraProperty : MonoBehaviour
    {
        public enum CameraAspectTypes// ratio=0.0001
        {
            [InspectorName("自定义")]
            None=0,
            
            [InspectorName("1:1")]
            WH_1x1=10000,
            
            [InspectorName("4:3")]
            WH_4x3=13333,
            
            [InspectorName("3:4")]
            WH_3x4=7500,
            
            [InspectorName("16:9")]
            WH_16x9=17777,
            
            [InspectorName("9:16")]
            WH_9x16=5625
        }

        
        [Header("屏幕机比例")]
        public CameraAspectTypes aspect = CameraAspectTypes.WH_16x9;
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


            if (aspect == CameraAspectTypes.None)
            {
                cam.aspect = (float)width / height;    
            }
            else
            {
                cam.aspect = ((int) aspect) * 0.0001f;
            }
            
        }
    }
}