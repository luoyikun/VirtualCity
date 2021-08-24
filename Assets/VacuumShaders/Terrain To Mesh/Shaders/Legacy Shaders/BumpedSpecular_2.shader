﻿Shader "VacuumShaders/Terrain To Mesh/Legacy Shaders/Bumped Specular/2 Textures" 
{
	Properties 
	{
		_Color("Tint Color", color) = (1, 1, 1, 1)
		_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)

		_V_T2M_Control("Control Map (RGBA)", 2D) = "black" {}

		//TTM				
		[V_T2M_Layer] _V_T2M_Splat1("Layer 1 (R)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat1_uvScale("", float) = 1
		[HideInInspector] _V_T2M_Splat1_bumpMap("", 2D) = ""{}
		[HideInInspector] _V_T2M_Splat1_Shininess("Shininess", Range(0.03, 1)) = 0.078125

		[V_T2M_Layer] _V_T2M_Splat2("Layer 2 (G)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat2_uvScale("", float) = 1
		[HideInInspector] _V_T2M_Splat2_bumpMap("", 2D) = ""{}
		[HideInInspector] _V_T2M_Splat2_Shininess("Shininess", Range(0.03, 1)) = 0.078125
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200 
		
		CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma target 3.0


		#define V_T2M_BUMP
		#define V_T2M_SPECULAR

		#include "../cginc/T2M_Deferred.cginc"		

		ENDCG
	} 

	FallBack "VacuumShaders/Terrain To Mesh/Legacy Shaders/Diffuse/2 Textures"
}
