Shader "HLFx/RadiusBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Power("power",Float)=0.05
        _Samplers("sampler",Float)=16
        _Center("center",Vector)=(0.5,0.5,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Cull Off
            
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
            float4 _Center;

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
                float2 pos=_Center.xy;//float2(0.5,0.5);
                float d=_Power/_Samplers;
                float4 color=float4(0,0,0,0);
                float2 rp=i.uv-pos;
                
                float2 uv=i.uv;

                if (_Power < 0.00001) {
                    color = tex2D(_MainTex, uv);
                }
                else {
                    // sample the texture
                    for (int i = 0; i < _Samplers; i++)
                    {
                        float a = i * d - _Power * 0.5;
                        float2 p = float2(cos(a) * rp.x - sin(a) * rp.y, cos(a) * rp.y + sin(a) * rp.x);
                        p += pos;
                        color += tex2D(_MainTex, p);
                    }
                    color /= _Samplers;
                    // apply fog
                }

                return color;
            }
            ENDCG
        }
    }
}
