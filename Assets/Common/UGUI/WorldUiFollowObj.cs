using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUiFollowObj : MonoBehaviour {

    Transform m_trans;
    public Transform m_followTrans;
    public Camera m_camera;
    public Vector3 m_offset; 
    // Use this for initialization
    void Start()
    {
        m_trans = this.transform;

    }

    private void LateUpdate()
    {
        if (m_followTrans != null && m_camera != null)
        {
            //Vector2 player2DPosition = m_camera.WorldToScreenPoint(m_followTrans.position);
            m_trans.position = m_followTrans.position + m_offset;

            Vector3 vDir = m_camera.transform.position - transform.position;
            vDir.Normalize();
            m_trans.rotation = Quaternion.LookRotation(-vDir);
        }
    }
}
