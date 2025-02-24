#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float time;

Texture2D DisplacementTexture;
sampler2D DisplacementTextureSampler = sampler_state
{
    Texture = <DisplacementTexture>;
};

Texture2D PaletteTexture;
sampler2D PaletteTextureSampler = sampler_state
{
    Texture = <PaletteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 UV : TEXCOORD0;
};

float2 random2(float2 p)
{
    return frac(sin(float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)))) * 43758.5453);
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = float4(1, 1, 1, 1);
    
    lerp(input.UV, float2(0.5, 0.5), -0.05);

    float2 displacedUV = input.UV;
   
    displacedUV.xy += (tex2D(DisplacementTextureSampler, input.UV).rg) / 2;  
    displacedUV.xy += (tex2D(DisplacementTextureSampler, displacedUV).rg) / 4;

    //
    
    displacedUV *= 2;
    
    float2 i_uv = floor(displacedUV);
    float2 f_uv = frac(displacedUV);
    
    float m_dist = 1.;
    
    for (int y = -1; y <= 1; y++)
    {
        for (int x = -1; x <= 1; x++)
        {
            float2 neighbor = float2(float(x), float(y));

            float2 _point = random2(i_uv + neighbor);

            _point = 0.5 + 0.5 * sin(time / 20 + 6.2831 * _point);

            float2 diff = neighbor + _point - f_uv;

            float dist = length(diff);

            m_dist = min(m_dist, dist);
        }
    }
    
    //

    displacedUV /= 2;
           
    color -= m_dist;

    if (m_dist > (sin(time / 4 + displacedUV.y * 4) / 12) + 0.2)
    {
        color *= 2;
    }
    
    // dither
    
    if (color.r + color.g + color.b <= 1.5 && int(input.UV.x * 320) % 2 == 0 && int(input.UV.y * 180) % 2 == 0)
    {
        color -= float4(1, 1, 1, 1);
    }
    
    else if (color.r + color.g + color.b <= 1.8 && int(input.UV.x * 320) % 2 == int(input.UV.y * 180) % 2 == 0 ? 1 : 0)
    {
        color -= float4(1, 1, 1, 1);
    }
    
    // quantization
    
    color.rgb /= 3;
    
    color.r = clamp(color.r, 0, 1);
    color.g = clamp(color.g, 0, 1);
    color.b = clamp(color.b, 0, 1);

    color = tex2D(PaletteTextureSampler, float2(color.r + color.g + color.b + 0.01, 0.5 + sin(time / 4 + displacedUV.y) / 2));
       
    //
        
    color.rgb -= sqrt(pow(abs(input.UV.x - 0.5), 2) + pow(abs(input.UV.y - 0.5), 2)) * 0.8;
    
    color.rgb *= 0.2 + sin(time / 3) / 10;
    
    return color;
    
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};