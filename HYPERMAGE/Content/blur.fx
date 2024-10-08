#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float2 offset;

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates + offset * 6) * 0.05 +
				   tex2D(SpriteTextureSampler, input.TextureCoordinates + offset * 5) * 0.05 +
				   tex2D(SpriteTextureSampler, input.TextureCoordinates + offset * 4) * 0.1 +
				   tex2D(SpriteTextureSampler, input.TextureCoordinates + offset * 3) * 0.1 +
				   tex2D(SpriteTextureSampler, input.TextureCoordinates + offset * 2) * 0.2 +
				   tex2D(SpriteTextureSampler, input.TextureCoordinates + offset * 1) * 0.3 +
				   tex2D(SpriteTextureSampler, input.TextureCoordinates) * 0.5 +
				   tex2D(SpriteTextureSampler, input.TextureCoordinates - offset * 1) * 0.3 +
	               tex2D(SpriteTextureSampler, input.TextureCoordinates - offset * 2) * 0.2 +
				   tex2D(SpriteTextureSampler, input.TextureCoordinates - offset * 3) * 0.1 +
				   tex2D(SpriteTextureSampler, input.TextureCoordinates - offset * 4) * 0.1 +
	               tex2D(SpriteTextureSampler, input.TextureCoordinates - offset * 5) * 0.05 +
				   tex2D(SpriteTextureSampler, input.TextureCoordinates - offset * 6) * 0.05;
	
    return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};