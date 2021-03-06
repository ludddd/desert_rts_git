Shader "Particles/Alpha Blended" {
    Properties{
        _MainTex("Particle Texture", 2D) = "white" {}
    }

        Category{
        Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        Cull Off Lighting Off ZWrite Off

        SubShader{
        Pass{

        CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_particles
#pragma multi_compile_fog

#include "UnityCG.cginc"

    sampler2D _MainTex;
    sampler2D _FogOfWar;


    struct appdata_t {
        float4 vertex : POSITION;
        fixed4 color : COLOR;
        float2 texcoord : TEXCOORD0;
    };

    struct v2f {
        float4 vertex : SV_POSITION;
        fixed4 color : COLOR;
        float2 texcoord : TEXCOORD0;
        float4 projPos : TEXCOORD2;
    };

    float4 _MainTex_ST;

    v2f vert(appdata_t v)
    {
        v2f o;
        o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
        o.color = v.color;
        o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
        o.projPos = ComputeScreenPos(o.vertex);
        return o;
    }

    fixed4 frag(v2f i) : SV_Target
    {
    fixed4 fowColor = tex2D(_FogOfWar, i.projPos.xy / i.projPos.w);
    fixed4 col = i.color * tex2D(_MainTex, i.texcoord);
    col.rgb *= fowColor.rgb;
    return col;
    }
        ENDCG
    }
    }
    }
}
