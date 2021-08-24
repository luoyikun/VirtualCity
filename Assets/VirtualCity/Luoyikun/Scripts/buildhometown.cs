using System;
using Framework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Framework.UI;
using ProtoDefine;
using SGF.Codec;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum EnDeveState
{
    Building = 0,
    InComing = 1,
    Noincome = 2
}

public enum EnLand
{
    None = -1,
    Space = 0,
    Home = 611,
    CommonBuild = 613
}

public enum BtMode
{
    Build,
    Display
}

public enum EnMyOhter
{
    My,
    Other
}

public class MoneyTreeData
{
    public string adData;
    public int modelId;
    public float maxIncome;
    public string incomeTime;
    public float incomeEveryDay;
}
public class buildhometown : MonoBehaviour
{
    public BtMode m_btMode = BtMode.Display;
    public EnMyOhter m_myOther = EnMyOhter.My;

    public GameObject m_boxPar;
    public GameObject m_boxTmp;

    public static string m_selectBoxId = ""; //选择的地块,1-25
    public static long m_selectModelId = -1; //选择的建筑,是commonz或者home,是模型数据的id...是模型策划列表的索引
    public string m_rewardBuildId = ""; //当前得到奖励的建筑id

    public bool m_isHaveBox;

    public static buildhometown m_instance;

    public GameObject m_buildPar;
    public GameObject m_curBuild;

    public GameObject m_guangGaoPar;

    public Dictionary<string, BoxCtrl> m_dicBox = new Dictionary<string, BoxCtrl>();//每个地块的管理器
    Dictionary<string, GameObject> m_dicBanBuildObj = new Dictionary<string, GameObject>();//每块地的广告牌obj
    public static Dictionary<string, GameObject> m_dicBuildObj = new Dictionary<string, GameObject>(); //所有buildid对应的build obj,包含house 与deve

    public Dictionary<string, GameObject> m_dicQiZhongJi = new Dictionary<string, GameObject>(); //起重机

    public EnLand m_selectType;

    //根据数据更改地形
    //地块信息,只包含玩家有的
    //public  List<Land> m_listLand = new List<Land>();

    //地块上commbuild
    //public  List<Devlopments> m_listDevlopments = new List<Devlopments>();
    public Dictionary<string, Devlopments> m_dicDevlopment = new Dictionary<string, Devlopments>();

    //全部地块信息
    public Dictionary<string, Land> m_dicLand = new Dictionary<string, Land>();

    //地块上home信息
    public Dictionary<string, House> m_dicHome = new Dictionary<string, House>();
    long m_playerId;

    public Transform m_qizhongjiPar;

    private bool bInTouch = false;
    private float ClickAfter = 0.0f;
    GameObject m_curClickBox;
    private Vector3 mousePosLast = Vector3.zero;
    private bool Dragged = false;

    private bool bTemporarySelect = false;

    public Camera m_cam;

    public Vector3 m_camPos = new Vector3(27, 200, 775);
    public Vector3 m_camAngles = new Vector3(60, 180, 0);
    public bool m_isLoadOk = false;

    public GameObject MoneyTreeObj;
    private void Awake()
    {
        m_instance = this;
    }
    // Use this for initialization
    void Start()
    {
        //AssetMgr.Instance.CreateText()

        //CreateAllBuild();

        CreateBox();

    }

    public int GetHaveOpenAreaNum()
    {
        int i = 0;
        foreach (var item in m_dicLand)
        {
            if (item.Value.buildType != (int)EnLand.None)
            {
                i++;
            }
        }
        return i;
    }

    void DataInit()
    {

        //初始化纯数据
        foreach (var it in VcData.m_dicHomeTownBuildPos.dic)
        {
            Land land = new Land();
            land.code = it.Key;
            land.buildType = (int)EnLand.None;
            m_dicLand[it.Key] = land;
        }
        m_dicDevlopment.Clear();


        m_dicHome.Clear();
        foreach (var item in m_dicBanBuildObj)
        {
            //Destroy(item.Value);
            PoolMgr.Instance.RecycleObj(item.Value);
        }

        m_dicBanBuildObj.Clear();

        foreach (var item in m_dicBuildObj)
        {
            //Destroy(item.Value);
            PoolMgr.Instance.RecycleObj(item.Value);
        }
        m_dicBuildObj.Clear();

        foreach (var item in m_dicQiZhongJi)
        {
            //Destroy(item.Value);
            PoolMgr.Instance.RecycleObj(item.Value);
        }
        m_dicQiZhongJi.Clear();
    }

