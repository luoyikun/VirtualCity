using Framework.Event;
using Framework.UI;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HtBuildHead
{
    GoHome,
    BuildProgress,
    Refresh
}


public class BuildIdTrans
{
    public string buildId;
    public Transform transBuild;
}

public class hometwonbuildheadpanel : UGUIPanel {
    public GameObject m_btnGoHome;
    public GameObject m_buildProgress;
    public GameObject m_btnRefresh;
    public GameObject m_btnGetGlod;
    public GameObject m_tmpRewardProgress;
    public GameObject m_MoneyTreeBtn;
    public GameObject m_locationMarkerPar;
    public GameObject m_locationMarkerTmp;
    GameObject m_curHeadUi;
    private GameObject m_MoneyTreeUi;
    Transform m_trans;
    Dictionary<string, GameObject> m_dicGetCold = new Dictionary<string, GameObject>(); // 头顶上的金币
    public static hometwonbuildheadpanel m_instance;
    Dictionary<string, GameObject> m_dicLocationMarker = new Dictionary<string, GameObject>(); //头上建造标识
	// Use this for initialization
	void Start () {
        m_trans = this.transform;
        m_instance = this;
    }

    void DataInit()
    {
        foreach (var item in m_dicGetCold)
        {
            Destroy(item.Value);
        }
        m_dicGetCold.Clear();

        if (m_curHeadUi != null)
        {
            Destroy(m_curHeadUi);
        }
    }

    public override void OnOpen()
    {
        DataInit();
    }
    private void OnEnable()
    {
        EventManager.Instance.AddEventListener(Common.EventStr.CreateBtCommonHeadUi, OnEvCreateBtCommonHeadUi);
        EventManager.Instance.AddEventListener(Common.EventStr.CreateBtHomeHeadUi, OnEvCreateBtHomeUi);

        EventManager.Instance.AddEventListener(Common.EventStr.DeleteBtHomeHeadUi, OnEvDeleteBtHomeHeadUi);
        EventManager.Instance.AddEventListener(Common.EventStr.CreateDeveGetGold, OnEvCreateDeveGetGold);
        EventManager.Instance.AddEventListener(Common.EventStr.UpdateMoneyTree, UpdateMoneyTree);
        EventManager.Instance.AddEventListener(Common.EventStr.BuildHomeTown, OnEvOnEvBuildHomeTown);
        EventManager.Instance.AddEventListener(Common.EventStr.HtUpdateCurHeadUi, OnEvHtUpdateCurHeadUi);
        EventManager.Instance.AddEventListener(Common.EventStr.HtGetOneDeveAwardOk, OnEvHtGetOneDeveAwardOk);
        EventManager.Instance.AddEventListener(Common.EventStr.CloseBtRefesh, OnEvCloseBtRefesh);
        EventManager.Instance.AddEventListener(Common.EventStr.CloseCurHeadUi, CloseCurHeadUi);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveEventListener(Common.EventStr.CreateBtCommonHeadUi, OnEvCreateBtCommonHeadUi);
        EventManager.Instance.RemoveEventListener(Common.EventStr.CreateBtHomeHeadUi, OnEvCreateBtHomeUi);
        EventManager.Instance.RemoveEventListener(Common.EventStr.UpdateMoneyTree, UpdateMoneyTree);
        EventManager.Instance.RemoveEventListener(Common.EventStr.DeleteBtHomeHeadUi, OnEvDeleteBtHomeHeadUi);
        EventManager.Instance.RemoveEventListener(Common.EventStr.CreateDeveGetGold, OnEvCreateDeveGetGold);
        EventManager.Instance.RemoveEventListener(Common.EventStr.BuildHomeTown, OnEvOnEvBuildHomeTown);
        EventManager.Instance.RemoveEventListener(Common.EventStr.HtUpdateCurHeadUi, OnEvHtUpdateCurHeadUi);
        EventManager.Instance.RemoveEventListener(Common.EventStr.HtGetOneDeveAwardOk, OnEvHtGetOneDeveAwardOk);
        EventManager.Instance.RemoveEventListener(Common.EventStr.CloseBtRefesh, OnEvCloseBtRefesh);
        EventManager.Instance.RemoveEventListener(Common.EventStr.CloseCurHeadUi, CloseCurHeadUi);
    }

