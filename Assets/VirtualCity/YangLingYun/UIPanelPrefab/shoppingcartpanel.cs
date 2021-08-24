using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using UnityEngine.UI;
using SuperScrollView;
using Newtonsoft.Json;
using ProtoDefine;
using SGF.Codec;

public class shoppingcartpanel : UGUIPanel
{
    public GameObject backBtn;
    public GameObject GoodsTmp;
    public GameObject GoodsPar;
    public GameObject AllSelectBtn;
    public GameObject DeleteSelectBtn;
    public GameObject EditBtn;
    bool IsSelect = false;
    //public List<GameObject> selectobj = new List<GameObject>();
    public LoopListView2 ScrollView;
    public List<GoodsKind> m_ListKind = new List<GoodsKind>();
    bool IsScrollViewInit = false;
    int TotalCount = 0;
    int mItemCountPerRow = 2;
    int ReqCount = 6;
    int itemCount = 0;
    public Text TotalPriceText;
    public double TotalPrice = 0;
    List<long?> listGoodsKindId = new List<long?>();
    //List<GoodsKind> SelectGoodsKind = new List<GoodsKind>();
    public List<long?> m_ListSelectGoodsId = new List<long?>();
    bool IsEdit = false;
    bool IsAllSelect = false;
    public Dictionary<long, ShoppingCartDic> DataMgrAccountCart = new Dictionary<long, ShoppingCartDic>();
    public Dictionary<long, ShoppingCartDic> PayGoodsList = new Dictionary<long, ShoppingCartDic>();
    public List<bool> ItemBoolList = new List<bool>();
    public Dictionary<long, double> ItemPriceDic = new Dictionary<long, double>();
    // Use this for initialization
    void Start()
    {
        ClickListener.Get(backBtn).onClick = clickBackBtn;
        //StartCoroutine(CreateTmp());
        //StopCoroutine(CreateTmp());
        ClickListener.Get(EditBtn).onClick = clickEditBtn;
        ClickListener.Get(AllSelectBtn).onClick = clickAllSelectBtn;
        ClickListener.Get(DeleteSelectBtn).onClick = clickDeleteSelectBtn;
    }
    public void Init()
    {
        DataMgrAccountCart = JsonConvert.DeserializeObject<Dictionary<long, ShoppingCartDic>>(DataMgr.m_account.goodsList);
        listGoodsKindId.Clear();
        PayGoodsList.Clear();
        itemCount = 0;
        TotalCount = 0;
        TotalPrice = 0;
        AllSelectFalse(AllSelectBtn);
        foreach (long Key in DataMgrAccountCart.Keys)
        {
            listGoodsKindId.Add(Key);
        }
        if (listGoodsKindId.Count >= 6)
        {
            ReqCount = 6;
        }
        else if (listGoodsKindId.Count < 6)
        {
            ReqCount = listGoodsKindId.Count;
        }
        SendReqGKM();
        TotalPriceText.text = "<color=#0A7AE8><size=38>合计：</size></color>" + TotalPrice + "元";
        //m_ReqGKM.listGoodsId = 
    }
    void SendReqGKM()
    {
        if (listGoodsKindId.Count != 0)
        {
            ReqGoodsKindInfoMessage m_ReqGKM = new ReqGoodsKindInfoMessage();
            m_ReqGKM.listGoodsKindId = new List<long?>();
            for (int i = 0; i < ReqCount; i++)
            {
                m_ReqGKM.listGoodsKindId.Add(listGoodsKindId[i]);
            }
            ReqCount += 6;
            HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqGoodsKindInfoMessage, m_ReqGKM);
        }
        else if (listGoodsKindId.Count == 0)
        {
            if (IsScrollViewInit == false)
            {
                ScrollView.InitListView(itemCount, OnGetItemByIndex);
                IsScrollViewInit = true;
            }
            else if (IsScrollViewInit == true)
            {
                ScrollView.SetListItemCount(itemCount);
                ScrollView.RefreshAllShownItem();
            }
            Hint.LoadTips("购物车内无商品", Color.white);
        }
    }
    void UpdateShoppingCart()
    {
        ReqAddGoodsListMessage m_ReqAGLM = new ReqAddGoodsListMessage();
        m_ReqAGLM.goodsListStr = JsonConvert.SerializeObject(DataMgrAccountCart);
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqAddGoodsListMessage, m_ReqAGLM);
    }
    void InitScrollView()
    {

    }
    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGoodsKindInfoMessage, OnNetRspGKIM);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, OnNetRspComment);
        Init();
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGoodsKindInfoMessage, OnNetRspGKIM);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, OnNetRspComment);
    }
    void OnNetRspComment(byte[] buf)
    {
        RspCommentMessage RspCM = PBSerializer.NDeserialize<RspCommentMessage>(buf);
        if (RspCM.rspcmd != 511)
        {
            return;
        }
        if (RspCM.code != 0)
        {
            List<long?> KeyID = new List<long?>();
            foreach (long Key in DataMgrAccountCart.Keys)
            {
                for (int i = 0; i < m_ListSelectGoodsId.Count; i++)
                {
                    if (Key == m_ListSelectGoodsId[i])
                    {
                        KeyID.Add(Key);
                    }
                }
            }

            if (KeyID != null)
            {
                foreach (long Key in KeyID)
                {
                    if (ItemPriceDic.ContainsKey((long)Key))
                    {
                        ItemPriceDic.Remove((long)Key);
                    }
                    DataMgrAccountCart.Remove((long)Key);
                }
                DataMgr.m_account.goodsList = JsonConvert.SerializeObject(DataMgrAccountCart);
                Init();
            }
            //Hint.LoadTips("删除成功", Color.white);
        }
        else if (RspCM.code == 0)
        {
            Hint.LoadTips(RspCM.tip, Color.white);
        }
    }
    void OnNetRspGKIM(byte[] buf)
    {
        RspGoodsKindInfoMessage RspGKIM = PBSerializer.NDeserialize<RspGoodsKindInfoMessage>(buf);
        m_ListKind.Clear();
        for (int i = 0; i < RspGKIM.listGoodsKids.Count; i++)
        {
            m_ListKind.Add(RspGKIM.listGoodsKids[i]);
        }

        itemCount = RspGKIM.listGoodsKids.Count / mItemCountPerRow;
        TotalCount = m_ListKind.Count;
        ItemBoolList.Clear();
        for (int i = 0; i < TotalCount; i++)
        {
            ItemBoolList.Add(false);
        }
        if (RspGKIM.listGoodsKids.Count % mItemCountPerRow > 0)
        {
            itemCount++;
        }
        if (TotalCount <= 6)
        {
            if (IsScrollViewInit == false)
            {
                ScrollView.InitListView(itemCount, OnGetItemByIndex);
                IsScrollViewInit = true;
            }
            else if (IsScrollViewInit == true)
            {
                ScrollView.SetListItemCount(itemCount);
                ScrollView.RefreshAllShownItem();
            }
        }
        else if (TotalCount > 6)
        {
            if (IsScrollViewInit == false)
            {
                ScrollView.InitListView(itemCount + 1, OnGetItemByIndexAndLoadMore);
                IsScrollViewInit = true;
            }
            else if (IsScrollViewInit == true)
            {
                ScrollView.SetListItemCount(itemCount + 1);
                ScrollView.RefreshAllShownItem();
            }
        }
    }
    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= itemCount)
        {
            return null;
        }
        LoopListViewItem2 item = listView.NewListViewItem("GoodsPar");
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }
        int ItemChildIndex;
        for (int i = 0; i < mItemCountPerRow; i++)
        {
            ItemChildIndex = index * mItemCountPerRow + i;
            if (ItemChildIndex >= TotalCount)
            {
                item.transform.GetChild(i).gameObject.SetActive(false);
                continue;
            }
            else
            {
                item.transform.GetChild(i).gameObject.SetActive(true);
            }
            // item.transform.GetChild(0).GetChild(1).gameObject.SetActive(ItemBoolList[index]);
            item.GetComponent<GoodsItem>().Init(i, ItemChildIndex, m_ListKind, this.GetComponent<shoppingcartpanel>(), DataMgrAccountCart);
        }
        //item.transform.GetChild(0).GetChild(1).gameObject.SetActive(ItemBoolList[index]);
        //item.transform.GetComponent<MonthBillMgr>().Ini(Old_RQBM[index].bills.Count, Old_RQBM[index].bills);
        return item;
    }
    LoopListViewItem2 OnGetItemByIndexAndLoadMore(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= itemCount + 1)
        {
            return null;
        }
        LoopListViewItem2 item = null;
        if (index == itemCount)
        {
            item = listView.NewListViewItem("LoadMoreBtn");
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
                ClickListener.Get(item.gameObject).onClick = clickLoadMoreBtn;
            }
            return item;
        }
        item = listView.NewListViewItem("GoodsPar");
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }
        int ItemChildIndex;
        for (int i = 0; i < mItemCountPerRow; i++)
        {
            ItemChildIndex = index * mItemCountPerRow + i;
            if (ItemChildIndex >= TotalCount)
            {
                item.transform.GetChild(i).gameObject.SetActive(false);
                continue;
            }
            else
            {
                item.transform.GetChild(i).gameObject.SetActive(true);
            }
            // item.transform.GetChild(0).GetChild(1).gameObject.SetActive(ItemBoolList[index]);
            item.GetComponent<GoodsItem>().Init(i, ItemChildIndex, m_ListKind, this.GetComponent<shoppingcartpanel>(), DataMgrAccountCart);
        }
        //item.transform.GetChild(0).GetChild(1).gameObject.SetActive(ItemBoolList[index]);
        //item.transform.GetComponent<MonthBillMgr>().Ini(Old_RQBM[index].bills.Count, Old_RQBM[index].bills);
        return item;
    }
    void clickLoadMoreBtn(GameObject obj)
    {
        SendReqGKM();
    }
    public void clickGoodsImage(GameObject obj)
    {

    }
    void clickEditBtn(GameObject obj)
    {
        if (IsEdit == false)
        {
            obj.transform.GetChild(0).GetComponent<Text>().text = "完成";
            DeleteSelectBtn.transform.Find("Text").GetComponent<Text>().text = "删除所选";
            TotalPriceText.gameObject.SetActive(false);
            IsEdit = true;
        }
        else if (IsEdit == true)
        {
            obj.transform.GetChild(0).GetComponent<Text>().text = "编辑";
            DeleteSelectBtn.transform.Find("Text").GetComponent<Text>().text = "去结算";
            TotalPriceText.gameObject.SetActive(true);
            IsEdit = false;
        }
    }
    void clickBackBtn(GameObject obj)
    {
        //UpdateShoppingCart();
        UIManager.Instance.PopSelf();
    }
    public void clickAllSelectBtn(GameObject obj)
    {
        if (IsAllSelect == false)
        {
            AllSelectTrue(obj);
        }
        else if (IsAllSelect == true)
        {
            AllSelectFalse(obj);
        }
        ScrollView.RefreshAllShownItem();
    }

    void AllSelectFalse(GameObject obj)
    {
        for (int i = 0; i < ItemBoolList.Count; i++)
        {
            ItemBoolList[i] = false;
        }
        PayGoodsList = new Dictionary<long, ShoppingCartDic>();
        obj.transform.Find("selectImage").gameObject.SetActive(false);
        IsAllSelect = false;
    }

    void AllSelectTrue(GameObject obj)
    {
        for (int i = 0; i < ItemBoolList.Count; i++)
        {
            ItemBoolList[i] = true;
        }
        PayGoodsList = DataMgrAccountCart;
        obj.transform.Find("selectImage").gameObject.SetActive(true);
        IsAllSelect = true;
    }

    void clickDeleteSelectBtn(GameObject obj)
    {
        if (IsEdit == true)
        {
            if (m_ListSelectGoodsId.Count != 0)
            {
                ReqRemoveGoodsListMessage m_ReqRGLM = new ReqRemoveGoodsListMessage();
                m_ReqRGLM.listGoodsKindId = m_ListSelectGoodsId;
                HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqRemoveGoodsListMessage, m_ReqRGLM);
                //Hint.LoadTips("删除成功", Color.white);
            }
        }
        else if (IsEdit == false)
        {
            if (PayGoodsList.Count != 0)
            {
                foreach (long Key in PayGoodsList.Keys)
                {
                    if (PayGoodsList[Key].number == 0)
                    {
                        Hint.LoadTips("某些物品无可购买数量", Color.white);
                        return;
                    }
                }
                UIManager.Instance.PushPanel(UIPanelName.querenxinxipanel, false, false, paragrm => { paragrm.GetComponent<querenxinxipanel>().Init(PayGoodsList, m_ListKind); });
                UpdateShoppingCart();
            }
            else if (PayGoodsList.Count == 0)
            {
                Hint.LoadTips("请选择要下单的商品", Color.white);
            }
        }
    }
    public void clickSelectBtn(GameObject obj)
    {
        if (ItemBoolList[int.Parse(obj.name)] == false)
        {
            ItemBoolList[int.Parse(obj.name)] = true;
            foreach (long Key in DataMgrAccountCart.Keys)
            {
                if (Key == long.Parse(obj.transform.parent.name))
                {
                    PayGoodsList.Add(Key, DataMgrAccountCart[Key]);
                }
            }
        }
        else if (ItemBoolList[int.Parse(obj.name)] == true)
        {
            ItemBoolList[int.Parse(obj.name)] = false;
            long? KeyID = null;
            foreach (long Key in PayGoodsList.Keys)
            {
                if (Key == long.Parse(obj.transform.parent.name))
                {
                    KeyID = Key;
                }
            }
            if (KeyID != null)
            {
                PayGoodsList.Remove((long)KeyID);
            }
        }
        //for (int i = 0; i < ItemBoolList.Count; i++)
        //{
        //    Debug.Log(ItemBoolList[i]);
        //}
        //ScrollView.RefreshItemByItemIndex(int.Parse(obj.name));
        ScrollView.RefreshAllShownItem();
    }
    void InitPrice()
    {

    }
    public void clickNumberBtn(GameObject obj)
    {
        int MaxI = 0;
        int i = DataMgrAccountCart[long.Parse(obj.transform.parent.parent.name)].number;
        foreach (long Key in DataMgrAccountCart.Keys)
        {
            if (long.Parse(obj.transform.parent.parent.name) == Key)
            {
                i = DataMgrAccountCart[Key].number;
                obj.transform.parent.GetChild(2).GetComponent<Text>().text = i.ToString();
                for (int j = 0; j < m_ListKind.Count; j++)
                {
                    if (m_ListKind[j].id == Key)
                    {
                        MaxI = (int)m_ListKind[j].number;
                    }
                }
            }
        }
        if (obj.name == "reduBtn")
        {
            if (i > 1)
            {
                i--;
                obj.transform.parent.GetChild(2).GetComponent<Text>().text = i.ToString();
            }
            if (i == 1)
            {
                Hint.LoadTips("已经到达最小值了噢", Color.white);
            }
        }
        else if (obj.name == "increaseBtn")
        {
            if (i < MaxI)
            {
                i++;
                obj.transform.parent.GetChild(2).GetComponent<Text>().text = i.ToString();
            }
            else
            {
                Hint.LoadTips("已到最大可购买数额", Color.white);
            }
        }
        foreach (long Key in DataMgrAccountCart.Keys)
        {
            if (long.Parse(obj.transform.parent.parent.name) == Key)
            {
                DataMgrAccountCart[Key].number = i;
            }
        }
        DataMgr.m_account.goodsList = JsonConvert.SerializeObject(DataMgrAccountCart);
        ScrollView.RefreshAllShownItem();
    }
}
