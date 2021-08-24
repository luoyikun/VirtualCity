using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using SuperScrollView;
using UnityEngine.UI;
using ProtoDefine;
using SGF.Codec;
using Framework.Event;

public class shoppingmunepanel : UGUIPanel {
    public GameObject BackBtn;
    public LoopListView2 ScrollView;
    bool IsScrollViewInit = false;
    public List<RspGetGoodsListMessage> m_RspGGLM=new List<RspGetGoodsListMessage>();
    public List<Goods> m_GoodsList = new List<Goods>();

  //  public static List<PartProperties> m_goodsId ;

    int TotalCount = 0;
    int ItemCount;
    int PageIndex=1;
    int mItemCountPerRow = 3;
    // Use this for initialization
    void Start () {
        /*
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
        */
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        //Debug.Log("goodlist:" +DataMgr.m_account.goodsList);
        //ReqGGLM(97816308728463419);
    }
    public void Init()
    {
        TotalCount = 0;
        ItemCount = 0;
        m_GoodsList.Clear();
        for (int i =0; i < m_RspGGLM.Count; i++)
        {
            if (m_RspGGLM[i].goodsList != null)
            {
                TotalCount += m_RspGGLM[i].goodsList.Count;
                for (int j = 0; j < m_RspGGLM[i].goodsList.Count; j++)
                {
                    m_GoodsList.Add(m_RspGGLM[i].goodsList[j]);
                }
            }
        }
        ItemCount = TotalCount / mItemCountPerRow;
        if (TotalCount % mItemCountPerRow > 0)
        {
            ItemCount++;
        }
        if (IsScrollViewInit == false)
        {
            ScrollView.InitListView(ItemCount, OnGetItemByIndex);
            IsScrollViewInit = true;
        }
        else if (IsScrollViewInit == true)
        {
            ScrollView.SetListItemCount(ItemCount);
            ScrollView.RefreshAllShownItem();
        }
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
       // PageIndex++;
    }
    public void ReqGGLM(long m_BusinessID)
    {
        ReqGetGoodsListMessage ReqGGLM = new ReqGetGoodsListMessage();
        ReqGGLM.businessId = m_BusinessID;
        ReqGGLM.pageIndex = PageIndex;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetGoodsListMessage, ReqGGLM);
    }

    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetGoodsListMessage, OnNetRspGGLM);
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetGoodsListMessage, OnNetRspGGLM);
    }
    void OnNetRspGGLM(byte[] buf)
    {
        RspGetGoodsListMessage RspQBM = PBSerializer.NDeserialize<RspGetGoodsListMessage>(buf);
        if (RspQBM.code != 0)
        {
            m_RspGGLM.Clear();
            m_RspGGLM.Add(RspQBM);
            Init();
        }
        else if (RspQBM.code == 0)
        {
            Hint.LoadTips(RspQBM.tip, Color.white);
        }
    }
    public void clickBackBtn(GameObject obj)
    {
        EventManager.Instance.DispatchEvent(Common.EventStr.CloseCamZhangGui);
        UIManager.Instance.PopSelf();
        //NewGuideMgr.Instance.StartOneNewGuide();
    }
    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= ItemCount)
        {
            return null;
        }

        //ItemData itemData = DataSourceMgr.Get.GetItemDataByIndex(index);
        //if (itemData == null)
        //{
        //    return null;
        //}
        //get a new item. Every item can use a different prefab, the parameter of the NewListViewItem is the prefab’name. 
        //And all the prefabs should be listed in ItemPrefabList in LoopListView2 Inspector Setting
        LoopListViewItem2 item = listView.NewListViewItem("Goods");
        //ListItem2 itemScript = item.GetComponent<ListItem2>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            //itemScript.Init();
        }
        int ItemChildIndex;
        for (int i = 0; i < mItemCountPerRow; i++)
        {
            ItemChildIndex = index* mItemCountPerRow +i;
            if (ItemChildIndex >= TotalCount)
            {
                item.transform.GetChild(i).gameObject.SetActive(false);
                continue;
            }
            else
            {
                item.transform.GetChild(i).gameObject.SetActive(true);
            }
            item.GetComponent<RefleshGoodsInfo>().Init(i,ItemChildIndex, m_GoodsList);
        }
        item.name=index.ToString();
        return item;
    }
}
