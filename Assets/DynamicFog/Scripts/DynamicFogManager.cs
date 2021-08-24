using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DynamicFogAndMist
{
	[ExecuteInEditMode]
	[HelpURL ("http://kronnect.com/taptapgo")]
	public class DynamicFogManager : MonoBehaviour
	{

		[Range (0, 1)]
		public float
			alpha = 1f;
		[Range (0, 1)]
		public float
			noiseStrength = 0.5f;
		[Range (0, 0.999f)]
		public float
			distance = 0.2f;
		[Range (0, 2f)]
		public float
			distanceFallOff = 1f;
		[Range (0, 500)]
		public float
			height = 1f;
		[Range (0, 1)]
		public float
			heightFallOff = 1f;
		public float
			baselineHeight = 0;
		public Color color = new Color (0.89f, 0.89f, 0.89f, 1);
		public GameObject sun;
		Light sunLight;
		Vector3 sunDirection = Vector3.zero;
		Color sunColor = Color.white;
		float sunIntensity = 1f;


		// Creates a private material used to the effect
		void OnEnable ()
		{
			UpdateMaterialProperties ();
		}

		void Reset ()
		{
			UpdateMaterialProperties ();
		}


		// Check possible alpha transition
		void Update ()
		{
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

			UpdateFogData();
		}

		public void UpdateMaterialProperties ()
		{
			UpdateFogData();
			UpdateFogColor ();
		}

		void UpdateFogData() {
			Vector4 data = new Vector4 (height + 0.001f, baselineHeight, Camera.main.farClipPlane * distance, heightFallOff);
			Shader.SetGlobalVector ("_FogData", data);
			Shader.SetGlobalFloat ("_FogData2", distanceFallOff * data.z + 0.0001f);
		}

		void UpdateFogColor ()
		{

			if (sun != null) {
				if (sunLight == null)
					sunLight = sun.GetComponent<Light> ();
				if (sunLight != null && sunLight.transform != sun.transform) {
					sunLight = sun.GetComponent<Light> ();
				}
				sunDirection = sun.transform.forward;
				if (sunLight != null) {
					sunColor = sunLight.color;
					sunIntensity = sunLight.intensity;
				}
			}
			
			float fogIntensity = sunIntensity * Mathf.Clamp01 (1.0f - sunDirection.y);
			Color fogColor = color * sunColor * fogIntensity;
			fogColor.a = alpha;
			Shader.SetGlobalColor ("_FogColor", fogColor);
		}


	}


}