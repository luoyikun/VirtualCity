// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.05 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.05;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1692,x:34180,y:32612,varname:node_1692,prsc:2|diff-5040-OUT,emission-555-OUT,voffset-4154-OUT;n:type:ShaderForge.SFN_Tex2d,id:1792,x:31792,y:32326,ptovrint:False,ptlb:Noise_01,ptin:_Noise_01,varname:node_1792,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-2950-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:6737,x:32628,y:32927,ptovrint:False,ptlb:Emission_02,ptin:_Emission_02,varname:node_6737,prsc:2,tex:ad1d78a1157365f449c1f4038a0cb978,ntxv:0,isnm:False|UVIN-5200-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1072,x:31318,y:32326,varname:node_1072,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:2950,x:31555,y:32326,varname:node_2950,prsc:2,spu:0.05,spv:0|UVIN-1072-UVOUT;n:type:ShaderForge.SFN_Color,id:3727,x:32702,y:32088,ptovrint:False,ptlb:Diffuse_Color,ptin:_Diffuse_Color,varname:node_3727,prsc:2,glob:False,c1:0.3068772,c2:0.6033648,c3:0.9485294,c4:1;n:type:ShaderForge.SFN_Multiply,id:5040,x:32702,y:32243,varname:node_5040,prsc:2|A-3727-RGB,B-2630-RGB;n:type:ShaderForge.SFN_TexCoord,id:2201,x:31318,y:32514,varname:node_2201,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:3716,x:31555,y:32514,varname:node_3716,prsc:2,spu:0.05,spv:0|UVIN-2201-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:4771,x:31315,y:32697,varname:node_4771,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:1564,x:31555,y:32697,varname:node_1564,prsc:2,spu:0,spv:0.03|UVIN-4771-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:6084,x:31792,y:32697,ptovrint:False,ptlb:Noise_02,ptin:_Noise_02,varname:node_6084,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-1564-UVOUT;n:type:ShaderForge.SFN_Panner,id:5200,x:32440,y:32927,varname:node_5200,prsc:2,spu:0,spv:0.05|UVIN-6629-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:6629,x:32241,y:32927,varname:node_6629,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:1496,x:32629,y:32620,ptovrint:False,ptlb:Emission_01,ptin:_Emission_01,varname:_node_6737_copy,prsc:2,tex:ad1d78a1157365f449c1f4038a0cb978,ntxv:0,isnm:False|UVIN-3652-OUT;n:type:ShaderForge.SFN_TexCoord,id:7380,x:31811,y:31959,varname:node_7380,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:9310,x:32014,y:31959,varname:node_9310,prsc:2,spu:0,spv:0.02|UVIN-7380-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:2630,x:32433,y:31959,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_2630,prsc:2,tex:dba111d458c33d34f9738dbfcd36f6a0,ntxv:0,isnm:False|UVIN-1826-OUT;n:type:ShaderForge.SFN_Add,id:3652,x:32446,y:32620,varname:node_3652,prsc:2|A-9197-UVOUT,B-6766-OUT;n:type:ShaderForge.SFN_Add,id:1826,x:32227,y:31959,varname:node_1826,prsc:2|A-9310-UVOUT,B-4158-OUT;n:type:ShaderForge.SFN_Multiply,id:6599,x:33016,y:32853,varname:node_6599,prsc:2|A-1496-R,B-6737-R;n:type:ShaderForge.SFN_Multiply,id:4158,x:32227,y:32107,varname:node_4158,prsc:2|A-1792-R,B-2074-OUT;n:type:ShaderForge.SFN_Slider,id:2074,x:31857,y:32127,ptovrint:False,ptlb:Water_Distort,ptin:_Water_Distort,varname:node_2074,prsc:2,min:0,cur:0.2,max:1;n:type:ShaderForge.SFN_Multiply,id:6766,x:32241,y:32676,varname:node_6766,prsc:2|A-4806-R,B-257-OUT,C-9107-OUT;n:type:ShaderForge.SFN_Multiply,id:884,x:33114,y:33489,varname:node_884,prsc:2|A-1792-RGB,B-247-OUT,C-193-OUT;n:type:ShaderForge.SFN_Vector1,id:247,x:32917,y:33491,varname:node_247,prsc:2,v1:0.5;n:type:ShaderForge.SFN_NormalVector,id:193,x:32917,y:33566,prsc:2,pt:False;n:type:ShaderForge.SFN_Slider,id:309,x:33147,y:33662,ptovrint:False,ptlb:The ups and downs,ptin:_Theupsanddowns,varname:node_309,prsc:2,min:0,cur:0.05,max:1;n:type:ShaderForge.SFN_Tex2d,id:4806,x:31792,y:32514,ptovrint:False,ptlb:Noise_03,ptin:_Noise_03,varname:_niuqu_03,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-3716-UVOUT;n:type:ShaderForge.SFN_Vector1,id:9107,x:32241,y:32833,varname:node_9107,prsc:2,v1:0.5;n:type:ShaderForge.SFN_TexCoord,id:9197,x:32241,y:32494,varname:node_9197,prsc:2,uv:0;n:type:ShaderForge.SFN_Power,id:1939,x:33534,y:32866,varname:node_1939,prsc:2|VAL-7083-OUT,EXP-5646-OUT;n:type:ShaderForge.SFN_Vector1,id:5646,x:33487,y:32995,varname:node_5646,prsc:2,v1:3.5;n:type:ShaderForge.SFN_Multiply,id:555,x:33872,y:32861,varname:node_555,prsc:2|A-4689-RGB,B-1651-OUT;n:type:ShaderForge.SFN_Color,id:4689,x:33842,y:32671,ptovrint:False,ptlb:Emission_Color,ptin:_Emission_Color,varname:node_4689,prsc:2,glob:False,c1:0.4980392,c2:0.9098039,c3:0.9960784,c4:1;n:type:ShaderForge.SFN_Add,id:7083,x:33294,y:32866,varname:node_7083,prsc:2|A-775-OUT,B-3916-OUT;n:type:ShaderForge.SFN_Vector1,id:3916,x:33294,y:32995,varname:node_3916,prsc:2,v1:0.7;n:type:ShaderForge.SFN_Multiply,id:4154,x:33304,y:33489,varname:node_4154,prsc:2|A-884-OUT,B-309-OUT;n:type:ShaderForge.SFN_OneMinus,id:257,x:31994,y:32697,varname:node_257,prsc:2|IN-6084-RGB;n:type:ShaderForge.SFN_Multiply,id:775,x:33016,y:33019,varname:node_775,prsc:2|A-6599-OUT,B-9227-OUT;n:type:ShaderForge.SFN_Vector1,id:9227,x:33016,y:33166,varname:node_9227,prsc:2,v1:5;n:type:ShaderForge.SFN_Add,id:2446,x:33167,y:32547,varname:node_2446,prsc:2|A-2630-R,B-125-OUT;n:type:ShaderForge.SFN_Slider,id:125,x:33010,y:32712,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_125,prsc:2,min:0,cur:0.3675209,max:1;n:type:ShaderForge.SFN_Multiply,id:9998,x:33402,y:32547,varname:node_9998,prsc:2|A-2446-OUT,B-1867-OUT;n:type:ShaderForge.SFN_Add,id:1651,x:33632,y:32671,varname:node_1651,prsc:2|A-9998-OUT,B-1939-OUT;n:type:ShaderForge.SFN_Vector1,id:1867,x:33402,y:32692,varname:node_1867,prsc:2,v1:0.3;proporder:2630-6737-1496-1792-6084-4806-3727-4689-125-2074-309;pass:END;sub:END;*/

