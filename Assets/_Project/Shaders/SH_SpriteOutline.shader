Shader "CustomRenderTexture/SH_SpriteOutline"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineThickness("Outline Thickness", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Cull Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            Name "SH_SpriteOutline"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _OutlineColor;
            float _OutlineThickness;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                float alpha = tex2D(_MainTex, input.uv).a;

                float outline = 0.0;
                float2 offsets[8] = {
                    float2(-1, 0), float2(1, 0),
                    float2(0, -1), float2(0, 1),
                    float2(-1, -1), float2(-1, 1),
                    float2(1, -1), float2(1, 1)
                };

                for (int k = 0; k < 8; k++)
                {
                    float2 offsetUV = input.uv + offsets[k] * _MainTex_TexelSize.xy * _OutlineThickness;
                    outline += tex2D(_MainTex, offsetUV).a;
                }

                if (alpha < 0.01 && outline > 0.01)
                {
                    return _OutlineColor;
                }

                fixed4 col = tex2D(_MainTex, input.uv);
                col.rgb *= col.a;
                return col;
            }
            ENDCG
        }
    }
}
