using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using UnityEngine.UI;
using SuperScrollView;
using ProtoDefine;
using SGF.Codec;

public class pinglunpanel : UGUIPanel
{
    public LoopListView2 ScrollView;
    bool IsLoadMoreScrollViewInit = false;
    private bool IsScrollViewInit = false;
    private bool IsHasMore = false;
    private bool IsInit = false;
    public GameObject ShaiXuanPar;
    public GameObject StarPar;
    public GameObject BackBtn;
    public GameObject QueRenBtn;
    public Text ManYiDuText;
    int TotalCount = 0;
    private GameObject lastClickObj;
    List<Comment> ListComment = new List<Comment>();
    public GameObject NoCommentText;
    private string LastTimeDate;
    private Goods Target_Goods;
    private bool IsJumpToCount = false;
    private int JumpToCount = 0;
    // Use this for initialization
    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspQueryCommentsMessage, OnNetQCM);
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspQueryCommentsMessage, OnNetQCM);
    }

    void OnNetQCM(byte[] buf)
    {
        RspQueryCommentsMessage rspQueryCommentsMessage = PBSerializer.NDeserialize<RspQueryCommentsMessage>(buf);
        if (rspQueryCommentsMessage.code == 0)
        {
            Hint.LoadTips(rspQueryCommentsMessage.tip, Color.white);
            IsHasMore = false;
            UpdateScrollView();
            return;
        }

        if (rspQueryCommentsMessage.comments.Count < 16)
        {
            IsHasMore = false;
        }
        else
        {
            IsHasMore = true;
        }
        for (int i = 0; i < rspQueryCommentsMessage.comments.Count; i++)
        {
            ListComment.Add(rspQueryCommentsMessage.comments[i]);
        }
        UpdateScrollView();
    }
    void UpdateScrollView()
    {
        TotalCount = ListComment.Count;
        LastTimeDate = ListComment[ListComment.Count-1].createtime;
        InitLoadMoreScrollView(IsLoadMoreScrollViewInit);
        if (IsJumpToCount == true)
        {
            ScrollView.MovePanelToItemIndex(JumpToCount, 0);
            IsJumpToCount = false;
        }
    }
    void Start()
    {
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        ClickListener.Get(QueRenBtn).onClick = clickBackBtn;
        for (int i = 0; i < ShaiXuanPar.transform.childCount; i++)
        {
            ClickListener.Get(ShaiXuanPar.transform.GetChild(i).gameObject).onClick = clickShaiXuanBtn;
        }
    }
    void clickShaiXuanBtn(GameObject obj)
    {
        if (lastClickObj == obj)
        {
            return;
        }

        for (int i = 0; i < ShaiXuanPar.transform.childCount; i++)
        {
            ShaiXuanPar.transform.GetChild(i).GetComponent<Image>().color = PublicFunc.StringToColor("FFFFFF");
            ShaiXuanPar.transform.GetChild(i).GetChild(0).GetComponent<Text>().color = PublicFunc.StringToColor("0A7AE8");
        }

        obj.GetComponent<Image>().color = PublicFunc.StringToColor("47B320");
        obj.transform.GetChild(0).GetComponent<Text>().color = PublicFunc.StringToColor("FFFFFF");

        lastClickObj = obj;

        if (IsInit == false)
        {
            UpdateScrollView();
            IsInit = true;
            return;
        }

        ReqQueryCommentsMessage reqQueryCommentsMessage = new ReqQueryCommentsMessage();
        reqQueryCommentsMessage.goodsId = (long)Target_Goods.id;

        if (obj.name == "All")
        {
            reqQueryCommentsMessage.star = -1;
        }
        else
        {
            reqQueryCommentsMessage.star = short.Parse(obj.name);
        }
        ListComment.Clear();

        //reqQueryCommentsMessage.lastDate = LastTimeDate;

        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqQueryCommentsMessage, reqQueryCommentsMessage);
        //if (obj.name != "All")
        //{
        //    int index = int.Parse(obj.name);
        //    for (int i = 0; i < ListComment.Count; i++)
        //    {
        //        if (ListComment[i].star == index)
        //        {
        //            ListShaiXuanComment.Add(ListComment[i]);
        //        }
        //    }
        //}
        //else if (obj.name == "All")
        //{
        //    for (int i = 0; i < ListComment.Count; i++)
        //    {
        //        ListShaiXuanComment.Add(ListComment[i]);
        //    }
        //    //ListShaiXuanComment = ListComment;
        //}
    }
    void InitShaiXuan(GameObject obj)
    {

    }

    void InitLoadMoreScrollView(bool IsInit)
    {
        if (TotalCount == 0)
        {
            NoCommentText.SetActive(true);
        }
        else
        {
            NoCommentText.SetActive(false);
        }
        if (IsInit == false)
        {
            ScrollView.InitListView(TotalCount + 1, OnGetItemByIndexLoadMore);
            IsLoadMoreScrollViewInit = true;
        }
        else if (IsInit == true)
        {
            ScrollView.SetListItemCount(TotalCount + 1);
            ScrollView.RefreshAllShownItem();
        }
    }
    public void Init(Goods m_Good, List<Comment> m_ListComment)
    {
        Target_Goods = m_Good;
        ListComment = m_ListComment;
        if (m_ListComment.Count < 16)
        {
            IsHasMore = false;
        }
        else
        {
            IsHasMore = true;
        }

        lastClickObj = null;
        LastTimeDate = m_ListComment[m_ListComment.Count-1].createtime;
        IsInit = false;
        //TotalCount = m_ListComment.Count;
        StarPar.GetComponent<Image>().fillAmount = (float)m_Good.commnetScore / 100;
        ManYiDuText.text = (int)m_Good.commnetScore + "%满意";
        clickShaiXuanBtn(ShaiXuanPar.transform.GetChild(0).gameObject);
    }
    LoopListViewItem2 OnGetItemByIndexLoadMore(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= TotalCount + 1)
        {
            return null;
        }
        LoopListViewItem2 item = null;
        if (index == TotalCount)
        {
            item = listView.NewListViewItem("LoadMoreBtn");
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }
            ClickListener.Get(item.gameObject).onClick = clickLoadMoreBtn;
            if (IsHasMore == false)
            {
                item.gameObject.SetActive(false);
            }
            return item;
        }
        item = listView.NewListViewItem("PingLunTmp");
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            //itemScript.Init();
        }
        InitPingLun(index, item.transform);
        return item;
    }

    void InitPingLun(int index, Transform tran)
    {
        tran.transform.Find("Titel").Find("UserName").GetComponent<Text>().text = ListComment[index].accountName;
        tran.transform.Find("Text").GetComponent<Text>().text = ListComment[index].text;
        tran.transform.Find("Titel").Find("TimeText").GetComponent<Text>().text = ListComment[index].createtime.Substring(0, 10);
        PublicFunc.CreateHeadImg(tran.transform.Find("Titel").Find("UserHeadImage").GetComponent<Image>(), ListComment[index].moudleId);
        for (int i = 0; i < tran.transform.Find("Titel").Find("Stars").childCount; i++)
        {
            tran.transform.Find("Titel").Find("Stars").GetChild(i).GetComponent<Image>().color = PublicFunc.StringToColor("B5B6B9");
        }
        for (int i = 0; i < ListComment[index].star; i++)
        {
            tran.transform.Find("Titel").Find("Stars").GetChild(i).GetComponent<Image>().color = PublicFunc.StringToColor("E6860A");
        }
    }
    void clickLoadMoreBtn(GameObject obj)
    {
        ReqQueryCommentsMessage reqQueryCommentsMessage = new ReqQueryCommentsMessage();
        JumpToCount = TotalCount;
        IsJumpToCount = true;
        reqQueryCommentsMessage.goodsId = (long)Target_Goods.id;
        if (lastClickObj.name == "All")
        {
            reqQueryCommentsMessage.star = -1;
        }
        else
        {
            reqQueryCommentsMessage.star = int.Parse(lastClickObj.name);
        }
        
        reqQueryCommentsMessage.lastDate = LastTimeDate;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqQueryCommentsMessage, reqQueryCommentsMessage);
    }

    void SengReqQCM()
    {

    }

    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
}
