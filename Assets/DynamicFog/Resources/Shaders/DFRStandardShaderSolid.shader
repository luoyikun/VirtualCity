Shader "DynamicFog/Reflections/Standard Shader Solid Color" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_ReflectionTex ("Reflections (RGB)", 2D) = "white" {}		
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard finalcolor:fogColor exclude_path:deferred  exclude_path:prepass fullforwardshadows
		#pragma target 3.0

		sampler2D _ReflectionTex;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		struct Input {
			float3 worldPos;
			float4 screenPos;			
		};

		#define SURFACE_STRUCT SurfaceOutputStandard
		#include "DFMSurfaceShaderCommon.cginc"

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 r = tex2D (_ReflectionTex, IN.screenPos.xy / IN.screenPos.w) * _Color;
	 		o.Albedo = r.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
