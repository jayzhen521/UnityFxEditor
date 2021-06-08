using System;
using UnityEngine;
using System.Collections.Generic;

namespace Packages.FxEditor
{
    public class TimelineClipsByLine : MonoBehaviour
    {
        public Timeline timelineObject = null;
        public GameObject rootOfClips = null;
        void UpdateTimelineClips()
        {
            List<Transform> ts = new List<Transform>();
            
            var count = rootOfClips.transform.childCount;
            for (var i = 0; i < count; i++)
            {
                ts.Add( rootOfClips.transform.GetChild(i));
            }

            ts.Sort(ByX);

            for (var i = 0; i < timelineObject.clips.Count&&i<ts.Count; i++)
            {
                var clip = timelineObject.clips[i];
                var obj = ts[i].gameObject;
                if (obj.GetComponent<Camera>() == null) continue;
                clip.rootObject = obj;
                clip.camera = obj.GetComponent<Camera>();
            
            }


        }
        private void OnDrawGizmos()
        {
            if (timelineObject == null||rootOfClips==null) return;
            UpdateTimelineClips();
        }

        public int ByX(Transform a, Transform b)
        {
            if (a.position.x > b.position.x) 
                return 1;
            else 
                return -1;
            
            return 0;
        }
    }
}