Shader "Custom/CRT_Curvature"
{
   Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Curvature ("Curvature", Range(0, 0.5)) = 0.15
        _Vignette ("Vignette", Range(0, 1)) = 0.25
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float _Curvature;
            float _Vignette;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float2 Warp(float2 uv)
            {
                float2 center = uv * 2.0 - 1.0;
                float r = dot(center, center);
                center *= 1.0 + _Curvature * r;
                return center * 0.5 + 0.5;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 warpedUV = Warp(i.uv);

                // discard outside
                if (warpedUV.x < 0 || warpedUV.x > 1 || warpedUV.y < 0 || warpedUV.y > 1)
                    return float4(0,0,0,1);

                fixed4 col = tex2D(_MainTex, warpedUV);

                // vignette
                float2 dist = abs(i.uv - 0.5);
                float vignette = 1.0 - (dist.x * dist.y * 4.0 * _Vignette);

                col.rgb *= vignette;

                return col;
            }
            ENDCG
        }
    }
}
