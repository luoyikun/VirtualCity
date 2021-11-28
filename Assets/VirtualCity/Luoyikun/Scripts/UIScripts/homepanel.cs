using Framework.Event;
using Framework.Tools;
using Framework.UI;
using Newtonsoft.Json;
using ProtoDefine;
using SGF.Codec;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//这个属于常驻界面
public class homepanel : UGUIPanel {
    public Image m_headImg;
    public Text m_textName;
    public Text m_textGold;
    public Text m_textDiamond;
    public Text m_textCoupon;
    public Text m_textRmb;
    public Text m_textAsset;
    public GameObject m_btnTiXian;
    public GameObject m_btnRank;
    public GameObject m_btnBuild;
    public GameObject m_btnNav;
    public GameObject m_btnPresonlInfo;

    public GameObject m_leftDown;
    public GameObject m_rightDown;
    public static homepanel m_instance;

    public GameObject m_btnExitBuild;
    public GameObject m_topBar;

    public GameObject m_walletPar; // 导航格子 
    public GameObject Textdisplay; //飞字
    public GameObject LittlegameBut;
    public GameObject Getrewards;
    Coroutine m_corGold;
    Coroutine m_corDiamond;
    Coroutine m_corSmoney;
    Coroutine m_corMoney;
    Coroutine m_corAsset;
    Coroutine IErecord;
    public GameObject m_btnOtherBack;
    public GameObject m_myHeadPar;
    public GameObject m_ohterHeadPar;
    public GameObject m_btnZan;
    public Sprite[] imgarr;
    private ChatUser Target_ChatUser;
    bool m_isHtHasZan = false;
    public int m_time = 0;
    public bool m_timeio;
    int fishId;
    //public static List<string> Player_name_arr=new List<string>();
    // Use this for initialization
    void Start() {
        m_instance = this;

        ClickListener.Get(m_btnTiXian).onClick = OnBtnTiXian;
        ClickListener.Get(m_btnRank).onClick = OnBtnRank;
        ClickListener.Get(m_btnBuild).onClick = OnBtnBuild;
        ClickListener.Get(m_btnNav).onClick = OnBtnNav;
        ClickListener.Get(m_btnPresonlInfo).onClick = OnBtnPresonlInfo;
        ClickListener.Get(m_btnExitBuild).onClick = OnBtnExitBuild;
       
        ClickListener.Get(LittlegameBut).onClick = Littlegame;

        ClickListener.Get(m_btnOtherBack).onClick = OnBtnOtherBack;
        ClickListener.Get(m_btnZan).onClick = OnBtnZan;
        ClickListener.Get(Getrewards.transform.GetChild(4).gameObject).onClick = OnBtnRewardClose;
        ClickListener.Get(Getrewards.transform.GetChild(5).gameObject).onClick = OnBtnRewardOpen;
        //ClickListener.Get(m_btn).onClick = OnBtnChat0;
        UiInitWhenOpen();

        if (NewGuideDemo.m_isNewGuideDemo == true)
        {
            m_btnTiXian.SetActive(true);
        }
    }

    public void OnBtnOtherBack(GameObject obj)
    {
        VirtualCityMgr.GotoHometown(EnMyOhter.My);
    }

    public void Littlegame(GameObject obj)
    {
        m_btnTiXian.SetActive(false);
        //  ReqGetFishMessage req = new ReqGetFishMessage();
        //   req.accountId = DataMgr.m_accountId;
        //   Debug.Log(req.accountId);
        //    GameSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetFishMessage, req);




        if (!m_timeio)
        {
            m_timeio = true;
            TimeManager.Instance.RemoveTask(OnTimeChatWorld);
            TimeManager.Instance.AddTask(1.0f, true, OnTimeChatWorld);
        }
        webdisplaypanel.Type = 1;
        UIManager.Instance.PushPanel(UIPanelName.webdisplaypanel, false, false);
    }

