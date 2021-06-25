Shader "HLFx/ColorDistanceRGB"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("color",Color)=(1,1,1,1)
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
                float2 uv1 : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            

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
                
                float2 uv=i.uv;
                // sample the texture
                float4 col = tex2D(_MainTex, uv);
                float d=distance(col.xyz,_Color.xyz);
                //col.xyz=float3(wd.x,0,0);
                
                // apply fog
                d=clamp(d*0.577,0,1);
                return float4(d,d,d,1);
            }
            ENDCG
        }
    }
}
