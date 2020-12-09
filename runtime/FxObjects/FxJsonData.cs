using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Object = System.Object;

namespace Packages.FxEditor
{
    // "duration": 1000,
    // "start_time": 0,
    // "path": "01/",
    // "edit_filter_id2": -1,
    // "edit_filter_id1": -1,
    // "end_time": 0,
    // "type": 2


    [Serializable]
    public class EffectItem
    {
        public int duration = 0;
        public int start_time=0;
        public int end_time=0;
        public int edit_filter_id1 = -1;
        public int edit_filter_id2 = -1;
        public int type = -2;
        public string path = "00";
    }

    public enum PlayMode
    {
        Once,//===0
        Loop,//===1
        PingPong
    }
    
    public class FxJsonData : MonoBehaviour
    {
        //-----------Json数据--------------
        public string name = "";
        public float totalDuration = 0.0f;
        public PlayMode play_mode = PlayMode.Once;
        
        public List<EffectItem> effectList = new List<EffectItem>();


     
        
        public string musicConfig = "999";
        public int isTransRand = 0;
        public int backgroundColor = 3;
        public int clipNum = 13;
        public int translationType = 2;
        public int moveType = 2;
        [Tooltip("新引擎标志")]
        public bool EngineType = true;

        public List<int> clip_duration = new List<int>();
        
        [Header("用户自定义数据")]
        //--------------------------------------------
        public string userdata = "999";
        public int userInteger = 9999;
        public float userFloat = 9999.0f;
        public Boolean userBoolean =true;
        //--------------------------------------------


        public void SaveTo(string filepath)
        {
            var text=JsonUtility.ToJson(this);
            File.WriteAllText(filepath,text);
        }


        void UpdateTimelineData()
        {
            clip_duration.Clear();
            
            var timeline = UnityEngine.Object.FindObjectOfType<Timeline>();
            if (timeline == null) return;
            effectList.Clear();
            float starttime = 0;
            totalDuration = 0;
            for (int i = 0; i < timeline.clips.Count; i++)
            {
                var clip = timeline.clips[i];
                
                var fx=new EffectItem();
                //fx.path = string.Format("{0:00}",i);
                fx.duration = (int) (clip.duration * 1000);
                fx.start_time=(int)(starttime*1000);
                fx.end_time=(int)((starttime+clip.duration)*1000);
                fx.type = (int) clip.type;
                effectList.Add(fx);
                starttime += clip.duration;


                if (clip.type == ClipType.PictureInPicture)
                {
                    clip_duration.Add(fx.duration);
                    totalDuration += fx.duration;
                }
                
            }

            clipNum = clip_duration.Count;
        }

        private void OnDrawGizmos()
        {
            UpdateTimelineData();
        }
    }
}