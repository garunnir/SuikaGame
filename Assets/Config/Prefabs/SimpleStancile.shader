// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "UI/Unlit/SimpleStencil"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)

        _MaskTex ("stencil (RGB)", 2D) = "white" {}
        _Cutout ("Cutout", Range(0.0, 1.0)) = 0.2

        //_StencilComp ("Stencil Comparison", Float) = 8
        //_Stencil ("Stencil ID", Float) = 0
        //_StencilOp ("Stencil Operation", Float) = 0
        //_StencilWriteMask ("Stencil Write Mask", Float) = 255
        //_StencilReadMask ("Stencil Read Mask", Float) = 255

        //_ColorMask ("Color Mask", Float) = 15

        //[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        LOD 100

        Tags
        {
            "Queue" = "Transparent"
            //"IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType"="Plane"
//"Queue"="AlphaTest"
//"RenderType"="TransparentCutout"
"IgnoreProjector"="True"
        }

        //Cull Off
        Lighting Off
        ZTest [unity_GUIZTestMode]
        //ZWrite Off

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
        //AlphaToMask On
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            //#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            //#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float2 texcoord2 : TEXCOORD1;
                fixed4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float2 texcoord2 : TEXCOORD1;
                float4 worldPosition : TEXCOORD2;
                fixed4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            sampler2D _MaskTex;
            float4 _MainTex_ST;
            float4 _MaskTex_ST;
            fixed4 _Color;
            float _Cutout;

            fixed4 _TextureSampleAdd;

            bool _UseClipRect;
            float4 _ClipRect;

            bool _UseAlphaClip;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(o.worldPosition);

                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.texcoord2 = TRANSFORM_TEX(v.texcoord2, _MaskTex);

                //if (_UIVertexColorAlwaysGammaSpace)
                //{
                //    if(!IsGammaSpace())
                //    {
                //        v.color.rgb = UIGammaToLinear(v.color.rgb);
                //    }
                //}

                o.color = v.color;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = (tex2D(_MainTex, i.texcoord)) * i.color;
                fixed4 stencil = tex2D(_MaskTex, i.texcoord2);
                float alpha = (1 - stencil.rgb);
                if(alpha>_Cutout)alpha=1;
                color.a = color.a - alpha;
                //color = color * _Color;
    if (distance(float2(0, 0), float2(i.worldPosition.xy)) > 0.45)
    {
        color.rgb = _Color * abs(sin(i.worldPosition.y + _Time.y)) + (_Color.gbr*0.8 + _Color) * abs(cos(i.worldPosition.y + _Time.y));
        //color.a *= abs(sin(_Time.y));
        
        //if ()
        //{
            
        //    color.rgb = _Color.gba * abs(cos(_Time.y));
            
        //}
        //else
        //{
            
        //}
    }

    //clip(color.a);
                //#ifdef UNITY_UI_CLIP_RECT
                //color.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                //#endif

                //#ifdef UNITY_UI_ALPHACLIP
                //clip (color.a - 0.001);
                //#endif

        return color;
}
            ENDCG

        }
    }
}
