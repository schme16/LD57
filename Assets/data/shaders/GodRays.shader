// Create a new shader file called "GodRays.shader"
Shader "Custom/GodRays"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightColor ("Light Color", Color) = (1, 0.95, 0.8, 1)
        _RayDensity ("Ray Density", Range(1, 100)) = 30
        _RayIntensity ("Ray Intensity", Range(0, 1)) = 0.5
        _RayLength ("Ray Length", Range(0, 1)) = 0.5
        _LightPos ("Light Position", Vector) = (0.5, 0.5, 0, 0)
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline"
        }
        LOD 100

        ZWrite Off
        Blend One One
        Cull Back

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _LightColor;
            float _RayDensity;
            float _RayIntensity;
            float _RayLength;
            float4 _LightPos;

            Varyings vert(Attributes input) {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            half4 frag(Varyings input) : SV_Target {
                // Direction from UV to light position
                float2 delta = input.uv - _LightPos.xy;
                float dist = length(delta);

                // Sample the base texture
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);

                // Calculate god rays
                half rays = 0;

                // Iterate to create rays
                for (int i = 0; i < _RayDensity; i++)
                {
                    float scale = 1.0 - _RayLength * (i / _RayDensity);
                    float2 uvRay = _LightPos.xy + delta * scale;

                    // Sample the scene depth at this position
                    half raySample = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvRay).r;
                    rays += raySample;
                }

                // Average and apply intensity
                rays /= _RayDensity;
                rays *= _RayIntensity;

                // Apply light color and fade with distance
                half4 rayColor = _LightColor * rays * max(0, 1.0 - dist * 2.0);

                return rayColor;
            }
            ENDHLSL
        }
    }
}