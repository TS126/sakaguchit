Shader "Custom/road" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType" = "Opaque"
			   "Queue" = "Transparent"}
		LOD 200
		cull off

		CGPROGRAM
		#pragma surface surf Standard alpha:fade
		#pragma target 3.0


		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Alpha = 0.6f;
			o.Albedo = _Color.rgb;

		}
		ENDCG
	}
	FallBack "Diffuse"
}
