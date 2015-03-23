//
// Intro to HLSL
//

// Shader output, position and diffuse color
struct VS_OUTPUT
{
    float4 pos  : POSITION;
    float4 diff : COLOR0;
};

// The direction of the light in world space
float3 LightDir = {0.0f, 0.0f, -1.0f};

// The world view and projection matrices
float4x4 WorldViewProj : WORLDVIEWPROJECTION;
float Time = 0.0f;

// Transform our coordinates into world space
VS_OUTPUT Transform(
    float4 Pos  : POSITION, 
    float3 Normal : NORMAL )
{
    // Declare our return variable
    VS_OUTPUT Out = (VS_OUTPUT)0;

    // Transform the normal into the same coord system
    float4 transformedNormal = mul(Normal, WorldViewProj);
    
    // Set our color
    Out.diff = sin(Pos + Time);
    
    // Calculate the directional light
    Out.diff *= dot(transformedNormal, LightDir);

    // Store our local position
    float4 tempPos = Pos;
    // Make the sphere 'wobble' some
    tempPos.y += cos(Pos + (Time * 2.0f));
    // Transform our position
    Out.pos = mul(tempPos, WorldViewProj);
//	Out.pos = mul(Pos, WorldViewProj);

    // Return
    return Out;
}

technique TransformDiffuse
{
    pass P0
    {
        // shaders
        VertexShader = compile vs_1_1 Transform();
        PixelShader  = NULL;
    }
}

