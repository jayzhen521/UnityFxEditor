using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

namespace Packages.FxEditor
{
    public class ShaderObject : DataObjectBase
    {
        //------registered shaders---------
        private static Dictionary<string, ShaderRegisterInformation> shaders =
            new Dictionary<string, ShaderRegisterInformation>();
        //---------------------------------------------------------------------

        public const int SourceTypeGLES3Vertex = 0;
        public const int SourceTypeGLES3Fragment = 1;
        public const int SourceTypeGLCoreVertex = 2;
        public const int SourceTypeGLCoreFragment = 3;

        public const int MaxSources = 32;
        
        //------states----------
        
        //---depth func---
        public const int StateDepthFuncNever=0;
        public const int StateDepthFuncLess=1;
        public const int StateDepthFuncEqual=2;
        public const int StateDepthFuncGreater=3;
        public const int StateDepthFuncNotEqual=4;
        public const int StateDepthFuncGequal=5;
        public const int StateDepthFuncAlways=6;
        
        //---alpha func---
        public const int StateAlphaFuncNever=0;
        public const int StateAlphaFuncLess=1;
        public const int StateAlphaFuncEqual=2;
        public const int StateAlphaFuncGreater=3;
        public const int StateAlphaFuncNotEqual=4;
        public const int StateAlphaFuncGequal=5;
        public const int StateAlphaFuncAlways=6;
        
        //---blend test---
        public const int StateBlendFactorZero=0;
        public const int StateBlendFactorOne=1;
        public const int StateBlendFactorSrcColor=2;
        public const int StateBlendFactorOneMinusSrcColor=3;
        public const int StateBlendFactorSrcAlpha=4;
        public const int StateBlendFactorOneMinusSrcAlpha=5;
        public const int StateBlendFactorDstColor=6;
        public const int StateBlendFactorOneMinusDstColor=7;
        public const int StateBlendFactorDstAlpha=8;
        public const int StateBlendFactorOneMinusDstAlpha=9;
        // public const int StateBlendFactorConstantColor=8;
        // public const int StateBlendFactorOneMinusConstantColor=9;
        // public const int StateBlendFactorSrcAlphaSaturate=10;
        // public const int StateBlendFactorSrc1Color=11;
        // public const int StateBlendFactorOneMinusSrc1Color=12;
        // public const int StateBlendFactorSrc1Alpha=13;
        // public const int StateBlendFactorOneMinusSrc1Alpha=14;
        
        //---stencil operator---
        public const int StateStencilOpKeep=0;
        public const int StateStencilOpZero=1;
        public const int StateStencilOpReplace=2;
        public const int StateStencilOpIncrement=3;
        public const int StateStencilOpIncrementWrap=4;
        public const int StateStencilOpDecrement=5;
        public const int StateStencilOpDecrementWrap=6;
        public const int StateStencilOpDecrementInvert=7;
        
        
        //----------------------
        public const int StateIndexDepthEnable=0;
        public const int StateIndexDepthFunc=1;
        public const int StateIndexBlendEnable=2;
        public const int StateIndexBlendSrcFactor=3;
        public const int StateIndexBlendDstFactor=4;
        
        //---------------------------------
        private Int64 _shaderUUID = 0;
        private string _name = "";
        List<ShaderParameter> _parameters = new List<ShaderParameter>();
        // private string _glesSource = "";
        // private string _glcoreSource = "";
        private bool _isRegeistered = false;

        public const int MaxStates = 16;
        private int[] states = new int[MaxStates];
        private int[] sourceStates = new int[MaxSources];
        private string[] sources = new string[MaxSources];
        
        
        
        //--------------
        public const string GLES3Header = "#version 300 es";
        public const string GLCoreHeader = "#version 410";
        //---------------------------------
        
