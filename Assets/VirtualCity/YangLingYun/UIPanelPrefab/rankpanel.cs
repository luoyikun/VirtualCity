using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using SuperScrollView;
using LitJson;
using ProtoDefine;
using SGF.Codec;
using Newtonsoft.Json;

public class RankList
{
    public List<Rank> rankList = new List<Rank>();
}
public class Rank
{
    public string UserName;
    public int UserZiChan;
    public int UserJiaYuanID;
}
public class rankpanel : UGUIPanel
{
    public GameObject RankScrollPar;
    public GameObject BackBtn;
    public GameObject RankTmp;
    public GameObject DownPar;
    public GameObject TopPar;
    public static rankpanel rp;
    public LoopListView2 RankScrollView;
    bool IsRankScrollViewInit = false;
    public GameObject QuanFuBtn;
    public GameObject HaoYouBtn;
    public GameObject LeftBtnPar;

    public GameObject HelpBtn;
    //public int TotalCount;
    string ranklist;
    RankList RL;
    public ReqGetRankMessage Target_ReqGRM;
    List<RankInfo> RankInfoList = new List<RankInfo>();
    Dictionary<string, int> RankJiangLiDic = new Dictionary<string, int>();
    int XiShu = 0;
    bool IsGoToHomeTwon = false;
    List<int> rankKey = new List<int>();
    List<int> rankValue = new List<int>();
    private int rankType = 0;
    //List<ChatUser> RankChatUser = new List<ChatUser>();
    private void Awake()
    {
        rp = this;

    }
    // Use this for initialization
    void Start()
    {
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        ClickListener.Get(HaoYouBtn).onClick = clickShaiXuanBtn;
        ClickListener.Get(QuanFuBtn).onClick = clickShaiXuanBtn;
        ClickListener.Get(HelpBtn).onClick = clickHelpBtn;
        SetScroll();
    }

