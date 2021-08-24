#if GAIA_PRESENT && UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using DynamicFogAndMist;
using UnityEditor;

namespace Gaia.GX.Kronnect
{
    /// <summary>
    /// Volumetric Fog & Mist extension for Gaia.
    /// </summary>
	public class DynamicFogGaiaExtension : MonoBehaviour
    {
        #region Generic informational methods

        /// <summary>
        /// Returns the publisher name if provided. 
        /// This will override the publisher name in the namespace ie Gaia.GX.PublisherName
        /// </summary>
        /// <returns>Publisher name</returns>
        public static string GetPublisherName()
        {
            return "Kronnect";
        }

        /// <summary>
        /// Returns the package name if provided
        /// This will override the package name in the class name ie public class PackageName.
        /// </summary>
        /// <returns>Package name</returns>
        public static string GetPackageName()
        {
            return "Dynamic Fog & Mist (mobile & VR compatible)";
        }

        #endregion

        #region Methods exposed by Gaia as buttons must be prefixed with GX_

        public static void GX_About()
        {
			EditorUtility.DisplayDialog("About Dynamic Fog & Mist", "Dynamic Fog & Mist is a full-screen image effect that adds live, moving Fog, Mist and Sky Haze to your scenes making them less dull and boring.", "OK");
        }

		static DynamicFog CheckFog() {
			DynamicFog fog = DynamicFog.instance;
			// Checks Dynamic Fog image effect is attached
			if (fog==null) {
				Camera camera = Camera.main;
				if (camera == null)
				{
					camera = FindObjectOfType<Camera>();
				}
				if (camera == null)
				{
					Debug.LogError("Could not find camera to add camera effects to. Please add a camera to your scene.");
					return null;
				}
				fog = camera.gameObject.AddComponent<DynamicFog>();
			}

			// set nice default params
			fog.height = 20;
			fog.distanceFallOff = 0.4f;

			// Finds a suitable Sun light and set color1 to it
			Light[] lights = FindObjectsOfType<Light>();
			Light sun = null;
			for (int k=0;k<lights.Length;k++) {
				Light light = lights[k];
				if (light.name.Equals("Sun")) {
					sun = light;
					break;
				}
			}
			if (sun==null) {
				for (int k=0;k<lights.Length;k++) {
					Light light = lights[k];
					if (light.type == LightType.Directional) {
							sun = light;
							break;
					}
				}
			}
			if (sun!=null) {
				fog.color = sun.color;
			}
			// Set fog base-line height to water level
			GaiaSceneInfo sceneInfo = GaiaSceneInfo.GetSceneInfo();
			fog.baselineHeight = sceneInfo.m_seaLevel;

			return fog;
		}

		public static void GX_Mist() {
			DynamicFog fog = CheckFog();
			if (fog==null) return;
			fog.preset = FOG_PRESET.Mist;
			fog.CheckPreset();
			fog.preset = FOG_PRESET.Custom; // enable to make modifications and preserve them
			PostMessage();
		}

		public static void GX_WindyMist() {
			DynamicFog fog = CheckFog();
			if (fog==null) return;
			fog.preset = FOG_PRESET.WindyMist;
			fog.CheckPreset();
			fog.preset = FOG_PRESET.Custom; // enable to make modifications and preserve them
			PostMessage();
		}

		public static void GX_Fog() {
			DynamicFog fog = CheckFog();
			if (fog==null) return;
			fog.preset = FOG_PRESET.Fog;
			fog.CheckPreset();
			fog.preset = FOG_PRESET.Custom; // enable to make modifications and preserve them
			PostMessage();
		}

		public static void GX_HeavyFog() {
			DynamicFog fog = CheckFog();
			if (fog==null) return;
			fog.preset = FOG_PRESET.HeavyFog;
			fog.CheckPreset();
			fog.preset = FOG_PRESET.Custom; // enable to make modifications and preserve them
			PostMessage();
		}
		
		public static void GX_GroundFog() {
			DynamicFog fog = CheckFog();
			if (fog==null) return;
			fog.preset = FOG_PRESET.GroundFog;
			fog.CheckPreset();
			fog.preset = FOG_PRESET.Custom; // enable to make modifications and preserve them
			PostMessage();
		}

		public static void GX_SandStorm() {
			DynamicFog fog = CheckFog();
			if (fog==null) return;
			fog.preset = FOG_PRESET.SandStorm;
			fog.CheckPreset();
			fog.preset = FOG_PRESET.Custom; // enable to make modifications and preserve them
			PostMessage();
		}

		static void PostMessage() {
			Debug.Log ("Dynamic Fog & Mist added to main Camera and configured to chosen preset. Review the script attached to the Camera and customize its settings to your preferences (fog height, distance, distance fall-off, ...).");
		}

		#endregion
     
    }
}

#endif