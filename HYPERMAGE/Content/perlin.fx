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

float2 randomGradient(float2 p)
{
    p = p + 0.02;
    float x = dot(p, float2(123.4, 234.5));
    float y = dot(p, float2(234.5, 345.6));
    float2 gradient = float2(x, y);
    gradient = sin(gradient);
    gradient = gradient * 43758.5453;

    gradient = sin(gradient + time / 4);
    return gradient;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 uv = input.UV;
    
    float3 black = float3(0.0, 0.0, 0.0);
    float3 white = float3(1.0, 1.0, 1.0);
	
    float3 color = black;
	
    uv = uv * 4.0;
    float2 gridId = floor(uv);
    float2 gridUv = frac(uv);
    color = float3(gridId, 0.0);
    color = float3(gridUv, 0.0);
    
    float2 bl = gridId + float2(0.0, 0.0);
    float2 br = gridId + float2(1.0, 0.0);
    float2 tl = gridId + float2(0.0, 1.0);
    float2 tr = gridId + float2(1.0, 1.0);
    
    float2 gradBl = randomGradient(bl);
    float2 gradBr = randomGradient(br);
    float2 gradTl = randomGradient(tl);
    float2 gradTr = randomGradient(tr);
    
    float2 distFromPixelToBl = gridUv - float2(0.0, 0.0);
    float2 distFromPixelToBr = gridUv - float2(1.0, 0.0);
    float2 distFromPixelToTl = gridUv - float2(0.0, 1.0);
    float2 distFromPixelToTr = gridUv - float2(1.0, 1.0);
    
    float dotBl = dot(gradBl, distFromPixelToBl);
    float dotBr = dot(gradBr, distFromPixelToBr);
    float dotTl = dot(gradTl, distFromPixelToTl);
    float dotTr = dot(gradTr, distFromPixelToTr);
    
    gridUv = smoothstep(0.0, 1.0, gridUv);
    
    float b = lerp(dotBl, dotBr, gridUv.x);
    float t = lerp(dotTl, dotTr, gridUv.x);
    float perlin = lerp(b, t, gridUv.y);
    
    color.rgb = perlin + 0.5;

    color.rgb = clamp(color.rgb, 0, 1);
    
    return float4(color, 1);
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};