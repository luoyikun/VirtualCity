using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class RepeatButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

    public bool invokeOnce = false;//是否只调用一次
    private bool hadInvoke = false;//是否已经调用过

    public float interval = 0.1f;//按下后超过这个时间则认定为"长按"
    private bool isPointerDown = false;
    private float recordTime;

    public UnityEvent onPress = new UnityEvent();//按住时调用
    public UnityEvent onRelease = new UnityEvent();//松开时调用

    public static RepeatButton Get(Transform trans)
    {
        if (trans.GetComponent<RepeatButton>() == null)
        {
            trans.gameObject.AddComponent<RepeatButton>();
        }
        return trans.GetComponent<RepeatButton>();
    }
    void Update()
    {
        if (invokeOnce && hadInvoke) return;
        if (isPointerDown)
        {
            if ((Time.time - recordTime) > interval)
            {
                onPress.Invoke();
                hadInvoke = true;
                recordTime = Time.time;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        recordTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        hadInvoke = false;
        onRelease.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerDown = false;
        hadInvoke = false;
        onRelease.Invoke();
    }
}
