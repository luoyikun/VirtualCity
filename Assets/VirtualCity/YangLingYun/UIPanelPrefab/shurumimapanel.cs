using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using UnityEngine.UI;
using ProtoDefine;
using SGF.Codec;
using Newtonsoft.Json;
using SuperScrollView;
using System;

public class shurumimapanel : UGUIPanel
{
    public int IsAliPay = -1;
    public GameObject PasswordWindows;
    public GameObject TiXianWindows;
    public GameObject QueRenWindows;
    public GameObject JiLuWindows;
    public GameObject content;
    public InputField PasswordInput;
    public InputField TiXianInput;
    Text TiXianJinEText;
    Text TiXianAccountText;
    public double TiXianJinE = 0;
    public long TiXianZhangHao;
    public PayAccount m_PayAccount;
    string Target_Password;
    public LoopListView2 ScrollView;
    bool IsScrollViewInit = false;
    int TotalCount = 0;
    List<Order> m_ListOrder = new List<Order>();
    public override void OnOpen()
    {
        // openPanelView(1);
        //TiXianJinE = 0;
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, OnNetRspCM);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspQueryOrderMessage, OnNetRspQOM);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCashMoneyMessage, OnNetRspCMM);
        // NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetWalletDateMessage, OnNetRspGWDM);
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, OnNetRspCM);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspQueryOrderMessage, OnNetRspQOM);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCashMoneyMessage, OnNetRspCMM);
        // NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetWalletDateMessage, OnNetRspGWDM);
    }

    void OnNetRspCMM(byte[] buf)
    {
        RspCashMoneyMessage Rsp = PBSerializer.NDeserialize<RspCashMoneyMessage>(buf);
        if (Rsp.code == 0)
        {
            if (Rsp.tips == null)
            {
                openPanelView(0);
                return;
            }
            //UIManager.Instance.PopSelf();
            UIManager.Instance.PushPanel(UIPanelName.tishengedupanel, false, true, paragrm => { paragrm.GetComponent<tishengedupanel>().Init(Rsp.tips); });
            //Hint.LoadTips(Rsp.tips, Color.white);
            PublicFunc.SendSkipNewGuide();
            return;
        }
        UIManager.Instance.PopSelf();
        NewGuideMgr.Instance.StartOneNewGuide();
        Hint.LoadTips(Rsp.tips, Color.white);
        UpdateWalletDate();
    }
    void OnNetRspQOM(byte[] buf)
    {
        RspQueryOrderMessage RspQOM = PBSerializer.NDeserialize<RspQueryOrderMessage>(buf);
        if (RspQOM.code != 0)
        {
            if (RspQOM.orders == null)
            {
                Hint.LoadTips("目前没有奖励记录", Color.white);
                return;
            }
            m_ListOrder = RspQOM.orders;
            //Debug.Log(m_ListOrder);
            TotalCount = RspQOM.orders.Count;
            if (IsScrollViewInit == false)
            {
                ScrollView.InitListView(TotalCount, OnGetItemByIndex);
                IsScrollViewInit = true;
            }
            else if (IsScrollViewInit == true)
            {
                ScrollView.SetListItemCount(TotalCount);
                ScrollView.RefreshAllShownItem();
            }
            openPanelView(3);
        }
        else if (RspQOM.code == 0)
        {
            Hint.LoadTips(RspQOM.tip, Color.white);
        }
    }
    void OnNetRspCM(byte[] buf)
    {
        RspCommentMessage RspCM = PBSerializer.NDeserialize<RspCommentMessage>(buf);
        if (RspCM.rspcmd == 511)
        {
            Hint.LoadTips(RspCM.tip, Color.white);
            return;
        }
        //if (RspCM.rspcmd == 519)
        //{
        //    if (RspCM.code != 0)
        //    {
        //        UIManager.Instance.PopSelf();
        //        Hint.LoadTips("提现申请成功", Color.white);
        //        UpdateWalletDate();
        //    }
        //    else if (RspCM.code == 0)
        //    {
        //        Hint.LoadTips(RspCM.tip, Color.white);
        //    }
        //}
    }
    void UpdateWalletDate()
    {
        ReqGetWalletDateMessage m_ReqGWDM = new ReqGetWalletDateMessage();
        m_ReqGWDM.accountId = (long)DataMgr.m_account.id;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetWalletDateMessage, m_ReqGWDM);
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf(true);
    }
    void clickQueRenBtn(GameObject obj)
    {
        //if (PasswordInput.text != DataMgr.m_account.password)
        //{
        //    Hint.LoadTips("密码错误，请确认", Color.white);
        //}
        //else if (PasswordInput.text == DataMgr.m_account.password)
        //{
        //    Target_Password = PasswordInput.text;
        //    openPanelView(1);
        //}
        if (PasswordInput.text == "")
        {
            Hint.LoadTips("请输入密码");
            return;
        }
        Target_Password = PasswordInput.text;

        ReqCashMoneyMessage m_ReqCMM = new ReqCashMoneyMessage();
        m_ReqCMM.cashMoney = (float)TiXianJinE;
        m_ReqCMM.payType = IsAliPay;
        if (IsAliPay == 1)
        {
            Hint.LoadTips("暂未支持奖励到微信", Color.white);
            return;
        }
        m_ReqCMM.payAccount = JsonConvert.SerializeObject(m_PayAccount);
        m_ReqCMM.password = Target_Password;
        string m_ReqCMMString = JsonConvert.SerializeObject(m_ReqCMM);

        ReqCashMoneyMessage Target_ReqCMM = new ReqCashMoneyMessage();

        Target_ReqCMM.content = RSAEncryption.RsaEncrypt_(m_ReqCMMString, RSAEncryption.Get_privatekey());

        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqCashMoneyMessage, Target_ReqCMM);
        //Debug.Log(RSAEncryption.RSAPrivateKeyDotNet2Java(RSAEncryption.Get_privatekey()));
        //UIManager.Instance.PopSelf(false);

    }



    void clickRetrieveBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.kefupanel, false, false, (param) => { }, true);
    }

    void clickQueRenTiXianBtn(GameObject obj)
    {
        for (int i = 0; i < DataMgr.businessModelProperties.Count; i++)
        {
            if (DataMgr.businessModelProperties[i].Name == "fast_cash_limit")
            {
                JieXi m_JieXi = new JieXi();
                m_JieXi = JsonConvert.DeserializeObject<JieXi>(DataMgr.businessModelProperties[i].Con);
                if (TiXianJinE > int.Parse(m_JieXi.v))
                {
                    openPanelView(0);
                    return;
                }
            }
        }

        if (TiXianJinE < 0.09f)
        {
            Hint.LoadTips("低于最低提现金额0.1元");
            return;
        }
        ReqCashMoneyMessage m_ReqCMM = new ReqCashMoneyMessage();
        m_ReqCMM.cashMoney = (float)TiXianJinE;
        m_ReqCMM.payType = IsAliPay;
        m_ReqCMM.payAccount = JsonConvert.SerializeObject(m_PayAccount);
        m_ReqCMM.password = null;
        string m_ReqCMMString = JsonConvert.SerializeObject(m_ReqCMM);

        ReqCashMoneyMessage Target_ReqCMM = new ReqCashMoneyMessage();

        Target_ReqCMM.content = RSAEncryption.RsaEncrypt_(m_ReqCMMString, RSAEncryption.Get_privatekey());

        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqCashMoneyMessage, Target_ReqCMM);
        //ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, true);
        //ispanel.SetContent("警告", "请确认奖励账号和姓名，如果账号有误将无法追回，你确定奖励到该账号么？");
        //ispanel.m_ok = () =>
        //{
        //    for (int i = 0; i < DataMgr.businessModelProperties.Count; i++)
        //    {
        //        if (DataMgr.businessModelProperties[i].Name == "fast_cash_limit")
        //        {
        //            JieXi m_JieXi = new JieXi();
        //            m_JieXi = JsonConvert.DeserializeObject<JieXi>(DataMgr.businessModelProperties[i].Con);
        //            if (TiXianJinE > int.Parse(m_JieXi.v))
        //            {
        //                openPanelView(0);
        //                return;
        //            }
        //        }
        //    }

        //    ReqCashMoneyMessage m_ReqCMM = new ReqCashMoneyMessage();
        //    m_ReqCMM.cashMoney = (float)TiXianJinE;
        //    m_ReqCMM.payType = IsAliPay;
        //    m_ReqCMM.payAccount = JsonConvert.SerializeObject(m_PayAccount);
        //    m_ReqCMM.password = null;
        //    string m_ReqCMMString = JsonConvert.SerializeObject(m_ReqCMM);

        //    ReqCashMoneyMessage Target_ReqCMM = new ReqCashMoneyMessage();

        //    Target_ReqCMM.content = RSAEncryption.RsaEncrypt_(m_ReqCMMString, RSAEncryption.Get_privatekey());

        //    HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqCashMoneyMessage, Target_ReqCMM);
        //};
    }
    void clickTiXianNextBtn(GameObject obj)
    {
        //UIManager.Instance.PopSelf(true);
        if (TiXianInput.text == "")
        {
            Hint.LoadTips("提现金额不能为空", Color.white);
            return;
        }
        if (double.Parse(TiXianInput.text) <= 0)
        {
            Hint.LoadTips("提现金额必须为大于零", Color.white);
            return;
        }
        if (double.Parse(TiXianInput.text) - (double)DataMgr.m_account.wallet.moneyNum < 0.01f || (double)DataMgr.m_account.wallet.moneyNum - double.Parse(TiXianInput.text) < 0.01f)
        {
            if (DataMgr.m_isNewGuide == false)
            {
                string url = AppConst.m_shareUrl + DataMgr.m_account.commendCode;
                AndroidFunc.WxShareWebpageCross(url);
            }
            TiXianJinE = double.Parse(TiXianInput.text);

            openPanelView(2);
        }
        else
        {
            Hint.LoadTips("提现金额不能大于剩余金额", Color.white);
        }
    }
    void clickAllBtn(GameObject obj)
    {
        TiXianInput.text = ((double)DataMgr.m_account.wallet.moneyNum).ToString("0.00");
        TiXianJinE = double.Parse(((double)DataMgr.m_account.wallet.moneyNum).ToString("0.00"));
    }
    void clickTiXianJiLuBtn(GameObject obj)
    {
        ReqQueryOrderMessage ReqQOM = new ReqQueryOrderMessage();
        ReqQOM.status = 10;
        ReqQOM.createTime = DataMgr.m_account.createtime;
        ReqQOM.lastDate = null;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqQueryOrderMessage, ReqQOM);
    }
    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= TotalCount)
        {
            return null;
        }
        LoopListViewItem2 item = listView.NewListViewItem("TiXianJiLu");
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            //itemScript.Init();
        }
        InitTiXianJiLu(index, item.transform);
        //item.transform.GetComponent<MonthBillMgr>().Ini(Old_RQBM[index].bills.Count, Old_RQBM[index].bills);
        return item;
    }
    void InitTiXianJiLu(int Index, Transform tran)
    {
        if (m_ListOrder[Index].payType == 0)
        {
            tran.Find("ALiPayIcon").gameObject.SetActive(true);
        }
        else if (m_ListOrder[Index].payType == 1)
        {
            tran.Find("WxPayIcon").gameObject.SetActive(true);
        }
        PayAccount m_PayAccount = JsonConvert.DeserializeObject<PayAccount>(m_ListOrder[Index].remark);
        tran.Find("AccountText").GetComponent<Text>().text = m_PayAccount.account;
        if (m_ListOrder[Index].orderStatus == "10")
        {
            tran.Find("Status").GetComponent<Text>().text = "奖励审核中";
        }
        else if (m_ListOrder[Index].orderStatus == "11")
        {
            if (m_ListOrder[Index].gmApproveResault == 0)
            {
                tran.Find("Status").GetComponent<Text>().text = "奖励审核未通过";
                tran.Find("Status").GetComponent<Text>().color = PublicFunc.StringToColor("456c92");
            }
            else if (m_ListOrder[Index].gmApproveResault == 1)
            {
                tran.Find("Status").GetComponent<Text>().text = "待打款";
                tran.Find("Status").GetComponent<Text>().color = PublicFunc.StringToColor("E6860A");
            }
        }
        tran.Find("CashNumber").GetComponent<Text>().text = "奖励金额" + m_ListOrder[Index].payNum + "元";
        tran.Find("Time").GetComponent<Text>().text = m_ListOrder[Index].createtime;
    }
    void clickJiLuBackBtn(GameObject obj)
    {
        openPanelView(1);
    }
    public void openPanelView(int idx)
    {
        //foreach (Transform trans in transform.GetComponentsInChildren<Transform>())
        //{
        //    if (trans.name == "BackBtn")
        //    {
        //        ClickListener.Get(trans.gameObject).onClick = clickBackBtn;
        //    }
        //}
        for (int i = 0; i < content.transform.childCount; i++)
        {
            content.transform.GetChild(i).gameObject.SetActive(false);
        }
        content.transform.GetChild(idx).gameObject.SetActive(true);
        switch (idx)
        {
            case 0:
                ClickListener.Get(PasswordWindows.transform.Find("BackBtn").gameObject).onClick = clickBackBtn;
                PasswordInput = PasswordWindows.transform.Find("InputField").GetComponent<InputField>();
                PasswordInput.text = "";
                ClickListener.Get(PasswordWindows.transform.Find("DownPar").Find("QueRenBtn").gameObject).onClick = clickQueRenBtn;
                ClickListener.Get(PasswordWindows.transform.Find("DownPar").Find("RetrieveBtn").gameObject).onClick = clickRetrieveBtn;
                break;
            case 1:
                if (IsAliPay == 0)
                {
                    TiXianWindows.transform.Find("Main").Find("AccountText").Find("AccountImage").GetChild(0).gameObject.SetActive(true);
                    TiXianWindows.transform.Find("Main").Find("AccountText").Find("AccountImage").GetChild(1).gameObject.SetActive(false);
                }
                else if (IsAliPay == 1)
                {
                    TiXianWindows.transform.Find("Main").Find("AccountText").Find("AccountImage").GetChild(0).gameObject.SetActive(false);
                    TiXianWindows.transform.Find("Main").Find("AccountText").Find("AccountImage").GetChild(1).gameObject.SetActive(true);
                }
                //     TiXianWindows.transform.Find("Main").Find("TiXianMoney").Find("TiXianEDuText").GetComponent<Text>().text=
                TiXianJinE = 0;
                TiXianWindows.transform.Find("Main").Find("AccountText").Find("Text").GetComponent<Text>().text =
                    m_PayAccount.account.ToString();
                TiXianWindows.transform.Find("Main").Find("TiXianMoney").Find("Text").GetComponent<Text>().text = ((double)DataMgr.m_account.wallet.moneyNum).ToString("0.00");
                TiXianInput = TiXianWindows.transform.Find("Main").Find("InputText").Find("InputField").GetComponent<InputField>();
                TiXianInput.text = "";
                TiXianInput.onEndEdit.AddListener(delegate { EndInput(TiXianInput); });
                ClickListener.Get(TiXianWindows.transform.Find("Main").Find("TiXianJiLuBtn").gameObject).onClick = clickTiXianJiLuBtn;
                ClickListener.Get(TiXianWindows.transform.Find("BackBtn").gameObject).onClick = clickBackBtn;
                ClickListener.Get(TiXianWindows.transform.Find("Main").Find("InputText").Find("AllBtn").gameObject).onClick = clickAllBtn;
                ClickListener.Get(TiXianWindows.transform.Find("Main").Find("NextBtn").gameObject).onClick = clickTiXianNextBtn;
                //TiXianWindows.transform.Find("Main").Find("")
                break;
            case 2:
                TiXianJinEText = QueRenWindows.transform.Find("Main").Find("Money").Find("Text").GetComponent<Text>();
                TiXianJinEText.text = TiXianJinE + "元";
                TiXianAccountText = QueRenWindows.transform.Find("Main").Find("Account").Find("Text").GetComponent<Text>();
                TiXianAccountText.text = m_PayAccount.account;
                ClickListener.Get(QueRenWindows.transform.Find("BackBtn").gameObject).onClick = clickBackBtn;
                ClickListener.Get(QueRenWindows.transform.Find("Main").Find("TiXianBtn").gameObject).onClick = clickQueRenTiXianBtn;
                break;
            case 3:
                ClickListener.Get(JiLuWindows.transform.Find("BG").Find("BackBtn").gameObject).onClick = clickJiLuBackBtn;
                break;
        }
    }

    void EndInput(InputField IF)
    {
        double MoneyNum = double.Parse(((double)DataMgr.m_account.wallet.moneyNum).ToString("0.00"));
        if (IF.text == "" || double.Parse(IF.text) < 0)
        {
            IF.text = "0";
        }
        if (double.Parse(TiXianInput.text) > MoneyNum)
        {
            IF.text = MoneyNum.ToString();
        }
    }
    private void NewMethod()
    {
        TiXianWindows.transform.Find("Main").Find("AccountText").Find("Text").GetComponent<Text>().text = m_PayAccount.account;
    }
}
