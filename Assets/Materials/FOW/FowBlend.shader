Shader "Custom/FOW/Blend"
{
    Properties{ 
        _MainTex("Texture", any) = "" {} 
        coeff("Explored Weight", Range(0, 1.0)) = 1.0
    }

        SubShader{

        Tags{ "ForceSupported" = "True" "RenderType" = "Overlay" }

        Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha
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

    sampler2D _MainTex;
    float coeff;

    uniform float4 _MainTex_ST;

    v2f vert(appdata_t v)
    {
        v2f o;
        o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
        o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
        return o;
    }

    fixed4 frag(v2f i) : SV_Target
    {
        float4 c = tex2D(_MainTex, i.texcoord);
        clip(c.a < 0.01f ? -1 : 1);
        c.a = 1.0f;
        c.rgb = coeff * c.a * c.rgb;
        return c;
    }
        ENDCG
    }
    }

        Fallback off
}

