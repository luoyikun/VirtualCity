Shader "DynamicFog/Opaque/Lambert Solid Color" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert finalcolor:fogColor exclude_path:deferred  exclude_path:prepass fullforwardshadows
		#pragma target 3.0

		fixed4 _Color;

		struct Input {
			float3 worldPos;
		};

		#define SURFACE_STRUCT SurfaceOutput
		#include "DFMSurfaceShaderCommon.cginc"

		void surf (Input IN, inout SurfaceOutput o) {
	 		o.Albedo = _Color.rgb;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
