using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DynamicFogAndMist {
				public enum FOG_TYPE {
								DesktopFogWithSkyHaze = 0,
								MobileFogWithSkyHaze = 1,
								MobileFogOnlyGround = 2,
								DesktopFogPlusWithSkyHaze = 3,
								MobileFogSimple = 4,
								MobileFogBasic = 5,
								MobileFogOrthogonal = 6,
								DesktopFogPlusOrthogonal = 7
				}

				static class FOG_TYPE_Ext {
								public static bool isPlus (this FOG_TYPE fogType) {
												return fogType == FOG_TYPE.DesktopFogPlusWithSkyHaze || fogType == FOG_TYPE.MobileFogSimple || fogType == FOG_TYPE.MobileFogBasic || fogType == FOG_TYPE.MobileFogOrthogonal || fogType == FOG_TYPE.DesktopFogPlusOrthogonal;
								}
				}

				public enum FOG_PRESET {
								Clear,
								Mist,
								WindyMist,
								GroundFog,
								Fog,
								HeavyFog,
								SandStorm,
								Custom
				}

				[ExecuteInEditMode]
				[RequireComponent (typeof(Camera))]
				[HelpURL ("http://kronnect.com/taptapgo")]
	#if UNITY_5_4_OR_NEWER
				[ImageEffectAllowedInSceneView]
				#endif
				public class DynamicFog : MonoBehaviour {

								[SerializeField]
								FOG_TYPE _effectType = FOG_TYPE.DesktopFogPlusWithSkyHaze;

								public FOG_TYPE effectType {
												get { return _effectType; }
												set {
																if (value != _effectType) {
																				_effectType = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField]
								FOG_PRESET _preset = FOG_PRESET.Mist;

								public FOG_PRESET preset {
												get { return _preset; }
												set {
																if (value != _preset) {
																				_preset = value;
																				UpdateMaterialPropertiesNow ();
																}
												}
								}

								[SerializeField]
								DynamicFogProfile _profile;

								public DynamicFogProfile profile {
												get { return _profile; }
												set {
																if (value != _profile) {
																				_profile = value;
																				if (_profile != null) {
																								_profile.Load (this);
																								_preset = FOG_PRESET.Custom;
																								UpdateMaterialProperties ();
																				}
																}
												}
								}

								[SerializeField]
								bool _useFogVolumes = false;

								public bool useFogVolumes {
												get { return _useFogVolumes; }
												set {
																if (value != _useFogVolumes) {
																				_useFogVolumes = value;
																}
												}
								}

								[SerializeField]
								bool _enableDithering = false;

								public bool enableDithering {
												get { return _enableDithering; }
												set {
																if (value != _enableDithering) {
																				_enableDithering = value;
																				UpdateMaterialProperties ();
																}
												}
								}


								[SerializeField, Range (0, 0.2f)]
								float _ditherStrength = 0.03f;

								public float ditherStrength {
												get { return _ditherStrength; }
												set {
																if (value != _ditherStrength) {
																				_ditherStrength = value;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0, 1)]
								float _alpha = 1.0f;

								public float alpha {
												get { return _alpha; }
												set {
																if (value != _alpha) {
																				_alpha = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0, 1)]
								float _noiseStrength = 0.5f;

								public float noiseStrength {
												get { return _noiseStrength; }
												set {
																if (value != _noiseStrength) {
																				_noiseStrength = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0.01f, 1)]
								float _noiseScale = 0.1f;

								public float noiseScale {
												get { return _noiseScale; }
												set {
																if (value != _noiseScale) {
																				_noiseScale = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0, 0.999f)]
								float	_distance = 0.1f;

								public float distance {
												get { return _distance; }
												set {
																if (value != _distance) {
																				_distance = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0.0001f, 2f)]
								float _distanceFallOff = 0.01f;

								public float distanceFallOff {
												get { return _distanceFallOff; }
												set {
																if (value != _distanceFallOff) {
																				_distanceFallOff = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0, 1.2f)]
								float _maxDistance = 0.999f;

								public float maxDistance {
												get { return _maxDistance; }
												set {
																if (value != _maxDistance) {
																				_maxDistance = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0.0001f, 0.5f)]
								float _maxDistanceFallOff = 0f;

								public float maxDistanceFallOff {
												get { return _maxDistanceFallOff; }
												set {
																if (value != _maxDistanceFallOff) {
																				_maxDistanceFallOff = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0, 500)]
								float _height = 1f;

								public float height {
												get { return _height; }
												set {
																if (value != _height) {
																				_height = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0, 500)]
								float _maxHeight = 100f;

								public float maxHeight {
												get { return _maxHeight; }
												set {
																if (value != _maxHeight) {
																				_maxHeight = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								// used in orthogonal fog
								[SerializeField, Range (0.0001f, 1)]
								float _heightFallOff = 0.1f;

								public float heightFallOff {
												get { return _heightFallOff; }
												set {
																if (value != _heightFallOff) {
																				_heightFallOff = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField]
								float	_baselineHeight = 0;

								public float baselineHeight {
												get { return _baselineHeight; }
												set {
																if (value != _baselineHeight) {
																				_baselineHeight = value;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField]
								bool _clipUnderBaseline = false;

								public bool clipUnderBaseline {
												get { return _clipUnderBaseline; }
												set {
																if (value != _clipUnderBaseline) {
																				_clipUnderBaseline = value;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0, 15)]
								float _turbulence = 0.1f;

								public float turbulence {
												get { return _turbulence; }
												set {
																if (value != _turbulence) {
																				_turbulence = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0, 5.0f)]
								float _speed = 0.1f;

								public float speed {
												get { return _speed; }
												set {
																if (value != _speed) {
																				_speed = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}


								[SerializeField]
								Vector3 _windDirection = new Vector3(1,0,1);

								public Vector3 windDirection {
												get { return _windDirection; }
												set {
																if (value != _windDirection) {
																				_windDirection = value;
																				UpdateMaterialProperties ();
																}
												}
								}


								[SerializeField]
								Color _color = Color.white;

								public Color color {
												get { return _color; }
												set {
																if (value != _color) {
																				_color = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField]
								Color _color2 = Color.gray;

								public Color color2 {
												get { return _color2; }
												set {
																if (value != _color2) {
																				_color2 = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0, 500)]
								float _skyHaze = 50f;

								public float skyHaze {
												get { return _skyHaze; }
												set {
																if (value != _skyHaze) {
																				_skyHaze = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0, 1)]
								float _skySpeed = 0.3f;

								public float skySpeed {
												get { return _skySpeed; }
												set {
																if (value != _skySpeed) {
																				_skySpeed = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0, 1)]
								float _skyNoiseStrength = 0.1f;

								public float skyNoiseStrength {
												get { return _skyNoiseStrength; }
												set {
																if (value != _skyNoiseStrength) {
																				_skyNoiseStrength = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0, 1)]
								float _skyAlpha = 1.0f;

								public float skyAlpha {
												get { return _skyAlpha; }
												set {
																if (value != _skyAlpha) {
																				_skyAlpha = value;
																				_preset = FOG_PRESET.Custom;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField]
								GameObject _sun;

								public GameObject sun {
												get { return _sun; }
												set {
																if (value != _sun) {
																				_sun = value;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField]
								bool _fogOfWarEnabled = false;

								public bool fogOfWarEnabled {
												get { return _fogOfWarEnabled; }
												set {
																if (value != _fogOfWarEnabled) {
																				_fogOfWarEnabled = value;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField]
								Vector3 _fogOfWarCenter;

								public Vector3 fogOfWarCenter {
												get { return _fogOfWarCenter; }
												set {
																if (value != _fogOfWarCenter) {
																				_fogOfWarCenter = value;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField]
								Vector3 _fogOfWarSize = new Vector3 (1024, 0, 1024);

								public Vector3 fogOfWarSize {
												get { return _fogOfWarSize; }
												set {
																if (value != _fogOfWarSize) {
																				_fogOfWarSize = value;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField]
								int _fogOfWarTextureSize = 256;

								public int fogOfWarTextureSize {
												get { return _fogOfWarTextureSize; }
												set {
																if (value != _fogOfWarTextureSize) {
																				_fogOfWarTextureSize = value;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField]
								bool	_useSinglePassStereoRenderingMatrix = false;

								public bool useSinglePassStereoRenderingMatrix {
												get { return _useSinglePassStereoRenderingMatrix; }
												set {
																if (value != _useSinglePassStereoRenderingMatrix) {
																				_useSinglePassStereoRenderingMatrix = value;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField]
								bool	_useXZDistance = false;

								public bool useXZDistance {
												get { return _useXZDistance; }
												set {
																if (value != _useXZDistance) {
																				_useXZDistance = value;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField, Range (0, 1)]
								float _scattering = 0.7f;

								public float scattering {
												get { return _scattering; }
												set {
																if (value != _scattering) {
																				_scattering = value;
																				UpdateMaterialProperties ();
																}
												}
								}

								[SerializeField]
								Color _scatteringColor = new Color (1, 1, 0.8f);

								public Color scatteringColor {
												get { return _scatteringColor; }
												set {
																if (value != _scatteringColor) {
																				_scatteringColor = value;
																				UpdateMaterialProperties ();
																}
												}
								}


								Material fogMatAdv, fogMatFogSky, fogMatOnlyFog, fogMatVol, fogMatSimple, fogMatBasic, fogMatOrthogonal, fogMatDesktopPlusOrthogonal;
								[SerializeField]
								Material
												fogMat;
								float initialFogAlpha, targetFogAlpha;
								float initialSkyHazeAlpha, targetSkyHazeAlpha;
								bool targetFogColors;
								Color initialFogColor1, targetFogColor1;
								Color initialFogColor2, targetFogColor2;
								float transitionDuration;
								float transitionStartTime;
								float currentFogAlpha, currentSkyHazeAlpha;
								bool transitionAlpha, transitionColor, transitionProfile;
								DynamicFogProfile initialProfile, targetProfile;
								Color currentFogColor1, currentFogColor2;
								Camera currentCamera;
								Texture2D fogOfWarTexture;
								Color32[] fogOfWarColorBuffer;
								Light sunLight;
								Vector3 sunDirection = Vector3.zero;
								Color sunColor = Color.white;
								float sunIntensity = 1f;
								static DynamicFog _fog;
								List<string> shaderKeywords;
								bool matOrtho;
								bool shouldUpdateMaterialProperties;


								public static DynamicFog instance { 
												get { 
																if (_fog == null) {
																				foreach (Camera camera in Camera.allCameras) {
																								_fog = camera.GetComponent<DynamicFog> ();
																								if (_fog != null)
																												break;
																				}
																}
																return _fog;
												} 
								}

								public string GetCurrentPresetName () {
												return Enum.GetName (typeof(FOG_PRESET), preset);
								}

								public Camera fogCamera {
												get {
																return currentCamera;
												}
								}

								// Creates a private material used to the effect
								void OnEnable () {
												if (fogMat == null) {
																Init ();
																UpdateMaterialPropertiesNow ();
												}
								}

								void Reset () {
												UpdateMaterialPropertiesNow ();
								}

								void OnDestroy () {
												fogMat = null;
												if (fogMatVol != null) {
																DestroyImmediate (fogMatVol);
																fogMatVol = null;
																if (fogMatDesktopPlusOrthogonal != null) {
																				DestroyImmediate (fogMatDesktopPlusOrthogonal);
																				fogMatDesktopPlusOrthogonal = null;
																}
												}
												if (fogMatAdv != null) {
																DestroyImmediate (fogMatAdv);
																fogMatAdv = null;
												}
												if (fogMatFogSky != null) {
																DestroyImmediate (fogMatFogSky);
																fogMatFogSky = null;
												}
												if (fogMatOnlyFog != null) {
																DestroyImmediate (fogMatOnlyFog);
																fogMatOnlyFog = null;
												}
												if (fogMatSimple != null) {
																DestroyImmediate (fogMatSimple);
																fogMatSimple = null;
												}
												if (fogMatBasic != null) {
																DestroyImmediate (fogMatBasic);
																fogMatBasic = null;
												}
												if (fogMatOrthogonal != null) {
																DestroyImmediate (fogMatOrthogonal);
																fogMatOrthogonal = null;
												}
												if (fogMatDesktopPlusOrthogonal != null) {
																DestroyImmediate (fogMatDesktopPlusOrthogonal);
																fogMatOrthogonal = null;
												}
												if (fogOfWarTexture != null) {
																DestroyImmediate (fogOfWarTexture);
																fogOfWarTexture = null;
												}
								}

								void Init () {
												targetFogAlpha = -1;
												targetSkyHazeAlpha = -1;
												currentCamera = GetComponent<Camera> ();
												UpdateFogOfWarTexture ();
												if (_profile != null)
																_profile.Load (this);

#if UNITY_EDITOR
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
#if UNITY_5_5_OR_NEWER
                _useSinglePassStereoRenderingMatrix = PlayerSettings.stereoRenderingPath == StereoRenderingPath.SinglePass;
#else
												_useSinglePassStereoRenderingMatrix = PlayerSettings.singlePassStereoRendering;
#endif
            }
#endif
        }

								// Check possible alpha transition
								void Update () {
												if (fogMat == null)
																return;
												
												// Check profile transition
												if (transitionProfile) {
																float t = (Time.time - transitionStartTime) / transitionDuration;
																if (t > 1)
																				t = 1;
																DynamicFogProfile.Lerp (initialProfile, targetProfile, t, this);
																if (t >= 1f) {
																				transitionProfile = false;
																}
												}

												// Check alpha transitions
												if (transitionAlpha) {
																if (targetFogAlpha >= 0) {
																				if (targetFogAlpha != currentFogAlpha || targetSkyHazeAlpha != currentSkyHazeAlpha) {
																								if (transitionDuration > 0) {
																												currentFogAlpha = Mathf.Lerp (initialFogAlpha, targetFogAlpha, (Time.time - transitionStartTime) / transitionDuration);
																												currentSkyHazeAlpha = Mathf.Lerp (initialSkyHazeAlpha, targetSkyHazeAlpha, (Time.time - transitionStartTime) / transitionDuration);
																								} else {
																												currentFogAlpha = targetFogAlpha;
																												currentSkyHazeAlpha = targetSkyHazeAlpha;
																												transitionAlpha = false;
																								}
																								fogMat.SetFloat ("_FogAlpha", currentFogAlpha);
																								SetSkyData ();
																				}
																} else if (currentFogAlpha != alpha || targetSkyHazeAlpha != currentSkyHazeAlpha) {
																				if (transitionDuration > 0) {
																								currentFogAlpha = Mathf.Lerp (initialFogAlpha, alpha, (Time.time - transitionStartTime) / transitionDuration);
																								currentSkyHazeAlpha = Mathf.Lerp (initialSkyHazeAlpha, alpha, (Time.time - transitionStartTime) / transitionDuration);
																				} else {
																								currentFogAlpha = alpha;
																								currentSkyHazeAlpha = skyAlpha;
																								transitionAlpha = false;
																				}
																				fogMat.SetFloat ("_FogAlpha", currentFogAlpha);
																				SetSkyData ();
																}
												}

												// Check color transitions
												if (transitionColor) {
																if (targetFogColors) {
																				if (targetFogColor1 != currentFogColor1 || targetFogColor2 != currentFogColor2) {
																								if (transitionDuration > 0) {
																												currentFogColor1 = Color.Lerp (initialFogColor1, targetFogColor1, (Time.time - transitionStartTime) / transitionDuration);
																												currentFogColor2 = Color.Lerp (initialFogColor2, targetFogColor2, (Time.time - transitionStartTime) / transitionDuration);
																								} else {
																												currentFogColor1 = targetFogColor1;
																												currentFogColor2 = targetFogColor2;
																												transitionColor = false;
																								}
																								fogMat.SetColor ("_FogColor", currentFogColor1);
																								fogMat.SetColor ("_FogColor2", currentFogColor2);
																				}
																} else if (currentFogColor1 != color || currentFogColor2 != color2) {
																				if (transitionDuration > 0) {
																								currentFogColor1 = Color.Lerp (initialFogColor1, color, (Time.time - transitionStartTime) / transitionDuration);
																								currentFogColor2 = Color.Lerp (initialFogColor2, color2, (Time.time - transitionStartTime) / transitionDuration);
																				} else {
																								currentFogColor1 = color;
																								currentFogColor2 = color2;
																								transitionColor = false;
																				}
																				fogMat.SetColor ("_FogColor", currentFogColor1);
																				fogMat.SetColor ("_FogColor2", currentFogColor2);
																}
												}

												// Updates sun illumination
												if (sun != null) {
																bool needFogColorUpdate = false;
																if (sun.transform.forward != sunDirection) {
																				needFogColorUpdate = true;
																}
																if (sunLight != null) {
																				if (sunLight.color != sunColor || sunLight.intensity != sunIntensity) {
																								needFogColorUpdate = true;
																				}
																}
																if (needFogColorUpdate)
																				UpdateFogColor ();
												}

								}

								void OnDidApplyAnimationProperties () {   // support for animating property based fields
												shouldUpdateMaterialProperties = true;
								}

								public void CheckPreset () {
												if (_preset != FOG_PRESET.Custom) {
																_effectType = FOG_TYPE.DesktopFogWithSkyHaze;
												}
												switch (preset) {
												case FOG_PRESET.Clear:
																alpha = 0;
																break;
												case FOG_PRESET.Mist:
																alpha = 0.75f;
																skySpeed = 0.11f;
																skyHaze = 15;
																skyNoiseStrength = 1;
																skyAlpha = 0.33f;
																distance = 0;
																distanceFallOff = 0.07f;
																height = 4.4f;
																heightFallOff = 1;
																turbulence = 0;
																noiseStrength = 0.6f;
																speed = 0.01f;
																color = new Color (0.89f, 0.89f, 0.89f, 1);
																color2 = color;
																maxDistance = 0.999f;
																maxDistanceFallOff = 0f;
																break;
												case FOG_PRESET.WindyMist:
																alpha = 0.75f;
																skySpeed = 0.3f;
																skyHaze = 35;
																skyNoiseStrength = 0.32f;
																skyAlpha = 0.33f;
																distance = 0;
																distanceFallOff = 0.07f;
																height = 2f;
																heightFallOff = 1;
																turbulence = 2;
																noiseStrength = 0.6f;
																speed = 0.06f;
																color = new Color (0.89f, 0.89f, 0.89f, 1);
																color2 = color;
																maxDistance = 0.999f;
																maxDistanceFallOff = 0f;
																break;
												case FOG_PRESET.GroundFog:
																alpha = 1;
																skySpeed = 0.3f;
																skyHaze = 35;
																skyNoiseStrength = 0.32f;
																skyAlpha = 0.33f;
																distance = 0;
																distanceFallOff = 0;
																height = 1f;
																heightFallOff = 1;
																turbulence = 0.4f;
																noiseStrength = 0.7f;
																speed = 0.005f;
																color = new Color (0.89f, 0.89f, 0.89f, 1);
																color2 = color;
																maxDistance = 0.999f;
																maxDistanceFallOff = 0f;
																break;
												case FOG_PRESET.Fog:
																alpha = 0.96f;
																skySpeed = 0.3f;
																skyHaze = 155;
																skyNoiseStrength = 0.6f;
																skyAlpha = 0.93f;
																distance = effectType.isPlus () ? 0.2f : 0.01f;
																distanceFallOff = 0.04f;
																height = 20f;
																heightFallOff = 1;
																turbulence = 0.4f;
																noiseStrength = 0.4f;
																speed = 0.005f;
																color = new Color (0.89f, 0.89f, 0.89f, 1);
																color2 = color;
																maxDistance = 0.999f;
																maxDistanceFallOff = 0f;
																break;
												case FOG_PRESET.HeavyFog:
																alpha = 1;
																skySpeed = 0.05f;
																skyHaze = 350;
																skyNoiseStrength = 0.8f;
																skyAlpha = 0.97f;
																distance = effectType.isPlus () ? 0.1f : 0f;
																distanceFallOff = 0.045f;
																height = 35f;
																heightFallOff = 0.88f;
																turbulence = 0.4f;
																noiseStrength = 0.24f;
																speed = 0.003f;
																color = new Color (0.86f, 0.847f, 0.847f, 1);
																color2 = color;
																maxDistance = 0.999f;
																maxDistanceFallOff = 0f;
																break;
												case FOG_PRESET.SandStorm:
																alpha = 1;
																skySpeed = 0.49f;
																skyHaze = 333;
																skyNoiseStrength = 0.72f;
																skyAlpha = 0.97f;
																distance = effectType.isPlus () ? 0.15f : 0f;
																distanceFallOff = 0.028f;
																height = 83f;
																heightFallOff = 0;
																turbulence = 15;
																noiseStrength = 0.45f;
																speed = 0.2f;
																color = new Color (0.364f, 0.36f, 0.36f, 1);
																color2 = color;
																maxDistance = 0.999f;
																maxDistanceFallOff = 0f;
																break;
												}
								}

								void OnPreCull () {
												if (currentCamera != null && currentCamera.depthTextureMode == DepthTextureMode.None) {
																currentCamera.depthTextureMode = DepthTextureMode.Depth;
												}
								}

								// Postprocess the image
								void OnRenderImage (RenderTexture source, RenderTexture destination) {
												if (fogMat == null || _alpha == 0 || currentCamera == null) {
																Graphics.Blit (source, destination);
																return;
												}

												if (shouldUpdateMaterialProperties) {
																shouldUpdateMaterialProperties = false;
																UpdateMaterialPropertiesNow ();
												}

												if (currentCamera.orthographic) {
																if (!matOrtho)
																				ResetMaterial ();
																fogMat.SetVector ("_ClipDir", currentCamera.transform.forward);
												} else {
																if (matOrtho)
																				ResetMaterial ();
												}

												if (useSinglePassStereoRenderingMatrix && UnityEngine.XR.XRSettings.enabled) {
																fogMat.SetMatrix ("_ClipToWorld", currentCamera.cameraToWorldMatrix);
												} else {
																fogMat.SetMatrix ("_ClipToWorld", currentCamera.cameraToWorldMatrix * currentCamera.projectionMatrix.inverse);
												}
												Graphics.Blit (source, destination, fogMat);
								}

								void ResetMaterial () {
												fogMat = null;
												fogMatAdv = null;
												fogMatFogSky = null;
												fogMatOnlyFog = null;
												fogMatSimple = null;
												fogMatBasic = null;
												fogMatVol = null;
												fogMatDesktopPlusOrthogonal = null;
												fogMatOrthogonal = null;
												UpdateMaterialProperties ();
								}

								public void UpdateMaterialProperties () {
												if (Application.isPlaying) {
																shouldUpdateMaterialProperties = true;
												} else {
																UpdateMaterialPropertiesNow ();
												}
								}

								void UpdateMaterialPropertiesNow () {
												CheckPreset ();
												CopyTransitionValues ();

												if (currentCamera == null)
																currentCamera = GetComponent<Camera> ();
												
												string matName;
												switch (effectType) {
												case FOG_TYPE.MobileFogOnlyGround:
																if (fogMatOnlyFog == null) {
																				if (currentCamera.orthographic) {
																								matOrtho = true;
																								matName = "Materials/DFOOnlyFog";
																				} else {
																								matOrtho = false;
																								matName = "Materials/DFGOnlyFog";
																				}
																				fogMatOnlyFog = Instantiate (Resources.Load<Material> (matName)) as Material;
																				fogMatOnlyFog.hideFlags = HideFlags.DontSave;
																}
																fogMat = fogMatOnlyFog;
																break;
												case FOG_TYPE.MobileFogWithSkyHaze:
																if (fogMatFogSky == null) {
																				if (currentCamera.orthographic) {
																								matOrtho = true;
																								matName = "Materials/DFOWithSky";
																				} else {
																								matOrtho = false;
																								matName = "Materials/DFGWithSky";
																				}
																				fogMatFogSky = Instantiate (Resources.Load<Material> (matName)) as Material;
																				fogMatFogSky.hideFlags = HideFlags.DontSave;
																}
																fogMat = fogMatFogSky;
																break;
												case FOG_TYPE.DesktopFogPlusWithSkyHaze:
																if (fogMatVol == null) {
																				if (currentCamera.orthographic) {
																								matOrtho = true;
																								matName = "Materials/DFODesktopPlus";
																				} else {
																								matOrtho = false;
																								matName = "Materials/DFGDesktopPlus";
																				}
																				fogMatVol = Instantiate (Resources.Load<Material> (matName)) as Material;
																				fogMatVol.hideFlags = HideFlags.DontSave;
																}
																fogMat = fogMatVol;
																break;
												case FOG_TYPE.MobileFogSimple:
																if (fogMatSimple == null) {
																				if (currentCamera.orthographic) {
																								matOrtho = true;
																								matName = "Materials/DFOSimple";
																				} else {
																								matOrtho = false;
																								matName = "Materials/DFGSimple";
																				}
																				fogMatSimple = Instantiate (Resources.Load<Material> (matName)) as Material;
																				fogMatSimple.hideFlags = HideFlags.DontSave;
																}
																fogMat = fogMatSimple;
																break;
												case FOG_TYPE.MobileFogBasic:
																if (fogMatBasic == null) {
																				if (currentCamera.orthographic) {
																								matOrtho = true;
																								matName = "Materials/DFOBasic";
																				} else {
																								matOrtho = false;
																								matName = "Materials/DFGBasic";
																				}
																				fogMatBasic = Instantiate (Resources.Load<Material> (matName)) as Material;
																				fogMatBasic.hideFlags = HideFlags.DontSave;
																}
																fogMat = fogMatBasic;
																break;
												case FOG_TYPE.MobileFogOrthogonal:
																if (fogMatOrthogonal == null) {
																				if (currentCamera.orthographic) {
																								matOrtho = true;
																								matName = "Materials/DFOOrthogonal";
																				} else {
																								matOrtho = false;
																								matName = "Materials/DFGOrthogonal";
																				}
																				fogMatOrthogonal = Instantiate (Resources.Load<Material> (matName)) as Material;
																				fogMatOrthogonal.hideFlags = HideFlags.DontSave;
																}
																fogMat = fogMatOrthogonal;
																break;
												case FOG_TYPE.DesktopFogPlusOrthogonal:
																if (fogMatDesktopPlusOrthogonal == null) {
																				if (currentCamera.orthographic) {
																								matOrtho = true;
																								matName = "Materials/DFODesktopPlusOrthogonal";
																				} else {
																								matOrtho = false;
																								matName = "Materials/DFGDesktopPlusOrthogonal";
																				}
																				fogMatDesktopPlusOrthogonal = Instantiate (Resources.Load<Material> (matName)) as Material;
																				fogMatDesktopPlusOrthogonal.hideFlags = HideFlags.DontSave;
																}
																fogMat = fogMatDesktopPlusOrthogonal;
																break;
												default:
																if (fogMatAdv == null) {
																				if (currentCamera.orthographic) {
																								matOrtho = true;
																								matName = "Materials/DFODesktop";
																				} else {
																								matOrtho = false;
																								matName = "Materials/DFGDesktop";
																				}
																				fogMatAdv = Instantiate (Resources.Load<Material> (matName)) as Material;
																				fogMatAdv.hideFlags = HideFlags.DontSave;
																}
																fogMat = fogMatAdv;
																break;
												}

												if (fogMat == null)
																return;

												float sp = effectType == FOG_TYPE.DesktopFogPlusWithSkyHaze ? _speed * 5f : _speed;
												fogMat.SetVector ("_FogSpeed", -_windDirection.normalized * sp);

												Vector4 noiseData = new Vector4 (_noiseStrength, _turbulence, currentCamera.farClipPlane * 15.0f / 1000f, _noiseScale);
												fogMat.SetVector ("_FogNoiseData", noiseData);

												Vector4 heightData = new Vector4 (_height + 0.001f, _baselineHeight, _clipUnderBaseline ? -0.01f : -10000, _heightFallOff);
												if (_effectType == FOG_TYPE.MobileFogOrthogonal || _effectType == FOG_TYPE.DesktopFogPlusOrthogonal) {
																heightData.z = maxHeight;
												}
												fogMat.SetVector ("_FogHeightData", heightData);

												fogMat.SetFloat ("_FogAlpha", currentFogAlpha);
												Vector4 distanceData = new Vector4 (_distance, _distanceFallOff, _maxDistance, _maxDistanceFallOff);
												if (effectType.isPlus ()) {
																distanceData.x = currentCamera.farClipPlane * _distance;
																distanceData.y = distanceFallOff * distanceData.x + 0.0001f;
																distanceData.z *= currentCamera.farClipPlane;
												}
												fogMat.SetVector ("_FogDistance", distanceData);

												UpdateFogColor ();
												SetSkyData ();

												if (shaderKeywords == null) {
																shaderKeywords = new List<string> ();
												} else {
																shaderKeywords.Clear ();
												}

												if (fogOfWarEnabled) {
																if (fogOfWarTexture == null) {
																				UpdateFogOfWarTexture ();
																}
																fogMat.SetTexture ("_FogOfWar", fogOfWarTexture);
																fogMat.SetVector ("_FogOfWarCenter", _fogOfWarCenter);
																fogMat.SetVector ("_FogOfWarSize", _fogOfWarSize);
																Vector3 ca = fogOfWarCenter - 0.5f * _fogOfWarSize;
																fogMat.SetVector ("_FogOfWarCenterAdjusted", new Vector3 (ca.x / _fogOfWarSize.x, 1f, ca.z / _fogOfWarSize.z));
																shaderKeywords.Add ("FOG_OF_WAR_ON");
												} 
												if (_enableDithering) {
																fogMat.SetFloat ("_FogDither", _ditherStrength);
																shaderKeywords.Add ("DITHER_ON");
												}
												fogMat.shaderKeywords = shaderKeywords.ToArray ();
								}

								void CopyTransitionValues () {
												currentFogAlpha = _alpha;
												currentSkyHazeAlpha = _skyAlpha;
												currentFogColor1 = _color;
												currentFogColor2 = _color2;
								}


								void SetSkyData () {
												// x = haze, y = speed, z = noise, w = alpha
												Vector4 skyData = new Vector4 (_skyHaze, _skySpeed, _skyNoiseStrength, currentSkyHazeAlpha);
												fogMat.SetVector ("_FogSkyData", skyData);
								}

								void UpdateFogColor () {
												if (fogMat == null)
																return;

												if (_sun != null) {
																if (sunLight == null)
																				sunLight = _sun.GetComponent<Light> ();
																if (sunLight != null && sunLight.transform != _sun.transform) {
																				sunLight = _sun.GetComponent<Light> ();
																}
																sunDirection = _sun.transform.forward;
																if (sunLight != null) {
																				sunColor = sunLight.color;
																				sunIntensity = sunLight.intensity;
																}
												}
			
												float fogIntensity = sunIntensity * Mathf.Clamp01 (1.0f - sunDirection.y);
												fogMat.SetColor ("_FogColor", fogIntensity * currentFogColor1 * sunColor);
												fogMat.SetColor ("_FogColor2", fogIntensity * currentFogColor2 * sunColor);
												Color sColor = fogIntensity * scatteringColor;
												fogMat.SetColor ("_SunColor", new Vector4 (sColor.r, sColor.g, sColor.b, scattering));
												fogMat.SetVector ("_SunDir", -sunDirection);

								}

								#region Fog Volume

								public void SetTargetProfile (DynamicFogProfile targetProfile, float duration) {
												if (!_useFogVolumes)
																return;
												this.preset = FOG_PRESET.Custom;
												this.initialProfile = ScriptableObject.CreateInstance<DynamicFogProfile> ();
												this.initialProfile.Save (this);
												this.targetProfile = targetProfile;
												this.transitionDuration = duration;
												this.transitionStartTime = Time.time;
												this.transitionProfile = true;
								}

								public void ClearTargetProfile (float duration) {
												SetTargetProfile (initialProfile, duration);
								}


								public void SetTargetAlpha (float newFogAlpha, float newSkyHazeAlpha, float duration) {
												if (!useFogVolumes)
																return;
												this.preset = FOG_PRESET.Custom;
												this.initialFogAlpha = currentFogAlpha;
												this.initialSkyHazeAlpha = currentSkyHazeAlpha;
												this.targetFogAlpha = newFogAlpha;
												this.targetSkyHazeAlpha = newSkyHazeAlpha;
												this.transitionDuration = duration;
												this.transitionStartTime = Time.time;
												this.transitionAlpha = true;
								}

								public void ClearTargetAlpha (float duration) {
												SetTargetAlpha (-1, -1, duration);
								}


								public void SetTargetColors (Color color1, Color color2, float duration) {
												if (!useFogVolumes)
																return;
												this.preset = FOG_PRESET.Custom;
												this.initialFogColor1 = currentFogColor1;
												this.initialFogColor2 = currentFogColor2;
												this.targetFogColor1 = color1;
												this.targetFogColor2 = color2;
												this.transitionDuration = duration;
												this.transitionStartTime = Time.time;
												this.targetFogColors = true;
												this.transitionColor = true;
								}

								public void ClearTargetColors (float duration) {
												this.targetFogColors = false;
												SetTargetColors (color, color2, duration);
								}

								#endregion

								#region Fog of War stuff

								void UpdateFogOfWarTexture () {
												if (!fogOfWarEnabled)
																return;
												int size = GetScaledSize (fogOfWarTextureSize, 1.0f);
												//			fogOfWarTexture = new Texture2D(size, size, TextureFormat.Alpha8, false);
												fogOfWarTexture = new Texture2D (size, size, TextureFormat.ARGB32, false);
												fogOfWarTexture.hideFlags = HideFlags.DontSave;
												fogOfWarTexture.filterMode = FilterMode.Bilinear;
												fogOfWarTexture.wrapMode = TextureWrapMode.Clamp;
												ResetFogOfWar ();
								}

								/// <summary>
								/// Changes the alpha value of the fog of war at world position. It takes into account FogOfWarCenter and FogOfWarSize.
								/// Note that only x and z coordinates are used. Y (vertical) coordinate is ignored.
								/// </summary>
								/// <param name="worldPosition">in world space coordinates.</param>
								/// <param name="radius">radius of application in world units.</param>
								public void SetFogOfWarAlpha (Vector3 worldPosition, float radius, float fogNewAlpha) {
												if (fogOfWarTexture == null)
																return;
			
												float tx = (worldPosition.x - fogOfWarCenter.x) / fogOfWarSize.x + 0.5f;
												if (tx < 0 || tx > 1f)
																return;
												float tz = (worldPosition.z - fogOfWarCenter.z) / fogOfWarSize.z + 0.5f;
												if (tz < 0 || tz > 1f)
																return;
			
												int tw = fogOfWarTexture.width;
												int th = fogOfWarTexture.height;
												int px = (int)(tx * tw);
												int pz = (int)(tz * th);
												int colorBufferPos = pz * tw + px;
												byte newAlpha8 = (byte)(fogNewAlpha * 255);
												Color32 existingColor = fogOfWarColorBuffer [colorBufferPos];
												if (newAlpha8 != existingColor.a) { // just to avoid over setting the texture in an Update() loop
																float tr = radius / fogOfWarSize.z;
																int delta = Mathf.FloorToInt (th * tr);
																for (int r = pz - delta; r <= pz + delta; r++) {
																				if (r > 0 && r < th - 1) {
																								for (int c = px - delta; c <= px + delta; c++) {
																												if (c > 0 && c < tw - 1) {
																																int distance = Mathf.FloorToInt (Mathf.Sqrt ((pz - r) * (pz - r) + (px - c) * (px - c)));
																																if (distance <= delta) {
																																				colorBufferPos = r * tw + c;
																																				Color32 colorBuffer = fogOfWarColorBuffer [colorBufferPos];
																																				colorBuffer.a = (byte)Mathf.Lerp (newAlpha8, colorBuffer.a, (float)distance / delta);
																																				fogOfWarColorBuffer [colorBufferPos] = colorBuffer;
																																				fogOfWarTexture.SetPixel (c, r, colorBuffer);
																																}
																												}
																								}
																				}
																}
																fogOfWarTexture.Apply ();
												}
								}

								public void ResetFogOfWarAlpha (Vector3 worldPosition, float radius) {
												if (fogOfWarTexture == null)
																return;
			
												float tx = (worldPosition.x - fogOfWarCenter.x) / fogOfWarSize.x + 0.5f;
												if (tx < 0 || tx > 1f)
																return;
												float tz = (worldPosition.z - fogOfWarCenter.z) / fogOfWarSize.z + 0.5f;
												if (tz < 0 || tz > 1f)
																return;
			
												int tw = fogOfWarTexture.width;
												int th = fogOfWarTexture.height;
												int px = (int)(tx * tw);
												int pz = (int)(tz * th);
												int colorBufferPos = pz * tw + px;
												float tr = radius / fogOfWarSize.z;
												int delta = Mathf.FloorToInt (th * tr);
												for (int r = pz - delta; r <= pz + delta; r++) {
																if (r > 0 && r < th - 1) {
																				for (int c = px - delta; c <= px + delta; c++) {
																								if (c > 0 && c < tw - 1) {
																												int distance = Mathf.FloorToInt (Mathf.Sqrt ((pz - r) * (pz - r) + (px - c) * (px - c)));
																												if (distance <= delta) {
																																colorBufferPos = r * tw + c;
																																Color32 colorBuffer = fogOfWarColorBuffer [colorBufferPos];
																																colorBuffer.a = 255;
																																fogOfWarColorBuffer [colorBufferPos] = colorBuffer;
																																fogOfWarTexture.SetPixel (c, r, colorBuffer);
																												}
																								}
																				}
																}
																fogOfWarTexture.Apply ();
												}
								}

								public void ResetFogOfWar () {
												if (fogOfWarTexture == null)
																return;
												int h = fogOfWarTexture.height;
												int w = fogOfWarTexture.width;
												int newLength = h * w;
												if (fogOfWarColorBuffer == null || fogOfWarColorBuffer.Length != newLength) {
																fogOfWarColorBuffer = new Color32[newLength];
												}
												Color32 opaque = new Color32 (255, 255, 255, 255);
												for (int k = 0; k < newLength; k++)
																fogOfWarColorBuffer [k] = opaque;
												fogOfWarTexture.SetPixels32 (fogOfWarColorBuffer);
												fogOfWarTexture.Apply ();
								}

								int GetScaledSize (int size, float factor) {
												size = (int)(size / factor);
												size /= 4;
												if (size < 1)
																size = 1;
												return size * 4;
								}

								#endregion

				}

}