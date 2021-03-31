using UnityEditor;
using UnityEngine;

namespace Packages.FxEditor
{
    public class TimelineMenuItems
    {
        [MenuItem("FxEditor/时间线工具/时间线")]
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

        [MenuItem("FxEditor/时间线工具/创建随机填充")]
        public static void OnCreateRandomTimeline()
        {
            var tl = UnityEngine.Object.FindObjectOfType<Timeline>();
            if (tl == null) OnCreateTimeline();
            tl = UnityEngine.Object.FindObjectOfType<Timeline>();
            tl.gameObject.AddComponent<RandomClips>();
        }
        
        [MenuItem("FxEditor/时间线工具/创建时间轴从剪贴板")]
        public static void OnCreateTLFromPast()
        {
            var TL = Object.FindObjectOfType<Timeline>();
            if (TL == null)
            {
                var obj=new GameObject("Timeline");
                TL=obj.AddComponent<Timeline>();

            }
            string text = GUIUtility.systemCopyBuffer;
            
            string[] lines = text.Split('\n');
            
            TL.clips.Clear();
            
            int i = 0;
            foreach (var line in lines)
            {
                var c=new TimelineClip();
                string[] dg= line.Split(' ');
                
                float a = int.Parse(dg[0]);
                    
                c.duration = a*0.001f;
                i += 2;
                
                TL.clips.Add(c);
                TL.clips.Add(new TimelineClip());
            }
            
            
            //-------
            for (var i = 0; i < TL.clips.Count; i++)
            {
                var c = TL.clips[i];
                if (i % 2 == 0)
                {
                    c.type = ClipType.PictureInPicture;
                }
                else
                {
                    c.type = ClipType.PictureInPicture;
                }
            }
            Debug.Log(text);
        }


        [MenuItem("FxEditor/时间线工具/随机填充")]
        public static void OnRandomClips()
        {
            var randomData = UnityEngine.Object.FindObjectOfType<RandomClips>();
            if (randomData == null)
            {
                EditorUtility.DisplayDialog("error", "没有创建RandomClips对象","确定");
                return;
            }
            
            randomData.Fill();
        }
    }
}