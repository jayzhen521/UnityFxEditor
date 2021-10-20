Shader "AET/BlindsWipe"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
    	_Count("数量",Int)=4
        _Level("级别",Range(0,1))=0
		_Edge("过渡",Range(0,1))=0
        _Angle("角度",Range(0,6.28318531))=0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
Blend SrcAlpha OneMinusSrcAlpha // Premultiplied transparency
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
				float2 uv2 : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            //sampler2D _MainTex;
            //float4 _MainTex_ST;
            float _Count;
            float _Level;
            float _Angle;
			float _Edge;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
				
				float2 pc=float2(0.5,0.5);
				float2 p=o.uv-pc;

		
				float r=_Angle;

				float sc=sin((r*2)%3.141592)*0.414214+1;
				p/=sc;
				p=float2(
					p.x*cos(r)-p.y*sin(r),
					p.y*cos(r)+p.x*sin(r)
				);
				p+=pc;
				o.uv2=p;

				
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
				
                float m=(i.uv2.x*_Count)%1;

				m=smoothstep(_Level,_Level+_Edge,m);
				
                //col.w=m;
               
                return float4(m.xxx,1);
            }
            ENDCG
        }
    }
}