    void clickHelpBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.noticepanel, false, false, (param) =>
            {
                NoticePanel notice = param.GetComponent<NoticePanel>();
                notice.ShowCommonNotice(2);
            }, true
        );
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetRankMessage, OnEvNetRankMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetPlayerByIdMessage, OnGetPlayerByIdMessage);
    }

    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetRankMessage, OnEvNetRankMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetPlayerByIdMessage, OnGetPlayerByIdMessage);
    }

    void OnGetPlayerByIdMessage(byte[] buf)
    {
        RspGetPlayerByIdMessage RspGPBIM = PBSerializer.NDeserialize<RspGetPlayerByIdMessage>(buf);
        if (IsGoToHomeTwon == true)
        {
            UIManager.Instance.PopSelf();
            //RspGetPlayerByIdMessage RspGPBIM = PBSerializer.NDeserialize<RspGetPlayerByIdMessage>(buf);
            VirtualCityMgr.GotoHometown(EnMyOhter.Other, RspGPBIM.users[0]);

            IsGoToHomeTwon = false;
            return;
        }
        //RankChatUser = RspGPBIM.users;
        //InitScrollView(RankChatUser.Count);
    }
    void OnEvNetRankMessage(byte[] buf)
    {
        RspGetRankMessage RspGRM = PBSerializer.NDeserialize<RspGetRankMessage>(buf);
        List<long?> Target_AccountList = new List<long?>();
        DownPar.transform.Find("RankText").GetComponent<Text>().text = "您未上榜";
        DownPar.transform.Find("UserNameText").GetComponent<Text>().text = DataMgr.m_account.userName;
        PublicFunc.CreateHeadImg(DownPar.transform.Find("HeadImage").GetComponent<Image>(), (long)DataMgr.m_account.modleId);
        switch (rankType)
        {
            case SystemDataPool.INCOME:
                DownPar.transform.Find("ZiChanText").GetComponent<Text>().text = DataMgr.m_account.wallet.mIncome.ToString("0.00");
                break;
            case SystemDataPool.DCOST:
                for (int i = 0; i < DataMgr.businessModelProperties.Count; i++)
                {
                    if (DataMgr.businessModelProperties[i].Name == "diamond2money")
                    {
                        JieXi m_JieXi = new JieXi();
                        m_JieXi = JsonConvert.DeserializeObject<JieXi>(DataMgr.businessModelProperties[i].Con);
                        XiShu = int.Parse(m_JieXi.v);
                        break;
                    }
                }
                DownPar.transform.Find("ZiChanText").GetComponent<Text>().text = (DataMgr.m_account.wallet.dCostNum / XiShu).ToString("0.00");
                break;
            case SystemDataPool.ASSET:
                DownPar.transform.Find("ZiChanText").GetComponent<Text>().text =
                    DataMgr.m_account.wallet.asset.ToString("0.00");
                break;
            case SystemDataPool.ZAN:
                DownPar.transform.Find("ZiChanText").GetComponent<Text>().text = DataMgr.m_zan.ToString();
                // DownPar.transform.Find("ZiChanText").Find("Text").gameObject.SetActive(true);
                foreach (var zhi in DataMgr.businessModelProperties)
                {
                    if (zhi.Name == "zan_reward")
                    {
                        RankJiangLiDic = JsonConvert.DeserializeObject<Dictionary<string, int>>(zhi.Con);
                    }
                }
                break;
        }
        if (RspGRM != null)
        {
            if (RspGRM.list == null)
            {
                RankInfoList = new List<RankInfo>();
            }
            else
            {
                RankInfoList = RspGRM.list;
            }
            RankScrollView = rp.transform.Find("Main/RankScroll/RankScrollView").gameObject.GetComponent<LoopListView2>();
            RankScrollView.gameObject.SetActive(true);
            for (int i = 0; i < RankInfoList.Count; i++)
            {
                Target_AccountList.Add(RankInfoList[i].accountId);
                if (RankInfoList[i].accountId == DataMgr.m_account.id)
                {
                    DownPar.transform.Find("RankText").GetComponent<Text>().text = (i + 1).ToString();
                    DownPar.transform.Find("UserNameText").GetComponent<Text>().text = RankInfoList[i].name;
                    DownPar.transform.Find("ZiChanText").GetComponent<Text>().text = RankInfoList[i].info.ToString();
                    PublicFunc.CreateHeadImg(DownPar.transform.Find("HeadImage").GetComponent<Image>(), (long)DataMgr.m_account.modleId);
                }
            }
            //ReqGetPlayerByIdMessage m_ReqGPBIM = new ReqGetPlayerByIdMessage();
            //m_ReqGPBIM.accountIds = Target_AccountList;
            //ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetPlayerByIdMessage, m_ReqGPBIM, EnSocket.Chat);
            //if (Target_ReqGRM.rankType == SystemDataPool.DCOST)
            //{
            //    for (int i = 0; i < DataMgr.businessModelProperties.Count; i++)
            //    {
            //        if (DataMgr.businessModelProperties[i].Name == "diamond2money")
            //        {
            //            JieXi m_JieXi = new JieXi();
            //            m_JieXi = JsonConvert.DeserializeObject<JieXi>(DataMgr.businessModelProperties[i].Con);
            //            XiShu = int.Parse(m_JieXi.v);
            //            break;
            //        }
            //    }
            //}
            switch (Target_ReqGRM.rankType)
            {
                case SystemDataPool.INCOME:
                    TopPar.transform.Find("Text").GetComponent<Text>().text = "收益数额";
                    break;
                case SystemDataPool.DCOST:
                    TopPar.transform.Find("Text").GetComponent<Text>().text = "慈善数额";
                    break;
                case SystemDataPool.ASSET:
                    TopPar.transform.Find("Text").GetComponent<Text>().text = "资产价值";
                    break;
                case SystemDataPool.ZAN:
                    TopPar.transform.Find("Text").GetComponent<Text>().text = "点赞数量";
                    break;
            }

            InitScrollView(RankInfoList.Count);
        }

    }

    public void SetScroll()
    {
        if (IsRankScrollViewInit == false)
        {
            IsRankScrollViewInit = true;
            RankScrollView.InitListView(1, OnGetItemByIndex);
        }
    }


    void InitScrollView(int m_TotalCount)
    {
        //Debug.Log(RankChatUser.Count+"rankchatuser");
        Debug.Log(RankInfoList.Count + "rankinfolist");


        RankScrollView.SetListItemCount(m_TotalCount);
        RankScrollView.RefreshAllShownItem();

    }
    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= RankInfoList.Count)
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
        LoopListViewItem2 item = listView.NewListViewItem("RankInfo");
        //ListItem2 itemScript = item.GetComponent<ListItem2>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            //itemScript.Init();
        }
        UpdateRankItem(item.transform, index);
        return item;
    }
    void UpdateRankItem(Transform tran, int idx)
    {
        tran.Find("RankText").GetChild(0).gameObject.SetActive(false);
        tran.Find("RankText").GetChild(1).gameObject.SetActive(false);
        tran.Find("RankText").GetChild(2).gameObject.SetActive(false);
        tran.Find("RankText").GetComponent<Text>().enabled = true;
        tran.Find("GoToBtn").gameObject.SetActive(true);
        tran.Find("Self").gameObject.SetActive(false);
        //itemScript.SetItemData(itemData, index);
        //item.transform.Find("Text").GetComponent<Text>().text = index.ToString();
        tran.name = RankInfoList[idx].accountId.ToString();
        tran.Find("RankText").GetComponent<Text>().text = (idx + 1).ToString();
        tran.Find("UserName").GetComponent<Text>().text = RankInfoList[idx].name;
        PublicFunc.CreateHeadImg(tran.Find("HeadImage").GetComponent<Image>(), (long)RankInfoList[idx].modelId);
        switch (Target_ReqGRM.rankType)
        {
            case SystemDataPool.DCOST:
                tran.Find("ZiChanText").Find("Text").gameObject.SetActive(false);
                tran.Find("ZiChanText").GetComponent<Text>().text = (RankInfoList[idx].info / XiShu).ToString();
                break;
            case SystemDataPool.ZAN:
                tran.Find("ZiChanText").Find("Text").gameObject.SetActive(true);
                tran.Find("ZiChanText").GetComponent<Text>().text = RankInfoList[idx].info.ToString();
                foreach (var Key in RankJiangLiDic.Keys)
                {
                    if (Key.Length == 1)
                    {
                        if (idx+1 == int.Parse(Key))
                        {
                            tran.Find("ZiChanText").Find("Text").GetComponent<Text>().text = RankJiangLiDic[Key]+"元现金奖励";
                            return;
                        }
                    }
                    else
                    {
                        int min = int.Parse(Key.Substring(0, Key.IndexOf("-")));
                        int max = int.Parse(Key.Substring(Key.IndexOf("-")+1));
                        if (idx+1 >= min && idx+1 <= max)
                        {
                            tran.Find("ZiChanText").Find("Text").GetComponent<Text>().text = RankJiangLiDic[Key] + "元现金奖励";
                            return;
                        }
                    }
                }
                break;
            default:
                tran.Find("ZiChanText").Find("Text").gameObject.SetActive(false);
                tran.Find("ZiChanText").GetComponent<Text>().text = RankInfoList[idx].info.ToString();
                break;
        }
            //item.transform.Find("ZiChanText").GetComponent<Text>().text = RankInfoList[index].info.ToString();
            ClickListener.Get(tran.transform.Find("GoToBtn").gameObject).onClick = clickGoToBtn;
        switch (idx)
        {
            case 0:
                tran.Find("RankText").GetChild(idx).gameObject.SetActive(true);
                tran.Find("RankText").GetComponent<Text>().enabled = false;
                break;
            case 1:
                tran.Find("RankText").GetChild(idx).gameObject.SetActive(true);
                tran.Find("RankText").GetComponent<Text>().enabled = false;
                break;
            case 2:
                tran.Find("RankText").GetChild(idx).gameObject.SetActive(true);
                tran.Find("RankText").GetComponent<Text>().enabled = false;
                break;
            default:
                tran.Find("RankText").GetChild(0).gameObject.SetActive(false);
                tran.Find("RankText").GetChild(1).gameObject.SetActive(false);
                tran.Find("RankText").GetChild(2).gameObject.SetActive(false);
                tran.Find("RankText").GetComponent<Text>().enabled = true;
                tran.Find("GoToBtn").gameObject.SetActive(true);
                tran.Find("Self").gameObject.SetActive(false);
                break;
        }
        if (tran.name == DataMgr.m_account.id.ToString())
        {
            tran.Find("GoToBtn").gameObject.SetActive(false);
            tran.Find("Self").gameObject.SetActive(true);
        }
    }
    void clickGoToBtn(GameObject obj)
    {
        ReqGetPlayerByIdMessage ReqGPBIM = new ReqGetPlayerByIdMessage();
        List<long?> Account = new List<long?>();
        Account.Add(long.Parse(obj.transform.parent.name));
        ReqGPBIM.accountIds = Account;
        ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetPlayerByIdMessage, ReqGPBIM, EnSocket.Chat);
        IsGoToHomeTwon = true;
    }
    void clickShaiXuanBtn(GameObject obj)
    {
        QuanFuBtn.transform.GetChild(1).GetComponent<Text>().color = new Vector4(0.0313f, 0.4274f, 0.8156f, 1);
        QuanFuBtn.transform.GetChild(0).gameObject.SetActive(false);
        HaoYouBtn.transform.GetChild(1).GetComponent<Text>().color = new Vector4(0.0313f, 0.4274f, 0.8156f, 1);
        HaoYouBtn.transform.GetChild(0).gameObject.SetActive(false);
        if (obj.name == "QuanFuBtn")
        {
            Target_ReqGRM.relationType = SystemDataPool.SERVER;
            ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetRankMessage, Target_ReqGRM);
            obj.transform.GetChild(0).gameObject.SetActive(true);
            obj.transform.GetChild(1).GetComponent<Text>().color = new Vector4(1, 1, 1, 1);
        }
        else if (obj.name == "HaoYouBtn")
        {
            Target_ReqGRM.friendIds = new List<long?>();

            if (DataMgr.m_RspGetSocialityInfoMessage.friendList.Count > 0)
            {
                for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.friendList.Count; i++)
                {
                    Target_ReqGRM.friendIds.Add(DataMgr.m_RspGetSocialityInfoMessage.friendList[i].accountId);
                }
            }
            Target_ReqGRM.friendIds.Add(DataMgr.m_account.id);
            Target_ReqGRM.relationType = SystemDataPool.FRIENDS;
            ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetRankMessage, Target_ReqGRM);
            obj.transform.GetChild(0).gameObject.SetActive(true);
            obj.transform.GetChild(1).GetComponent<Text>().color = new Vector4(1, 1, 1, 1);
        }
    }
    public void clickLefMenuBtn(GameObject obj)
    {
        ReqGetRankMessage ReqGRM = new ReqGetRankMessage();
        switch (obj.name)
        {
            case "0":
                ReqGRM.rankType = SystemDataPool.INCOME;
                rankType = SystemDataPool.INCOME;
                break;
            case "1":
                ReqGRM.rankType = SystemDataPool.DCOST;
                rankType = SystemDataPool.DCOST;
                break;
            case "2":
                ReqGRM.rankType = SystemDataPool.ASSET;
                rankType = SystemDataPool.ASSET;
                break;
            case "3":
                ReqGRM.rankType = SystemDataPool.ZAN;
                rankType = SystemDataPool.ZAN;
                break;
        }
        Target_ReqGRM = ReqGRM;
        clickShaiXuanBtn(QuanFuBtn);
    }
}
