Shader "Custom/FogUnlitDepth"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.75, 0.8, 0.85, 0.4)
        _MainTex ("Texture", 2D) = "white" {}
        _DepthFade ("Depth Fade", Range(0.01, 5)) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Pass
        {
            Name "Forward"
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // includes de URP
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _MainTex_ST;
                float _DepthFade;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            float4 frag (Varyings IN) : SV_Target
            {
                // color base
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * _BaseColor;

                // coordenadas de pantalla para muestrear depth
                float2 uv = IN.positionCS.xy / IN.positionCS.w;

                // profundidad de la escena (0..1)
                float sceneDepth01 = SampleSceneDepth(uv);
                // convertir a profundidad de ojo
                float sceneDepth = LinearEyeDepth(sceneDepth01, _ZBufferParams);

                // profundidad de este fragmento
                float myDepth = LinearEyeDepth(IN.positionCS.z / IN.positionCS.w, _ZBufferParams);

                // fade según la diferencia
                float fade = saturate( (sceneDepth - myDepth) / _DepthFade );

                col.a *= fade;

                return col;
            }
            ENDHLSL
        }
    }
}
