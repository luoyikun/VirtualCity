using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using System;
using ProtoDefine;
using SGF.Codec;
using Newtonsoft.Json;

public class PersonnalInfo : UGUIPanel
{
    public Text CharitableScoreText;
    public Text GoldCoinsText;
    public Text DiamondsText;
    public Text CashCouponText;
    public Text CashText;
    public Text AssetText;
    public Text EduText;
    public GameObject MenuPar;
    public GameObject TiXianBtn;
    public GameObject BackBtn;
    public GameObject CiShanBtn;
    public GameObject webBut;
    public GameObject audioBut;
    public Image HeadImage;
    int XiShu = 0;
    public GameObject m_btnTuiJian;
    public GameObject m_btnTiaoKuan;
    public GameObject m_btnQieHuanZhangHao;
    public Text commendCodeText;
    public GameObject GouWuBangZhuBtn;

    public GameObject DaiLiBangZhuBtn;
    //public Text DaiLiWeiText;
    // Use this for initialization
    void Start()
    {
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        ClickListener.Get(TiXianBtn).onClick = clickTiXianBtn;
        ClickListener.Get(CiShanBtn).onClick = clickCiShanBtn;
        ClickListener.Get(m_btnTuiJian).onClick = OnBtnTuiJian;
        ClickListener.Get(m_btnTiaoKuan).onClick = OnBtnTiaoKuan;
        ClickListener.Get(m_btnQieHuanZhangHao).onClick = OnBtnQieHuanZhangHao;

        ClickListener.Get(DaiLiBangZhuBtn).onClick = clickDaiLiBangZhuBtn;
        ClickListener.Get(GouWuBangZhuBtn).onClick = clickGouWuBangZhuBtn;
        ClickListener.Get(webBut).onClick = clickweb;
        ClickListener.Get(audioBut).onClick = clickaudio;

    }

    void clickDaiLiBangZhuBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.noticepanel, false, false, (param) =>
            {
                NoticePanel notice = param.GetComponent<NoticePanel>();
                notice.ShowCommonNotice(1);
                //NewGuideMgr.Instance.StartOneNewGuide();
            }, true
        );
    }

    void OnBtnQieHuanZhangHao(GameObject obj)
    {
        VirtualCityMgr.SwitchAccount();
    }

    void OnBtnTiaoKuan(GameObject obj)
    {
        UIManager.Instance.PushPanel(Vc.AbName.noticepanel, false, false, (param) =>
        {
            NoticePanel notice = param.GetComponent<NoticePanel>();
            notice.ShowTermsOfService();
        });
    }

    void OnBtnTuiJian(GameObject obj)
    {
        //if (DataMgr.m_account.hadProxy == 0)
        //{
        //    Hint.LoadTips("请先获取代理权", Color.white);
        //}
        //else
        //{
            string url = AppConst.m_shareUrl + DataMgr.m_account.commendCode;
            AndroidFunc.WxShareWebpageCross(url);
        //}
    }
    void UpdateWalletDate()
    {
        ReqGetWalletDateMessage m_ReqGWDM = new ReqGetWalletDateMessage();
        m_ReqGWDM.accountId = (long)DataMgr.m_account.id;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetWalletDateMessage, m_ReqGWDM);
    }
    void TextInit()
    {
        string headimagepath = PublicFunc.GetUserHeadImg(DataMgr.m_account);
        AssetMgr.Instance.CreateSpr(headimagepath, "charactericon", (spr) => { HeadImage.sprite = spr; });
        for (int i = 0; i < DataMgr.businessModelProperties.Count; i++)
        {
            if (DataMgr.businessModelProperties[i].Name == "diamond2money")
            {
                JieXi m_JieXi = new JieXi();
                m_JieXi = JsonConvert.DeserializeObject<JieXi>(DataMgr.businessModelProperties[i].Con);
                XiShu = int.Parse(m_JieXi.v);
                break;
            }
        }
        CharitableScoreText.text = "<color=#969696>您有慈善基金：</color>" + DataMgr.m_account.wallet.dCostNum / XiShu;
        if (DataMgr.m_account.wallet.goldNum != 0)
        {
            GoldCoinsText.text = DataMgr.m_account.wallet.goldNum.ToString("##,###");
        }
        else
        {
            GoldCoinsText.text = DataMgr.m_account.wallet.goldNum.ToString();
        }
        //GoldCoinsText.text = String.Format("{0:N}", DataMgr.m_account.wallet.goldNum);
        if (DataMgr.m_account.wallet.diamondNum != 0)
        {
            DiamondsText.text = DataMgr.m_account.wallet.diamondNum.ToString("##,###");
        }
        else
        {
            DiamondsText.text = DataMgr.m_account.wallet.diamondNum.ToString();
        }
        //DiamondsText.text = DataMgr.m_account.wallet.diamondNum.ToString();
        //DiamondsText.text= String.Format("{0:N}", DataMgr.m_account.wallet.diamondNum);
        //CashCouponText.text = ((double)DataMgr.m_account.wallet.sMoneyNum).ToString("0.00");
        CashCouponText.text = String.Format("{0:N}", DataMgr.m_account.wallet.sMoneyNum);
        //CashText.text = ((double)DataMgr.m_account.wallet.moneyNum).ToString("0.00");
        CashText.text = String.Format("{0:N}", DataMgr.m_account.wallet.moneyNum);
        //AssetText.text = DataMgr.m_account.wallet.asset.ToString();
        AssetText.text = DataMgr.m_zan.ToString();
        //DataMgr.m_account.commendCode
        commendCodeText.text = "你的邀请码为：" + DataMgr.m_account.commendCode;
        EduText.text ="额度：" +DataMgr.m_account.wallet.cashLimit+"元";
        if (DataMgr.m_account.hadProxy == 1)
        {
            EduText.gameObject.SetActive(false);
        }
        //DaiLiWeiText.text = "剩余代理位:<color=#0A7AE8>" + 9.ToString()+"</color>";
        for (int i = 0; i < MenuPar.transform.childCount; i++)
        {
            ClickListener.Get(MenuPar.transform.GetChild(i).gameObject).onClick = clickMenu;
        }
    }
    // Update is called once per frame
    public override void OnOpen()
    {
        UpdateWalletDate();
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspQueryOrderMessage, OnNetRspQOM);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspQueryBillMessage, OnNetRspQBM);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetWalletDateMessage, OnNetRspGWDM);
        // NetEventManager.Instance.AddEventListener()

        string str;
        if (JsonMgr.GetJsonString(AppConst.LocalPath + "/config") == "true")
        {
            str = "关闭音乐";
        }
        else
        {
            str = "打开音乐";
        }
        audioBut.transform.GetChild(0).GetComponent<Text>().text = str;
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspQueryOrderMessage, OnNetRspQOM);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspQueryBillMessage, OnNetRspQBM);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetWalletDateMessage, OnNetRspGWDM);
    }
    void OnNetRspGWDM(byte[] buf)
    {
        RspGetWalletDateMessage RspGWDM = PBSerializer.NDeserialize<RspGetWalletDateMessage>(buf);
        DataMgr.m_account.wallet = RspGWDM.wallet;
        TextInit();
    }
    public void OnNetRspQBM(byte[] buf)
    {
        RspQueryBillMessage RspQBm = PBSerializer.NDeserialize<RspQueryBillMessage>(buf);
        UIManager.Instance.PushPanel(UIPanelName.billflowpanel, false, false, pragrm =>
        {
            // allbillscroll.ABS.Init(RspQBm);
        });
    }
    public void OnNetRspQOM(byte[] buf)
    {
        RspQueryOrderMessage RspQOM = PBSerializer.NDeserialize<RspQueryOrderMessage>(buf);
    }
    void clickTiXianBtn(GameObject obj)
    {
        Debug.Log("点击了clickTiXianBtn");
        //UIManager.Instance.PushPanel(UIPanelName.tixianpanel);
        List<PayAccount> Target_ListPayAccount =
            JsonConvert.DeserializeObject<List<PayAccount>>(DataMgr.m_account.userPayAccount);
        if (Target_ListPayAccount.Count == 0)
        {
            UIManager.Instance.PushPanel(UIPanelName.editaccountpanel, false, true, (param) =>
            {
                editaccountpanel eap = param.GetComponent<editaccountpanel>();
                eap.m_ListPayAccount = Target_ListPayAccount;
                eap.Stateinit(false);
                eap.IsALiPay = true;
            });
            return;
        }
        PayAccount Target_PayAccount = Target_ListPayAccount[0];
        UIManager.Instance.PushPanel(UIPanelName.shurumimapanel, false, true, paragrm =>
        {
            paragrm.GetComponent<shurumimapanel>().m_PayAccount = Target_PayAccount;
            paragrm.GetComponent<shurumimapanel>().openPanelView(1);
            paragrm.GetComponent<shurumimapanel>().IsAliPay = Target_PayAccount.payType;
        });
    }
    public void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf(true);
        //NewGuideMgr.Instance.StartOneNewGuide();
    }
    void clickCiShanBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.cishanpanel, false, false);
    }

    void clickGouWuBangZhuBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.noticepanel, false, false, (param) =>
            {
                NoticePanel notice = param.GetComponent<NoticePanel>();
                notice.ShowCommonNotice(0);
                //NewGuideMgr.Instance.StartOneNewGuide();
            }, true
        );
    }
    void clickMenu(GameObject obj)
    {
        switch (obj.name)
        {
            case "DingdanBtn":
                UIManager.Instance.PushPanel(UIPanelName.dingdanpanel, false, false, paragrm => { paragrm.GetComponent<dingdanpanel>().init(); });
                break;
            case "AnQuanBtn":
                UIManager.Instance.PushPanel(UIPanelName.accountsecuritypanel, false, false, paragrm => { paragrm.GetComponent<accountsecuritypanel>().Init(DataMgr.m_account.userPayAccount); });
                break;
            case "LiuShuiBtn":
                //ReqQueryBillMessage ReqQBM = new ReqQueryBillMessage();
                //ReqQBM.billType = -1;
                //ReqQBM.lastDate = null;
                //HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqQueryBillMessage, ReqQBM);
                UIManager.Instance.PushPanel(UIPanelName.billflowpanel, false, false, paragrm => { paragrm.GetComponent<billflowpanel>().Init(); });
                break;
            case "DiZhiBtn":
                UIManager.Instance.PushPanel(UIPanelName.addresspanel);
                break;
            case "GouWuCheBtn":
                UIManager.Instance.PushPanel(UIPanelName.shoppingcartpanel);
                break;
        }
    }

    void clickweb(GameObject obj)
    {
        webdisplaypanel.Type = 0;
        UIManager.Instance.PushPanel(UIPanelName.webdisplaypanel, false, false);
    }
    void clickaudio(GameObject obj)
    {
        string str = obj.transform.GetChild(0).GetComponent<Text>().text;
        if (str == "关闭音乐")
        {
            obj.transform.GetChild(0).GetComponent<Text>().text = "打开音乐";
            JsonMgr.SaveJsonString("false", AppConst.LocalPath + "/config");
            Audio_control.instance.CloseallAudio(false);
        }
        else
        {
            obj.transform.GetChild(0).GetComponent<Text>().text = "关闭音乐";
            JsonMgr.SaveJsonString("true", AppConst.LocalPath + "/config");
            Audio_control.instance.CloseallAudio(true);
        }
    }

    void Exit()
    {
        UIManager.Instance.PopSelf(true);
    }
}