Shader "Shader Forge/Water_Shader" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Emission_02 ("Emission_02", 2D) = "white" {}
        _Emission_01 ("Emission_01", 2D) = "white" {}
        _Noise_01 ("Noise_01", 2D) = "white" {}
        _Noise_02 ("Noise_02", 2D) = "white" {}
        _Noise_03 ("Noise_03", 2D) = "white" {}
        _Diffuse_Color ("Diffuse_Color", Color) = (0.3068772,0.6033648,0.9485294,1)
        _Emission_Color ("Emission_Color", Color) = (0.4980392,0.9098039,0.9960784,1)
        _Emission ("Emission", Range(0, 1)) = 0.3675209
        _Water_Distort ("Water_Distort", Range(0, 1)) = 0.2
        _Theupsanddowns ("The ups and downs", Range(0, 1)) = 0.05
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _Noise_01; uniform float4 _Noise_01_ST;
            uniform sampler2D _Emission_02; uniform float4 _Emission_02_ST;
            uniform float4 _Diffuse_Color;
            uniform sampler2D _Noise_02; uniform float4 _Noise_02_ST;
            uniform sampler2D _Emission_01; uniform float4 _Emission_01_ST;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _Water_Distort;
            uniform float _Theupsanddowns;
            uniform sampler2D _Noise_03; uniform float4 _Noise_03_ST;
            uniform float4 _Emission_Color;
            uniform float _Emission;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                float4 node_4098 = _Time + _TimeEditor;
                float2 node_2950 = (o.uv0+node_4098.g*float2(0.05,0));
                float4 _Noise_01_var = tex2Dlod(_Noise_01,float4(TRANSFORM_TEX(node_2950, _Noise_01),0.0,0));
                v.vertex.xyz += ((_Noise_01_var.rgb*0.5*v.normal)*_Theupsanddowns);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 node_4098 = _Time + _TimeEditor;
                float2 node_2950 = (i.uv0+node_4098.g*float2(0.05,0));
                float4 _Noise_01_var = tex2D(_Noise_01,TRANSFORM_TEX(node_2950, _Noise_01));
                float2 node_1826 = ((i.uv0+node_4098.g*float2(0,0.02))+(_Noise_01_var.r*_Water_Distort));
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(node_1826, _Diffuse));
                float3 diffuse = (directDiffuse + indirectDiffuse) * (_Diffuse_Color.rgb*_Diffuse_var.rgb);
