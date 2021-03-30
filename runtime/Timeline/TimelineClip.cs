using System;
using UnityEngine;

namespace Packages.FxEditor
{
    public enum ClipType
    {
        Translator=2,
        PictureInPicture=5
    }
    
    [Serializable]
    public class TimelineClip
    {
        public Camera camera = null;
        public GameObject rootObject = null;
        public float duration = 1.0f;
        public ClipType type = ClipType.PictureInPicture;
        public float clipTimeRatio = 0.0f;
        
        
    }
    
    
    
}