    private void OnEnable()
    {
        //DataInit();
        m_cam.transform.localPosition = m_camPos;
        m_cam.transform.localEulerAngles = m_camAngles;
        EventManager.Instance.AddEventListener(Common.EventStr.BuildHomeTown, OnEvBuildHomeBtnClick);
        //NetEventManager.Instance.AddEventListener(MsgIdDefine.RspBuildInfoChangeMessage, OnNetEvRspBuildInfoChangeMessage);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspBuildInfoMessage, OnNetRspBuildInfoMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspOpenAreaMessage, OnNetRspOpenAreaMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspBuildDevlopmentMessage, OnNetRspBuildDevlopmentMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspRemoveDevMessage, OnNetRspRemoveDevMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetOneDevInfoMessage, OnNetRspGetOneDevInfoMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspSpeedUpMessage, OnNetRspSpeedUpMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetRewardMessage, OnNetRspGetRewardMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspRemoveHouseMessage, OnNetRspRemoveHouseMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspBuildHouseMessage, OnNetRspBuildHouseMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspRebuildMessage, OnNetRspRebuildMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetMoneyTreeMessage, OnNetRGMTM);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspRewardMoneyTreeMessage, OnNetRMTM);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommandUIMessage,OnNetRCUIM);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveEventListener(Common.EventStr.BuildHomeTown, OnEvBuildHomeBtnClick);
        //NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspBuildInfoChangeMessage, OnNetEvRspBuildInfoChangeMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspOpenAreaMessage, OnNetRspOpenAreaMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspBuildInfoMessage, OnNetRspBuildInfoMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspBuildDevlopmentMessage, OnNetRspBuildDevlopmentMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspRemoveDevMessage, OnNetRspRemoveDevMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetOneDevInfoMessage, OnNetRspGetOneDevInfoMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspSpeedUpMessage, OnNetRspSpeedUpMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetRewardMessage, OnNetRspGetRewardMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspRemoveHouseMessage, OnNetRspRemoveHouseMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspBuildHouseMessage, OnNetRspBuildHouseMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspRebuildMessage, OnNetRspRebuildMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetMoneyTreeMessage, OnNetRGMTM);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspRewardMoneyTreeMessage, OnNetRMTM);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommandUIMessage,OnNetRCUIM);
    }

    void OnNetRCUIM(byte[] buf)
    {
        RspCommandUIMessage rsp = PBSerializer.NDeserialize<RspCommandUIMessage>(buf);
        if (rsp.ui == null)
        {
            return;
        }
        UIManager.Instance.PushPanel(rsp.ui);
    }

