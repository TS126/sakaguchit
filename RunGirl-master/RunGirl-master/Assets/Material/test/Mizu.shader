Shader "Custom/Mizu" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_Metallic("Metallic",Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType" = "Opaque"}
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard alpha:fade
		#pragma target 3.0


		struct Input {
			float3 worldNormal;
			float3 viewDir;
		};

		fixed4 _Color;
		half _Metallic;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = _Color;
			float alpha = 1 - (abs(dot(IN.worldNormal, IN.viewDir)));
			o.Alpha = alpha * 0.9f;
			o.Metallic = _Metallic;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
