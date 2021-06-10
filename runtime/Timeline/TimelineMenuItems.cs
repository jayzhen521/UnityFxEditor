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
                var obj = new GameObject("Timeline");
                TL = obj.AddComponent<Timeline>();
            }

            string text = GUIUtility.systemCopyBuffer;

            string[] lines = text.Split('\n');


            TL.clips.Clear();
            int i = 0;
            foreach (var line in lines)
            {
                var c = new TimelineClip();
                string[] dg = line.Split(' ');

                float a = int.Parse(dg[0]);

                c.duration = a * 0.001f;
                i += 2;

                TL.clips.Add(c);
                TL.clips.Add(new TimelineClip());
            }


            //-------set default value of type
            {
                for (i = 0; i < TL.clips.Count; i++)
                {
                    var c = TL.clips[i];
                    if (i % 2 == 0)
                    {
                        c.type = ClipType.PictureInPicture;
                    }
                    else
                    {
                        c.type = ClipType.Translator;
                    }
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
                EditorUtility.DisplayDialog("error", "没有创建RandomClips对象", "确定");
                return;
            }

            randomData.Fill();
        }


        [MenuItem("FxEditor/时间线工具/Timeline排序")]
        public static void OnSetTimeLine()
        {
            bool configed = false;
            var ts = Object.FindObjectsOfType<Timeline>();


            Timeline tl = null;
            GameObject root = null;
            if (ts.Length == 1) tl = ts[0];


            if (tl != null)
            {
                var count = tl.clips.Count;
                var transforms = Object.FindObjectsOfType<Transform>();
                Transform transform = null;
                var tm = 0;
                foreach (var transform1 in transforms)
                {
                    if (transform1.childCount == count)
                    {
                        tm++;
                        transform = transform1;
                    }
                }

                if (tm == 1)
                {
                    root = transform.gameObject;
                }
            }

            configed = (tl != null && root != null);

            GameObject timelineSortGameObject = new GameObject();
            var tts = timelineSortGameObject.AddComponent<TimelineClipsByLine>();

            if (configed)
            {
                tts.timelineObject = tl;
                tts.rootOfClips = root;
                EditorUtility.DisplayDialog("Timeline", "已经自动配置数据", "关闭");
            }
            else
            {
                EditorUtility.DisplayDialog("Timeline", "没有自动配置数据", "关闭");
            }
        }


        [MenuItem("FxEditor/时间线工具/自动填充类型")]
        public static void OnAutoSetTypes()
        {
            var ts = Object.FindObjectsOfType<Timeline>();

            Timeline timeline = null;

            if (Selection.objects.Length == 1)
            {
                GameObject obj = Selection.activeObject as GameObject;
                if (obj != null)
                {
                    timeline = obj.GetComponent<Timeline>();
                }
            }

            if (Selection.objects.Length == 1)
            {
                timeline = ts[0];
            }
            else
            {
                Debug.LogError("场景中不止一个Timeline对象，请选择Timeline对象");
                return;
            }
            
            
            
        }
    }
}