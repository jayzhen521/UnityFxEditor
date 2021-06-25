Shader "HLFx/ColorDistanceHSL"
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







			
			float3 rgb2hsv(float3 c)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
    float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

            float4 frag (v2f i) : SV_Target
            {
                
                float2 uv=i.uv;
                // sample the texture
                float4 col = tex2D(_MainTex, uv);
                float d=distance(col.xyz,_Color.xyz);
                

	            float3 c1=rgb2hsv(col.xyz);
	            float3 c2=rgb2hsv(_Color.xyz);

            	c1.x*=6.2831853072;
            	c2.x*=6.2831853072;
            	
	            float3 ddd=float3(
	            	sin(c1.x)*c1.y*c1.z-sin(c2.x)*c2.y*c2.z,
	            	cos(c1.x)*c1.y*c1.z-cos(c2.x)*c2.y*c2.z,
	            	c1.z-c2.z
	            	);
                
                float m=ddd.x*ddd.x+ddd.y*ddd.y+ddd.z*ddd.z;
                m=sqrt(m);
                return float4(m,m,m,1);
            }
            ENDCG
        }
    }
}
