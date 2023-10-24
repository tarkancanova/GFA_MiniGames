// This shader fills the mesh shape with a color predefined in the code.
Shader "Example/URPUnlitShaderBasic"
{
    // The properties block of the Unity shader. In this example this block is empty
    // because the output color is predefined in the fragment shader code.
    Properties
    {
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _AmbientStrength("Ambient Strength", Float) = 0.1
    }

    // The SubShader block containing the Shader code. 
    SubShader
    {
        // SubShader Tags define when and under which conditions a SubShader block or
        // a pass is executed.
        Tags
        {
            "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline"
        }

        Pass
        {
            // The HLSL code block. Unity SRP uses the HLSL language.
            HLSLPROGRAM
            // This line defines the name of the vertex shader. 
            #pragma vertex vert
            // This line defines the name of the fragment shader. 
            #pragma fragment frag

            // The Core.hlsl file contains definitions of frequently used HLSL
            // macros and functions, and also contains #include references to other
            // HLSL files (for example, Common.hlsl, SpaceTransforms.hlsl, etc.).
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // The structure definition defines which variables it contains.
            // This example uses the Attributes structure as an input structure in
            // the vertex shader.
            struct Attributes
            {
                // The positionOS variable contains the vertex positions in object
                // space.
                float4 positionOS : POSITION;
                float4 normalOS : NORMAL;
                float2 uv: TEXCOORD0;
            };

            struct Varyings
            {
                // The positions in this struct must have the SV_POSITION semantic.
                float4 positionHCS : SV_POSITION;
                float4 normalOS : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float2 uv: TEXCOORD2;
            };

            float colormap_red(float x)
            {
                if (x < 0.0)
                {
                    return 54.0 / 255.0;
                }
                else if (x < 20049.0 / 82979.0)
                {
                    return (829.79 * x + 54.51) / 255.0;
                }
                else
                {
                    return 1.0;
                }
            }

            float colormap_green(float x)
            {
                if (x < 20049.0 / 82979.0)
                {
                    return 0.0;
                }
                else if (x < 327013.0 / 810990.0)
                {
                    return (8546482679670.0 / 10875673217.0 * x - 2064961390770.0 / 10875673217.0) / 255.0;
                }
                else if (x <= 1.0)
                {
                    return (103806720.0 / 483977.0 * x + 19607415.0 / 483977.0) / 255.0;
                }
                else
                {
                    return 1.0;
                }
            }

            float colormap_blue(float x)
            {
                if (x < 0.0)
                {
                    return 54.0 / 255.0;
                }
                else if (x < 7249.0 / 82979.0)
                {
                    return (829.79 * x + 54.51) / 255.0;
                }
                else if (x < 20049.0 / 82979.0)
                {
                    return 127.0 / 255.0;
                }
                else if (x < 327013.0 / 810990.0)
                {
                    return (792.02249341361393720147485376583 * x - 64.364790735602331034989206222672) / 255.0;
                }
                else
                {
                    return 1.0;
                }
            }

            float4 colormap(float x)
            {
                return float4(colormap_red(x), colormap_green(x), colormap_blue(x), 1.0);
            }

            // https://iquilezles.org/articles/warp
            /*float noise( in vec2 x )
            {
                vec2 p = floor(x);
                vec2 f = fract(x);
                f = f*f*(3.0-2.0*f);
                float a = textureLod(iChannel0,(p+vec2(0.5,0.5))/256.0,0.0).x;
                float b = textureLod(iChannel0,(p+vec2(1.5,0.5))/256.0,0.0).x;
                float c = textureLod(iChannel0,(p+vec2(0.5,1.5))/256.0,0.0).x;
                float d = textureLod(iChannel0,(p+vec2(1.5,1.5))/256.0,0.0).x;
                return mix(mix( a, b,f.x), mix( c, d,f.x),f.y);
            }*/


            float rand(float2 n)
            {
                return frac(sin(dot(n, float2(12.9898, 4.1414))) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 ip = floor(p);
                float2 u = frac(p);
                u = u * u * (3.0 - 2.0 * u);

                float res = lerp(
                    lerp(rand(ip), rand(ip + float2(1.0, 0.0)), u.x),
                    lerp(rand(ip + float2(0.0, 1.0)), rand(ip + float2(1.0, 1.0)), u.x), u.y);
                return res * res;
            }

            float fbm(float2 p)
            {
                float G = exp2(-0.5);
                float f = 1.0;
                float a = 1.0;
                float t = 0.0;
                for( int i=0; i<4; i++ )
                {
                    t += a*noise(f*(p + _Time.x * (i * 0.4)));
                    f *= 2.0;
                    a *= G;
                }
                return t;
            }

            float pattern(float2 p)
            {
                return fbm(p + fbm(p + fbm(p)));
            }

            float sqr(float x)
            {
                return x * x;
            }

            // The vertex shader definition with properties defined in the Varyings 
            // structure. The type of the vert function must match the type (struct)
            // that it returns.
            Varyings vert(Attributes IN)
            {
                // Declaring the output object (OUT) with the Varyings struct.
                Varyings OUT;
                // The TransformObjectToHClip function transforms vertex positions
                // from object space to homogenous space
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalOS = IN.normalOS;
                OUT.positionWS = TransformObjectToWorld(IN.normalOS);
                OUT.uv = IN.uv;

                // Returning the output.
                return OUT;
            }

            // The fragment shader definition.            
            half4 frag(Varyings IN) : SV_Target
            {
                // Defining the color variable and returning it.
                half3 normalWS = TransformObjectToWorldNormal(IN.normalOS);

                Light mainLight = GetMainLight();

                float ldn = max(0, dot(normalWS, mainLight.direction));
                float3 diffuseColor = (ldn * float4(mainLight.color.rgb, 1));


                float3 view = normalize(_WorldSpaceCameraPos - IN.positionWS);
                float3 reflectDir = reflect(-mainLight.direction, normalWS);

                float spec = pow(max(dot(view, reflectDir), 0.0), 250);
                float3 specular = 0.5 * spec * mainLight.color * ldn;

                for (int i = 0; i < _AdditionalLightsCount.x; i++)
                {
                    Light l = GetAdditionalLight(i, IN.positionWS);
                    diffuseColor.rgb += l.color * l.distanceAttenuation;
                }

                float3 ambientColor = 0.1 * mainLight.color;



                float shade = 1 - pattern(IN.uv * 5);



                return float4(colormap(shade).rgb, shade);

                // return float4((ambientColor + diffuseColor + specular), 1) * float4(1, 1, 1, 1);


                
            }
            ENDHLSL
        }
    }
}