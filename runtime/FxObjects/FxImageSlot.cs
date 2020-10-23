using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Packages.FxEditor
{
    public class FxImageSlot : MonoBehaviour
    {
        public int slotID=0;
        public int width = 32;
        public int height = 32;
        public int channelName = 0;
        public List<string> names = new List<string>();
        
        private void UpdateNames(Shader shader)
        {
            names.Clear();
            int c = shader.GetPropertyCount();
            for (int i = 0; i < c; i++)
            {
                var type = shader.GetPropertyType(i);
                if (type == ShaderPropertyType.Texture)
                {
                    var name = shader.GetPropertyName(i);
                    names.Add(name);
                }
            }
        }
        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;
            
            
            var mat = GetComponent<Renderer>().sharedMaterial;
            UpdateNames(mat.shader);
            mat.SetTexture(names[channelName],Texture2D.redTexture);
        }
    }
}