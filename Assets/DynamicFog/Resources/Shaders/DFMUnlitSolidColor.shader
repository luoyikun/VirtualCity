Shader "DynamicFog/Opaque/Unlit Solid Color" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 3.0

		#include "UnityCG.cginc"

		fixed4 _Color;

		struct v2f {
			float4 pos: SV_POSITION;
			float3 worldPos: TEXCOORD0;
		};

		#include "DFMVertexFragmentShaderCommon.cginc"

		v2f vert(appdata v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			o.worldPos.y -= _FogData.y;
			return o;
		}

		fixed4 frag (v2f IN): SV_Target {
			fixed4 c = _Color;
			fogColor(IN, c);
	 		return c;
		}

		ENDCG
		}
	}
	FallBack "Diffuse"
}
