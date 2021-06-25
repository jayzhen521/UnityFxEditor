using System;
using System.Collections.Generic;
using UnityEngine;

namespace Packages.FxEditor
{
    public class AnimationNode
    {
        public float start=0.0f;
        public float end=0.0f;
        public GameObject obj = null;
        public Vector3 pos=new Vector3(0,0,0);
    }
    public class ProceduralAnimation : MonoBehaviour
    {
        [Header("Time")] 
        public float startTime = 0.0f;
        public float effectDuration = 5;
        
        private Animator[] _animations = null;

        private void Start()
        {
             _animations= gameObject.GetComponentsInChildren<Animator>();
             foreach (var animation1 in _animations)
             {
                 animation1.enabled = false;
             }
        }

        private void Update()
        {
            if (_animations == null || _animations.Length <= 0) return;
            
            float dtime = effectDuration / _animations.Length;
            float t = GlobalUtility.time - startTime;
            if (t < 0) return;

            foreach (var animation1 in _animations)
            {
                var ap = animation1.gameObject.GetComponent<AnimationProperty>();
                if (ap == null) continue;
                if (animation1.enabled == false&&(ap.OrderID*dtime<t))
                {
                    animation1.enabled = true;
                }
            }
        }
    }
}