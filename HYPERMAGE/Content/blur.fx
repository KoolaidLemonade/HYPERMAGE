#if OPENGL
	#define SVPOSITION POSITION
	#define VSSHADERMODEL vs_3_0	
	#define PSSHADERMODEL ps_3_0
#else
	#define VSSHADERMODEL vs_4_0level91
	#define PSSHADERMODEL ps_4_0level91
#endif

float2 offset;

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SVPOSITION;
	float4 Color : COLOR0;
    float2 UV : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(SpriteTextureSampler, input.UV + offset * 6) * 0.05 +
				   tex2D(SpriteTextureSampler, input.UV + offset * 5) * 0.05 +
				   tex2D(SpriteTextureSampler, input.UV + offset * 4) * 0.1 +
				   tex2D(SpriteTextureSampler, input.UV + offset * 3) * 0.1 +
				   tex2D(SpriteTextureSampler, input.UV + offset * 2) * 0.2 +
				   tex2D(SpriteTextureSampler, input.UV + offset * 1) * 0.3 +
				   tex2D(SpriteTextureSampler, input.UV) * 0.5 +
				   tex2D(SpriteTextureSampler, input.UV - offset * 1) * 0.3 +
	               tex2D(SpriteTextureSampler, input.UV - offset * 2) * 0.2 +
				   tex2D(SpriteTextureSampler, input.UV - offset * 3) * 0.1 +
				   tex2D(SpriteTextureSampler, input.UV - offset * 4) * 0.1 +
	               tex2D(SpriteTextureSampler, input.UV - offset * 5) * 0.05 +
				   tex2D(SpriteTextureSampler, input.UV - offset * 6) * 0.05;
	
    return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PSSHADERMODEL MainPS();
	}
};