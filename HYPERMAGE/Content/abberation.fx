#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float2 mousePosition;
float power;

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 UV : TEXCOORD0;
};



float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 col;
	
    float2 redOffset = (float2(0, 0.002) * power) * (input.UV - mousePosition.xy);
    float2 greenOffset = (float2(-0.002, -0.002) * power) * (input.UV - mousePosition.xy);
    float2 blueOffset = (float2(0.002, -0.002) * power) * (input.UV - mousePosition.xy);

    col.r = tex2D(SpriteTextureSampler, input.UV + redOffset.xy).r;
    col.g = tex2D(SpriteTextureSampler, input.UV + greenOffset.xy).g;
    col.ba = tex2D(SpriteTextureSampler, input.UV + blueOffset.xy).ba;

    return col;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};