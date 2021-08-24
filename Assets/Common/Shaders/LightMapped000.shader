// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Lightmapped/Diffuse_000 " {
	Properties {
		_Color ("Main Tint", Color) = (1,1,1,1)
		_MainTex ("Main Tex", 2D) = "white" {}
		_LightMap ("Lightmap (RGB)", 2D) = "white" {}
		_Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
		_Light ("Light", Range(0,12)) = 5.5
		//用于决定我们调用clip进行透明度测试时使用的判断条件
	}
	SubShader{
		//第一个tag 把Queue标签设置为AlphaTest 
		//而RenderType 标签可以让Unity把这个Shader归入到提前定义的组以指明该Shader 是
		//一个使用 了透明度测试的Shader（RenderType标签通常用于着色器替换功能）
		//IgnoreProjector 设置为True，这意味着这个Shader 不会受到投影器的影响。
		//通常，使用了透明度测试的Shader 都应该在SubShader 中设置这三个标签
		Tags {"Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
		Pass {
			Tags {"LightMode" = "ForwardBase"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Lighting.cginc"
			
			fixed4 _Color;
			sampler2D _MainTex;
			sampler2D _LightMap;
			float4 _MainTex_ST;
			float4 _LightMap_ST;
			fixed _Cutoff;
			float _Light;
			
			struct a2v {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
			
				float4 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
			};
			
			struct v2f {
			
				float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
			};
			
			v2f vert(a2v v){
				v2f o;
			
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.texcoord1 = TRANSFORM_TEX(v.texcoord1, _LightMap);
                
				
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target{
				
				fixed4 texColor = tex2D(_MainTex, i.texcoord);
			
				
				//透明度测试
				clip(texColor.a - _Cutoff);  //修剪 Equal to    if((texColor.a - _Cutoff) < 0.0){discard};
				
				fixed3 albedo = texColor.rgb * _Color.rgb;
				
				
                albedo.rgb *= DecodeLightmap(tex2D(_LightMap, i.texcoord1)) * _Light;
                
				
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo.rgb;
				
				return fixed4(ambient , 1.0);
			}
			ENDCG
		}
	}
	Fallback "Transparent/Cutout/VertexLit"
}