    public void OnBtnZan(GameObject obj)
    {
        if (m_isHtHasZan == false)
        {
            ReqZanHometownMessage req = new ReqZanHometownMessage();
            req.playerId = DataMgr.m_taInfo.accountId;
            GameSocket.Instance.SendMsgProto(MsgIdDefine.ReqZanHometownMessage, req);
        }
    }
    public void CloseBtnTiXian(EventData data)
    {
        m_btnTiXian.SetActive(false);
    }
    public void OnBtnExitBuild(GameObject obj)
    {
        m_leftDown.SetActive(true);
        m_rightDown.SetActive(true);

        if ( DataMgr.m_account.hadProxy == 0)
        {
            m_btnTiXian.SetActive(true);
        }
        else if (DataMgr.m_account.hadProxy == 1)
        {
            m_btnTiXian.SetActive(false);
        }
        m_btnExitBuild.SetActive(false);
        m_topBar.SetActive(false);
        switch (DataMgr.m_curScene)
        {
            case EnCurScene.Home:
                HomeMgr.m_instance.ChangeToDisplay();
                EventManager.Instance.DispatchEvent(Common.EventStr.BuildHome, new EventDataEx<bool>(false));
                break;
            case EnCurScene.Hometown:
                buildhometown.m_instance.m_btMode = BtMode.Display;
                buildhometown.m_instance.SetBoxSelect("");

                EventManager.Instance.DispatchEvent(Common.EventStr.BuildHomeTown, new EventDataEx<bool>(false));

                if (buildhometown.m_instance.m_curBuild != null)
                {
                    Destroy(buildhometown.m_instance.m_curBuild);
                }
                break;
            default:
                break;
        }
    }
    public void OnBtnPresonlInfo(GameObject obj)
    {
        //if (UIManager.Instance.IsTopPanel(UIPanelName.woyaoqupanel) == true)
        //{
        //    UIManager.Instance.PopSelf();
        //}
        if (DataMgr.m_myOther == EnMyOhter.My)
        {
            UIManager.Instance.PushPanel(UIPanelName.personalinfo, false, false, (param) => {
                //NewGuideMgr.Instance.StartOneNewGuide();
            });
        }
    }

