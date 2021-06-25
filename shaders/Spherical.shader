Shader "HLFx/Spherical"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Power("power",Float)=0.1
        _Center("center",Vector)=(0.5,0.5,0,0)
        _Radius("radius",Float)=0.2
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
            
            float _Radius;
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
                float2 uv=i.uv-_Center;                
                float2 N=normalize(uv);
                float l=sqrt(uv.x*uv.x+uv.y*uv.y);

                float d=min(l,_Radius);
                d/=_Radius;
                
                
                float p=sqrt(d-d*d);
                
                uv=(i.uv-_Center)*(1-p*_Power)+_Center;

                float4 color=tex2D(_MainTex,uv);

                
                
                return color;
                
            }
            ENDCG
        }
    }
}
