#define EditorLoadAb

using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using ProtoDefine;
using SGF.Codec;
using Newtonsoft.Json;
using ProtoBuf;
using Framework.Event;
using UnityEditor;
using System.IO;
using Framework.Tools;

public enum EnCurScene
{
    Home,
    Hometown,
    Business,
    Shop
}

public enum EnServer
{
    MyServer,
    OtherServer
}

public class TestLong
{
    public int code;
    public int buildType = -1;
}

[ProtoContract]
public class MapTest
{
    [ProtoMember(1)]
    public Dictionary<int, int> dic = new Dictionary<int, int>();
}


public class VirtualCityMgr : MonoBehaviour
{

    Transform m_tarns;
    //public Camera m_camera;
    public static VirtualCityMgr m_instance;
    public Transform m_bufferPar;

    //static ChatUser m_otherUser;
    public static bool m_isGetGameData = false;
    public static bool m_isGetChatData = false;
    public string m_bigVersion = "1.0";
    // Use this for initialization
    void Start()
    {
        m_instance = this;

        if (Application.isMobilePlatform == false)
        {
            DataMgr.m_resolution = 1.0f;
        }
        int width = (int)(Screen.currentResolution.width * DataMgr.m_resolution);
        int height = (int)(Screen.currentResolution.height * DataMgr.m_resolution);
        Screen.SetResolution(width, height, true);

        Debug.Log("屏幕分辨率:" + Screen.width + "," + Screen.height);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 30;

        m_tarns = this.transform;
        if (Application.isMobilePlatform == true)
        {
            MyLogCallback.Instance.Startup();
        }

        //if (PublicFunc.IsExistFile("NewGuide.txt") == false)
        //{
        //    DataMgr.m_isNewGuide = true;
        //}
        //启动全局缓冲池
        PoolMgr.Instance.StartUp();

        //启动消息中心
        MessageCenter.Instance.StartUp();
        Framework.Tools.TimeManager.Instance.StartUp();
        SmssMgr.Instance.StartUp();
        Loom.Current.StartUp();
        HttpMgr.Instance.Startup();
        SpProto.StartUp();
#if UNITY_EDITOR && EditorLoadAb

        VcData.Instance.LoadData(Init);
#else
        AssetMgr.Instance.Init(Init);
#endif 

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            AliComponent.Instance.Startup();
            WeChatComponent.Instance.Startup();
        }
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, OnNetRspCommentMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGameDataMessage, OnNetEvGameData);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetSocialityInfoMessage, OnNetEvRspGetSocialityInfoMessage);
        //NetEventManager.Instance.ScriptAddDelegate(this, OnNetEvGameData);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetConnectionMessage, OnNetRspGetConnectionMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspAllocationScenceMessage, OnNetRspAllocationScenceMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetProxyUserMessage, OnEvNetGetProxyUserMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCloseServerMessage, OnNetRspCloseServerMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.ChangeAmountMessage, OnNetChangeAmountMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetRedPacketMessage, OnNetRspGRPM);
    }

    private void OnDestroy()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, OnNetRspCommentMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGameDataMessage, OnNetEvGameData);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetSocialityInfoMessage, OnNetEvRspGetSocialityInfoMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetConnectionMessage, OnNetRspGetConnectionMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspAllocationScenceMessage, OnNetRspAllocationScenceMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetProxyUserMessage, OnEvNetGetProxyUserMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCloseServerMessage, OnNetRspCloseServerMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.ChangeAmountMessage, OnNetChangeAmountMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetRedPacketMessage, OnNetRspGRPM);

    }

    void OnNetRspGRPM(byte[] buf)
    {
        RspGetRedPacketMessage rsp = PBSerializer.NDeserialize<RspGetRedPacketMessage>(buf);

        UIManager.Instance.PushPanel(UIPanelName.hongbaopanel, false, true, paragrm => { paragrm.GetComponent<hongbaopanel>().Init(rsp.amount); });
    }

    void OnNetChangeAmountMessage(byte[] buf)
    {
        ChangeAmountMessage rsp = PBSerializer.NDeserialize<ChangeAmountMessage>(buf);
        float[] bufV = new float[5] { 0, 0, 0, 0, 0 };
        bufV[rsp.amountType] = rsp.amount;
        UpdateWallet((int)(bufV[0]), (int)(bufV[1]), bufV[2], bufV[3], (int)bufV[4]);
    }
    void OnNetRspCloseServerMessage(byte[] buf)
    {

        CloseAllSocket();
        //账号异地登录，强制下线
        GameSocket.Instance.StopHeart();
        ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, false);
        ispanel.SetContent("提示", "服务停机更新，请下线", false);
        ispanel.m_ok = () =>
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif


        };
    }
    void OnEvNetGetProxyUserMessage(byte[] buf)
    {
        RspGetProxyUserMessage RspGPUM = PBSerializer.NDeserialize<RspGetProxyUserMessage>(buf);
        DataMgr.m_ProxyUserList = RspGPUM.proxyUsers;
        //ChatWindow.transform.Find("Main").Find("ProxyRelation").GetComponent<ProxyRelation>().Init(RspGPUM.proxyUsers);
        //InitProxy(RspGPUM.proxyUsers);
    }
    void OnNetEvRspGetSocialityInfoMessage(byte[] buf)
    {
        RspGetSocialityInfoMessage rspGsim = PBSerializer.NDeserialize<RspGetSocialityInfoMessage>(buf);
        if (rspGsim == null)
        {
            Hint.LoadTips("社交信息为空", Color.black);
            Debug.LogError("社交信息为空");
        }
        DataMgr.m_RspGetSocialityInfoMessage = rspGsim;
        if (DataMgr.m_RspGetSocialityInfoMessage.friendList == null)
        {
            DataMgr.m_RspGetSocialityInfoMessage.friendList = new List<ChatUser>();
        }

        if (DataMgr.m_RspGetSocialityInfoMessage.inChatGroup == null)
        {
            DataMgr.m_RspGetSocialityInfoMessage.inChatGroup = new List<ChatGroup>();
        }

        if (DataMgr.m_RspGetSocialityInfoMessage.systemNotifies == null)
        {
            DataMgr.m_RspGetSocialityInfoMessage.systemNotifies = new List<SystemNotify>();
        }

        if (DataMgr.m_RspGetSocialityInfoMessage.proxyUsers == null)
        {
            DataMgr.m_RspGetSocialityInfoMessage.proxyUsers = new List<ProxyUser>();
        }
        //DataMgr.m_FriendList =
        //    JsonConvert.DeserializeObject<List<ChatUser>>(DataMgr.m_RspGetSocialityInfoMessage.friendList);
        //DataMgr.m_ChatGroupList =
        //    JsonConvert.DeserializeObject<List<ChatGroup>>(DataMgr.m_RspGetSocialityInfoMessage.inChatGroup);
        m_isGetChatData = true;

    }

    void OnNetRspCommentMessage(byte[] buf)
    {
        RspCommentMessage RspCM = PBSerializer.NDeserialize<RspCommentMessage>(buf);
        if (RspCM.rspcmd == LoginDataPool.RSP_LOGINOUT)
        {
            Debug.Log("账号异地登录，强制下线");
            CloseAllSocket();
            //账号异地登录，强制下线
            GameSocket.Instance.StopHeart();

            ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, false);
            ispanel.SetContent("提示", "您的账号异地登录，请下线", false);
            ispanel.m_ok = () =>
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif


            };

        }
    }

    static void CloseAllSocket()
    {
        GameSocket.Instance.Close();
        ChatSocket.Instance.Close();
        HallSocket.Instance.Close();
        OtherSocket.Instance.Close();
        RoomSocket.Instance.Close();
    }

    void OnApplicationQuit()
    {
        CloseAllSocket();
    }

    void OnNetRspGetConnectionMessage(byte[] buf)
    {
        RspGetConnectionMessage rsp = PBSerializer.NDeserialize<RspGetConnectionMessage>(buf);
        if (rsp.code == 0)
        {
            Hint.LoadTips(rsp.tip, Color.white);
        }
        else
        {
            if (rsp.tip == "game")
            {
                DoGotoHometwon(EnMyOhter.Other, DataMgr.m_taInfo);
            }
            else if (rsp.tip == "scence")
            {
                DoGotoShanYeJie();
            }
        }
    }
    void OnNetEvGameData(byte[] buf)
    {
        RspGameDataMessage rsp = PBSerializer.NDeserialize<RspGameDataMessage>(buf);
        if (rsp.code == 0)
        {
            Hint.LoadTips(rsp.tip, Color.white);
        }
        else
        {
            DataMgr.homeProperties = rsp.homeProperties;
            DataMgr.partProperties = rsp.partProperties;
            DataMgr.devlopmentProperties = rsp.devlopmentProperties;
            DataMgr.businessModelProperties = rsp.businessModelProperties;

            DataMgr.m_listRoleProperties = rsp.roleProperties;
            for (int i = 0; i < rsp.roleProperties.Count; i++)
            {
                RoleProperties role = rsp.roleProperties[i];
                DataMgr.m_dicRoleProperties[(long)role.Id] = role;
            }
            if (rsp.shopsProperties != null)
            {
                for (int i = 0; i < rsp.shopsProperties.Count; i++)
                {
                    ShopsProperties shop = rsp.shopsProperties[i];
                    DataMgr.m_dicShopsProperties[(long)shop.id] = shop;
                }
            }
            woyaoqupanel.NavData();

            if (DataMgr.homeProperties != null)
            {
                for (int i = 0; i < DataMgr.homeProperties.Count; i++)
                {
                    DataMgr.m_dicHomeProperties[(long)DataMgr.homeProperties[i].id] = DataMgr.homeProperties[i];
                }
            }

            if (DataMgr.partProperties != null)
            {
                for (int i = 0; i < DataMgr.partProperties.Count; i++)
                {
                    DataMgr.m_dicPartProperties[(long)DataMgr.partProperties[i].id] = DataMgr.partProperties[i];
                }
            }

            if (DataMgr.devlopmentProperties != null)
            {
                for (int i = 0; i < DataMgr.devlopmentProperties.Count; i++)
                {
                    DataMgr.m_dicDevlopmentProperties[(long)DataMgr.devlopmentProperties[i].id] = DataMgr.devlopmentProperties[i];
                }
            }

            if (DataMgr.businessModelProperties != null)
            {
                for (int i = 0; i < DataMgr.businessModelProperties.Count; i++)
                {
                    //DataMgr.m_dicBusinessModelProperties[(long)DataMgr.businessModelProperties[i].Id] = DataMgr.businessModelProperties[i];
                }
            }


            if (rsp.userProperties != null)
            {
                for (int i = 0; i < rsp.userProperties.Count; i++)
                {
                    SysProperties sysPro = rsp.userProperties[i];
                    switch (sysPro.Name)
                    {
                        case "gm":
                            kefupanel.m_info = JsonUtility.FromJson<kefuInfoJson>(sysPro.Con);
                            break;
                        //case "publicInfo":
                        //    NoticePanel.m_info = JsonUtility.FromJson<NoticePanelJson>(sysPro.Con);
                        //    break;
                        case "openAreaCast":
                            DataMgr.m_dicOpenAreaCast = JsonConvert.DeserializeObject<Dictionary<string, OpenAreaCast>>(sysPro.Con);
                            break;

                        case "list_inpart_type":
                            DataMgr.m_dicHomeUnitInDoorPage = JsonConvert.DeserializeObject<Dictionary<string, string>>(sysPro.Con);

                            break;
                        case "list_outpart_type":
                            DataMgr.m_dicHomeUnitOutDoorPage = JsonConvert.DeserializeObject<Dictionary<string, string>>(sysPro.Con);
                            break;
                        //case "sevice_term":
                        //    NoticePanel.m_sevice = JsonConvert.DeserializeObject<SeviceTermJson>(sysPro.Con);
                        //    break;
                        case "uintTime":
                            DataMgr.m_deveGetGoldUnitTime = JsonConvert.DeserializeObject<SysVint>(sysPro.Con).v;
                            break;
                        case "world_chat_cost":
                            DataMgr.m_WorldChatConsumeGold = JsonConvert.DeserializeObject<SysVint>(sysPro.Con).v;
                            break;
                        case "forum_adress":
                            DataMgr.m_forum_url = JsonConvert.DeserializeObject<SysVString>(sysPro.Con).v;
                            break;
                        case "friendHPTimes":
                            DataMgr.m_friendHPTimes = JsonConvert.DeserializeObject<SysVint>(sysPro.Con).v;
                            break;
                        case "slefHPTimes":
                            DataMgr.m_slefHPTimes = JsonConvert.DeserializeObject<SysVint>(sysPro.Con).v;
                            break;
                        case "reBuildCast":
                            DataMgr.m_reBuildCast = JsonConvert.DeserializeObject<SysVFloat>(sysPro.Con).v;
                            break;
                        case "auto_confirm_days":
                            DataMgr.m_ShouHuoShiJian = JsonConvert.DeserializeObject<SysVint>(sysPro.Con).v;
                            break;
                        default:
                            break;
                    }
                }
            }


            m_isGetGameData = true;
        }
    }
    void Init()
    {

        AssetMgr.Instance.CreateObjOne(Vc.AbName.uiloadpanel, Vc.AbName.uiloadpanel, VirtualCityMgr.m_instance.m_bufferPar, Vector3.zero, Vector3.zero, new Vector3(-10000, 0, 0), (uiload) =>
        {
            UIManager.Instance.PushPanel(UIPanelName.loadingpanel, false, false,
            (param) =>
            {
                loadingpanel load = param.GetComponent<loadingpanel>();
                load.AddObjPreLoad(Vc.AbName.uiloadpanel);
                load.AddObjPreLoad(UIPanelName.loginpanel);

                load.AddObjPreLoad(UIPanelName.kefupanel);

                load.AddObjPreLoad(UIPanelName.noticepanel);
                load.AddObjPreLoad(Vc.AbName.newguidepanel);



                load.m_finishLoad = () =>
                {
                    UIManager.Instance.PushPanel(UIPanelName.loginpanel);
                };
                load.AllPreLoad();
            }
            );


        });



    }


    public static void OnLoginHallOk(RspLoginMessage pro)
    {


        //SyncTime.Sync(SyncTime.Server2Stamp(pro.time));

        ReqGameDataMessage reqGame = new ReqGameDataMessage();
        reqGame.clientVersion = DataMgr.clientVersion;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqGameDataMessage, reqGame);



        DataMgr.m_accountId = (long)pro.accountId;
        LoginPanel.m_id = (long)pro.accountId;

        //连接新的gamesocket
        //NetManager.Instance.OnRemove(EnSocket.Hall);

        string[] bufIp = pro.serverIp.Split(':');
        Debug.Log("GameServer ip:" + pro.serverIp);
        GameSocket.Instance.m_ipPort = pro.serverIp;
        GameSocket.Instance.Close();
        GameSocket.Instance.Connect(bufIp[0], int.Parse(bufIp[1]));

        //NetManager.Instance.m_onGameContentOk += SendMsgLoadPlayer;



        GameSocket.Instance.m_onConnectOk = () =>
        {
            ReqLoadPlayerMessage loadPlayer = new ReqLoadPlayerMessage();
            loadPlayer.playerId = (long)pro.accountId;
            GameSocket.Instance.SendMsgProto(MsgIdDefine.ReqLoadPlayerMessage, loadPlayer);
        };
    }

    public static void SaveLoginInfo()
    {
        LocalLoginInfo info = new LocalLoginInfo();
        info.uid = DataMgr.m_accountId;
        info.deviceId = SystemInfo.deviceUniqueIdentifier;

        string sInfo = JsonUtility.ToJson(info);
        JsonMgr.SaveJsonString(sInfo, AppConst.LocalPath + "/login.mi");
    }

    static void SendMsgLoadPlayer()
    {
        ReqLoadPlayerMessage loadPlayer = new ReqLoadPlayerMessage();
        loadPlayer.playerId = (long)LoginPanel.m_id;
        GameSocket.Instance.SendMsgProto(MsgIdDefine.ReqLoadPlayerMessage, loadPlayer);
        Debug.Log("LoadPlayer");



        //NetManager.Instance.m_onGameContentOk -= SendMsgLoadPlayer;
    }
    //public void CameraSetShyBox(bool isSkyBox)
    //{
    //    if (isSkyBox)
    //    {
    //        m_camera.clearFlags = CameraClearFlags.Skybox;
    //    }
    //    else {
    //        m_camera.clearFlags = CameraClearFlags.SolidColor;
    //    }
    //}

    void BreakLineReconnection()
    {

    }


    public static void OnEvNetLoadPlayer(RspLoadPlayerMessage ret, bool isNew = true)
    {
        {
            SyncTime.Sync(SyncTime.Server2Stamp(ret.time));
            DataMgr.m_account = ret.account;
            DataMgr.m_userOtherData = ret.userOtherData;
            if (ret.zanRecodeMap != "" && ret.zanRecodeMap != null && ret.zanRecodeMap != "{}")
            {
                DataMgr.m_dicZan = JsonConvert.DeserializeObject<Dictionary<long, List<string>>>(ret.zanRecodeMap);
            }
            if (DataMgr.m_userOtherData != null)
            {
                if (NewGuideMgr.SetGuideIdx(DataMgr.m_userOtherData.newStep))
                {
                    NewGuideMgr.Instance.Startup();
                }
            }
            else
            {
                NewGuideMgr.SetGuideIdx(-1);
            }
            //if (DataMgr.m_account.hadProxy == 1)
            //{
            //    DataMgr.m_isNewGuide = false;
            //}
            //DataMgr.m_proxyUser = ret.proxyUser;
            GameSocket.Instance.StartHeart();
            GameSocket.Instance.m_heart.m_act = () =>
            {
                GameSocket.Instance.m_isBreakLineReconnection = true;
                HallSocket.Instance.ReConnect();

                ChatSocket.Instance.ReConnect();

                GameSocket.Instance.ReConnect();
                GameSocket.Instance.m_onReconnectOk = () =>
                {

                    Hint.LoadTips("断线重连成功", Color.white);
                    ReqHeartBeatMessage reqHeartBeatMessage = new ReqHeartBeatMessage();
                    reqHeartBeatMessage.accountId = DataMgr.m_account.id;
                    GameSocket.Instance.SendMsgProto(MsgIdDefine.ReqHeartBeatMessage, reqHeartBeatMessage);
                    //GameSocket.Instance.RestartHeart();
                    if (DataMgr.m_isLoginOk == true)
                    {
                        //VirtualCityMgr.GotoHometown(EnMyOhter.My);
                    }
                };

            };
            if (isNew == false)
            {
                if (DataMgr.m_account.password == null)
                {
                    //if (UIManager.Instance.IsTopPanel(UIPanelName.uiloadpanel))
                    //{
                    //    UIManager.Instance.PopSelf(false);
                    //}
                    uiloadpanel.Instance.Close();
                    UIManager.Instance.PushPanel(Vc.AbName.passwordpanel, false, true, null, true);
                }
                else if (DataMgr.m_account.password == "*" && !File.Exists(AppConst.LocalPath + "/Rsa.txt"))
                {
                    //if (UIManager.Instance.IsTopPanel(UIPanelName.uiloadpanel))
                    //{
                    //    UIManager.Instance.PopSelf(false);
                    //}
                    uiloadpanel.Instance.Close();
                    UIManager.Instance.PushPanel(Vc.AbName.inputonepasswordpanel, false, true, null, true);
                }
            }
            if (ret.account.modleId == null)
            {
                //未注册模型用户

            }
            else
            {
                ReqChatLoginMessage chatLogin = new ReqChatLoginMessage();
                chatLogin.chatUser = new ChatUser();
                chatLogin.chatUser.accountId = (long)ret.account.id;
                chatLogin.chatUser.modelId = (long)ret.account.modleId;
                chatLogin.chatUser.userName = ret.account.userName;


                if (PublicFunc.IsJsonNull(DataMgr.m_account.friendList) == false)
                {

                    List<long> friendList = JsonConvert.DeserializeObject<List<long>>(DataMgr.m_account.friendList);
                    chatLogin.friendList = new List<long?>();
                    for (int i = 0; i < friendList.Count; i++)
                    {
                        chatLogin.friendList.Add(friendList[i]);
                    }

                    Debug.Log("好友列表");
                }

                if (PublicFunc.IsJsonNull(DataMgr.m_account.groupList) == false)
                {
                    List<long> groupList = JsonConvert.DeserializeObject<List<long>>(DataMgr.m_account.groupList);
                    chatLogin.groupList = new List<long?>();
                    for (int i = 0; i < groupList.Count; i++)
                    {
                        chatLogin.groupList.Add(groupList[i]);
                    }
                    Debug.Log("群");
                }
                if (ret.account.wallet.mIncome != null)
                {
                    chatLogin.chatUser.income = (double)ret.account.wallet.mIncome;
                }
                chatLogin.chatUser.serverIp = ret.account.serverIp;

                ChatSocket.Instance.Connect(AppConst.m_newChatIp, AppConst.m_newChatPort);
                DataMgr.m_ReqChatLoginMessage = chatLogin;
                ChatSocket.Instance.m_onConnectOk = () =>
                {
                    ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqChatLoginMessage, DataMgr.m_ReqChatLoginMessage);
                };

            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameSocket.Instance.Connect("127.0.0.1", 5004);
        }
    }
    public static void UpdateWallet(int gold, int diamond, double sMoney, double money, int asset)
    {
        DataMgr.m_account.wallet.goldNum += gold;
        DataMgr.m_account.wallet.diamondNum += diamond;
        DataMgr.m_account.wallet.sMoneyNum += sMoney;
        DataMgr.m_account.wallet.moneyNum += money;
        DataMgr.m_account.wallet.asset += asset;

        WalletUpdateDiff diff = new WalletUpdateDiff();
        diff.goldDiff = gold;
        diff.diamondDiff = diamond;
        diff.sMoneyDiff = sMoney;
        diff.moneyDiff = money;
        diff.assetDiff = asset;
        EventManager.Instance.DispatchEvent(Common.EventStr.WalletUpdate, new EventDataEx<WalletUpdateDiff>(diff));
    }

    public static void EnterGame()
    {
        if (DataMgr.m_account.modleId == null)
        {
            UIManager.Instance.PushPanelDeleteSelf(UIPanelName.createcharacterpanel);
        }
        else
        {
            UIManager.Instance.PopSelf();
            //DataMgr.m_curScene = EnCurScene.Hometown;
            //UIManager.Instance.PushPanelDeleteSelf(UIPanelName.hometwonbuildheadpanel, false,
            //    (buildhead) =>
            //    {
            //        UIManager.Instance.PushPanel(DataMgr.m_testUI);

            //        AssetMgr.Instance.CreateObj("buildhometown", "buildhometown", null, Vector3.zero, Vector3.zero, Vector3.one,
            //            (homeTown) => { homeTown.GetComponent<buildhometown>().SendReqGetHometoneInfoMessage((long)DataMgr.m_account.id, EnMyOhter.My); }

            //            );
            //    },
            //    UIManager.CanvasType.Top
            //    );
        }
    }


    public static void GotoHome(EnMyOhter enMyOhter, House info, ChatUser user = null)
    {
        uiloadpanel.Instance.Open(false);
        Debug.Log("导航去家");
        EventManager.Instance.DispatchEvent(Common.EventStr.DeleteBtHomeHeadUi);
        if (DataMgr.m_curSceneObj != null)
        {
            DataMgr.m_curSceneObj.transform.SetParent(VirtualCityMgr.m_instance.m_bufferPar.transform, false);
            //DataMgr.m_curSceneObj.SetActive(false);
        }
        DataMgr.m_myOther = enMyOhter;
        DataMgr.m_curScene = EnCurScene.Home;
        UIManager.Instance.PopAll();
        UIManager.Instance.PushPanel(UIPanelName.loadingpanel, false, false,
            (param) =>
            {
                loadingpanel load = param.GetComponent<loadingpanel>();
                load.AddObjPreLoad(AppConst.homemgr);
                load.AddToPool(DataMgr.m_dicHomeProperties[(long)info.modelId].modleData);
                load.AddToPool(PublicFunc.GetUserModelName(DataMgr.m_account));
                load.AddObjPreLoad(UIPanelName.joystickpanel);
                //loadingpanel.m_preMax += 1;
                load.m_finishLoadBefore = () =>
                {

                    loadingpanel.m_dicBufObj[AppConst.homemgr].transform.SetParent(null, false);
                    DataMgr.m_curSceneObj = loadingpanel.m_dicBufObj[AppConst.homemgr];
                    HomeMgr.m_instance.SetInfoAndInit(info);
                };

                load.m_finishLoad = () =>
                {
                    UIManager.Instance.PushPanel(UIPanelName.homepanel, false, false, (obj) =>
                    {
                        homepanel home = obj.GetComponent<homepanel>();
                        home.SetState(EnCurScene.Home, enMyOhter, DataMgr.m_taInfo);
                        UIManager.Instance.PushPanel(UIPanelName.chatpanel,false,false,null,true);
                    }, true);


                    UIManager.Instance.PushPanel(UIPanelName.joystickpanel, false, false, (paragm) =>
                    {
                        VariableJoystick joy = paragm.transform.GetChild(0).GetComponent<VariableJoystick>();
                        HomeMgr.m_instance.SetJoystick(joy);
                        uiloadpanel.Instance.Close();
                    },true);
                };
                load.AllPreLoad();
            }
            );
    }

    public static void GotoShop(long shopId)
    {
        uiloadpanel.Instance.Open(false);
        if (DataMgr.m_curSceneObj != null)
        {
            DataMgr.m_curSceneObj.transform.SetParent(VirtualCityMgr.m_instance.m_bufferPar.transform, false);
        }
        DataMgr.m_myOther = EnMyOhter.My;
        DataMgr.m_curScene = EnCurScene.Shop;

        UIManager.Instance.PopAll();
        UIManager.Instance.PushPanel(UIPanelName.loadingpanel, false, false,
            (param) =>
            {
                loadingpanel load = param.GetComponent<loadingpanel>();
                load.AddObjPreLoad(Vc.AbName.shopmgr);
                load.AddObjPreLoad(UIPanelName.joystickpanel);
                string shopModelName = DataMgr.m_dicShopsProperties[shopId].moduleId;
                load.AddToPool(shopModelName);

                load.m_finishLoad = () =>
                {

                    UIManager.Instance.PushPanel(UIPanelName.homepanel, false, false, (obj) =>
                    {
                        homepanel home = obj.GetComponent<homepanel>();
                        home.SetState(EnCurScene.Shop, EnMyOhter.My);
                        UIManager.Instance.PushPanel(UIPanelName.chatpanel);
                    }, true);

                    loadingpanel.m_dicBufObj[Vc.AbName.shopmgr].transform.SetParent(null, false);
                    DataMgr.m_curSceneObj = loadingpanel.m_dicBufObj[Vc.AbName.shopmgr];

                    UIManager.Instance.PushPanel(UIPanelName.joystickpanel, false, false, (paragm) =>
                    {
                        VariableJoystick joy = paragm.transform.GetChild(0).GetComponent<VariableJoystick>();
                        ShopMgr.m_instance.SetJoystick(joy, shopId);
                        uiloadpanel.Instance.Close();
                    });
                    NewGuideMgr.Instance.StartOneNewGuide();
                    //UIManager.Instance.PushPanel(UIPanelName.joystickpanel);
                };

                load.AllPreLoad();
            }
            );
    }


    void OnNetRspAllocationScenceMessage(byte[] buf)
    {
        RspAllocationScenceMessage rsp = PBSerializer.NDeserialize<RspAllocationScenceMessage>(buf);
        string[] ipPort = rsp.serverIp.Split(':');

        RoomSocket.Instance.m_ipPort = rsp.serverIp;
        Debug.Log("商业街ip:" + rsp.serverIp);
        RoomSocket.Instance.ForceClose();
        RoomSocket.Instance.Connect(ipPort[0], int.Parse(ipPort[1]));
        RoomSocket.Instance.m_onConnectOk = () =>
        {
            ReqGetConnectionMessage req = new ReqGetConnectionMessage();
            req.playerId = (long)DataMgr.m_account.id;
            req.runInfo = "scence";
            RoomSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetConnectionMessage, req);
        };

    }

    public static void DoGotoShanYeJie()
    {
        bool isNew = true;
        if (DataMgr.m_curScene == EnCurScene.Shop)
        {
            ShopMgr.m_instance.SendReqRunOutShopMessage(true);
            isNew = false;
        }

        EventManager.Instance.DispatchEvent(Common.EventStr.DeleteBtHomeHeadUi);

        DataMgr.m_myOther = EnMyOhter.My;
        DataMgr.m_curScene = EnCurScene.Business;

        if (DataMgr.m_curSceneObj != null)
        {
            DataMgr.m_curSceneObj.transform.SetParent(VirtualCityMgr.m_instance.m_bufferPar.transform, false);
            //DataMgr.m_curSceneObj.SetActive(false);
        }
        UIManager.Instance.PopAll();
        UIManager.Instance.PushPanel(UIPanelName.loadingpanel, false, false,
            (param) =>
            {
                loadingpanel load = param.GetComponent<loadingpanel>();
                load.AddObjPreLoad(AppConst.shangyejie);
                load.AddObjPreLoad(UIPanelName.joystickpanel);
                load.AddObjPreLoad(Vc.AbName.homepanel);
                load.AddObjPreLoad(Vc.AbName.chatpanel);
                load.AddObjPreLoad(Vc.AbName.joystickpanel);
                load.AddObjPreLoad(Vc.AbName.hudpanel);
                foreach (var item in DataMgr.m_dicRoleProperties)
                {
                    load.AddObjPreLoad(item.Value.ModelDate);

                }
                load.m_finishLoad = () =>
                {

                    UIManager.Instance.PushPanel(UIPanelName.homepanel, false, false, (obj) =>
                    {
                        homepanel home = obj.GetComponent<homepanel>();
                        home.SetState(EnCurScene.Business, EnMyOhter.My);
                        UIManager.Instance.PushPanel(UIPanelName.chatpanel);

                    }, true);

                    loadingpanel.m_dicBufObj[AppConst.shangyejie].transform.SetParent(null, false);
                    DataMgr.m_curSceneObj = loadingpanel.m_dicBufObj[AppConst.shangyejie];

                    UIManager.Instance.PushPanel(UIPanelName.joystickpanel, false, false, (paragm) =>
                    {
                        VariableJoystick joy = paragm.transform.GetChild(0).GetComponent<VariableJoystick>();
                        SYJMgr.m_instance.SetJoystick(joy, isNew);
                        uiloadpanel.Instance.Close();
                    });

                    NewGuideMgr.Instance.StartOneNewGuide();
                    //UIManager.Instance.PushPanel(UIPanelName.joystickpanel);
                };

                load.AllPreLoad();
            }
            );
    }

    /// <summary>
    /// 前往商业街
    /// </summary>
    /// <param name="isFromShop"></param>
    public static void GotoShanYeJie()
    {
        uiloadpanel.Instance.Open(false);
        if (DataMgr.m_curScene != EnCurScene.Shop)
        {
            ReqAllocationScenceMessage req = new ReqAllocationScenceMessage();
            req.accountId = (long)DataMgr.m_account.id;
            HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqAllocationScenceMessage, req);
        }
        else if (DataMgr.m_curScene == EnCurScene.Shop)
        {
            DoGotoShanYeJie();
        }
    }

    static void DoGotoHometwon(EnMyOhter enMyOhter, ChatUser user = null)
    {
        Debug.Log("导航去家园");
        if (DataMgr.m_curSceneObj != null)
        {
            DataMgr.m_curSceneObj.transform.SetParent(VirtualCityMgr.m_instance.m_bufferPar.transform, false);
        }
        UIManager.Instance.PopAll();

        UIManager.Instance.PushPanel(UIPanelName.loadingpanel, false, false,
            (param) =>
            {

                loadingpanel load = param.GetComponent<loadingpanel>();
                load.AddObjPreLoad(Vc.AbName.buildhometown);
                load.AddObjPreLoad(Vc.AbName.uiloadpanel);
                load.AddObjPreLoad(Vc.AbName.rankpanel);
                load.AddObjPreLoad(UIPanelName.homepanel);
                load.AddObjPreLoad(UIPanelName.hometwonbuildheadpanel);

                load.AddObjPreLoad(UIPanelName.homeselectbuildpanel);
                load.AddObjPreLoad(UIPanelName.htspeeduppanel);
                load.AddObjPreLoad(UIPanelName.woyaoqupanel);
                load.AddSprPreLoad("cangku", "commonbuild");
                load.AddSprPreLoad("zhuzhai_rixi01", "homeicon");
                load.AddSprPreLoad("GoldColor", "commonicon");
                load.AddObjPreLoad(Vc.AbName.chatpanel);
                load.AddObjPreLoad(Vc.AbName.hthistorypanel);
                load.AddObjPreLoad(Vc.AbName.reseffectpanel);
                //load.AddObjPreLoad(Vc.AbName.dingdanpanel);
                //load.AddObjPreLoad(Vc.AbName.accountsecuritypanel);
                //load.AddObjPreLoad(Vc.AbName.billflowpanel);
                //load.AddObjPreLoad(Vc.AbName.addresspanel);
                //load.AddObjPreLoad(Vc.AbName.shoppingcartpanel);
                //load.AddObjPreLoad(Vc.AbName.chatwindowspanel);
                //load.AddObjPreLoad(Vc.AbName.paypanel);
                load.AddObjPreLoad(Vc.AbName.personalinfo);
                load.AddObjPreLoad(Vc.AbName.editaccountpanel);
                load.m_finishLoad = () =>
                {
                    Debug.Log("加载完成");

                    UIManager.Instance.PushPanel(UIPanelName.hometwonbuildheadpanel, false, false,
                                (buildhead) =>
                                {
                                    //UIManager.Instance.PushPanel(Vc.AbName.reseffectpanel, false, false, null, true, UIManager.CanvasType.Top);
                                    UIManager.Instance.PushPanel(Vc.AbName.reseffectpanel, false, false, null, true);
                                    UIManager.Instance.PushPanel(Vc.AbName.homepanel, false, false, (obj) =>
                                    {
                                        homepanel home = obj.GetComponent<homepanel>();
                                        home.SetState(EnCurScene.Hometown, enMyOhter, user);
                                        UIManager.Instance.PushPanel(UIPanelName.chatpanel, false, false, null, true);
                                    }, true);



                                    loadingpanel.m_dicBufObj[Vc.AbName.buildhometown].transform.SetParent(null, false);

                                    long playerId = 0;
                                    if (user == null)
                                    {
                                        playerId = (long)DataMgr.m_account.id;
                                    }
                                    else
                                    {
                                        playerId = user.accountId;
                                    }
                                    loadingpanel.m_dicBufObj[Vc.AbName.buildhometown].GetComponent<buildhometown>().SendReqGetHometoneInfoMessage(playerId, enMyOhter);
                                    buildhometown.m_instance.m_btMode = BtMode.Display;
                                    DataMgr.m_curSceneObj = loadingpanel.m_dicBufObj[Vc.AbName.buildhometown];

                                    uiloadpanel.Instance.Close();
                                },
                                false//,
                                     //UIManager.CanvasType.Top
                                );
                };

                load.AllPreLoad();
            }
        );


    }

    public static void SwitchAccount()
    {
        DataMgr.m_curScene = EnCurScene.Hometown;
        DataMgr.m_myOther = EnMyOhter.My;
        m_isGetGameData = false;
        m_isGetChatData = false;
        DataMgr.m_isNewHaveLoginInfo = false;
        TimeManager.Instance.RemoveAllTask();
        GameSocket.Instance.StopHeart();
        PublicFunc.DeleteFile(AppConst.LocalPath + "/login.mi");
        PublicFunc.DeleteFile(AppConst.LocalPath + "/Rsa.txt");
        CloseAllSocket();

        UIManager.Instance.PopAll();
        loadingpanel.RemoveAll();

        UIManager.Instance.PushPanel(UIPanelName.loadingpanel, false, false,
            (param) =>
            {
                loadingpanel load = param.GetComponent<loadingpanel>();

                load.AddObjPreLoad(Vc.AbName.uiloadpanel);
                load.AddObjPreLoad(UIPanelName.loginpanel);

                load.AddObjPreLoad(UIPanelName.kefupanel);

                load.AddObjPreLoad(UIPanelName.noticepanel);
                load.AddObjPreLoad(Vc.AbName.newguidepanel);

                load.m_finishLoad = () =>
                {
                    UIManager.Instance.PushPanel(UIPanelName.loginpanel);
                };
                load.AllPreLoad();
            }
            );

    }
    public static void GotoHometown(EnMyOhter enMyOhter, ChatUser user = null)
    {
        uiloadpanel.Instance.Open(false);
        if (DataMgr.m_curScene == EnCurScene.Business)
        {
            SYJMgr.m_instance.SendReqRunOutStreetMessage();
            RoomSocket.Instance.Close();
        }

        if (DataMgr.m_curScene == EnCurScene.Shop)
        {
            ShopMgr.m_instance.SendReqRunOutShopMessage(false);
            RoomSocket.Instance.Close();
        }
        DataMgr.m_curScene = EnCurScene.Hometown;

        if (enMyOhter == EnMyOhter.Other && user != null)
        {
            DataMgr.m_taInfo = user;
        }
        //要根据ip 确定当前是哪个服务器的
        if (user == null)
        {
            DataMgr.m_curServer = EnServer.MyServer;
            DataMgr.m_myOther = EnMyOhter.My;
            OtherSocket.Instance.Close();
            DoGotoHometwon(enMyOhter);
        }
        else
        {
            //m_otherUser = user;
            if (user.serverIp == GameSocket.Instance.m_ipPort)
            {
                DataMgr.m_curServer = EnServer.MyServer;
                DataMgr.m_myOther = EnMyOhter.Other;
                OtherSocket.Instance.Close();
                DoGotoHometwon(enMyOhter, user);
            }
            else
            {
                DataMgr.m_curServer = EnServer.OtherServer;
                DataMgr.m_myOther = EnMyOhter.Other;

                if (user.serverIp == OtherSocket.Instance.m_ipPort)
                {
                    ReqGetConnectionMessage req = new ReqGetConnectionMessage();
                    req.playerId = (long)DataMgr.m_account.id;
                    req.runInfo = "game";
                    PublicFunc.ToGameServer(MsgIdDefine.ReqGetConnectionMessage, req);
                }
                else
                {
                    OtherSocket.Instance.m_ipPort = user.serverIp;
                    OtherSocket.Instance.Close();
                    string[] bufIp = user.serverIp.Split(':');
                    OtherSocket.Instance.Connect(bufIp[0], int.Parse(bufIp[1]));
                    OtherSocket.Instance.m_onConnectOk = () =>
                    {
                        ReqGetConnectionMessage req = new ReqGetConnectionMessage();
                        req.playerId = (long)DataMgr.m_account.id;
                        req.runInfo = "game";
                        OtherSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetConnectionMessage, req);
                    };
                }
            }
        }
    }

    #region 离线假数据
    public static void SetOfflineData()
    {
        DataMgr.m_account = new Account();
        DataMgr.m_account.wallet = new AccountWallet();
        DataMgr.m_account.id = 1;

        OpenAreaCast cast = new OpenAreaCast();
        DataMgr.m_dicOpenAreaCast["1"] = cast;

        
    }
#endregion
}


public class DiffTestLong : IEqualityComparer<TestLong>
{
    public bool Equals(TestLong x, TestLong y)
    {
        if (x.code == y.code)
        {
            return x.buildType == y.buildType;
        }
        else
        {
            return true;
        }
        //return x.id == y.id;
    }

    public int GetHashCode(TestLong obj)
    {
        if (obj == null)
        {
            return 0;
        }
        else
        {
            return obj.ToString().GetHashCode();
        }
    }
}
