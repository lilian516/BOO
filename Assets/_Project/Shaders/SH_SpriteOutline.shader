Shader "Custom/SH_OutlineShadow"
{
    Properties
    {
        [PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
        _MainTex_TexelSize("MainTex Texel Size", Vector) = (1,1,1,1)
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineSize ("Outline Size", Float) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "CanUseSpriteAtlas"="True" }
        LOD 100
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _Color;
            float4 _OutlineColor;
            float _OutlineSize;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float alpha = tex2D(_MainTex, uv).a;

                if (_OutlineSize > 0.0 && alpha < 0.1)
                {
                    float outline = 0.0;
                    float2 offset = _MainTex_TexelSize.xy * _OutlineSize;

                    outline += tex2D(_MainTex, uv + float2(-offset.x, 0)).a;
                    outline += tex2D(_MainTex, uv + float2(offset.x, 0)).a;
                    outline += tex2D(_MainTex, uv + float2(0, -offset.y)).a;
                    outline += tex2D(_MainTex, uv + float2(0, offset.y)).a;
                    outline += tex2D(_MainTex, uv + float2(-offset.x, -offset.y)).a;
                    outline += tex2D(_MainTex, uv + float2(-offset.x, offset.y)).a;
                    outline += tex2D(_MainTex, uv + float2(offset.x, -offset.y)).a;
                    outline += tex2D(_MainTex, uv + float2(offset.x, offset.y)).a;

                    if (outline > 0.0)
                        return _OutlineColor;
                }

                fixed4 col = tex2D(_MainTex, uv) * _Color;
                if (col.a < 0.1) discard;
                return col;
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
