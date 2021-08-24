using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

[System.Serializable]
public class kefuInfoJson
{
    public string phone;
    public string wx;
    public string qq;
}

public class kefupanel : UGUIPanel {
    public GameObject m_btnPhone;
    public GameObject m_btnWx;
    public GameObject m_btnQQ;
    public GameObject m_btnClose;

    public Text m_textPhone;
    public Text m_textWx;
    public Text m_textQQ;
    public static kefuInfoJson m_info = new kefuInfoJson();
    // Use this for initialization
    void Start () {
        ClickListener.Get(m_btnPhone).onClick = OnBtnPhone;
        ClickListener.Get(m_btnWx).onClick = OnBtnWx;
        ClickListener.Get(m_btnClose).onClick = OnBtnClose;
        ClickListener.Get(m_btnQQ).onClick = OnBtnQQ;

        if (m_info.phone != null && m_info.phone.Length != 0)
        {
            m_textPhone.text = m_info.phone;
            m_textWx.text = m_info.wx;
            m_textQQ.text = m_info.qq;
        }
    }
	

    void OnBtnClose(GameObject obj)
    {
        UIManager.Instance.PopSelf(false);
    }
    void OnBtnPhone(GameObject obj)
    {
        AndroidFunc.CallPhone(m_textPhone.text);
    }

    void OnBtnWx(GameObject obj)
    {
        AndroidFunc.CopyToB(m_textWx.text);
        Hint.LoadTips("已复制微信号", Color.white);
    }

    void OnBtnQQ(GameObject obj)
    {
        AndroidFunc.CopyToB(m_textWx.text);
        Hint.LoadTips("已复制QQ号", Color.white);
    }
}
