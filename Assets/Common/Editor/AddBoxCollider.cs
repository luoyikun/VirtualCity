using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class AddBoxCollider : EditorWindow
{

    [MenuItem("AddBoxCollider/AddBoxCollider")]
    public static void DoAddBoxCollider()
    {
        GameObject obj = Selection.activeGameObject;
        if (obj.GetComponent<Collider>() == null)
        {
            Transform parent = Selection.activeGameObject.transform;
            Vector3 postion = parent.position;
            Quaternion rotation = parent.rotation;
            Vector3 scale = parent.localScale;
            parent.position = Vector3.zero;
            parent.rotation = Quaternion.Euler(Vector3.zero);
            parent.localScale = Vector3.one;

            Collider[] colliders = parent.GetComponentsInChildren<Collider>();
            foreach (Collider child in colliders)
            {
                DestroyImmediate(child);
            }
            Vector3 center = Vector3.zero;
            Renderer[] renders = parent.GetComponentsInChildren<Renderer>();
            foreach (Renderer child in renders)
            {
                center += child.bounds.center;
            }
            center /= parent.GetComponentsInChildren<Renderer>().Length;
            Bounds bounds = new Bounds(center, Vector3.zero);
            foreach (Renderer child in renders)
            {
                bounds.Encapsulate(child.bounds);
            }
            BoxCollider boxCollider = parent.gameObject.AddComponent<BoxCollider>();
            boxCollider.center = bounds.center - parent.position;
            boxCollider.size = bounds.size;

            parent.position = postion;
            parent.rotation = rotation;
            parent.localScale = scale;
        }
    }

    [MenuItem("AddBoxCollider/RemoveCollider")]
    static void RemoveCollider()
    {
        GameObject obj = Selection.activeGameObject;
        foreach (var it in obj.transform.GetComponentsInChildren<Transform>())
        {
            Collider[] bufColl = it.transform.GetComponents<Collider>();
            for (int i = 0; i < bufColl.Length; i++)
            {
                DestroyImmediate(bufColl[i]);
            }
        }
    }

    public static void DoAddBoxColliderByObj(GameObject obj)
    {
        if (obj.GetComponent<Collider>() == null)
        {
            Transform parent = obj.transform;
            Vector3 postion = parent.position;
            Quaternion rotation = parent.rotation;
            Vector3 scale = parent.localScale;
            parent.position = Vector3.zero;
            parent.rotation = Quaternion.Euler(Vector3.zero);
            parent.localScale = Vector3.one;

            Collider[] colliders = parent.GetComponentsInChildren<Collider>();
            foreach (Collider child in colliders)
            {
                DestroyImmediate(child,true);
            }
            Vector3 center = Vector3.zero;
            Renderer[] renders = parent.GetComponentsInChildren<Renderer>();
            foreach (Renderer child in renders)
            {
                center += child.bounds.center;
            }
            center /= parent.GetComponentsInChildren<Renderer>().Length;
            Bounds bounds = new Bounds(center, Vector3.zero);
            foreach (Renderer child in renders)
            {
                bounds.Encapsulate(child.bounds);
            }
            BoxCollider boxCollider = parent.gameObject.AddComponent<BoxCollider>();
            boxCollider.center = bounds.center - parent.position;
            boxCollider.size = bounds.size;
            boxCollider.isTrigger = true;
            parent.position = postion;
            parent.rotation = rotation;
            parent.localScale = scale;
        }
    }
}
