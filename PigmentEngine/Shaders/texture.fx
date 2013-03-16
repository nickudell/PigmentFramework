
//GLOBALS
Texture2D shaderTexture;
SamplerState SampleType;

cbuffer MatrixBuffer
{
	matrix worldMatrix;
	matrix viewMatrix;
	matrix projectionMatrix;
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
};

//Vertex shader
PIX_IN VShader(VEX_IN input)
{
	PIX_IN output;
	output.position = mul(input.position, worldMatrix);
	output.position = mul(output.position, viewMatrix);
	output.position = mul(output.position, projectionMatrix);
	output.tex= input.tex;
	return output;
}

//Pixel shader
float4 PShader(PIX_IN input) : SV_Target
{
	float4 textureColour = shaderTexture.Sample(SampleType,input.tex);

	return textureColour;
}