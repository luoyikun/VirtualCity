		#define DYNAMIC_FOG_STEPS 2

		float4 _FogData;
		fixed4 _FogColor;
		float  _FogData2;

		void fogColor (Input IN, SURFACE_STRUCT o, inout fixed4 color) {
	       	#ifdef UNITY_PASS_FORWARDADD
         	_FogColor = 0;
         	#endif

			float3 worldPos = IN.worldPos;
			worldPos.y -= _FogData.y;
			float3 wsCameraPos = float3(_WorldSpaceCameraPos.x, _WorldSpaceCameraPos.y - (_FogData.y + 0.0001), _WorldSpaceCameraPos.z);
  			if (wsCameraPos.y<=_FogData.x || worldPos.y<=_FogData.x) {

				// Determine "fog length" and initial ray position between object and camera, cutting by fog distance params
				float3 adir = worldPos - wsCameraPos;
		
				// ceiling cut
				float delta = length(adir.xz);
				float2 ndirxz = adir.xz / delta;
				delta /= adir.y;
		
				float h = min(wsCameraPos.y, _FogData.x);
				float xh = delta * (wsCameraPos.y - h);
				float2 xz = wsCameraPos.xz - ndirxz * xh;
				float3 fogCeilingCut = float3(xz.x, h, xz.y);
		
				// does fog stars after pixel? If it does, exit now
				float dist = length(adir);
				float distanceToFog = distance(fogCeilingCut, wsCameraPos);
				if (distanceToFog<dist) {

					// floor cut
					float hf = 0;
					// edge cases
					if (delta>0 && worldPos.y > -0.5) {
						hf = _FogData.x;
					} else if (delta<0 && worldPos.y < 0.5) {
						hf = worldPos.y;
					}
					float xf = delta * ( hf - wsCameraPos.y ); 
					float2 xzb = wsCameraPos.xz - ndirxz * xf;
					float3 fogFloorCut = float3(xzb.x, hf, xzb.y);

					// fog length is...
					float fogLength = distance(fogCeilingCut, fogFloorCut);
					fogLength = min(fogLength, dist - distanceToFog);
					if (fogLength>0) {
						fogFloorCut = fogCeilingCut + (adir/dist) * fogLength;
						float3 st = (fogFloorCut - fogCeilingCut) / DYNAMIC_FOG_STEPS;
						float3 pos = fogCeilingCut;
						fixed4 fogColor = fixed4(0,0,0,0);
						float incDist = fogLength / DYNAMIC_FOG_STEPS;
						UNITY_UNROLL
						for (int k=DYNAMIC_FOG_STEPS;k>=0;k--, pos += st, distanceToFog += incDist) {
							float fh = (_FogData.x - pos.y) / (_FogData.x * _FogData.w) - 0.1;
							float fl = (distanceToFog - _FogData.z) / _FogData2;
							fh = min(fh, fl);
							fixed4 col = _FogColor;
							col.a *= saturate (fh);
							col.rgb *= col.a;
							fogColor += col * (1.0 - fogColor.a);
						}
						fogColor *= _FogColor.a;
			 			color = color * (1.0 - fogColor.a) + fogColor;
					}
				}
	 		}
	    }

		