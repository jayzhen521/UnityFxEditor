Shader "HLFx/LUT"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LUTMap ("LUT", 2D) = "blue" {}
    	_Size("Size",Float)=4
		_ImageSize("Image size",Float)=4096
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
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
                float2 uv2 : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform sampler2D _LUTMap;
            float4 _LUTMap_ST;
			uniform float _Size;
			uniform float _ImageSize;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv, _LUTMap);
                
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
			float2 RGBtoUV(float3 rgb){
			    float s=_Size;
			    
			    float cells=s*s;
			    float maxcells=cells-1.0;
			    
			    float cellwidth=1.0/s;
			    float pixelwidth=1.0/_ImageSize;
			    

			    float2 outuv=float2(0,0);
			    outuv=rgb.rg*(cellwidth-pixelwidth);
			    
			    float num=floor(rgb.b*maxcells+0.0001);
			    num=min(num,maxcells);
            	float u=(num%s)*cellwidth;
			    float v=floor(num/s)*cellwidth;
			    float2 pos=float2(u,v);
			    pos=clamp(pos,0.0,(1.0-cellwidth));
			    
			    float2 ouv=outuv+pos+pixelwidth*0.5;
			    ouv=clamp(ouv,0.0,1.0);
			    
                return ouv;
			}
            float4 frag (v2f i) : SV_Target
            {
            	float3 color1=tex2D(_MainTex,i.uv).xyz;
            	float2 outuv=RGBtoUV(color1);
                // sample the texture
                float3 col = tex2D(_LUTMap, outuv).xyz;
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return float4(col,1);
            }
            ENDCG
        }
    }
}