#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
float2 resolution;

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
    float2 uv = input.UV * 2 - 1;
    float2 offset = uv.yx / 7;
    uv = uv + uv * offset * offset;
    uv = uv * 0.5 + 0.5;
	
    float4 col = tex2D(SpriteTextureSampler, uv);	
	
    if (uv.x <= 0.0f || 1.0f <= uv.x || uv.y <= 0.0f || 1.0f <= uv.y)
    {
        col.rgb = 0;	
    }

    uv = uv * 2.0f - 1.0f;
    float2 vignette = 50 / resolution.xy;
    vignette = smoothstep(0.0f, vignette, 1.0f - abs(uv));
    vignette = saturate(vignette);

    col.g *= (sin(input.UV.y * resolution.y * 2.0f) + 1.0f) * 0.15f + 1.0f;
    col.rb *= (cos(input.UV.y * resolution.y * 2.0f) + 1.0f) * 0.135f + 1.0f;

    return saturate(col) * vignette.x * vignette.y;
	
	return col;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};