Shader "TOZ/Debug/FlowColors" {
	Properties {
		_FlowTex("Flow Texture", 2D) = "gray" {}
	}

	SubShader {
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry" "IgnoreProjector" = "True" }
		LOD 100

		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }

			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing

			sampler2D _FlowTex;

			struct a2v {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 coord0 : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert(a2v v) {
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_TRANSFER_INSTANCE_ID(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.coord0 = v.texcoord.xy;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed3 result = tex2D(_FlowTex, i.coord0.xy).rgb;
				return fixed4(result, 1.0);
			}
			ENDCG
		}
	}

	Fallback Off
}