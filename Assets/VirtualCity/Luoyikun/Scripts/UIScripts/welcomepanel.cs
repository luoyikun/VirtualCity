using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class welcomepanel : UGUIPanel {
    public GameObject m_btnZhuanQu;
	// Use this for initialization
	void Start () {
        ClickListener.Get(m_btnZhuanQu).onClick = OnBtnZhuanQu;

    }

    public override void OnOpen()
    {
        
    }

    public override void OnClose()
    {
           
    }

    public void OnBtnZhuanQu(GameObject obj)
    {
        UIManager.Instance.PopSelf(false);
        NewGuideMgr.Instance.StartOneNewGuide();
    }
}