    void OnNetRMTM(byte[] buf)
    {
        RspRewardMoneyTreeMessage rsp = PBSerializer.NDeserialize<RspRewardMoneyTreeMessage>(buf);
        if (rsp.code == 0)
        {
            if (rsp.tips == null)
            {
                UIManager.Instance.PushPanel(UIPanelName.rewardmoneytreepanel, false, false, paragrm => { paragrm.GetComponent<rewardmoneytreepanel>().Init(0); });
                return;
            }
            Hint.LoadTips(rsp.tips, Color.white);
            EventManager.Instance.DispatchEvent(Common.EventStr.UpdateMoneyTree, new EventDataEx<int>(-1));
            return;
        }
        UIManager.Instance.PushPanel(UIPanelName.rewardmoneytreepanel, false, false, paragrm => { paragrm.GetComponent<rewardmoneytreepanel>().Init(1); });
    }
    void OnNetRGMTM(byte[] buf)
    {
        RspGetMoneyTreeMessage rsp = PBSerializer.NDeserialize<RspGetMoneyTreeMessage>(buf);
        DataMgr.m_MoneyTree = rsp.moneyTree;
        if (DataMgr.m_MoneyTree.isIncome == 0)
        {
            return;
        }
        if (DataMgr.m_account.hadProxy == 1 && DataMgr.m_MoneyTree.income == 0)
        {
            string CurrentTime = DateTime.Now.ToString("G");

            DateTime m_Time = Convert.ToDateTime(CurrentTime);

            DateTime Target_Time = Convert.ToDateTime(DataMgr.m_MoneyTree.incomeTime);

            //TimeSpan ts = Target_Time - m_Time;
            if (m_Time < Target_Time)
            {
                EventManager.Instance.DispatchEvent(Common.EventStr.UpdateMoneyTree, new EventDataEx<int>(1));
                return;
            }
            EventManager.Instance.DispatchEvent(Common.EventStr.UpdateMoneyTree, new EventDataEx<int>(3));
            return;
        }

        if (DataMgr.m_account.hadProxy == 1 && DataMgr.m_MoneyTree.income == 10)
        {
            EventManager.Instance.DispatchEvent(Common.EventStr.UpdateMoneyTree, new EventDataEx<int>(2));
            return;
        }
    }
    void OnNetRspRebuildMessage(byte[] buf)
    {
        RspRebuildMessage rsp = PBSerializer.NDeserialize<RspRebuildMessage>(buf);
        if (rsp.code == 0)
        {
            Hint.LoadTips(rsp.tip, Color.white);
        }
        else
        {
            //翻新成功，删除得到金币
            EventManager.Instance.DispatchEvent(Common.EventStr.HtGetOneDeveAwardOk, new EventDataEx<string>(rsp.devlopments.id));

            Hint.LoadTips("翻新建筑", Color.white);
            //VirtualCityMgr.UpdateWallet(rsp.gold, rsp.diamend, 0, 0);


            m_dicDevlopment[rsp.devlopments.id] = rsp.devlopments;
            //m_dicBuildObj[rsp.devlopments.id].GetComponent<DeveTimer>().SetInfo(rsp.devlopments);
            //EventManager.Instance.DispatchEvent(Common.EventStr.HtUpdateCurHeadUi, new EventDataEx<Devlopments>(rsp.devlopment));


            UpdateOneDeveModel(rsp.devlopments);

            EventManager.Instance.DispatchEvent(Common.EventStr.CloseBtRefesh);

        }
    }
    void OnNetRspBuildHouseMessage(byte[] buf)
    {
        RspBuildHouseMessage rsp = PBSerializer.NDeserialize<RspBuildHouseMessage>(buf);

        if (rsp.code == 0)
        {
            Hint.LoadTips(rsp.tip, Color.white);
        }
        else
        {
            m_dicBuildObj[rsp.house.id] = m_curBuild;
            m_dicHome[rsp.house.id] = rsp.house;
            Land land = new Land();
            land.code = rsp.house.landCode;
            land.buildType = (int)EnLand.Home;
            land.buildId = rsp.house.id;
            land.Model = rsp.house.modelId;
            m_dicLand[rsp.house.landCode] = land;


            //VirtualCityMgr.UpdateWallet(rsp.gold, rsp.diament, 0, 0);

            if (UIManager.Instance.IsTopPanel(UIPanelName.hometownborpanel))
            {
                UIManager.Instance.PopSelf();
            }

            BuildSuccess();
        }

    }
    //删除一个家
    void OnNetRspRemoveHouseMessage(byte[] buf)
    {
        RspRemoveHouseMessage rsp = PBSerializer.NDeserialize<RspRemoveHouseMessage>(buf);

        House info = rsp.house;

        if (m_dicHome.ContainsKey(info.id))
        {
            m_dicHome.Remove(info.id);
        }

        foreach (var item in m_dicLand)
        {
            if (item.Value.buildId == info.id)
            {
                item.Value.buildType = (int)EnLand.Space;
                break;
            }
        }

        if (m_dicBuildObj.ContainsKey(info.id))
        {
            //Destroy(m_dicBuildObj[info.id]);
            PoolMgr.Instance.RecycleObj(m_dicBuildObj[info.id]);
            m_dicBuildObj.Remove(info.id);
        }


        if (UIManager.Instance.IsTopPanel(UIPanelName.hometownborpanel))
        {
            UIManager.Instance.PopSelf();
        }

        BuildSuccess();

    }
    void OnNetRspGetRewardMessage(byte[] buf)
    {
        RspGetRewardMessage rsp = PBSerializer.NDeserialize<RspGetRewardMessage>(buf);
        if (rsp.code == 0)
        {
            Hint.LoadTips(rsp.tips, Color.white);
            EventManager.Instance.DispatchEvent(Common.EventStr.HtGetOneDeveAwardOk, new EventDataEx<string>(rsp.devlopment.id));
        }
        else
        {

            ExPlayGold play = new ExPlayGold();
            play.type = 0;
            play.source = PublicFunc.PosWorld2Overlay(m_cam, buildhometown.m_dicBuildObj[rsp.devlopment.id].transform.position);
            //play.target = homepanel.m_instance.m_textGold.transform.position;
            play.target = new Vector3(play.source.x, play.source.y + 200, play.source.z);

            play.count = rsp.gold;
            EventManager.Instance.DispatchEvent(Common.EventStr.PlayGetGoldEffect, new EventDataEx<ExPlayGold>(play));
            //VirtualCityMgr.UpdateWallet(rsp.gold, 0, 0, 0);

            m_dicDevlopment[rsp.devlopment.id] = rsp.devlopment;
            //m_dicBuildObj[rsp.devlopment.id].GetComponent<DeveTimer>().SetInfo(rsp.devlopment);
            //EventManager.Instance.DispatchEvent(Common.EventStr.HtUpdateCurHeadUi, new EventDataEx<Devlopments>(rsp.devlopment));


            UpdateOneDeveModel(rsp.devlopment);

            EventManager.Instance.DispatchEvent(Common.EventStr.HtGetOneDeveAwardOk, new EventDataEx<string>(rsp.devlopment.id));
        }

    }

