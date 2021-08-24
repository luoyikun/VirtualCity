using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Framework.UI;
using UnityEngine.UI;
using LitJson;
using ProtoDefine;
using Newtonsoft.Json;
using ProtoBuf.Serializers;
using SGF.Codec;

public class xiangqingpanel : UGUIPanel
{
    public GameObject StateImagePar;
    public GameObject StateBtnPar;
    public Order1List ol;
    public GameObject MainInfoObj;
    public GameObject backBtn;
    public Text UserNameText;
    public Text TelephoneText;
    public Text AddressText;
    public RawImage GoodsImage;
    public Text GoodsInfo;
    public Text GoodsStateText;
    public Text GoodsPriceText;
    public Text GoodsNumText;
    public Text OrderIDText;
    public Text OrderPriceText;
    public Text RealPriceText;
    public Text OrderTimeText;
    public GameObject DiYongObj;
    public Text GouWuJinText;
    public Text CashText;
    Order Target_Order = new Order();

    public Text CountDownText;
    // Use this for initialization
    void Start()
    {
        //AssetMgr.Instance.CreateText("order1list", "order1list", init);
        ClickListener.Get(backBtn).onClick = clickBackBtn;
    }
    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGoodsInfoMessage, OnNetRspGIM);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, OnNetRspCM);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspComfirmReceiptMessage, OnNetRspCRM);
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGoodsInfoMessage, OnNetRspGIM);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, OnNetRspCM);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspComfirmReceiptMessage, OnNetRspCRM);
    }
    void OnNetRspCRM(byte[] buf)
    {
        RspComfirmReceiptMessage RspCRM = PBSerializer.NDeserialize<RspComfirmReceiptMessage>(buf);
        if (RspCRM.code != 0)
        {
            UIManager.Instance.PushPanel(UIPanelName.dingdanwindowpanel, false, true, paragrm => { paragrm.GetComponent<dingdanwindowpanel>().QueRenShouHuo(Target_Order); });
        }
        else if (RspCRM.code == 0)
        {
            Hint.LoadTips(RspCRM.tips, Color.white);
        }
    }
    void OnNetRspCM(byte[] buf)
    {
        RspCommentMessage RspCM = PBSerializer.NDeserialize<RspCommentMessage>(buf);
        if (RspCM.rspcmd != 508)
        {
            return;
        }
        if (RspCM.code != 0)
        {
            //UIManager.Instance.
            UIManager.Instance.PushPanel(UIPanelName.dingdanwindowpanel, false, true, paragrm => { paragrm.GetComponent<dingdanwindowpanel>().QueRenShouHuo(Target_Order); });
        }
        else if (RspCM.code == 0)
        {
            Hint.LoadTips(RspCM.tip, Color.white);
        }
    }
    void OnNetRspGIM(byte[] buf)
    {
        RspGoodsInfoMessage RspQBM = PBSerializer.NDeserialize<RspGoodsInfoMessage>(buf);
        if (RspQBM.code == 0)
        {
            Hint.LoadTips(RspQBM.tips, Color.white);
            return;
        }
        UIManager.Instance.PushPanel(UIPanelName.goodsdetailspanel, false, false, paragm => { paragm.GetComponent<goodsdetailspanel>().Init(RspQBM.goods); });
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf(true);
    }
    public void init(Order order)
    {
        Target_Order = order;
        Address m_Address = JsonConvert.DeserializeObject<Address>(order.expressInfo);
        //ol = JsonUtility.FromJson<Order1List>(text);
        initStateImage(int.Parse(order.orderStatus));
        GoodsImage.GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + order.goodsKindUrl);
        ClickListener.Get(GoodsImage.gameObject).onClick = clickJumpToGoods;
        UserNameText.text = m_Address.m_RA.Name;
        TelephoneText.text = order.customerTel;
        AddressText.text = m_Address.m_RA.ProvinceName + "-" + m_Address.m_RA.CityName + "-" + m_Address.m_RA.ExpAreaName + "-" + m_Address.m_RA.Address;
        GoodsInfo.text = order.businessName + "/" + order.goodsKindname;
        initStateText(order.orderStatus);
        GoodsPriceText.text = (order.payNum / order.goodsNum) + "元";
        GoodsNumText.text = "x" + order.goodsNum.ToString();
        OrderIDText.text = order.no.ToString();
        OrderPriceText.text = order.payNum.ToString("0.00");
        RealPriceText.text = (order.payNum - order.paySMoney-order.payMoney).ToString("0.00");
        OrderTimeText.text = order.createtime;
        GouWuJinText.text = order.paySMoney.ToString("0.00");
        CashText.text = order.payMoney.ToString("0.00");
        if (order.paySMoney != 0)
        {
            DiYongObj.transform.Find("GouWuJin").gameObject.SetActive(true);
            DiYongObj.transform.Find("GouWuJin").Find("Text").GetComponent<Text>().text = "-" + order.paySMoney + "元";
        }
        else if (order.paySMoney == 0)
        {
            DiYongObj.transform.Find("GouWuJin").gameObject.SetActive(false);
        }
        if (order.payMoney != 0)
        {
            DiYongObj.transform.Find("Cash").gameObject.SetActive(true);
            DiYongObj.transform.Find("Cash").Find("Text").GetComponent<Text>().text = "-" + order.payMoney + "元";
        }
        else if (order.payMoney == 0)
        {
            DiYongObj.transform.Find("Cash").gameObject.SetActive(false);
        }
    }
    void clickJumpToGoods(GameObject obj)
    {
        ReqGoodsInfoMessage ReqGIM = new ReqGoodsInfoMessage();
        ReqGIM.goodsId = Target_Order.goodsId;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqGoodsInfoMessage, ReqGIM);
        // UIManager.Instance.PushPanel(UIPanelName.goodsdetailspanel, false, false, paragm => {/* paragm.GetComponent<goodsdetailspanel>().Init();*/ }); 
    }
    void initStateText(string state)
    {
        switch (state)
        {
            case "0":
                GoodsStateText.text = "已下单待付款";
                break;
            case "1":
                GoodsStateText.text = "已付款待发货";
                break;
            case "2":
                GoodsStateText.text = "已发货待签收";
                break;
            case "3":
                GoodsStateText.text = "已发货待签收";
                break;
            case "4":
                GoodsStateText.text = "确认收货";
                break;
            case "5":
                GoodsStateText.text = "退货申请中，待审批";
                break;
            case "6":
                if (Target_Order.gmApproveResault == 0)
                {
                    GoodsStateText.text = "退货申请已被驳回";
                }
                break;
        }
    }
    void initMainInfo(int state)
    {
        if (state > 1)
        {
            DiYongObj.gameObject.SetActive(true);
        }
        else if (state <= 1)
        {
            DiYongObj.gameObject.SetActive(false);
        }
        initStateImage(state);
    }
    void initStateImage(int state)
    {
        switch (state)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 12:
                for (int i = 0; i < StateImagePar.transform.GetChild(0).childCount; i++)
                {
                    if (i <= state)
                    {
                        StateImagePar.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
                        StateImagePar.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
                    }
                    else if (i > state)
                    {
                        StateImagePar.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                        StateImagePar.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                    }
                }
                break;
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
                if (Target_Order.hasSend == 0)
                {
                    for (int i = 0; i < StateImagePar.transform.GetChild(0).childCount; i++)
                    {
                        if (i <= 1)
                        {
                            StateImagePar.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
                            StateImagePar.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
                        }
                        else if (i > 1)
                        {
                            StateImagePar.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                            StateImagePar.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
                else if (Target_Order.hasSend == 1)
                {
                    for (int i = 0; i < StateImagePar.transform.GetChild(0).childCount; i++)
                    {
                        if (i <= 2)
                        {
                            StateImagePar.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
                            StateImagePar.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
                        }
                        else if (i > 2)
                        {
                            StateImagePar.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                            StateImagePar.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
                break;
        }
        initStateBtn(state);

    }
    void initStateBtn(int state)
    {
        for (int i = 0; i < StateBtnPar.transform.childCount; i++)
        {
            StateBtnPar.transform.GetChild(i).gameObject.SetActive(false);
        }
        switch (state)
        {
            case 0:
                StateBtnPar.transform.GetChild(state).gameObject.SetActive(true);
                break;
            case 1:
                StateBtnPar.transform.GetChild(state).gameObject.SetActive(true);
                ClickListener.Get(StateBtnPar.transform.GetChild(state).Find("RefundBtn").gameObject).onClick = clickTuiKuanBtn;
                break;
            case 2:
                StateBtnPar.transform.GetChild(state).gameObject.SetActive(true);
                ClickListener.Get(StateBtnPar.transform.GetChild(state).Find("ConfrimBtn").gameObject).onClick = clickQueRenShouHuoBtn;
                ClickListener.Get(StateBtnPar.transform.GetChild(state).Find("RefundBtn").gameObject).onClick = clickTuiKuanBtn;
                StateBtnPar.transform.GetChild(2).Find("RefundBtn").Find("Text").GetComponent<Text>().text = "申请退货";
                ClickListener.Get(StateBtnPar.transform.GetChild(state).Find("WuLiuBtn").gameObject).onClick = clickWuLiuBtn;

                string CurrentTime = DateTime.Now.ToString("G");

                DateTime m_Time = Convert.ToDateTime(CurrentTime);

                DateTime Target_Time = Convert.ToDateTime(Target_Order.sendTime);

                Target_Time=Target_Time.AddDays(15);

                TimeSpan ts = Target_Time - m_Time;

                CountDownText.text = "还剩"+ts.Days+"天"+ts.Hours+"时帮您自动确认";
                break;
            case 3:
            case 4:
                StateBtnPar.transform.GetChild(3).gameObject.SetActive(true);
                ClickListener.Get(StateBtnPar.transform.GetChild(3).Find("EvaluateBtn").gameObject).onClick = clickEvaluateBtn;
                break;
            case 6:
                if (Target_Order.gmApproveResault == 0)
                {
                    StateBtnPar.transform.GetChild(2).gameObject.SetActive(true);
                    ClickListener.Get(StateBtnPar.transform.GetChild(2).Find("ConfrimBtn").gameObject).onClick = clickQueRenShouHuoBtn;
                    ClickListener.Get(StateBtnPar.transform.GetChild(2).Find("RefundBtn").gameObject).onClick = clickTuiKuanBtn;
                    StateBtnPar.transform.GetChild(2).Find("RefundBtn").Find("Text").GetComponent<Text>().text = "重新申请";
                    ClickListener.Get(StateBtnPar.transform.GetChild(2).Find("WuLiuBtn").gameObject).onClick = clickWuLiuBtn;

                    CurrentTime = DateTime.Now.ToString("G");

                    m_Time = Convert.ToDateTime(CurrentTime);

                    Target_Time = Convert.ToDateTime(Target_Order.sendTime);

                    Target_Time = Target_Time.AddDays(15);

                    ts = Target_Time - m_Time;

                    CountDownText.text = "还剩" + ts.Days + "天" + ts.Hours + "时帮您自动确认";
                }
                break;
            case 5:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
                StateBtnPar.transform.Find("4").gameObject.SetActive(true);
                ClickListener.Get(StateBtnPar.transform.Find("4").Find("TuiKuanXiangQingBtn").gameObject).onClick = clickTuiKuanXiangQingBtn;
                break;
            case 12:
                break;
        }
    }
    void clickEvaluateBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.dingdanwindowpanel, false, true, paragrm => { paragrm.GetComponent<dingdanwindowpanel>().PingLun(Target_Order); });
    }
    void clickWuLiuBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.dingdanwindowpanel, false, true, paragrm => { paragrm.GetComponent<dingdanwindowpanel>().ChaKanWuLiu(Target_Order); });
    }
    void clickQueRenShouHuoBtn(GameObject obj)
    {
        ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, false);
        ispanel.SetContent("提示", "是否要确认收货？");
        ispanel.m_ok = () => { dingdanpanel.ddp.SendQueRenDingDan(Target_Order); };
        //UIManager.Instance.PushPanel(UIPanelName.dingdanwindowpanel, false, true, paragrm => { paragrm.GetComponent<dingdanwindowpanel>().QueRenShouHuo(Target_Order); });
    }
    void clickTuiKuanXiangQingBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
        UIManager.Instance.PushPanel(UIPanelName.tuihuopanel, false, false, paragrm => { paragrm.GetComponent<tuihuopanel>().init(Target_Order); });
    }
    void clickTuiKuanBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.dingdanwindowpanel, false, true, paragrm => { paragrm.GetComponent<dingdanwindowpanel>().TuiKuan(Target_Order); });
    }
}
