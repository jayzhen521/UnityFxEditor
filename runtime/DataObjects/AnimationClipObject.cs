using System.IO;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;

namespace Packages.FxEditor
{
    public class AnimationClipObject:DataObjectBase
    {

        
        public const uint AnimationTypeCurve = 1;
        public const uint AnimationTypeSampler = 2;
        
        public float duration = 0.0f;
        public float frameReate = 120.0f;
        
        public uint animationType = AnimationTypeSampler;

        private AnimationClip animationClip = null;
        private Shader shader = null;
        MaterialPropertyBlock block=new MaterialPropertyBlock();
        
        public AnimationClipObject(AnimationClip clip,Shader s)
        {
            ObjectType = ObjectTypeAnimationClip;
            //--------------------------
            animationClip = clip;
            shader = s;
        }

        public void SamplerData(float fps)
        {
            GameObject gameObject=new GameObject("TMP");
            // var mesh=new Mesh();
            // var filter=gameObject.AddComponent<MeshFilter>();
            var renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material=new Material(Shader.Find(shader.name));
            
            animationClip.SampleAnimation(gameObject,0.5f);
            renderer.GetPropertyBlock(block);
            

        }

        public override void Write(Stream stream)
        {
            
        }
    }
}