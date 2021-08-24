using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
public class AddNavTagEditor : Editor
{

    [MenuItem("AddNavTag/AddNavTag")]
    public static void AddNavTag()
    {
        GameObject obj = Selection.activeGameObject;
        foreach (var item in obj.transform.GetComponentsInChildren<Renderer>())
        {
            if (item.GetComponent<NavMeshSourceTag>() == null)
            {
                item.gameObject.AddComponent<NavMeshSourceTag>();
            }
        }


        Transform box_1l = GetTransform(obj.transform, "box_1l");
        if (box_1l != null)
        {
            foreach (var trans in box_1l.transform.GetComponentsInChildren<Renderer>())
            {
                trans.gameObject.tag = "Wall";
                
                AddBoxCollider.DoAddBoxColliderByObj(trans.gameObject);
            }
        }

        Transform box_2l = GetTransform(obj.transform, "box_2l");
        if (box_2l != null)
        {
            foreach (var trans in box_2l.transform.GetComponentsInChildren<Renderer>())
            {
                trans.gameObject.tag = "Wall";
                
                AddBoxCollider.DoAddBoxColliderByObj(trans.gameObject);
            }
        }

        Transform box_3l = GetTransform(obj.transform, "box_3l");
        if (box_3l != null)
        {
            foreach (var trans in box_3l.transform.GetComponentsInChildren<Renderer>())
            {
                trans.gameObject.tag = "Wall";
                
                AddBoxCollider.DoAddBoxColliderByObj(trans.gameObject);
            }
        }

        Transform box_dz = GetTransform(obj.transform, "box_dz");
        if (box_dz != null)
        {
            foreach (var trans in box_dz.transform.GetComponentsInChildren<Renderer>())
            {
                trans.gameObject.tag = "Wall";
                
                AddBoxCollider.DoAddBoxColliderByObj(trans.gameObject);
            }
        }



    }


    public static Transform GetTransform(Transform check, string name)
    {

        foreach (Transform t in check.GetComponentsInChildren<Transform>())
        {
            if (t.name == name)
            {
                return t;
            }
        }
        return null;
    }

}
