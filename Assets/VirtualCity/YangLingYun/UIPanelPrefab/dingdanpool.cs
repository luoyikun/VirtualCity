using Framework.UI;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dingdanpool : MonoBehaviour
{
    //public ScrollCallback scb;
    public Text GoodsPriceText;
    public Text GoodsNumText;
    public Text GoodsInfoText;
    public Text StateText;
    public Text GoodsSimglePriceText;
    public Text TimeText;
    // public RawImage GoodsRawImage;
    public GameObject BtnPar;
    public GameObject WuLiuBtn;
    public GameObject TuiHuoBtn;
    public GameObject CancelBtn;
    public GameObject RefundBtn;
    public GameObject QueRenBtn;
    public GameObject PaymentBtn;
    public GameObject EvaluateBtn;
    public GameObject TuiKuanBtn;
    public string state = "0";
    public Order Target_Order;
    bool IsHasImage = false;
    // Use this for initialization
    private void Awake()
    {
        GoodsPriceText = this.transform.Find("Text").GetComponent<Text>();
        GoodsNumText = this.transform.Find("GoodsQuntityText").GetComponent<Text>();
        GoodsInfoText = this.transform.Find("GoodsInfoText").GetComponent<Text>();
        StateText = this.transform.Find("StateText").GetComponent<Text>();
        GoodsSimglePriceText = this.transform.Find("PriceText").GetComponent<Text>();
        TimeText = this.transform.Find("TimeText").GetComponent<Text>();
        BtnPar = this.transform.Find("BtnPar").gameObject;
        WuLiuBtn = BtnPar.transform.Find("WuLiuBtn").gameObject;
        TuiHuoBtn = BtnPar.transform.Find("TuiHuoBtn").gameObject;
        CancelBtn = BtnPar.transform.Find("CancelBtn").gameObject;
        RefundBtn = BtnPar.transform.Find("RefundBtn").gameObject;
        QueRenBtn = BtnPar.transform.Find("QueRenBtn").gameObject;
        PaymentBtn = BtnPar.transform.Find("PaymentBtn").gameObject;
        EvaluateBtn = BtnPar.transform.Find("EvaluateBtn").gameObject;
        TuiKuanBtn = BtnPar.transform.Find("TuiKuanBtn").gameObject;
        ClickListener.Get(RefundBtn).onClick = clickRefundBtn;
        ClickListener.Get(WuLiuBtn).onClick = clickWuLiuBtn;
        ClickListener.Get(TuiHuoBtn).onClick = clickTuiHuoBtn;
        ClickListener.Get(CancelBtn).onClick = clickCancelBtn;
        ClickListener.Get(QueRenBtn).onClick = clickQueRenBtn;
        ClickListener.Get(PaymentBtn).onClick = clickPaymentBtn;
        ClickListener.Get(EvaluateBtn).onClick = clickEvaluateBtn;
        ClickListener.Get(TuiKuanBtn).onClick = clickTuiKuanBtn;
        //scb.callback = Dingdancallback;
    }
    public void Dingdancallback(Order order)
    {
        Target_Order = order;
        ClickListener.Get(gameObject).onClick = clickGoods;
        this.transform.Find("GoodsImage").GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + order.goodsKindUrl);
        //Debug.Log(order.id+order.goodsKindUrl);
        //GoodsRawImage.transform.GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + order.goodsKindUrl);
        //ClickListener.Get(GoodsRawImage).onClick = clickJumpToGoods;
        gameObject.name = order.id.ToString();
        state = order.orderStatus;
        TimeText.text = order.createtime;
        // GoodsInfoText.text= order.goodsKindname+order.goodsKindId;
        GoodsInfoText.text = order.businessName + "/" + order.goodsKindname;
        GoodsNumText.text = "x" + order.goodsNum;
        GoodsPriceText.text = order.payNum + "元";
        GoodsSimglePriceText.text = (order.payNum / order.goodsNum) + "元";
        for (int i = 0; i < BtnPar.transform.childCount; i++)
        {
            BtnPar.transform.GetChild(i).gameObject.SetActive(false);
        }
        switch (state)
        {
            case "0":
                StateText.text = "已下单待付款";
                PaymentBtn.gameObject.SetActive(true);
                CancelBtn.gameObject.SetActive(true);
                break;
            case "1":
                StateText.text = "已付款，待发货";
                RefundBtn.gameObject.SetActive(true);
                break;
            case "2":
                StateText.text = "已发货，待签收";
                WuLiuBtn.gameObject.SetActive(true);
                TuiHuoBtn.gameObject.SetActive(true);
                QueRenBtn.gameObject.SetActive(true);
                break;
            case "3":
                StateText.text = "已签收，待确认";
                WuLiuBtn.gameObject.SetActive(true);
                TuiHuoBtn.gameObject.SetActive(true);
                TuiHuoBtn.transform.Find("Text").GetComponent<Text>().text = "申请退货";
                QueRenBtn.gameObject.SetActive(true);
                break;
            case "4":
                StateText.text = "确认收货，待评论";
                EvaluateBtn.gameObject.SetActive(true);
                break;
            case "5":
                StateText.text = "退货申请中，待审批";
                TuiKuanBtn.gameObject.SetActive(true);
                break;
            case "6":
                if (Target_Order.gmApproveResault == 0)
                {
                    StateText.text = "退款审批被驳回";
                    WuLiuBtn.gameObject.SetActive(true);
                    TuiHuoBtn.gameObject.SetActive(true);
                    TuiHuoBtn.transform.Find("Text").GetComponent<Text>().text = "重新申请";
                    QueRenBtn.gameObject.SetActive(true);

                }
                else if (Target_Order.gmApproveResault == 1)
                {
                    StateText.text = "退款审批完成,请将物品邮寄给商家";
                    TuiKuanBtn.gameObject.SetActive(true);
                }
                break;
            case "7":
                StateText.text = "客户已发货，待商家确认";
                TuiKuanBtn.gameObject.SetActive(true);
                break;
            case "8":
                StateText.text = "商家确认收货，待退款";
                TuiKuanBtn.gameObject.SetActive(true);
                break;
            case "9":
                StateText.text = "已退款";
                TuiKuanBtn.gameObject.SetActive(true);
                break;
            case "12":
                StateText.text = "已评论";
                break;
        }
    }
    void clickJumpToGoods(GameObject obj)
    {

    }
    void clickRefundBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.dingdanwindowpanel, false, true, paragrm => { paragrm.GetComponent<dingdanwindowpanel>().TuiKuan(Target_Order); });
    }
    void clickWuLiuBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.dingdanwindowpanel, false, true, paragrm => { paragrm.GetComponent<dingdanwindowpanel>().ChaKanWuLiu(Target_Order); });
    }
    void clickTuiHuoBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.dingdanwindowpanel, false, true, paragrm => { paragrm.GetComponent<dingdanwindowpanel>().TuiKuan(Target_Order); });
    }
    void clickCancelBtn(GameObject obj)
    {

    }
    void clickQueRenBtn(GameObject obj)
    {
        ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, false);
        ispanel.SetContent("提示", "是否要确认收货？");
        ispanel.m_ok = () => { dingdanpanel.ddp.SendQueRenDingDan(Target_Order); };
    }
    void clickPaymentBtn(GameObject obj)
    {

    }
    void clickEvaluateBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.dingdanwindowpanel, false, true, paragrm => { paragrm.GetComponent<dingdanwindowpanel>().PingLun(Target_Order); });
    }
    void clickTuiKuanBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.tuihuopanel, false, false, paragrm => { paragrm.GetComponent<tuihuopanel>().init(Target_Order); });
    }
    void clickGoods(GameObject obj)
    {
        switch (state)
        {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
                UIManager.Instance.PushPanel(UIPanelName.xiangqingpanel, false, false, paragrm => { paragrm.GetComponent<xiangqingpanel>().init(Target_Order); });
                break;
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
            case "12":
                UIManager.Instance.PushPanel(UIPanelName.xiangqingpanel, false, false, paragrm => { paragrm.GetComponent<xiangqingpanel>().init(Target_Order); });
                // UIManager.Instance.PushPanel(UIPanelName.tuihuopanel,false,false,paragrm=> { paragrm.GetComponent<tuihuopanel>().init(Target_Order); });
                break;
        }
    }
}