    //当前存在的deve更新
    void UpdateOneDeveModel(Devlopments devlopment)
    {
        if (m_dicBuildObj[devlopment.id].GetComponent<DeveTimer>() == null)
        {
            m_dicBuildObj[devlopment.id].AddComponent<DeveTimer>();
        }
        DeveTimer deTime = m_dicBuildObj[devlopment.id].GetComponent<DeveTimer>();
        deTime.SetInfo(devlopment);

        switch (devlopment.status)
        {
            case (int)EnDeveState.Building:
                if (m_dicQiZhongJi.ContainsKey(devlopment.id) == false)
                {
                    CreateOneQiZhongJi(devlopment.landCode, devlopment.id);
                }
                break;
            case (int)EnDeveState.InComing:
                RemoveOneQiZhongJi(devlopment.id);

                break;
            case (int)EnDeveState.Noincome:
                break;
            default:
                break;
        }
    }
    void OnNetRspSpeedUpMessage(byte[] buf)
    {
        RspSpeedUpMessage rsp = PBSerializer.NDeserialize<RspSpeedUpMessage>(buf);

        if (rsp.code == 0)
        {
            Hint.LoadTips(rsp.tip, Color.white);
        }
        else
        {
            DevlopmentProperties pro = DataMgr.m_dicDevlopmentProperties[(long)rsp.devlopments.modelId];
            if (DataMgr.m_myOther == EnMyOhter.My)
            {
                if (rsp.diamend == 0)
                {
                    Hint.LoadTips("有好友帮我加速,快去查看吧", Color.white);
                }
                else
                {
                    Hint.LoadTips("加速成功,消耗" + rsp.diamend + "钻石", Color.white);
                }
            }
            else
            {
                string sAdd = "";
                if (rsp.gold != 0)
                {
                    sAdd += rsp.gold + "金币";
                }
                if (rsp.diamend != 0)
                {
                    sAdd += rsp.diamend + "钻石";
                }
                Hint.LoadTips("帮好友加速成功,获得" + sAdd + "奖励", Color.white);
            }
            m_dicDevlopment[rsp.devlopments.id] = rsp.devlopments;
            m_dicBuildObj[rsp.devlopments.id].GetComponent<DeveTimer>().SetInfo(rsp.devlopments);

            //VirtualCityMgr.UpdateWallet(rsp.gold,rsp.diamend,0,0);
            EventManager.Instance.DispatchEvent(Common.EventStr.HtUpdateCurHeadUi, new EventDataEx<Devlopments>(rsp.devlopments));

            UpdateOneDeveModel(rsp.devlopments);

        }
    }
    void OnNetRspGetOneDevInfoMessage(byte[] buf)
    {
        RspGetOneDevInfoMessage rsp = PBSerializer.NDeserialize<RspGetOneDevInfoMessage>(buf);

        m_dicDevlopment[rsp.devlopment.id] = rsp.devlopment;


        UpdateOneDeveModel(rsp.devlopment);
    }

    void RemoveOneQiZhongJi(string buildId)
    {
        if (m_dicQiZhongJi.ContainsKey(buildId))
        {
            //Destroy(m_dicQiZhongJi[buildId]);
            PoolMgr.Instance.RecycleObj(m_dicQiZhongJi[buildId]);
            m_dicQiZhongJi.Remove(buildId);
        }
    }
    public void OnNetRspRemoveDevMessage(byte[] buf)
    {
        RspRemoveDevMessage rsp = PBSerializer.NDeserialize<RspRemoveDevMessage>(buf);
        Devlopments deveInfo = rsp.getDevlopments();

        //删除成功，并且把得到金币删除
        EventManager.Instance.DispatchEvent(Common.EventStr.HtGetOneDeveAwardOk, new EventDataEx<string>(deveInfo.id));

        if (m_dicDevlopment.ContainsKey(deveInfo.id))
        {
            m_dicDevlopment.Remove(deveInfo.id);
        }

        foreach (var item in m_dicLand)
        {
            if (item.Value.buildId == deveInfo.id)
            {
                item.Value.buildType = (int)EnLand.Space;
                break;
            }
        }

        if (m_dicBuildObj.ContainsKey(deveInfo.id))
        {
            //Destroy(m_dicBuildObj[deveInfo.id]);
            PoolMgr.Instance.RecycleObj(m_dicBuildObj[deveInfo.id]);

            m_dicBuildObj.Remove(deveInfo.id);
        }

        if (m_dicQiZhongJi.ContainsKey(deveInfo.id))
        {
            //Destroy(m_dicQiZhongJi[deveInfo.id]);
            PoolMgr.Instance.RecycleObj(m_dicQiZhongJi[deveInfo.id]);
            m_dicQiZhongJi.Remove(deveInfo.id);
        }

        if (UIManager.Instance.IsTopPanel(UIPanelName.hometownborpanel))
        {
            UIManager.Instance.PopSelf();
        }

        BuildSuccess();
    }
    /// <summary>
    /// 新建一处房屋
    /// </summary>
    /// <param name="buf"></param>
    public void OnNetRspBuildDevlopmentMessage(byte[] buf)
    {
        RspBuildDevlopmentMessage rsp = PBSerializer.NDeserialize<RspBuildDevlopmentMessage>(buf);
        if (rsp.code == 0)
        {
            Hint.LoadTips(rsp.tip, Color.white);
        }
        else
        {
            m_dicDevlopment[rsp.devlopment.id] = rsp.devlopment;
            Land land = new Land();
            land.code = rsp.devlopment.landCode;
            land.buildType = (int)EnLand.CommonBuild;
            land.buildId = rsp.devlopment.id;
            land.Model = rsp.devlopment.modelId;
            m_dicLand[rsp.devlopment.landCode] = land;


            //VirtualCityMgr.UpdateWallet(rsp.gold, rsp.diament, 0, 0);

            DeveTimer deTime = m_curBuild.AddComponent<DeveTimer>();
            deTime.SetInfo(rsp.devlopment);
            m_dicBuildObj[rsp.devlopment.id] = m_curBuild;

            //建立起重机
            CreateOneQiZhongJi(rsp.devlopment.landCode, rsp.devlopment.id);

            if (UIManager.Instance.IsTopPanel(UIPanelName.hometownborpanel))
            {
                UIManager.Instance.PopSelf();
            }

            BuildSuccess();
        }
    }

