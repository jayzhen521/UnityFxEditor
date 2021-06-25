using System;
using System.Collections.Generic;
using UnityEngine;

namespace Packages.FxEditor.JsonTypes
{
    public class FxJsonFXWebp : FxJsonDataBase
    {
        public enum EffectTypes
        {
            [InspectorName("全局Fx")] Global = 1,
            [InspectorName("局部fx")] Local = 2
        }

        public enum GravityTypes
        {
            [InspectorName("上左")] TopLeft = 1,
            [InspectorName("上中")] TopCenter,
            [InspectorName("上右")] TopRight,
            [InspectorName("中左")] CenterLeft,
            [InspectorName("中")] Center,
            [InspectorName("中右")] CenterRight,
            [InspectorName("下左")] BottomLeft,
            [InspectorName("下中")] BottomCenter,
            [InspectorName("下右")] BottomRight
        }

        public enum PlayMode
        {
            [InspectorName("循环")]Loop=1
        }
        
        [Serializable]
        public class SoundData
        {
            public string file = "";
            public int start = 0;
            public int mode = 1;
            public int start_time = 0;
            public int end_time = 0;
            public float volume = 0;
        }


        public int duration = 0;
        public List<SoundData> sounds = new List<SoundData>();
        public EffectTypes type = EffectTypes.Local;
        public float scale = 1.0f;
        public float width = 5.0f;
        public float height = 5.0f;
        public int editorTime = 100;
        public GravityTypes gravity = GravityTypes.Center;
        public PlayMode play_mode = PlayMode.Loop;
        public int isGravity = 1;
        public bool EngineType = true;


        private void OnDrawGizmos()
        {
        }
    }
}