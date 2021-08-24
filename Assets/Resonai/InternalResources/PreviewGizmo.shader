Shader "Resonai/PreviewGizmo" {
   Properties {
		  [HideInInspector] _MainTex ("Texture Image", 2D) = "white" {}
			[HideInInspector] _Scale("Scale", Float) = 0.3
			[HideInInspector] _SpriteW("Sprite width", Float) = 154
			[HideInInspector] _SpriteH("Sprite height", Float) = 99
			[HideInInspector] _Color("Tint color", Color) = (1,1,1,1)
			[HideInInspector] _Height("Height of gizmo", Float) = 0.65
	 }
		 SubShader{
			 Tags
			 {
				 "Queue" = "Transparent"
				 "SortingLayer" = "Resources_Sprites"
				 "IgnoreProjector" = "True"
				 "RenderType" = "Transparent"
				 "PreviewType" = "Plane"
				 "CanUseSpriteAtlas" = "True"
				 "DisableBatching" = "True"
			 }

			 Cull Off
			 Lighting Off
			 ZWrite Off
			 Blend SrcAlpha OneMinusSrcAlpha

				Pass {
					 CGPROGRAM

					 #pragma vertex vert  
					 #pragma fragment frag

					 #include "UnityCG.cginc"

					 // User-specified uniforms 
					uniform float _Scale;
					uniform float _SpriteW;
					uniform float _SpriteH;
					uniform float4 _Color;
					uniform float _Height;
					uniform sampler2D _MainTex;
					float4 _MainTex_ST;

         struct vertexInput {
            float4 vertex : POSITION;
            float4 tex : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float2 tex : TEXCOORD0;
         };

         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;

						output.tex = TRANSFORM_TEX(input.tex, _MainTex);
						
						float4x4 mat = UNITY_MATRIX_M;
						
						float4 col_vector_0 = mat._m00_m10_m20_m30;
						float4 col_vector_1 = mat._m01_m11_m21_m31;
						float model_scale_x = sqrt(dot(col_vector_0, col_vector_0));
						float model_scale_y = sqrt(dot(col_vector_1, col_vector_1));

						float4 base_scale_with_aspect = float4(_Scale, _Scale * _SpriteH / _SpriteW, 1, 0);
						float4 scale_with_obj_scale = base_scale_with_aspect * float4(model_scale_x, model_scale_y, 1, 1);
						float4 vertex_pos = float4(input.vertex.x, input.vertex.y, 0, 0) * scale_with_obj_scale;
						float4 object_position = float4(UnityObjectToViewPos(float3(0, 0, 0)), 1);
						
						vertex_pos += object_position + (float4(0, _Height, 0, 0) * model_scale_y);

						output.pos = mul(UNITY_MATRIX_P, vertex_pos);

            return output;
         }
 
         float4 frag(vertexOutput input) : SV_Target
         {
            return tex2D(_MainTex, input.tex) * _Color;
         }
 
         ENDCG
      }
   }
}
