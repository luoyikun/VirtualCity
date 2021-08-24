using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using UnityEngine.UI;
using System;
using ProtoDefine;
using LitJson;
using SuperScrollView;
using SGF.Codec;

public class dingdanpanel : UGUIPanel {

    public static dingdanpanel ddp;
    // public Order1List ol;
    //public List<Order1> Order1list = new List<Order1>();
    public GameObject backBtn;
    public LoopListView2 ScrollView;
    bool IsScrollViewInit = false;
    int TotalCount = 9;
    int Page = 1;
    string LastDate=null;
    RspQueryOrderMessage m_RsQOM;
    Order Target_Order = new Order();
    bool IsFirstOpen = false;

    public GameObject TargetGameObject;
    // Use this for initialization
    private void Awake()
    {
        //init();
        //AssetMgr.Instance.CreateText("order1list", "order1list", init);
    }
    private void Start()
    {
        
        foreach (Transform tran in transform.GetComponentsInChildren<Transform>())
        {
            
        }
        //LeftMuneMgr.LFM.clickLeftMenuBtn(LeftMuneMgr.LFM.LeftMenuPar.transform.GetChild(0).gameObject);

    }

    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspQueryOrderMessage, OnNetRspQOM);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspComfirmReceiptMessage,OnNetRspCRM);
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspQueryOrderMessage, OnNetRspQOM);
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
    public void OnNetRspQOM(byte[] buf)
    {
        RspQueryOrderMessage RspQOM= PBSerializer.NDeserialize<RspQueryOrderMessage>(buf);
        if (RspQOM.code != 0)
        {
            m_RsQOM = RspQOM;
            UpdateDingdan(m_RsQOM.orders);
        }
        else if (RspQOM.code == 0)
        {
            // Debug.Log(RspQOM.tip);
            //if()
            Hint.LoadTips(RspQOM.tip, Color.white);
        }
    }
    private void OnSelectItem(GameObject obj)
    {
        obj.GetComponent<Image>().color = Color.green;
    }
    public void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
    private void OnUnSelectItem(GameObject obj)
    {
        obj.GetComponent<Image>().color = Color.white;
    }
    public void init()
    {
        ddp = this;
        ClickListener.Get(backBtn).onClick = clickBackBtn;
        if (LeftMuneMgr.LFM == null)
        {
            LeftMuneMgr.LFM = transform.Find("LeftMenu").GetComponent<LeftMuneMgr>();
            LeftMuneMgr.LFM.LeftMenuPar = LeftMuneMgr.LFM.gameObject;
            LeftMuneMgr.LFM.clickLeftMenuBtn(LeftMuneMgr.LFM.LeftMenuPar.transform.GetChild(0).gameObject);
        }
        else
        {
            LeftMuneMgr.LFM.clickLeftMenuBtn(LeftMuneMgr.LFM.LeftMenuPar.transform.GetChild(0).gameObject);
        }
    }

    void UpdateDingdan(List<Order> m_orders)
    {
        List<Order> m_TuiKuanOrder = new List<Order>();
        if (m_orders != null)
        {
            //for (int i = 0; i < m_orders.Count; i++)
            //{
            //    switch (m_orders[i].orderStatus)
            //    {
            //        case "6":
            //        case "7":
            //        case "8":
            //        case "9":
            //            m_TuiKuanOrder.Add(m_orders[i]);
            //            break;
            //    }
            //}
            //for (int i = m_TuiKuanOrder.Count - 1; i >= 0; i--)
            //{
            //    if (m_orders.Contains(m_TuiKuanOrder[i]))
            //    {
            //        m_orders.Remove(m_TuiKuanOrder[i]);
            //    }
            //}
            TotalCount = m_orders.Count;
        }
        else if (m_orders == null)
        {
            TotalCount = 0;
            Hint.LoadTips("暂无订单", Color.white);
        }
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

    void ReqQueryMessage(int state)
    {
        ReqQueryOrderMessage ReqQOM = new ReqQueryOrderMessage();
        ReqQOM.status = state;
        ReqQOM.createTime = DataMgr.m_account.createtime;
        ReqQOM.lastDate = LastDate;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqQueryOrderMessage, ReqQOM);
    }
    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= TotalCount)
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
        //item.GetComponent<dingdanpool>().Dingdancallback(m_RsQOM.orders[index]);
        dingdanpool m_dingdanpool = null;
        if (item.GetComponent<dingdanpool>() == null)
        {
             m_dingdanpool = item.gameObject.AddComponent<dingdanpool>();
        }
        else
        {
            m_dingdanpool = item.GetComponent<dingdanpool>();
        }
        m_dingdanpool.Dingdancallback(m_RsQOM.orders[index]);
        LastDate = m_RsQOM.orders[index].createtime;
        return item;
    }
    public void clickLeftMenu(int state)
    {
        LastDate = null;
        ReqQueryMessage(state);
    }
    public void SendQueRenDingDan(Order m_Order)
    {
        Target_Order = m_Order;
        ReqConfirmReceiptMessage m_ReqCRM = new ReqConfirmReceiptMessage();
        m_ReqCRM.order = m_Order;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqConfirmReceiptMessage, m_ReqCRM);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
