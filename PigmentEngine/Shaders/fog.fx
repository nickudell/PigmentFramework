//GLOBALS
Texture2D shaderTexture;
SamplerState SampleType;

cbuffer MatrixBuffer
{
	matrix worldMatrix;
	matrix viewMatrix;
	matrix projectionMatrix;
};

cbuffer FogBuffer
{
	float fogStart;
	float fogEnd;
	float3 fogColour;
};
//Vertex Shader input
struct VEX_IN
{
	float4 position : POSITION;
	float2 tex : TEXCOORD0;
};

//Vertex shader output
//Pixel shader input
struct PIX_IN
{
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
	float fogFactor : FOG;
};

//Vertex shader
PIX_IN VShader(VEX_IN input)
{
	PIX_IN output;
	output.position = mul(input.position, worldMatrix);
	output.position = mul(output.position, viewMatrix);
	float4 cameraPosition = output.position;
	output.position = mul(output.position, projectionMatrix);
	output.fogFactor = saturate((fogEnd - cameraPosition.z) / (fogEnd - fogStart));
	output.tex= input.tex;
	return output;
}

//Pixel shader
float4 PShader(PIX_IN input) : SV_Target
{
	float4 textureColour = shaderTexture.Sample(SampleType,input.tex);
	return input.fogFactor * textureColour + (1.0 - input.fogFactor) * float4(fogColour,1);
}