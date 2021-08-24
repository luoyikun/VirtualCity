using UnityEngine;

public static class TWTools
{
	/// <summary>
	/// Unity4 has changed GameObject.active to GameObject.SetActive.
	/// </summary>

	static public void SetActiveSelf(GameObject go, bool state)
	{
#if UNITY_3_5
		go.active = state;
#else
		go.SetActive(state);
#endif
	}

	/// <summary>
	/// Activate the specified object and all of its children.
	/// </summary>

	static void Activate(Transform t)
	{
		SetActiveSelf(t.gameObject, true);

		// Prior to Unity 4, active state was not nested. It was possible to have an enabled child of a disabled object.
		// Unity 4 onwards made it so that the state is nested, and a disabled parent results in a disabled child.
#if UNITY_3_5
		for (int i = 0, imax = t.GetChildCount(); i < imax; ++i)
		{
			Transform child = t.GetChild(i);
			Activate(child);
		}
#else
		// If there is even a single enabled child, then we're using a Unity 4.0-based nested active state scheme.
		for (int i = 0, imax = t.childCount; i < imax; ++i)
		{
			Transform child = t.GetChild(i);
			if (child.gameObject.activeSelf) return;
		}

		// If this point is reached, then all the children are disabled, so we must be using a Unity 3.5-based active state scheme.
		for (int i = 0, imax = t.childCount; i < imax; ++i)
		{
			Transform child = t.GetChild(i);
			Activate(child);
		}
#endif
	}

	/// <summary>
	/// Deactivate the specified object and all of its children.
	/// </summary>

	static void Deactivate(Transform t)
	{
#if UNITY_3_5
		for (int i = 0, imax = t.GetChildCount(); i < imax; ++i)
		{
			Transform child = t.GetChild(i);
			Deactivate(child);
		}
#endif
		SetActiveSelf(t.gameObject, false);
	}

	/// <summary>
	/// SetActiveRecursively enables children before parents. This is a problem when a widget gets re-enabled
	/// and it tries to find a panel on its parent.
	/// </summary>

	static public void SetActive(GameObject go, bool state)
	{
		if (state)
		{
			Activate(go.transform);
		}
		else
		{
			Deactivate(go.transform);
		}
	}

	/// <summary>
	/// Add a new child game object.
	/// </summary>

	static public GameObject AddChild (GameObject parent)
	{
		GameObject go = new GameObject();

		if (parent != null)
		{
			Transform t = go.transform;
			t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			go.layer = parent.layer;
		}
		return go;
	}

	/// <summary>
	/// Instantiate an object and add it to the specified parent.
	/// </summary>

	static public GameObject AddChild (GameObject parent, GameObject prefab)
	{
		GameObject go = GameObject.Instantiate(prefab) as GameObject;

		if (go != null && parent != null)
		{
			Transform t = go.transform;
			t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			go.layer = parent.layer;
		}
		return go;
	}

	/// <summary>
	/// Destroy the specified object, immediately if in edit mode.
	/// </summary>

	static public void Destroy (UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (Application.isPlaying) UnityEngine.Object.Destroy(obj);
			else UnityEngine.Object.DestroyImmediate(obj);
		}
	}

	/// <summary>
	/// Destroy the specified object immediately, unless not in the editor, in which case the regular Destroy is used instead.
	/// </summary>

	static public void DestroyImmediate (UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (Application.isEditor) UnityEngine.Object.DestroyImmediate(obj);
			else UnityEngine.Object.Destroy(obj);
		}
	}
}
