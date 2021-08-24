using BE;
using BitBenderGames;
using Framework.Event;
using Framework.UI;
using Newtonsoft.Json;
using ProtoDefine;
using SGF.Codec;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public enum EnFloor
{
    floor_dz = 0,
    floor_1l = 1,
    floor_2l = 2,
    floor_3l = 3,
    floor_wd = 4,
}

public enum EnHomeUnit
{
    HomeUnit = 0,
    HomeUnitIsTable = 1,
    HomeUnitOnTable = 2,
    HomeUnitOutDoor = 3
}
public class HomeMgr : MonoBehaviour {
    public Camera m_cameraBuild;
    
    private bool bInTouch = false;
    private float ClickAfter = 0.0f;
    private Vector3 mousePosLast = Vector3.zero;
    private bool Dragged = false;

    private bool bTemporarySelect = false;

    public Transform m_playerPar;
    public Transform m_unitPar;
    public Transform m_selectUnit;
    public Transform m_clickUnit;
    public Transform m_newUnit;
    public bool m_isCreateNew = false;

    public EnBuildMode m_buildMode = EnBuildMode.Build;
    public static HomeMgr m_instance;


    public const string HomeUnit = "HomeUnit";
    public const string Floor = "Floor";
    public const string Wall = "Wall";
    public const string HomeUnitOnTable = "HomeUnitOnTable";
    public const string HomeUnitIsTable = "HomeUnitIsTable";
    public const string HomeUnitOutDoor = "HomeUnitOutDoor";

    public const string FloorOutDoor = "FloorOutDoor";
    public const int m_layerInDoor = 9;
    public const int m_layerOutDoor = 11;
    public const int m_layerWall = 12;
    public MouseOrbitImproved m_cameraPlayer;

    public GameObject m_homeAllPar;
    public Transform m_terrainPar;
    public Dictionary<long, GameObject> m_dicPlayer = new Dictionary<long, GameObject>();//创建的player


    public Dictionary<string, Devlopments> m_dicDevlopment = new Dictionary<string, Devlopments>();

    //地块上home信息
    public Dictionary<string, House> m_dicHome = new Dictionary<string, House>();

    public Transform m_lookInDoor;
    public Transform m_lookOutDoor;
    public int m_maxLayer;

    Vector3 m_outDoorInitPos;
    Dictionary<int, GameObject> m_dicLayerObj = new Dictionary<int, GameObject>(); // 要设置隐藏显示的层

    GameObject m_curHome;
    public Transform m_homePar;
    public Dictionary<int, Dictionary<int, GameObject>> m_dicUnitInLayer = new Dictionary<int, Dictionary<int, GameObject>>(); //每一层上物体 

    //public Dictionary<string, HomeUnit> m_dicUnit = new Dictionary<string, HomeUnit>();

    public Transform m_otherBuildPar;

    public bool m_isCanPlace = true;

    int m_putIdx = -1;

    public bool m_isClickUpdate = false; //是否是点击更新，还是点下一个好的更新

    public House m_userHomeInfo;//玩家数据相关属性
    public HomeProperties m_tableHomeInfo;//策划数据

    public bool m_isFirtEnter = false; //是否刚进入

    public LocalNavMeshBuilder m_navBuilder;

    Vector3 m_posLast;
    Vector3 m_anglesLast;
    private void Awake()
    {
        m_instance = this;
    }
    // Use this for initialization
    void Start () {
        

        DataInitOnce();

#if UNITY_EDITOR
        //VcData.Instance.LoadData(Init);

#else
        //VcData.Instance.LoadData(Init);
        //AssetMgr.Instance.Init(Init);
#endif


        //xzPlane = new Plane(new Vector3(0f, 1f, 0f), 0f);

    }

    public void RemoveOneDicUnitInLayer(int layer, int instanceId)
    {
        if (m_dicUnitInLayer.ContainsKey(layer))
        {
            if (m_dicUnitInLayer[layer].ContainsKey(instanceId))
            {
                //if (m_dicUnitInLayer[layer][instanceId] != null)
                //{
                //    Destroy(m_dicUnitInLayer[layer][instanceId]);
                //}
                m_dicUnitInLayer[layer].Remove(instanceId);
            }
        }
    }