    public void OnNetRspOpenAreaMessage(byte[] buf)
    {
        RspOpenAreaMessage rsp = PBSerializer.NDeserialize<RspOpenAreaMessage>(buf);
        if (rsp.code == 0)
        {
            Hint.LoadTips(rsp.tip, Color.white);
        }
        else
        {
            m_selectBoxId = rsp.land.code;
            m_dicLand[rsp.land.code] = rsp.land;

            // 钱包变化
            //VirtualCityMgr.UpdateWallet(rsp.gold, rsp.diamend, 0, 0);
            //删除一处广告牌
            if (m_dicBanBuildObj.ContainsKey(rsp.land.code))
            {
                //Destroy(m_dicBanBuildObj[m_selectBoxId]);
                PoolMgr.Instance.RecycleObj(m_dicBanBuildObj[rsp.land.code]);
                m_dicBanBuildObj.Remove(rsp.land.code);
            }
        }
    }


    /// <summary>
    /// 请求hometown所有信息
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="myOther"></param>
    public void SendReqGetHometoneInfoMessage(long accountId, EnMyOhter myOther)
    {
        DataInit();
        m_myOther = myOther;
        m_playerId = accountId;
        //m_isFirstGetLand = true;
        ReqGetHometoneInfoMessage req = new ReqGetHometoneInfoMessage(accountId);
        PublicFunc.ToGameServer(MsgIdDefine.ReqGetHometoneInfoMessage, req);
    }

    public void SendGetDataByPlayerId()
    {
        DataInit();
        ReqGetHometoneInfoMessage req = new ReqGetHometoneInfoMessage(m_playerId);
        PublicFunc.ToGameServer(MsgIdDefine.ReqGetHometoneInfoMessage, req);
    }

    /// <summary>
    /// 时间改变，建筑物状态变化
    /// </summary>
    public void SendGetOneDeveInfo(string buildId)
    {
        ReqGetOneDevInfoMessage req = new ReqGetOneDevInfoMessage();
        req.devId = buildId;
        req.friendId = m_playerId;
        PublicFunc.ToGameServer(MsgIdDefine.ReqGetOneDevInfoMessage, req);
    }



