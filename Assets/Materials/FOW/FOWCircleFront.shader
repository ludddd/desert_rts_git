Shader "Custom/FOW/CircleFront"
{
    Properties{ 
        _MainTex("Texture", 2D) = "" {}
    }

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

    sampler2D _MainTex;
    float intensity;

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
        float u = 2 * distance(float2(0.5, 0.5), i.texcoord);
        float4 c = tex2D(_MainTex, float2(u, 0.5));
        //clip(c.a < 0.01f ? -1 : 1);        
        c.rgb = c.a * c.rgb;
        c.a = c.r;
        return c;
    }
        ENDCG
    }
    }

        Fallback off
}