    public void OnBtnRewardClose(GameObject obj)
    {
        Getrewards.SetActive(false);
    }
    public void OnBtnRewardOpen(GameObject obj)
    {
        Getrewards.SetActive(false);
        Littlegame(obj);
    }

    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspZanHomeTownMessage, OnNetRspZanHomeTownMessage);

      //  NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetFishMessage, OnNetRspBuyreward);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCashFishMessage, OnNetRspBuyrewardtype);
       

        if (DataMgr.m_account.wallet.asset != 0)
        {
            m_textAsset.text = DataMgr.m_account.wallet.asset.ToString("##,###");
        }
        else
        {
            m_textAsset.text = DataMgr.m_account.wallet.asset.ToString();
        }
        //UiInitWhenOpen();
        m_textGold.text = PublicFunc.GetAmountInt((int)DataMgr.m_account.wallet.goldNum);

        m_textDiamond.text = PublicFunc.GetAmountInt((int)DataMgr.m_account.wallet.diamondNum);

        //m_textCoupon.text = PublicFunc.GetAmountDoub(PublicFunc.GetDicimals((double)DataMgr.m_account.wallet.sMoneyNum));
        m_textCoupon.text = PublicFunc.GetAmountDoub((double)DataMgr.m_account.wallet.sMoneyNum);
        //m_textRmb.text = PublicFunc.GetAmountDoub(PublicFunc.GetDicimals((double)DataMgr.m_account.wallet.moneyNum));
        m_textRmb.text = PublicFunc.GetAmountDoub(((double)DataMgr.m_account.wallet.moneyNum));

        if (Textdisplay != null) {
            Textdisplay.SetActive(false);
        }
        StartCoroutine(IEtest(VcData.m_listFakeName));

    }

    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspZanHomeTownMessage, OnNetRspZanHomeTownMessage);

      //  NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetFishMessage, OnNetRspBuyreward);

        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCashFishMessage, OnNetRspBuyrewardtype);
        Image btnZan = m_btnZan.GetComponent<Image>();
        AssetMgr.Instance.CreateSpr("Common_empty_point", "commonicon", (spr) => { btnZan.sprite = spr; });
    }

    void OnNetRspZanHomeTownMessage(byte[] buf)
    {
        RspZanHomeTownMessage rsp = PBSerializer.NDeserialize<RspZanHomeTownMessage>(buf);
        if (rsp.code == 0)
        {
            Hint.LoadTips(rsp.tips);
        }
        else {
            Image btnZan = m_btnZan.GetComponent<Image>();
            AssetMgr.Instance.CreateSpr("HomeTown_btn_22_1", "commonicon", (spr) => { btnZan.sprite = spr; });
            m_isHtHasZan = true;

            int str = int.Parse(m_ohterHeadPar.transform.Find("Praise/Text").GetComponent<Text>().text);
            str++;
            m_ohterHeadPar.transform.Find("Praise/Text").GetComponent<Text>().text = str.ToString();
            Debug.Log("收到回包");

            if (DataMgr.m_dicZan.ContainsKey(rsp.playerId) == false)
            {
                DataMgr.m_dicZan[rsp.playerId] = new List<string>();
                DataMgr.m_dicZan[rsp.playerId].Add("homeTown");
            }
            else {
                DataMgr.m_dicZan[rsp.playerId].Add("homeTown");
            }
        }
    }
    /*
    void OnNetRspBuyreward(byte[] buf)
    {
        RspGetFishMessage rsp = PBSerializer.NDeserialize<RspGetFishMessage>(buf);
        Debug.Log("回包"+rsp.fishId.ToString());
        fishId=(int)rsp.fishId;
        if (fishId!=0) {
            if (!m_timeio)
            {
                m_timeio = true;
                TimeManager.Instance.RemoveTask(OnTimeChatWorld);
                TimeManager.Instance.AddTask(1.0f, true, OnTimeChatWorld);
            }
        }
        if (fishId == -1)
        {
            Hint.LoadTips("金币不足", Color.white);
           m_btnTiXian.SetActive(true);
        }
        else
        {
            webdisplaypanel.Type = 1;
            UIManager.Instance.PushPanel(UIPanelName.webdisplaypanel, false, false);
        }
    }
    */
    void OnNetRspBuyrewardtype(byte[] buf)
    {
        RspCashFishMessage rsp = PBSerializer.NDeserialize<RspCashFishMessage>(buf);
        Debug.Log(rsp.number+rsp.type.ToString());
        if (rsp.type == "none")
        {
            return;
        }

        Getrewards.SetActive(true);
        Image img = Getrewards.transform.GetChild(2).GetComponent<Image>();
        Text m_text = Getrewards.transform.GetChild(3).GetComponent<Text>();
        switch (rsp.type)
        {
            case "gold":
                img.sprite = imgarr[0];
                m_text.text =string.Format(rsp.number.ToString()+"金币");
                break;
            case "diamond":
                img.sprite = imgarr[1]; 
                m_text.text = string.Format(rsp.number.ToString() + "钻石");
                break;
            case "part":
                //AssetMgr.Instance.CreateSpr("GetReward_money", "homepanel", (spr) => { m_icon.sprite = spr; });
                break;
            case "money":
               img.sprite = imgarr[2]; 
                m_text.text = string.Format(rsp.number.ToString() + "现金");
                break;
            default:
                break;
        }
    }


    private void OnEnable()
    {
        
        EventManager.Instance.AddEventListener(Common.EventStr.CloseTiXian, CloseBtnTiXian);
        EventManager.Instance.AddEventListener(Common.EventStr.WalletUpdate, OnEvWalletUpdate);
        //NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCreateOrderMessage, OnNetRspCreateOrderMessage);


        NetEventManager.Instance.AddEventListener(MsgIdDefine.TopBarMessage, Receive_data);
    }

    public void UiInitWhenOpen()
    {
        //m_btnExitBuild.SetActive(false);
        
        if (DataMgr.m_account != null && DataMgr.m_account.hadProxy == 0)
        {
            m_btnTiXian.SetActive(true);
        }
        else {
            if (NewGuideDemo.m_isNewGuideDemo == true)
            {
                m_btnTiXian.SetActive(true);
            }
            else {
                m_btnTiXian.SetActive(false);
            }
        }
        m_rightDown.SetActive(true);
    }
    private void OnDisable()
    {
        EventManager.Instance.RemoveEventListener(Common.EventStr.WalletUpdate, OnEvWalletUpdate);
        EventManager.Instance.RemoveEventListener(Common.EventStr.CloseTiXian, CloseBtnTiXian);
        //NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCreateOrderMessage, OnNetRspCreateOrderMessage);


        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.TopBarMessage, Receive_data);
    }

    //void OnNetRspCreateOrderMessage(byte[] buf)
    //{
    //    RspCreateOrderMessage rsp = PBSerializer.NDeserialize<RspCreateOrderMessage>(buf);
    //    if (rsp.code == 0)
    //    {
    //        Hint.LoadTips(rsp.tips, Color.white);
    //    }
    //    else {
    //        if (Application.platform == RuntimePlatform.Android)
    //        {
    //            AndroidFunc.AliPay(rsp.payInfo);
    //        }
    //        else if (Application.platform == RuntimePlatform.IPhonePlayer)
    //        {
    //            AliComponent.Instance.AliPay(rsp.payInfo);
    //        }
    //    }
    //}

    public void OnBtnChat(GameObject obj)
    {
        //UIManager.Instance.PushPanel(UIPanelName.chatpanel);
        //Debug.Log("支付宝测试");
        //string testStr = "{\"order\": [{ 		\"id\": null, 		\"accountId\": 97834127255928847, 		\"businessName\": null, 		\"businessId\": 0, 		\"no\": null, 		\"goodsId\": 97889172932526089, 		\"goodsKindname\": null, 		\"goodsKindId\": 97889172932526090, 		\"goodsKindUrl\": null, 		\"goodsNum\": 1, 		\"payStatus\": null, 		\"payType\": 0, 		\"payTime\": null, 		\"payInfo\": null, 		\"payNum\": 0.0, 		\"payMoney\": 0.0, 		\"paySMoney\": 0.0, 		\"expressInfo\": \"{\\\"ID\\\":1,\\\"Name\\\":\\\"2311232131\\\",\\\"Telephone\\\":12321323121,\\\"DiQu\\\":\\\"213231321123\\\",\\\"DiZhi\\\":\\\"213321321\\\",\\\"YouZhengBianMa\\\":213232,\\\"IsMoRen\\\":true}\", 		\"customerTel\": \"12321323121\", 		\"hasSend\": 0, 		\"sendTime\": null, 		\"expressCode\": null, 		\"hasGet\": 0, 		\"orderStatus\": null, 		\"isRefund\": 0, 		\"refundReason\": null, 		\"refundNum\": 0, 		\"refundMoney\": 0.0, 		\"gmApproveResault\": 0, 		\"gmApproveTime\": null, 		\"refundExpressInfo\": null, 		\"refundExpressCode\": null, 		\"refundSendTime\": null, 		\"hasBusinessGet\": 0, 		\"hasRefund\": 0, 		\"refundPayInfo\": null, 		\"billId\": 0, 		\"hasPayBusiness\": 0, 		\"remark\": null, 		\"createtime\": null, 		\"updatetime\": null 	}] }";
        //ReqCreateOrderMessage req = JsonConvert.DeserializeObject<ReqCreateOrderMessage>(testStr);

        //HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqCreateOrderMessage, req);

        UIManager.Instance.PushPanel(UIPanelName.chatpanel, false, false);    }

    void OnEvWalletUpdate(EventData data)
    {
        var exdata = data as EventDataEx<WalletUpdateDiff>;
        WalletUpdateDiff diff = exdata.GetData();


        int oriGold = (int)DataMgr.m_account.wallet.goldNum - diff.goldDiff;
        int oriDiamond = (int)DataMgr.m_account.wallet.diamondNum - diff.diamondDiff;
        int oriAsset = (int)DataMgr.m_account.wallet.asset - diff.assetDiff;

        double oriSmoney = (double)DataMgr.m_account.wallet.sMoneyNum - diff.sMoneyDiff;
        double oriMoney = (double)DataMgr.m_account.wallet.moneyNum - diff.moneyDiff;

        int toGold = (int)DataMgr.m_account.wallet.goldNum;
        int toDiamond = (int)DataMgr.m_account.wallet.diamondNum;

        double toSmoney = (double)DataMgr.m_account.wallet.sMoneyNum;
        double toMoney = (double)DataMgr.m_account.wallet.moneyNum;

        int toAsset = (int) DataMgr.m_account.wallet.asset;


        Debug.Log("钱包信息变化");
        //m_textGold.text = PublicFunc.GetAmountInt((int)DataMgr.m_account.wallet.goldNum);

        //m_textDiamond.text = PublicFunc.GetAmountInt((int)DataMgr.m_account.wallet.diamondNum);

        //m_textCoupon.text = PublicFunc.GetAmountInt((int)DataMgr.m_account.wallet.sMoneyNum);

        //m_textRmb.text = PublicFunc.GetAmountInt((int)DataMgr.m_account.wallet.moneyNum);

        if (m_corGold != null && diff.goldDiff != 0)
        {
            StopCoroutine(m_corGold);
        }
        if (diff.goldDiff != 0)
        {
            m_corGold = StartCoroutine(PublicFunc.YieldDynamicText(m_textGold, oriGold, toGold));
        }

        if (m_corDiamond != null && diff.diamondDiff != 0 )
        {
            StopCoroutine(m_corDiamond);
        }
        if (diff.diamondDiff != 0)
        {
            m_corDiamond = StartCoroutine(PublicFunc.YieldDynamicText(m_textDiamond, oriDiamond, toDiamond));
        }

        if (m_corSmoney != null && diff.sMoneyDiff != 0)
        {
            StopCoroutine(m_corSmoney);
        }
        if (diff.sMoneyDiff != 0)
        {
            m_corSmoney = StartCoroutine(PublicFunc.YieldDynamicText(m_textCoupon, oriSmoney, toSmoney));
        }

        if (m_corMoney != null && diff.moneyDiff != 0)
        {
            StopCoroutine(m_corMoney);
        }
        if (diff.moneyDiff != 0)
        {
            m_corMoney = StartCoroutine(PublicFunc.YieldDynamicText(m_textRmb, oriMoney, toMoney));
        }
        if (m_corAsset != null && diff.assetDiff != 0)
        {
            StopCoroutine(m_corAsset);
        }
        if (diff.assetDiff != 0)
        {
            m_corAsset = StartCoroutine(PublicFunc.YieldDynamicTextNoM(m_textAsset, oriAsset, toAsset));
        }
    }
    public void OnBtnTiXian(GameObject obj)
    {
        //if (UIManager.Instance.IsTopPanel(UIPanelName.woyaoqupanel) == true)
        //{
        //    UIManager.Instance.PopSelf();
        //}
        if (NewGuideDemo.m_isNewGuideDemo == false)
        {
            if (DataMgr.m_account.hadProxy == null || DataMgr.m_account.hadProxy == 0)
            {
                UIManager.Instance.PushPanel(UIPanelName.dailiquanpanel, false, false, paragrm =>
                {
                    paragrm.GetComponent<dailiquanpanel>().OpenPanelWindows(0);
                    //NewGuideMgr.Instance.StartOneNewGuide();
                });

            }
        }
        else {
            UIManager.Instance.PushPanel(UIPanelName.dailiquanpanel, false, false, paragrm =>
            {
                paragrm.GetComponent<dailiquanpanel>().OpenPanelWindows(0);
                //NewGuideMgr.Instance.StartOneNewGuide();
            });
        }
    }

    public void OnBtnRank(GameObject obj)
    {
        if (UIManager.Instance.IsTopPanel(UIPanelName.woyaoqupanel) == true)
        {
            UIManager.Instance.PopSelf();
        }
        UIManager.Instance.PushPanel(UIPanelName.rankpanel);
    }

    public void OnBtnBuild(GameObject obj)
    {
        //if (UIManager.Instance.IsTopPanel(UIPanelName.woyaoqupanel) == true)
        //{
        //    UIManager.Instance.PopSelf();
        //}

        
        m_btnExitBuild.SetActive(true);
        m_leftDown.SetActive(false);
        m_rightDown.SetActive(false);
        m_btnTiXian.SetActive(false);
        m_topBar.SetActive(true);

        if (DataMgr.m_curScene == EnCurScene.Hometown)
        {
            buildhometown.m_instance.SetBoxCollider(true);
            buildhometown.m_instance.m_btMode = BtMode.Build;
            //打开建造面板
            EventManager.Instance.DispatchEvent(Common.EventStr.BuildHomeTown, new EventDataEx<bool>(true));
                        if (DataMgr.m_isNewGuide == true)
            {
                hometwonbuildheadpanel.m_instance.CreateLocationMarker(DataMgr.m_newBoxId, EnLand.None);
                //NewGuideMgr.Instance.StartOneNewGuide();
            }        }
        else if (DataMgr.m_curScene == EnCurScene.Home)
        {
            HomeMgr.m_instance.ChangeToBuild();
            EventManager.Instance.DispatchEvent(Common.EventStr.BuildHome, new EventDataEx<bool>(true));
        }
    }


    public void SetState(EnCurScene curScene,EnMyOhter enMyOhter,ChatUser user = null)
    {
        UiInitWhenOpen();
        DataMgr.m_curScene = curScene;
        switch (curScene)
        {
            case EnCurScene.Home:
                m_btnBuild.SetActive(true);
                break;
            case EnCurScene.Hometown:
                m_btnBuild.SetActive(true);
                break;
            case EnCurScene.Business:
                m_btnBuild.SetActive(false);
                break;
            case EnCurScene.Shop:
                m_btnBuild.SetActive(false);
                break;
            default:
                break;
        }

        switch (enMyOhter)
        {
            case EnMyOhter.My:
                //m_btnBuild.SetActive(false);
                m_walletPar.SetActive(true);
                m_myHeadPar.SetActive(true);
                m_ohterHeadPar.SetActive(false);
                m_btnOtherBack.SetActive(false);
                break;
            case EnMyOhter.Other:
                m_btnBuild.SetActive(false);
                m_walletPar.SetActive(false);
                m_myHeadPar.SetActive(false);
                m_ohterHeadPar.SetActive(true);
                m_btnOtherBack.SetActive(true);

                m_isHtHasZan = false;
                if (DataMgr.m_dicZan.ContainsKey(DataMgr.m_taInfo.accountId) == true)
                {
                    if (DataMgr.m_dicZan[DataMgr.m_taInfo.accountId].Contains("homeTown") == true)
                    {
                        m_isHtHasZan = true;
                    }
                }

                Image btnZan = m_btnZan.GetComponent<Image>();
                if (m_isHtHasZan == false)
                {
                    AssetMgr.Instance.CreateSpr("HomeTown_btn_26", "commonicon", (spr) => { btnZan.sprite = spr; });
                }
                else {
                    AssetMgr.Instance.CreateSpr("HomeTown_btn_22_1", "commonicon", (spr) => { btnZan.sprite = spr; });
                }
                break;
        }

        UpdateHeadImg(enMyOhter,user);
    }

    public void UpdateHeadImg(EnMyOhter myOther,ChatUser user = null)
    {
        Target_ChatUser = user;
        if (myOther == EnMyOhter.Other)
        {
            Image otherHeadImg = m_ohterHeadPar.transform.Find("information/Image/Image").GetComponent<Image>();
            Text otherName = m_ohterHeadPar.transform.Find("information/Text").GetComponent<Text>();
            string headImg = PublicFunc.GetUserHeadImg(user.modelId);
            m_ohterHeadPar.transform.Find("Praise/Text").GetComponent<Text>().text=DataMgr.m_zan.ToString();
            AssetMgr.Instance.CreateSpr(headImg, "charactericon", (spr) => { otherHeadImg.sprite = spr; });
            ClickListener.Get(m_ohterHeadPar.transform.Find("information").gameObject).onClick = clickOtherPlayerHead;
            otherName.text = user.userName;
        }
        else if (myOther == EnMyOhter.My)
        {
            string headImg = PublicFunc.GetUserHeadImg((long)DataMgr.m_account.modleId);

            AssetMgr.Instance.CreateSpr(headImg, "charactericon", (spr) => { m_headImg.sprite = spr; });

            m_textName.text = DataMgr.m_account.userName;
        }
    }

    void clickOtherPlayerHead(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.userinfopanel,false,true,paragrm=>{paragrm.GetComponent<userinfopanel>().InitPlayer(Target_ChatUser.accountId);});
    }
    public void speedtext(string str,int type)
    {
        Textdisplay.SetActive(true);
        if (IErecord!=null){
            StopCoroutine(IErecord);
        }
        GameObject Textobg = Textdisplay.transform.GetChild(0).GetChild(0).gameObject;
        Text text = Textobg.GetComponent<Text>();
        Textobg.GetComponent<RectTransform>().sizeDelta = new Vector2(str.Length * text.fontSize, 70);
        Textobg.transform.localPosition = Vector3.zero;
        text.text = str;
        IErecord = StartCoroutine(IEspeedtext(type,Textobg.transform, str.Length * text.fontSize));
    }

    IEnumerator IEspeedtext(int type,Transform pos,float width)
    {
        while (pos.localPosition.x+960> -width)
        {
            if (type == 0&& (pos.localPosition.x + 960)-10 <= -width)
            {
                pos.localPosition = Vector3.zero;
            }
            pos.localPosition = new Vector3(pos.localPosition.x - 5, 0, 0);
            yield return 0;
        }
        Textdisplay.SetActive(false);
    }

    public void Receive_data(byte[] buf)
    {
        TopBarMessage rsp = PBSerializer.NDeserialize<TopBarMessage>(buf);
        speedtext(rsp.getMessage(),1);

    }

    

    IEnumerator IEtest(List<TinyPlayer> arr)
    {
        string str;
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(30, 60));
            int num = Random.Range(0, 6);
            if (num == 1|| num == 2)
            {
                str =string.Format("恭喜"+arr[Random.Range(0, arr.Count)].name+"：获取了VIP");
            }
            else
            {
                str = string.Format("恭喜" + arr[Random.Range(0, arr.Count)].name + "：提现了"+ Random.Range(1,51).ToString()+"0元");
            }
            speedtext(str, 1);
        }
    }


    void OnTimeChatWorld()
    {
        if (m_timeio)
        {
            m_time++;
        }
        Debug.Log(m_time);
        if (m_time>=60)
        {
            TimeManager.Instance.RemoveTask(OnTimeChatWorld);
            ReqCashFishMessge req = new ReqCashFishMessge();
            req.fishId = fishId;// DataMgr.m_taInfo.accountId;
            GameSocket.Instance.SendMsgProto(MsgIdDefine.ReqCashFishMessge, req);
            m_time = 0;
        }
    }

    public void OnBtnNav(GameObject obj)
    {
        //if (UIManager.Instance.IsTopPanel(UIPanelName.woyaoqupanel) == true)
        //{
        //    UIManager.Instance.PopSelf();
        //}
        //else {

        //    UIManager.Instance.PushPanel(UIPanelName.woyaoqupanel, false, true,
        //    (param) => {
        //        param.GetComponent<woyaoqupanel>().InfoUpdate();
        //    }
        //        );

        //}
        Debug.Log("点击了导航");
        if (DataMgr.m_curScene == EnCurScene.Business)
        {
            UIManager.Instance.PushPanel(UIPanelName.searchgoodspanel);
            return;
        }
        UIManager.Instance.PushPanel(UIPanelName.woyaoqupanel, false, true,
            (param) =>
            {
                woyaoqupanel woyao = param.GetComponent<woyaoqupanel>();
                woyao.InfoUpdate();
                //woyao.m_actOpenFinish = () => { NewGuideMgr.Instance.StartOneNewGuide(2); };
                
            }
        );

    }

    public void BtnExitBuildSet(bool isActive)
    {
        m_btnExitBuild.SetActive(isActive);
    }
}
