Shader "Custom/sakana" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" 
			   "Queue" = "Transparent"}
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard alpha:fade
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float3 worldNormal;
			float3 viewDir;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = _Color.rgb;
			o.Alpha = (abs(dot(IN.viewDir, IN.worldNormal))) * 0.5;
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}
