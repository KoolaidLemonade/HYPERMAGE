#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float time;
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

float Wave(float2 UV, float time, float frequency, float speed, float amplitude)
{
    return sin(UV.x * frequency + time * (speed)) + cos(UV.y * frequency + time * (speed)) * amplitude;
}


float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = lerp(float4(1, 1, 1, 1), float4(0, 0, 0, 0), Wave(input.UV, time, 10, 50, 50));
	
    float4 color2 = tex2D(SpriteTextureSampler, input.UV);
	
    color.rgb *= 0.02;
	
    color.a *= 0;
	
    return color2 - color;

}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};