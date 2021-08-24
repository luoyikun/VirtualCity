using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Attaching this script to an object will cause it to merge all meshes parented to this object.
/// It's useful for cutting down on the number of draw calls.
/// </summary>

[AddComponentMenu("Tasharen/Merge Meshes")]
public class MergeMeshes : MonoBehaviour
{
	public enum PostMerge
	{
		DisableRenderers,
		DestroyRenderers,
		DisableGameObjects,
		DestroyGameObjects,
	}

	/// <summary>
	/// Material that will be used by the merged mesh. Determined automatically if none was specified.
	/// </summary>

	public Material material;

	/// <summary>
	/// What to do after the merge has finished.
	/// </summary>

	public PostMerge afterMerging = PostMerge.DisableRenderers;

	string mName;
	Transform mTrans;
	Mesh mMesh;
	MeshFilter mFilter;
	MeshRenderer mRen;
	List<GameObject> mDisabledGO = new List<GameObject>();
	List<Renderer> mDisabledRen = new List<Renderer>();
	bool mMerge = true;

	/// <summary>
	/// Merge the meshes on start.
	/// </summary>

	void Start ()
	{
		if (mMerge) Merge(true);
		enabled = false;
	}

	/// <summary>
	/// Merge the meshes and disable the script.
	/// </summary>

	void Update ()
	{
		if (mMerge) Merge(true);
		enabled = false;
	}

	/// <summary>
	/// Merge all meshes
	/// </summary>

