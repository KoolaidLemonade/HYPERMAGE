#if OPENGL
	#define SVPOSITION POSITION
	#define VSSHADERMODEL vs_3_0
	#define PSSHADERMODEL ps_3_0
#else
	#define VSSHADERMODEL vs_4_0level91
	#define PSSHADERMODEL ps_4_0level91
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
	float4 Position : SVPOSITION;
	float4 Color : COLOR0;
	float2 UV : TEXCOORD0;
};

float Wave(float2 UV, float time, float frequency, float speed, float amplitude)
{
    return cos(UV.x * frequency + time * (speed * power)) + sin(UV.y * frequency + time * (speed * power)) * amplitude;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 distortedCoords = input.UV;
    
    float distortion = (sin(input.UV * (5 + pow(power, 3)) + time) * 0.05) + tan(input.UV);
    
    distortedCoords += distortion;

    float wave1 = Wave(distortedCoords, time, 12.0, 1.0, 10);
    float wave2 = Wave(distortedCoords, time * 0.5, 20.0, 1.5, 10);
    float wave3 = (wave1 + wave2) * 0.5;
    
    wave3 *= 0.2;

    float colorShift = sin(time);
    
    float4 col1 = float4(0.2, 0.1, 1.0, 1.0);
    float4 col2 = float4(1.0, 0.0, 0.5, 1.0);
    float4 col3 = float4(0.6, 0.1, 0.3, 1.0);
    float4 col4 = float4(0.0, 1.0, 1.0, 1.0);
    
    float4 shiftingColor = lerp(lerp(col1, col2, colorShift), lerp(col3, col4, colorShift), 1.0);

    float4 color = lerp(col1, shiftingColor, wave3);

    color.rgb *= input.UV.x;
    
    color.rgb *= 1 - input.UV.x;
    
    color.rgb *= input.UV.y;
    
    color.rgb *= 1 - input.UV.y;
    
    color *= 4;
    
    float glow = pow(wave3, 4.0);
    
    color += glow * -0.6;
    
    color *= power;
    
    return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PSSHADERMODEL MainPS();
	}
};