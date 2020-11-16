using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Packages.FxEditor
{
    public class AnimationClipProperty
    {
        public Int64 objectID = 0;
        public string name = "";
        public int type = ShaderParameter.ParameterTypeFloat;

        public AnimationClipProperty(string _name, int typ)
        {
            name = _name;
            type = typ;
        }
    }

    public class AnimationClipObject : DataObjectBase
    {
        public const int AnimationTypeUnknown = 0;
        public const int AnimationTypeCurve = 1;
        public const int AnimationTypeSampler = 2;

        private AnimationClip animationClip = null;
        private Shader shader = null;
        private string matrix = "";


        MaterialPropertyBlock block = new MaterialPropertyBlock();


        //------------data block----------------
        public int animationType = AnimationTypeSampler;
        public float duration = 0.0f;
        public float frameRate = 240.0f;
        public int framesCount = 0;
        public List<AnimationClipProperty> animationDataProperies = new List<AnimationClipProperty>();
        //-------------------------
        public List<Matrix4x4> glyphMatrix=new List<Matrix4x4>();
        public List<Vector4> _Color=new List<Vector4>();
        

        //------------------------------------------------------

        public AnimationClipObject(AnimationClip clip)
        {
            ObjectType = ObjectTypeAnimationClip;
            //--------------------------
            animationClip = clip;
            duration = clip.length;
            framesCount = (int) Math.Floor(duration * frameRate);
            SamplerData();
        }


        public void SamplerData()
        {
            
            animationDataProperies.Add(new AnimationClipProperty("glyphMatrix",
                ShaderParameter.ParameterTypeMatrix4x4));
            animationDataProperies.Add(new AnimationClipProperty("_Color", ShaderParameter.ParameterTypeFloat4));

            var binds = AnimationUtility.GetCurveBindings(animationClip);
            
            
            GameObject gameObject=new GameObject("TMP");
            gameObject.tag = "EditorOnly";
            
            var renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material=new Material(Shader.Find("HLFx/TextureColorMask"));
            renderer.GetPropertyBlock(block);
            
            for (int i = 0; i <= framesCount; i++)
            {
                
                float time = duration * i / framesCount;
                animationClip.SampleAnimation(gameObject,time);
                renderer.GetPropertyBlock(block);
                
                
                glyphMatrix.Add(gameObject.transform.localToWorldMatrix);
                
                var color = block.GetColor("_Color");
                _Color.Add(new Vector4(color.r,color.g,color.g,color.a));
            }
        }

        public override void Write(Stream stream)
        {
            
            Write(stream, duration);
            Write(stream, frameRate);
            Write(stream, animationType);
            Write(stream, framesCount);
            
            //-------animation data--------
            Write(stream,animationDataProperies.Count);
            foreach (var p in animationDataProperies)
            {
                Write(stream,p.objectID);
                Write(stream,p.name);
                Write(stream,p.type);
            }
            Write(stream,glyphMatrix.ToArray());
            Write(stream,_Color.ToArray());
        }
    }
}