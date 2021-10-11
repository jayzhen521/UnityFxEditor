Shader "AET/BlendModeDARKEN"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BackTex ("背景", 2D) = "white" {}
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
                float2 uvbk : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _BackTex;
            float4 _MainTex_ST;
            float _Level;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvbk=(o.vertex.xy+1)*0.5;
                o.uvbk.y=1-o.uvbk.y;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            //#define BLEND_MODE_ADD
            //#define BLEND_MODE_MULTI
            //#define BLEND_MODE_LIGHTEN
            #define BLEND_MODE_DARKEN

            
            float3 rgb2hsv(float3 c)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
    float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}
            float3 hsv2rgb(float3 c)
{
    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                
    return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}
            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 colfg = tex2D(_MainTex, i.uv);
                float4 colbk = tex2D(_BackTex, i.uvbk);

                
                #ifdef BLEND_MODE_ADD
                fixed4 col=colfg+colbk;
                col=min(1,col);
                return col;
                #endif

                #ifdef BLEND_MODE_MULTI
                fixed4 col=colfg*colbk;
                
                return col;
                #endif

                #ifdef BLEND_MODE_LIGHTEN
                return max(colfg,colbk);
                #endif

                #ifdef BLEND_MODE_DARKEN
                return min(colfg,colbk);
                #endif
                
                
                
                
            }
            ENDCG
        }
    }
}
