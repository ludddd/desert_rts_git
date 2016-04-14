Shader "Custom/MobileDiffuseFOW" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 150

CGPROGRAM
#pragma surface surf Lambert noforwardadd

sampler2D _MainTex;
sampler2D _FogOfWar;
float4x4 _FOWToUV;

struct Input {
	float3 worldPos;
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
    float4 wPos = float4(IN.worldPos.x, IN.worldPos.y, IN.worldPos.z, 1);
    float4 uv = mul(wPos, _FOWToUV);
    fixed4 fowColor = tex2D(_FogOfWar, uv.xy);
	o.Albedo = (c * fowColor).rgb;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Mobile/Diffuse"
}
