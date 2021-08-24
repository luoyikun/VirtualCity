Shader "AQUAS/Mobile/Bicolored Foamy" {
    Properties {
        [NoScaleOffset]_NormalTexture ("Normal Texture", 2D) = "white" {}
        _NormalTiling ("Normal Tiling", Float ) = 1
        _DeepWaterColor ("Deep Water Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _ShallowWaterColor ("Shallow Water Color", Color) = (0.4411765,0.9537525,1,1)
        _DepthTransparency ("Depth Transparency", Float ) = 1.5
        _ShoreFade ("Shore Fade", Float ) = 0.3
        _ShoreTransparency ("Shore Transparency", Float ) = 0.04
        _ShallowDeepBlend ("Shallow-Deep-Blend", Float ) = 3.6
        _Fade ("Shallow-Deep-Fade", Float ) = 3
        [HideInInspector]_ReflectionTex ("Reflection Tex", 2D) = "white" {}
        [MaterialToggle] _UseReflections ("Enable Reflections", Float ) = 0.08586914
        _Reflectionintensity ("Reflection intensity", Range(0, 1)) = 0.5
        _Distortion ("Distortion", Range(0, 2)) = 0.3
        _Specular ("Specular", Float ) = 1
        _SpecularColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
        _Gloss ("Gloss", Float ) = 0.8
        _LightWrapping ("Light Wrapping", Float ) = 1.5
        _Refraction ("Refraction", Range(0, 1)) = 0.5
        _WaveSpeed ("Wave Speed", Float ) = 40
        [NoScaleOffset]_FoamTexture ("Foam Texture", 2D) = "white" {}
        _FoamTiling ("Foam Tiling", Float ) = 3
        _FoamBlend ("Foam Blend", Float ) = 0.15
        _FoamVisibility ("Foam Visibility", Range(0, 1)) = 0.3
        _FoamIntensity ("Foam Intensity", Float ) = 5
        _FoamContrast ("Foam Contrast", Range(0, 0.5)) = 0.25
        _FoamColor ("Foam Color", Color) = (0.5,0.5,0.5,1)
        _FoamSpeed ("Foam Speed", Float ) = 120
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ColorMask RGB
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal xboxone ps4 psp2 n3ds wiiu 
            #pragma target 2.0
            uniform float4 _LightColor0;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _DeepWaterColor;
            uniform float4 _ShallowWaterColor;
            uniform float _ShallowDeepBlend;
            uniform float _Fade;
            uniform sampler2D _ReflectionTex; uniform float4 _ReflectionTex_ST;
            uniform float _Reflectionintensity;
            uniform fixed _UseReflections;
            uniform float _DepthTransparency;
            uniform float _Specular;
            uniform float _Gloss;
            uniform float _LightWrapping;
            uniform sampler2D _NormalTexture;
            uniform float _Refraction;
            uniform float _WaveSpeed;
            uniform float _Distortion;
            uniform float4 _SpecularColor;
            uniform float _NormalTiling;
            uniform float _FoamVisibility;
            uniform float _FoamBlend;
            uniform float4 _FoamColor;
            uniform float _FoamIntensity;
            uniform float _FoamContrast;
            uniform sampler2D _FoamTexture;
            uniform float _FoamSpeed;
            uniform float _FoamTiling;
            uniform float _ShoreFade;
            uniform float _ShoreTransparency;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 projPos : TEXCOORD5;
                UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                float3 objScale = 1.0/recipObjScale;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                float3 objScale = 1.0/recipObjScale;
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float _rotator1_ang = 1.5708;
                float _rotator1_spd = 1.0;
                float _rotator1_cos = cos(_rotator1_spd*_rotator1_ang);
                float _rotator1_sin = sin(_rotator1_spd*_rotator1_ang);
                float2 _rotator1_piv = float2(0.5,0.5);
                float2 _rotator1 = (mul(i.uv0-_rotator1_piv,float2x2( _rotator1_cos, -_rotator1_sin, _rotator1_sin, _rotator1_cos))+_rotator1_piv);
                float2 _division1 = ((objScale.rb*_NormalTiling)/1000.0);
                float4 _timer1 = _Time;
                float3 _multiplier3 = (float3((_WaveSpeed/_division1),0.0)*(_timer1.r/100.0));
                float2 _multiplier1 = ((_rotator1+_multiplier3)*_division1);
                float4 _texture1 = tex2D(_NormalTexture,_multiplier1);
                float2 _multiplier2 = ((i.uv0+_multiplier3)*_division1);
                float4 _texture2 = tex2D(_NormalTexture,_multiplier2);
                float3 _subtractor1 = (_texture1.rgb-_texture2.rgb);
                float3 normalLocal = lerp(float3(0,0,1),_subtractor1,_Refraction);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _Gloss;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float3 specularColor = (_Specular*_SpecularColor.rgb);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 w = float3(_LightWrapping,_LightWrapping,_LightWrapping)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = forwardLight * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 _power = pow(saturate(max(_DeepWaterColor.rgb,(_ShallowWaterColor.rgb*(saturate((sceneZ-partZ)/_ShallowDeepBlend)*-1.0+1.0)))),_Fade);
                float2 _componentMask = _subtractor1.rg;
                float2 _remap = (((sceneUVs * 2 - 1).rg+(float2(_componentMask.r,_componentMask.g)*_Distortion))*0.5+0.5);
                float4 _ReflectionTex_var = tex2D(_ReflectionTex,TRANSFORM_TEX(_remap, _ReflectionTex));
                float _rotator2_ang = 1.5708;
                float _rotator2_spd = 1.0;
                float _rotator2_cos = cos(_rotator2_spd*_rotator2_ang);
                float _rotator2_sin = sin(_rotator2_spd*_rotator2_ang);
                float2 _rotator2_piv = float2(0.5,0.5);
                float2 _rotator2 = (mul(i.uv0-_rotator2_piv,float2x2( _rotator2_cos, -_rotator2_sin, _rotator2_sin, _rotator2_cos))+_rotator2_piv);
                float2 _division2 = ((objScale.rb*_FoamTiling)/1000.0);
                float4 _multiplier8 = _Time;
                float3 _multiplier7 = (float3((_FoamSpeed/_division2),0.0)*(_multiplier8.r/100.0));
                float2 _multiplier5 = ((_rotator2+_multiplier7)*_division2);
                float4 _texture3 = tex2D(_FoamTexture,_multiplier5);
                float2 _multiplier6 = ((i.uv0+_multiplier7)*_division2);
                float4 _texture4 = tex2D(_FoamTexture,_multiplier6);
                float _value = 0.0;
                float3 _multiplier4 = ((((_value + ( (dot((_texture3.rgb-_texture4.rgb),float3(0.3,0.59,0.11)) - _FoamContrast) * (1.0 - _value) ) / ((1.0 - _FoamContrast) - _FoamContrast))*_FoamColor.rgb)*(_FoamIntensity*(-1.0)))*(saturate((sceneZ-partZ)/_FoamBlend)*-1.0+1.0));
                float3 diffuseColor = lerp(lerp( _power, lerp(_ReflectionTex_var.rgb,_power,(1.0 - _Reflectionintensity)), _UseReflections ),(_multiplier4*_multiplier4),_FoamVisibility);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor,(saturate((sceneZ-partZ)/_ShoreTransparency)*pow(saturate((sceneZ-partZ)/_DepthTransparency),_ShoreFade)));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
