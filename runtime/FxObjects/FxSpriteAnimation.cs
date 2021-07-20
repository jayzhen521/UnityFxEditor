using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace Packages.FxEditor
{
    public class FxSpriteAnimation : ScriptBase
    {
        
        public Texture2D texture = null;
        public int widthSlices = 1;
        public int heightSlices = 1;
        public float spriteIndex = 0;
        
        public int channelName = 0;
        public List<string> names = new List<string>();
        
        
        private void UpdateNames(Shader shader)
        {
            names.Clear();
            int c = shader.GetPropertyCount();
            for (int i = 0; i < c; i++)
            {
                var type = shader.GetPropertyType(i);
                if (type ==ShaderPropertyType.Texture)
                {
                    var name = shader.GetPropertyName(i);
                    names.Add(name);
                }
            }
        }

        void UpdateUI()
        {
            var shader = GetComponent<Renderer>().sharedMaterial.shader;
            UpdateNames(shader);
        }

        public void UpdateTextureCoord()
        {
            if (texture == null)
            {
                return;
            }
            
            var material =GlobalUtility.GetMaterial(GetComponent<Renderer>());
            var name = names[channelName];
            
            material.SetTexture(name,texture);
            var delta=new Vector2(1.0f/widthSlices,1.0f/heightSlices);
            
            
            var maxCount = widthSlices * heightSlices;
            var index = spriteIndex % maxCount;
            var x = Mathf.Floor( index % widthSlices)*delta.x;
            var y = Mathf.Floor(index/widthSlices);//delta.y);//(heightSlices-Mathf.Floor(spriteIndex / heightSlices))*delta.y;
            
            y = heightSlices - y-1;
            y *= delta.y;
            
            material.SetVector(name+"_ST",new Vector4(delta.x,delta.y,x,y));
            
            
        }

        private void Update()
        {
            UpdateTextureCoord();
            Debug.Log(spriteIndex);
        }

        private void OnDrawGizmos()
        {
            UpdateUI();
            UpdateTextureCoord();
        }

        public override void UpdateAnimation()
        {
            UpdateTextureCoord();
            
        }
        
    }
}