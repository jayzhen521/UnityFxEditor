Shader "HLFx/Droplet"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Center("center",Vector)=(0.5,0.5,0,0)
        //_Inner("inner",Float)=0
        _Zoom("zoom",Float)=1
        _Power("power",Float)=0.01
        _Offset("offset",Float)=0
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
            float4 _Center;
            float _Inner;
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
                float2 dir=(i.uv-_Center.xy);
                dir=normalize(float2(-1,-1));
                
                float d=distance(i.uv,_Center.xy);
                
                
                
                
                float p=sin(d*_Zoom+_Offset)*_Power;

//                float innerD=clamp(d,_Inner,1);
//                p*=innerD;
                
                float2 uv=i.uv+dir*p;
                // sample the texture
                float4 col = tex2D(_MainTex, uv);
                if(uv.x<0||uv.y<0||uv.x>1||uv.y>1)col=float4(0,0,0,1);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
