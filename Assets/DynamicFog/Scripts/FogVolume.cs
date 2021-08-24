using UnityEngine;
using System.Collections;

namespace DynamicFogAndMist {
				public class FogVolume : MonoBehaviour {

								const float GRAY = 227f / 255f;


								[Tooltip ("Enables transition to a given profile.")]
								public bool enableProfileTransition;

								[Tooltip ("Assign the transition profile.")]
								public DynamicFogProfile targetProfile;


								[Tooltip ("Enables alpha transition.")]
								public bool enableAlphaTransition;

								[Tooltip ("Target alpha for fog when camera enters this fog volume")]
								[Range (0, 1)]
								public float targetFogAlpha = 0.5f;
								[Tooltip ("Target alpha for sky haze when camera enters this fog volume")]
								[Range (0, 1)]
								public float targetSkyHazeAlpha = 0.5f;


								[Tooltip ("Enables fog color transition.")]
								public bool enableFogColorTransition;

								[Tooltip ("Target fog color 1 when gamera enters this fog folume")]
								public Color targetFogColor1 = new Color (GRAY, GRAY, GRAY);

								[Tooltip ("Target fog color 2 when gamera enters this fog folume")]
								public Color targetFogColor2 = new Color (GRAY, GRAY, GRAY);

								[Tooltip ("Set this to zero for changing fog alpha immediately upon enter/exit fog volume.")]
								public float transitionDuration = 3.0f;


								[Tooltip ("Set collider that will trigger this fog volume. If not set, this fog volume will react to any collider which has the main camera. If you use a third person controller, assign the character collider here.")]
								public Collider targetCollider;

								[Tooltip ("When enabled, a console message will be printed whenever this fog volume is entered or exited.")]
								public bool debugMode;

								[Tooltip ("Assign target Dynamic Fog component that will be affected by this volume.")]
								public DynamicFog targetFog;


								bool cameraInside;

								void Start () {
												if (targetFog == null)
																targetFog = DynamicFog.instance;
												if (targetFog != null)
																targetFog.useFogVolumes = true;
								}

								void OnTriggerEnter (Collider other) {
												if (cameraInside || targetFog == null)
																return;
												// Check if other collider has the main camera attached
												if (other == targetCollider || other.gameObject.transform.GetComponentInChildren<Camera> () == targetFog.fogCamera) {
																cameraInside = true;
																if (enableProfileTransition && targetProfile != null) {
																				targetFog.SetTargetProfile (targetProfile, transitionDuration);
																} 
																if (enableAlphaTransition) {
																				targetFog.SetTargetAlpha (targetFogAlpha, targetSkyHazeAlpha, transitionDuration);
																}
																if (enableFogColorTransition) {
																				targetFog.SetTargetColors (targetFogColor1, targetFogColor2, transitionDuration);
																}
																if (debugMode) {
																				Debug.Log ("Fog Volume entered by " + other.name);
																}
												}
								}

								void OnTriggerExit (Collider other) {
												if (!cameraInside || targetFog == null)
																return;
												if (other == targetCollider || other.gameObject.transform.GetComponentInChildren<Camera> () == targetFog.fogCamera) {
																cameraInside = false;
																if (enableProfileTransition && targetProfile != null) {
																				targetFog.ClearTargetProfile (transitionDuration);
																} 
																if (enableAlphaTransition) {
																				targetFog.ClearTargetAlpha (transitionDuration);
																}
																if (enableFogColorTransition) {
																				targetFog.ClearTargetColors (transitionDuration);
																}
																if (debugMode) {
																				Debug.Log ("Fog Volume exited by " + other.name);
																}
												}
								}

				}

}