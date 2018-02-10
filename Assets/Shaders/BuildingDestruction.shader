// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Toolbox/BuildingDestruction" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_TransitionTex("Transition Tex", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Cutoff("Cutoff", Range(0,1)) = 0
		_Fade("Fade", Range(0, 1)) = 0
	}
		SubShader{
			Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 100

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct appdata {
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f {
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
		float2 uv1 : TEXCOORD1;
	};

	sampler2D _MainTex;
	uniform float4 _MainTex_TexelSize;
	sampler2D _TransitionTex;
	fixed4 _Color;
	float _Cutoff;
	float _Fade;


	v2f vert(appdata v) {
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		o.uv1 = v.uv;

#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			o.uv1.xy = 1 - o.uv1.xy;
#endif
		return o;
	}
		fixed4 frag(v2f i) : SV_TARGET{
			fixed4 transit = tex2D(_TransitionTex, i.uv1);
			fixed4 col = tex2D(_MainTex, i.uv);
			if (transit.r > _Cutoff)
				 col = lerp(col, _Color, _Fade);
				//col = _Color;

			//col.a = 0;

			return col;
		}
		ENDCG
	}
	}
		FallBack "Diffuse"
}
