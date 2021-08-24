using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ispanel : UGUIPanel {

    public GameObject m_btnOk;
    public GameObject m_btnCancel;
    public UnityAction m_ok;
    public UnityAction m_cancel;

    public Text m_textTitle;
    public Text m_textContent;

    public bool m_isClickOk = false;
	// Use this for initialization
	void Start () {
        gameObject.name = m_type;
        ClickListener.Get(m_btnOk).onClick = OnBtnOk;
        ClickListener.Get(m_btnCancel).onClick = OnBtnCancel;
	}

    public void SetContent(string title, string Content,bool isTwoBtn = true)
    {
        m_textTitle.text = title;
        m_textContent.text = Content;
        if (isTwoBtn == true)
        {
            m_btnCancel.SetActive(true);
        }
        else {
            m_btnCancel.SetActive(false);
        }
    }
    void OnBtnOk(GameObject obj)
    {
        m_isClickOk = true;
        UIManager.Instance.PopSelf(false);
        if (m_ok != null)
        {
            m_ok();
        }
        
    }

    void OnBtnCancel(GameObject obj)
    {
        UIManager.Instance.PopSelf(false);
        if (m_cancel != null)
        {
            m_cancel();
        }
        
    }
}
