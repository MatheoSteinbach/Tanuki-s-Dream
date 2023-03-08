Shader "TOZ/Water/Flow" {
	Properties {
		[Header(Bumpmap Properties)]
		_BumpTex0("Bump Texture0", 2D) = "bump" {}
		_BumpTex1("Bump Texture1", 2D) = "bump" {}
		_BumpSpeed("Bump Speed", Range(0.0, 0.1)) = 0.01
		[Space]
		[Header(Flow Properties)]
		[NoScaleOffset]_FlowTex("Flow Texture", 2D) = "black" {}
		[NoScaleOffset]_NoiseTex("Noise Texture", 2D) = "white" {}
		_FlowSpeed("Flow Speed", Range(-1.0, 1.0)) = 0.5
	}

	SubShader {
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry" "IgnoreProjector" = "True" }
		LOD 100
		
		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }

			Cull Off
			//ZWrite Off

			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing

			UNITY_DECLARE_TEXCUBE(_CubeTex);
			sampler2D _BumpTex0, _BumpTex1;
			float4 _BumpTex0_ST, _BumpTex1_ST;
			half _BumpSpeed;

			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(sampler2D, _FlowTex)
			UNITY_INSTANCING_BUFFER_END(Props)
			//sampler2D _FlowTex;
			sampler2D _NoiseTex;
			half _FlowSpeed;

			static const half _FullCycle = 1.0;
			static const half _HalfCycle = _FullCycle * 0.5;

			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float4 coord0 : TEXCOORD0; 	// Bump Textures
				float4 TBNW0 : TEXCOORD1;
				float4 TBNW1 : TEXCOORD2;
				float4 TBNW2 : TEXCOORD3;
				float3 eye : TEXCOORD4;
				float4 coord1 : TEXCOORD5; 	// Flow(xy) and Noise(xy)
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert(a2v v) {
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.pos = UnityObjectToClipPos(v.vertex);

				float2 uv = TRANSFORM_TEX(v.texcoord, _BumpTex0);
				float offset = frac(_Time.y * _BumpSpeed);
				o.coord0.xy = uv + offset;
				o.coord0.zw = uv - offset;

				float3 pW = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 nW = UnityObjectToWorldNormal(v.normal);
				fixed3 tW = UnityObjectToWorldDir(v.tangent.xyz);
				fixed3 bW = cross(nW, tW) * (v.tangent.w * unity_WorldTransformParams.w);
				o.TBNW0 = float4(tW.x, bW.x, nW.x, pW.x);
				o.TBNW1 = float4(tW.y, bW.y, nW.y, pW.y);
				o.TBNW2 = float4(tW.z, bW.z, nW.z, pW.z);

				o.eye = WorldSpaceViewDir(v.vertex);

				o.coord1.xy = TRANSFORM_TEX(v.texcoord, _BumpTex1);
				o.coord1.zw = v.texcoord.xy;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

				//Flow
				half4 flowTex = tex2D(UNITY_ACCESS_INSTANCED_PROP(Props, _FlowTex), i.coord1.zw);
				half2 flowDir = fixed2(flowTex.rg) * 2.0 - 1.0;
				half ofs0 = frac((_Time.y * _FlowSpeed) + 0.5) * _FullCycle;
				half ofs1 = frac((_Time.y * _FlowSpeed) + 1.0) * _FullCycle;
				float cycleOfs = tex2D(_NoiseTex, i.coord1.zw).r * 0.5;
				float2 phase0 = flowDir * (cycleOfs + ofs0);
				float2 phase1 = flowDir * (cycleOfs + ofs1);
				half blend = abs(_HalfCycle - ofs0) / _HalfCycle;

				//Normals
				half4 b0 = tex2D(_BumpTex1, i.coord1.xy + phase0);
				half4 b1 = tex2D(_BumpTex1, i.coord1.xy + phase1);
				half4 bump1 = lerp(b0, b1, blend);
				//fixed3 normal = UnpackNormal(bump);

				half4 bump0 = (tex2D(_BumpTex0, i.coord0.xy) + tex2D(_BumpTex0, i.coord0.zw)) * 0.5;

				half4 bump = lerp(bump0, bump1, flowTex.a);

				fixed3 normal = UnpackNormal(bump);
				fixed3 normalW;
				normalW.x = dot(i.TBNW0.xyz, normal);
				normalW.y = dot(i.TBNW1.xyz, normal);
				normalW.z = dot(i.TBNW2.xyz, normal);

				//Projections
				float3 eye = normalize(i.eye);
				float3 reflRay = reflect(-eye, normalW);

				//Water color
				fixed3 result = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, reflRay).rgb;

				//Result
				return fixed4(result, 1.0);
			}
			ENDCG
		}
	}

	FallBack Off
}