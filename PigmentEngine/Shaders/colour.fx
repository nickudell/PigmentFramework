
//GLOBALS
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
	float3 colour : COLOR;
};

//Vertex shader output
//Pixel shader input
struct PIX_IN
{
	float4 position : SV_POSITION;
	float4 colour : COLOR;
};

//Vertex shader
PIX_IN VShader(VEX_IN input)
{
	PIX_IN output;
	output.position = mul(input.position, worldMatrix);
	output.position = mul(output.position, viewMatrix);
	output.position = mul(output.position, projectionMatrix);
	output.colour = float4(input.colour,1);
	return output;
}

//Pixel shader
float4 PShader(PIX_IN input) : SV_Target
{
	return input.colour;
}