	public void Merge (bool immediate)
	{
		if (!immediate)
		{
			mMerge = true;
			enabled = true;
			return;
		}

		mMerge = false;
		mName = name;
		mFilter = GetComponent<MeshFilter>();
		mTrans = transform;

		Clear();
		MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();
		if (filters.Length == 0 || (mFilter != null && filters.Length == 1)) return;

		GameObject go = gameObject;
		Matrix4x4 w2l = go.transform.worldToLocalMatrix;

		int vertexCount = 0;
		int indexCount = 0;
		int normCount = 0;
		int tanCount = 0;
		int colCount = 0;
		int uv1Count = 0;
		int uv2Count = 0;

		// First we need to calculate the number of vertices, normals, etc
		foreach (MeshFilter filter in filters)
		{
			if (filter == mFilter) continue;

			if (filter.gameObject.isStatic)
			{
				Debug.LogError("MergeMeshes can't merge objects marked as static", filter.gameObject);
				continue;
			}

			Mesh mesh = filter.sharedMesh;

			// Assume the first material if none was specified
			if (material == null) material = filter.GetComponent<Renderer>().sharedMaterial;

			// Vertex colors and indices must always be present
			vertexCount += mesh.vertexCount;
			indexCount += mesh.triangles.Length;

			// Other components are optional
			if (mesh.normals != null) normCount += mesh.normals.Length;
			if (mesh.tangents != null) tanCount += mesh.tangents.Length;
			if (mesh.colors != null) colCount += mesh.colors.Length;
			if (mesh.uv != null) uv1Count += mesh.uv.Length;
			if (mesh.uv2 != null) uv2Count += mesh.uv2.Length;
		}

		if (vertexCount == 0)
		{
			Debug.LogWarning("Unable to find any non-static objects to merge", this);
			return;
		}

		Vector3[] verts = new Vector3[vertexCount];
		int[] indices = new int[indexCount];

		Vector2[] uv1 = (uv1Count == vertexCount) ? new Vector2[vertexCount] : null;
		Vector2[] uv2 = (uv2Count == vertexCount) ? new Vector2[vertexCount] : null;
		Vector3[] norms = (normCount == vertexCount) ? new Vector3[vertexCount] : null;
		Vector4[] tans = (tanCount == vertexCount) ? new Vector4[vertexCount] : null;
		Color[] cols = (colCount == vertexCount) ? new Color[vertexCount] : null;

		int meshOffset = 0;
		int indexOffset = 0;
		int vertexOffset = 0;

		// Run through each filter again
		foreach (MeshFilter filter in filters)
		{
			if (filter == mFilter || filter.gameObject.isStatic) continue;

			Mesh mesh = filter.sharedMesh;
			if (mesh.vertexCount == 0) continue;

			// Matrix that will transform from relative-to-mesh to world space coordinates
			Matrix4x4 l2w = filter.transform.localToWorldMatrix;

			// Disable this renderer
			Renderer ren = filter.GetComponent<Renderer>();

			// If we are not destroying renderers, add this renderer to the list
			if (afterMerging != PostMerge.DestroyRenderers)
			{
				ren.enabled = false;
				mDisabledRen.Add(ren);
			}
			
			// Disable the game object
			if (afterMerging == PostMerge.DisableGameObjects)
			{
				GameObject root = filter.gameObject;
				Transform trans = root.transform;
				
				// Find the object's root (rigidbody)
				while (trans != mTrans)
				{
					if (trans.GetComponent<Rigidbody>() != null)
					{
						root = trans.gameObject;
						break;
					}
					trans = trans.parent;
				}
				mDisabledGO.Add(root);
				TWTools.SetActive(root, false);
			}

			Vector3[] mv = mesh.vertices;
			Vector3[] mn = (norms != null) ? mesh.normals : null;
			Vector4[] mt = (tans != null) ? mesh.tangents : null;
			Vector2[] u1 = (uv1 != null) ? mesh.uv : null;
			Vector2[] u2 = (uv2 != null) ? mesh.uv2 : null;
			Color[] mc = (cols != null) ? mesh.colors : null;
			int[] mi = mesh.triangles;

			// Copy all vertices, tangents, normals, etc
			for (int i = 0, imax = mv.Length; i < imax; ++i)
			{
				verts[vertexOffset] = w2l.MultiplyPoint3x4(l2w.MultiplyPoint3x4(mv[i]));

				if (norms != null) norms[vertexOffset] = w2l.MultiplyVector(l2w.MultiplyVector(mn[i]));
				if (cols != null) cols[vertexOffset] = mc[i];
				if (uv1 != null) uv1[vertexOffset] = u1[i];
				if (uv2 != null) uv2[vertexOffset] = u2[i];

				if (tans != null)
				{
					Vector4 tan4 = mt[i];
					Vector3 tan3 = new Vector3(tan4.x, tan4.y, tan4.z);
					tan3 = w2l.MultiplyVector(l2w.MultiplyVector(tan3));
					tan4.x = tan3.x;
					tan4.y = tan3.y;
					tan4.z = tan3.z;
					tans[vertexOffset] = tan4;
				}
				++vertexOffset;
			}

			// Copy over the indices
			for (int i = 0, imax = mi.Length; i < imax; ++i) indices[indexOffset++] = meshOffset + mi[i];
			meshOffset = vertexOffset;

			// Destroy the game object if needed
			if (afterMerging == PostMerge.DestroyGameObjects) Destroy(filter.gameObject);
			else if (afterMerging == PostMerge.DestroyRenderers) Destroy(ren);
		}

		// If objects get destroyed, there is no point in keeping the original filters
		if (afterMerging == PostMerge.DestroyGameObjects)
		{
			filters = null;
			mDisabledGO.Clear();
		}

		if (verts.Length > 0)
		{
			// Create a temporary mesh
			if (mMesh == null)
			{
				mMesh = new Mesh();
				mMesh.hideFlags = HideFlags.DontSave;
			}
			else mMesh.Clear();

			mMesh.name = mName;
			mMesh.vertices = verts;
			mMesh.normals = norms;
			mMesh.tangents = tans;
			mMesh.colors = cols;
			mMesh.uv = uv1;
			mMesh.uv2 = uv2;
			mMesh.triangles = indices;
			mMesh.RecalculateBounds();

			// Add a mesh filter
			if (mFilter == null)
			{
				mFilter = go.AddComponent<MeshFilter>();
				mFilter.mesh = mMesh;
			}
			
			// Add a mesh renderer
			if (mRen == null) mRen = go.AddComponent<MeshRenderer>();
			mRen.sharedMaterial = material;
			mRen.enabled = true;

			// Just for clarity, show the number of triangles in the name of the game object
			go.name = mName + " (" + (indices.Length / 3) + " tri)";
		}
		else Release();
		enabled = false;
		//BroadcastMessage("OnMergedMeshes", SendMessageOptions.DontRequireReceiver);
	}

	/// <summary>
	/// Clear the merged meshes, reverting back to the state prior to the Merge() call.
	/// </summary>

	public void Clear ()
	{
		for (int i = 0, imax = mDisabledGO.Count; i < imax; ++i)
		{
			GameObject go = mDisabledGO[i];
			if (go) TWTools.SetActive(go, true);
		}

		for (int i = 0, imax = mDisabledRen.Count; i < imax; ++i)
		{
			Renderer ren = mDisabledRen[i];
			if (ren) ren.enabled = true;
		}

		mDisabledGO.Clear();
		mDisabledRen.Clear();

		if (mRen != null) mRen.enabled = false;
	}

	/// <summary>
	/// Clear the merged meshes and release all memory.
	/// </summary>

	public void Release ()
	{
		Clear();

		TWTools.Destroy(mRen);
		TWTools.Destroy(mFilter);
		TWTools.Destroy(mMesh);

		mFilter = null;
		mMesh = null;
		mRen = null;
	}
}