#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace AQUAS
{
	public class AQUAS_Screenshotter : MonoBehaviour {

		[Header("This script is only for use in the Editor!")]
		[Header("Use F12 to Capture Screenshot")]
		new public GameObject camera;
		public int sizeX = 1920;
		public int sizeY = 1080;
		public string folder = "Screenshots";
		new public string name = "Screenshot";

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

			if(Input.GetKeyDown(KeyCode.F12))
			{
				RenderTexture rt = new RenderTexture(sizeX, sizeY, 24);

				camera.GetComponent<Camera>().targetTexture = rt;

				Texture2D screenShot = new Texture2D(sizeX, sizeY, TextureFormat.RGB24, false);

				camera.GetComponent<Camera>().Render();
				RenderTexture.active = rt;
				screenShot.ReadPixels(new Rect(0, 0, sizeX, sizeY), 0, 0);
				camera.GetComponent<Camera>().targetTexture = null;
				RenderTexture.active = null;
				DestroyImmediate(rt);

				byte[] bytes = screenShot.EncodeToJPG();
				string filename = string.Format("{0}/{1}/{2}_{3}.jpg", Application.dataPath, folder, name, System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

				if (!System.IO.Directory.Exists(string.Format("{0}/{1}", Application.dataPath, folder)))
				{
					System.IO.Directory.CreateDirectory(string.Format("{0}/{1}", Application.dataPath, folder));
				}

				System.IO.File.WriteAllBytes(filename, bytes);
				AssetDatabase.Refresh();
				Debug.Log("Screenshot taken!");
			}

		}
	}
}
#endif
