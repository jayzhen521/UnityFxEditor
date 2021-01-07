Shader "HLFx/BumpMapRGB"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpTex ("Bump", 2D) = "white" {}
        _Angle("angle",Float)=0
        _Power("power",Range(0,1))=0.1
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
                float2 uv1 : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _BumpTex;
            float4 _BumpTex_ST;
            float _Angle;
            float _Power;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv1 = TRANSFORM_TEX(v.uv, _BumpTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float3 bump = tex2D(_BumpTex, i.uv1).xyz;
                float r=_Angle;
                float2 dir=float2(sin(r),cos(r));
                dir=normalize(dir);
                //float2 uv=i.uv+dir*(_Power*bump);
                float4 col =float4(0,0,0,1);
                 col.x=tex2D(_MainTex, i.uv+dir*(_Power*bump.x)).x;
                col.y=tex2D(_MainTex, i.uv+dir*(_Power*bump.y)).y;
                col.z=tex2D(_MainTex, i.uv+dir*(_Power*bump.z)).z;
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
