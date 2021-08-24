using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace DynamicFogAndMist {
				[CustomEditor (typeof(DynamicFogManager))]
				public class DynamicFogManagerEditor : Editor {

								static GUIStyle titleLabelStyle, sectionHeaderStyle;
								static Color titleColor;
								static bool[] expandSection = new bool[5];
								const string SECTION_PREFS = "DynamicFogExpandSection";
								static string[] sectionNames = new string[] {
												"Fog Properties",
								};
								const int FOG_PROPERTIES = 0;
								SerializedProperty sun;
								SerializedProperty alpha, distance, distanceFallOff, height, heightFallOff, baselineHeight, color;

								void OnEnable () {
												titleColor = EditorGUIUtility.isProSkin ? new Color (0.52f, 0.66f, 0.9f) : new Color (0.12f, 0.16f, 0.4f);
												for (int k = 0; k < expandSection.Length; k++) {
																expandSection [k] = EditorPrefs.GetBool (SECTION_PREFS + k, false);
												}
												sun = serializedObject.FindProperty ("sun");

												alpha = serializedObject.FindProperty ("alpha");
												distance = serializedObject.FindProperty ("distance");
												distanceFallOff = serializedObject.FindProperty ("distanceFallOff");
												height = serializedObject.FindProperty ("height");
												heightFallOff = serializedObject.FindProperty ("heightFallOff");
												baselineHeight = serializedObject.FindProperty ("baselineHeight");

												color = serializedObject.FindProperty ("color");
												sun = serializedObject.FindProperty ("sun");

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

												EditorGUILayout.PropertyField (sun, new GUIContent ("Sun", "Assign a game object (a directional light acting as Sun for example) to make the fog color sync automatically with the Sun orientation and light intensity."));


												EditorGUILayout.Separator ();
												expandSection [FOG_PROPERTIES] = EditorGUILayout.Foldout (expandSection [FOG_PROPERTIES], sectionNames [FOG_PROPERTIES], sectionHeaderStyle);

												if (expandSection [FOG_PROPERTIES]) {
																EditorGUILayout.PropertyField (alpha, new GUIContent ("Alpha", "Global fog transparency. You can also change the transparency at color level."));
																EditorGUILayout.PropertyField (distance, new GUIContent ("Distance", "The starting distance of the fog measure in linear 0-1 values (0=camera near clip, 1=camera far clip)."));
																EditorGUILayout.PropertyField (distanceFallOff, new GUIContent ("Distance Fall Off", "Makes the fog appear smoothly on the near distance."));
																EditorGUILayout.PropertyField (height, new GUIContent ("Height", "Height of the fog in meters."));
																EditorGUILayout.PropertyField (heightFallOff, new GUIContent ("Height Fall Off", "Increase to make the fog change gradually its density based on height."));
																EditorGUILayout.PropertyField (baselineHeight, new GUIContent ("Baseline Height", "Vertical position of the fog in meters. Height is counted above this baseline height."));
																EditorGUILayout.PropertyField (color);
												}
												EditorGUILayout.Separator ();

												if (serializedObject.ApplyModifiedProperties () || (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed")) {
																DynamicFogManager fog = (DynamicFogManager)target;
																fog.UpdateMaterialProperties ();
												}
								}

				}

}
