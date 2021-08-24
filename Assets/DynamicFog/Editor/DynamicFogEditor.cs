using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace DynamicFogAndMist {
				[CustomEditor (typeof(DynamicFog))]
				public class DynamicFogEditor : Editor {

								static GUIStyle titleLabelStyle, sectionHeaderStyle;
								static Color titleColor;
								static bool[] expandSection = new bool[5];
								const string SECTION_PREFS = "DynamicFogExpandSection";
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
								static string[] sectionNames = new string[] {
												"Fog Properties",
												"Sky Properties",
												"Fog of War"
								};
								const int FOG_PROPERTIES = 0;
								const int SKY_PROPERTIES = 1;
								const int FOG_OF_WAR = 2;
								SerializedProperty effectType, preset, useFogVolumes, enableDithering, ditherStrength, useSinglePassStereoRenderingMatrix;
								SerializedProperty alpha, noiseStrength, noiseScale, distance, distanceFallOff, maxDistance, maxDistanceFallOff, height, maxHeight;
								SerializedProperty heightFallOff, baselineHeight, clipUnderBaseline, turbulence, speed, windDirection, color, color2;
								SerializedProperty skyHaze, skySpeed, skyNoiseStrength, skyAlpha, sun, scattering, scatteringColor;
								SerializedProperty fogOfWarEnabled, fogOfWarCenter, fogOfWarSize, fogOfWarTextureSize;
								bool profileChanges;
								DynamicFog _fog;

								void OnEnable () {
												titleColor = EditorGUIUtility.isProSkin ? new Color (0.52f, 0.66f, 0.9f) : new Color (0.12f, 0.16f, 0.4f);
												for (int k = 0; k < expandSection.Length; k++) {
																expandSection [k] = EditorPrefs.GetBool (SECTION_PREFS + k, false);
												}
												effectType = serializedObject.FindProperty ("_effectType");
												preset = serializedObject.FindProperty ("_preset");
												useFogVolumes = serializedObject.FindProperty ("_useFogVolumes");
												enableDithering = serializedObject.FindProperty ("_enableDithering");
												ditherStrength = serializedObject.FindProperty ("_ditherStrength");
												#if UNITY_5_4_OR_NEWER
												useSinglePassStereoRenderingMatrix = serializedObject.FindProperty ("_useSinglePassStereoRenderingMatrix");
#endif

												alpha = serializedObject.FindProperty ("_alpha");
												noiseStrength = serializedObject.FindProperty ("_noiseStrength");
												noiseScale = serializedObject.FindProperty ("_noiseScale");
												distance = serializedObject.FindProperty ("_distance");
												distanceFallOff = serializedObject.FindProperty ("_distanceFallOff");
												maxDistance = serializedObject.FindProperty ("_maxDistance");
												maxDistanceFallOff = serializedObject.FindProperty ("_maxDistanceFallOff");
												maxHeight = serializedObject.FindProperty ("_maxHeight");
												height = serializedObject.FindProperty ("_height");
												heightFallOff = serializedObject.FindProperty ("_heightFallOff");
												baselineHeight = serializedObject.FindProperty ("_baselineHeight");
												clipUnderBaseline = serializedObject.FindProperty ("_clipUnderBaseline");
												turbulence = serializedObject.FindProperty ("_turbulence");
												speed = serializedObject.FindProperty ("_speed");
												windDirection = serializedObject.FindProperty ("_windDirection");

												color = serializedObject.FindProperty ("_color");
												color2 = serializedObject.FindProperty ("_color2");
												skyHaze = serializedObject.FindProperty ("_skyHaze");
												skySpeed = serializedObject.FindProperty ("_skySpeed");
												skyNoiseStrength = serializedObject.FindProperty ("_skyNoiseStrength");
												skyAlpha = serializedObject.FindProperty ("_skyAlpha");
												sun = serializedObject.FindProperty ("_sun");
												scattering = serializedObject.FindProperty ("_scattering");
												scatteringColor = serializedObject.FindProperty ("_scatteringColor");

												fogOfWarEnabled = serializedObject.FindProperty ("_fogOfWarEnabled");
												fogOfWarCenter = serializedObject.FindProperty ("_fogOfWarCenter");
												fogOfWarSize = serializedObject.FindProperty ("_fogOfWarSize");
												fogOfWarTextureSize = serializedObject.FindProperty ("_fogOfWarTextureSize");

												_fog = (DynamicFog)target;
								}

								void OnDestroy () {
												// Save folding sections state
												for (int k = 0; k < expandSection.Length; k++) {
																EditorPrefs.SetBool (SECTION_PREFS + k, expandSection [k]);
												}
								}

								public override void OnInspectorGUI () {
												#if UNITY_5_6_OR_NEWER
												serializedObject.UpdateIfRequiredOrScript ();
												#else
												serializedObject.UpdateIfDirtyOrScript ();
												#endif

												if (sectionHeaderStyle == null) {
																sectionHeaderStyle = new GUIStyle (EditorStyles.foldout);
												}
												sectionHeaderStyle.normal.textColor = titleColor;
												sectionHeaderStyle.margin = new RectOffset (12, 0, 0, 0);
												sectionHeaderStyle.fontStyle = FontStyle.Bold;

												if (titleLabelStyle == null) {
																titleLabelStyle = new GUIStyle (EditorStyles.label);
												}
												titleLabelStyle.normal.textColor = titleColor;
												titleLabelStyle.fontStyle = FontStyle.Bold;


												EditorGUILayout.Separator ();

												EditorGUILayout.BeginHorizontal ();
												EditorGUILayout.LabelField ("General Settings", titleLabelStyle);
												if (GUILayout.Button ("Help", GUILayout.Width (40))) {
																if (!EditorUtility.DisplayDialog ("Dynamic Fog & Mist", "To learn more about a property in this inspector move the mouse over the label for a quick description (tooltip).\n\nPlease check README file in the root of the asset for details and contact support.\n\nIf you like Dynamic Fog & Mist, please rate it on the Asset Store. For feedback and suggestions visit our support forum on kronnect.com.", "Close", "Visit Support Forum")) {
																				Application.OpenURL ("http://kronnect.com/taptapgo");
																}
												}
												EditorGUILayout.EndHorizontal ();

												int prevPreset = preset.intValue;
												bool prevFogVolumes = useFogVolumes.boolValue;
												int prevDither = enableDithering.intValue;
												float prevDitherStrength = ditherStrength.floatValue;

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

												EditorGUILayout.PropertyField (preset, new GUIContent ("Legacy Preset", "Amount of detail of the liquid effect. The 'Simple' setting does not use 3D textures which makes it compatible with mobile."));

												EditorGUILayout.BeginHorizontal ();
												_fog.profile = (DynamicFogProfile)EditorGUILayout.ObjectField (new GUIContent ("Profile", "Create or load stored presets."), _fog.profile, typeof(DynamicFogProfile), false);

												if (_fog.profile != null) {
																EditorGUILayout.EndHorizontal ();
																EditorGUILayout.BeginHorizontal ();
																GUILayout.Label ("", GUILayout.Width (130));
																if (GUILayout.Button (new GUIContent ("Create", "Creates a new profile which is a copy of the current settings."), GUILayout.Width (60))) {
																				CreateProfile ();
																				profileChanges = false;
																				GUIUtility.ExitGUI ();
																				return;
																}
																if (GUILayout.Button (new GUIContent ("Revert", "Updates fog settings with the profile configuration."), GUILayout.Width (60))) {
																				profileChanges = false;
																				_fog.profile.Load (_fog);
																}
																if (!profileChanges)
																				GUI.enabled = false;
																if (GUILayout.Button (new GUIContent ("Apply", "Updates profile configuration with changes in this inspector."), GUILayout.Width (60))) {
																				profileChanges = false;
																				_fog.profile.Save (_fog);
																}
																GUI.enabled = true;
												} else {
																if (GUILayout.Button (new GUIContent ("Create", "Creates a new profile which is a copy of the current settings."), GUILayout.Width (60))) {
																				CreateProfile ();
																				GUIUtility.ExitGUI ();
																				return;
																}
												}
												EditorGUILayout.EndHorizontal ();


												EditorGUILayout.PropertyField (useFogVolumes, new GUIContent ("Use Fog Volumes", "Enables fog volumes. These are zones which changes the transparency of the fog automatically, either making it disappear or appear."));

												#if UNITY_5_4_OR_NEWER
												#if UNITY_5_5_OR_NEWER
												useSinglePassStereoRenderingMatrix.boolValue = PlayerSettings.stereoRenderingPath == StereoRenderingPath.SinglePass;
												#else
												useSinglePassStereoRenderingMatrix.boolValue = PlayerSettings.singlePassStereoRendering;
												#endif
												GUI.enabled = false;
												EditorGUILayout.PropertyField (useSinglePassStereoRenderingMatrix, new GUIContent ("Single Pass Stereo", "Enables Single Pass Stereo Rendering when using in VR (automatically set based on player settings."));
												GUI.enabled = true;
												#endif

												int effect = effectType.intValue;

												EditorGUILayout.PropertyField (enableDithering, new GUIContent ("Enable Dithering", "Reduces banding artifacts."));
												if (enableDithering.boolValue) {
																EditorGUILayout.PropertyField (ditherStrength, new GUIContent ("   Dither Strength", "Intensity of dither blending."));
												}

												EditorGUILayout.PropertyField (sun, new GUIContent ("Sun", "Assign a game object (a directional light acting as Sun for example) to make the fog color sync automatically with the Sun orientation and light intensity."));
												if (effect == (int)FOG_TYPE.DesktopFogPlusWithSkyHaze) {
																if (sun.objectReferenceValue == null) {
																				EditorGUILayout.HelpBox ("Light scattering requires a Sun reference.", MessageType.Info);
																				GUI.enabled = false;
																}
																EditorGUILayout.PropertyField (scattering, new GUIContent ("Light Scattering", "Amount of Sun light diffusion when it crosses the fog towards viewer."));
																EditorGUILayout.PropertyField (scatteringColor, new GUIContent ("   Scattering Color", "Tint color for the light scattering effect."));
																GUI.enabled = true;
												}

												EditorGUILayout.Separator ();
												expandSection [FOG_PROPERTIES] = EditorGUILayout.Foldout (expandSection [FOG_PROPERTIES], sectionNames [FOG_PROPERTIES], sectionHeaderStyle);

												if (expandSection [FOG_PROPERTIES]) {
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
												}

												if (effect != 2 && effect != 4 && effect != 5 && effect != (int)FOG_TYPE.DesktopFogPlusOrthogonal) {
																EditorGUILayout.Separator ();
																expandSection [SKY_PROPERTIES] = EditorGUILayout.Foldout (expandSection [SKY_PROPERTIES], sectionNames [SKY_PROPERTIES], sectionHeaderStyle);

																if (expandSection [SKY_PROPERTIES]) {
																				EditorGUILayout.PropertyField (skyHaze, new GUIContent ("Haze", "Vertical range for the sky haze."));
																				EditorGUILayout.PropertyField (skySpeed, new GUIContent ("Speed", "Speed of sky haze animation."));
																				EditorGUILayout.PropertyField (skyNoiseStrength, new GUIContent ("Noise Strength", "Amount of noise for the sky haze effect."));
																				EditorGUILayout.PropertyField (skyAlpha, new GUIContent ("Alpha", "Transparency of sky haze."));
																}
												}

												EditorGUILayout.Separator ();
												expandSection [FOG_OF_WAR] = EditorGUILayout.Foldout (expandSection [FOG_OF_WAR], sectionNames [FOG_OF_WAR], sectionHeaderStyle);

												if (expandSection [FOG_OF_WAR]) {
																EditorGUILayout.PropertyField (fogOfWarEnabled, new GUIContent ("Enabled", "Enables fog of war feature. This requires that you assign a fog of war mask texture at runtime. Read documentation or demo scene for details."));
																EditorGUILayout.PropertyField (fogOfWarCenter, new GUIContent ("Center", "World space position of the center of the fog of war mask texture."));
																EditorGUILayout.PropertyField (fogOfWarSize, new GUIContent ("Area Size", "Size of the fog of war area in world space units."));
																EditorGUILayout.PropertyField (fogOfWarTextureSize, new GUIContent ("Texture Size", "Size of the fog of war mask texture."));
												}

												EditorGUILayout.Separator ();

												if (serializedObject.ApplyModifiedProperties () || (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed")) {
																DynamicFog fog = (DynamicFog)target;
																if (prevPreset == preset.intValue && prevFogVolumes == useFogVolumes.boolValue && prevDither == enableDithering.intValue && prevDitherStrength == ditherStrength.floatValue)
																				fog.preset = FOG_PRESET.Custom;
																if (fog.profile != null)
																				profileChanges = true;
																fog.UpdateMaterialProperties ();
												}
								}


								#region Profile handling

								void CreateProfile () {

												DynamicFogProfile newProfile = ScriptableObject.CreateInstance<DynamicFogProfile> ();
												newProfile.Save (_fog);

												AssetDatabase.CreateAsset (newProfile, "Assets/DynamicFogProfile.asset");
												AssetDatabase.SaveAssets ();

												EditorUtility.FocusProjectWindow ();
												Selection.activeObject = newProfile;

												_fog.profile = newProfile;
								}


								#endregion


				}

}
