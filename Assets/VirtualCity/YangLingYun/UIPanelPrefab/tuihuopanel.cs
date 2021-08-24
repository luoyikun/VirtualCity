using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using ProtoDefine;
using Newtonsoft.Json;
using SGF.Codec;

public class RefundAddress
{
    public string Name="";
    public long Mobile;
    public string Address="";
    public string CityName="";
    public string ExpAreaName="";
    public string ProvinceName="";
}
public class tuihuopanel : UGUIPanel
{
    public GameObject StateImagePar;
    public GameObject StateBtnPar;
    // public Order1List ol;
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
    public Text GouWuJinText;
    public Text CashText;
    Order Target_Order = new Order();

    public Text CountDownText;
    // Use this for initialization
    void Start()
    {
        ClickListener.Get(backBtn).onClick = clickBackBtn;
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf(true);
    }
    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGoodsInfoMessage, OnNetRspGIM);
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGoodsInfoMessage, OnNetRspGIM);
    }
    void OnNetRspGIM(byte[] buf)
    {
        RspGoodsInfoMessage RspQBM = PBSerializer.NDeserialize<RspGoodsInfoMessage>(buf);
        UIManager.Instance.PushPanel(UIPanelName.goodsdetailspanel, false, false, paragm => { paragm.GetComponent<goodsdetailspanel>().Init(RspQBM.goods); });
    }
    public void init(Order order)
    {
        Target_Order = order;
        initMainInfo(int.Parse(order.orderStatus));
        GoodsImage.GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + order.goodsKindUrl);
        if (order.hasSend == 1)
        {
            RefundAddress m_RefundAddress = JsonConvert.DeserializeObject<RefundAddress>(order.refundExpressInfo);

            TelephoneText.text = m_RefundAddress.Mobile.ToString();
            AddressText.text = m_RefundAddress.ProvinceName + "-" + m_RefundAddress.CityName + "-" + m_RefundAddress.ExpAreaName + "-" + m_RefundAddress.Address;
            TelephoneText.gameObject.SetActive(true);
            AddressText.gameObject.SetActive(true);
            UserNameText.text = order.businessName;
            UserNameText.gameObject.SetActive(true);
            MainInfoObj.transform.Find("Text").gameObject.SetActive(true);
        }
        else if (order.hasSend == 0)
        {
            TelephoneText.gameObject.SetActive(false);
            AddressText.gameObject.SetActive(false);
            UserNameText.gameObject.SetActive(false);
            MainInfoObj.transform.Find("Text").gameObject.SetActive(false);
        }
        UserNameText.text = order.businessName;
        GoodsInfo.text = order.businessName + "/" + order.goodsKindname;
        initStateText(order.orderStatus);
        GoodsPriceText.text = (order.payNum / order.goodsNum).ToString() + "元";
        GoodsNumText.text = "x" + order.goodsNum.ToString();
        OrderIDText.text = order.no.ToString();
        OrderPriceText.text = order.payNum.ToString();
        RealPriceText.text = (order.payNum - order.paySMoney - order.payMoney).ToString("0.00");
        OrderTimeText.text = order.createtime;
        GouWuJinText.text = "-" + order.paySMoney.ToString("0.00");
        ClickListener.Get(GoodsImage.gameObject).onClick = clickJumpToGoods;
        if (order.paySMoney == 0)
        {
            GouWuJinText.transform.parent.gameObject.SetActive(false);
        }
        CashText.text = "-" + order.payMoney.ToString("0.00");
        if (order.payMoney == 0)
        {
            CashText.transform.parent.gameObject.SetActive(false);
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
            case "5":
                GoodsStateText.text = "退货申请中待审批";
                break;
            case "6":
                GoodsStateText.text = "退款审批完成";
                break;
            case "7":
                GoodsStateText.text = "客户已发货待商家确认";
                break;
            case "8":
                GoodsStateText.text = "商家确认收货待退款";
                break;
            case "9":
                GoodsStateText.text = "已退款";
                break;
        }
    }
    void initMainInfo(int state)
    {
        initStateImage(state);
    }
    void initStateImage(int state)
    {
        //StateImagePar.transform.GetChild(0).Find(state.ToString()).gameObject.SetActive(false);
        //StateImagePar.transform.GetChild(1).Find(state.ToString()).gameObject.SetActive(true);
        if (Target_Order.hasSend == 0)
        {
            StateImagePar.transform.GetChild(0).gameObject.SetActive(true);
            StateImagePar.transform.GetChild(1).gameObject.SetActive(false);
            switch (state)
            {
                case 5:
                case 6:
                case 7:
                case 8:
                    StateImagePar.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    StateImagePar.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                    break;
                case 9:
                    StateImagePar.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                    StateImagePar.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    break;
            }
        }
        else if (Target_Order.hasSend == 1)
        {
            StateImagePar.transform.GetChild(0).gameObject.SetActive(false);
            StateImagePar.transform.GetChild(1).gameObject.SetActive(true);
            for (int i = 0; i < StateImagePar.transform.GetChild(1).childCount; i++)
            {
                StateImagePar.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
            }
            switch (state)
            {
                case 5:
                case 6:
                    StateImagePar.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                    break;
                case 7:
                case 8:
                    StateImagePar.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                    break;
                case 9:
                    StateImagePar.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
                    break;
            }
        }
        //for (int i = 0; i < StateImagePar.transform.GetChild(0).childCount; i++)
        //{
        //    if (i <= state-5)
        //    {
        //        StateImagePar.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        //        StateImagePar.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
        //    }
        //    else if (i > state-5)
        //    {
        //        StateImagePar.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
        //        StateImagePar.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
        //    }
        //}
        initStateBtn(state);
    }
    void initStateBtn(int state)
    {
        for (int i = 0; i < StateBtnPar.transform.childCount; i++)
        {
            StateBtnPar.transform.GetChild(i).gameObject.SetActive(false);
        }
        StateBtnPar.transform.Find(state.ToString()).gameObject.SetActive(true);
        switch (state)
        {
            case 5:
                //StateBtnPar.transform.Find(state.ToString()).gameObject.SetActive(true);
                if (Target_Order.hasSend == 0)
                {
                    StateBtnPar.transform.Find(state.ToString()).Find("TuiHuoText").gameObject.SetActive(false);
                    StateBtnPar.transform.Find(state.ToString()).Find("TuiKuanText").gameObject.SetActive(true);
                }
                else if (Target_Order.hasSend == 1)
                {
                    StateBtnPar.transform.Find(state.ToString()).Find("TuiHuoText").gameObject.SetActive(true);
                    StateBtnPar.transform.Find(state.ToString()).Find("TuiKuanText").gameObject.SetActive(false);
                }
                break;
            case 6:
                //StateBtnPar.transform.Find(state.ToString()).gameObject.SetActive(true);
                ClickListener.Get(StateBtnPar.transform.Find(state.ToString()).Find("WuLiuBtn").gameObject).onClick = clickTianXieWuLiu;
                break;
            case 7:
                string CurrentTime = DateTime.Now.ToString("G");

                DateTime m_Time = Convert.ToDateTime(CurrentTime);

                DateTime Target_Time = Convert.ToDateTime(Target_Order.refundSendTime);

                Target_Time = Target_Time.AddDays(15);

                TimeSpan ts = Target_Time - m_Time;

                CountDownText.text = "还剩" + ts.Days + "天" + ts.Hours + "时商家自动确认";
                //StateBtnPar.transform.Find(state.ToString()).gameObject.SetActive(true);
                break;
            case 8:
                //StateBtnPar.transform.Find(state.ToString()).gameObject.SetActive(true);
                break;
            case 9:
                //StateBtnPar.transform.Find(state.ToString()).gameObject.SetActive(true);
                ClickListener.Get(StateBtnPar.transform.Find(state.ToString()).Find("KeFuBtn").gameObject).onClick = clicklianxiKeFu;
                break;
        }
    }
    void clickTianXieWuLiu(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.dingdanwindowpanel, false, true, paragrm => { paragrm.GetComponent<dingdanwindowpanel>().TianXieWuLiu(Target_Order); });
    }

    void clicklianxiKeFu(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.kefupanel, false, false, (param) => { }, true);
    }
}
