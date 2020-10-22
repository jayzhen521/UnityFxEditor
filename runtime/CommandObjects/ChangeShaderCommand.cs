
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;


namespace Packages.FxEditor
{
    public class ChangeShaderCommand : CommandObjectBase
    {
        private ShaderObject _shaderObject;
        private readonly List<ShaderParameter> _parameters = new List<ShaderParameter>();
        private Matrix4x4 matirxObjectToWorld;
        private Matrix4x4 matrixVP;
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        
        
        public ChangeShaderCommand(Camera cam, GameObject gameObject, Exporter exporter)
        {
            ObjectType = CommandTypeChangeShader;
            //-------------------------------------------------
            var viewMatrix = cam.worldToCameraMatrix;
            var projectMatrix = cam.projectionMatrix;
            matrixVP =   projectMatrix*viewMatrix;
            matirxObjectToWorld = gameObject.transform.localToWorldMatrix;

            
            //------------------
            var renderer = gameObject.GetComponent<Renderer>();
            
            var shader = renderer.sharedMaterial.shader;
            _shaderObject = exporter.GetObject(shader) as ShaderObject;

            //------------------
            renderer.GetPropertyBlock(block);
            

            var count = shader.GetPropertyCount();

            for (var i = 0; i < count; i++)
            {
                var p = new ShaderParameter(shader.GetPropertyName(i),
                    ShaderParameter.GetTypeFromPropertyType(shader.GetPropertyType(i)));
                switch (p.type)
                {
                    case ShaderParameter.ParameterTypeColor:
                        p.colorValue = block.GetColor(p.name);
                        if (p.colorValue == new Color(0, 0, 0, 0))
                            p.colorValue = renderer.material.GetColor(p.name);
                        break;
                    case ShaderParameter.ParameterTypeFloat:
                        p.floatValue = block.GetFloat(p.name);
                        if(p.floatValue==0.0f)
                            p.floatValue = renderer.material.GetFloat(p.name);
                        break;
                    case ShaderParameter.ParameterTypeTexture2D:
                    {
                        var tex = renderer.material.GetTexture(p.name);
                        if (tex == null) tex = Texture2D.whiteTexture;
                        p.textureValue = exporter.GetObject(tex) as TextureObject;
                    }
                        break;
                    case ShaderParameter.ParameterTypeFloat4:
                        p.vectorValue = block.GetVector(p.name);
                        if(p.vectorValue==new Vector4(0,0,0,0))
                            p.vectorValue = renderer.material.GetVector(p.name);
                        break;
                }
                _parameters.Add(p);
                if (p.type == ShaderParameter.ParameterTypeTexture2D)
                {
                    var texcoord = new ShaderParameter(p.name + "_ST", ShaderParameter.ParameterTypeFloat4);
                    //texcoord.vectorValue = renderer.material.GetVector(texcoord.name);
                    texcoord.vectorValue = block.GetVector(texcoord.name);
                    if(texcoord.vectorValue==new Vector4(0,0,0,0))
                        texcoord.vectorValue = renderer.material.GetVector(texcoord.name);
                    _parameters.Add(texcoord);
                }
            }
            //------------------------------------------------------
        }

        protected override void Write(Stream stream)
        {
            Write(stream, _shaderObject.ObjectID);
            Write(stream, matirxObjectToWorld);
            Write(stream, matrixVP);
            
            
            for (int i = 0; i < _parameters.Count; i++)
            {
                var p = _parameters[i];
                switch (p.type)
                {
                    case ShaderParameter.ParameterTypeColor:
                        Write(stream, p.colorValue);
                        break;
                    case ShaderParameter.ParameterTypeFloat4:
                        Write(stream, p.vectorValue);
                        break;
                    case ShaderParameter.ParameterTypeFloat:
                        Write(stream, p.floatValue);
                        break;
                    case ShaderParameter.ParameterTypeTexture2D:
                        Write(stream, p.textureValue.ObjectID);
                        
                        break;
                }
            }
        }
    }
}