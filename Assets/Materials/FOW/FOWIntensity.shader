Shader "Custom/FOW/Intensity"
{
    SubShader{

    Tags{ "ForceSupported" = "True" "RenderType" = "Overlay" }

    Lighting Off
    Blend SrcAlpha One
    Cull Off
    ZWrite Off
    ZTest Always

    Pass{
    CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

    struct appdata_t {
        float4 vertex : POSITION;
        float2 texcoord : TEXCOORD0;
    };

    struct v2f {
        float4 vertex : SV_POSITION;
        float2 texcoord : TEXCOORD0;
    };

    

    v2f vert(appdata_t v)
    {
        v2f o;
        o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
        o.texcoord = v.texcoord;
        return o;
    }

    fixed4 frag(v2f i) : SV_Target
    {
        float4 c = i.texcoord.x;
        return c;
    }
        ENDCG
    }
    }

        Fallback off
}

