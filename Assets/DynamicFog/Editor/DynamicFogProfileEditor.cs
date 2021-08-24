using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace DynamicFogAndMist {
				[CustomEditor (typeof(DynamicFogProfile))]
				public class DynamicFogProfileEditor : Editor {

								static GUIStyle titleLabelStyle;
								static Color titleColor;
								static GUIContent[] FOG_TYPE_OPTIONS = new GUIContent[] { 
												new GUIContent ("Desktop Fog Plus + Sky Haze"), 
												new GUIContent ("Desktop Fog Plus (Orthogonal)"), 
												new GUIContent ("Desktop Fog + Sky Haze"),
												new GUIContent ("Mobile Fog Plus (Simplified)"),
												new GUIContent ("Mobile Fog + Sky Haze"), 
												new GUIContent ("Mobile Fog (No Sky Haze)"),
												new GUIContent ("Mobile Fog (Orthogonal)"),
												new GUIContent ("Mobile Fog (Basic)")
								};
								static int[] FOG_TYPE_VALUES = new int[] {
												(int)FOG_TYPE.DesktopFogPlusWithSkyHaze,
												(int)FOG_TYPE.DesktopFogPlusOrthogonal,
												(int)FOG_TYPE.DesktopFogWithSkyHaze,
												(int)FOG_TYPE.MobileFogSimple,
												(int)FOG_TYPE.MobileFogWithSkyHaze,
												(int)FOG_TYPE.MobileFogOnlyGround,
												(int)FOG_TYPE.MobileFogOrthogonal,
												(int)FOG_TYPE.MobileFogBasic
								};
								const int FOG_PROPERTIES = 0;
								const int SKY_PROPERTIES = 1;
								SerializedProperty effectType, enableDithering, ditherStrength;
								SerializedProperty alpha, noiseStrength, noiseScale, distance, distanceFallOff, maxDistance, maxDistanceFallOff, height, maxHeight;
								SerializedProperty heightFallOff, baselineHeight, clipUnderBaseline, turbulence, speed, windDirection, color, color2;
								SerializedProperty skyHaze, skySpeed, skyNoiseStrength, skyAlpha, scattering, scatteringColor;

								void OnEnable () {
												titleColor = EditorGUIUtility.isProSkin ? new Color (0.52f, 0.66f, 0.9f) : new Color (0.12f, 0.16f, 0.4f);

												effectType = serializedObject.FindProperty ("effectType");
												enableDithering = serializedObject.FindProperty ("enableDithering");
												ditherStrength = serializedObject.FindProperty ("ditherStrength");

												alpha = serializedObject.FindProperty ("alpha");
												noiseStrength = serializedObject.FindProperty ("noiseStrength");
												noiseScale = serializedObject.FindProperty ("noiseScale");
												distance = serializedObject.FindProperty ("distance");
												distanceFallOff = serializedObject.FindProperty ("distanceFallOff");
												maxDistance = serializedObject.FindProperty ("maxDistance");
												maxDistanceFallOff = serializedObject.FindProperty ("maxDistanceFallOff");
												maxHeight = serializedObject.FindProperty ("maxHeight");
												height = serializedObject.FindProperty ("height");
												heightFallOff = serializedObject.FindProperty ("heightFallOff");
												baselineHeight = serializedObject.FindProperty ("baselineHeight");
												clipUnderBaseline = serializedObject.FindProperty ("clipUnderBaseline");
												turbulence = serializedObject.FindProperty ("turbulence");
												speed = serializedObject.FindProperty ("speed");
												windDirection = serializedObject.FindProperty ("windDirection");

												color = serializedObject.FindProperty ("color");
												color2 = serializedObject.FindProperty ("color2");
												skyHaze = serializedObject.FindProperty ("skyHaze");
												skySpeed = serializedObject.FindProperty ("skySpeed");
												skyNoiseStrength = serializedObject.FindProperty ("skyNoiseStrength");
												skyAlpha = serializedObject.FindProperty ("skyAlpha");
												scattering = serializedObject.FindProperty ("scattering");
												scatteringColor = serializedObject.FindProperty ("scatteringColor");
								}

								public override void OnInspectorGUI () {
												serializedObject.Update();
									
												EditorGUILayout.Separator ();

												DrawTitleLabel("General Settings");

												EditorGUILayout.IntPopup (effectType, FOG_TYPE_OPTIONS, FOG_TYPE_VALUES, new GUIContent ("Effect Type", "Choose a shader variant. Each variant provides different capabilities. Read documentation for explanation."));
												switch (effectType.intValue) {
												case (int)FOG_TYPE.DesktopFogPlusWithSkyHaze:
																EditorGUILayout.HelpBox ("BEST IMMERSION. 5 step raymarching based fog effect which does not require geometry. Uses a more complex algorithm to simulate 3D noise in world space. Also adds sky haze at the background.", MessageType.Info);
																break;
												case (int)FOG_TYPE.DesktopFogPlusOrthogonal:
																EditorGUILayout.HelpBox ("Variant of Desktop Plus With Sky Haze which treats distance fog and height fog separately. This variant does not add sky haze at the background.", MessageType.Info);
																break;
												case (int)FOG_TYPE.DesktopFogWithSkyHaze:
																EditorGUILayout.HelpBox ("Depth based fog effect that lays out fog over existing geometry. Uses two noise textures and adds haze effect at the background to complete fog composition (geometry fog + sky fog).", MessageType.Info);
																break;
												case (int)FOG_TYPE.MobileFogWithSkyHaze:
																EditorGUILayout.HelpBox ("Depth based fog effect over existing geometry. Similar to Desktop Fog with Sky, but uses one noise texture instead of two, and also adds haze effect at the background to complete fog composition (geometry fog + sky fog).", MessageType.Info);
																break;
												case (int)FOG_TYPE.MobileFogOnlyGround:
																EditorGUILayout.HelpBox ("FASTEST, Depth based fog effect over existing geometry. Similar to Mobile Fog with Sky, but only affects geometry (no sky haze).", MessageType.Info);
																break;
												case (int)FOG_TYPE.MobileFogSimple:
																EditorGUILayout.HelpBox ("GREAT PERFORMANCE/QUALITY. Similar to Desktop Fog Plus with Sky Haze but uses 3 steps instead of 5 and does not add sky haze at the background.", MessageType.Info);
																break;
												case (int)FOG_TYPE.MobileFogOrthogonal:
																EditorGUILayout.HelpBox ("Variant of Mobile Fog (Simplified) which treats distance fog and height fog separately. Does not use noise textures. Does not adds sky haze at the background.", MessageType.Info);
																break;
												case (int)FOG_TYPE.MobileFogBasic:
																EditorGUILayout.HelpBox ("FASTEST, Ray-marching variant. Uses only 1 step. Does not use noise textures. Does not adds sky haze at the background.", MessageType.Info);
																break;
												}

												int effect = effectType.intValue;

															EditorGUILayout.PropertyField (enableDithering, new GUIContent ("Enable Dithering", "Reduces banding artifacts."));
												if (enableDithering.boolValue) {
																EditorGUILayout.PropertyField (ditherStrength, new GUIContent ("   Dither Strength", "Intensity of dither blending."));
												}


												EditorGUILayout.PropertyField (scattering, new GUIContent ("Light Scattering", "Amount of Sun light diffusion when it crosses the fog towards viewer."));
												EditorGUILayout.PropertyField (scatteringColor, new GUIContent ("   Scattering Color", "Tint color for the light scattering effect."));

												EditorGUILayout.Separator ();
												DrawTitleLabel ("Fog Settings");
												EditorGUILayout.PropertyField (alpha, new GUIContent ("Alpha", "Global fog transparency. You can also change the transparency at color level."));
												if (effect != 4 && effect != 5 && effect != 6) {
																EditorGUILayout.PropertyField (noiseStrength, new GUIContent ("Noise Strength", "Set this value to zero to use solid colors."));
																EditorGUILayout.PropertyField (noiseScale, new GUIContent ("Noise Scale", "Scale factor for sampling noise."));
												}
												EditorGUILayout.PropertyField (distance, new GUIContent ("Distance", "The starting distance of the fog measure in linear 0-1 values (0=camera near clip, 1=camera far clip)."));
												EditorGUILayout.PropertyField (distanceFallOff, new GUIContent ("Distance Fall Off", "Makes the fog appear smoothly on the near distance."));
												if (effect < 4)
																EditorGUILayout.PropertyField (maxDistance, new GUIContent ("Max Distance", "The end distance of the fog measure in linear 0-1 values (0=camera near clip, 1=camera far clip)."));
												if (effect < 3) {
																EditorGUILayout.PropertyField (maxDistanceFallOff, new GUIContent ("Distance Fall Off", "Makes the fog disappear smoothly on the far distance."));
												}

																if (effect == (int)FOG_TYPE.MobileFogOrthogonal || effect == (int)FOG_TYPE.DesktopFogPlusOrthogonal) {
																				EditorGUILayout.PropertyField (maxHeight, new GUIContent ("Max Height", "Max. height of the fog in meters."));
																}
																EditorGUILayout.PropertyField (height, new GUIContent ("Height", "Height of the fog in meters."));
																EditorGUILayout.PropertyField (heightFallOff, new GUIContent ("Height Fall Off", "Increase to make the fog change gradually its density based on height."));
																EditorGUILayout.PropertyField (baselineHeight, new GUIContent ("Baseline Height", "Vertical position of the fog in meters. Height is counted above this baseline height."));

																if (effect < 3) {
																				EditorGUILayout.PropertyField (clipUnderBaseline, new GUIContent ("Clip Under Baseline", "Enable this property to only render fog above baseline height."));
																				EditorGUILayout.PropertyField (turbulence, new GUIContent ("Turbulence", "Amount of fog turbulence."));
																}

												if (effect < 4 || effect == (int)FOG_TYPE.DesktopFogPlusOrthogonal) {
																EditorGUILayout.PropertyField (speed, new GUIContent ("Speed", "Speed of fog animation if noise strength or turbulence > 0 (turbulence not available in Desktop Fog Plus mode)."));
																EditorGUILayout.PropertyField (windDirection, new GUIContent ("Wind Direction", "Direction of the wind to take into account for the fog animation."));
												}

																EditorGUILayout.PropertyField (color);
																if (effect != 4 && effect != 5)
																				EditorGUILayout.PropertyField (color2);
												
												if (effect != 2 && effect != 4 && effect != 5 && effect != (int)FOG_TYPE.DesktopFogPlusOrthogonal) {
																EditorGUILayout.Separator ();
																DrawTitleLabel ("Sky Settings");
																				EditorGUILayout.PropertyField (skyHaze, new GUIContent ("Haze", "Vertical range for the sky haze."));
																				EditorGUILayout.PropertyField (skySpeed, new GUIContent ("Speed", "Speed of sky haze animation."));
																				EditorGUILayout.PropertyField (skyNoiseStrength, new GUIContent ("Noise Strength", "Amount of noise for the sky haze effect."));
																				EditorGUILayout.PropertyField (skyAlpha, new GUIContent ("Alpha", "Transparency of sky haze."));
												}
											
												EditorGUILayout.Separator ();

												if (serializedObject.ApplyModifiedProperties () || (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed")) {
																// Triggers profile reload on all Volumetric Fog scripts
																DynamicFog[] fogs = FindObjectsOfType<DynamicFog> ();
																for (int t = 0; t < targets.Length; t++) {
																				DynamicFogProfile profile = (DynamicFogProfile)targets [t];
																				for (int k = 0; k < fogs.Length; k++) {
																								if (fogs [k] != null && fogs [k].profile == profile) {
																												profile.Load (fogs [k]);
																								}
																				}
																}
																EditorUtility.SetDirty (target);
												}
								}

								void DrawTitleLabel (string s) {
												if (titleLabelStyle == null) {
																titleLabelStyle = new GUIStyle (GUI.skin.label);
												}
												titleLabelStyle.normal.textColor = titleColor;
												titleLabelStyle.fontStyle = FontStyle.Bold;
												GUILayout.Label (s, titleLabelStyle);
								}


				}

}