    void UpdateMoneyTree(EventData data)
    {
        if (m_MoneyTreeUi != null)
        {
            Destroy(m_MoneyTreeUi);
        }
        m_MoneyTreeUi = PublicFunc.CreateTmp(m_MoneyTreeBtn, m_trans, Vector3.zero, Vector3.zero, Vector3.one);
        var datavar = data as EventDataEx<int>;
        int state = datavar.GetData();
        //m_curHeadUi.GetComponent<RefeshDeveBuild>().SetInfo(deve);
        if (state == -1)
        {
            Destroy(m_MoneyTreeUi);
            return;
        }
        m_MoneyTreeUi.GetComponent<MoneyTreeCtrl>().UpdateButton(state);
        m_MoneyTreeUi.GetComponent<UiFollowObj>().m_followTrans = buildhometown.m_instance.MoneyTreeObj.transform;
        m_MoneyTreeUi.GetComponent<UiFollowObj>().m_camera = buildhometown.m_instance.m_cam;
    }

    void OnEvCloseBtRefesh(EventData data)
    {
        if (m_curHeadUi != null)
        {
            Destroy(m_curHeadUi);
        }
    }
    void OnEvHtGetOneDeveAwardOk(EventData data)
    {
        var exdata = data as EventDataEx<string>;
        string id = exdata.GetData();

        if (m_dicGetCold.ContainsKey(id))
        {
            Destroy(m_dicGetCold[id]);
            m_dicGetCold.Remove(id);
        }
       
    }
    void OnEvHtUpdateCurHeadUi(EventData data)
    {
        var exdata = data as EventDataEx<Devlopments>;
        Devlopments deve = exdata.GetData();

        if (m_curHeadUi != null)
        {

            if (m_curHeadUi.GetComponent<CommonBuildProgress>() != null)
            {
                m_curHeadUi.GetComponent<CommonBuildProgress>().SetInfo(deve);
            }
        }
    }
    void OnEvOnEvBuildHomeTown(EventData data)
    {
        var exdata = data as EventDataEx<bool>;
        bool isBuildMode = exdata.GetData();

        if (isBuildMode == true)
        {
            foreach (var item in m_dicGetCold)
            {
                item.Value.SetActive(false);
            }

            if (m_curHeadUi != null)
            {
                Destroy(m_curHeadUi);
            }
        }
        else {
            foreach (var item in m_dicGetCold)
            {
                item.Value.SetActive(true);
            }
        }
    }

    void OnEvCreateDeveGetGold(EventData data)
    {
        var exdata = data as EventDataEx<string>;
        string id = exdata.GetData();

        if (m_dicGetCold.ContainsKey(id) == false)
        {
            DeveTimer deve = buildhometown.m_dicBuildObj[id].GetComponent<DeveTimer>();
            Devlopments deveUserData = deve.m_deveInfo;
            if (DataMgr.m_myOther == EnMyOhter.Other && PublicFunc.IsMyFriend(DataMgr.m_taInfo.accountId))
            {
                if (deveUserData.stoneRecod != null)
                {
                    if (deveUserData.stoneRecod.ContainsKey(DataMgr.m_accountId.ToString()))
                    {
                        //已经是我的好友，并且偷取过这个建筑，不可再偷
                        return;
                    }
                }
            }
            GameObject obj = PublicFunc.CreateTmp(m_btnGetGlod, m_trans, Vector3.zero, Vector3.zero, Vector3.one);
            obj.GetComponent<HtDeveGetGold>().SetInfo(id);
            obj.GetComponent<UiFollowObj>().m_followTrans = buildhometown.m_dicBuildObj[id].transform;
            obj.GetComponent<UiFollowObj>().m_camera = buildhometown.m_instance.m_cam;
            m_dicGetCold[id] = obj;
        }
    }

    void OnEvDeleteBtHomeHeadUi(EventData data)
    {
        //if (m_curHeadUi != null)
        //{
        //    Destroy(m_curHeadUi);
        //}

        DataInit();
    }


    public override void OnClose()
    {

    }

