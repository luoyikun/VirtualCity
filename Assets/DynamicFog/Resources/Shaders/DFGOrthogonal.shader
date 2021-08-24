Shader "DynamicFog/Image Effect/Orthogonal" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_FogAlpha ("Alpha", Range (0, 1)) = 0.8
		_FogDistance ("Distance Params", Vector) = (0.1, 0.001, 1.0, 0.15)
		_FogHeightData ("Baseline Height", Vector) = (1,0,0,0.1)  // x = height, y = base height, z = max height, w = height fall off
		_FogColor ("Color", Color) = (1,1,1,1)
		_FogOfWarCenter("Fog Of War Center", Vector) = (0,0,0)
		_FogOfWarSize("Fog Of War Size", Vector) = (1,1,1)
		_FogOfWar ("Fog of War Mask", 2D) = "white" {}
		_ClipDir("Camera Direction", Vector) = (0,0,1)
		_FogDither ("Fog Dither Strength", Float) = 0.03
	}
	SubShader {
    ZTest Always Cull Off ZWrite Off
   	Fog { Mode Off }
	Pass {

	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#pragma fragmentoption ARB_precision_hint_fastest
	#pragma multi_compile __ FOG_OF_WAR_ON
	#pragma multi_compile __ DITHER_ON
	#pragma target 3.0

	#include "DynamicFogCommon.cginc"
	#define DYNAMIC_FOG_STEPS 2

	float4 _FogDistance; // x = min distance, y = min distance falloff, x = max distance, y = max distance fall off
	float4 _FogHeightData;
	fixed4 _FogColor;

    float3 wsCameraPos;
    
	float3 getWorldPos(v2f i, float depth01) {
    	// Reconstruct the world position of the pixel
     	wsCameraPos = float3(_WorldSpaceCameraPos.x, _WorldSpaceCameraPos.y - _FogHeightData.y, _WorldSpaceCameraPos.z);
    	float3 worldPos = (i.cameraToFarPlane * depth01) + wsCameraPos;
    	worldPos.y += 0.00001; // fixes artifacts when worldPos.y = _WorldSpaceCameraPos.y which is really rare but occurs at y = 0
    	return worldPos;
    }

	fixed4 getFogColor(float2 uv, float3 worldPos, float depth, fixed4 color) {
		
		// early exit if fog is not crossed
		if (wsCameraPos.y>_FogHeightData.z && worldPos.y>_FogHeightData.z) {
			return color;		
		}

		half voidAlpha = _FogAlpha;

		// Determine "fog length" and initial ray position between object and camera, cutting by fog distance params
		float3 adir = worldPos - wsCameraPos;
		
		// ceiling cut
		float delta = length(adir.xz);
		float2 ndirxz = adir.xz / delta;
		delta /= adir.y;
		
		float h = min(wsCameraPos.y, _FogHeightData.z);
		float xh = delta * (wsCameraPos.y - h);
		float2 xz = wsCameraPos.xz - ndirxz * xh;
		float3 fogCeilingCut = float3(xz.x, h, xz.y);
		
		// does fog stars after pixel? If it does, exit now
		float dist = length(adir);
		float distanceToFog = distance(fogCeilingCut, wsCameraPos);
		if (distanceToFog>=dist) return color;

		// floor cut
		float hf = 0;
		// edge cases
		if (delta>0 && worldPos.y > -0.5) {
			hf = _FogHeightData.z;
		} else if (delta<0 && worldPos.y < 0.5) {
			hf = worldPos.y;
		}
		float xf = delta * ( hf - wsCameraPos.y ); 
		float2 xzb = wsCameraPos.xz - ndirxz * xf;
		float3 fogFloorCut = float3(xzb.x, hf, xzb.y);

		// fog length is...
		float fogLength = distance(fogCeilingCut, fogFloorCut);
		fogLength = min(fogLength, dist - distanceToFog);
		if (fogLength<=0) return color;
		fogFloorCut = fogCeilingCut + (adir/dist) * fogLength;
		
		#if FOG_OF_WAR_ON
		if (depth<0.999) {
			float2 fogTexCoord = fogFloorCut.xz / _FogOfWarSize.xz - _FogOfWarCenterAdjusted.xz;
			voidAlpha *= tex2D(_FogOfWar, fogTexCoord).a;
			if (voidAlpha <=0) return color;
		}
		#endif
		
		float3 st = (fogFloorCut - fogCeilingCut) / DYNAMIC_FOG_STEPS;
		float3 pos = fogCeilingCut;
		fixed4 fogColor = fixed4(0,0,0,0);
		float incDist = fogLength / DYNAMIC_FOG_STEPS;
		UNITY_UNROLL
		for (int k=DYNAMIC_FOG_STEPS;k>=0;k--, pos += st, distanceToFog += incDist) {
			float fh = (_FogHeightData.x - pos.y) / (_FogHeightData.x * _FogHeightData.w) - 0.1;
			float fl = (distanceToFog - _FogDistance.x) / _FogDistance.y;
			fh = max(fh, fl);
			fixed4 col = _FogColor;
			col.a *= saturate (fh);
			col.rgb *= col.a;
			fogColor += col * (1.0 - fogColor.a);
		}
		fogColor *= voidAlpha;
	 	return color * (1.0 - fogColor.a) + fogColor;
	}

	// Fragment Shader
	fixed4 frag (v2f i) : SV_Target {
   		fixed4 color = tex2D(_MainTex, i.uv);
		float depth = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.depthUV)));
		float3 worldPos = getWorldPos(i, depth);
		color = getFogColor(i.uv, worldPos, depth, color);
		#if DITHER_ON
		ApplyColor(i.uv, color);
		#endif
		return color;
	}
	ENDCG
	}
}
FallBack Off

}