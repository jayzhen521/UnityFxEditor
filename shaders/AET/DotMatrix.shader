Shader "AET/DotMatrix"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BallsTex("Dot",2D)="white"{}
        _SizeMask("Size",2D)="white"{}
        //_RotMask("Rot",2D)="white"{}
        _RotOffset("Rotation",Float)=0
        //_NoiseTex("Texture",2D)="white"{}

        _hPixels("水平马赛克",Float)=32
        _vPixels("垂直马赛克",Float)=32
        _Size("大小",Range(0,4))=1
        _Blur("模糊",Range(0,1))=0
        //_dValue("溶解",Range(0,1))=0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            Blend One OneMinusSrcAlpha // Premultiplied transparency
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _BallsTex;
            //sampler2D _RotMask;
            sampler2D _SizeMask;
            float _RotOffset;
            float4 _MainTex_ST;
            float _Size;
            float _Blur;

            float _hPixels;
            float _vPixels;
            float _dValue;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                float2 duv=float2(1.0/_hPixels,1.0/_vPixels);
                float2 cp=duv*0.5;
                float2 uv=i.uv;

                //-----------
                float2 suv=uv/duv;
                suv-=floor(suv);
                
                float2 suv1=uv/duv;
                suv1-=floor(suv1);
                float2 ffuv=suv1;
                suv1.x+=0.5;
                suv1.y=1-suv1.y;

                float2 suv2=uv/duv;
                suv2-=floor(suv2);
                suv2.x-=0.5;
                suv2.y=1-suv2.y;

                float c=floor(uv.y/duv.y);
                c%=2;

                if(c==1)
                {
                    suv.y=1-suv.y;
                    suv1.y=1-suv1.y;
                    suv2.y=1-suv2.y;
                }
                
                uv=floor((uv)/duv)*duv;
                float dx=duv.x*0.5;
                float a1=tex2D(_BallsTex, suv).r;
                float a2=tex2D(_BallsTex, suv1).r;
                float a3=tex2D(_BallsTex, suv2).r;
                float a=a1+a2+a3;
                float4 col = tex2D(_MainTex, float2(uv.x,uv.y));

                
                
                
                
                //rot--
                                float rv=0;//tex2D(_RotMask,uv).r;
                                rv+=_RotOffset;
                                //rv*=1;
                                
                                float2 rvv=float2(cos(rv),sin(rv));
                                
                                
                                float2 ddvv=(ffuv-0.5);
                                ffuv.x=rvv.x*ddvv.x-rvv.y*ddvv.y;
                                ffuv.y=rvv.x*ddvv.y+rvv.y*ddvv.x;
                                ffuv+=0.5;
                                
                                
                                //return rvv.x;
                
                
                float4 col1 = tex2D(_SizeMask, uv);
                //float4 col1 = tex2D(_MainTex, i.uv);
                float m=col1.x;
                
                //float4 col2 = tex2D(_MainTex, float2(uv.x+dx,uv.y))*a3;
                //float4 fc=col+col1+col2;
                //fc/=a;
                m*=_Size;
                ffuv=clamp((ffuv-0.5)/m+0.5,0,1);


                
                float a55=tex2D(_BallsTex, ffuv).r;

                a55=smoothstep(0,_Blur,a55);
                
                col*=a55;
                return col;
                //col.xyz=float3(wd.x,0,0);

                
                //ddv=max(min(1,ddv),0);

				/*
float ddv=tex2D(_NoiseTex, float2(uv.x,uv.y)).r*a1+
                    tex2D(_NoiseTex, float2(uv.x-dx,uv.y))*a2+
                        tex2D(_NoiseTex, float2(uv.x+dx,uv.y))*a3
                        ;
                ddv/=a;
                #ifdef DIS
                ddv=smoothstep(_dValue-0.1,_dValue+0.1,ddv);
                
                // apply fog
                fc.xyz*=ddv;
                #endif

				*/
                //return fc;
            }
            ENDCG
        }
    }
}