    void DataInitOnce()
    {
        m_dicUnitInLayer[0] = new Dictionary<int, GameObject>();
        m_dicUnitInLayer[1] = new Dictionary<int, GameObject>();
        m_dicUnitInLayer[2] = new Dictionary<int, GameObject>();
        m_dicUnitInLayer[3] = new Dictionary<int, GameObject>();
    }
    private void OnEnable()
    {
        
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspPutHousePartMessage, OnNetRspPutHousePartMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspRemoveHousePartMessage, OnNetRspRemoveHousePartMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspUpdateHousePartMessage, OnNetRspUpdateHousePartMessage);
    }

    private void OnDisable()
    {
        //DataInit();
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspPutHousePartMessage, OnNetRspPutHousePartMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspRemoveHousePartMessage, OnNetRspRemoveHousePartMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspUpdateHousePartMessage, OnNetRspUpdateHousePartMessage);
    }

    void OnNetRspPutHousePartMessage(byte[] buf)
    {
        RspPutHousePartMessage rsp = PBSerializer.NDeserialize<RspPutHousePartMessage>(buf);
        if (rsp.code == 0)
        {
            Hint.LoadTips(rsp.tip, Color.white);
        }
        else {
            m_newUnit.GetComponent<HomeUnit>().m_putStatus.id = rsp.putStatusId;

            OperationSucceed();
        }
    }

    void OnNetRspRemoveHousePartMessage(byte[] buf)
    {
        RspRemoveHousePartMessage rsp = PBSerializer.NDeserialize<RspRemoveHousePartMessage>(buf);
        if (rsp.code == 0)
        {
            Hint.LoadTips(rsp.tip, Color.white);
        }
        else {

            OnRspRecycle();
        }
    }

    void OnNetRspUpdateHousePartMessage(byte[] buf)
    {
        RspUpdateHousePartMessage rsp = PBSerializer.NDeserialize<RspUpdateHousePartMessage>(buf);
        if (rsp.code == 0)
        {
            Hint.LoadTips(rsp.tip, Color.white);
        }
        else
        {
            OperationSucceed(); 
        }
    }
 

    private void Init()
    {
        //VirtualCityMgr.GotoHome(EnMyOhter.My);
    }

    void DataInit()
    {
        m_buildMode = EnBuildMode.Display;
        foreach (var item in m_dicPlayer)
        {
            if (item.Value != null)
            {
                PoolMgr.Instance.RecycleObj(item.Value);
                //Destroy(item.Value);
            }
        }
        m_dicPlayer.Clear();

        //回收每个子部件
        for (int i = m_unitPar.childCount - 1; i >= 0; i--)
        {
            HomeUnit homeUnit = m_unitPar.GetChild(i).gameObject.GetComponent<HomeUnit>();
            homeUnit.DestroyChildWhenRecyle();
            PoolMgr.Instance.RecycleObj(m_unitPar.GetChild(i).gameObject);
        }

        for (int i = m_homePar.childCount - 1; i >= 0; i--)
        {
            //Destroy(m_homePar.GetChild(i).gameObject);
            foreach (var item in m_dicLayerObj)
            {
                item.Value.SetActive(true);
            }
            PoolMgr.Instance.RecycleObj(m_homePar.GetChild(i).gameObject);
        }

        for (int i = m_otherBuildPar.childCount - 1; i >= 0; i--)
        {
            PoolMgr.Instance.RecycleObj(m_otherBuildPar.GetChild(i).gameObject);
        }

        foreach (var item in m_dicUnitInLayer)
        {
            item.Value.Clear();
        }

        m_isCanPlace = true;
        m_isCreateNew = false;
        m_showLayer = 1;
        
    }

    void CreateAllOtherBuild()
    {
        //foreach (var it in DataMgr.m_dicLand)
        //{
        //    StartCoroutine(YieldCreateBuild(it.Value));
        //}

        foreach (var it in buildhometown.m_instance.m_dicLand)
        {
            if (it.Value.buildId == m_userHomeInfo.id)
            {
                continue;
            }
            StartCoroutine(YieldCreateBuild(it.Value));
        }
    }

    IEnumerator YieldCreateBuild(Land land)
    {
        switch (land.getBuildType())
        {
            case (int)EnLand.None:
                //建一块牌子

                //CreateGuangGaoPai(land.getCode());


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

        string modelName = info.modleData + "_lodhome";
        //建造模型在当前地块
        //AssetMgr.Instance.CreateObj(info.modleDataGoodJianMo, info.modleDataGoodJianMo, m_otherBuildPar.transform, pos, angles, scale,
        //    (param) => { PublicFunc.AddNavTag(param); }
        //    );

        AssetMgr.Instance.CreateObj(modelName, modelName, m_otherBuildPar.transform, pos, angles, scale,
           (param) => { PublicFunc.AddNavTag(param); }
           );
    }


    public void CreateAllHomeUnit()
    {
        if (m_userHomeInfo.putStatusList != null)
        {
            foreach (var item in m_userHomeInfo.putStatusList)
            {
                PutStatus info = item.Value;
                PartProperties partInfo = DataMgr.m_dicPartProperties[(long)info.modelId];

                Vector3 pos = new Vector3(info.x, info.y, info.z);
                //Vector3 angle = new Vector3(info.dirX, info.dirY, info.dirY);
                Quaternion que = new Quaternion(info.dirX, info.dirY, info.dirZ,info.dirW);

                AssetMgr.Instance.CreateObj(partInfo.modleData, partInfo.modleData, m_unitPar, new Vector3(1000, 10000, 1000), Vector3.zero, new Vector3(-10000,0,0),
                    (param) =>
                    {
                        param.transform.position = pos;
                        //param.transform.eulerAngles = angle;
                        param.transform.rotation = que;
                        HomeUnit homeUnit = param.AddComponent<HomeUnit>();
                        homeUnit.m_putStatus = info;

                        param.tag = ((EnHomeUnit)(partInfo.selftype)).ToString();

                        homeUnit.InitByDataWhenCreate();

                    }

                    );
            }
        }
    }
    public void CreateCommonBuildByData(long cbId, string landIdx, string buildId)
    {
        string sMd = DataMgr.m_dicDevlopmentProperties[cbId].modleData;
        ModelData md = JsonConvert.DeserializeObject<ModelData>(sMd);

        Vector3 pos = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].x,
            (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].y,
            (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].z
            );
        //Vector3 angles = Vector3.zero;

        Vector3 angles = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].dirX,
        (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].dirY,
        (float)VcData.m_dicHomeTownBuildPos.dic[landIdx.ToString()].dirZ
        );

        Vector3 scale = new Vector3(-10000, 0, 0);
        //建造模型在当前地块
        AssetMgr.Instance.CreateObj(md.name + "lod", md.name + "lod", m_otherBuildPar.transform, pos, angles, scale,
           (param) => {
               PublicFunc.AddNavTag(param);
                 }
            );

        if (buildhometown.m_instance.m_dicDevlopment[buildId].status == (short)EnDeveState.Building)
        {
            AssetMgr.Instance.CreateObj("qizhongjilod", "qizhongjilod", m_otherBuildPar.transform, pos, angles, Vector3.one
                );
        }
    }


    void ConstDataInit()
    {
        string[] lookIn = m_tableHomeInfo.lookIn.Split(',');
        Vector3 homePosIn = new Vector3(float.Parse(lookIn[0]), float.Parse(lookIn[1]), float.Parse(lookIn[2]));
        Vector3 homePosInWorld = m_curHome.transform.TransformPoint(homePosIn);
        Vector3 homePosInHomeMgrLocal = m_homeAllPar.transform.InverseTransformPoint(homePosInWorld);
        m_lookInDoor.transform.localPosition = homePosInHomeMgrLocal;

        string[] lookOut = m_tableHomeInfo.lookOut.Split(',');
        Vector3 homePosOut = new Vector3(float.Parse(lookOut[0]), float.Parse(lookOut[1]), float.Parse(lookOut[2]));
        Vector3 homePosOutWorld = m_curHome.transform.TransformPoint(homePosOut);
        Vector3 homePosOutHomeMgrLocal = m_homeAllPar.transform.InverseTransformPoint(homePosOutWorld);
        m_lookOutDoor.transform.localPosition = homePosOutHomeMgrLocal;

        Ray ray = new Ray(m_lookOutDoor.position, -m_lookOutDoor.up);
        RaycastHit hit;
        int layerMask = 1 << m_layerOutDoor;
        if (Physics.Raycast(ray, out hit, 1000.0f, layerMask))
        {
            m_outDoorInitPos = hit.point;
        }

        


    }

    IEnumerator StartNavMesh()
    {
        m_navBuilder.StartNavMesh();
        yield return null;
    }
    public void SetInfoAndInit(House info)
    {
        m_isFirtEnter = true;
        m_userHomeInfo = info;
        m_tableHomeInfo = DataMgr.m_dicHomeProperties[(long)m_userHomeInfo.modelId];
        m_maxLayer = m_tableHomeInfo.floor;
        DataInit();

        GameObject house = PoolMgr.Instance.GetOne(m_tableHomeInfo.modleData);
        house.transform.parent = m_homePar;
        house.transform.localPosition = Vector3.zero;
        

        Vector3 angles = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[m_userHomeInfo.landCode].dirX,
        (float)VcData.m_dicHomeTownBuildPos.dic[m_userHomeInfo.landCode].dirY,
        (float)VcData.m_dicHomeTownBuildPos.dic[m_userHomeInfo.landCode].dirZ
        );

        house.transform.localEulerAngles = angles;
        house.transform.localScale = Vector3.one;

        m_curHome = house;

        GetLayerObjData(house);
        ConstDataInit();
        CreateMySelf();

        CreateAllHomeUnit();

        //AssetMgr.Instance.CreateObj(m_tableHomeInfo.modleData, m_tableHomeInfo.modleData, m_homePar, Vector3.zero, new Vector3(0, 180, 0), Vector3.one,
        //    (obj) =>
        //    {
        //        GetLayerObjData(obj);
        //        ConstDataInit();

        //        CreateMySelf();

        //        CreateAllHomeUnit();
        //    }

        //    );



        CreateAllOtherBuild();
        
        

        Vector3 oldPos = new Vector3((float)VcData.m_dicHomeTownBuildPos.dic[m_userHomeInfo.landCode].x, (float)VcData.m_dicHomeTownBuildPos.dic[m_userHomeInfo.landCode].y, (float)VcData.m_dicHomeTownBuildPos.dic[m_userHomeInfo.landCode].z);
        Vector3 posTerrain = new Vector3(-oldPos.x, -oldPos.y, -oldPos.z);
        m_terrainPar.localPosition = posTerrain;
        m_cameraBuild.gameObject.SetActive(false);
        m_cameraPlayer.gameObject.SetActive(true);

        foreach (var item in m_dicLayerObj)
        {
            item.Value.SetActive(true);
        }

    }


    void GetLayerObjData(GameObject obj)
    {
        m_dicLayerObj.Clear();
        Transform floor1L = PublicFunc.GetTransform(obj.transform, "1l");
        if (floor1L != null)
        {
            m_dicLayerObj[1] = floor1L.gameObject;
        }

        Transform floor2L = PublicFunc.GetTransform(obj.transform, "2l");
        if (floor2L != null)
        {
            m_dicLayerObj[2] = floor2L.gameObject;
        }

        Transform floor3L = PublicFunc.GetTransform(obj.transform, "3l");
        if (floor3L != null)
        {
            m_dicLayerObj[3] = floor3L.gameObject;
        }

        Transform floorWd = PublicFunc.GetTransform(obj.transform, "wd");
        if (floorWd != null)
        {
            m_dicLayerObj[4] = floorWd.gameObject;
        }
    }
    void CreateMySelf()
    {
        string modelName = PublicFunc.GetUserModelName(DataMgr.m_account);

        GameObject player = PoolMgr.Instance.GetOne(modelName);
        player.transform.parent = m_playerPar;
        player.transform.position = m_outDoorInitPos;
        player.transform.localEulerAngles = new Vector3(0, 180, 0);
        player.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);


        m_cameraPlayer.target = player.transform.Find("LookAt");
        MoveController moveCtrl = player.AddComponent<MoveController>();
        moveCtrl.m_camera = m_cameraPlayer.transform;
        //moveCtrl.m_joy = joyTmp;
        player.GetComponent<NavMeshAgent>().enabled = true;
        m_dicPlayer[(long)DataMgr.m_account.id] = player;
        player.tag = "Player";
        m_navBuilder.m_Tracked = player.transform;
        //StartCoroutine(StartNavMesh());


        //AssetMgr.Instance.CreateObj(modelName, modelName, m_playerPar, m_outDoorInitPos, new Vector3(0,180,0), new Vector3(0.8f,0.8f,0.8f), (parma) =>
        //{
        //    m_cameraPlayer.target = parma.transform.Find("LookAt");
        //    MoveController moveCtrl = parma.AddComponent<MoveController>();
        //    moveCtrl.m_camera = m_cameraPlayer.transform;
        //    //moveCtrl.m_joy = joyTmp;
        //    parma.GetComponent<NavMeshAgent>().enabled = true;
        //    m_dicPlayer[(long)DataMgr.m_account.id] = parma;
        //    parma.tag = "Player";
        //    m_navBuilder.m_Tracked = parma.transform;
        //    m_navBuilder.StartNavMesh();

        //});
    }


    public void SetJoystick(VariableJoystick joyTmp)
    {
        m_dicPlayer[(long)DataMgr.m_account.id].GetComponent<MoveController>().m_joy = joyTmp;
        StartCoroutine(StartNavMesh());
    }
    void SendCreateNew()
    {
        
        HomeUnit unit = m_newUnit.GetComponent<HomeUnit>();
        DoSendCreateNew();
        if (m_newUnit.tag == HomeUnitIsTable)
        {
            Dictionary<string, PutStatus> dic = new Dictionary<string, PutStatus>();
            List<int> listDelete = new List<int>();
            foreach (var item in unit.m_dicChild)
            {
                if (item.Value != null)
                {
                    dic[item.Value.m_putStatus.id] = item.Value.m_putStatus;
                }
                else
                {
                    listDelete.Add(item.Key);
                }
            }
            for (int i = 0; i < listDelete.Count; i++)
            {
                unit.m_dicChild.Remove(listDelete[i]);
            }
            if (dic.Count != 0)
            {
                DoSendUpdate(dic);
            }
        }

        
    }

    void DoSendCreateNew()
    {
        Debug.Log("创建新的给服务器");
        ReqPutHousePartMessage req = new ReqPutHousePartMessage();
        req.houseId = m_userHomeInfo.id;
        req.putStatus = m_newUnit.GetComponent<HomeUnit>().m_putStatus;
        PublicFunc.ToGameServer(MsgIdDefine.ReqPutHousePartMessage, req);
        //OnRspSendCreateNew();
    }

    void OnRspSendCreateNew()
    {
        Debug.Log("创建新的服务器回复");
        m_putIdx++;
        m_newUnit.GetComponent<HomeUnit>().m_putStatus.id = m_putIdx.ToString();

        OperationSucceed();
    }

    void SendUpdate()
    {
        HomeUnit unit = m_selectUnit.GetComponent<HomeUnit>();
        Dictionary<string, PutStatus> dic = new Dictionary<string, PutStatus>();
        dic[unit.m_putStatus.id] = unit.m_putStatus;

        if (m_selectUnit.tag == HomeUnitIsTable)
        {

            List<int> listDelete = new List<int>();
            foreach (var item in unit.m_dicChild)
            {
                if (item.Value != null)
                {
                    dic[item.Value.m_putStatus.id] = item.Value.m_putStatus;
                }
                else
                {
                    listDelete.Add(item.Key);
                }
            }

            for (int i = 0; i < listDelete.Count; i++)
            {
                unit.m_dicChild.Remove(listDelete[i]);
            }
        }

        DoSendUpdate(dic);

        
    }


    void DoSendUpdate(Dictionary<string,PutStatus> dic)
    {
        Debug.Log("更新给服务器:" + dic.Count);

        ReqUpdateHousePartMessage req = new ReqUpdateHousePartMessage();
        req.houseId = m_userHomeInfo.id;
        req.putStatusMap = dic;
        PublicFunc.ToGameServer(MsgIdDefine.ReqUpdateHousePartMessage, req);
        //OnRspSendUpdate();
    }


    void OnRspSendUpdate()
    {
        Debug.Log("更新回复");
        OperationSucceed();
    }

    public void SendRecycle()
    {
        if (m_selectUnit == null)
        {
            return;
        }

        if (m_isCreateNew == false)
        {
            List<string> list = new List<string>();
            List<int> listObjRec = new List<int>();
            HomeUnit unit = m_selectUnit.GetComponent<HomeUnit>();
            list.Add(unit.m_putStatus.id);
            listObjRec.Add(unit.gameObject.GetInstanceID());
            if (m_selectUnit.tag == HomeUnitIsTable)
            {

                foreach (var item in unit.m_dicChild)
                {
                    list.Add(item.Value.m_putStatus.id);
                    listObjRec.Add(item.Value.gameObject.GetInstanceID());
                }

            }

            DoSendRecycle(list);
            EventManager.Instance.DispatchEvent(Common.EventStr.HomeUnitRecycle, new EventDataEx<List<int>>(listObjRec));
        }
        else {
            //PoolMgr.Instance.RecycleObj(m_selectUnit.gameObject);
            //m_isClickUpdate = true;
            //OperationSucceed();
            OnRspRecycle();
        }
        
    }

    void DoSendRecycle(List<string> list)
    {
        Debug.Log("回收一个给服务器:"+ list.Count);
        ReqRemoveHousePartMessage req = new ReqRemoveHousePartMessage();
        req.houseId = m_userHomeInfo.id;
        req.putStatusId = list;
        PublicFunc.ToGameServer(MsgIdDefine.ReqRemoveHousePartMessage, req);
    }

    void OnRspRecycle()
    {
        Debug.Log("回收回复");
        m_isClickUpdate = true;
        if (m_selectUnit != null)
        {
            HomeUnit unit = m_selectUnit.GetComponent<HomeUnit>();
            if (m_dicUnitInLayer[(int)unit.m_putStatus.hasPut].ContainsKey(m_selectUnit.GetInstanceID()))
            {
                m_dicUnitInLayer[(int)unit.m_putStatus.hasPut].Remove(m_selectUnit.GetInstanceID());
            }

            if (m_selectUnit.tag == HomeUnitIsTable)
            {
                foreach (var item in unit.m_dicChild)
                {
                    if (m_dicUnitInLayer[(int)item.Value.m_putStatus.hasPut].ContainsKey(item.Value.gameObject.GetInstanceID()))
                    {
                        m_dicUnitInLayer[(int)item.Value.m_putStatus.hasPut].Remove(item.Value.gameObject.GetInstanceID());
                    }
                }
            }

            if (m_selectUnit.tag == HomeUnitOnTable)
            {
                if (unit.m_par != null)
                {
                    unit.m_par.RemoveChild(unit);
                }
            }
            HomeUnit homeUnit = m_selectUnit.GetComponent<HomeUnit>();
            homeUnit.DestroyChildWhenRecyle();
            List<int> listObjRec = new List<int>();
            listObjRec.Add(m_selectUnit.gameObject.GetInstanceID());
            EventManager.Instance.DispatchEvent(Common.EventStr.HomeUnitRecycle, new EventDataEx<List<int>>(listObjRec));

            PoolMgr.Instance.RecycleObj(m_selectUnit.gameObject);

            OperationSucceed();
            
        }
    }

    void  OperationSucceed()
    {
        if (m_isClickUpdate == true)
        {
            m_selectUnit = null;
            m_clickUnit = null;

            UIManager.Instance.PopSelf();
            UIManager.Instance.PushPanel(UIPanelName.homeunitselectpanel);
        }
        m_isCanPlace = true;
        m_isCreateNew = false;
    }
    public void UnitSelect(Transform unitNew)
    {
        m_isClickUpdate = false;
        // if user select selected building again
        bool SelectSame = (unitNew == m_selectUnit) ? true : false;

     
        if (SelectSame)
            return;

        if (m_isCanPlace == true)
        {

            // 如果当前可以直接打钩，默认向服务器同步下数据
            SelectUnitPlaceOk();
            if (m_isCreateNew == false)
            {
                m_posLast = unitNew.position;
                m_anglesLast = unitNew.eulerAngles;
            }

            //把旧m_selectUnit发送给服务器

            m_selectUnit = unitNew;

            if (m_selectUnit != null)
            {
                //点击了建筑，摄像头移动到建筑的上方
                if (m_buildMode == EnBuildMode.Build)
                {
                    EventManager.Instance.DispatchEvent(Common.EventStr.BuildCamMove, new EventDataEx<Vector3>(m_selectUnit.position));
                    EventManager.Instance.DispatchEvent(Common.EventStr.HomeUnitCamFollow, new EventDataEx<Transform>(m_selectUnit));

                }

                UIManager.Instance.PopSelf();
                UIManager.Instance.PushPanel(UIPanelName.homeunitplacepanel);
            }
        }
        else if (m_isCanPlace == false)
        {

        }
    }

    bool IsHomeUnit(string tag)
    {
        if (tag == HomeUnit || tag == HomeUnitIsTable || tag == HomeUnitOnTable || tag == HomeUnitOutDoor)
        {
            return true;
        }
        return false;
    }

    bool IsInDoorUnit(string tag)
    {
        if (tag == HomeUnit || tag == HomeUnitIsTable || tag == HomeUnitOnTable)
        {
            return true;
        }
        return false;
    }

    bool IsOutDoorUnit(string tag)
    {
        if (tag == HomeUnitOutDoor)
        {
            return true;
        }
        return false;
    }

    void Pick()
    {
        Ray ray = m_cameraBuild.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = 1 << m_layerWall;
        if (Physics.Raycast(ray, out hit,float.MaxValue,~layerMask))
        {
            if (IsHomeUnit(hit.collider.gameObject.tag))
            {
                Debug.Log("点击到了家具：" + hit.collider.gameObject.name);
                if (m_isCanPlace == true)
                {
                    UnitSelect(hit.collider.transform);
                }
                //m_selectUnit = hit.collider.transform;

            }
            else if (m_selectUnit != null && IsInDoorUnit(m_selectUnit.tag) && hit.collider.gameObject.tag == Floor)
            {
                //强制移动到点击处
                SelectUnitMove(hit.point);
                //点击了建筑，摄像头移动到建筑的上方
                if (m_buildMode == EnBuildMode.Build)
                {
                    EventManager.Instance.DispatchEvent(Common.EventStr.BuildCamMove, new EventDataEx<Vector3>(m_selectUnit.position));
                    EventManager.Instance.DispatchEvent(Common.EventStr.HomeUnitCamFollow, new EventDataEx<Transform>(m_selectUnit));

                }
            }

            else if (m_selectUnit != null && IsOutDoorUnit(m_selectUnit.tag) && hit.collider.gameObject.tag == FloorOutDoor)
            {
                SelectUnitMove(hit.point);
                //点击了建筑，摄像头移动到建筑的上方
                if (m_buildMode == EnBuildMode.Build)
                {
                    EventManager.Instance.DispatchEvent(Common.EventStr.BuildCamMove, new EventDataEx<Vector3>(m_selectUnit.position));
                    EventManager.Instance.DispatchEvent(Common.EventStr.HomeUnitCamFollow, new EventDataEx<Transform>(m_selectUnit));

                }
            }
            //else if (hit.collider.gameObject.tag == Floor)
            //{
            //    UnitSelect(null);
            //}
        }
    }

    void SelectUnitMove(Vector3 hit)
    {
        hit.y -= 0.01f;
        m_selectUnit.GetComponent<HomeUnit>().SetPlace(hit);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_buildMode != EnBuildMode.Build)
        {
            return;
        }
        if (Input.GetMouseButton(0)) //当前有手指按下
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

            //手指是非按下状态
            if (bInTouch == false)
            {

                bInTouch = true;//手指第一次按下且没松手
                ClickAfter = 0.0f;
                mousePosLast = Input.mousePosition;
                bTemporarySelect = false;
                Dragged = false;

                Ray ray = m_cameraBuild.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                int layerMask = 1 << m_layerWall;
                if (Physics.Raycast(ray, out hit,float.MaxValue,~layerMask) )
                {
                    Debug.Log("建造射线点击到了：" + hit.transform.name);
                    if (IsHomeUnit(hit.collider.gameObject.tag))
                    {
                        m_clickUnit = hit.collider.transform;
                    }
                    else
                    {
                        m_clickUnit = null;
                    }
                }
                else
                {
                    m_clickUnit = null;
                }
            }
            else//当前手指是按下状态
            {
                if (Vector3.Distance(Input.mousePosition, mousePosLast) > 0.01f)
                {
                    if (!Dragged)
                    {
                        Dragged = true;
                        if ((m_selectUnit != null) && (m_selectUnit == m_clickUnit))
                        {
                            if (m_buildMode == EnBuildMode.Build)
                            {
                                //MobileTouchCamera.instance.OnDragSceneObject();
                                EventManager.Instance.DispatchEvent(Common.EventStr.CameraDrag, new EventDataEx<bool>(false));
                            }
                        }
                    }

                    mousePosLast = Input.mousePosition;
                    //if ((m_selectUnit != null) && (m_clickUnit == m_selectUnit) && m_buildMode == EnBuildMode.Build)
                    if ((m_selectUnit != null)  && m_buildMode == EnBuildMode.Build)
                    {
                        //Ray ray = m_cameraBuild.ScreenPointToRay(Input.mousePosition);
                        Ray ray = m_cameraBuild.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
                        if (IsInDoorUnit(m_selectUnit.tag))
                        {
                            int layerMask = 1 << m_layerInDoor;
                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit,float.MaxValue, layerMask))
                            {
                                Vector3 pos = hit.point;
                                pos.y -= 0.01f;

                                hit.point = pos;
                                m_selectUnit.GetComponent<HomeUnit>().SetPlace(hit.point);
                            }
                        }
                        else if (IsOutDoorUnit(m_selectUnit.tag))
                        {
                            int layerMask = 1 << m_layerOutDoor;
                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit, float.MaxValue,layerMask))
                            {
                                Vector3 pos = hit.point;
                                pos.y -= 0.01f;

                                hit.point = pos;
                                m_selectUnit.GetComponent<HomeUnit>().SetPlace(hit.point);
                            }
                        }
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
                            Pick();
                            //Debug.Log ("Update2 buildingSelected:"+((buildingSelected != null) ? buildingSelected.name : "none"));
                            //按下到松手且不移动，判定为点击状态
                            //PickOneBox();
                        }
                    }
                }
            }
        }
        else //当前没有手指按下,即可能刚松开了手指
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
                        Pick();
                    }
                }
            }
        }
    }


    public void ChangeToBuild()
    {
        m_navBuilder.StopNavMesh();
        m_buildMode = EnBuildMode.Build;
        m_cameraBuild.transform.localPosition = new Vector3(0, 20, 0);
        Vector3 angles = new Vector3(90, 0, -(float)VcData.m_dicHomeTownBuildPos.dic[m_userHomeInfo.landCode].dirY);


        //Vector3 homePosInWorld = m_curHome.transform.TransformPoint(homePosIn);
        //Vector3 homePosInHomeMgrLocal = m_homeAllPar.transform.InverseTransformPoint(homePosInWorld);


        m_cameraBuild.transform.localEulerAngles = angles;
        m_cameraBuild.gameObject.SetActive(true);
        m_cameraPlayer.gameObject.SetActive(false);
        if (UIManager.Instance.IsTopPanel(UIPanelName.joystickpanel))
        {
            UIManager.Instance.PopSelf();
        }
        UIManager.Instance.PushPanel(UIPanelName.homeunitselectpanel);

        m_dicLayerObj[4].SetActive(false);// 
        SetLayerShow(m_showLayer);
    }

    IEnumerator YieldChangeToDisplay()
    {
        m_cameraBuild.gameObject.SetActive(false);
        m_cameraPlayer.gameObject.SetActive(true);

        
        m_buildMode = EnBuildMode.Display;


        //显示所有楼层
        foreach (var item in m_dicLayerObj)
        {
            item.Value.SetActive(true);
        }

        foreach (var item in m_dicUnitInLayer)
        {
            foreach (var unit in item.Value)
            {
                if (unit.Value != null)
                {
                    unit.Value.SetActive(true);
                }
            }
        }

        //if (UIManager.Instance.IsTopPanel(UIPanelName.homeunitselectpanel))
        //{
        //    UIManager.Instance.PopSelf();
        //}
        //建造模式必有一个ui
        UIManager.Instance.PopSelf();
        UIManager.Instance.PushPanel(UIPanelName.joystickpanel);
        m_selectUnit = null;
        m_clickUnit = null;
        m_navBuilder.StartNavMesh();
        yield return null;
    }

    Coroutine m_corChangeToDisplay;
    public void ChangeToDisplay()
    {
        if (m_corChangeToDisplay != null)
        {
            StopCoroutine(m_corChangeToDisplay);
        }
        m_corChangeToDisplay = StartCoroutine(YieldChangeToDisplay());
    }

    /// <summary>
    /// 点击创建一个家具
    /// </summary>
    /// <param name="part"></param>
    public void CreateNewUnitByClick(PartProperties part)
    {
        m_isCreateNew = true;
        string modelName = part.modleData;

        EnHomeUnit selfType = (EnHomeUnit)part.selftype;


        Vector3 pos = Vector3.zero;


        switch (selfType)
        {
            case EnHomeUnit.HomeUnit:

            case EnHomeUnit.HomeUnitIsTable:

            case EnHomeUnit.HomeUnitOnTable:


                {
                    Ray ray = new Ray(m_lookInDoor.position, -m_lookInDoor.up);
                    RaycastHit hit;
                    int layerMask = 1 << m_layerInDoor;
                    if (Physics.Raycast(ray, out hit, 1000.0f, layerMask))
                    {
                        pos = hit.point;
                        pos.y -= 0.01f;
                    }
                }
                break;
            case EnHomeUnit.HomeUnitOutDoor:
                {
                    Ray ray = new Ray(m_lookOutDoor.position, -m_lookOutDoor.up);
                    RaycastHit hit;
                    int layerMask = 1 << m_layerOutDoor;
                    if (Physics.Raycast(ray, out hit, 1000.0f, layerMask))
                    {
                        pos = hit.point;
                        pos.y -= 0.01f;
                    }
                }
                break;
            default:
                break;
        }

        AssetMgr.Instance.CreateObj(modelName, modelName, m_unitPar, pos, Vector3.zero, new Vector3(-10000,0,0),
            (param)=>{
                param.tag = ((EnHomeUnit)(part.selftype)).ToString();
                HomeUnit unit = param.AddComponent<HomeUnit>();
                unit.m_putStatus.modelId = part.id;
                SetSelectUnit(param.transform);
                UIManager.Instance.PopSelf();
                UIManager.Instance.PushPanel(UIPanelName.homeunitplacepanel);
        }
            );
    }

    void SetSelectUnit(Transform trans)
    {
        m_selectUnit = trans;
        m_clickUnit = trans;
        if (m_buildMode == EnBuildMode.Build)
        {
            EventManager.Instance.DispatchEvent(Common.EventStr.BuildCamMove, new EventDataEx<Vector3>(m_selectUnit.position));
            EventManager.Instance.DispatchEvent(Common.EventStr.HomeUnitCamFollow, new EventDataEx<Transform>(m_selectUnit));

        }
    }

    public void SelectUnitRotateByClick(bool isShun)
    {
        if (m_selectUnit != null)
        {
            if (isShun == true)
            {
                Vector3 angles = m_selectUnit.transform.localEulerAngles;
                angles.y += 1.0f;

                m_selectUnit.transform.localEulerAngles = angles;
            }
            else
            {
                Vector3 angles = m_selectUnit.transform.localEulerAngles;
                angles.y -= 1.0f;

                m_selectUnit.transform.localEulerAngles = angles;
            }
        }
    }

    public void SelectUnitRotate(bool isShun)
    {
        if (m_selectUnit != null)
        {
            if (isShun == true)
            {
                m_selectUnit.Rotate(Vector3.up * 180 * Time.deltaTime, Space.Self);
            }
            else {
                m_selectUnit.Rotate(Vector3.up * -180 * Time.deltaTime, Space.Self);
            }
        }
    }


    void ChangeLayer()
    {
        HomeUnit homeUint = m_selectUnit.GetComponent<HomeUnit>();

        //有些楼层可以直接从1楼拖到2楼
        foreach (var item in m_dicUnitInLayer)
        {
            if (item.Value.ContainsKey(m_selectUnit.GetInstanceID()))
            {
                item.Value.Remove(m_selectUnit.GetInstanceID());
            }
        }
        m_dicUnitInLayer[homeUint.m_putStatus.hasPut][m_selectUnit.GetInstanceID()] = m_selectUnit.gameObject;

        //同时把自己的子物体放到改楼层
        if (m_selectUnit.tag == HomeUnitIsTable)
        {
            List<int> listDelete = new List<int>();

            foreach (var item in homeUint.m_dicChild)
            {
                if (item.Value != null)
                {
                    item.Value.ChildSetPutStatus();

                    foreach (var itemLayer in m_dicUnitInLayer)
                    {
                        if (itemLayer.Value.ContainsKey(item.Value.gameObject.GetInstanceID()))
                        {
                            itemLayer.Value.Remove(item.Value.gameObject.GetInstanceID());
                        }
                    }
                    m_dicUnitInLayer[homeUint.m_putStatus.hasPut][item.Value.gameObject.GetInstanceID()] = item.Value.gameObject;
                }
                else
                {
                    listDelete.Add(item.Key);
                }
            }
            for (int i = 0; i < listDelete.Count; i++)
            {
                homeUint.m_dicChild.Remove(listDelete[i]);
            }
        }
    }
    public void SelectUnitPlaceOk(bool isClick = false)
    {
        m_isClickUpdate = isClick;
        if (m_selectUnit != null)
        {
            ChangeLayer();
            

            if (m_isCreateNew == true)
            {
                m_newUnit = m_selectUnit;
                SendCreateNew();
                m_isCreateNew = false;
            }
            else {
                SendUpdate();
            }
          
        }

    }

    public void SelectUnitCancel()
    {
        if (m_selectUnit != null)
        {
            if (m_isCreateNew == true)
            {
                OnRspRecycle();
                //PoolMgr.Instance.RecycleObj(m_selectUnit.gameObject);
                //同时ui回复一个
            }
            else
            {
                //回到上一步的状态
                m_selectUnit.transform.position = m_posLast;
                m_selectUnit.transform.eulerAngles = m_anglesLast;
                ChangeLayer();

                m_selectUnit = null;
                m_clickUnit = null;

                UIManager.Instance.PopSelf(); //关闭placeui
                UIManager.Instance.PushPanel(UIPanelName.homeunitselectpanel);
            }
        }
    }

    int m_showLayer = 1;
    //显示第几个楼层的东西
    public void SetLayerShow(int layer)
    {
        m_showLayer = layer;
        foreach (var item in m_dicLayerObj)
        {
            if (item.Key > layer)
            {
                item.Value.SetActive(false);
                if (item.Key != 4)
                {
                    foreach (var unit in m_dicUnitInLayer[item.Key])
                    {
                        if (unit.Value != null)
                        {
                            unit.Value.SetActive(false);
                        }
                    }
                }
            }
            else {
                item.Value.SetActive(true);
                if (item.Key != 4)
                {
                    foreach (var unit in m_dicUnitInLayer[item.Key])
                    {
                        if (unit.Value != null)
                        {
                            unit.Value.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}
