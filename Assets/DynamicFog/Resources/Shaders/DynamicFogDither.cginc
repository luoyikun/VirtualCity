
	float4 _MainTex_TexelSize;
	fixed  _FogDither;

	void ApplyColor(float2 uv, inout fixed4 color) {
		float dither = dot(float2(2.4084507, 3.2535211), uv * _MainTex_TexelSize.zw);
		dither = frac(dither) - 0.4;
		color *= 1.0 + dither * _FogDither;
	}