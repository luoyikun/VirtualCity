using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using ProtoDefine;
using SGF.Codec;
using Newtonsoft.Json;
using SuperScrollView;
using Framework.Event;

public class ShoppingCartDic
{
    public long goodsId;
    public int number;
}
public class goodsdetailspanel : UGUIPanel
{
    public GameObject Starts;
    public GameObject GuiGeBtn;
    public GameObject KongBaiObj;
    public GameObject PingLunBtn;
    public GameObject MainPar;
    public GameObject InfoPicture;
    public GameObject ChaKanShoppingCartBtn;
    public GameObject ChatZhangGuiBtn;
    public GameObject PlaceOrderBtn;
    public GameObject AddShoppingCartBtn;
    public GameObject BackBtn;
    public GameObject d_img;
    public GameObject obj_Tips;
    float m_xangles = 0.0f;
    public static goodsdetailspanel gdp;
    public LoopListView2 ScrollView;
    bool IsScrollViewInit = false;
    int TotalCount = 0;
    public Text GoodsPriceText;
    public Text FenLeiText;
    public Text GoodName;
    public Text GoodInfo;
    public Text GoodPrice;
    public string GuiGeInfo = "";
    public long GoodsID;
    public Text PingJiaShuText;
    public Text ManYiDuText;
    public GameObject PingLunObj;
    public Text TuPianNumberText;
    Goods goods = new Goods();
    GoodsKind goodsKind = new GoodsKind();
    public List<GoodsKind> m_goodsKindList = new List<GoodsKind>();
    List<Comment> m_CommentList = new List<Comment>();

    public static List<PartProperties> m_goodsId;

    //public List<string> m_IP = new List<string>();
    public int GoodsNumbers;
    double minscale;
    double maxscale;

