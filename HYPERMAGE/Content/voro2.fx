#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float time;
matrix WorldViewProjection;

Texture2D DisplacementTexture;
sampler2D DisplacementTextureSampler = sampler_state
{
    Texture = <DisplacementTexture>;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
    float2 UV : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, WorldViewProjection);
	output.Color = input.Color;

	return output;
}

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
    
    return color;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};