#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float time;
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
	float2 TextureCoordinates : TEXCOORD0;
};

float rand1(float n)
{
    return frac(sin(n) * 43758.5453123);
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);

    color.r *= rand1(time + rand1(input.TextureCoordinates.x) + rand1(input.TextureCoordinates.y));
    color.g *= rand1(time + rand1(input.TextureCoordinates.x) + rand1(input.TextureCoordinates.y) + 1);
    color.b *= rand1(time + rand1(input.TextureCoordinates.y) + 2);
    color.a *= rand1(time + rand1(input.TextureCoordinates.y) + rand1(input.TextureCoordinates.y) + 3);
	
    color.rgba -= power;
	
    return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};