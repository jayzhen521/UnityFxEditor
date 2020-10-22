Shader "HLFx/Wave"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Zoom("zoom",Float)=1
        _Power("power",Float)=0.1
        _Offset("Offset",Float)=0
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
            
            float _Zoom;
            float _Power;
            float _Offset;

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
                float d=sin(uv.x*_Zoom+_Offset)*_Power;
                uv.y+=d;
                // sample the texture
                
                float4 col = tex2D(_MainTex, uv);
                if(uv.x<0||uv.y<0||uv.x>1||uv.y>1)col=float4(0,0,0,1);
                
                return col;
            }
            ENDCG
        }
    }
}
