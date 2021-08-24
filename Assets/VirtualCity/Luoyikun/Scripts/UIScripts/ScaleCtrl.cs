using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCtrl : MonoBehaviour {
    bool m_isPause = false;
    Transform m_trans;
    // Use this for initialization
    void Start () {
        m_trans = this.transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_isPause == false)
        {
            float scale = 0.1f*Mathf.Sin(Time.time * 2);
            m_trans.localScale = new Vector3(1 + scale, 1 + scale, 1 + scale);
            //m_trans.position = new Vector3(m_trans.position.x, m_trans.position.y + Mathf.Sin(Time.time * 2), m_trans.position.z);
        }
    }
}
