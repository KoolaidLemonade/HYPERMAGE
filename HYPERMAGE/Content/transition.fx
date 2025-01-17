#if OPENGL
	#define SVPOSITION POSITION
	#define VSSHADERMODEL vs_3_0
	#define PSSHADERMODEL ps_3_0
#else
	#define VSSHADERMODEL vs_4_0level91
	#define PSSHADERMODEL ps_4_0level91
#endif

float time;

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

float4 MainPS(VertexShaderOutput In) : COLOR
{	
    float duration = 3;
    
    float waveFrequency = 10 * (1 - In.UV);
    float waveAmplitude = 0.5 * (1 - In.UV);
    
    float waveFrequency2 = 10 * In.UV;
    float waveAmplitude2 = 0.5 * In.UV;
    
    float waveSpeed = pow(time, 2.5);

    float wave = sin(1 - In.UV.y * waveFrequency + waveSpeed) * waveAmplitude;
    float wave2 = sin(1 - In.UV.x * waveFrequency + waveSpeed) * waveAmplitude;
    float wave3 = sin(In.UV.y * waveFrequency2 + waveSpeed) * waveAmplitude2;
    float wave4 = sin(In.UV.x * waveFrequency2 + waveSpeed) * waveAmplitude2;
    
    float wave5 = sin(1 - In.UV * waveFrequency / 2 + waveSpeed / 2) * waveAmplitude;
    float wave6 = sin(In.UV * waveFrequency2 / 2 + waveSpeed / 2) * waveAmplitude2;
    
    float2 distortedUV = In.UV;
    
    distortedUV += wave * pow(time, 4) / duration;
    distortedUV += wave2 * pow(time, 4) / duration;
    distortedUV += wave3 * pow(time, 4) / duration;
    distortedUV += wave4 * pow(time, 4) / duration;
    distortedUV += wave5 * pow(time, 4) / duration;
    distortedUV += wave6 * pow(time, 4) / duration;
	
    float4 color = tex2D(SpriteTextureSampler, distortedUV);
	
    return lerp(color, 0, time / duration);
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PSSHADERMODEL MainPS();
	}
};