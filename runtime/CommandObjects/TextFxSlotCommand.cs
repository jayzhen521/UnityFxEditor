using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Packages.FxEditor
{
    public class TextFxSlotCommand:CommandObjectBase
    {
        private const int AlignmentLeft = 0;
        private const int AlignmentCenter = 1;
        private const int AlignmentRight = 2;
        
        
        private const int TextAnimationTypeUnknown = 0;
        private const int TextAnimationTypeSequence = 1;
        private const int TextAnimationTypeRandomize = 2;
        
        
        
        private int id = 0;
        private float size = 1.0f;
        private int textAlignment = 0;
        private Matrix4x4 matrixVP = new Matrix4x4();
        private Matrix4x4 matirxObjectToWorld = new Matrix4x4();
        
        private float duration = 0.0f;

        
        private List<AnimationClipObject> animationClips = new List<AnimationClipObject>();
        private int textAnimationType = TextAnimationTypeUnknown;
        
        
        
        
        
        
        public TextFxSlotCommand(Camera cam, GameObject gameObject, Exporter exporter)
        {
            ObjectType = CommandTypeTextFxSlot;
            //----------------------------------
            var textobj = gameObject.GetComponent<TextFx>();
            id = textobj.soltID;
            size = textobj.size;
            switch (textobj.align)
            {
                case TextAlignment.Left :
                    textAlignment = AlignmentLeft;
                    break;
                case TextAlignment.Center:
                    textAlignment = AlignmentCenter;
                    break;
                case TextAlignment.Right:
                    textAlignment = AlignmentRight;
                    break;
            }
            
            var viewMatrix = cam.worldToCameraMatrix;
            var projectMatrix = cam.projectionMatrix;
            matrixVP =   projectMatrix*viewMatrix;
            matirxObjectToWorld = gameObject.transform.localToWorldMatrix;
            
            //----------time-----------
            duration = textobj.effectDuration;

            var clip = exporter.GetObject(textobj.clip) as AnimationClipObject;
            if (clip != null)
            {
                animationClips.Add(clip);
            }
            
            switch (textobj.animationType)
            {
                case TextAnimationType.Sequence:
                    textAnimationType = TextAnimationTypeSequence;
                    break;
                case TextAnimationType.Randomize:
                    textAnimationType = TextAnimationTypeRandomize;
                    break;
                
            }
        }

        protected override void Write(Stream stream)
        {
            Write(stream,id);
            Write(stream,size);
            Write(stream,textAlignment);
            
            Write(stream,matrixVP);
            Write(stream,matirxObjectToWorld);
            
            //-----------time-----------
            Write(stream,duration);
            
            //-----------animation-----------
            Write(stream,textAnimationType);
            int count = animationClips.Count;
            Write(stream,count);
            for(int i=0;i<count;i++)
                Write(stream,animationClips[i].ObjectID);
        }
    }
}