Shader "Resonai/Preview Object" {
   Properties {
      _MainTex ("Texture Image", 2D) = "white" {}
   }
   SubShader {
      Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "DisableBatching"="True"}

      Lighting Off
      Blend SrcAlpha OneMinusSrcAlpha

      Pass {
         CGPROGRAM

         #pragma vertex vert
         #pragma fragment frag

         #include "UnityCG.cginc"

         uniform sampler2D _MainTex;

         float4 _MainTex_ST;

         struct vertexInput {
            float4 vertex : POSITION;
            float4 tex : TEXCOORD0;
            float4 color : COLOR;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float2 tex : TEXCOORD0;
            float4 color : COLOR;
         };

         float angle(float3 v1, float3 v2)
         {
            return acos(clamp(dot(v1,v2),-1,1));
         }

         vertexOutput vert(vertexInput input)
         {
            vertexOutput output;

            const float PI = 3.14159265359;
            const float PI_2 = 6.28318530718;
            float4 params     = round(input.color * 100.0);
            float this_ring   = params.r;
            float this_column = params.g;
            float rings       = params.b;
            float columns     = params.a;
            

            float3 from_camera =  - ObjSpaceViewDir(float4(0,0,0,1));
            float theta = angle(normalize(from_camera), float3(0,-1,0));
            float3 camera_on_ground = normalize(float3(from_camera.x,0,from_camera.z));
            float y_angle = angle(camera_on_ground, float3(0,0,1));

            if (camera_on_ground.x < 0.0 )
            {
               y_angle = PI_2 - y_angle;
            }

            float k = PI/(rings-1);
            float cur_ring = (rings-1) - floor(theta/k + 0.5);

            float c = PI_2/(columns);
            float cur_column = floor(y_angle/c+0.5);
            if (cur_column >= columns) cur_column -=columns;

            output.tex = input.tex;
            output.pos = UnityObjectToClipPos(input.vertex);
            output.color = float4(0,0,0,(cur_ring == this_ring && cur_column == this_column));
            return output;
         }

         float4 frag(vertexOutput input) : SV_Target
         {
            fixed4 col = input.color;
            if (input.color.a > 0.0)
            {
                col = tex2D(_MainTex, input.tex);
            }
            clip(col.a - 0.5);
            return col;
         }

         ENDCG
      }
   }
}
