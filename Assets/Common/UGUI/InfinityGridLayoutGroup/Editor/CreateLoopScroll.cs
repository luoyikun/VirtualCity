using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateLoopScroll : Editor {

    [MenuItem("GameObject/UI/LoopScrollVertical")]
    static void CreatLoopScrollVertical()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = PublicFunc.CreateObjFromRes("LoopScrollVertical", Selection.activeTransform);
                go.name = "LoopScrollVertical";
                go.transform.localScale = Vector3.one;
                go.transform.localEulerAngles = Vector3.zero;
                go.transform.localPosition = Vector3.zero;
                Selection.activeTransform = go.transform;
            }
        }
    }

    [MenuItem("GameObject/UI/LoopScrollHorizontal")]
    static void CreatLoopScrollHorizontal()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = PublicFunc.CreateObjFromRes("LoopScrollHorizontal", Selection.activeTransform);
                go.name = "LoopScrollHorizontal";
                go.transform.localScale = Vector3.one;
                go.transform.localEulerAngles = Vector3.zero;
                go.transform.localPosition = Vector3.zero;
                Selection.activeTransform = go.transform;
            }
        }
    }
}
