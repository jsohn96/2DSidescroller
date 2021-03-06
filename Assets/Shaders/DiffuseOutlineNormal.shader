﻿Shader "Custom/DiffuseOutlineNormal" {
	Properties {
		[Toggle] _UseMainTexture("Use Main Texture", Int) = 0
	    _MainTex ("Main Texture (RGB)", 2D) = "white" {}
	    [Toggle] _UseColor("Use Color", Int) = 0
	    _Color ("Color", Color) = (1,1,1,1)
	    [Toggle] _UseOutlineColor("Use Outline Color", Int) = 0
	    _OutlineColor("Outline Color", Color) = (1,1,1,1)
	    [Toggle] _UseOutlineWidth("Use Outline Width", Int) = 0
	    _OutlineWidth("Outline Width", Range(0.0, 1.1)) = 1.05
	    [Toggle] _UseNormalExtrusion("Use Normal Extrusion", Int) = 0
	    _OutlineNormalExtrusion("Outline Normal Extrusion", Range(0, 1)) = 0.03
	}
	SubShader {
		Tags {
			"Queue" = "Transparent"
		}
		Pass {
			Name "OUTLINE"
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert

			#pragma fragment frag
			#include "UnityCG.cginc"


			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;
			};

			float4 _OutlineColor;
			float _OutlineWidth;
			float _OutlineNormalExtrusion;

			v2f vert (appdata IN){
				IN.vertex.xyz *= _OutlineWidth;
				v2f OUT;

				OUT.pos = UnityObjectToClipPos(IN.vertex);

				float3 norm = mul ((float3x3)UNITY_MATRIX_IT_MV, IN.normal);
				float2 offset = TransformViewToProjection(norm.xy);
				OUT.pos.xy += offset * OUT.pos.z * _OutlineNormalExtrusion;

				return OUT;
			}

			float4 frag (v2f IN) : SV_Target {
				return _OutlineColor;
			}
			ENDCG

		}

		CGPROGRAM
		#pragma surface surf Lambert


		struct Input {
		    float2 uv_MainTex;
		};

		sampler2D _MainTex;
		float4 _Color;

		void surf (Input IN, inout SurfaceOutput OUT) {
		    fixed4 color = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		    OUT.Albedo = color.rgb;
		    OUT.Alpha = color.a;
		}
		ENDCG

	}

	Fallback "Diffuse"
	CustomEditor "CustomShaderGUI"
}
