using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Packages.FxEditor
{
    [Serializable]
    public class TagWeight
    {
        public string tag = "";
        public float weight = 0.0f;

        private List<string> guids = new List<string>();


        

        public TagWeight(string t, float v)
        {
            tag = t;
            weight = v;
        }

        public void AddGUID(string gid)
        {
            guids.Add(gid);
            
        }

        public string GetGUID(int i)
        {
            return guids[i];
        }

        public int GetCount()
        {
            return guids.Count;
        }
    }
    
    
    public class RandomClips : MonoBehaviour
    {
        
        public string clipsPath = "";
        public List<TagWeight> tags = new List<TagWeight>();
        private string lastPath = "";

        private int[] fillCounts =null;
        private Timeline _timeline = null;
        private float lastPosX = 0.0f;
        
        private TagWeight AddTag(string tag)
        {
            foreach (var tagWeight in tags)
            {
                if (tagWeight.tag == tag)
                {
                    return tagWeight;
                }
            }

            var r = new TagWeight(tag, 0.0f);
            tags.Add(r);


            return r;
        }
        private void OnDrawGizmos()
        {
            
            if (lastPath == clipsPath) return;
            lastPath = clipsPath;
            Debug.Log("update tags!");
            tags.Clear();
            
            
            var assets = AssetDatabase.FindAssets("*",new []{clipsPath});
            foreach (var asset in assets)
            {
                var lbs = AssetDatabase.GetLabels( AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(asset)));
                foreach (var lb in lbs)
                {
                    var wt=AddTag(lb);
                    wt.AddGUID(asset);
                }
            }
        }


        private void ClearClips()
        {
            foreach (var timelineClip in _timeline.clips)
            {
                if (timelineClip.rootObject == null) continue;
                UnityEngine.Object.DestroyImmediate(timelineClip.rootObject);
            }
        }
        private void ComputeFillCount()
        {
            float sum = 0;
            foreach (var tag in tags)
            {
                sum += tag.weight;
            }

            int count = _timeline.clips.Count;
            
            fillCounts = new int[tags.Count];
            for (int i = 0; i < tags.Count; i++)
            {
                fillCounts[i] = (int)Mathf.Ceil(count * tags[i].weight / sum);
                Debug.Log(fillCounts[i]);
            }

            
        }

        private void AddObjectToScene(int i,string id)
        {
            var path = AssetDatabase.GUIDToAssetPath(id);
            var obj = AssetDatabase.LoadMainAssetAtPath(path) as GameObject;
            
            
            //var retobj=UnityEngine.Object.Instantiate(obj);
            var retobj = PrefabUtility.InstantiatePrefab(obj) as GameObject;

            
             var cam=retobj.GetComponent<Camera>();
            float h = cam.orthographicSize * 2;
            float w = h * cam.aspect;

            var nodeBound = GlobalUtility.GetGameObjectBounds(retobj);
            w = Math.Max(w, nodeBound.size.x);
            
            retobj.transform.position = new Vector3(lastPosX, 0, 0);
            
            lastPosX+=((w+1)*2);
            var clip = _timeline.clips[i];
            clip.rootObject = retobj;
            clip.camera = cam;//retobj.GetComponent<Camera>();
            

            //Debug.Log(bound.size.x);
        }
        public void Fill()
        {
            lastPosX = 0.0f;
            
            _timeline = UnityEngine.Object.FindObjectOfType<Timeline>();
            if (_timeline == null)
            {
                EditorUtility.DisplayDialog("error", "没有发现Timeline对象", "确定");
                return;
            }
            ClearClips();
            ComputeFillCount();

            var dt = DateTime.Now;
            var seed = dt.Ticks;
            System.Random rnd = new System.Random((int)(seed%100000));

            List<string> objectids = new List<string>();
            for (int i = 0; i < fillCounts.Length; i++)
            {
                var c = fillCounts[i];
                if (c == 0) continue;
                var tag = tags[i];
                var count = tag.GetCount();
                if (count <= 0) continue;
                for (int j = 0; j < c; j++)
                {
                    objectids.Add(tag.GetGUID(rnd.Next(count)));
                }
            }

            int index = 0;
            while (objectids.Count>0&&index<_timeline.clips.Count)
            {
                int i = rnd.Next(objectids.Count);
                var objid = objectids[i];
                AddObjectToScene(index,objid);
                index++;
                objectids.RemoveAt(i);
            }
        }
    }
}