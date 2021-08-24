using UnityEngine;

[AddComponentMenu("Common/Full Screen Option")]
public class FullScreenOption : MonoBehaviour
{
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.F5))
		{
			if (Screen.fullScreen)
			{
				Screen.SetResolution(1280, 720, false);
			}
			else
			{
				Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
			}
		}
	}
}