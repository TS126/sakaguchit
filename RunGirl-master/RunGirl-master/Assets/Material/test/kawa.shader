Shader "Custom/kawa" {
	Properties {
		_MainTex("WaterTexture",2D) = "white"{}
		_NormalTex("Normal Tex", 2D) = "bump"{}
		_Distortion("Distortion", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		Tags { "Queue" = "Transparent"}
		LOD 200

		GrabPass{}

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _GrabTexture;
		sampler2D _NormalTex;
		float _Distortion;

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalTex;
			float4 screenPos;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float2 grabUV = (IN.screenPos.xy / IN.screenPos.w);
			float2 uv = IN.uv_MainTex;
			fixed2 normalTex = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex)).rg;
			grabUV += normalTex * _Distortion;

			uv.x += 2 * _Time;
			uv.y += 1 * _Time;

			fixed3 grab = tex2D(_GrabTexture, grabUV).rgb;

			o.Emission = grab;
			o.Albedo = tex2D(_MainTex,uv);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
