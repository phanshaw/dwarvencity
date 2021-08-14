Shader "Splat/SplatMax"
{
    Properties
    {
        _HeightMultiplier("Multiplier", float) = 1
        _BaseMap("Splat", 2D) = "white" {}
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
            //Blend SrcAlpha OneMinusSrcAlpha
            
            //ZWrite Off
			//ZTest LEqual
			//Offset 0 , 0
			//ColorMask RGBA
            
            LOD 100

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                float4 positionHCS  : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID 
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            CBUFFER_END

             UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float, _HeightMultiplier)
            UNITY_INSTANCING_BUFFER_END(Props)

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float heightMul = UNITY_ACCESS_INSTANCED_PROP(Props, _HeightMultiplier);
                return SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * heightMul;
            }
            ENDHLSL
        }
    }
}