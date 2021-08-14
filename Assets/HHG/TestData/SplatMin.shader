Shader "Splat/SplatMin"
{
    Properties
    {
        [MainTexture] _BaseMap("Splat", 2D) = "white" {}
    }

    // Universal Render Pipeline subshader. If URP is installed this will be used.
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline"}

        Pass
        {
            Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
            
            //Blend One One, One OneMinusSrcAlpha
			BlendOp Max
            Blend One One
            
            ZWrite Off
			ZTest LEqual
			//Offset 0 , 0
			ColorMask RGBA
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                float4 positionHCS  : SV_POSITION;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 sample = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) ;
                return sample;
            }
            ENDHLSL
        }
    }
}