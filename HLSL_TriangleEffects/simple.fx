//
// Intro to HLSL
//

// Shader output, position and diffuse color
struct VS_OUTPUT
{
    float4 pos  : POSITION;
    float4 diff : COLOR0;
};

// The world view and projection matrices
float4x4 WorldViewProj : WORLDVIEWPROJECTION;
float Time = 1.0f;

// Transform our coordinates into world space
VS_OUTPUT Transform(float4 Pos  : POSITION)
{
    // Declare our return variable
    VS_OUTPUT Out = (VS_OUTPUT)0;

    // Transform our position
    Out.pos = mul(Pos + Time/3, WorldViewProj);
    // Set our color
    Out.diff.r = Out.pos[2] * Time;
	Out.diff.g = Out.pos[1] * Time;
    Out.diff.b = Out.pos[0] * Time;

    // Return
    return Out;
}

technique TransformDiffuse
{
    pass P0
    {
		CullMode = 0;

        // shaders
        VertexShader = compile vs_1_1 Transform();
        PixelShader  = NULL;
    }
}