    public void CloseCurHeadUi(EventData data)
    {
        if (m_curHeadUi != null)
        {
            Destroy(m_curHeadUi);
        }
    }
    void OnEvCreateBtCommonHeadUi(EventData data)
    {
        if (m_curHeadUi != null)
        {
            Destroy(m_curHeadUi);
        }

        var exdata = data as EventDataEx<string>;
        string buildId = exdata.GetData();

        Devlopments deve = buildhometown.m_instance.m_dicDevlopment[buildId];

        if (buildhometown.m_dicBuildObj[deve.id] == false)
        {
            //当前建筑没加载出来
            return;
        }
        switch (deve.status)
        {
            case (short)EnDeveState.Building:

                if (PublicFunc.IsHomeTownMyOrFriend())
                {
                    m_curHeadUi = PublicFunc.CreateTmp(m_buildProgress, m_trans, Vector3.zero, Vector3.zero, Vector3.one);
                    m_curHeadUi.GetComponent<CommonBuildProgress>().SetInfo(deve);
                    m_curHeadUi.GetComponent<UiFollowObj>().m_followTrans = buildhometown.m_dicBuildObj[deve.id].transform;
                    m_curHeadUi.GetComponent<UiFollowObj>().m_camera = buildhometown.m_instance.m_cam;
                }
                    break;
            case (short)EnDeveState.InComing:
                if (PublicFunc.IsHomeTownMyOrFriend())
                {
                    m_curHeadUi = PublicFunc.CreateTmp(m_tmpRewardProgress, m_trans, Vector3.zero, Vector3.zero, Vector3.one);
                    m_curHeadUi.GetComponent<GetNextReward>().SetInfo(deve);
                    m_curHeadUi.GetComponent<UiFollowObj>().m_followTrans = buildhometown.m_dicBuildObj[deve.id].transform;
                    m_curHeadUi.GetComponent<UiFollowObj>().m_camera = buildhometown.m_instance.m_cam;
                }
                break;
            case (short)EnDeveState.Noincome:
                if (DataMgr.m_myOther == EnMyOhter.My)
                {
                    m_curHeadUi = PublicFunc.CreateTmp(m_btnRefresh, m_trans, Vector3.zero, Vector3.zero, Vector3.one);
                    m_curHeadUi.GetComponent<RefeshDeveBuild>().SetInfo(deve);
                    m_curHeadUi.GetComponent<UiFollowObj>().m_followTrans = buildhometown.m_dicBuildObj[deve.id].transform;
                    m_curHeadUi.GetComponent<UiFollowObj>().m_camera = buildhometown.m_instance.m_cam;
                }
                break;
            default:
                break;
        }

    }

    void OnEvCreateBtHomeUi(EventData data)
    {
        if (m_curHeadUi != null)
        {
            Destroy(m_curHeadUi);
        }
        
        var exdata = data as EventDataEx<string>;
        string buildId = exdata.GetData();

        if (buildhometown.m_dicBuildObj.ContainsKey(buildId))
        {
            House info = buildhometown.m_instance.m_dicHome[buildId];
            m_curHeadUi = PublicFunc.CreateTmp(m_btnGoHome, m_trans, Vector3.zero, Vector3.zero, Vector3.one);
            m_curHeadUi.name = buildId;
            ClickListener.Get(m_curHeadUi).onClick = OnBtnGoHome;

            m_curHeadUi.GetComponent<UiFollowObj>().m_followTrans = buildhometown.m_dicBuildObj[buildId].transform;
            m_curHeadUi.GetComponent<UiFollowObj>().m_camera = buildhometown.m_instance.m_cam;
        }
    }

    void OnBtnGoHome(GameObject obj)
    {
        VirtualCityMgr.GotoHome(buildhometown.m_instance.m_myOther,buildhometown.m_instance.m_dicHome[obj.name]);
    }

    public void CreateLocationMarker(string id,GameObject obj)
    {
        GameObject mark = PublicFunc.CreateTmp(m_locationMarkerTmp,m_locationMarkerPar.transform,Vector3.zero,Vector3.zero,Vector3.one);
        mark.GetComponent<UiFollowObj>().m_camera = buildhometown.m_instance.m_cam;
        mark.GetComponent<UiFollowObj>().m_followTrans = obj.transform;
        m_dicLocationMarker[id] = mark;
        mark.name = id;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type">0: 开垦  1：建造</param>
    public void CreateLocationMarker(string id,EnLand type = EnLand.None)
    {
        m_locationMarkerPar.SetActive(true);
        GameObject mark = PublicFunc.CreateTmp(m_locationMarkerTmp, m_locationMarkerPar.transform, Vector3.zero, Vector3.zero, Vector3.one);
        mark.GetComponent<UiFollowObj>().m_camera = buildhometown.m_instance.m_cam;
        mark.GetComponent<UiFollowObj>().m_followTrans = buildhometown.m_instance.m_dicBox[id].transform;
        m_dicLocationMarker[id] = mark;
        mark.name = id;
        LocationMarker loca = mark.GetComponent<LocationMarker>();
        loca.SetInfo(id, type);
      
    }

    public void CloseLocationMarker(string id)
    {
        if (m_dicLocationMarker.ContainsKey(id))
        {
            Destroy(m_dicLocationMarker[id]);
            m_dicLocationMarker.Remove(id);
        }
    }

    public void OpenLocationMarker()
    {

    }
}
