using UnityEngine;
using UnityEditor;

namespace Lean.Pool
{
	[CustomEditor(typeof(LeanGameObjectPool))]
	public class LeanGameObjectPool_Editor : Editor
	{
		[MenuItem("GameObject/Lean/Pool", false, 1)]
		public static void CreateLocalization()
		{
			var gameObject = new GameObject(typeof(LeanPool).Name);

			Undo.RegisterCreatedObjectUndo(gameObject, "Create Pool");

			gameObject.AddComponent<LeanGameObjectPool>();

			Selection.activeGameObject = gameObject;
		}

		// Draw the whole inspector
		public override void OnInspectorGUI()
		{
			var pool = (LeanGameObjectPool)target;

			EditorGUI.BeginChangeCheck();
			{
				EditorGUILayout.Separator();

				EditorGUILayout.PropertyField(serializedObject.FindProperty("Prefab"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("Notification"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("Preload"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("Capacity"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("Recycle"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("Persist"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("Stamp"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("Warnings"));

				EditorGUILayout.Separator();

				EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.IntField("Spawned", pool.Spawned);
					EditorGUILayout.IntField("Despawned", pool.Despawned);
					EditorGUILayout.IntField("Total", pool.Total);
				EditorGUI.EndDisabledGroup();
			}
			if (EditorGUI.EndChangeCheck() == true)
			{
				EditorUtility.SetDirty(target);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}