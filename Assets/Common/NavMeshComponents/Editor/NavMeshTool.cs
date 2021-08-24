using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTool : Editor {

    [MenuItem("NavMeshTool/AddNotWalk")]
    public static void AddNotWalk()
    {
        GameObject obj = Selection.activeGameObject;
        foreach (var trans in obj.transform.GetComponentsInChildren<Renderer>())
        {
            if (trans.gameObject.GetComponent<NavMeshModifier>() == null)
            {
                trans.gameObject.AddComponent<NavMeshModifier>();
            }

            NavMeshModifier nav = trans.gameObject.GetComponent<NavMeshModifier>();
            nav.overrideArea = true;
            nav.area = 1;
        }
    }

    [MenuItem("NavMeshTool/RemoveNotWalk")]
    public static void RemoveNotWalk()
    {
        GameObject obj = Selection.activeGameObject;
        foreach (var trans in obj.transform.GetComponentsInChildren<Renderer>())
        {
            if (trans.gameObject.GetComponent<NavMeshModifier>() == null)
            {
                DestroyImmediate(trans.gameObject.GetComponent<NavMeshModifier>());
            }
        }
    }
}
