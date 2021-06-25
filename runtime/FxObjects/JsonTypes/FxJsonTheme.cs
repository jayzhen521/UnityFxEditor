using System;
using System.Collections.Generic;
using UnityEngine;

namespace Packages.FxEditor.JsonTypes
{
    public class FxJsonTheme : FxJsonDataBase
    {
        public enum ScreenAspect
        {
            [InspectorName("16:9")]Screen_16x9=1,
            [InspectorName("4:3")]Screen_4x3,
            [InspectorName("1:1")]Screen_1x1,
            [InspectorName("3:4")]Screen_3x4,
            [InspectorName("9:16")]Screen_9x16
        }

        public class MusicConfig
        {
            public enum MusicType
            {
                [InspectorName("应用内置音乐")]Default=0,
                [InspectorName("本季音乐")]Local,
                [InspectorName("在线音乐")]Online
            }
            
            
            
            public string path = "";
            public string en = "";
            public string zh = "";
            public string musicId = "";
            public int trimStartTime = 0;
            public int trimEndTime = 0;
            public MusicType musicType = MusicType.Local;
            

        }


        public class EffectClip
        {
            public int duration = 0;
            public int start_time=0;
            public string path = "";
            public float text_wh_ration=0.4f;
            public Color text_bg_color=Color.black;
            public int end_time = 0;
            public int is_cover = 0;
            public Color text_color = Color.white;
            public int is_append_clip = 1;
            public int type = 3;
            public int is_water = 0;

        }
        public enum TranslationTypes
        {
            [InspectorName("介于片段1和片段2")]A=0,
            [InspectorName("重叠在片段1")]B,
            [InspectorName("重叠在片段2")]C,
            [InspectorName("从片段1尾部开始到片段2开头")]D
        }
        
        public int filterId = -1;
        public int totalDurtion = 0;
        public List<ScreenAspect> supportSizes = new List<ScreenAspect>();
        public MusicConfig musicConfig = new MusicConfig();
        public int isTransRand = 0;
        public int backgroundColor = 3;
        public int isClipPlay = 0;
        public int clipNum = 0;
        public List<int> clip_duration = new List<int>();
        public int musicTimeStamp = 0;
        public TranslationTypes translationType = TranslationTypes.A;
        public int moveType = 2;
        public bool EngineType = true;
        public List<EffectClip> effectList = new List<EffectClip>();
        
        
        
        private void OnDrawGizmos()
        {
            
            
        }
    }
}