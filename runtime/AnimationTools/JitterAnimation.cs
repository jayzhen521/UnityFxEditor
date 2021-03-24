using System;
using System.Numerics;
using UnityEngine;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

namespace Packages.FxEditor
{
    public class JitterAnimation : MonoBehaviour
    {
        public int frequency = 1;
        public float power = 0.1f;

        private int count = 0;
        private Vector3 pos = Vector3.zero;
        private Random rnd = new Random();
        private void Start()
        {
            pos = transform.position;
            count = 0;
        }

        private void Update()
        {
            count++;
            if ((count % frequency) == 0)
            {
                float x = (float)rnd.NextDouble() * power-power*.5f;
                float y = (float)rnd.NextDouble() * power-power*.5f;
                transform.position = pos + (new Vector3(x, y, 0));
            }
        }
    }
}