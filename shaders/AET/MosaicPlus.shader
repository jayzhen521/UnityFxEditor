Shader "AET/MosaicPlus"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BallsTex("Texture",2D)="white"{}
        //_NoiseTex("Texture",2D)="white"{}

        _hPixels("水平马赛克",Float)=32
        _vPixels("垂直马赛克",Float)=32
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
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            

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
                float4 col = tex2D(_MainTex, float2(uv.x,uv.y))*a1;
                float4 col1 = tex2D(_MainTex, float2(uv.x-dx,uv.y))*a2;
                float4 col2 = tex2D(_MainTex, float2(uv.x+dx,uv.y))*a3;
                float4 fc=col+col1+col2;
                fc/=a;
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
                return fc;
            }
            ENDCG
        }
    }
}