Shader "DynamicFog/Reflections/Lambert" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ReflectionTex ("Reflections (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert finalcolor:fogColor exclude_path:deferred  exclude_path:prepass fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _ReflectionTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float4 screenPos;
		};

		#define SURFACE_STRUCT SurfaceOutput
		#include "DFMSurfaceShaderCommon.cginc"

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 r = tex2D (_ReflectionTex, IN.screenPos.xy / IN.screenPos.w);
	 		o.Albedo = (c*r*_Color).rgb;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
