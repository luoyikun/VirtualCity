using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using DG.Tweening;
using Net;
using ProtoDefine;
using SGF.Codec;
using LitJson;
using Framework.Event;
using Newtonsoft.Json;
using UnityScript.Scripting.Pipeline;

public class chatpanel : UGUIPanel
{
    public InputField IF;
    public bool IsZhanKai = false;
    public GameObject BackBtn;
    public static chatpanel cp;
    float SizeDeltaX = 310;//输入框X轴原始的大小
    float AnchoredX = 389;//输入框X轴原始的位置
    public GameObject leftMenuPar;
    public GameObject SearchImage;
    public GameObject CloseZhanKaiBtn;
    public GameObject SysNotifyTmp;
    GameObject OpenMainWindows;
    public static RspGetSocialityInfoMessage m_RspGSIM;
    List<SystemNotify> CreatedSystemNotify = new List<SystemNotify>();
    List<ChatUser> SearchChatUser = new List<ChatUser>();

    public GameObject DownChatWindow;
    public GameObject DownChatRedPoint;
    public Text DownChatText;
    public GameObject ChatWindow;
    public bool IsTopPanel = false;
    public GameObject ObjPar;
    public GameObject MainPar;
    private string chatGroupHeadString = "ChatGroup";
    private string chatPlayerHeadString = "PlayerChat";

    public GameObject SearchPar;

    private GameObject ProxyMain;

    private GameObject SystemNotifyMain;

    private GameObject WorldChatMain;

    public List<chat> CreatedChats = new List<chat>();

    private bool IsInit = false;

    private int leftFriendChildCount = 0;//创建好友的顺序下标

    private int leftOwnGroupChildCount = 0;//创建自己的群的顺序下标

    private int leftInGroupChildCount = 0;//创建加入的群的顺序下标
                                          //public static List<ChatUser> m_AllFriend;

    private void Awake()
    {
        cp = this;

    }

