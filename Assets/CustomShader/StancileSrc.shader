Shader "Unlit/stencile"
{
    Properties
    {
        [Toggle] _TextureExist("TextureExist",int)=0
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Blend SrcAlpha OneMinusSrcAlpha
        //자신이 있는 부분에선 나머지를 전부 투명하게 한다.
        //뒤에있는것은 그리지 않는다.
        Stencil
        {
            ref 1
            Pass Replace//레퍼런스 저장
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
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };

        sampler2D _MainTex;
        float4 _MainTex_ST;
        int _TextureExist;

        v2f vert(appdata v)
        {

            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            UNITY_TRANSFER_FOG(o,o.vertex);
            return o;
        }

        fixed4 frag(v2f i) : SV_Target
        {
            if(_TextureExist==0)
            {
                return fixed4(0,0,0,0);
            }
            else
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }

        }
        ENDCG
        }
    }
}