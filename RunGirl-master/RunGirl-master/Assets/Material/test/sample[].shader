Shader "Custom/sample" {
	Properties{
		_NormalTex("Normal Tex",2D) = "bump"{}
		_Distortion("Distortion",Float) = 1
		_Color("Color",Color) = (1,1,1,1)
	}

	SubShader{
		Tags{
		"Queue" = "Transparent"
		"RenderType" = "Transparent"
	}

		GrabPass{}
		Cull off
		CGPROGRAM
#pragma target 3.0
#pragma surface surf Standard fullforwardshadows

		sampler2D _GrabTexture;
		sampler2D _NormalTex;
		float _Distortion;
		fixed4 _Color;

	struct Input {
		float2 uv_NormalTex;
		float4 screenPos;
	};

	void surf(Input IN, inout SurfaceOutputStandard o) {
		float2 grabUV = (IN.screenPos.xy / IN.screenPos.w );
		float2 normalTex = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex)).rg;
		grabUV += normalTex * _Distortion;
		fixed3 grab = tex2D(_GrabTexture, grabUV).rgb;

		o.Emission = grab;
		o.Albedo = _Color.rgb;
	}
	ENDCG
	}

		FallBack "Transparent/Diffuse"
}