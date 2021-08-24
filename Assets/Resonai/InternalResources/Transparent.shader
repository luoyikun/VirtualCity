Shader "Resonai/Transparent" {
  SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100
 
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha 
 
    Pass {  
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #pragma target 2.0
      #pragma multi_compile_fog
      
      #include "UnityCG.cginc"

      struct appdata_t {
        float4 vertex : POSITION;
        UNITY_VERTEX_INPUT_INSTANCE_ID
      };

      struct v2f {
        float4 vertex : SV_POSITION;
        UNITY_VERTEX_OUTPUT_STEREO
      };

      fixed4 tColor;
      
      v2f vert (appdata_t v)
      {
        v2f o;
        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
        o.vertex = UnityObjectToClipPos(v.vertex);
        return o;
      }
      
      fixed4 frag (v2f i) : COLOR
      {
        fixed4 col = tColor;
        return col;
      }
      ENDCG
    }
  }
}