    void OnNetRspBuildInfoMessage(byte[] buf)
    {
        RspBuildInfoMessage rsp = PBSerializer.NDeserialize<RspBuildInfoMessage>(buf);

        if (rsp.code == 0)
        {
            Hint.LoadTips(rsp.tips, Color.white);
        }
        else
        {
            if (rsp.landMap != null)
            {
                Dictionary<string, Land> landDictionary = rsp.landMap;

                foreach (var item in landDictionary)
                {
                    m_dicLand[item.Key] = item.Value;
                }
            }

            if (rsp.devlopmentsMap != null)
            {
                m_dicDevlopment = rsp.devlopmentsMap;
            }

            if (rsp.housesMap != null)
            {
                m_dicHome = rsp.housesMap;
            }
            
           DataMgr.m_zan= rsp.zan;

            long num = rsp.getZan();
            homepanel.m_instance.m_btnZan.transform.Find("Text").GetComponent<Text>().text = num.ToString();
            Debug.Log("赞更新了" + rsp.getZan().ToString());

            CreateAllBuild();
            UIManager.Instance.PushPanel(Vc.AbName.hthistorypanel, false, false, null, true);
            if (DataMgr.m_isOpenWelcome == false)
            {
                DataMgr.m_isOpenWelcome = true;
                UIManager.Instance.PushPanel(Vc.AbName.welcomepanel, false, false, null, true);
            }
            else {
                NewGuideMgr.Instance.StartOneNewGuide();
            }
        }
        if (DataMgr.m_myOther == EnMyOhter.My)
        {
            if (DataMgr.m_account.hadProxy == 0)
            {
                EventManager.Instance.DispatchEvent(Common.EventStr.UpdateMoneyTree, new EventDataEx<int>(0));
            }
            else if (DataMgr.m_account.hadProxy == 1)
            {
                //JieXi mJieXi=new JieXi();
                MoneyTreeData m_MoneyTreeData=new MoneyTreeData();
                for (int i = 0; i < DataMgr.businessModelProperties.Count; i++)
                {
                    if (DataMgr.businessModelProperties[i].Name == "money_tree")
                    {
                        m_MoneyTreeData = JsonConvert.DeserializeObject<MoneyTreeData>(DataMgr.businessModelProperties[i].Con);
                        break;
                    }
                }
                if (DataMgr.m_account.wallet.mIncome < m_MoneyTreeData.maxIncome)
                {
                    ReqGetMoneyTreeMessage reqGMTM = new ReqGetMoneyTreeMessage();
                    reqGMTM.accountId = DataMgr.m_accountId;
                    GameSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetMoneyTreeMessage, reqGMTM);
                }
            }
        }
    }


    void OnEvBuildHomeBtnClick(EventData data)
    {
        var exdata = data as EventDataEx<bool>;
        bool isBuild = exdata.GetData();
        if (isBuild == true)
        {
            //m_boxPar.SetActive(true);
            AllBoxVisible(true);

        }
        else
        {
            //m_boxPar.SetActive(false);
            AllBoxVisible(false);
            SetBoxCollider(true);
        }
    }

    void CreateAllBuild()
    {
        //foreach (var it in DataMgr.m_dicLand)
        //{
        //    StartCoroutine(YieldCreateBuild(it.Value));
        //}

        foreach (var it in m_dicLand)
        {
            StartCoroutine(YieldCreateBuild(it.Value));
        }
    }

    /// <summary>
    /// 建造模式与非建造模式设置
    /// </summary>
    /// <param name="isVisibal"></param>
    void AllBoxVisible(bool isVisibal)
    {
        foreach (var it in m_dicBox)
        {
            it.Value.OnSetVisible(isVisibal);
        }
    }
    IEnumerator YieldCreateBuild(Land land)
    {
        switch (land.getBuildType())
        {
            case (int)EnLand.None:
                //建一块牌子

                CreateGuangGaoPai(land.getCode());


                break;
            case (int)EnLand.Space:
                break;
            case (int)EnLand.Home:
                CreateHomeByData((long)land.Model, land.code, land.buildId);
                break;
            case (int)EnLand.CommonBuild:
                CreateCommonBuildByData((long)land.Model, land.getCode(), land.buildId);
                break;
            default:
                break;
        }
        yield return null;
    }
    void CreateBox()
    {
        foreach (var it in VcData.m_dicHomeTownBuildPos.dic)
        {
            StartCoroutine(YieldCreateBox(it));
        }
    }

    IEnumerator YieldCreateBox(KeyValuePair<string, HomeBuildPos> item)
    {
        GameObject obj = PublicFunc.CreateTmp(m_boxTmp);
        obj.SetActive(true);
        BoxCtrl box = obj.GetComponent<BoxCtrl>();
        box.m_id = item.Key;
        //box.m_land = DataMgr.m_dicLand[box.m_id];
        obj.transform.SetParent(m_boxPar.transform, false);
        obj.transform.localPosition = new Vector3((float)item.Value.x, (float)item.Value.y, (float)item.Value.z);
        obj.transform.localEulerAngles = new Vector3((float)item.Value.dirX, (float)item.Value.dirY, (float)item.Value.dirZ);
        //m_dicBox.Add(box);
        m_dicBox[box.m_id] = box;
        yield return null;
    }


    public void CreateGuangGaoPai(string landIdx)
    {
        Vector3 pos = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].x,
            (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].y,
            (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].z
            );
        Vector3 angle = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].dirX,
    (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].dirY,
    (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].dirZ
    );

        AssetMgr.Instance.CreateObj("guanggaopai", "guanggaopai", m_guangGaoPar.transform, pos, angle, Vector3.one,
            (obj) =>
            {
                m_dicBanBuildObj[landIdx] = obj;
            }
            );
    }


    public void CreateHomeByData(long cbId, string landIdx, string buildId)
    {
        HomeProperties info = DataMgr.m_dicHomeProperties[cbId];

        Vector3 pos = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].x,
            (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].y,
            (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].z
            );
        //Vector3 angles = new Vector3(0, 180, 0);
        Vector3 angles = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].dirX,
    (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].dirY,
    (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].dirZ
    );
        Vector3 scale = Vector3.one;

        //建造模型在当前地块
        AssetMgr.Instance.CreateObj(info.modleDataGoodJianMo, info.modleDataGoodJianMo, m_buildPar.transform, pos, angles, scale,
            (obj) =>
            {
                m_dicBuildObj[buildId] = obj;

            }
            );
    }


    public void CreateCommonBuildByData(long cbId, string landIdx, string buildId)
    {
        string sMd = DataMgr.m_dicDevlopmentProperties[cbId].modleData;
        ModelData md = JsonConvert.DeserializeObject<ModelData>(sMd);

        Vector3 pos = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].x,
            (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].y,
            (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].z
            );
        Vector3 angles = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].dirX,
         (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].dirY,
         (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].dirZ
         );
        //if (md.name == "gejuyuan")
        //{
        //    angles = Vector3.zero;
        //}
        //else
        //{
        //    angles = new Vector3(-90, 0, 0);
        //}
        Vector3 scale = new Vector3(-10000, 0, 0);
        //建造模型在当前地块
        AssetMgr.Instance.CreateObj(md.name, md.name, m_buildPar.transform, pos, angles, scale,
            (obj) =>
            {
                m_dicBuildObj[buildId] = obj;
                DeveTimer deTimer = obj.AddComponent<DeveTimer>();
                deTimer.SetInfo(m_dicDevlopment[buildId]);
            }
            );

        if (m_dicDevlopment[buildId].status == (short)EnDeveState.Building)
        {
            AssetMgr.Instance.CreateObj("qizhongji", "qizhongji", m_qizhongjiPar, pos, angles, Vector3.one,
                (obj) =>
                {
                    m_dicQiZhongJi[buildId] = obj;
                }
                );
        }
    }


    void CreateOneQiZhongJi(string landId, string buildId)
    {

        Vector3 pos = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[landId.ToString()].x,
            (float)VcData.m_dicHomeTownBuildPos.dic[landId.ToString()].y,
            (float)VcData.m_dicHomeTownBuildPos.dic[landId.ToString()].z
            );

        Vector3 angles = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[landId.ToString()].dirX,
    (float)VcData.m_dicHomeTownBuildPos.dic[landId.ToString()].dirY,
    (float)VcData.m_dicHomeTownBuildPos.dic[landId.ToString()].dirZ
    );

        AssetMgr.Instance.CreateObj("qizhongji", "qizhongji", m_qizhongjiPar, pos, angles, Vector3.one,
             (obj) =>
             {
                 m_dicQiZhongJi[buildId] = obj;
             }
         );
    }

    public void CreateHomeByClick(long id)
    {
        m_selectModelId = id;
        string sMd = DataMgr.m_dicHomeProperties[id].modleDataGoodJianMo;
        Vector3 pos = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[m_selectBoxId.ToString()].x,
            (float)VcData.m_dicHomeTownBuildPos.dic[m_selectBoxId.ToString()].y,
            (float)VcData.m_dicHomeTownBuildPos.dic[m_selectBoxId.ToString()].z
            );
        //Vector3 angles = new Vector3(0,180,0);
        Vector3 angles = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[m_selectBoxId.ToString()].dirX,
    (float)VcData.m_dicHomeTownBuildPos.dic[m_selectBoxId.ToString()].dirY,
    (float)VcData.m_dicHomeTownBuildPos.dic[m_selectBoxId.ToString()].dirZ
    );
        Vector3 scale = Vector3.one;
        //建造模型在当前地块
        AssetMgr.Instance.CreateObj(sMd, sMd, m_buildPar.transform, pos, angles, scale,
            (obj) => { m_curBuild = obj; }
            );



    }
    // 点击了第几项
    public void CreateCommonBuildByClick(long cbId)
    {
        m_selectModelId = cbId;
        string sMd = DataMgr.m_dicDevlopmentProperties[cbId].modleData;
        ModelData md = JsonConvert.DeserializeObject<ModelData>(sMd);

        Vector3 pos = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[m_selectBoxId.ToString()].x,
            (float)VcData.m_dicHomeTownBuildPos.dic[m_selectBoxId.ToString()].y,
            (float)VcData.m_dicHomeTownBuildPos.dic[m_selectBoxId.ToString()].z
            );
        //Vector3 angles = Vector3.zero;
        Vector3 angles = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[m_selectBoxId.ToString()].dirX,
            (float)VcData.m_dicHomeTownBuildPos.dic[m_selectBoxId.ToString()].dirY,
            (float)VcData.m_dicHomeTownBuildPos.dic[m_selectBoxId.ToString()].dirZ
            );

        //if (md.name == "gejuyuan")
        //{
        //    angles = Vector3.zero;
        //}
        //else
        //{
        //    angles = new Vector3(-90, 0, 0);
        //}
        Vector3 scale = new Vector3(-10000, 0, 0);
        //建造模型在当前地块
        AssetMgr.Instance.CreateObj(md.name, md.name, m_buildPar.transform, pos, angles, scale,
            (obj) => { m_curBuild = obj; }
            );


    }

    public void SetBoxSelect(string idx)
    {
        foreach (var item in m_dicBox)
        {
            if (item.Key == idx)
            {
                item.Value.OnSelectChange(true);
            }
            else
            {
                item.Value.OnSelectChange(false);
            }
        }
    }

    public void SetBoxCollider(bool isEnable)
    {
        foreach (var item in m_dicBox)
        {
            item.Value.OnSetCollider(isEnable);
        }
    }

    //通过选择面板出现建筑的类型
    /// <summary>
    /// 每建造一个东西，确定用什么够买 
    /// </summary>
    public void SendBuild()
    {
        switch (m_selectType)
        {
            case EnLand.None:
                break;
            case EnLand.Space:
                break;
            case EnLand.Home:
                {

                    HomeProperties homeInfo = DataMgr.m_dicHomeProperties[m_selectModelId];
                    UIManager.Instance.PushPanel(Vc.AbName.paypanel, false, true, (param) =>
                    {
                        paypanel pay = param.GetComponent<paypanel>();
                        pay.SetContent("提示", "确定建造" + homeInfo.name, homeInfo.gold, homeInfo.diamond);
                        pay.m_GoldPay = () =>
                        {
                            ReqBuildHouseMessage info = new ReqBuildHouseMessage();
                            info.LandCode = m_selectBoxId;
                            info.houseModelId = m_selectModelId;
                            info.costDiamond = 0;
                            PublicFunc.ToGameServer(MsgIdDefine.ReqBuildHouseMessage, info);
                        };
                        pay.m_DiamondPay = () =>
                        {
                            ReqBuildHouseMessage info = new ReqBuildHouseMessage();
                            info.LandCode = m_selectBoxId;
                            info.houseModelId = m_selectModelId;
                            info.costDiamond = 1;
                            PublicFunc.ToGameServer(MsgIdDefine.ReqBuildHouseMessage, info);
                        };
                    }, true);



                }
                break;
            case EnLand.CommonBuild:
                {
                    DevlopmentProperties deveInfo = DataMgr.m_dicDevlopmentProperties[m_selectModelId];
                    UIManager.Instance.PushPanel(Vc.AbName.paypanel, false, true, (param) =>
                    {
                        paypanel pay = param.GetComponent<paypanel>();
                        pay.SetContent("提示", "确定建造" + deveInfo.cnName, (int)deveInfo.gold, (int)deveInfo.diamond);
                        pay.m_GoldPay = () =>
                        {
                            ReqBuildDevlopmentMessage commonBuild = new ReqBuildDevlopmentMessage();
                            commonBuild.setLandCode(m_selectBoxId);
                            commonBuild.setDevModelId(m_selectModelId);
                            commonBuild.costDiamond = 0;
                            PublicFunc.ToGameServer(MsgIdDefine.ReqBuildDevlopmentMessage, commonBuild);
                        };
                        pay.m_DiamondPay = () =>
                        {
                            ReqBuildDevlopmentMessage commonBuild = new ReqBuildDevlopmentMessage();
                            commonBuild.setLandCode(m_selectBoxId);
                            commonBuild.setDevModelId(m_selectModelId);
                            commonBuild.costDiamond = 1;
                            PublicFunc.ToGameServer(MsgIdDefine.ReqBuildDevlopmentMessage, commonBuild);
                        };
                    }, true);

                }
                break;
            default:
                break;
        }

    }

    public void BuildSuccess()
    {
        buildhometown.m_instance.SetBoxSelect("");
        buildhometown.m_instance.SetBoxCollider(true);
        buildhometown.m_instance.m_curBuild = null;
    }

    //移除当前地块的建筑
    public void SendRemove()
    {
        switch (m_dicLand[m_selectBoxId].buildType)
        {
            case (int)EnLand.Home:
                {
                    ReqRemoveHouseMessage remove = new ReqRemoveHouseMessage();
                    remove.houseId = buildhometown.m_instance.m_dicLand[m_selectBoxId].buildId;
                    PublicFunc.ToGameServer(MsgIdDefine.ReqRemoveHouseMessage, remove);
                }
                break;
            case (int)EnLand.CommonBuild:
                {
                    ReqRemoveDevMessage remove = new ReqRemoveDevMessage();
                    remove.devId = buildhometown.m_instance.m_dicLand[m_selectBoxId].buildId;
                    PublicFunc.ToGameServer(MsgIdDefine.ReqRemoveDevMessage, remove);
                }
                break;
            default:
                break;
        }

    }

    //public string m_getRewardId = "";
    public void SendReqGetRewardMessage(string devId)
    {
        ReqGetRewardMessage get = new ReqGetRewardMessage();
        get.devId = devId;
        get.playerId = m_playerId;
        get.playerName = DataMgr.m_account.userName;
        //m_getRewardId = devId;
        PublicFunc.ToGameServer(MsgIdDefine.ReqGetRewardMessage, get);
    }

    public void SendReqSpeedUpMessage(string deveId)
    {
        ReqSpeedUpMessage speedUp = new ReqSpeedUpMessage();
        speedUp.devId = deveId;
        speedUp.playerId = m_playerId;
        speedUp.playerName = DataMgr.m_account.userName;
        PublicFunc.ToGameServer(MsgIdDefine.ReqSpeedUpMessage, speedUp);
    }

    void PickOneBox()
    {
        Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "HtBox")
            {
                hit.collider.gameObject.GetComponent<BoxCtrl>().OnPick();

            }
        }

    }
    public void Update()
    {
        //if (Input.GetMouseButton(0))
        //{

        //    if (EventSystem.current.IsPointerOverGameObject())
        //    {
        //        //Debug.Log("left-click over a GUI element!");
        //        return;
        //    }

        //    if (!bInTouch)
        //    {
        //        bInTouch = true;

        //    }
        //}
        //else
        //{
        //    if (bInTouch)
        //    {
        //        bInTouch = false;

        //        EventManager.Instance.DispatchEvent(Common.EventStr.DeleteBtHomeHeadUi);
        //    }
        //}

        if (Input.GetMouseButton(0))
        {

            if (Application.isMobilePlatform && Input.touchCount > 0)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    return;
            }
            else if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (bInTouch == false)
            {

                bInTouch = true;//当前点击了一个建筑
                ClickAfter = 0.0f;
                mousePosLast = Input.mousePosition;
                bTemporarySelect = false;
                Dragged = false;

                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit hit;
                //if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.tag == "HtBox"))
                //{
                //    m_curClickBox = hit.collider.gameObject;
                //}
                //else
                //{
                //    m_curClickBox = null;
                //}
            }
            else
            {
                if (Vector3.Distance(Input.mousePosition, mousePosLast) > 0.01f)
                {
                    if (!Dragged)
                    {
                        Dragged = true;
                    }
                }
                else
                {
                    if (!Dragged)
                    {
                        ClickAfter += Time.deltaTime;
                        if (!bTemporarySelect && (ClickAfter > 0.5f))
                        {
                            bTemporarySelect = true;
                            //Debug.Log ("Update2 buildingSelected:"+((buildingSelected != null) ? buildingSelected.name : "none"));
                            //按下到松手且不移动，判定为点击状态
                            PickOneBox();
                        }
                    }
                }
            }
        }
        else
        {
            if (bInTouch)
            {
                bInTouch = false;

                if (Dragged)
                {

                }
                else
                {
                    if (bTemporarySelect)
                    {
                    }
                    else
                    {
                        PickOneBox();
                    }
                }
            }
        }
    }
}

//地不同只会因为type不同
public class DiffLand : IEqualityComparer<Land>
{
    public bool Equals(Land x, Land y)
    {
        return x.code == y.code;
    }

    public int GetHashCode(Land obj)
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
