using System.Collections;
using System.Collections.Generic;
using Framework.Event;
using Framework.UI;
using ProtoDefine;
using SGF.Codec;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;

public class searchgoodspanel : UGUIPanel {
    public LoopListView2 ScrollView;
    private bool IsScrollViewInit = false;
    int TotalCount = 0;
    public List<Goods> GoodsList = new List<Goods>();
    public GameObject BackBtn;
    public InputField InputF;
    public GameObject SearchBtn;
    public GameObject GoToHomeBtn;
    public Text TipsText;
    public string TipString="“酒”";
    void Start()
    {

        InputF.onEndEdit.AddListener(delegate { EndInput(InputF); });
        ClickListener.Get(InputF.gameObject).onClick = clickInputFiled;
       // InputF.start
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        ClickListener.Get(GoToHomeBtn).onClick = clickGoToHome;
        // ClickListener.Get(SearchBtn).onClick = clickSearchBtn;
    }

    void clickInputFiled(GameObject obj)
    {
        obj.transform.Find("SearchImage").gameObject.SetActive(false);
        obj.transform.Find("Placeholder").gameObject.SetActive(false);
    }
    void clickGoToHome(GameObject obj)
    {
        VirtualCityMgr.GotoHometown(EnMyOhter.My);
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }

    void EndInput(InputField inputFiled)
    {
        if (inputFiled.text == "")
        {
            return;
        }
        //inputFiled.transform.Find("Image").gameObject.SetActive(false);
        ReqSearchGoodsMessage ReqSGM = new ReqSearchGoodsMessage();
        ReqSGM.goodsName = inputFiled.text;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqSearchGoodsMessage, ReqSGM);
    }
    void clickSearchBtn(GameObject obj)
    {
        if (InputF.text == "")
        {
            return;
        }

        ReqSearchGoodsMessage ReqSGM=new ReqSearchGoodsMessage();
        ReqSGM.goodsName = InputF.text;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqSearchGoodsMessage,ReqSGM);
    }
    public override void OnOpen()
    {
        InputF.text = "";
        TipsText.text = "大家都在搜索" + TipString;
        if (IsScrollViewInit == true)
        {
            ScrollView.SetListItemCount(0);
            ScrollView.RefreshAllShownItem();
        }
        InputF.transform.Find("SearchImage").gameObject.SetActive(true);
        InputF.transform.Find("Placeholder").gameObject.SetActive(true);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspSearchGoodsMessage,OnNetRspSGM);
        EventManager.Instance.AddEventListener(Common.EventStr.NewGuideParam, OnEvNewGuide);
    } 

    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspSearchGoodsMessage, OnNetRspSGM);
        EventManager.Instance.RemoveEventListener(Common.EventStr.NewGuideParam, OnEvNewGuide);
    }

    void OnEvNewGuide(EventData data)
    {
        var exdata = data as EventDataEx<NewGuideItem>;
        var info = exdata.GetData();
        if (info.panelName == m_type)
        {
            InputF.text = info.param;
            ReqSearchGoodsMessage ReqSGM = new ReqSearchGoodsMessage();
            ReqSGM.goodsName = info.param;
            HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqSearchGoodsMessage, ReqSGM);
        }
            
    }
    void OnNetRspSGM(byte[] buf)
    {
        RspSearchGoodsMessage Rsp = PBSerializer.NDeserialize<RspSearchGoodsMessage>(buf);
        if (Rsp.goodsList == null)
        {
            Hint.LoadTips("暂无商品", Color.white);
            return;
        }
        UpdateGoods(Rsp.goodsList);
    }

    void UpdateGoods(List<Goods> m_GoodsList)
    {
        GoodsList = m_GoodsList;
        TotalCount = m_GoodsList.Count;
        if (TotalCount == 0)
        {
            return;
        }

        if (IsScrollViewInit == false)
        {
            ScrollView.InitListView(TotalCount, OnGetItemByIndex);
            IsScrollViewInit = true;
        }
        else
        {
            ScrollView.SetListItemCount(TotalCount);
            ScrollView.RefreshAllShownItem();
        }
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
        LoopListViewItem2 item = listView.NewListViewItem("Good");
        //ListItem2 itemScript = item.GetComponent<ListItem2>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            //itemScript.Init();
        }
        item.GetComponent<searchGoodItem>().Init(GoodsList[index]);
        //item.name = index.ToString();
        return item;
    }
}
