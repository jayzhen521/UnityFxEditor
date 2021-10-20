Shader "AET/Vignette"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Level("范围",Range(0,5))=1
        _LevelInner("内范围",Range(0,5))=1
        _posX("x",Range(0,1))=0
        _posY("y",Range(0,1))=0
        _color("颜色",Color)=(1,1,1,1)
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
            float _Level;
            float _LevelInner;
            float _posX;
            float _posY;
            float4 _color;

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
                // sample the texture
                float2 uv=i.uv;
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 pos=float2(_posX,_posY);
                float2 dd=uv-pos;
                dd*=(5-_Level);
                float dotv=1-min((dd.x*dd.x+dd.y*dd.y),1);
                dotv=min(dotv*(5-_LevelInner),1);

                float4 fc=lerp(_color,col,dotv);
                
                
                return fc;
            }
            ENDCG
        }
    }
}
