using System;
using System.Numerics;
using System.Runtime.InteropServices;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

namespace Packages.FxEditor
{
    public class JitterAnimation : ScriptBase
    {
        public int frequency = 1;
        public float powerT = 0.1f;
        public float powerR = 0.1f;
        public float powerS = 0.1f;
        public int seed = DateTime.Now.Millisecond;

        private int count = 0;
        private Vector3 pos = Vector3.zero;
        private Vector3 sc = Vector3.zero;
        private UnityEngine.Quaternion rot=UnityEngine.Quaternion.identity;
        private Random rnd = null;
        private void Start()
        {
            BeginExport();
        }

        private void Update()
        {
            UpdateAnimation();
        }

        public override void BeginExport()
        {
            rot = transform.rotation;
            rnd = new Random(seed);
            pos = transform.position;
            sc = transform.localScale;
            count = 0;
        }

        public override void UpdateAnimation()
        {
            count++;
            if ((count % frequency) == 0)
            {
                float x = (float)rnd.NextDouble() * powerT-powerT*.5f;
                float y = (float)rnd.NextDouble() * powerT-powerT*.5f;
                transform.position = pos + (new Vector3(x, y, 0));
                
                x = (float)rnd.NextDouble() * powerS-powerS*.5f;
                y = (float)rnd.NextDouble() * powerS-powerS*.5f;
                transform.localScale = sc + (new Vector3(x, x, 0));
                 
                 
                x = (float)rnd.NextDouble() * powerR-powerR*.5f;
                UnityEngine.Quaternion q = UnityEngine.Quaternion.AngleAxis(x, Vector3.forward);
                transform.rotation =rot*q;
            }
        }

        public override void EndExport()
        {
            base.EndExport();
        }
    }
}