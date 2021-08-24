using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KEventDelegate : MonoBehaviour
{
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;

    static public bool IsHave(GameObject go)
    {
        bool isHave = false;
        if (go.GetComponent<KEventDelegate>() != null)
        {
            isHave = true;
        }
        return isHave;
    }

    static public KEventDelegate Get(GameObject go)
    {
        KEventDelegate listener = go.GetComponent<KEventDelegate>();
        if (listener == null) listener = go.AddComponent<KEventDelegate>();

       
        return listener;
    }

    public void OnClick(GameObject go)
    {
        if (onClick != null)
            onClick(go);
    }

}
