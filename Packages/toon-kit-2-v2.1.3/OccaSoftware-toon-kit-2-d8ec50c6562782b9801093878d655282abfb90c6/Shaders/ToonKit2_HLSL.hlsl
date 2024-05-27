#ifndef TK2_UTILS_INCLUDED
#define TK2_UTILS_INCLUDED

#pragma multi_compile _ _SCREEN_SPACE_OCCLUSION
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
#pragma multi_compile _ _SHADOWS_SOFT
#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS



void DebugViews_float(float state, half3 NoDebug, half3 Normals, half3 BaseColor, half3 BaseLighting, half3 AdditionalLighting, half3 DirectAO, half3 IndirectAO, half3 Shadows, half3 Specular, half3 Rim, half3 Ambient, half3 Emission, out half3 DebugColor)
{
	DebugColor = NoDebug;

#ifdef SHADERGRAPH_PREVIEW
return;
#endif

	switch (state)
	{
		case 0:
			break;
		case 1:
			DebugColor = Normals;
			break;
		case 2:
			DebugColor = BaseColor;
			break;
		case 3:
			DebugColor = BaseLighting;
			break;
		case 4:
			DebugColor = AdditionalLighting;
			break;
		case 5:
			DebugColor = DirectAO * IndirectAO;
			break;
		case 6: 
			DebugColor = Shadows;
			break;
		case 7:
			DebugColor = Specular;
			break;
		case 8:
			DebugColor = Rim;
			break;
		case 9:
			DebugColor = Ambient;
			break;
		case 10:
			DebugColor = Emission;
			break;
	}
}

float CalculateLuminance(float3 Color)
{
	return dot(Color, float3(0.2126, 0.7152, 0.0722));
}

void GetLuminance_float(float3 Color, out float Luminance)
{
	Luminance = 1;
#ifdef SHADERGRAPH_PREVIEW
return;
#endif

	Luminance = CalculateLuminance(Color);
}

half4 GetMainLightShadowCoord(half3 PositionWS)
{
	half4 shadowCoord = float4(0, 0, 0, 0);

#ifdef SHADERGRAPH_PREVIEW
return shadowCoord;
#else

    #ifdef SHADOWS_SCREEN
        half4 clipPos = TransformWorldToHClip(PositionWS);
        shadowCoord = ComputeScreenPos(clipPos);
    #else
		shadowCoord = TransformWorldToShadowCoord(PositionWS);
    #endif

	return shadowCoord;
#endif
}

void GetMainLightData_float(float3 PositionWS, out float3 Direction, out float3 Color, out float DistanceAtten, out float ShadowAtten)
{
    Direction = float3(0.5, 0.5, 0);
    Color = 1;
    DistanceAtten = 1;
    ShadowAtten = 1;

#ifdef SHADERGRAPH_PREVIEW
	return;
#else
	float4 shadowCoord = GetMainLightShadowCoord(PositionWS);
    
    Light mainLight = GetMainLight(shadowCoord);
    Direction = normalize(mainLight.direction);
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
	ShadowAtten = mainLight.shadowAttenuation;
	#endif
}

void GetAdditionalLightData_float(float3 PositionWS, float3 NormalWS, float Threshold, out float3 LightingNPR, out float3 LightingPBR)
{
	LightingNPR = float3(0,0,0);
	LightingPBR = float3(0,0,0);

#ifdef SHADERGRAPH_PREVIEW
return;
#else

	NormalWS = normalize(NormalWS);
    Threshold = max(0.0001, Threshold);
	#if UNITY_VERSION >= 202220
	uint meshRenderingLayers = GetMeshRenderingLayer();
	#else
    uint meshRenderingLayers = GetMeshRenderingLightLayer();
	#endif
    
    int count = GetAdditionalLightsCount();
	for (int i = 0; i < count; i++)
    {
		half4 shadowMask = half4(1,1,1,1);
		Light light = GetAdditionalLight(i, PositionWS, shadowMask);
		
		#ifdef _LIGHT_LAYERS
		if(IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
		#endif
		{
			half NoL = saturate(dot(NormalWS, light.direction));
            half attenuation = light.distanceAttenuation * light.shadowAttenuation * NoL;
            LightingPBR += light.color * attenuation;
			
            float luminance = CalculateLuminance(light.color);
            half attenuationNPR = step(Threshold, light.distanceAttenuation * light.shadowAttenuation) * step(1e-10, NoL);
            half lightIntensityNPR = luminance * attenuationNPR;
            LightingNPR += lightIntensityNPR * light.color;
        }
	}
#endif
}

void GetAmbientOcclusionData_float(float2 UV, out float IndirectAO, out float DirectAO, out float DirectLightingStrength) {
	IndirectAO = 1;
	DirectAO = 1;
	DirectLightingStrength = 0.25;

#ifdef SHADERGRAPH_PREVIEW
return;
#else

	#ifdef _SCREEN_SPACE_OCCLUSION
		AmbientOcclusionFactor aoFactor = GetScreenSpaceAmbientOcclusion(UV);
		IndirectAO = aoFactor.indirectAmbientOcclusion;
		DirectAO = aoFactor.directAmbientOcclusion;	
		DirectLightingStrength = _AmbientOcclusionParam.w;
	#endif

#endif
}
#endif