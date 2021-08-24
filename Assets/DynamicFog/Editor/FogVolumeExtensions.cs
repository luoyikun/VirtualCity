using UnityEngine;
using UnityEditor;
using System.Collections;

namespace DynamicFogAndMist {
	public class FogVolumeExtensions : MonoBehaviour {

		[MenuItem("GameObject/Create Other/Dynamic Fog Volume")]
		static void CreateFogVolume (MenuCommand menuCommand) {
			GameObject fogVolume = Resources.Load<GameObject> ("Prefabs/FogVolume");
			if (fogVolume == null) {
				Debug.LogError ("Could not load FogVolume from Resources/Prefabs folder!");
				return;
			}
			GameObject newFogVolume = Instantiate (fogVolume);
			newFogVolume.name = "Dynamic Fog Volume";

			// Ensure it gets reparented if this was a context click (otherwise does nothing)
			GameObjectUtility.SetParentAndAlign (newFogVolume, menuCommand.context as GameObject);

			// Register root object for undo.
			Undo.RegisterCreatedObjectUndo (newFogVolume, "Create Dynamic Fog Volume");
			Selection.activeObject = newFogVolume;

			// Enables fog volumes in fog component
			DynamicFog fog = Camera.main.GetComponent<DynamicFog> ();
			if (fog != null)
				fog.useFogVolumes = true;
		}
	}

}
