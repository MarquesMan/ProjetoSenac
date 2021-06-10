Shader "Custom/LightOnly"
{
     Properties {
         _MainTexture ("Main Texture", 2D) = "white" {}
         _Normal ("Normal", 2D) = "bump" {}
         _Glossiness ("Glossiness", Range(1, 30)) = 1
         _Specularity ("Specularity", Range(0, 4)) = 1
         _Opacity ("Opacity", Range(0, 1)) = 0
         _MainColor ("Main Color", Color) = (1,1,1,1)
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
             ZWrite Off
             
             AlphaToMask On
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #define UNITY_PASS_FORWARDBASE
             #define _GLOSSYENV 1
             #include "UnityCG.cginc"
             #include "Lighting.cginc"
             #include "UnityPBSLighting.cginc"
             #include "UnityStandardBRDF.cginc"
             #pragma multi_compile_fwdbase
             #pragma multi_compile_fog
             #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
             #pragma target 3.0
             uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
             uniform sampler2D _Normal; uniform float4 _Normal_ST;
             uniform float _Glossiness;
             uniform float _Specularity;
             uniform float _Opacity;
             uniform float4 _MainColor;
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
                 UNITY_FOG_COORDS(5)
             };
             VertexOutput vert (VertexInput v) {
                 VertexOutput o = (VertexOutput)0;
                 o.uv0 = v.texcoord0;
                 o.normalDir = UnityObjectToWorldNormal(v.normal);
                 o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                 o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                 o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                 float3 lightColor = _LightColor0.rgb;
                 o.pos = UnityObjectToClipPos( v.vertex );
                 UNITY_TRANSFER_FOG(o,o.pos);
                 return o;
             }
             float4 frag(VertexOutput i) : COLOR {
                 i.normalDir = normalize(i.normalDir);
                 float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                 float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                 float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                 float3 normalLocal = _Normal_var.rgb;
                 float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                 float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                 float3 lightColor = _LightColor0.rgb;
                 float3 halfDirection = normalize(viewDirection+lightDirection);
 ////// Lighting:
                 float attenuation = 1;
                 float4 _MainTexture_var = tex2D(_MainTexture,TRANSFORM_TEX(i.uv0, _MainTexture));
                 float3 finalColor = (attenuation*_LightColor0.rgb*(((_MainColor.rgb*_MainTexture_var.rgb)*max(0,dot(lightDirection,normalDirection)))+(pow(max(0,dot(normalDirection,halfDirection)),_Glossiness)*_Specularity)));
                 fixed4 finalRGBA = fixed4(finalColor,_Opacity);
                 UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                 return finalRGBA;
             }
             ENDCG
         }
         Pass {
             Name "FORWARD_DELTA"
             Tags {
                 "LightMode"="ForwardAdd"
             }
             Blend One One
             ZWrite Off
             
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #define UNITY_PASS_FORWARDADD
             #define _GLOSSYENV 1
             #include "UnityCG.cginc"
             #include "AutoLight.cginc"
             #include "Lighting.cginc"
             #include "UnityPBSLighting.cginc"
             #include "UnityStandardBRDF.cginc"
             #pragma multi_compile_fwdadd
             #pragma multi_compile_fog
             #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
             #pragma target 3.0
             uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
             uniform sampler2D _Normal; uniform float4 _Normal_ST;
             uniform float _Glossiness;
             uniform float _Specularity;
             uniform float _Opacity;
             uniform float4 _MainColor;
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
                 LIGHTING_COORDS(5,6)
                 UNITY_FOG_COORDS(7)
             };
             VertexOutput vert (VertexInput v) {
                 VertexOutput o = (VertexOutput)0;
                 o.uv0 = v.texcoord0;
                 o.normalDir = UnityObjectToWorldNormal(v.normal);
                 o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                 o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                 o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                 float3 lightColor = _LightColor0.rgb;
                 o.pos = UnityObjectToClipPos( v.vertex );
                 UNITY_TRANSFER_FOG(o,o.pos);
                 TRANSFER_VERTEX_TO_FRAGMENT(o)
                 return o;
             }
             float4 frag(VertexOutput i) : COLOR {
                 i.normalDir = normalize(i.normalDir);
                 float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                 float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                 float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                 float3 normalLocal = _Normal_var.rgb;
                 float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                 float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                 float3 lightColor = _LightColor0.rgb;
                 float3 halfDirection = normalize(viewDirection+lightDirection);
 ////// Lighting:
                 float attenuation = LIGHT_ATTENUATION(i);
                 float4 _MainTexture_var = tex2D(_MainTexture,TRANSFORM_TEX(i.uv0, _MainTexture));
                 float3 finalColor = (attenuation*_LightColor0.rgb*(((_MainColor.rgb*_MainTexture_var.rgb)*max(0,dot(lightDirection,normalDirection)))+(pow(max(0,dot(normalDirection,halfDirection)),_Glossiness)*_Specularity)));
                 fixed4 finalRGBA = fixed4(finalColor,0);
                 UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                 return finalRGBA;
             }
             ENDCG
         }
     }
     FallBack "Diffuse"
     // CustomEditor "ShaderForgeMaterialInspector"
}