////// Emissive:
                float2 node_3716 = (i.uv0+node_4098.g*float2(0.05,0));
                float4 _Noise_03_var = tex2D(_Noise_03,TRANSFORM_TEX(node_3716, _Noise_03));
                float2 node_1564 = (i.uv0+node_4098.g*float2(0,0.03));
                float4 _Noise_02_var = tex2D(_Noise_02,TRANSFORM_TEX(node_1564, _Noise_02));
                float3 node_3652 = (float3(i.uv0,0.0)+(_Noise_03_var.r*(1.0 - _Noise_02_var.rgb)*0.5));
                float4 _Emission_01_var = tex2D(_Emission_01,TRANSFORM_TEX(node_3652, _Emission_01));
                float2 node_5200 = (i.uv0+node_4098.g*float2(0,0.05));
                float4 _Emission_02_var = tex2D(_Emission_02,TRANSFORM_TEX(node_5200, _Emission_02));
                float3 emissive = (_Emission_Color.rgb*(((_Diffuse_var.r+_Emission)*0.3)+pow((((_Emission_01_var.r*_Emission_02_var.r)*5.0)+0.7),3.5)));
/// Final Color:
                float3 finalColor = diffuse + emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _Noise_01; uniform float4 _Noise_01_ST;
            uniform sampler2D _Emission_02; uniform float4 _Emission_02_ST;
            uniform float4 _Diffuse_Color;
            uniform sampler2D _Noise_02; uniform float4 _Noise_02_ST;
            uniform sampler2D _Emission_01; uniform float4 _Emission_01_ST;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _Water_Distort;
            uniform float _Theupsanddowns;
            uniform sampler2D _Noise_03; uniform float4 _Noise_03_ST;
            uniform float4 _Emission_Color;
            uniform float _Emission;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                float4 node_919 = _Time + _TimeEditor;
                float2 node_2950 = (o.uv0+node_919.g*float2(0.05,0));
                float4 _Noise_01_var = tex2Dlod(_Noise_01,float4(TRANSFORM_TEX(node_2950, _Noise_01),0.0,0));
                v.vertex.xyz += ((_Noise_01_var.rgb*0.5*v.normal)*_Theupsanddowns);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 node_919 = _Time + _TimeEditor;
                float2 node_2950 = (i.uv0+node_919.g*float2(0.05,0));
                float4 _Noise_01_var = tex2D(_Noise_01,TRANSFORM_TEX(node_2950, _Noise_01));
                float2 node_1826 = ((i.uv0+node_919.g*float2(0,0.02))+(_Noise_01_var.r*_Water_Distort));
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(node_1826, _Diffuse));
                float3 diffuse = directDiffuse * (_Diffuse_Color.rgb*_Diffuse_var.rgb);
/// Final Color:
                float3 finalColor = diffuse;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCollector"
            Tags {
                "LightMode"="ShadowCollector"
            }
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCOLLECTOR
            #define SHADOW_COLLECTOR_PASS
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcollector
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _Noise_01; uniform float4 _Noise_01_ST;
            uniform float _Theupsanddowns;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_COLLECTOR;
                float2 uv0 : TEXCOORD5;
                float3 normalDir : TEXCOORD6;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                float4 node_6501 = _Time + _TimeEditor;
                float2 node_2950 = (o.uv0+node_6501.g*float2(0.05,0));
                float4 _Noise_01_var = tex2Dlod(_Noise_01,float4(TRANSFORM_TEX(node_2950, _Noise_01),0.0,0));
                v.vertex.xyz += ((_Noise_01_var.rgb*0.5*v.normal)*_Theupsanddowns);
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_SHADOW_COLLECTOR(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                SHADOW_COLLECTOR_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Cull Off
            Offset 1, 1
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _Noise_01; uniform float4 _Noise_01_ST;
            uniform float _Theupsanddowns;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                float4 node_933 = _Time + _TimeEditor;
                float2 node_2950 = (o.uv0+node_933.g*float2(0.05,0));
                float4 _Noise_01_var = tex2Dlod(_Noise_01,float4(TRANSFORM_TEX(node_2950, _Noise_01),0.0,0));
                v.vertex.xyz += ((_Noise_01_var.rgb*0.5*v.normal)*_Theupsanddowns);
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
