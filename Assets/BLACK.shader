Shader "Custom/BLACK" {
	Properties {
		_Color ("Color", Color) = (0.5, 0.5, 0.5, 0.5)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf None

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
		};
		
		half4 LightingNone(SurfaceOutput s, half3 lightDir, half atten) {
			half4 c = _Color;	
//			c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
//        	c.a = s.Alpha;
        	return _Color;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c;// = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