    // Use this for initialization
    void clickOpenChat(GameObject obj)
    {
        //Debug.LogError("ClickOpenChat");
        IsTopPanel = true;
        transform.SetAsLastSibling();
        DownChatRedPoint.SetActive(false);
        ChatWindow.SetActive(true);
    }
    public void CloseChatWindws()
    {
        IsTopPanel = false;
        ChatWindow.SetActive(false);
    }
    public void UpdateDownChatText(string TitalText, string ChatText)
    {
        if (TitalText != "我")
        {
            DownChatRedPoint.SetActive(true);
        }
        DownChatText.text = TitalText + ":" + ChatText;
    }
    public void Init()
    {
        CloseChatWindws();
        if (IsInit == true)
        {
            return;
        }
        IsInit = true;
        IF.onEndEdit.AddListener(delegate { EndInput(IF); });
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        ClickListener.Get(CloseZhanKaiBtn).onClick = clickCloseBtn;
        ClickListener.Get(IF.gameObject).onClick = StartInput;
        ClickListener.Get(DownChatWindow.transform.GetChild(1).gameObject).onClick = clickOpenChat;
        ClickListener.Get(DownChatWindow.transform.GetChild(0).gameObject).onClick = clickOpenChat;
        m_RspGSIM = DataMgr.m_RspGetSocialityInfoMessage;
        InitChat(m_RspGSIM.friendList, m_RspGSIM.inChatGroup, m_RspGSIM.systemNotifies, m_RspGSIM.proxyUsers);
    }
    private void OnEnable()
    {
        //添加网络事件监听
        //NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetSocialityInfoMessage, OnNetEvRspGetSocialityInfoMessage);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.SystemNotifyMessage, OnNetSystemNotifyMessage);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspSearchUserMessage, OnNetSearchUserMessage);


        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetPlayerByIdMessage, OnGetPlayerByIdMessage);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspUpdateUserInfoMessage, OnEvNetUpdateUserInfo);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.ChatMessage, OnEvNetChatMessage);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, OnEvNetCommentMessage);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetProxyUserMessage, OnEvNetGetProxyUserMessage);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspUpdateProxyMessage, OnEvNetUpdateProxyMessage);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspSendMessage, OnEvNetSendMessage);

        EventManager.Instance.AddEventListener(Common.EventStr.BuildHomeTown, OnEvBuildHomeTown);

        EventManager.Instance.AddEventListener(Common.EventStr.BuildHome, OnEvBuildHome);

        //ChatWindow.SetActive(false);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspLeaveGroupMessage, OnNetRspLGM);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspAddFriendMessage, OnNetRspAFM);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspDeleteFriendMessage, OnNetRspDFM);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspJoinGroupMessage, OnNetRspJGM);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspFriendLoginMessage, OnNetFLM);
    }
    public override void OnOpen()
    {
        //IsTopPanel = true;
        Init();
        DownChatWindow.SetActive(true);
    }

    public void OnDisable()
    {
        //NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetSocialityInfoMessage, OnNetEvRspGetSocialityInfoMessage);

        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.SystemNotifyMessage, OnNetSystemNotifyMessage);

        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspSearchUserMessage, OnNetSearchUserMessage);


        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetPlayerByIdMessage, OnGetPlayerByIdMessage);

        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspUpdateUserInfoMessage, OnEvNetUpdateUserInfo);

        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.ChatMessage, OnEvNetChatMessage);

        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, OnEvNetCommentMessage);

        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetProxyUserMessage, OnEvNetGetProxyUserMessage);

        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspUpdateProxyMessage, OnEvNetUpdateProxyMessage);

        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspSendMessage, OnEvNetSendMessage);

        EventManager.Instance.RemoveEventListener(Common.EventStr.BuildHomeTown, OnEvBuildHomeTown);

        EventManager.Instance.RemoveEventListener(Common.EventStr.BuildHome, OnEvBuildHome);

        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspLeaveGroupMessage, OnNetRspLGM);

        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspAddFriendMessage, OnNetRspAFM);

        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspDeleteFriendMessage, OnNetRspDFM);

        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspJoinGroupMessage, OnNetRspJGM);

        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspFriendLoginMessage, OnNetFLM);
    }

    void OnEvNetGetProxyUserMessage(byte[] buf)
    {
        RspGetProxyUserMessage rspGetProxyUserMessage = PBSerializer.NDeserialize<RspGetProxyUserMessage>(buf);
        ProxyMain.GetComponent<ProxyRelation>().Init(rspGetProxyUserMessage.proxyUsers);
    }

    public void SendGetProxyUser()
    {
        ReqGetPorxyUserMessage m_reqGPUM = new ReqGetPorxyUserMessage();
        m_reqGPUM.accountId = DataMgr.m_accountId;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetPorxyUserMessage, m_reqGPUM);
    }
    void OnNetFLM(byte[] buf)
    {
        RspFriendLoginMessage rspFriendLoginMessage = PBSerializer.NDeserialize<RspFriendLoginMessage>(buf);
        SwitchPlayerOnlineState(rspFriendLoginMessage.online, rspFriendLoginMessage.friendId);
    }
    void OnNetRspJGM(byte[] buf)
    {
        RspJoinGroupMessage rspjonJoinGroupMessage = PBSerializer.NDeserialize<RspJoinGroupMessage>(buf);
        UpdateGourpInfo(rspjonJoinGroupMessage.group);
    }
    void OnNetRspDFM(byte[] buf)
    {
        RspDeleteFriendMessage rspDeleteFriendMessage = PBSerializer.NDeserialize<RspDeleteFriendMessage>(buf);
        DeleteChat(rspDeleteFriendMessage.accountId);
        if (UIManager.Instance.IsTopPanel("userinfopanel"))
        {
            if (userinfopanel.uip.Target_ChatUser.accountId == rspDeleteFriendMessage.accountId)
                UIManager.Instance.PopSelf();
        }
    }

    void OnNetRspAFM(byte[] buf)
    {
        RspAddFriendMessage rspAddFriendMessage = PBSerializer.NDeserialize<RspAddFriendMessage>(buf);
        UpdatePlayerInfo(rspAddFriendMessage.chatUser);
    }
    void OnNetRspLGM(byte[] buf)
    {
        RspLeaveGroupMessage rspLeaveGroupMessage = PBSerializer.NDeserialize<RspLeaveGroupMessage>(buf);
        DeleteChat(rspLeaveGroupMessage.groupId, true);
        if (UIManager.Instance.IsTopPanel("chatwindowspanel"))
        {
            UIManager.Instance.PopSelf();
        }
    }
    public override void OnClose()
    {//移除网络事件监听
        //Hint.LoadTips("聊天界面被关闭了！", Color.white);
        //Debug.Log("聊天界面被关闭了！");
        //IsTopPanel = false;
    }

    void OnEvBuildHome(EventData data)
    {
        var exdata = data as EventDataEx<bool>;
        bool isBuild = exdata.GetData();
        if (isBuild == false)
        {
            DownChatWindow.gameObject.SetActive(true);
        }
        else if (isBuild == true)
        {
            DownChatWindow.gameObject.SetActive(false);
        }
    }
    void OnEvNetSendMessage(byte[] buf)
    {
        RspSendMessage m_RspSM = PBSerializer.NDeserialize<RspSendMessage>(buf);
        Hint.LoadTips(m_RspSM.tip, Color.white);
    }
    void OnEvNetUpdateProxyMessage(byte[] buf)
    {
        RspUpdateProxyMessage RspUPM = PBSerializer.NDeserialize<RspUpdateProxyMessage>(buf);
        //RspUPUM.
        ProxyMain.GetComponent<ProxyRelation>().UpdateProxyUser(RspUPM);
    }

    //void OnEvNetGetProxyUserMessage(byte[] buf)
    //{
    //    RspGetProxyUserMessage RspGPUM = PBSerializer.NDeserialize<RspGetProxyUserMessage>(buf);
    //    ChatWindow.transform.Find("Main").Find("ProxyRelation").GetComponent<ProxyRelation>().Init(RspGPUM.proxyUsers);
    //    //InitProxy(RspGPUM.proxyUsers);
    //}

    void OnEvNetCommentMessage(byte[] buf)
    {//获取通用通知
        RspCommentMessage RspCM = PBSerializer.NDeserialize<RspCommentMessage>(buf);
        switch (RspCM.rspcmd)
        {
            //case 508:
            //    if (RspCM.code == 1)
            //    {
            //        //Debug.Log(RspCM.code);
            //        //ReqGetSociality();
            //    }
            //    break;
            //case 507:
            //    if (RspCM.code == 1)
            //    {
            //        //DestroyImmediate(MainPar.transform.Find(RspCM.tip).gameObject);
            //        //DestroyImmediate(leftMenuPar.transform.Find(RspCM.tip).gameObject);
            //        if (leftMenuPar.transform.Find(RspCM.tip).gameObject != null)
            //        {
            //            DestroyImmediate(leftMenuPar.transform.Find(RspCM.tip).gameObject);
            //        }

            //    }
            //    break;
        }
    }
    void OnEvBuildHomeTown(EventData Data)
    {
        var exdata = Data as EventDataEx<bool>;
        bool isBuild = exdata.GetData();
        if (isBuild == false)
        {
            DownChatWindow.gameObject.SetActive(true);
        }
        else if (isBuild == true)
        {
            DownChatWindow.gameObject.SetActive(false);
        }
    }
    void OnEvNetChatMessage(byte[] buf)
    {//聊天信息响应
        ChatMessage ChatM = PBSerializer.NDeserialize<ChatMessage>(buf);
        switch (ChatM.messageFunc)
        {
            case 0:
                switch (ChatM.messageType)
                {
                    case SocialityDataPool.FRIEND:
                        if (ChatM.accountId == DataMgr.m_accountId)
                        {//如果消息返回来了，说明对方已经离线
                            SwitchPlayerOnlineState(0, (long)ChatM.accountId);
                            Hint.LoadTips("对方已离线，你的信息可能无法送达", Color.white);
                            return;
                        }
                        OpenLeftRedPoint(chatPlayerHeadString + ChatM.accountId);
                        //OpenLeftRedPoint(chatPlayerHeadString + ChatM.accountId);
                        for (int i = 0; i < CreatedChats.Count; i++)
                        {
                            if (CreatedChats[i].targetChatUser == null)
                            {
                                continue;
                            }

                            if (CreatedChats[i].targetChatUser.accountId == ChatM.accountId)
                            {
                                CreatedChats[i].UpdateOtherChatMessage(ChatM.message);
                                SwitchPlayerOnlineState(1,CreatedChats[i].ID);
                                return;
                            }
                        }

                        for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.friendList.Count; i++)
                        {
                            if (DataMgr.m_RspGetSocialityInfoMessage.friendList[i].accountId == ChatM.accountId)
                            {
                                CreateChatMain(DataMgr.m_RspGetSocialityInfoMessage.friendList[i]);

                                for (int j = 0; j < CreatedChats.Count; j++)
                                {
                                    if (CreatedChats[j].targetChatUser == null)
                                    {
                                        continue;
                                    }

                                    if (CreatedChats[j].targetChatUser.accountId == ChatM.accountId)
                                    {
                                        CreatedChats[j].UpdateOtherChatMessage(ChatM.message);
                                        UpdateDownChatText(ChatM.name, ChatM.message);
                                        return;
                                    }
                                }
                            }
                        }
                        //MainPar.transform.Find(chatPlayerHeadString + ChatM.Id).GetComponent<chat>().UpdateOtherChatMessage(ChatM.message);
                        break;
                    case SocialityDataPool.GROUP:
                        if (ChatM.accountId == DataMgr.m_accountId)
                        {
                            return;
                        }
                        for (int i = 0; i < CreatedChats.Count; i++)
                        {
                            if (CreatedChats[i].targetChatGroup == null)
                            {
                                continue;
                            }

                            if (CreatedChats[i].targetChatGroup.Id == ChatM.Id)
                            {
                                OpenLeftRedPoint(chatGroupHeadString + ChatM.Id);
                                CreatedChats[i].UpdateOtherChatMessage(ChatM.message, ChatM.accountId, ChatM.name, ChatM.modleId);
                                UpdateDownChatText("[" + CreatedChats[i].targetChatGroup.Name + "]" + ChatM.name, ChatM.message);
                            }
                        }
                        break;
                    case SocialityDataPool.WORLD:
                        if (ChatM.accountId == DataMgr.m_accountId)
                        {
                            return;
                        }
                        WorldChatMain.GetComponent<chat>().UpdateOtherChatMessage(ChatM.message, ChatM.accountId, ChatM.name, ChatM.modleId);
                        UpdateDownChatText("[世界聊天]" + ChatM.name, ChatM.message);
                        break;
                }
                break;
            case 1:
                switch (ChatM.messageType)
                {
                    case SocialityDataPool.FRIEND:
                        if (ChatM.accountId == DataMgr.m_accountId)
                        {//如果消息返回来了，说明对方已经离线
                            SwitchPlayerOnlineState(0, (long)ChatM.accountId);
                            Hint.LoadTips("对方已离线，你的信息可能无法送达", Color.white);
                            return;
                        }
                        OpenLeftRedPoint(chatPlayerHeadString + ChatM.accountId);
                        for (int i = 0; i < CreatedChats.Count; i++)
                        {
                            if (CreatedChats[i].targetChatUser == null)
                            {
                                continue;
                            }

                            if (CreatedChats[i].targetChatUser.accountId == ChatM.accountId)
                            {
                                CreatedChats[i].UpdateOtherInviteMessage(ChatM.message);
                                UpdateDownChatText(ChatM.name, "邀请你去他的家园");
                                SwitchPlayerOnlineState(1, CreatedChats[i].ID);
                                return;
                            }
                        }
                        break;
                    case SocialityDataPool.GROUP:
                        if (ChatM.accountId == DataMgr.m_accountId)
                        {
                            return;
                        }
                        for (int i = 0; i < CreatedChats.Count; i++)
                        {
                            if (CreatedChats[i].targetChatGroup == null)
                            {
                                continue;
                            }

                            if (CreatedChats[i].targetChatGroup.Id == ChatM.Id)
                            {
                                OpenLeftRedPoint(chatGroupHeadString + ChatM.Id);
                                CreatedChats[i].UpdateOtherInviteMessage(ChatM.message, ChatM.accountId,ChatM.name, ChatM.modleId);
                                UpdateDownChatText("[" + CreatedChats[i].targetChatGroup.Name + "]" + ChatM.name, "邀请你去他的家园");
                            }
                        }
                        break;
                    case SocialityDataPool.WORLD:
                        if (ChatM.accountId == DataMgr.m_accountId)
                        {
                            return;
                        }
                        WorldChatMain.GetComponent<chat>().UpdateOtherInviteMessage(ChatM.message,ChatM.accountId, ChatM.name, ChatM.modleId);
                        UpdateDownChatText("[世界聊天]" + ChatM.name, "邀请大家去他的家园");
                        break;
                }
                break;
        }

    }
    void OnEvNetUpdateUserInfo(byte[] buf)
    {
        RspUpdateUserInfoMessage RspUUIM = PBSerializer.NDeserialize<RspUpdateUserInfoMessage>(buf);
        //long[] friend = new long[] { };
        //if (DataMgr.m_account.friendList != null)
        //{
        //    string friendID = DataMgr.m_account.friendList;
        //    friend = JsonUtil.fromJson<long[]>(friendID);
        //}
        //List<long> friendList = new List<long>(friend);
        //string Friend = "";
        //if (RspUUIM.code == 0)
        //{
        //    Debug.Log("失败");
        //}
        //else if (RspUUIM.code == 1)
        //{
        //    switch (RspUUIM.userInfoMap[0].infoType)
        //    {
        //        case 607:
        //            friendList.Add(long.Parse(RspUUIM.userInfoMap[0].info));
        //            friend = friendList.ToArray();
        //            Friend = JsonUtil.toJson<long[]>(friend);
        //            DataMgr.m_account.friendList = Friend;
        //            break;
        //        case 608:
        //            for (int i = friendList.Count - 1; i >= 0; i--)
        //            {
        //                if (friendList[i] == long.Parse(RspUUIM.userInfoMap[0].info))
        //                {
        //                    friendList.RemoveAt(i);
        //                }
        //            }
        //            friend = friendList.ToArray();
        //            Friend = JsonUtil.toJson<long[]>(friend);
        //            DataMgr.m_account.friendList = Friend;
        //            break;
        //    }
        //}
        //// ReqGetSociality();
        //clickCloseBtn(CloseZhanKaiBtn);
    }
    void OnGetPlayerByIdMessage(byte[] buf)
    {
        RspGetPlayerByIdMessage RspGPBIM = PBSerializer.NDeserialize<RspGetPlayerByIdMessage>(buf);
        //if (SearchChatUserID != null)
        //{//判断之前是否有搜索到的Chat
        //    if (MainPar.transform.Find(SearchChatUserID.ToString()) == null)
        //    {
        //        GameObject GroupChat = PublicFunc.CreateTmp(MainPar.transform.GetChild(1).gameObject, MainPar.transform);
        //        GroupChat.name = SearchChatUserID.ToString();
        //        GroupChat.GetComponent<chat>().State = SocialityDataPool.GROUP;
        //        GroupChat.GetComponent<chat>().targetChatUser = RspGPBIM.users[0];
        //        GroupChat.GetComponent<chat>().Init();
        //        GroupChat.gameObject.SetActive(false);
        //    }
        //    if (MainPar.transform.Find(SearchChatUserID.ToString()) != null)
        //    {
        //        MainPar.transform.Find(SearchChatUserID.ToString()).GetComponent<chat>().targetChatUser = RspGPBIM.users[0];
        //        MainPar.transform.Find(SearchChatUserID.ToString()).GetComponent<chat>().UpdateOtherChatMessage(SearchChatMessage);
        //    }
        //    UpdateDownChatText("[聊天群]" + RspGPBIM.users[0].userName, SearchChatMessage);
        //    leftMenuPar.transform.Find(SearchChatUserID.ToString()).Find("RedPoint").gameObject.SetActive(true);
        //}
        //else if (SearchChatUserID == null)
        //{
        //    if (MainPar.transform.Find("WorldChatNews") == null)
        //    {
        //        GameObject WorldChat = PublicFunc.CreateTmp(MainPar.transform.GetChild(1).gameObject, MainPar.transform);
        //        WorldChat.name = "WorldChatNews";
        //        WorldChat.GetComponent<chat>().State = SocialityDataPool.WORLD;
        //        WorldChat.GetComponent<chat>().Init();
        //        WorldChat.transform.GetComponent<chat>().targetChatUser = RspGPBIM.users[0];
        //        WorldChat.transform.GetComponent<chat>().UpdateOtherChatMessage(SearchChatMessage);
        //    }
        //    else if (MainPar.transform.Find("WorldChatNews") != null)
        //    {
        //        MainPar.transform.Find("WorldChatNews").transform.GetComponent<chat>().targetChatUser = RspGPBIM.users[0];
        //        MainPar.transform.Find("WorldChatNews").transform.GetComponent<chat>().UpdateOtherChatMessage(SearchChatMessage);
        //    }
        //    UpdateDownChatText("[世界]" + RspGPBIM.users[0].userName, SearchChatMessage);
        //}
        if (IsTopPanel == true)
        {//判断聊天界面是否在最顶层
            if (RspGPBIM.users[0].accountId == DataMgr.m_accountId)
            {
                return;
            }
            UIManager.Instance.PushPanel(UIPanelName.userinfopanel, false, true, (paragm) =>
            {
                //paragm.GetComponent<userinfopanel>().Target_ChatUser = RspGPBIM.users[0];
                paragm.GetComponent<userinfopanel>().Init(RspGPBIM.users[0]);
            }
);
        }

    }
    void OnNetSearchUserMessage(byte[] buf)
    {
        RspSearchUserMessage RspSUM = PBSerializer.NDeserialize<RspSearchUserMessage>(buf);
        if (IsTopPanel == false)
        {
            return;
        }

        if (RspSUM.userName == null)
        {
            Hint.LoadTips("没有找到用户");
            return;
        }
        for (int i = 0; i < RspSUM.userName.Count; i++)
        {
            bool IsHas = false;
            for (int j = 0; j < SearchChatUser.Count; j++)
            {
                if (SearchChatUser[j].accountId == RspSUM.userName[i].accountId)
                {
                    IsHas = true;
                    break;
                }
            }

            if (IsHas == true)
            {
                continue;
            }
            SearchChatUser.Add(RspSUM.userName[i]);
        }
        CreateSearch(SearchChatUser);
    }
    void OnNetSystemNotifyMessage(byte[] buf)
    {
        SystemNotifyMessage SysNM = PBSerializer.NDeserialize<SystemNotifyMessage>(buf);
        DownChatRedPoint.SetActive(true);
        CreateSystemNotify(SysNM.systemNotify);
        OpenLeftRedPoint("SystemNews");
        UpdateDownChatText("[系统通知]", SysNM.systemNotify.Content);
    }

    void CreateSystemNotify(SystemNotify mSystemNotify)
    {
        if (CreatedSystemNotify.Count != 0)
        {
            foreach (SystemNotify targetNotify in CreatedSystemNotify)
            {
                if (targetNotify.Id == mSystemNotify.Id)
                {
                    Debug.LogWarning("已创建该系统通知" + mSystemNotify.Id);
                    return;
                }
            }
        }

        if (mSystemNotify.HasHandle == 1)
        {
            Debug.LogWarning("该系统通知已经处理过" + mSystemNotify.Id);
            return;
        }
        GameObject SystemNotifyObj = PublicFunc.CreateTmp(SysNotifyTmp, SystemNotifyMain.transform.GetChild(1).GetChild(0).GetChild(0).transform);

        //SysNotifyMassage mSystemNotifyMessage = SystemNotifyObj.GetComponent<SysNotifyMassage>();

        SystemNotifyObj.GetComponent<SysNotifyMassage>().m_systemnotify = mSystemNotify;

        SystemNotifyObj.GetComponent<SysNotifyMassage>().Init();

        CreatedSystemNotify.Add(mSystemNotify);

    }
    public void InitChat(List<ChatUser> friendList, List<ChatGroup> inChatGroups, List<SystemNotify> systemNotifies, List<ProxyUser> proxyUsers)
    {
        if (chatLeftBtn.clb == null)
        {
            chatLeftBtn.clb = ChatWindow.transform.Find("ChatLeftMenu").GetComponent<chatLeftBtn>();
        }
        InitSystemNotifies(systemNotifies);

        InitWorldChat();

        InitOwnGroup(inChatGroups);

        InitProxy(proxyUsers);

        InitInChatGourp(inChatGroups);

        InitFriend(friendList);
        chatLeftBtn.clb.Init();
        chatLeftBtn.clb.clickLeftMenuBtn(leftMenuPar.transform.Find("SystemNews").gameObject);
    }

    void InitWorldChat()
    {
        GameObject wordChatLeft = PublicFunc.CreateTmp(ObjPar.transform.Find("WorldChat").gameObject, leftMenuPar.transform);//左边栏的创建

        wordChatLeft.name = "WorldChat";

        WorldChatMain = PublicFunc.CreateTmp(ObjPar.transform.Find("Chat").gameObject, MainPar.transform);//主界面的创建

        WorldChatMain.name = "WorldChat";//主面板界面的名字要和左边栏的对应

        WorldChatMain.GetComponent<chat>().State = SocialityDataPool.WORLD;

        WorldChatMain.GetComponent<chat>().Init();

        WorldChatMain.SetActive(false);
    }
    void InitSystemNotifies(List<SystemNotify> systemNotifies)
    {
        GameObject SysNotifyLeft = PublicFunc.CreateTmp(ObjPar.transform.Find("SystemNews").gameObject, leftMenuPar.transform);//左侧按钮创建

        SysNotifyLeft.name = "SystemNews";

        SysNotifyLeft.GetComponent<ChatType>().m_tag = ChatType.TypeTag.SystemNews;

        SystemNotifyMain = PublicFunc.CreateTmp(ObjPar.transform.Find("SystemNewsMain").gameObject, MainPar.transform);//主面板界面创建

        SystemNotifyMain.name = "SystemNewsMain";

        SystemNotifyMain.SetActive(false);
        if (systemNotifies.Count == 0)
        {
            Debug.Log("没有系统通知");
            return;
        }
        for (int i = 0; i < systemNotifies.Count; i++)
        {
            CreateSystemNotify(systemNotifies[i]);
        }
    }
    void InitProxy(List<ProxyUser> proxyUserList)
    {
        GameObject ProxyLeft = PublicFunc.CreateTmp(ObjPar.transform.Find("ProxyRelation").gameObject, leftMenuPar.transform);//左边菜单按钮创建

        ProxyLeft.name = "ProxyRelation";

        ProxyMain = PublicFunc.CreateTmp(ObjPar.transform.Find("ProxyRelationMain").gameObject, MainPar.transform);//主界面创建

        ProxyMain.name = "ProxyRelationMain";

        ProxyMain.GetComponent<ProxyRelation>().Init(proxyUserList);//脚本初始化

        ProxyMain.SetActive(false);
    }
    void InitInChatGourp(List<ChatGroup> chatGroups)
    {
        leftInGroupChildCount = leftMenuPar.transform.childCount;//获取当前左边栏已有的子物体的总数,每次从这个位置开始创建
        Debug.Log("LeftInGroupChildCount" + leftInGroupChildCount);
        for (int i = 0; i < chatGroups.Count; i++)
        {
            if (chatGroups[i].AccountId == DataMgr.m_accountId)
            {
                continue;
            }
            CreateChat(chatGroups[i]);
        }
    }

    void InitOwnGroup(List<ChatGroup> chatGroups)
    {
        leftOwnGroupChildCount = leftMenuPar.transform.childCount;//获取当前左边栏已有的子物体的总数,每次从这个位置开始创建
        Debug.Log(leftOwnGroupChildCount + "LeftOwnGroupChildCount");
        ChatGroup TargetChatGroup = null;
        for (int i = 0; i < chatGroups.Count; i++)
        {
            if (chatGroups[i].AccountId != DataMgr.m_accountId)
            {
                continue;
            }
            CreateChat(chatGroups[i]);
            TargetChatGroup = chatGroups[i];
        }

        if (TargetChatGroup == null)
        {
            //如果没有自己曾经创建的群，创建一个创建群对象
            createGroup();
        }
    }
    void InitFriend(List<ChatUser> friendList)
    {

        leftFriendChildCount = leftMenuPar.transform.childCount;//获取当前左边栏已有的子物体的总数,每次从这个位置开始创建
        if (friendList.Count == 0)
        {
            Debug.Log("好友数量为0");
            return;
        }
        Debug.Log(leftFriendChildCount + "LeftFriendChildCount");
        for (int i = 0; i < friendList.Count; i++)
        {
            CreateChat(friendList[i]);
        }

        //为防止好友过多，一次性创建大量的聊天界面，所以把创建聊天界面放到了点击事件里，如果不点击玩家进行聊天，则不会创建对应的聊天界面   ---已取消，在某些状态下会导致无法创建主面板的问题 

    }
    public void clickHeadImage(GameObject obj)
    {//点击用户头像之后，向服务器申请玩家数据
        SendGetPlayerByIdMessage(long.Parse(obj.name));
    }

    public void SendGetPlayerByIdMessage(long ID)
    {
        ReqGetPlayerByIdMessage ReqGPBIM = new ReqGetPlayerByIdMessage();
        ReqGPBIM.accountIds = new List<long?>();
        ReqGPBIM.accountIds.Add(ID);
        ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetPlayerByIdMessage, ReqGPBIM, EnSocket.Chat);
    }
    public void clickBackBtn(GameObject obj)
    {
        //UIManager.Instance.PopSelf();
        IsTopPanel = false;
        ChatWindow.SetActive(false);
    }
    public void clickChaKan(GameObject obj)
    {//系统通知里的同意按钮的触发事件
        SystemNotifyMessage _SystemNotify = new SystemNotifyMessage();
        _SystemNotify.systemNotify = new SystemNotify();
        _SystemNotify.systemNotify = obj.transform.parent.parent.GetComponent<SysNotifyMassage>().m_systemnotify;
        _SystemNotify.systemNotify.AccountId = DataMgr.m_account.id;
        _SystemNotify.systemNotify.HasHandle = 1;
        _SystemNotify.systemNotify.HandleRes = "1";
        switch (obj.transform.parent.parent.GetComponent<SysNotifyMassage>().m_MassgeType)
        {
            case SysNotifyMassage.MassageType.SysNotification:
                _SystemNotify.systemNotify.Type = (int)SysNotifyMassage.MassageType.SysNotification;
                break;
            case SysNotifyMassage.MassageType.AddFriend:
                _SystemNotify.systemNotify.Type = (int)SysNotifyMassage.MassageType.AddFriend;
                break;
            case SysNotifyMassage.MassageType.Invatation:
                _SystemNotify.systemNotify.Type = (int)SysNotifyMassage.MassageType.Invatation;
                break;
        }
        DestroyImmediate(obj.transform.parent.parent.gameObject);
        ChatSocket.Instance.SendMsgProto(MsgIdDefine.SystemNotifyMessage, _SystemNotify, EnSocket.Chat);
        //ReqGetSociality();
    }
    public void clickHuLue(GameObject obj)
    {//系统通知里的忽略按钮的触发事件
        SystemNotifyMessage _SystemNotify = new SystemNotifyMessage();
        _SystemNotify.systemNotify = new SystemNotify();
        _SystemNotify.systemNotify = obj.transform.parent.GetComponent<SysNotifyMassage>().m_systemnotify;
        _SystemNotify.systemNotify.AccountId = DataMgr.m_account.id;
        _SystemNotify.systemNotify.HasHandle = 1;
        _SystemNotify.systemNotify.HandleRes = "0";
        switch (obj.transform.parent.GetComponent<SysNotifyMassage>().m_MassgeType)
        {
            case SysNotifyMassage.MassageType.SysNotification:
                _SystemNotify.systemNotify.Type = (int)SysNotifyMassage.MassageType.SysNotification;
                break;
            case SysNotifyMassage.MassageType.AddFriend:
                _SystemNotify.systemNotify.Type = (int)SysNotifyMassage.MassageType.AddFriend;
                break;
            case SysNotifyMassage.MassageType.Invatation:
                _SystemNotify.systemNotify.Type = (int)SysNotifyMassage.MassageType.Invatation;
                break;
        }
        DestroyImmediate(obj.transform.parent.gameObject);
        ChatSocket.Instance.SendMsgProto(MsgIdDefine.SystemNotifyMessage, _SystemNotify, EnSocket.Chat);
    }
    public void StartInput(GameObject obj)
    {//完成输入之后开启搜索
        if (IsZhanKai == false)
        {
            Tween move = DOTween.To(() => BackBtn.GetComponent<RectTransform>().anchoredPosition, r => BackBtn.GetComponent<RectTransform>().anchoredPosition = r, new Vector2(-124, BackBtn.GetComponent<RectTransform>().anchoredPosition.y), 0.5f);
            RectTransform rt = IF.transform.GetComponent<RectTransform>();
            //AnchoredX = rt.anchoredPosition.x;//记录输入框原始的位置
            //SizeDeltaX = rt.sizeDelta.x;//记录输入框原始的尺寸
            SearchImage.SetActive(false);
            CloseZhanKaiBtn.SetActive(true);
            Tween zhankaiAnchored = DOTween.To(() => rt.anchoredPosition, r => rt.anchoredPosition = r, new Vector2(286, rt.anchoredPosition.y), 0.5f);
            Tween zhankaiSizeDelta = DOTween.To(() => rt.sizeDelta, r => rt.sizeDelta = r, new Vector2(516, rt.sizeDelta.y), 0.5f);
            IsZhanKai = true;
        }
    }
    void clickCloseBtn(GameObject obj)
    {//搜索按钮的关闭事件
        if (IsZhanKai == true)
        {
            IF.text = "";
            SearchImage.SetActive(true);
            Tween move = DOTween.To(() => BackBtn.GetComponent<RectTransform>().anchoredPosition, r => BackBtn.GetComponent<RectTransform>().anchoredPosition = r, new Vector2(124, BackBtn.GetComponent<RectTransform>().anchoredPosition.y), 0.5f);
            RectTransform rt = IF.transform.GetComponent<RectTransform>();
            Tween zhankaiAnchored = DOTween.To(() => rt.anchoredPosition, r => rt.anchoredPosition = r, new Vector2(AnchoredX, rt.anchoredPosition.y), 0.5f);
            Tween zhankaiSizeDelta = DOTween.To(() => rt.sizeDelta, r => rt.sizeDelta = r, new Vector2(SizeDeltaX, rt.sizeDelta.y), 0.5f);
            obj.SetActive(false);
            IsZhanKai = false;
            SearchPar.transform.parent.parent.gameObject.SetActive(false);
            chatLeftBtn.clb.gameObject.SetActive(true);
        }

    }

    public void UpdateGourpInfo(ChatGroup mChatGroup)
    {
        //if (DataMgr.m_RspGetSocialityInfoMessage.inChatGroup.Count == 0)
        //{
        //    //列表里没有这个群，那么就把这个群加入到列表里，并创建对象
        //    DataMgr.m_RspGetSocialityInfoMessage.inChatGroup.Add(mChatGroup);
        //    CreateChat(mChatGroup);
        //    return;
        //}
        for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.inChatGroup.Count; i++)
        {
            if (DataMgr.m_RspGetSocialityInfoMessage.inChatGroup[i].Id == mChatGroup.Id)
            {//如果在列表中找到了这个群，就把这个群的替换，并且更新按钮上的信息
                DataMgr.m_RspGetSocialityInfoMessage.inChatGroup[i] = mChatGroup;
                leftMenuPar.transform.Find(chatGroupHeadString + DataMgr.m_RspGetSocialityInfoMessage.inChatGroup[i].Id).GetComponent<ChatType>().UpdateInfo(DataMgr.m_RspGetSocialityInfoMessage.inChatGroup[i]);
                return;
            }

        }
        //列表里没有这个群，那么就把这个群加入到列表里，并创建对象
        DataMgr.m_RspGetSocialityInfoMessage.inChatGroup.Add(mChatGroup);
        CreateChat(mChatGroup);
    }

    void CloseCreateGourpUI()
    {
        if (leftMenuPar.transform.Find("CreateGourp") != null)
            leftMenuPar.transform.Find("CreateGourp").gameObject.SetActive(false);
    }

    public void DeleteChat(long id, bool IsChatGroup = false)
    {
        GameObject destroyObj;
        if (IsChatGroup == true)
        {//删除群
            destroyObj = leftMenuPar.transform.Find(chatGroupHeadString + id).gameObject;//左侧菜单删除
            destroyObj.SetActive(false);
            DestroyImmediate(destroyObj);
            destroyObj = null;


            destroyObj = MainPar.transform.Find(chatGroupHeadString + id).gameObject;//主面板界面删除
            destroyObj.SetActive(false);
            DestroyImmediate(destroyObj);
            destroyObj = null;

            ChatGroup targetChatGroup = null;
            for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.inChatGroup.Count; i++)
            {
                if (DataMgr.m_RspGetSocialityInfoMessage.inChatGroup[i].Id == id)
                {
                    targetChatGroup = DataMgr.m_RspGetSocialityInfoMessage.inChatGroup[i];
                    break;
                }
            }

            if (targetChatGroup.AccountId == DataMgr.m_accountId)//如果删的是自己创建的群，则减少加入群和好友的创建下标
            {
                leftFriendChildCount--;
                leftInGroupChildCount--;
            }


            for (int i = 0; i < CreatedChats.Count; i++)
            {
                if (CreatedChats[i].targetChatGroup != null)
                {
                    if (CreatedChats[i].targetChatGroup.Id == targetChatGroup.Id)
                    {
                        CreatedChats.Remove(CreatedChats[i]);//从已创建的聊天中移除这个聊天
                        break;
                    }
                }
            }

            if (targetChatGroup != null)
            {
                DataMgr.m_RspGetSocialityInfoMessage.inChatGroup.Remove(targetChatGroup);
                targetChatGroup = null;//从已有的群里移除这个群
            }

            for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.inChatGroup.Count; i++)
            {
                if (DataMgr.m_RspGetSocialityInfoMessage.inChatGroup[i].AccountId == DataMgr.m_accountId)
                {
                    targetChatGroup = DataMgr.m_RspGetSocialityInfoMessage.inChatGroup[i];
                }
            }

            if (targetChatGroup == null)
            {
                //如果没有自己曾经创建的群，创建一个创建群对象
                createGroup();
            }
        }
        else
        {//删除私人聊天
            destroyObj = leftMenuPar.transform.Find(chatPlayerHeadString + id).gameObject;//左侧栏删除
            destroyObj.SetActive(false);
            DestroyImmediate(destroyObj);
            destroyObj = null;

            if (MainPar.transform.Find(chatPlayerHeadString + id) != null)
            {
                destroyObj = MainPar.transform.Find(chatPlayerHeadString + id).gameObject;//主面板界面删除
                destroyObj.SetActive(false);
                DestroyImmediate(destroyObj);
                destroyObj = null;
            }

            ChatUser targetChatUser = null;

            for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.friendList.Count; i++)
            {
                if (DataMgr.m_RspGetSocialityInfoMessage.friendList[i].accountId == id)
                {
                    targetChatUser = DataMgr.m_RspGetSocialityInfoMessage.friendList[i];
                    break;
                }
            }

            for (int i = 0; i < CreatedChats.Count; i++)
            {
                if (CreatedChats[i].targetChatUser != null)
                {
                    if (CreatedChats[i].targetChatUser.accountId == targetChatUser.accountId)
                    {
                        CreatedChats.Remove(CreatedChats[i]);//从已创建的聊天中移除这个聊天
                        break;
                    }
                }
            }

            if (targetChatUser != null)
            {
                DataMgr.m_RspGetSocialityInfoMessage.friendList.Remove(targetChatUser);
                targetChatUser = null;
            }

        }
        Resources.UnloadUnusedAssets();
    }

    void createGroup()
    {
        //如果没有自己曾经创建的群，创建一个创建群按钮

        GameObject createGroupLeft = PublicFunc.CreateTmp(ObjPar.transform.Find("UserQunNews").gameObject, leftMenuPar.transform);

        ClickListener.Get(createGroupLeft).onClick = chatLeftBtn.clb.clickLeftMenuBtn;
        createGroupLeft.name = "CreateGroup";
        createGroupLeft.transform.Find("Name").GetComponent<Text>().text = "创建新群";
        createGroupLeft.transform.SetSiblingIndex(leftOwnGroupChildCount);
    }
    public void UpdatePlayerInfo(ChatUser mChatUser)
    {
        for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.friendList.Count; i++)
        {
            if (DataMgr.m_RspGetSocialityInfoMessage.friendList[i].accountId == mChatUser.accountId)
            {//如果在列表中找到了这个好友，就替换对应的ChatUser，并更新按钮上的信息
                DataMgr.m_RspGetSocialityInfoMessage.friendList[i] = mChatUser;
                leftMenuPar.transform.Find("PlayerChat" + DataMgr.m_RspGetSocialityInfoMessage.friendList[i].accountId).GetComponent<ChatType>().UpdateInfo(DataMgr.m_RspGetSocialityInfoMessage.friendList[i]);
                return;
            }
        }
        //列表里没有这个人，把这个人加入到列表里，并创建对应按钮
        DataMgr.m_RspGetSocialityInfoMessage.friendList.Add(mChatUser);
        CreateChat(mChatUser);
    }
    public void clickChatLeftBtn(GameObject obj)
    {//左侧菜单点击事件
        switch (obj.GetComponent<ChatType>().m_tag)
        {
            case ChatType.TypeTag.SystemNews:
                if (MainPar.transform.Find("SystemNewsMain") == null)
                {
                    Hint.LoadTips("找不到对象SystemNewsMain", Color.white);
                    return;
                }
                MainPar.transform.Find("SystemNewsMain").gameObject.SetActive(true);
                break;
            case ChatType.TypeTag.OwnGroup:

                if (obj.name == "CreateGroup")
                {
                    UIManager.Instance.PushPanel(UIPanelName.chatwindowspanel, false, true, (param) =>
                    {
                        chatwindowspanel.cwp.openWindwos("CreateGroup");
                    });
                    return;
                }
                MainPar.transform.Find(obj.name).gameObject.SetActive(true);
                break;
            case ChatType.TypeTag.InGroup:
                MainPar.transform.Find(obj.name).gameObject.SetActive(true);
                break;
            case ChatType.TypeTag.WorldChatNews:
                if (MainPar.transform.Find(obj.name) == null)
                {
                    Hint.LoadTips("找不到对象WorldChat", Color.white);
                    return;
                }
                MainPar.transform.Find(obj.name).gameObject.SetActive(true);
                break;
            case ChatType.TypeTag.Proxy:
                if (MainPar.transform.Find("ProxyRelationMain") == null)
                {
                    Hint.LoadTips("找不到对象ProxyRelationMain", Color.white);
                    return;
                }
                SendGetProxyUser();
                MainPar.transform.Find("ProxyRelationMain").gameObject.SetActive(true);
                break;
            case ChatType.TypeTag.PersonChat:
                //Debug.LogError(obj.name);
                if (MainPar.transform.Find(obj.name) != null)
                {
                    MainPar.transform.Find(obj.name).gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogError("找不到已经创建的页面" + obj.name);
                }
                //else if (MainPar.transform.Find(obj.name) == null)
                //{
                //    for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.friendList.Count; i++)
                //    {
                //        if (long.Parse(obj.name.Replace(chatPlayerHeadString, "")) ==
                //            DataMgr.m_RspGetSocialityInfoMessage.friendList[i].accountId)
                //        {
                //            CreateChatMain(DataMgr.m_RspGetSocialityInfoMessage.friendList[i]);
                //            MainPar.transform.Find(obj.name).gameObject.SetActive(true);
                //            return;
                //        }
                //    }
                //    Debug.LogWarning("无法创建对应主面板，因为ID为" + long.Parse((obj.name.Replace(chatPlayerHeadString, "")) + "的ChatUser未找到"));
                //}
                break;
            case ChatType.TypeTag.OfflinePersonChat:
                //Debug.LogError(obj.name);
                if (MainPar.transform.Find(obj.name) != null)
                {
                    MainPar.transform.Find(obj.name).gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogError("找不到已经创建的页面" + obj.name);
                }
                //else if (MainPar.transform.Find(obj.name) == null)
                //{
                //    for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.friendList.Count; i++)
                //    {
                //        if (long.Parse(obj.name.Replace(chatPlayerHeadString, "")) ==
                //            DataMgr.m_RspGetSocialityInfoMessage.friendList[i].accountId)
                //        {
                //            CreateChatMain(DataMgr.m_RspGetSocialityInfoMessage.friendList[i]);
                //            MainPar.transform.Find(obj.name).gameObject.SetActive(true);
                //            return;
                //        }
                //    }
                //    Debug.LogWarning("无法创建对应主面板，因为ID为" + long.Parse((obj.name.Replace(chatPlayerHeadString, "")) + "的ChatUser未找到"));
                //}
                break;
            case ChatType.TypeTag.SearchObj:
                UIManager.Instance.PushPanel(UIPanelName.userinfopanel, false, true, paragrm =>
                  {
                      paragrm.GetComponent<userinfopanel>().Init(obj.GetComponent<ChatType>().TargetChatUser);
                  });
                break;
        }
    }
    public void clickSendBtn(GameObject obj)
    {
        ChatMessage ChatM = new ChatMessage();
        ChatM.accountId = (long)DataMgr.m_account.id;
        ChatM.modleId = (int)DataMgr.m_account.modleId;
        ChatM.name = DataMgr.m_account.userName;
        ChatM.message = obj.GetComponent<chat>().IF.transform.Find("Text").GetComponent<Text>().text;
        switch (obj.GetComponent<chat>().State)
        {
            case SocialityDataPool.FRIEND:
                ChatM.messageType = SocialityDataPool.FRIEND;
                ChatM.Id = long.Parse(obj.name.Replace(chatPlayerHeadString, ""));
                break;
            case SocialityDataPool.GROUP:
                ChatM.messageType = SocialityDataPool.GROUP;
                //ChatM.name ="[" +obj.GetComponent<chat>().targetChatGroup.Name+"]";
                ChatM.Id = long.Parse(obj.name.Replace(chatGroupHeadString, ""));
                break;
            case SocialityDataPool.WORLD:
                ChatM.messageType = SocialityDataPool.WORLD;
                //ChatM.name = "[世界聊天]"+DataMgr.m_account.userName;
                ChatM.Id = null;
                break;
        }

        ChatM.messageFunc = 0;
        ChatSocket.Instance.SendMsgProto(MsgIdDefine.ChatMessage, ChatM);
    }

    public void clickInviteBtn(GameObject obj)
    {
        ChatMessage ChatM = new ChatMessage();
        ChatM.accountId = (long)DataMgr.m_account.id;
        ChatM.modleId = (int)DataMgr.m_account.modleId;
        ChatM.name = DataMgr.m_account.userName;

        InviteMessage m_InviteMessage = new InviteMessage();
        m_InviteMessage.AccountID = DataMgr.m_accountId;
        m_InviteMessage.ServerIP = DataMgr.m_account.serverIp;
        m_InviteMessage.ModelID = DataMgr.m_account.modleId;
        m_InviteMessage.UserName = DataMgr.m_account.userName;
        ChatM.message = JsonConvert.SerializeObject(m_InviteMessage);
        switch (obj.GetComponent<chat>().State)
        {
            case SocialityDataPool.FRIEND:
                ChatM.messageType = SocialityDataPool.FRIEND;
                ChatM.Id = long.Parse(obj.name.Replace(chatPlayerHeadString, ""));
                break;
            case SocialityDataPool.GROUP:
                ChatM.messageType = SocialityDataPool.GROUP;
                //ChatM.name ="[" +obj.GetComponent<chat>().targetChatGroup.Name+"]";
                ChatM.Id = long.Parse(obj.name.Replace(chatGroupHeadString, ""));
                break;
            case SocialityDataPool.WORLD:
                ChatM.messageType = SocialityDataPool.WORLD;
                //ChatM.name = "[世界聊天]"+DataMgr.m_account.userName;
                ChatM.Id = null;
                break;
        }

        ChatM.messageFunc = 1;
        ChatSocket.Instance.SendMsgProto(MsgIdDefine.ChatMessage, ChatM);
    }
    public void clickDaLaBaBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.chatwindowspanel, false, true, (param) =>
        {
            chatwindowspanel.cwp.openWindwos("DaLaBa");
        });
    }
    public void EndInput(InputField ipt)
    {
        if (ipt.text == "")
        {
            //Hint.LoadTips("搜索内容不能为空", Color.white);
            return;
        }
        for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.friendList.Count; i++)
        {
            if (DataMgr.m_RspGetSocialityInfoMessage.friendList[i].userName.Contains(ipt.text))
            {
                SearchChatUser.Add(DataMgr.m_RspGetSocialityInfoMessage.friendList[i]);//把好友列表里所有包括了这个字的ChatUser加入要创建的列表里
            }
        }
        ReqSearchUserMessage ReqSUM = new ReqSearchUserMessage();
        ReqSUM.userName = ipt.text;
        ReqSUM.groupId = null;
        ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqSearchUserMessage, ReqSUM, EnSocket.Chat);
        if (SearchPar.transform.childCount != 0)
        {
            for (int i = SearchPar.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(SearchPar.transform.GetChild(i).gameObject);
            }
        }
    }
    public void clickQunSheZhiBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.chatwindowspanel, false, true, (Paragm) =>
        {
            IsTopPanel = false;
            //if (m_RspGSIM.inChatGroup != null)
            //{
            //    for (int i = 0; i < m_RspGSIM.inChatGroup.Count; i++)
            //    {
            //        if (obj.transform.parent.parent.GetComponent<chat>().targetChatGroup.Id == m_RspGSIM.inChatGroup[i].Id)
            //        {
            //            chatwindowspanel.Target_ChatGroup = m_RspGSIM.inChatGroup[i];
            //            break;
            //        }
            //    }
            //}
            chatwindowspanel.Target_ChatGroup = obj.transform.parent.parent.GetComponent<chat>().targetChatGroup;
            Paragm.GetComponent<chatwindowspanel>().Init("GroupSetting");
        });
    }

    void OpenLeftRedPoint(string OpenObj)
    {
        if (chatLeftBtn.clb.clickObjN.name == OpenObj)
        {
            Debug.LogWarning("亮红点的物体已经被点选");
            return;
        }
        if (leftMenuPar.transform.Find(OpenObj) != null)
        {
            leftMenuPar.transform.Find(OpenObj).Find("RedPoint").gameObject.SetActive(true);
        }
    }

    public void CreateChat(ChatUser mChatUser)
    {//好友的创建
        CreateChatLeft(mChatUser);
        CreateChatMain(mChatUser);
    }

    public void CreateChat(ChatGroup mChatGroup)
    {//群的创建
        //创建左边菜单
        CreateChatLeft(mChatGroup);
        //创建主界面聊天
        CreateChatMain(mChatGroup);
    }

    public void CreateSearch(List<ChatUser> mSearChatUsers)
    {
        chatLeftBtn.clb.gameObject.SetActive(false);
        SearchPar.transform.parent.parent.gameObject.SetActive(true);
        for (int i = 0; i < mSearChatUsers.Count; i++)
        {
            if (mSearChatUsers[i].accountId == DataMgr.m_accountId)
            {
                
                continue;
            }
            CreateSearchLeft(mSearChatUsers[i]);
        }
        SearchChatUser.Clear();
    }

    public void CreateSearchLeft(ChatUser mSearChatUser)
    {
        GameObject obj = PublicFunc.CreateTmp(ObjPar.transform.Find("PlayerChat").gameObject, SearchPar.transform);
        obj.name = "Search" + mSearChatUser.accountId;
        obj.transform.
            Find("GreenBG").gameObject.SetActive(false);

        obj.transform.Find("Name").GetComponent<Text>().text = mSearChatUser.userName;

        obj.transform.Find("Name").GetComponent<Text>().color = new Vector4(0.0313f, 0.4274f, 0.8156f, 1);

        PublicFunc.CreateHeadImg(obj.transform.Find("HeadImage").GetComponent<Image>()
            , mSearChatUser.modelId);

        obj.GetComponent<ChatType>().m_tag = ChatType.TypeTag.SearchObj;

        obj.GetComponent<ChatType>().TargetChatUser = mSearChatUser;

        ClickListener.Get(obj).onClick = chatLeftBtn.clb.clickLeftMenuBtn;
    }

    void CreateChatLeft(ChatUser mChatUser)
    {//好友的左侧菜单创建
        GameObject playerChatLeft = null;
        if (leftMenuPar.transform.Find(chatPlayerHeadString + mChatUser.accountId) == null)
        {
            playerChatLeft = PublicFunc.CreateTmp(ObjPar.transform.Find("PlayerChat").gameObject
                , leftMenuPar.transform);//左侧菜单创建

            PublicFunc.CreateHeadImg(playerChatLeft.transform.Find("HeadImage").GetComponent<Image>()
                , mChatUser.modelId);

            playerChatLeft.GetComponent<ChatType>().TargetChatUser = mChatUser;

            playerChatLeft.name = chatPlayerHeadString + mChatUser.accountId;

            playerChatLeft.transform.Find("Name").GetComponent<Text>().text = mChatUser.userName;

            SwitchPlayerOnlineState(mChatUser.online, mChatUser.accountId);

            ClickListener.Get(playerChatLeft).onClick = chatLeftBtn.clb.clickLeftMenuBtn;


        }
    }

    void CreateChatLeft(ChatGroup mChatGroup)
    {//群的左侧菜单创建
        GameObject chatGroup;
        if (leftMenuPar.transform.Find(chatGroupHeadString + mChatGroup.Id) == null)
        {
            chatGroup = PublicFunc.CreateTmp(ObjPar.transform.Find("UserQunNews").gameObject, leftMenuPar.transform);

            ClickListener.Get(chatGroup).onClick = chatLeftBtn.clb.clickLeftMenuBtn;

            chatGroup.name = chatGroupHeadString + mChatGroup.Id;
            chatGroup.transform.Find("Name").GetComponent<Text>().text = mChatGroup.Name;
            if (mChatGroup.AccountId == DataMgr.m_accountId)
            {
                chatGroup.GetComponent<ChatType>().m_tag = ChatType.TypeTag.OwnGroup;
                chatGroup.transform.SetSiblingIndex(leftOwnGroupChildCount);
                CloseCreateGourpUI();
                leftInGroupChildCount++;
                leftFriendChildCount++;//每次创建管理的群，创建加入的群以及创建好友的下标就往下增加
            }
            else
            {//leftInGroupChildCount 必须在InitInChatGroup里赋值
                chatGroup.GetComponent<ChatType>().m_tag = ChatType.TypeTag.InGroup;
                chatGroup.transform.SetSiblingIndex(leftInGroupChildCount);
                leftFriendChildCount++;//创建加入的群，创建好友的下标增加
            }
        }
    }

    void CreateChatMain(ChatUser mChatUser)
    {//好友的主面板创建
        GameObject playerChatMain = null;
        playerChatMain = PublicFunc.CreateTmp(ObjPar.transform.Find("Chat").gameObject, MainPar.transform);//创建一个私人聊天
        playerChatMain.name = chatPlayerHeadString + mChatUser.accountId;
        playerChatMain.GetComponent<chat>().State = SocialityDataPool.FRIEND;
        playerChatMain.GetComponent<chat>().ID = mChatUser.accountId;
        playerChatMain.GetComponent<chat>().Init();
        playerChatMain.GetComponent<chat>().targetChatUser = mChatUser;
        playerChatMain.SetActive(false);
        CreatedChats.Add(playerChatMain.GetComponent<chat>());
        if (mChatUser.online == 0)
        {
            playerChatMain.transform.Find("OfflineText").gameObject.SetActive(true);
        }
        else
        {
            playerChatMain.transform.Find("OfflineText").gameObject.SetActive(false);
        }
        //SwitchPlayerOnlineState(mChatUser.online, mChatUser.accountId);
    }

    void CreateChatMain(ChatGroup mChatGroup)
    {//群的主面板创建
        GameObject chatGroup = null;
        if (MainPar.transform.Find(chatGroupHeadString + mChatGroup.Id) == null)
        {
            chatGroup = PublicFunc.CreateTmp(ObjPar.transform.Find("Chat").gameObject, MainPar.transform);
            chatGroup.name = chatGroupHeadString + mChatGroup.Id;
            chatGroup.GetComponent<chat>().State = SocialityDataPool.GROUP;
            chatGroup.GetComponent<chat>().ID = (long)mChatGroup.Id;
            chatGroup.GetComponent<chat>().targetChatGroup = mChatGroup;
            chatGroup.GetComponent<chat>().Init();
            chatGroup.SetActive(false);
        }
        CreatedChats.Add(chatGroup.GetComponent<chat>());
    }

    void SwitchPlayerOnlineState(int state, long id)
    {
        GameObject playerChatLeft = null;
        
        if (leftMenuPar.transform.Find(chatPlayerHeadString + id) != null)
        {
            playerChatLeft = leftMenuPar.transform.Find(chatPlayerHeadString + id).gameObject;
        }
        GameObject playerChatMain = null;

        if (MainPar.transform.Find(chatPlayerHeadString + id) != null)
        {
            playerChatMain = MainPar.transform.Find(chatPlayerHeadString + id).gameObject;
        }
        if (state == 0)
        {
            if (playerChatLeft != null)
            {
                playerChatLeft.GetComponent<ChatType>().m_tag = ChatType.TypeTag.OfflinePersonChat;
                playerChatLeft.transform.SetAsLastSibling(); //每次创建离线的好友，把他设置为最后一个子物体
                playerChatLeft.transform.Find("HeadImage").GetChild(0).gameObject.SetActive(true);
                for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.friendList.Count; i++)
                {
                    if (DataMgr.m_RspGetSocialityInfoMessage.friendList[i].accountId == id)
                    {
                        DataMgr.m_RspGetSocialityInfoMessage.friendList[i].online = state;
                    }
                }
            }
            if (playerChatMain != null)
            {
                playerChatMain.transform.Find("OfflineText").gameObject.SetActive(true);//打开表示该玩家已离线的Text;
            }
        }
        else
        {
            if (playerChatLeft != null)
            {
                playerChatLeft.GetComponent<ChatType>().m_tag = ChatType.TypeTag.PersonChat;
                playerChatLeft.transform.SetSiblingIndex(leftFriendChildCount); //每次创建在线好友，把他设置为第 已有的子物体总数 个物体
                playerChatLeft.transform.Find("HeadImage").GetChild(0).gameObject.SetActive(false);
                for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.friendList.Count; i++)
                {
                    if (DataMgr.m_RspGetSocialityInfoMessage.friendList[i].accountId == id)
                    {
                        DataMgr.m_RspGetSocialityInfoMessage.friendList[i].online = state;
                    }
                }

            }

            if (playerChatMain != null)
            {
                playerChatMain.transform.Find("OfflineText").gameObject.SetActive(false);//关闭表示该玩家已离线的Text;
            }
        }
    }

    void SwitchPlayerOnlineStateLeft(GameObject LeftObj)
    {

    }

    void SwitchPlayerOnlineStateMain(GameObject MainObj)
    {

    }
}
