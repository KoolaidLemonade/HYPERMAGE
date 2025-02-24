#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float range;
float2 center;

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

float rand(float n)
{
    return frac(sin(n) * 43758.5453123);
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 pixelSize = 1 / float2(320, 180);
	
    float4 color = tex2D(SpriteTextureSampler, input.UV);

    float2 distortedUV = input.UV;
    distortedUV.y += sin((input.UV.x + time / 10) * 150) / 60 * (abs(distortedUV.x - center.x) + 0.1) * (range * 3);
    distortedUV.x += cos((input.UV.y + time / 10) * 150) / 60 * (abs(distortedUV.y - center.y) + 0.1) * (range * 3);

    distortedUV.y += sin((input.UV.x + time / 20) * 120) / 100 * (abs(distortedUV.x - center.x) + 0.1) * (range * 1.5);
    distortedUV.x += cos((input.UV.y + time / 20) * 120) / 100 * (abs(distortedUV.y - center.y) + 0.1) * (range * 1.5);
    
    if (abs(distortedUV.x - center.x) > range || abs(distortedUV.y - center.y) > range)
    {
        color.rgba = 0;
		
        if (abs(distortedUV.x - center.x) > range && abs(distortedUV.x - center.x) < range + pixelSize.x && abs(distortedUV.y - center.y) < range + pixelSize.y || abs(distortedUV.y - center.y) > range && abs(distortedUV.y - center.y) < range + pixelSize.y && abs(distortedUV.x - center.x) < range + pixelSize.x)
        {
            color.rgba = 1;
        }
    }
	
    return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};