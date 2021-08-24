using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class homeunitplacepanel : UGUIPanel {

    public GameObject m_btnShun;
    public GameObject m_btnNi;
    public GameObject m_btnHuiShou;
    public GameObject m_btnOk;
    public GameObject m_btnCanel;
    public static homeunitplacepanel m_instance;
    bool m_isPlaceOk = false;
    // Use this for initialization
    void Start () {
        m_instance = this;
        ClickListener.Get(m_btnShun).onClick = OnBtnShun;
        ClickListener.Get(m_btnNi).onClick = OnBtnNi;
        ClickListener.Get(m_btnHuiShou).onClick = OnBtnHuiShou;
        ClickListener.Get(m_btnOk).onClick = OnBtnOk;
        ClickListener.Get(m_btnCanel).onClick = OnBtnCanel;

        RepeatButton.Get(m_btnShun.transform).onPress.AddListener(() => { OnBtnRotate(true); });
        RepeatButton.Get(m_btnNi.transform).onPress.AddListener(() => { OnBtnRotate(false); });
    }


    void OnBtnRotate(bool isShun)
    {
        HomeMgr.m_instance.SelectUnitRotate(isShun);
    }
    void OnBtnShun(GameObject obj)
    {
        HomeMgr.m_instance.SelectUnitRotateByClick(true); 
    }

    void OnBtnNi(GameObject obj)
    {
        HomeMgr.m_instance.SelectUnitRotateByClick(false);
    }

    void OnBtnHuiShou(GameObject obj)
    {
        HomeMgr.m_instance.SendRecycle();
    }

    void OnBtnOk(GameObject obj)
    {
        if (m_isPlaceOk == true)
        {
            HomeMgr.m_instance.SelectUnitPlaceOk(true);
        }
    }

    void OnBtnCanel(GameObject obj)
    {
        HomeMgr.m_instance.SelectUnitCancel();
    }

    public void SetOkBtn(bool isActive)
    {
        m_isPlaceOk = isActive;
        if (m_isPlaceOk == false)
        {
            m_btnOk.GetComponent<Image>().color = Color.gray;
            m_btnOk.transform.Find("Image").GetComponent<Image>().color = Color.gray;
        }
        else {
            m_btnOk.GetComponent<Image>().color = Color.white;
            m_btnOk.transform.Find("Image").GetComponent<Image>().color = Color.white;
        }
    }
}
