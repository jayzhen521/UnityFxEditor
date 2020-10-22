using System;
using UnityEngine;

namespace Packages.FxEditor
{
    
    [Serializable]
    public class TimelineClip
    {
        public Camera camera = null;
        public GameObject rootObject = null;
        public float duration = 1.0f;
    }
}