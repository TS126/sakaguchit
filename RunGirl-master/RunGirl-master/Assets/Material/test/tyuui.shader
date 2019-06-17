Shader "Custom/tyuui" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="TransparentCutout" "Queue" = "AlphaTest" }
		LOD 200
		cull off

		CGPROGRAM
		#pragma surface surf Standard alpha:fade
		#pragma target 3.0

		

		struct Input {
			float2 uv_MainTex;
		};

		sampler2D _MainTex;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Transparent/Cutout/Diffuse"
}
