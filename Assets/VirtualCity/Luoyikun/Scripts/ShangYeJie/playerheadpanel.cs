using Framework.Tools;
using Framework.UI;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerheadpanel : UGUIPanel {

    public static playerheadpanel m_instance;
    private  BufferPool m_pool;
    public GameObject m_tmpHead;
    Transform m_trans;
    Dictionary<long, GameObject> m_dicHead = new Dictionary<long, GameObject>();
    private void Start()
    {
        m_instance = this;
        m_trans = this.transform;
    }
    public override void OnOpen()
    {
        foreach (var item in m_dicHead)
        {
            m_pool.Recycle(item.Value);
        }
        m_dicHead.Clear();
    }

    public override void OnClose()
    {
        
    }

    public void CreateOneHeadUi(long id,string name,Transform followTrans)
    {
        if (m_pool == null)
        {
            m_pool = new BufferPool(m_tmpHead, m_trans, 1);
        }

        GameObject tempObj = m_pool.GetObject();
        WorldUiFollowObj uiFollow = tempObj.GetComponent<WorldUiFollowObj>();
        if (DataMgr.m_curScene == EnCurScene.Business)
        {
            uiFollow.m_camera = SYJMgr.m_instance.m_myCamera;
        }
        uiFollow.m_followTrans = followTrans;
        uiFollow.GetComponent<Text>().text = name;
        m_dicHead[id] = tempObj;
    }

    public void Recycle(long id)
    {
        if (m_pool == null)
        {
            m_pool = new BufferPool(m_tmpHead, m_trans, 1);
        }
        if (m_dicHead.ContainsKey(id))
        {
            m_pool.Recycle(m_dicHead[id]);
            m_dicHead.Remove(id);
        }
    }
}
