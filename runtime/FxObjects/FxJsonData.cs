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
        public string path = "";
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
            var timeline = UnityEngine.Object.FindObjectOfType<Timeline>();
            if (timeline == null) return;
            effectList.Clear();
            int starttime = 0;
            for (int i = 0; i < timeline.clips.Count; i++)
            {
                var clip = timeline.clips[i];
                
                var fx=new EffectItem();
                fx.path = string.Format("{0:00}",i);
                effectList.Add(fx);
            }
        }

        private void OnDrawGizmos()
        {
            UpdateTimelineData();
        }
    }
}