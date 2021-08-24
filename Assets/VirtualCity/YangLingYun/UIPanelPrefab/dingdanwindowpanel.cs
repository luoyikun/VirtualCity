using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using ProtoDefine;
using SGF.Codec;
using Newtonsoft.Json;

public class Reason
{
    public List<string> text = new List<string>();
}
public class expressMessage
{
    public string AcceptStation;
    public string AcceptTime;
    public string Remark;
}
public class dingdanwindowpanel : UGUIPanel
{
    public GameObject XuanZeWindows;
    public GameObject ShenQingWindows;
    public GameObject TianXieWuLiuWindows;
    public GameObject ChaKanWuLiuWindwos;
    public GameObject MainPar;
    public GameObject QueDingShouHuoWindows;
    public GameObject PingLunWindows;
    public GameObject ExpressTmp;
    Text ReasonText;
    Text TuiHuoBianHaoText;
    Text TuiHuoTelephoneText;
    Text TuiHuoShuoMingText;
    Text PingJiaText;
    Order Target_Order = new Order();
    int Score = 5;
    double GoodsSingePrice = 0;
    double RefundMoney = 0;
    int RefundNumber = 0;
    bool IsHasSend = false;
    bool IsExpressOK = false;
    //bool IsScore = false;
    // Use this for initialization
    public override void OnOpen()
    {
        MainPar = this.transform.Find("Content").gameObject;
        for (int i = 0; i < MainPar.transform.childCount; i++)
        {
            MainPar.transform.GetChild(i).gameObject.SetActive(false);
        }
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, OnNetRspCM);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCreateCommentsMessage, OnNetRspCCM);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetExpressInfoMessage, OnNetRspGEIM);
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, OnNetRspCM);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCreateCommentsMessage, OnNetRspCCM);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetExpressInfoMessage, OnNetRspGEIM);
    }
    void OnNetRspGEIM(byte[] buf)
    {
        RspGetExpressInfoMessage Rsp_GEIM = PBSerializer.NDeserialize<RspGetExpressInfoMessage>(buf);
        ClickListener.Get(ChaKanWuLiuWindwos.transform.Find("BG").Find("BackBtn").gameObject).onClick = clickBackBtn;
        ChaKanWuLiuWindwos.SetActive(true);

        Transform target_Tran = ChaKanWuLiuWindwos.transform.Find("Main").Find("GoodInfo");

        target_Tran.Find("GoodsImage").GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + Target_Order.goodsKindUrl);
        target_Tran.Find("GoodName").GetComponent<Text>().text = Target_Order.businessName + "/" + Target_Order.goodsKindname;
        target_Tran.Find("GoodPrice").GetComponent<Text>().text = Target_Order.payNum + "元";
        target_Tran.Find("GoodsNum").GetComponent<Text>().text = "x" + Target_Order.goodsNum;

        target_Tran = ChaKanWuLiuWindwos.transform.Find("Main").Find("ScrollView").GetChild(0).GetChild(0);

        target_Tran.Find("expressBusiness").Find("Name").GetComponent<Text>().text = "快递公司:" + Rsp_GEIM.expressInfo.expressBusinessName;
        target_Tran.Find("expressBusiness").Find("Code").GetComponent<Text>().text = "快递单号:" + Rsp_GEIM.expressInfo.expressCode;
        IsExpressOK = false;
        if (Rsp_GEIM.expressInfo.expressStatus == 3)
        {
            IsExpressOK = true;
        }
        if (Rsp_GEIM.expressInfo.expressMessage != null)
        {
            List<expressMessage> TargetExpressMsagge = JsonConvert.DeserializeObject<List<expressMessage>>(Rsp_GEIM.expressInfo.expressMessage);
            IniExpress(TargetExpressMsagge, target_Tran);
        }
        else
        {
            Hint.LoadTips("目前没有物流信息", Color.white);
            
            target_Tran.Find("expressBusiness").Find("Code").GetComponent<Text>().text = "快递单号:" + Target_Order.expressCode;
        }
    }
    void IniExpress(List<expressMessage> m_ExpressMessage,Transform ExpressPar)
    {
        if (ExpressPar.childCount > 1)
        {
            for (int i = ExpressPar.childCount - 1; i >= 1; i--)
            {
                DestroyImmediate(ExpressPar.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < m_ExpressMessage.Count; i++)
        {
            GameObject obj = PublicFunc.CreateTmp(ExpressTmp, ExpressPar);
            obj.transform.Find("AcceptStation").GetComponent<Text>().text = m_ExpressMessage[i].AcceptStation;
            obj.transform.Find("AcceptTime").GetComponent<Text>().text = m_ExpressMessage[i].AcceptTime;
            if (IsExpressOK == true)
            {
                obj.transform.Find("OkIcon").gameObject.SetActive(true);
                obj.transform.Find("Icon").gameObject.SetActive(false);
            }
        }
    }
    void OnNetRspCCM(byte[] buf)
    {
        RspCreateCommentsMessage RspCCM = PBSerializer.NDeserialize<RspCreateCommentsMessage>(buf);
        if (RspCCM.code != 0)
        {
            backAndBack(new GameObject());
            Hint.LoadTips("评价成功", Color.white);
            //LeftMuneMgr.LFM.clickLeftMenuBtn(LeftMuneMgr.LFM.transform.GetChild(0).gameObject);
        }
        else if (RspCCM.code == 0)
        {
            Hint.LoadTips(RspCCM.tips, Color.white);
        }
    }
    void OnNetRspCM(byte[] buf)
    {
        RspCommentMessage RspCM = PBSerializer.NDeserialize<RspCommentMessage>(buf);
        if (RspCM.rspcmd != 512)
        {
            return;
        }
        if (RspCM.code != 0)
        {
            backAndBack(new GameObject());
            Debug.Log(RspCM.rspcmd);
            //LeftMuneMgr.LFM.clickLeftMenuBtn(LeftMuneMgr.LFM.transform.GetChild(0).gameObject);
            //UIManager.Instance.PushPanel(UIPanelName.dingdanpanel, false, false, paragrm => { LeftMuneMgr.LFM.clickLeftMenuBtn(LeftMuneMgr.LFM.transform.GetChild(0).gameObject); });
        }
        else if (RspCM.code == 0)
        {
            Hint.LoadTips(RspCM.tip, Color.white);
        }
    }
    public void Init()
    {
    }
    public void TuiKuan(Order m_Order)
    {
        Target_Order = m_Order;
        if (m_Order.hasSend == 1)
        {
            IsHasSend = true;
        }
        else
        {
            IsHasSend = false;
        }
        ShenQingWindows.SetActive(true);
        GoodsSingePrice = m_Order.payNum / m_Order.goodsNum;
        ShenQingWindows.transform.Find("Main").Find("TuiKuanJinE").Find("HasSend").Find("InputJinE").GetComponent<InputField>().text="";
        ShenQingWindows.transform.Find("Main").Find("GoodInfo").Find("GoodsImage").GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + m_Order.goodsKindUrl);
        ShenQingWindows.transform.Find("Main").Find("GoodInfo").Find("Text").GetComponent<Text>().text = m_Order.businessName + "/" + m_Order.goodsKindname;
        //ShenQingWindows.transform.Find("Main").Find("GoodsNum").Find("NumberText").GetComponent<Text>().text = m_Order.goodsNum.ToString();
        if (IsHasSend == true)
        {
            ShenQingWindows.transform.Find("Main").Find("TuiKuanJinE").Find("HasSend").gameObject.SetActive(true);
            ShenQingWindows.transform.Find("Main").Find("GoodsNum").Find("HasSend").gameObject.SetActive(true);
            ShenQingWindows.transform.Find("Main").Find("GoodsNum").Find("GoodsAllNumber").gameObject.SetActive(false);
            ShenQingWindows.transform.Find("Main").Find("GoodsNum").Find("HasSend").Find("NumberText").GetComponent<Text>().text = m_Order.goodsNum.ToString();
            //ShenQingWindows.transform.Find("Main").Find("TuiKuanJinE").Find("JinQian").GetComponent<Text>().text = m_Order.payNum.ToString();
            ShenQingWindows.transform.Find("Main").Find("TuiKuanJinE").Find("JinQian").gameObject.SetActive(false);
            ShenQingWindows.transform.Find("Main").Find("TuiKuanJinE").Find("HasSend").Find("InputJinE").GetComponent<InputField>().onEndEdit.AddListener(delegate { EndInput(ShenQingWindows.transform.Find("Main").Find("TuiKuanJinE").Find("HasSend").Find("InputJinE").GetComponent<InputField>()); });
            ShenQingWindows.transform.Find("Main").Find("TuiKuanJinE").Find("HasSend").Find("InputJinE").Find("Placeholder").GetComponent<Text>().text = "可退款" + (m_Order.goodsNum * GoodsSingePrice) + "元";
        }
        else if (IsHasSend == false)
        {
            ShenQingWindows.transform.Find("Main").Find("TuiKuanJinE").Find("HasSend").gameObject.SetActive(false);
            ShenQingWindows.transform.Find("Main").Find("GoodsNum").Find("HasSend").gameObject.SetActive(false);
            ShenQingWindows.transform.Find("Main").Find("GoodsNum").Find("GoodsAllNumber").gameObject.SetActive(true);
            ShenQingWindows.transform.Find("Main").Find("TuiKuanJinE").Find("JinQian").gameObject.SetActive(true);
            ShenQingWindows.transform.Find("Main").Find("GoodsNum").Find("GoodsAllNumber").GetComponent<Text>().text = m_Order.goodsNum.ToString();
            ShenQingWindows.transform.Find("Main").Find("TuiKuanJinE").Find("JinQian").GetComponent<Text>().text = m_Order.payNum+"元";
            ShenQingWindows.transform.Find("Main").Find("GoodsNum").Find("HasSend").Find("NumberText").GetComponent<Text>().text = "";
            RefundMoney = m_Order.goodsNum * GoodsSingePrice;
        }
        ReasonText = ShenQingWindows.transform.Find("Main").Find("InputYuanYin").Find("Text").GetComponent<Text>();
        ShenQingWindows.transform.Find("Main").Find("InputYuanYin").GetComponent<InputField>().text = "";
        ClickListener.Get(ShenQingWindows.transform.Find("Main").Find("GoodsNum").Find("HasSend").Find("ZengJia").gameObject).onClick = clickIncreaseAndDecrease;
        ClickListener.Get(ShenQingWindows.transform.Find("Main").Find("GoodsNum").Find("HasSend").Find("JianShao").gameObject).onClick = clickIncreaseAndDecrease;
        ClickListener.Get(ShenQingWindows.transform.Find("TiJiao").gameObject).onClick = clickTiJiaoTuiKuanBtn;
        ClickListener.Get(ShenQingWindows.transform.Find("BG").Find("BackBtn").gameObject).onClick = clickBackBtn;
    }
    void EndInput(InputField Input)
    {
        if (Input.text == "")
        {
            return;
        }
        double TuiKuanJinE = double.Parse(Input.text);
        if (TuiKuanJinE < 0)
        {
            TuiKuanJinE = (Target_Order.goodsNum * GoodsSingePrice);
        }
        if (TuiKuanJinE >= (Target_Order.goodsNum * GoodsSingePrice))
        {
            TuiKuanJinE = (Target_Order.goodsNum * GoodsSingePrice);
            Input.text = TuiKuanJinE.ToString();
        }
        RefundMoney = TuiKuanJinE;
    }
    void clickTiJiaoTuiKuanBtn(GameObject obj)
    {
        if (ReasonText.text != "")
        {
            ReqApplyRefundMessage m_ReqARM = new ReqApplyRefundMessage();
            Reason m_reason = new Reason();
            m_reason.text.Add(ReasonText.text);
            Target_Order.refundReason = JsonConvert.SerializeObject(m_reason);
            Target_Order.refundNum = RefundNumber;
            Target_Order.refundMoney = RefundMoney;
            m_ReqARM.orderInfo = Target_Order;
            HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqApplyRefundMessage, m_ReqARM);
        }
        else if (ReasonText.text == "")
        {
            Hint.LoadTips("请填写退款原因", Color.white);
        }
    }
    void backAndBack(GameObject obj)
    {
            UIManager.Instance.PopSelf();
        if (UIManager.Instance.IsTopPanel("xiangqingpanel")|| UIManager.Instance.IsTopPanel("tuihuopanel"))
        {
            UIManager.Instance.PopSelf();
            if (LeftMuneMgr.LFM == null)
            {
                LeftMuneMgr.LFM = transform.Find("LeftMenu").GetComponent<LeftMuneMgr>();
                LeftMuneMgr.LFM.LeftMenuPar = LeftMuneMgr.LFM.gameObject;
                LeftMuneMgr.LFM.clickLeftMenuBtn(LeftMuneMgr.LFM.TargetGameObject);
            }
            else
            {
                LeftMuneMgr.LFM.clickLeftMenuBtn(LeftMuneMgr.LFM.TargetGameObject);
            }
        }
        LeftMuneMgr.LFM.clickLeftMenuBtn(LeftMuneMgr.LFM.transform.GetChild(0).gameObject);
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
       //UIManager.Instance.PushPanel(UIPanelName.dingdanpanel, false, false, paragrm => { LeftMuneMgr.LFM.clickLeftMenuBtn(LeftMuneMgr.LFM.transform.GetChild(0).gameObject); });
    }
    void clickTianXieWuLiu(GameObject obj)
    {
        if (TuiHuoBianHaoText.text != "" && TuiHuoTelephoneText.text != "" && TuiHuoShuoMingText.text != "")
        {
            ReqUpdateRefundMessage m_ReqURM = new ReqUpdateRefundMessage();
            Target_Order.refundExpressCode = TuiHuoBianHaoText.text;
            Target_Order.remark = TuiHuoShuoMingText.text;
            m_ReqURM.orderInfo = Target_Order;
            HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdateRefundMessage, m_ReqURM);
        }
        else
        {
            Hint.LoadTips("请填写相关内容", Color.white);
        }
    }
    public void TianXieWuLiu(Order m_Order)
    {
        TianXieWuLiuWindows.SetActive(true);
        Target_Order = m_Order;
        TianXieWuLiuWindows.transform.Find("Main").Find("GoodInfo").Find("GoodsImage").GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + m_Order.goodsKindUrl);
        TianXieWuLiuWindows.transform.Find("Main").Find("GoodInfo").Find("GoodName").GetComponent<Text>().text = m_Order.businessName + "/" + m_Order.goodsKindname;
        TianXieWuLiuWindows.transform.Find("Main").Find("InputYuanYin").GetComponent<InputField>().text = "";
        TuiHuoBianHaoText = TianXieWuLiuWindows.transform.Find("Main").Find("InputYuanYin").Find("Text").GetComponent<Text>();
        TianXieWuLiuWindows.transform.Find("Main").Find("InputTelephone").GetComponent<InputField>().text = "";
        TuiHuoTelephoneText = TianXieWuLiuWindows.transform.Find("Main").Find("InputTelephone").Find("Text").GetComponent<Text>();
        TianXieWuLiuWindows.transform.Find("Main").Find("InputShuoMing").GetComponent<InputField>().text = "";
        TuiHuoShuoMingText = TianXieWuLiuWindows.transform.Find("Main").Find("InputShuoMing").Find("Text").GetComponent<Text>();
        ClickListener.Get(TianXieWuLiuWindows.transform.Find("TiJiao").gameObject).onClick = clickTianXieWuLiu;
        ClickListener.Get(TianXieWuLiuWindows.transform.Find("BG").Find("BackBtn").gameObject).onClick = clickBackBtn;
    }
    public void TuiHuo(Order m_Order)
    {

    }
    void clickIncreaseAndDecrease(GameObject obj)
    {
        int Number = int.Parse(ShenQingWindows.transform.Find("Main").Find("GoodsNum").Find("HasSend").Find("NumberText").GetComponent<Text>().text);
        if (obj.name == "ZengJia")
        {
            if (Number < Target_Order.goodsNum)
            {
                Number++;
            }
            else
            {
                Hint.LoadTips("已经到达上限", Color.white);
            }
        }
        else if (obj.name == "JianShao")
        {
            if (Number > 1)
            {
                Number--;
            }
            else
            {
                Hint.LoadTips("不能再减少啦", Color.white);
            }
        }
        ShenQingWindows.transform.Find("Main").Find("GoodsNum").Find("HasSend").Find("NumberText").GetComponent<Text>().text = Number.ToString();
        //ShenQingWindows.transform.Find("Main").Find("TuiKuanJinE").Find("JinQian").GetComponent<Text>().text = (Number * GoodsSingePrice).ToString();
    }
    public void QueRenShouHuo(Order m_Order)
    {
        Target_Order = m_Order;
        QueDingShouHuoWindows.SetActive(true);
        ClickListener.Get(QueDingShouHuoWindows.transform.Find("TiJiao").gameObject).onClick = clickLiJiPingJia;
        ClickListener.Get(QueDingShouHuoWindows.transform.Find("BG").Find("BackBtn").gameObject).onClick = backAndBack;
    }
    public void clickLiJiPingJia(GameObject obj)
    {
        PingLun(Target_Order);
    }
    public void PingLun(Order m_Order)
    {
        Target_Order = m_Order;
        PingLunWindows.SetActive(true);
        PingLunWindows.transform.Find("Main").Find("GoodInfo").Find("GoodsImage").GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + m_Order.goodsKindUrl);
        PingLunWindows.transform.Find("Main").Find("GoodInfo").Find("GoodName").GetComponent<Text>().text = m_Order.businessName + "/" + m_Order.goodsKindname;
        PingLunWindows.transform.Find("Main").Find("GoodInfo").Find("GoodPrice").GetComponent<Text>().text = m_Order.payNum + "元";
        PingLunWindows.transform.Find("Main").Find("GoodInfo").Find("GoodsNum").GetComponent<Text>().text = "x" + m_Order.goodsNum;
        PingLunWindows.transform.Find("Main").Find("InputField").GetComponent<InputField>().text = "";
        ClickListener.Get(PingLunWindows.transform.Find("TiJiao").gameObject).onClick = clickTiJiaoPingJia;
        PingJiaText = PingLunWindows.transform.Find("Main").Find("InputField").Find("Text").GetComponent<Text>();
        ClickListener.Get(PingLunWindows.transform.Find("BG").Find("BackBtn").gameObject).onClick = clickBackBtn;
        for (int i = 0; i < PingLunWindows.transform.Find("Main").Find("Stars").Find("StarsPar").childCount; i++)
        {
            ClickListener.Get(PingLunWindows.transform.Find("Main").Find("Stars").Find("StarsPar").GetChild(i).gameObject).onClick = clickStar;
        }
        clickStar(PingLunWindows.transform.Find("Main").Find("Stars").Find("StarsPar").GetChild(Score - 1).gameObject);
    }
    public void clickTiJiaoPingJia(GameObject obj)
    {
        if (PingJiaText.text != "")
        {
            ReqCreateCommentsMessage m_ReqCCM = new ReqCreateCommentsMessage();
            m_ReqCCM.comment = new Comment();
            m_ReqCCM.comment.star = Score;
            m_ReqCCM.comment.goodsId = Target_Order.goodsId;
            m_ReqCCM.comment.text = PingJiaText.text;
            m_ReqCCM.comment.accountId = (long)DataMgr.m_account.id;
            m_ReqCCM.comment.accountName = DataMgr.m_account.userName;
            m_ReqCCM.comment.moudleId = (long)DataMgr.m_account.modleId;
            m_ReqCCM.comment.goodsKindId = Target_Order.goodsKindId;
            m_ReqCCM.orderNo = Target_Order.no;
            HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqCreateCommentsMessage, m_ReqCCM);
        }
        else
        {
            Hint.LoadTips("请填写评价信息", Color.white);
        }
    }
    public void ChaKanWuLiu(Order m_Order)
    {
        Target_Order = m_Order;
        ReqGetExpressInfoMessage m_ReqGEIM = new ReqGetExpressInfoMessage();
        m_ReqGEIM.expressCode = m_Order.expressCode;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetExpressInfoMessage, m_ReqGEIM);
    }
    public void clickStar(GameObject obj)
    {
        Score = int.Parse(obj.name);
        for (int i = 0; i < PingLunWindows.transform.Find("Main").Find("Stars").Find("StarsPar").childCount; i++)
        {
            PingLunWindows.transform.Find("Main").Find("Stars").Find("StarsPar").GetChild(i).GetComponent<Image>().color = PublicFunc.StringToColor("B5B6B9");
        }
        for (int i = 0; i < Score; i++)
        {
            PingLunWindows.transform.Find("Main").Find("Stars").Find("StarsPar").GetChild(i).GetComponent<Image>().color = PublicFunc.StringToColor("E6860A");
        }
    }
}
