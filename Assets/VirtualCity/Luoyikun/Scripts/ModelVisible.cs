using Framework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelVisible : MonoBehaviour {
    public string m_id ;
    public Camera m_cam;
    //void OnBecameVisible()
    //{
    //    if (m_id != -1)
    //    {
    //        //EventManager.Instance.DispatchEvent(Common.EventStr.ModelVisible, new EventDataEx<bool>(true));
    //        hudpanel.m_instance.SetVisible(m_id, true);
    //    }
    //}

    //void  OnBecameInvisible()
    //{
    //    if (m_id != -1)
    //    {
    //        //EventManager.Instance.DispatchEvent(Common.EventStr.ModelVisible, new EventDataEx<bool>(false));
    //        hudpanel.m_instance.SetVisible(m_id, false);
    //    }
    //}

    public bool IsInView(Vector3 worldPos)
    {
        Transform camTransform = m_cam.transform;
        Vector2 viewPos = m_cam.WorldToViewportPoint(worldPos);
        Vector3 dir = (worldPos - camTransform.position).normalized;
        float dot = Vector3.Dot(camTransform.forward, dir);     //判断物体是否在相机前面  

        //if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        //    return true;
        //else
        //    return false;

        if (dot > 0 )
            return true;
        else
            return false;
    }
    void Update()
    {
        if (m_cam == null)
        {
            return;
        }
        if (IsInView(transform.position))
        {
            //Debug.Log("目前本物体在摄像机范围内");
            hudpanel.m_instance.SetVisible(m_id, true);
        }
        else
        {
            //Debug.Log("目前本物体不在摄像机范围内");
            hudpanel.m_instance.SetVisible(m_id, false);
        }
    }

}
