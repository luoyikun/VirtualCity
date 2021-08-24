﻿
Shader "BFI/Font4"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_MainTex2("Texture2", 2D) = "white" {}
		_MainTex3("Texture3", 2D) = "white" {}
		_MainTex4("Texture4", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass
		{
			Name "Default"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
			#include "BFILogic.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;

			v2f vert(appdata_t v)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = v.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = v.texcoord;

				OUT.color = v.color * _Color;
				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _MainTex2;
			sampler2D _MainTex3;
			sampler2D _MainTex4;

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color  = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
				half4 color2 = (tex2D(_MainTex2, IN.texcoord - float2(1.0, 0)) + _TextureSampleAdd) * IN.color;
				half4 color3 = (tex2D(_MainTex3, IN.texcoord - float2(2.0, 0)) + _TextureSampleAdd) * IN.color;
				half4 color4 = (tex2D(_MainTex4, IN.texcoord - float2(3.0, 0)) + _TextureSampleAdd) * IN.color;
				color  *= when_le(IN.texcoord.x, 1.0);
				color2 *= and(when_gt(IN.texcoord.x, 1.0), when_le(IN.texcoord.x, 2.0));
				color3 *= and(when_gt(IN.texcoord.x, 2.0), when_le(IN.texcoord.x, 3.0));
				color4 *= when_gt(IN.texcoord.x, 3.0);
				color  += color2 + color3 + color4;

				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

				#ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
}
