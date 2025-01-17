#if OPENGL
	#define SVPOSITION POSITION
	#define VSSHADERMODEL vs_3_0
	#define PSSHADERMODEL ps_3_0
#else
	#define VSSHADERMODEL vs_4_0level91
	#define PSSHADERMODEL ps_4_0level91
#endif

sampler sprite : register(s0);
matrix WorldViewProjection;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
    float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SVPOSITION;
	float4 Color : COLOR0;
    float2 UV : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, WorldViewProjection);
	output.Color = input.Color;
    output.UV = input.UV;

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    return input.Color * tex2D(sprite, input.UV);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VSSHADERMODEL MainVS();
		PixelShader = compile PSSHADERMODEL MainPS();
	}
};