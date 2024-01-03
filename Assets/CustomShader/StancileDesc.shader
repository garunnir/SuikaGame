Shader "Unlit/stencileReceive"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		Colorize("Colorize", Range(0.0, 1.0)) = 1
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		//자신이 있는 부분에선 나머지를 전부 투명하게 한다.
		//뒤에있는것은 그리지 않는다.
		Stencil
		{
		    Ref 1
		    Comp NotEqual//레프1이 담긴 버퍼와 일치하지 않는것만 보이게
		}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
			   float4 vertex : POSITION;
			   float2 uv : TEXCOORD0;
			   float4 color:COLOR;
			};

			struct v2f
			{
			   float2 uv : TEXCOORD0;
			   float4 vertex : SV_POSITION;
			   float4 color :COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float Colorize;

			v2f vert(appdata v)
			{
			   v2f o;
			   o.vertex = UnityObjectToClipPos(v.vertex);
			   o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			   UNITY_TRANSFER_FOG(o,o.vertex);
			   o.color=v.color;
			   return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float4 texcolor = tex2D(_MainTex, i.uv); // texture's pixel color
				float4 vertexcolor = i.color; // this is coming from UnityEngine.UI.Image.Color
				texcolor.rgba = texcolor.rgba * (1 - Colorize) + vertexcolor.rgba * Colorize;
				return texcolor;
			}
			ENDCG
		} 
	}
}