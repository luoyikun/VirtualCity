using UnityEngine;
using System.Collections;

namespace DynamicFogAndMist {

				[ExecuteInEditMode]
				public class DynamicFogOfWar : MonoBehaviour {

								public int fogOfWarTextureSize = 512;


								Material fogMat;
								static DynamicFogOfWar _instance;
								Texture2D fogOfWarTexture;
								Color32[] fogOfWarColorBuffer;

								public static DynamicFogOfWar instance {
												get {
																if (_instance == null) {
																				_instance = FindObjectOfType<DynamicFogOfWar> ();
																}
																return _instance;
												}
								}

								void OnEnable () {
												fogMat = GetComponent<MeshRenderer> ().sharedMaterial;
												UpdateFogOfWarTexture ();
								}

								void OnDisable () {
												if (fogOfWarTexture != null) {
																DestroyImmediate (fogOfWarTexture);
																fogOfWarTexture = null;
												}
								}

								void Update () {
												fogMat.SetVector ("_FogOfWarData", new Vector4 (transform.position.x, transform.position.z, transform.localScale.x, transform.localScale.y));
								}


								void UpdateFogOfWarTexture () {
												int size = GetScaledSize (fogOfWarTextureSize, 1.0f);
												fogOfWarTexture = new Texture2D (size, size, TextureFormat.ARGB32, false);
												fogOfWarTexture.hideFlags = HideFlags.DontSave;
												fogOfWarTexture.filterMode = FilterMode.Bilinear;
												fogOfWarTexture.wrapMode = TextureWrapMode.Clamp;
												fogMat.mainTexture = fogOfWarTexture;
												ResetFogOfWar ();
								}

								int GetScaledSize (int size, float factor) {
												size = (int)(size / factor);
												size /= 4;
												if (size < 1)
																size = 1;
												return size * 4;
								}


								#region Fog of War Public API

								/// <summary>
								/// Changes the alpha value of the fog of war at world position. It takes into account FogOfWarCenter and FogOfWarSize.
								/// Note that only x and z coordinates are used. Y (vertical) coordinate is ignored.
								/// </summary>
								/// <param name="worldPosition">in world space coordinates.</param>
								/// <param name="radius">radius of application in world units.</param>
								public void SetFogOfWarAlpha (Vector3 worldPosition, float radius, float fogNewAlpha) {
												if (fogOfWarTexture == null)
																return;

												float tx = (worldPosition.x - transform.position.x) / transform.localScale.x + 0.5f;
												if (tx < 0 || tx > 1f)
																return;
												float tz = (worldPosition.z - transform.position.z) / transform.localScale.y + 0.5f;
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
																float tr = radius / transform.localScale.y;
																int delta = Mathf.FloorToInt (th * tr);
																for (int r = pz - delta; r <= pz + delta; r++) {
																				if (r > 0 && r < th - 1) {
																								for (int c = px - delta; c <= px + delta; c++) {
																												if (c > 0 && c < tw - 1) {
																																int distance = (int) (Mathf.Sqrt ((pz - r) * (pz - r) + (px - c) * (px - c)));
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

								public void SetFogOfWarTerrainBoundary (Terrain terrain, float borderWidth) {
												TerrainData td = terrain.terrainData;
												int tw = td.heightmapWidth;
												int th = td.heightmapHeight;
												float ta = td.size.y;
												float[,] heights = td.GetHeights (0, 0, tw, th);
												float y0 = transform.position.y - 1f;
												float y1 = transform.position.y + 10f;
												Vector3 halfSize = new Vector3(-td.size.x * 0.5f, 0, -td.size.z * 0.5f);
												for (int j = 0; j < th; j++) {
																for (int k = 0; k < tw; k++) {
																				float h = heights [j,k] * ta + terrain.transform.position.y;
																				if (h>y0 && h<y1) {
																								Vector3 wpos = transform.position + halfSize + new Vector3( td.size.x * (k+0.5f) / tw, 0, td.size.z * (j+0.5f) / th);
																								SetFogOfWarAlpha(wpos, borderWidth, 0);
																				}
																}
												}
								}


								#endregion


				}

}