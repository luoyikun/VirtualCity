using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickListener : MonoBehaviour, IPointerClickHandler
{
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;
    public VoidDelegate onNewGuideClick;
    public object parameter;
    public object parameter1;

    static public ClickListener Get(GameObject go)
    {
        ClickListener listener = go.GetComponent<ClickListener>();
        if (listener == null) listener = go.AddComponent<ClickListener>();
        return listener;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
        {
            Audio_control.instance.palybut();
            onClick(gameObject);
        }

        if (onNewGuideClick != null && DataMgr.m_isNewGuide == true)
        {
            onNewGuideClick(gameObject);
        }
    }
}