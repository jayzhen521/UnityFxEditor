Shader "HLFx/Pixelate"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WeightTex("Weight",2D)="white"{}
        _hPixels("Horiz Pixels",Float)=32
        _vPixels("Vert Pixels",Float)=32
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
            sampler2D _WeightTex;
            float4 _WeightTex_ST;
            float _hPixels;
            float _vPixels;

            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv1 = TRANSFORM_TEX(v.uv, _WeightTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            
            float4 frag (v2f i) : SV_Target
            {
                float2 duv=float2(1/_hPixels,1/_vPixels);
                float2 cp=duv*0.5;
                float2 uv=i.uv;
                //uv.x=floor(uv.x/_hPixels)*_hPixels;

                float4 wd = tex2D(_WeightTex, i.uv1);
                duv*=(0.5+wd.xy);
                uv=floor((uv-0.5)/duv)*duv+0.5;
                
                
                // sample the texture
                float4 col = tex2D(_MainTex, uv);
                //col.xyz=float3(wd.x,0,0);
                
                // apply fog
                
                return col;
            }
            ENDCG
        }
    }
}
