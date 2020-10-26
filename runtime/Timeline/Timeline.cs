using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Packages.FxEditor
{
    public class Timeline : MonoBehaviour
    {
        public List<TimelineClip> clips=new List<TimelineClip>();

        private void Start()
        {
            
            foreach (var timelineClip in clips)
            {
                if (timelineClip.rootObject == null) continue;
                float timeScale = ComputeAnimationDuration(timelineClip.rootObject,timelineClip.duration);
                Debug.Log(timeScale);
                var animatoies = timelineClip.rootObject.GetComponentsInChildren<Animator>();
                foreach (var animator in animatoies)
                {
                    animator.enabled = false;
                    animator.speed = timeScale;
                }
            }
        }

        private void Update()
        {
            updateCamera();
            updateAnimation();
        }

        float ComputeAnimationDuration(GameObject obj, float time)
        {
            var ams = obj.GetComponentsInChildren<Animator>();
            float d = 0;
            foreach (var animator in ams)
            {
                var animationClips = AnimationUtility.GetAnimationClips(animator.gameObject);
            
                foreach (var animationClip in animationClips)
                {
                    d = Mathf.Max(animationClip.length,d);
                }    
            }
            

            if (d <= 0.001f) return 1;
            return d/time;
        }
        void updateAnimation()
        {
            float time = 0;
            foreach (var timelineClip in clips)
            {
                float endtime = time+timelineClip.duration;

                if (timelineClip.rootObject == null)
                {
                    time = endtime;
                    continue;
                }
                
                var animatoies = timelineClip.rootObject.GetComponentsInChildren<Animator>();
                foreach (var animator in animatoies)
                {
                    if (Time.time >= time && Time.time < endtime)
                    {
                        animator.enabled = true;
                        
                    }
                    else
                    {
                        animator.enabled = false;
                    }
                }
                
                time = endtime;
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
                if (Time.time >= time && Time.time < endtime)
                {
                    timelineClip.camera.enabled = true;
                    SceneConfig.currentCamera = timelineClip.camera;
                }
                time = endtime;
            }
        }
    }
}