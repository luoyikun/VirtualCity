using Framework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFollowObj : MonoBehaviour {

    Transform m_trans;
    public Transform m_followTrans;
    public Camera m_camera;
	// Use this for initialization
	void Start () {
        m_trans = this.transform;
        
    }

    private void OnEnable()
    {
        EventManager.Instance.AddEventListener(Common.EventStr.HomeUnitCamFollow, OnEvHomeUnitCamFollow);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveEventListener(Common.EventStr.HomeUnitCamFollow, OnEvHomeUnitCamFollow);
    }

    void OnEvHomeUnitCamFollow(EventData data)
    {
        var exData = data as EventDataEx<Transform>;
        m_followTrans = exData.GetData();
    }

    private void LateUpdate()
    {
        if (m_followTrans != null && m_camera != null)
        {
            Vector2 player2DPosition = m_camera.WorldToScreenPoint(m_followTrans.position);
            m_trans.position = player2DPosition;
        }
    }

}
