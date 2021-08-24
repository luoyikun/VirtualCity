using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using UnityEngine.UI;
using Newtonsoft.Json;
using ProtoDefine;

public class accountsecuritypanel :  UGUIPanel{
    public GameObject Main;
    public GameObject backBtn;
    public static accountsecuritypanel asp;
    GameObject MainShow;
    public string telephone;
    public List<PayAccount> m_LPA = new List<PayAccount>();
    // Use this for initialization
    void Start () {
        ClickListener.Get(backBtn).onClick = clickBackBtn;
        
        
	}
    public override void OnOpen()
    {
        Init(DataMgr.m_account.userPayAccount);
    }
    public override void OnClose()
    {

    }
    public void Init(string m_ListAccount)
    {
        asp = this;
        if (m_ListAccount != "[]")
        {
            m_LPA = JsonConvert.DeserializeObject<List<PayAccount>>(m_ListAccount);
        }
        else
        {
            m_LPA = new List<PayAccount>();
        }
        LeftMuneMgr.LFM.clickLeftMenuBtn(LeftMuneMgr.LFM.LeftMenuPar.transform.GetChild(0).gameObject);
    }
    public void clickAccountBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.editaccountpanel,false,true,(param)=> {
            editaccountpanel edit = param.GetComponent<editaccountpanel>();
            PayAccount Target_PayAccount = new PayAccount();
            Target_PayAccount.account = obj.transform.Find("TelephoneText").GetComponent<Text>().text;
            Target_PayAccount.realName = obj.transform.Find("NameText").GetComponent<Text>().text;
            edit.m_PayAccount = Target_PayAccount;
            edit.m_ListPayAccount = m_LPA;
            edit.Stateinit(true);
            if (obj.transform.parent.gameObject == accountTest.AT.ZFBAccountPar)
            {
                edit.IsALiPay = true;
            }
            else if (obj.transform.parent.gameObject == accountTest.AT.WXAccountPar)
            {
                edit.IsALiPay = false;
            }
        });
        //editaccountpanel.eap.AccountString = obj.transform.Find("TelephoneText").GetComponent<Text>().text;
        //editaccountpanel.eap.Stateinit(true);
    }
    public void clickAddAcountBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.editaccountpanel, false,true,(param)=> {
            editaccountpanel eap = param.GetComponent<editaccountpanel>();
            eap.Target_Object = obj;
            eap.m_ListPayAccount = m_LPA;
            eap.Stateinit(false);
            if (obj.transform.parent.gameObject == accountTest.AT.ZFBAccountPar)
            {
                eap.IsALiPay = true;
            }
            else if (obj.transform.parent.gameObject == accountTest.AT.WXAccountPar)
            {
                eap.IsALiPay = false;
            }
        });
        //editaccountpanel.eap.Stateinit(false);
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
	void Update () {
		
	}
}
