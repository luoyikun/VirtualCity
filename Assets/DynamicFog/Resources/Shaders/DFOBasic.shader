Shader "DynamicFog/Image Effect/Orthographic Basic" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_FogAlpha ("Alpha", Range (0, 1)) = 0.8
		_FogDistance ("Distance Params", Vector) = (0.1, 0.001, 1.0, 0.15)
		_FogHeightData ("Baseline Height", Vector) = (1,0,0,0.1)  // x = height, y = base height, z = clipping minimum height, w = height fall off
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

	#include "UnityCG.cginc"
	#include "DynamicFogDither.cginc"

	#define DYNAMIC_FOG_STEPS 1

	sampler2D _MainTex;
	sampler2D_float _CameraDepthTexture;
	float4 _MainTex_ST;
	fixed _FogAlpha;
	float4 _FogDistance; // x = min distance, y = min distance falloff, x = max distance, y = max distance fall off
	float4 _FogHeightData;
	fixed4 _FogColor;

    #if FOG_OF_WAR_ON 
    sampler2D _FogOfWar;
    float3 _FogOfWarCenter;
    float3 _FogOfWarSize;
    float3 _FogOfWarCenterAdjusted;
    #endif

    float4x4 _ClipToWorld;
    float3 _ClipDir;
    float3 wsCameraPos;
    
    struct appdata {
    	float4 vertex : POSITION;
		half2 texcoord : TEXCOORD0;
    };
    
    
	struct v2f {
	    float4 pos : SV_POSITION;
	    float2 uv: TEXCOORD0;
    	float2 depthUV : TEXCOORD1;
    	float3 cameraToFarPlane : TEXCOORD2;
	};

	v2f vert(appdata v) {
    	v2f o;
    	o.pos = UnityObjectToClipPos(v.vertex);
    	o.uv = UnityStereoScreenSpaceUVAdjust(v.texcoord, _MainTex_ST);
    	o.depthUV = o.uv;
   	      
    	#if UNITY_UV_STARTS_AT_TOP
    	if (_MainTex_TexelSize.y < 0) {
	        // Depth texture is inverted WRT the main texture
    	    o.depthUV.y = 1 - o.depthUV.y;
    	}
    	#endif
               
    	// Clip space X and Y coords
    	float2 clipXY = o.pos.xy / o.pos.w;
               
    	// Position of the far plane in clip space
    	float4 farPlaneClip = float4(clipXY, 1, 1);
               
    	// Homogeneous world position on the far plane
    	farPlaneClip *= float4(1,_ProjectionParams.x,1,1);    	
    	float4 farPlaneWorld4 = mul(_ClipToWorld, farPlaneClip);
               
    	// World position on the far plane
    	float3 farPlaneWorld = farPlaneWorld4.xyz / farPlaneWorld4.w;
               
    	// Vector from the camera to the far plane
    	o.cameraToFarPlane = farPlaneWorld - _WorldSpaceCameraPos;
 
    	return o;
	}


	float3 getWorldPos(v2f i, float depth01) {
    	// Reconstruct the world position of the pixel
    	wsCameraPos = float3(_WorldSpaceCameraPos.x, _WorldSpaceCameraPos.y - _FogHeightData.y, _WorldSpaceCameraPos.z);
    	float3 worldPos = i.cameraToFarPlane - _ClipDir * _ProjectionParams.z * (1.0 - depth01) + wsCameraPos;
    	return worldPos;
    }

	fixed4 getFogColor(float2 uv, float3 worldPos, float depth, fixed4 color) {
		
		// early exit if fog is not crossed
		if (wsCameraPos.y>_FogHeightData.x && worldPos.y>_FogHeightData.x) {
			return color;		
		}

		fixed voidAlpha = _FogAlpha;

		// Determine "fog length" and initial ray position between object and camera, cutting by fog distance params
		float3 adir = worldPos - wsCameraPos;
		
		// ceiling cut
		float delta = length(adir.xz);
		float2 ndirxz = adir.xz / delta;
		delta /= adir.y;
		
		float h = min(wsCameraPos.y, _FogHeightData.x);
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
			hf = _FogHeightData.x;
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
		
		distanceToFog += fogLength;
		float fh = (_FogHeightData.x - fogFloorCut.y) / (_FogHeightData.x * _FogHeightData.w) - 0.1;
		float fl = (distanceToFog - _FogDistance.x) / _FogDistance.y;
		fh = min(fh, fl);
		fixed4 fogColor = _FogColor;
		fogColor.a *= saturate (fh);
		fogColor.rgb *= fogColor.a;

		#if DITHER_ON
		float dither = dot(float2(2.4084507, 3.2535211), uv * _MainTex_TexelSize.zw);
		dither = frac(dither) - 0.4;
		fogColor *= 1.0 + dither * 0.1;
		#endif
		fogColor *= voidAlpha;
	 	return color * (1.0 - fogColor.a) + fogColor;
	}

	// Fragment Shader
	fixed4 frag (v2f i) : SV_Target {
   		fixed4 color = tex2D(_MainTex, i.uv);
		float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.depthUV));
		#if UNITY_REVERSED_Z
		depth = 1.0 - depth;
		#endif
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