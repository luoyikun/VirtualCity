using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DynamicFogAndMist {
	
				[CreateAssetMenu (fileName = "DynamicFogProfile", menuName = "Dynamic Fog Profile", order = 100)]
				public class DynamicFogProfile : ScriptableObject {

								public FOG_TYPE effectType = FOG_TYPE.DesktopFogPlusWithSkyHaze;
								public bool enableDithering = false;
								[Range (0, 0.2f)]
								public float ditherStrength = 0.03f;
								[Range (0, 1)]
								public float
												alpha = 1.0f;
								[Range (0, 1)]
								public float
												noiseStrength = 0.5f;
								[Range (0.01f, 1)]
								public float
												noiseScale = 0.1f;
								[Range (0, 0.999f)]
								public float
												distance = 0.1f;
								[Range (0.0001f, 2f)]
								public float
												distanceFallOff = 0.01f;
								[Range (0, 1.2f)]
								public float
												maxDistance = 0.999f;
								[Range (0.0001f, 0.5f)]
								public float
												maxDistanceFallOff = 0f;
								[Range (0, 500)]
								public float
												height = 1f;
								[Range (0, 500)]
								public float
												maxHeight = 100f;
								// used in orthogonal fog
								[Range (0.0001f, 1)]
								public float
												heightFallOff = 0.1f;
								public float
												baselineHeight = 0;
								public bool clipUnderBaseline = false;
								[Range (0, 15)]
								public float
												turbulence = 0.1f;
								[Range (0, 5.0f)]
								public float
												speed = 0.1f;
								public Vector3 windDirection = new Vector3 (1, 0, 1);
								public Color color = Color.white;
								public Color color2 = Color.gray;
								[Range (0, 500)]
								public float
												skyHaze = 50f;
								[Range (0, 1)]
								public float
												skySpeed = 0.3f;
								[Range (0, 1)]
								public float
												skyNoiseStrength = 0.1f;
								[Range (0, 1)]
								public float
												skyAlpha = 1.0f;

								public bool	useXZDistance = false;
								[Range (0, 1)]
								public float scattering = 0.7f;
								public Color scatteringColor = new Color (1, 1, 0.8f);


								/// <summary>
								/// Applies profile settings to a given fog instance
								/// </summary>
								public void Load (DynamicFog fog) {
												fog.preset = FOG_PRESET.Custom;
												fog.effectType = effectType;
												fog.enableDithering = enableDithering;
												fog.ditherStrength = ditherStrength;
												fog.alpha = alpha;
												fog.noiseStrength = noiseStrength;
												fog.noiseScale = noiseScale;
												fog.distance = distance;
												fog.distanceFallOff = distanceFallOff;
												fog.maxDistance = maxDistance;
												fog.maxDistanceFallOff = maxDistanceFallOff;
												fog.height = height;
												fog.maxHeight = maxHeight;
												fog.heightFallOff = heightFallOff;
												fog.baselineHeight = baselineHeight;
												fog.clipUnderBaseline = clipUnderBaseline;
												fog.turbulence = turbulence;
												fog.speed = speed;
												fog.windDirection = windDirection;
												fog.color = color;
												fog.color2 = color2;
												fog.skyHaze = skyHaze;
												fog.skySpeed = skySpeed;
												fog.skyNoiseStrength = skyNoiseStrength;
												fog.skyAlpha = skyAlpha;
												fog.useXZDistance = useXZDistance;
												fog.scattering = scattering;
												fog.scatteringColor = scatteringColor;
								}


								/// <summary>
								/// Replaces profile settings with current fog configuration
								/// </summary>
								public void Save (DynamicFog fog) {
												effectType = fog.effectType;
												enableDithering = fog.enableDithering;
												ditherStrength = fog.ditherStrength;
												alpha = fog.alpha;
												noiseStrength = fog.noiseStrength;
												noiseScale = fog.noiseScale;
												distance = fog.distance;
												distanceFallOff = fog.distanceFallOff;
												maxDistance = fog.maxDistance;
												maxDistanceFallOff = fog.maxDistanceFallOff;
												height = fog.height;
												maxHeight = fog.maxHeight;
												heightFallOff = fog.heightFallOff;
												baselineHeight = fog.baselineHeight;
												clipUnderBaseline = fog.clipUnderBaseline;
												turbulence = fog.turbulence;
												speed = fog.speed;
												windDirection = fog.windDirection;
												color = fog.color;
												color2 = fog.color2;
												skyHaze = fog.skyHaze;
												skySpeed = fog.skySpeed;
												skyNoiseStrength = fog.skyNoiseStrength;
												skyAlpha = fog.skyAlpha;
												useXZDistance = fog.useXZDistance;
												scattering = fog.scattering;
												scatteringColor = fog.scatteringColor;
								}

								/// <summary>
								/// Lerps between profile1 and profile2 using t as the transition amount (0..1) and assign the values to given fog
								/// </summary>
								public static void Lerp (DynamicFogProfile profile1, DynamicFogProfile profile2, float t, DynamicFog fog) {
												if (t < 0)
																t = 0;
												else if (t > 1f)
																t = 1f;
												fog.enableDithering = t < 0.5f ? profile1.enableDithering : profile2.enableDithering;
												fog.ditherStrength = profile1.ditherStrength * (1f - t) + profile2.ditherStrength * t;
												fog.alpha = profile1.alpha * (1f - t) + profile2.alpha * t;
												fog.noiseStrength = profile1.noiseStrength * (1f - t) + profile2.noiseStrength * t;
												fog.noiseScale = profile1.noiseScale * (1f - t) + profile2.noiseScale * t;
												fog.distance = profile1.distance * (1f - t) + profile2.distance * t;
												fog.distanceFallOff = profile1.distanceFallOff * (1f - t) + profile2.distanceFallOff * t;
												fog.maxDistance = profile1.maxDistance * (1f - t) + profile2.maxDistance * t;
												fog.maxDistanceFallOff = profile1.maxDistanceFallOff * (1f - t) + profile2.maxDistanceFallOff * t;
												fog.height = profile1.height * (1f - t) + profile2.height * t;
												fog.maxHeight = profile1.maxHeight * (1f - t) + profile2.maxHeight * t;
												fog.heightFallOff = profile1.heightFallOff * (1f - t) + profile2.heightFallOff * t;
												fog.baselineHeight = profile1.baselineHeight * (1f - t) + profile2.baselineHeight * t;
												fog.clipUnderBaseline = t < 0.5f ? profile1.clipUnderBaseline : profile2.clipUnderBaseline;
												fog.turbulence = profile1.turbulence * (1f - t) + profile2.turbulence * t;
												fog.speed = profile1.speed * (1f - t) + profile2.speed * t;
												fog.windDirection = profile1.windDirection * (1f - t) + profile2.windDirection * t;
												fog.color = profile1.color * (1f - t) + profile2.color * t;
												fog.color2 = profile1.color2 * (1f - t) + profile2.color * t;
												fog.skyHaze = profile1.skyHaze * (1f - t) + profile2.skyHaze * t;
												fog.skySpeed = profile1.skySpeed * (1f - t) + profile2.skySpeed * t;
												fog.skyNoiseStrength = profile1.skyNoiseStrength * (1f - t) + profile2.skyNoiseStrength * t;
												fog.skyAlpha = profile1.skyAlpha * (1f - t) + profile2.skyAlpha * t;
												fog.useXZDistance = t < 0.5f ? profile1.useXZDistance : profile2.useXZDistance;
												fog.scattering = profile1.scattering * (1f - t) + profile2.scattering * t;
												fog.scatteringColor = profile1.scatteringColor * (1f - t) + profile2.scatteringColor * t;
								}

				}

}