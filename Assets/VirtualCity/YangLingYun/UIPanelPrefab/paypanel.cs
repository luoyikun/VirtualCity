using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using UnityEngine.Events;
using UnityEngine.UI;

public class paypanel : UGUIPanel {
    //public GameObject m_btnOk;
    public GameObject m_GoldPayBtn;
    public GameObject m_DiamondPayBtn;
    public GameObject m_btnCancel;
    public GameObject IconPar;
    //public UnityAction m_ok;
    public UnityAction m_GoldPay;
    public UnityAction m_DiamondPay;
    public UnityAction m_cancel;

    public Text m_textTitle;
    public Text m_textContent;
    // Use this for initialization
    void Start () {
        //ClickListener.Get(m_btnOk).onClick = OnBtnOk;
        ClickListener.Get(m_GoldPayBtn).onClick = clickGoldPayBtn;
        ClickListener.Get(m_DiamondPayBtn).onClick = clickDiamondPayBtn;
        ClickListener.Get(m_btnCancel).onClick = OnBtnCancel;
    }
    public void SetContent(string Titel, string Content, int Gold, int Diamond)
    {
        m_textTitle.text = Titel;
        m_textContent.text = Content;
        m_GoldPayBtn.gameObject.SetActive(false);
        m_DiamondPayBtn.gameObject.SetActive(false);
        if (Gold != 0)
        {
            m_GoldPayBtn.SetActive(true);
            m_GoldPayBtn.transform.Find("Text").GetComponent<Text>().text =  Gold + "金币";
            //IconPar.transform.Find("Gold").gameObject.SetActive(true);
            //IconPar.transform.Find("Gold").Find("Text").GetComponent<Text>().text = Gold.ToString();
        }
        if (Diamond != 0)
        {
            m_DiamondPayBtn.SetActive(true);
            m_DiamondPayBtn.transform.Find("Text").GetComponent<Text>().text = Diamond + "钻石";
            //IconPar.transform.Find("Diamond").gameObject.SetActive(true);
            //IconPar.transform.Find("Diamond").Find("Text").GetComponent<Text>().text = Diamond.ToString();
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
    //void OnBtnOk(GameObject obj)
    //{
    //    //m_isClickOk = true;
    //    UIManager.Instance.PopSelf();
    //    if (m_ok != null)
    //    {
    //        m_ok();
    //    }

    //}
    void clickGoldPayBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf(false);
        if (m_GoldPay != null)
        {
            m_GoldPay();
        }
    }
    void clickDiamondPayBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf(false);
        if (m_DiamondPay != null)
        {
            m_DiamondPay();
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