    public GoodsKind Target_GoodsKind = new GoodsKind();
    ShoppingCartDic Target_Dic = new ShoppingCartDic();
    //public Dictionary<long, ShoppingCartDic> Target_Dic = new Dictionary<long, ShoppingCartDic>();
    public Dictionary<long, ShoppingCartDic> m_Dic = new Dictionary<long, ShoppingCartDic>();
    Dictionary<long, ShoppingCartDic> DataMgrAccountDic = new Dictionary<long, ShoppingCartDic>();
    bool IsAddGoods = false;
    // Use this for initialization
    void Start()
    {
        if (m_goodsId == null)
        {
            m_goodsId = new List<PartProperties>();
            foreach (var item in DataMgr.partProperties)
            {
                // Debug.Log(item.goodsKindId.ToString());
                if (item.goodsKindId != 0f)
                {
                    m_goodsId.Add(item);
                    // Debug.Log(item.Value.id + item.Value.goodsKindId);
                }
            }
        }

        gdp = this;
        //ClickListener.Get(GuiGeBtn).onClick = clickGuiGeBtn;
        ClickListener.Get(ChaKanShoppingCartBtn).onClick = clickChaKanShoppingCart;
        //ClickListener.Get(AddShoppingCartBtn).onClick = clickAddShoppingCart;
        //ClickListener.Get(PlaceOrderBtn).onClick = clickPlaceOrder; 
        ClickListener.Get(ChatZhangGuiBtn).onClick = clickChatZhangGui;
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        ClickListener.Get(d_img).onClick = ShowModel;
    }
    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGoodsInfoMessage, OnNetRspGIM);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, OnNetRspCM);
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGoodsInfoMessage, OnNetRspGIM);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, OnNetRspCM);
    }

    //public override void OnPause(bool isHide = false, bool isAcceptMsg = false)
    //{

    //}
    void OnNetRspCM(byte[] buf)
    {
        RspCommentMessage RspCM = PBSerializer.NDeserialize<RspCommentMessage>(buf);
        if (RspCM.rspcmd == 511)
        {
            if (RspCM.code != 0)
            {
                DataMgr.m_account.goodsList = JsonConvert.SerializeObject(DataMgrAccountDic);
                Hint.LoadTips("添加购物车成功", Color.white);
            }
            else if (RspCM.code == 0)
            {
                Hint.LoadTips(RspCM.tip, Color.white);
            }
        }
    }
    void OnNetRspGIM(byte[] buf)
    {
        RspGoodsInfoMessage RspQBM = PBSerializer.NDeserialize<RspGoodsInfoMessage>(buf);

        List<bool> goodsNumberBoolList = new List<bool>();//用来记录这件商品里所有规格的数量是否为0

        m_goodsKindList=new List<GoodsKind>();
        if(RspQBM.goodsKinds!=null)
        m_goodsKindList = RspQBM.goodsKinds;

        m_CommentList = new List<Comment>();

        if(RspQBM.comments!=null)
        m_CommentList = RspQBM.comments;
        if (RspQBM.code != 0)
        {
            if (RspQBM.goodsKinds != null)
            {
                for (int i = 0; i < RspQBM.goodsKinds.Count; i++)
                {
                    if (RspQBM.goodsKinds[i].number == 0)
                    {
                        goodsNumberBoolList.Add(false);//如果有某件商品的数量等于0，就用false表示
                    }
                    else if (RspQBM.goodsKinds[i].number > 0)
                    {
                        goodsNumberBoolList.Add(true);
                    }
                }
                if (goodsNumberBoolList.Contains(true))
                {//如果所有规格里面至少包含一个数量大于0的规格
                    IsHasGuiGe(true);
                }
                else if (!goodsNumberBoolList.Contains(true))
                {//如果所有规格里面都不包含数量大于0的规格
                    IsHasGuiGe(false);
                }
                TotalCount = RspQBM.goodsKinds.Count;
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
            }
            if (RspQBM.comments != null)
            {
                //IsHasPingLun(true);
                PingLunObj.SetActive(true);
                PingLunBtn.GetComponent<Button>().enabled = true;
                PingLunBtn.transform.parent.Find("Mask").gameObject.SetActive(false);
                ClickListener.Get(PingLunBtn).onClick = clickPingLunBtn;
                PingLunObj.transform.Find("UserName").GetComponent<Text>().text = RspQBM.comments[0].accountName;
                PublicFunc.CreateHeadImg(PingLunObj.transform.Find("UserHeadImage").GetComponent<Image>(), RspQBM.comments[0].moudleId);
                for (int i = 0; i < RspQBM.comments[0].star; i++)
                {
                    PingLunObj.transform.Find("Stars").transform.GetChild(i).GetComponent<Image>().color = PublicFunc.StringToColor("E6860A");
                }
                PingLunObj.transform.Find("Text").GetComponent<Text>().text = RspQBM.comments[0].text;
            }
            else if (RspQBM.comments == null)
            {
                //IsHasPingLun(false);
                
                PingLunObj.SetActive(false);
                PingLunBtn.GetComponent<Button>().enabled = false;
                PingLunBtn.transform.parent.Find("Mask").gameObject.SetActive(true);
            }
        }
        else if (RspQBM.code == 0)
        {
            Hint.LoadTips(RspQBM.tips, Color.white);
        }
    }
    void IsHasGuiGe(bool IsHas = false)
    {
        if (IsHas == false)
        {
            GuiGeBtn.GetComponent<Image>().raycastTarget = false;
            //GuiGeBtn.transform.Find("LeftText").GetComponent<Text>().text = "暂无可选规格";
            GuiGeBtn.transform.Find("LeftText").gameObject.SetActive(false);
            GuiGeBtn.transform.Find("JianTou").gameObject.SetActive(false);
            GuiGeBtn.transform.Find("Text").gameObject.SetActive(false);
            GuiGeBtn.transform.Find("Mask").gameObject.SetActive(true);
            AddShoppingCartBtn.GetComponent<Image>().raycastTarget = false;
            AddShoppingCartBtn.GetComponent<Image>().color = PublicFunc.StringToColor("969696");
            AddShoppingCartBtn.transform.Find("Text").GetComponent<Text>().color = PublicFunc.StringToColor("969696");
            PlaceOrderBtn.GetComponent<Image>().raycastTarget = false;
            PlaceOrderBtn.GetComponent<Image>().color = PublicFunc.StringToColor("969696");
            PlaceOrderBtn.transform.Find("Text").GetComponent<Text>().color = PublicFunc.StringToColor("969696");
        }
        else if (IsHas == true)
        {
            GuiGeBtn.GetComponent<Image>().raycastTarget = true;
            //GuiGeBtn.transform.Find("LeftText").GetComponent<Text>().text = "已选：";
            GuiGeBtn.transform.Find("LeftText").gameObject.SetActive(true);
            GuiGeBtn.transform.Find("JianTou").gameObject.SetActive(true);
            GuiGeBtn.transform.Find("Text").gameObject.SetActive(true);
            GuiGeBtn.transform.Find("Mask").gameObject.SetActive(false);
            ClickListener.Get(GuiGeBtn).onClick = clickGuiGeBtn;
            AddShoppingCartBtn.GetComponent<Image>().raycastTarget = true;
            AddShoppingCartBtn.GetComponent<Image>().color = PublicFunc.StringToColor("FFFFFF");
            AddShoppingCartBtn.transform.Find("Text").GetComponent<Text>().color = PublicFunc.StringToColor("FFFFFF");
            ClickListener.Get(AddShoppingCartBtn).onClick = clickAddShoppingCart;
            PlaceOrderBtn.GetComponent<Image>().raycastTarget = true;
            PlaceOrderBtn.GetComponent<Image>().color = PublicFunc.StringToColor("FFFFFF");
            PlaceOrderBtn.transform.Find("Text").GetComponent<Text>().color = PublicFunc.StringToColor("FFFFFF");
            ClickListener.Get(PlaceOrderBtn).onClick = clickPlaceOrder;
        }
    }
    void IsHasPingLun(bool IsHas = false)
    {
        if (IsHas == false)
        {

        }
        else if (IsHas == true)
        {

        }
    }
    public void Init(Goods m_Good)
    {
        goods = m_Good;
        ReqGoodsInfoMessage ReqGIM = new ReqGoodsInfoMessage();
        
        InitMain(JsonConvert.DeserializeObject<List<string>>(m_Good.infoPicture));
        ReqGIM.goodsId = m_Good.id;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqGoodsInfoMessage, ReqGIM);
        GoodName.text = m_Good.name;
        GoodInfo.text = m_Good.infoText;
        GoodPrice.text = m_Good.priceMin.ToString();
        GuiGeBtn.transform.Find("Text").GetComponent<Text>().text = GuiGeInfo;
        PingJiaShuText.text = "（" + m_Good.commentTotle + "）";
        ManYiDuText.text = (int)m_Good.commnetScore + "%满意";
        GoodsNumbers = 1;
        goodsKind = new GoodsKind();
    }
    void InitMain(List<string> PictureList)
    {
        for (int i = MainPar.transform.childCount - 1; i >= 1; i--)
        {
            DestroyImmediate(MainPar.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < PictureList.Count; i++)
        {
           GameObject obj = PublicFunc.CreateTmp(InfoPicture, MainPar.transform);
            obj.GetComponent<GoodsInfoImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + PictureList[i]);
        }
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
    public void clickAddShoppingCart(GameObject obj)
    {
        DataMgrAccountDic = JsonConvert.DeserializeObject<Dictionary<long, ShoppingCartDic>>(DataMgr.m_account.goodsList);
        if (goodsKind.id == null)
        {
            UIManager.Instance.PushPanel(UIPanelName.guigepanel, false, true, paragrm => { paragrm.GetComponent<guigepanel>().Init(m_goodsKindList); },true);
            return;
        }
        foreach (long Key in DataMgrAccountDic.Keys)
        {
            if (Key == goodsKind.id)
            {
                //IsHaveShoppingCart = true;
                Hint.LoadTips("购物车已有这件商品", Color.white);
                return;
            }
        }
        ReqAddGoodsListMessage ReqAGLM = new ReqAddGoodsListMessage();
        m_Dic.Clear();
        Target_Dic = new ShoppingCartDic();
        Target_Dic.goodsId = (long)goods.id;
        Target_Dic.number = GoodsNumbers;
        m_Dic.Add((long)goodsKind.id, Target_Dic);
        foreach (long Key in m_Dic.Keys)
        {
            foreach (ShoppingCartDic Value in m_Dic.Values)
            {
                DataMgrAccountDic.Add(Key, Value);
                break;
            }
            break;
        }
        string AddString = JsonConvert.SerializeObject(DataMgrAccountDic);
        ReqAGLM.goodsListStr = AddString;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqAddGoodsListMessage, ReqAGLM);
    }
    public void clickPlaceOrder(GameObject obj)
    {
        if (goodsKind.id == null)
        {
            UIManager.Instance.PushPanel(UIPanelName.guigepanel, false, true, paragrm => { paragrm.GetComponent<guigepanel>().Init(m_goodsKindList); },true);
            // Hint.LoadTips("请选择规格", Color.white);
            return;
        }
        Dictionary<long, ShoppingCartDic> m_PayGoodsDic = new Dictionary<long, ShoppingCartDic>();
        List<GoodsKind> m_GoodsKindList = new List<GoodsKind>();
        m_GoodsKindList.Add(Target_GoodsKind);
        Target_Dic = new ShoppingCartDic();
        Target_Dic.goodsId = (long)goods.id;
        Target_Dic.number = GoodsNumbers;
        m_PayGoodsDic.Add((long)Target_GoodsKind.id, Target_Dic);
        UIManager.Instance.PushPanel(UIPanelName.querenxinxipanel, false, false, paragrm => { paragrm.GetComponent<querenxinxipanel>().Init(m_PayGoodsDic, m_GoodsKindList); });
        //ReqCreateOrderMessage ReqCOM = new ReqCreateOrderMessage();
        //ReqCOM.order = new List<Order>();
        //Order m_Order = new Order();
        //m_Order.accountId = DataMgr.m_account.id;
        //m_Order.goodsKindId = goodsKind.id;
        //m_Order.goodsNum = GoodsNumbers;
        //m_Order.payType=
    }
    void clickChaKanShoppingCart(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.shoppingcartpanel);
    }
    void clickChatZhangGui(GameObject obj)
    {       
        UIManager.Instance.PushPanel(UIPanelName.shoppingchatpanel, false, false, paragrm => { paragrm.GetComponent<shoppingchatpanel>().Init(goods); });
    }
    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= TotalCount)
        {
            return null;
        }
        LoopListViewItem2 item = listView.NewListViewItem("RawImage");
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            //itemScript.Init();
        }
        item.transform.GetComponent<ImageDownLoader>().SetOnlineTexture(AppConst.ImageHeadUrl + m_goodsKindList[index].kindPicture);

          d_img.name = m_goodsKindList[index].id.ToString();
     //   item.gameObject.AddComponent<Button>();
        if (Mode(long.Parse(d_img.name)) == null)
        {
            d_img.SetActive(false);
            obj_Tips.SetActive(false);
        }
        else
        {
            d_img.SetActive(true);
            obj_Tips.SetActive(true);
        }
      //  ClickListener.Get(item.gameObject).onClick = dragGoodsImage;

        TuPianNumberText.text = item.ItemIndex + "/" + TotalCount;
        return item;
    }
    void clickPingLunBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.pinglunpanel, false, true, paragrm => { paragrm.GetComponent<pinglunpanel>().Init(goods, m_CommentList); });
    }
    void clickGuiGeBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.guigepanel, false, true, paragrm => { paragrm.GetComponent<guigepanel>().Init(m_goodsKindList); },true);
        //Debug.Log(GoodsNumbers);
    }
    public void InitGuiGe(GoodsKind m_GoodsKind, int Numbers, int Index)
    {
        goodsKind = m_GoodsKind;
        //GoodName.text =m_GoodsKind.name;
        FenLeiText.text = m_GoodsKind.name;
        GoodsPriceText.text = m_GoodsKind.value.ToString();
        Target_GoodsKind = m_GoodsKind;
        //Debug.Log(GoodsNumbers);
        GoodsNumbers = Numbers;
        ScrollView.MovePanelToItemIndex(Index, 0);
        ScrollView.FinishSnapImmediately();
        //GoodInfo.text=m_GoodsKind.info
    }
    string Mode(long num)
    {
        Debug.Log("长度："+m_goodsId.Count.ToString());
        for (int i = 0; i < m_goodsId.Count; i++)
        {
            Debug.Log(m_goodsId[i].goodsKindId.ToString() +":"+ num.ToString());
            if (m_goodsId[i].goodsKindId==num)
            {
                Debug.Log("取到数据");
                 minscale= m_goodsId[i].minScale;
                 maxscale= m_goodsId[i].maxScale;
                return m_goodsId[i].modleData;
            }
        }
        Debug.Log("没有数据");
        return null;
    }
    void ShowModel(GameObject obj)
    {
        string Mode_data = Mode(long.Parse(obj.name));
        Debug.Log("模型数据" + Mode_data);
        UIManager.Instance.PushPanel(UIPanelName.showmodelpanel, false, false, (param) => {
            rete_mx showModel = param.GetComponent<rete_mx>();
            showModel.CreateModel(Mode_data, (float)maxscale, (float)minscale);
        }, true);
    }
 }