        public static void RegisterShaders()
        {
            Int64 startID = 100;
            startID++;
            { //HLFx/TextureColorMask
                var name = "HLFx/TextureColorMask";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("TextureColorMask/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            startID++;
            { //HLFx/TextureColorMaskAdditive
                var name = "HLFx/TextureColorMaskAdditive";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("TextureColorMask/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOne;
                shaders[name] = regobject;
            }
            
            startID++;
            { //HLFx/TextureColorMaskMultiply
                var name = "HLFx/TextureColorMaskMultiply";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("TextureColorMask/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorZero;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorSrcColor;
                shaders[name] = regobject;
            }
            
            startID++;
            { //HLFx/BumpMap
                var name = "HLFx/BumpMap";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("BumpMap/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("BumpMap/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            
            startID++;
            { //HLFx/DirectionalBlur
                var name = "HLFx/DirectionalBlur";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("DirectionalBlur/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            
            startID++;
            { //HLFx/ZoomBlur
                var name = "HLFx/ZoomBlur";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("ZoomBlur/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            
            startID++;
            { //HLFx/RadiusBlur
                var name = "HLFx/RadiusBlur";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("RadiusBlur/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            
            startID++;
            { //HLFx/Droplet
                var name = "HLFx/Droplet";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("Droplet/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            
            startID++;
            { //HLFx/Wave
                var name = "HLFx/Wave";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("Wave/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            
            startID++;
            { //HLFx/Pixelate
                var name = "HLFx/Pixelate";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("Pixelate/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            
            startID++;
            { //HLFx/ColorDistanceRGB
                var name = "HLFx/ColorDistanceRGB";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("ColorDistanceRGB/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            
            startID++;
            { //HLFx/Inverse
                var name = "HLFx/Inverse";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("Inverse/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            
            startID++;
            { //HLFx/Smoothstep
                var name = "HLFx/Smoothstep";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("Smoothstep/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            //------------------text shaders----------
            startID++;
            { //HLFx/TextShaders/SolidColor
                var name = "HLFx/Text/SolidColor";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("TextShaders/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("TextShaders/SolidColor/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            //------------------Particle shaders----------
            startID++;
            { //HLFx/TextureColorMask
                var name = "HLFx/ParticleSystem/TextureColorMask";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("ParticleSystem/TextureColorMask/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("ParticleSystem/TextureColorMask/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            startID++;
            { //HLFx/TextureColorMaskAdditive
                var name = "HLFx/ParticleSystem/TextureColorMaskAdditive";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("TextureColorMask/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOne;
                shaders[name] = regobject;
            }
            
            startID++;
            { //HLFx/TextureColorMaskMultiply
                var name = "HLFx/ParticleSystem/TextureColorMaskMultiply";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("TextureColorMask/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorZero;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorSrcColor;
                shaders[name] = regobject;
            }
            
            startID++;
            { //HLFx/Spherical
                var name = "HLFx/Spherical";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("Spherical/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            startID++;
            { //HLFx/TextureColorMask
                var name = "HLFx/LUT";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("LUT/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            startID++;
            { //HLFx/BumpMapRGB
                var name = "HLFx/BumpMapRGB";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("BumpMapRGB/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("BumpMapRGB/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            startID++;
            { //HLFx/TextureColorMaskPM
                var name = "HLFx/TextureColorMaskPM";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("TextureColorMaskPM/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            startID++;
            { //HLFx/ColorDistanceHSL
                var name = "HLFx/ColorDistanceHSL";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("ColorDistanceHSL/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            startID++;
            { //HLFx/SmoothstepFloat
                var name = "HLFx/SmoothstepFloat";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("SmoothstepFloat/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            startID++;
            { //HLFx/TextureColorComponents
                var name = "HLFx/TextureColorComponents";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("TextureColorComponents/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            startID++;
            { //HLFx/TextureColorTransparentPM
                var name = "HLFx/TextureColorTransparentPM";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("Common/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("TextureColorTransparentPM/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
            
            startID++;
            { //HLFx/ImageMash
                var name = "HLFx/ImageMask";
                var regobject=new ShaderRegisterInformation(
                    startID
                );
                
                //-source
                var source = GlobalUtility.GetShaderCode("ImageMask/GLES3Vertex.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Vertex] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreVertex] = source.Replace(GLES3Header, GLCoreHeader);
                
                source = GlobalUtility.GetShaderCode("ImageMask/GLES3Fragment.hlshader");
                regobject.sources[ShaderObject.SourceTypeGLES3Fragment] = source;
                regobject.sources[ShaderObject.SourceTypeGLCoreFragment] = source.Replace(GLES3Header,GLCoreHeader);
                
                //-----------------------------

                regobject.states[ShaderObject.StateIndexBlendEnable] = 1;
                regobject.states[ShaderObject.StateIndexBlendSrcFactor] = ShaderObject.StateBlendFactorOne;
                regobject.states[ShaderObject.StateIndexBlendDstFactor] = ShaderObject.StateBlendFactorOneMinusSrcAlpha;
                shaders[name] = regobject;
            }
        }
        //-------------------------------

        public ShaderObject(Shader shader)
        {
            ObjectType = ObjectTypeShader;
            //-----------------------------------------
            _name = shader.name;
            DefaultParameters();
            int count = shader.GetPropertyCount();
            for (var i = 0; i < count; i++)
            {
                var p = new ShaderParameter(shader.GetPropertyName(i),
                    ShaderParameter.GetTypeFromPropertyType(shader.GetPropertyType(i)));

                _parameters.Add(p);

                if (p.type == ShaderParameter.ParameterTypeTexture2D)
                {
                    var p2=new ShaderParameter(p.name+"_ST",
                        ShaderParameter.ParameterTypeFloat4);
                    _parameters.Add(p2);
                }
            }

            //
            if (!shaders.ContainsKey(shader.name))
            {
                _isRegeistered = false;
                Debug.LogError("shader:" + shader.name + "不支持!");
                return;
            }

            
            _isRegeistered = true;
            
            var info = shaders[shader.name];
            _shaderUUID = info.ShaderUUID;

            for (var i = 0; i < MaxSources; i++)
            {
                sourceStates[i] = 0;
                sources[i] = info.sources[i];
                if (sources[i] != "none")
                {
                    sourceStates[i] = 1;
                }
                
            }
            //_glcoreSource = GlobalUtility.GetShaderCode(info.GLCoreSource);
            //_glesSource = GlobalUtility.GetShaderCode(info.GLES3Source);
            
            //states
            for (var i = 0; i < MaxStates; i++)
            {
                if (info.states[i] < 0) continue;
                states[i] = info.states[i];
            }
        }
        private void DefaultParameters()
        {
            //model view matrix
            //uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
            //uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];

            _parameters.Add(new ShaderParameter("hlslcc_mtx4x4unity_ObjectToWorld",
                ShaderParameter.ParameterTypeMatrix4x4));
            _parameters.Add(new ShaderParameter("hlslcc_mtx4x4unity_MatrixVP", 
                ShaderParameter.ParameterTypeMatrix4x4));
            
            //------default states
            states[StateIndexBlendEnable] = 0;
            states[StateIndexBlendSrcFactor] =StateBlendFactorOne;
            states[StateIndexBlendDstFactor] = StateBlendFactorOneMinusSrcAlpha;
            //--------------------------------------
            
        }

        public override void Write(Stream stream)
        {
            if (!_isRegeistered) { return; }

            Write(stream, _shaderUUID);
            Write(stream, _name);
            Write(stream,states);
            
            Write(stream, _parameters.Count);
            for (int i = 0; i < _parameters.Count; i++)
            {
                var p = _parameters[i];

                Write(stream, p.name);
                Write(stream, p.type);
            }

            //write source
            Write(stream,sourceStates);
            for (var i = 0; i < MaxSources; i++)
            {
                
                if (sourceStates[i] == 0) continue;
                 Write(stream, sources[i]);
            }
            // Write(stream, SourceTypeGLES3);
            // Write(stream, _glesSource);
            // Write(stream, SourceTypeGLCore);
            // Write(stream, _glcoreSource);
        }
    }
}