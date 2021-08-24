// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "DynamicFog/Fog Of War" {
	Properties {
                _MainTex ("Base (RGBA)", 2D) = "white" {}
                _NoiseTex ("Noise (RGB)", 2D) = "white" {}
         		_Color ("Color", Color) = (1,1,1,1)
         		_Speed ("Speed", Range(0,0.1)) = 0.002
         		_Direction ("Direction", Vector) = (1,1,0)
         		_Scale1 ("Scale 1", Range(1, 10)) = 8
         		_Scale2 ("Scale 2", Range(1, 10)) = 2
         		_FogOfWarData ("Data", Vector) = (0,0,2000,2000)
        }
   SubShader {
                Tags { "Queue"="Transparent" "RenderType"="Transparent" }
                Blend SrcAlpha OneMinusSrcAlpha
                LOD 200

        Pass {
                CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
                #pragma target 3.0
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                sampler2D _NoiseTex;
                float _Speed, _Scale1, _Scale2;
                half4 _FogOfWarData;
                float3 _Direction;
                fixed4 _Color;

                struct appdata {
    				float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
    			};

				struct v2f {
	    			float4 pos : SV_POSITION;
	    			float3 worldPos: TEXCOORD0;
	    			float2 uv: TEXCOORD1;
	    			float2 uv2: TEXCOORD2;
				};

				v2f vert(appdata v) {
    				v2f o;
    				o.pos = UnityObjectToClipPos(v.vertex);
    				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
    				float2 ndir = normalize(_Direction.xy);
    				o.uv = (v.texcoord + _Time.yy * _Speed * _Direction.xy) * _Scale1;
    				o.uv2 = (v.texcoord + _Time.yy * _Speed * _Direction.xy * 3.5) * _Scale2;
    				return o;
    			}

    			fixed4 frag (v2f i) : SV_Target {
            	    half2 fogTexCoord = (i.worldPos.xz - _FogOfWarData.xy) / _FogOfWarData.zw + 0.5.xx;
                	fixed fogAlpha = tex2D (_MainTex, fogTexCoord).a;
					fixed4 fog1 = tex2D(_NoiseTex, i.uv);
					fixed4 fog2 = tex2D(_NoiseTex, i.uv2);
                    fixed4 fog = (fog1 + fog2) * 0.5;
                    return fixed4(fog.rgb * _Color.rgb, fogAlpha);
                }
                ENDCG
               }
		}
	}
