// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Mobile/OpaqueDiffuseWithColor" {
Properties {
	_Color("Color", Color) = (1, 1, 1, 1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Tags{  "RenderType"="Opaque" }
	Blend SrcAlpha OneMinusSrcAlpha
	

CGPROGRAM
#pragma surface surf Lambert noforwardadd

sampler2D _MainTex;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = _Color.rgb * c.rgb;
	o.Alpha = _Color.a * c.a;
}
ENDCG
}
                                                
Fallback "Mobile/VertexLit"
}
