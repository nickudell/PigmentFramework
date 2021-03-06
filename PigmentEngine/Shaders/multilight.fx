﻿//GLOBALS
Texture2D deferredTextures[4];
SamplerState SampleType;

cbuffer MatrixBuffer
{
	matrix worldMatrix;
	matrix viewMatrix;
	matrix projectionMatrix;
};

cbuffer CameraBuffer
{
	float3 cameraPosition;
	float padding;
};

cbuffer LightBuffer
{
	float4 ambientColour;
	float4 diffuseColour;
	float3 lightDirection;
	float specularPower;
	float4 specularColor;
}

//Vertex Shader input
struct VEX_IN
{
	float4 position : POSITION;
	float2 tex : TEXCOORD0;
	float3 normal : NORMAL;
	float3 tangent : TANGENT;
	float3 binormal : BINORMAL;
};

//Vertex shader output
//Pixel shader input
struct PIX_IN
{
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
	float3 normal : NORMAL;
	float3 tangent : TANGENT;
	float3 binormal : BINORMAL;
	float3 viewDirection : TEXCOORD1;
};

//Vertex shader
PIX_IN VShader(VEX_IN input)
{
	PIX_IN output;
	output.position = mul(input.position, worldMatrix);
	output.viewDirection = cameraPosition.xyz - output.position.xyz;
	output.viewDirection = normalize(output.viewDirection);
	output.position = mul(output.position, viewMatrix);
	output.position = mul(output.position, projectionMatrix);
	output.normal = mul(input.normal,(float3x3)worldMatrix);
	output.normal = normalize(output.normal);
	output.tangent = mul(input.tangent, (float3x3)worldMatrix);
	output.tangent = normalize(output.tangent);
	output.binormal = mul(input.binormal, (float3x3)worldMatrix);
	output.binormal = normalize(output.binormal);
	output.tex= input.tex;
	return output;
}

//Pixel shader
float4 PShader(PIX_IN input) : SV_Target
{
	float4 textureColour = shaderTextures[0].Sample(SampleType,input.tex);
	
	float4 bumpMap = shaderTextures[1].Sample(SampleType,input.tex);
	bumpMap = (bumpMap*2.0f)-1.0f;
	float3 bumpNormal = input.normal + bumpMap.x * input.tangent + bumpMap.y * input.binormal;
	bumpNormal = normalize(bumpNormal);

	float3 lightDir = -lightDirection;
	float lightIntensity = saturate(dot(bumpNormal,lightDir));

	float3 reflection = normalize(2*lightIntensity*bumpNormal - lightDir);
	float4 specular = pow(saturate(dot(reflection,input.viewDirection)),specularPower);

	float4 lightColour = ambientColour + saturate(diffuseColour * max(lightIntensity,0));

	lightColour =  lightColour * textureColour;
	return saturate(lightColour + specular);
}