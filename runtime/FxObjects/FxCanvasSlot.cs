using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;

namespace Packages.FxEditor
{
    public class FxCanvasSlot : MonoBehaviour
    {
        public int channelName = 0;
        public List<string> names = new List<string>();
        public FxCanvasObject canvas = null;
        
        private List<Texture> _textures=new List<Texture>();
        private void UpdateNames(Shader shader)
        {
            names.Clear();
            _textures.Clear();
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


        void UpdateTexture()
        {
            if (canvas == null) return;
            if (channelName >= names.Count) return;
            
            var name = names[channelName];
            var mat = GetComponent<Renderer>().sharedMaterial;
            var tex = canvas.GetRenderTexture();
            mat.SetTexture(name,tex);
        }

        private void Update()
        {
            UpdateTexture();
        }

        private void OnDrawGizmos()
        {
            
            if (GlobalConfig.isPlaying)
            {
                Debug.Log("playing");
                return;
            }
            
            
            var mat = GlobalUtility.GetObjectMaterial(this.gameObject);
            // if(mat==null)mat=GetComponent<Renderer>().material;
            // if (mat == null) return;
            UpdateNames(mat.shader);
            UpdateTexture();
        }
    }
}
