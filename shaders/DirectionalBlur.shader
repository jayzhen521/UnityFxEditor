Shader "HLFx/DirectionalBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Power("power",Float)=0.1
        _Samplers("samplers",Float)=32
        _Angle("angle",Float)=0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
            float4 _MainTex_ST;
            
            float _Power;
            float _Samplers;
            float _Angle;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {   
                float d=_Power/_Samplers;
                float4 color=float4(0,0,0,0);
                float r=(_Angle*3.1415926)/180.0;
                float dy=cos(r);
                float dx=sin(r);
                float2 dir=normalize(float2(dx,dy));
                float2 uv=i.uv+_Power*0.5*dir;
                
                // sample the texture
                for(int i=0;i<_Samplers;i++)
                {   
                    color+= tex2D(_MainTex, uv+i*d*dir);    
                }
                color/=_Samplers;
                // apply fog
                
                return color;
            }
            ENDCG
        }
    }
}
