using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class NewArrowsCtrl : MonoBehaviour {

    public Transform m_img;
    Transform m_trans;
    bool m_isPause = false;
    void Start()
    {
        m_trans = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isPause == false)
        {
            m_trans.position = new Vector3(m_trans.position.x, m_trans.position.y  + Mathf.Sin(Time.time * 2) * PublicFunc.GetHeightFactor(), m_trans.position.z);
        }
    }

    public void SetDir(int dir = -1)
    {
        m_isPause = true;
        Vector3 scale = m_img.localScale;
        scale.y = dir;
        m_img.localScale = scale;
        m_isPause = false;
    }
}
