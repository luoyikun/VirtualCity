using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using Newtonsoft.Json;
using ProtoDefine;
using SGF.Codec;

public class tixianpanel : UGUIPanel
{
    public static tixianpanel tp;
    public GameObject BackBtn;
    public List<PayAccount> Target_ListPayAccount;
    PayAccount Target_PayAccount = new PayAccount();
    // Use this for initialization
    void Start()
    {

    }
    public override void OnOpen()
    {
        tp = this;
        if (DataMgr.m_account.userPayAccount == null)
        {
            Debug.Log("提现账号Json为null");
            return;
        }
        if (DataMgr.m_account.userPayAccount != "[]")
        {
            Target_ListPayAccount = JsonConvert.DeserializeObject<List<PayAccount>>(DataMgr.m_account.userPayAccount);
        }
        else
        {
            Target_ListPayAccount = new List<PayAccount>();
        }
        LeftMuneMgr.LFM.clickLeftMenuBtn(LeftMuneMgr.LFM.LeftMenuPar.transform.GetChild(0).gameObject);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetWalletDateMessage, OnNetRspGWDM);
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetWalletDateMessage, OnNetRspGWDM);
    }
    void OnNetRspGWDM(byte[] buf)
    {
        RspGetWalletDateMessage RspGWDM = PBSerializer.NDeserialize<RspGetWalletDateMessage>(buf);
        DataMgr.m_account.wallet = RspGWDM.wallet;
        //TextInit();
    }
    private void Awake()
    {
        tp = this;
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
    }
    public void clickAccountEditBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.editaccountpanel, false, true, (param) =>
        {
            editaccountpanel eap = param.GetComponent<editaccountpanel>();
            Target_PayAccount.account = obj.transform.parent.Find("TelephoneText").GetComponent<Text>().text;
            Target_PayAccount.realName = obj.transform.parent.Find("NameText").GetComponent<Text>().text;
            eap.m_ListPayAccount = Target_ListPayAccount;
            eap.m_PayAccount = Target_PayAccount;
            if (obj.transform.parent.parent.parent.parent.parent.name == "ZFB")
            {
                eap.IsALiPay = true;
            }
            else
            {
                eap.IsALiPay = false;
            }
            eap.Stateinit(true);
        });

    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
    public void clickAccountBtn(GameObject obj)
    {
        //Debug.Log(obj.name);
        //if (DataMgr.m_account.hadProxy==null || DataMgr.m_account.hadProxy == 0)
        //{
        //    UIManager.Instance.PushPanel(UIPanelName.dailiquanpanel, false, true, paragrm => { paragrm.GetComponent<dailiquanpanel>().OpenPanelWindows(0); });
        //}
        UIManager.Instance.PushPanel(UIPanelName.shurumimapanel, false, true, paragrm =>
        {
            Target_PayAccount.account = obj.transform.Find("TelephoneText").GetComponent<Text>().text;
            Target_PayAccount.realName = obj.transform.Find("NameText").GetComponent<Text>().text;
            paragrm.GetComponent<shurumimapanel>().m_PayAccount = Target_PayAccount;
            paragrm.GetComponent<shurumimapanel>().openPanelView(1);
            paragrm.GetComponent<shurumimapanel>().IsAliPay = Target_PayAccount.payType;
        });
    }
    public void clickAddAcountBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.editaccountpanel, false, true, (param) =>
        {
            editaccountpanel eap = param.GetComponent<editaccountpanel>();
            eap.m_ListPayAccount = Target_ListPayAccount;
            eap.Stateinit(false);
            if (obj.transform.parent.parent.parent.parent.name == "ZFB")
            {
                eap.IsALiPay = true;
            }
            else
            {
                eap.IsALiPay = false;
            }
        });
    }
    // Update is called once per frame
    void Update()
    {

    }
}
