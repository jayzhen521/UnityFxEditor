using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace Packages.FxEditor
{
    public class Timeline : ScriptBase
    {
        public List<TimelineClip> clips=new List<TimelineClip>();

        private void Start()
        {
            BeginExport();
            
        }

        public override void BeginExport()
        {
            foreach (var timelineClip in clips)
            {
                if (timelineClip.rootObject == null) continue;
                float timeScale = ComputeAnimationDuration(timelineClip.rootObject,0)/timelineClip.duration;
                
                var animatoies = timelineClip.rootObject.GetComponentsInChildren<Animator>();
                timelineClip.clipTimeRatio = timeScale;
                
            }
            
            var scripts = UnityEngine.Object.FindObjectsOfType<ScriptBase>();
            foreach (var scriptBase in scripts)
            {
                if(scriptBase is Timeline)continue;
                scriptBase.BeginExport();
            }
            base.BeginExport();
        }

        public void MyUpdate()
        {
            updateCamera();
            updateAnimation();
        }

        public override void UpdateAnimation()
        {
            MyUpdate();
            Debug.Log("uuuuuuuuuuu");
        }

        public override void EndExport()
        {
            var scripts = UnityEngine.Object.FindObjectsOfType<ScriptBase>();
            foreach (var scriptBase in scripts)
            {
                if(scriptBase is Timeline)continue;
                scriptBase.EndExport();
            }
        }

        private void Update()
        {
            MyUpdate();
        }

        float ComputeAnimationDuration(GameObject obj, float time)
        {
            var ams = obj.GetComponentsInChildren<Animator>();
            float d = 0;
            foreach (var animator in ams)
            {
                animator.enabled = false;
                var animationClips = AnimationUtility.GetAnimationClips(animator.gameObject);
                if (animationClips == null) continue;
                
                foreach (var animationClip in animationClips)
                {
                    d = Mathf.Max(animationClip.length,d);
                    animationClip.SampleAnimation(animator.gameObject,time);
                }
            }
            
            
            
            return d;
        }
        void updateAnimation()
        {
            
            
            float starttime = 0;
            for(var i=0;i<clips.Count;i++)
            //foreach (var timelineClip in clips)
            {
                var timelineClip = clips[i];

                float endtime =starttime+timelineClip.duration;
                float time = GlobalUtility.time - starttime;
                
                if (timelineClip.rootObject == null||GlobalUtility.time<starttime||GlobalUtility.time>endtime)
                {
                    starttime = endtime;    
                    continue;
                }

                float t = time*timelineClip.clipTimeRatio;
                
                
                ComputeAnimationDuration(timelineClip.rootObject,t);


                var scripts = UnityEngine.Object.FindObjectsOfType<ScriptBase>();
                foreach (var scriptBase in scripts)
                {
                    if(scriptBase is Timeline)continue;
                    scriptBase.UpdateAnimation();
                }
                starttime = endtime;    
            }
        }
        
        void updateCamera()
        {
            foreach (var timelineClip in clips)
            {
                if (timelineClip.camera != null) timelineClip.camera.enabled = false;
            }
            //------------------------------------------------
            
            float time = 0;
            foreach (var timelineClip in clips)
            {
                if (timelineClip.camera == null) continue;
                
                float endtime = time + timelineClip.duration;
                if (GlobalUtility.time >= time && GlobalUtility.time < endtime)
                {
                    timelineClip.camera.enabled = true;
                    SceneConfig.currentCamera = timelineClip.camera;
                }
                time = endtime;
            }
        }


        void UpdateDuration()
        {
            var cfg = UnityEngine.Object.FindObjectOfType<SceneConfig>();
            if (cfg == null) return;
            
            float duraiton = 0.0f;
            foreach (var timelineClip in clips)
            {
                duraiton += timelineClip.duration;
            }

            cfg.duration = duraiton;
        }
        private void OnDrawGizmos()
        {
            UpdateDuration();
        }

        public GameObject GetRootObjectByTime(float t)
        {
            GameObject root = null;

            float time = 0;
            foreach (var clip in clips)
            {
                if (t >= time)
                {
                    root = clip.rootObject;
                }
                    
                time += clip.duration;
            }

            return root;
        }
    }
}