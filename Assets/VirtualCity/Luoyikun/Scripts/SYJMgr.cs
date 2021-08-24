using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Framework.UI;
using UnityEngine.EventSystems;
using ProtoDefine;
using SGF.Codec;
using Framework.Tools;
using Framework.Event;
using Newtonsoft.Json;
using HUDText;

public class SYJMgr : MonoBehaviour {

    public static SYJMgr m_instance;

    public Transform m_playerPar;
    //public Transform m_camera;
    public MouseOrbitImproved m_camera;
    public Dictionary<long, GameObject> m_dicPlayerObj = new Dictionary<long, GameObject>();//创建的player

    public string m_sceneId;
    public Transform m_myPlayer;
    public Dictionary<long?, Player> m_dicPlayerInfo = new Dictionary<long?, Player>();

    Vector3 m_posLast;


    private bool bInTouch = false;
    private float ClickAfter = 0.0f;
    private Vector3 mousePosLast = Vector3.zero;
    private bool Dragged = false;

    private bool bTemporarySelect = false;

    public Camera m_myCamera;

    //NavMeshAgent m_myAgent;
    //Animator m_myAni;

    MoveController m_myMoveCtrl;

    public Transform m_teleportbeamPar;

    public GameObject m_camZhangGui;

    public Transform m_zhangGuiPar;
public Transform m_npcPar;    public GameObject TextImage;    // Use this for initialization
    private void Awake()
    {
        m_instance = this;
    }
    void Start() {
        CreateFixModels();
        
    }

    private void OnDestroy()
    {
        
    }

    void OnEvCloseCamZhangGui(EventData data)
    {
        m_camZhangGui.SetActive(false);
    }
    void OnEvChatWithShopkeeper(EventData data)
    {
        var exdata = data as EventDataEx<bool>;
        bool isOpen = exdata.GetData();
        m_camZhangGui.SetActive(isOpen);
    }
    void MoveCamZhangGui(long shopId)
    {
        ShopsProperties info = DataMgr.m_dicShopsProperties[shopId];
        PosAndAngle posAndAn = JsonConvert.DeserializeObject<PosAndAngle>(info.cameraPos);
        Vector3 pos = new Vector3(posAndAn.x, posAndAn.y, posAndAn.z);
        Vector3 angle = new Vector3(posAndAn.dirX, posAndAn.dirY, posAndAn.dirZ);
        m_camZhangGui.transform.localPosition = pos;
        m_camZhangGui.transform.localEulerAngles = angle;
    }
    public void SetJoystick(VariableJoystick joyTmp,bool isNew = true)
    {
        DataInit();
        //UIManager.Instance.PushPanel(UIPanelName.playerheadpanel, false, false, (param) => { CreateMySelf(joyTmp); }, true);
        UIManager.Instance.PushPanel(Vc.AbName.hudpanel,false,false, (param) => {
            hudpanel hud = param.GetComponent<hudpanel>();
            hud.SetCamera(m_myCamera);
            hud.ClearAll();
            CreateMySelf(joyTmp, isNew); }, true);
        //RecyleNpc();
        //CreateNpc();
    }

    void RecyleNpc()
    {
        for (int i = m_npcPar.childCount-1; i >= 0; i--)
        {
            Destroy(m_npcPar.GetChild(i).gameObject);
        }
    }
    void CreateFixModels()
    {
        foreach (var item in DataMgr.m_dicShopsProperties)
        {
            if (item.Value.isInOut == 0)
            {
                //Vector3 pos = new Vector3(item.Value.x, item.Value.y, item.Value.z);
                //AssetMgr.Instance.CreateObj(Vc.AbName.teleportbeam, Vc.AbName.teleportbeam, m_teleportbeamPar, pos, Vector3.zero, Vector3.one,
                //    (param) =>
                //    {
                //        TeleportCtrl tele = param.AddComponent<TeleportCtrl>();
                //        tele.m_info = item.Value;F
                //    }
                //    );
                PosAndAngle posAndAn = JsonConvert.DeserializeObject<PosAndAngle>(item.Value.npcPos);
                Vector3 pos = new Vector3(posAndAn.x, posAndAn.y, posAndAn.z);
                Vector3 angle = new Vector3(posAndAn.dirX, posAndAn.dirY, posAndAn.dirZ);
                AssetMgr.Instance.CreateObj(item.Value.npcModel, item.Value.npcModel, m_zhangGuiPar, pos, angle, Vector3.one, (param) =>
                {
                    ZhangGuiInfo info = param.AddComponent<ZhangGuiInfo>();
                    info.m_info = item.Value;
                });
            }
            else if (item.Value.isInOut == 1)
            {
                PosAndAngle posAndAn = JsonConvert.DeserializeObject<PosAndAngle>(item.Value.npcPos);
                Vector3 pos = new Vector3(posAndAn.x, posAndAn.y, posAndAn.z);
                Vector3 angle = new Vector3(posAndAn.dirX, posAndAn.dirY, posAndAn.dirZ);
                AssetMgr.Instance.CreateObj(item.Value.npcModel, item.Value.npcModel, m_zhangGuiPar, pos, angle, Vector3.one, (param) => {
                    ZhangGuiInfo info = param.AddComponent<ZhangGuiInfo>();
                    info.m_info = item.Value;
                });
            }
        }
    }
    private void OnEnable()
    {
        EventManager.Instance.AddEventListener(Common.EventStr.PlayerNavGotoPoint, OnEvPlayerNavGotoPoint);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspRunInStreetMessage, OnNetRspRunInStreetMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspSyncStreetMessage, OnNetRspSyncStreetMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspRunInScenceMessage, OnNetRspRunInScenceMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspRunOutScenceMessage, OnNetRspRunOutScenceMessage);
        EventManager.Instance.AddEventListener(Common.EventStr.CloseCamZhangGui, OnEvCloseCamZhangGui);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.ChatMessage,OnNetChatMessage);
        EventManager.Instance.AddEventListener(Common.EventStr.ChatWithShopkeeper, OnEvChatWithShopkeeper);
    }


    private void OnDisable()
    {
        //WhenDestroy();
        DataInit();
        EventManager.Instance.RemoveEventListener(Common.EventStr.PlayerNavGotoPoint, OnEvPlayerNavGotoPoint);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspRunInStreetMessage, OnNetRspRunInStreetMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspSyncStreetMessage, OnNetRspSyncStreetMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspRunInScenceMessage, OnNetRspRunInScenceMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspRunOutScenceMessage, OnNetRspRunOutScenceMessage);
        TimeManager.Instance.RemoveTask(SendReqSyncStreetMessage);
        EventManager.Instance.RemoveEventListener(Common.EventStr.CloseCamZhangGui, OnEvCloseCamZhangGui);
        EventManager.Instance.RemoveEventListener(Common.EventStr.ChatWithShopkeeper, OnEvChatWithShopkeeper);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.ChatMessage, OnNetChatMessage);
    }

    void OnNetChatMessage(byte[] buf)
    {
        ChatMessage rsp = PBSerializer.NDeserialize<ChatMessage>(buf);
        if (rsp.messageFunc != 0)
        {
            return;
        }
        if (m_dicPlayerObj.ContainsKey(rsp.accountId))
        {
            hudpanel.m_instance.CreateOneHeadUi(rsp.accountId, rsp.message, m_dicPlayerObj[rsp.accountId].transform,"Chat");
        }
    }
    void OnEvPlayerNavGotoPoint(EventData data)
    {
        var exdata = data as EventDataEx<Vector3>;
        Vector3 pos = exdata.GetData();

        m_myMoveCtrl.SetPointByNav(pos);
        
    }
    public void SendReqRunOutStreetMessage()
    {
        ReqRunOutStreetMessage req = new ReqRunOutStreetMessage();
        req.scenceId = m_sceneId;
        req.playerId = (long)DataMgr.m_account.id;
        RoomSocket.Instance.SendMsgProto(MsgIdDefine.ReqRunOutStreetMessage, req);
        
    }

    void OnNetRspRunOutScenceMessage(byte[] buf)
    {
        RspRunOutScenceMessage rsp = PBSerializer.NDeserialize<RspRunOutScenceMessage>(buf);

        if (m_dicPlayerInfo != null)
        {
            if (m_dicPlayerInfo.ContainsKey(rsp.playerId))
            {
                m_dicPlayerInfo.Remove(rsp.playerId);
            }
        }
        if (m_dicPlayerObj.ContainsKey(rsp.playerId))
        {
            //Destroy(m_dicPlayerObj[rsp.playerId]);
            PoolMgr.Instance.RecycleObj(m_dicPlayerObj[rsp.playerId]);
            //playerheadpanel.m_instance.Recycle(rsp.playerId);
            hudpanel.m_instance.Recycle(rsp.playerId);
            m_dicPlayerObj.Remove(rsp.playerId);
        }

    }
    void OnNetRspRunInScenceMessage(byte[] buf)
    {
        RspRunInScenceMessage rsp = PBSerializer.NDeserialize<RspRunInScenceMessage>(buf);
        if (rsp.player.accountId == DataMgr.m_account.id)
        {
            return;
        }
        if (m_dicPlayerInfo.ContainsKey(rsp.player.accountId) == false)
        {
            m_dicPlayerInfo[rsp.player.accountId] = rsp.player;
        }
        if (m_dicPlayerObj.ContainsKey(rsp.player.accountId) == false)
        {
            Vector3 pos = new Vector3(rsp.playerStatus.x, rsp.playerStatus.y, rsp.playerStatus.z);
            Vector3 angles = new Vector3(rsp.playerStatus.px, rsp.playerStatus.py, rsp.playerStatus.pz);

            string modelName = DataMgr.m_dicRoleProperties[m_dicPlayerInfo[rsp.player.accountId].modelId].ModelDate;
            AssetMgr.Instance.CreateObj(modelName, modelName, m_playerPar, Vector3.zero, Vector3.zero, Vector3.one, (param) =>
            {
                param.transform.position = pos;
                param.transform.eulerAngles = angles;
                m_dicPlayerObj[(long)rsp.player.accountId] = param;
                param.GetComponent<NavMeshAgent>().enabled = true;
                param.AddComponent<RemotePlayerCtrl>().m_playerInfo = m_dicPlayerInfo[rsp.player.accountId];
                param.tag = DataMgr.m_tagRemote;
                Transform paramPar = param.transform.Find("LookAt");
                //playerheadpanel.m_instance.CreateOneHeadUi(rsp.player.accountId, rsp.player.name, param.transform.Find("LookAt"));
                hudpanel.m_instance.CreateOneHeadUi(rsp.player.accountId, rsp.player.name, param.transform);
                //PublicFunc.GetFirstRenderer(param).AddComponent<ModelVisible>().m_id = rsp.player.accountId;

                ModelVisible mv = param.AddComponent<ModelVisible>();
                mv.m_id = rsp.player.accountId.ToString();
                mv.m_cam = m_myCamera;
            }
            );


        }
    }

    void OnNetRspSyncStreetMessage(byte[] buf)
    {
        RspSyncStreetMessage rsp = PBSerializer.NDeserialize<RspSyncStreetMessage>(buf);
        Dictionary<long?, PlayerStatus> dicPlayerStatus = new Dictionary<long?, PlayerStatus>();
        if (rsp.playerStatusMap != null)
        {
            dicPlayerStatus = rsp.playerStatusMap;
        }
        foreach (var item in dicPlayerStatus)
        {
            if (PublicFunc.IsMy((long)item.Key))
            {
                continue;
            }

            if (m_dicPlayerObj.ContainsKey((long)item.Key) == true)
            {

                Vector3 pos = new Vector3(item.Value.x, item.Value.y, item.Value.z);
                Vector3 angles = new Vector3(item.Value.px, item.Value.py, item.Value.pz);
                NavMeshAgent Agent = m_dicPlayerObj[(long)item.Key].GetComponent<NavMeshAgent>();
                Agent.destination = pos;
                
            }

        }
    }

    void OnNetRspRunInStreetMessage(byte[] buf)
    {
        // 得到在场玩家的信息
        RspRunInStreetMessage rsp = PBSerializer.NDeserialize<RspRunInStreetMessage>(buf);
        if (rsp.players == null)
        {
            m_dicPlayerInfo = new Dictionary<long?, Player>();
        }
        else
        {
            m_dicPlayerInfo = rsp.players;
        }
        
        m_sceneId = rsp.scenceId;

        if (rsp.playerStatus != null)
        {
            foreach (var item in rsp.playerStatus)
            {
                if (item.Key == DataMgr.m_account.id)
                {
                    continue;
                }
                if (m_dicPlayerObj.ContainsKey((long)item.Key) == false)
                {
                    string modelName = DataMgr.m_dicRoleProperties[m_dicPlayerInfo[item.Key].modelId].ModelDate;

                    Vector3 pos = new Vector3(item.Value.x, item.Value.y, item.Value.z);
                    Vector3 angles = new Vector3(item.Value.px, item.Value.py, item.Value.pz);

                    AssetMgr.Instance.CreateObj(modelName, modelName, m_playerPar, Vector3.zero, Vector3.zero, Vector3.one, (param) =>
                    {
                        param.transform.position = pos;
                        param.transform.eulerAngles = angles;
                        m_dicPlayerObj[(long)item.Key] = param;
                        param.GetComponent<NavMeshAgent>().enabled = true;
                        param.AddComponent<RemotePlayerCtrl>().m_playerInfo = m_dicPlayerInfo[item.Key];
                        param.tag = DataMgr.m_tagRemote;
                        //playerheadpanel.m_instance.CreateOneHeadUi((long)item.Key, m_dicPlayerInfo[item.Key].name, param.transform.Find("LookAt"));
                        Transform paramPar = param.transform.Find("LookAt");
                        hudpanel.m_instance.CreateOneHeadUi((long)item.Key, m_dicPlayerInfo[item.Key].name, param.transform);
                        //PublicFunc.GetFirstRenderer(param).AddComponent<ModelVisible>().m_id = m_dicPlayerInfo[item.Key].accountId;
                        ModelVisible mv = param.AddComponent<ModelVisible>();
                        mv.m_id = m_dicPlayerInfo[item.Key].accountId.ToString();
                        mv.m_cam = m_myCamera;
                    }
                    );
                }
            }
        }

        TimeManager.Instance.AddTask(0.3f, true, SendReqSyncStreetMessage);
    }
    void DataInit()
    {
        foreach (var item in m_dicPlayerObj)
        {
            if (item.Value != null)
            {
                //Destroy(item.Value);
                PoolMgr.Instance.RecycleObj(item.Value);
            }
        }
        m_dicPlayerObj.Clear();

        if (m_dicPlayerInfo != null)
        {
            m_dicPlayerInfo.Clear();
        }
    }

    void RunAniSet(GameObject obj ,bool isRun)
    {
        obj.GetComponent<Animator>().SetBool("IsRun", isRun);
    }
    void CreateMySelf(VariableJoystick joyTmp,bool isNew = true)
    {
        string modelName = PublicFunc.GetUserModelName(DataMgr.m_account);

        //{ "x":9.36,"y":-0.007,"z":-12.46,"dirX":0,"dirY":179.85,"dirZ":0}
        AssetMgr.Instance.CreateObj(modelName, modelName, m_playerPar, new Vector3(10,0,-10), new Vector3(0,180,0), Vector3.one, (parma) => {
            m_camera.target = parma.transform.Find("LookAt");
            MoveController moveCtrl = parma.AddComponent<MoveController>();
            moveCtrl.m_camera = m_camera.transform;
            moveCtrl.m_joy = joyTmp;
            m_myMoveCtrl = moveCtrl;
            parma.tag = "Player";
            parma.GetComponent<NavMeshAgent>().enabled = true;
            m_dicPlayerObj[(long)DataMgr.m_account.id] = parma;
            m_myPlayer = parma.transform;

           

            if (isNew == true)
            {
                ReqRunInStreetMessage req = new ReqRunInStreetMessage();
                PlayerStatus playerSta = new PlayerStatus();

                playerSta.x = parma.transform.position.x;
                playerSta.y = parma.transform.position.y;
                playerSta.z = parma.transform.position.z;

                playerSta.px = parma.transform.eulerAngles.x;
                playerSta.py = parma.transform.eulerAngles.y;
                playerSta.pz = parma.transform.eulerAngles.z;
                req.playerStatus = playerSta;

                RoomSocket.Instance.SendMsgProto(MsgIdDefine.ReqRunInStreetMessage, req);
            }
            m_posLast = parma.transform.position;
            Transform paramPar = parma.transform.Find("LookAt");
            hudpanel.m_instance.CreateOneHeadUi((long)DataMgr.m_account.id, DataMgr.m_account.userName, parma.transform);
            
            //playerheadpanel.m_instance.CreateOneHeadUi((long)DataMgr.m_account.id, DataMgr.m_account.userName, parma.transform.Find("LookAt"));
        });
    }

    void WhenDestroy()
    {
        m_myMoveCtrl.StopNav();
    }
    void SendReqSyncStreetMessage()
    {
        if ((m_posLast - m_myPlayer.transform.position).sqrMagnitude <= 0.1f)
        {
            return;
        }
         
        ReqSyncStreetMessage req = new ReqSyncStreetMessage();
        req.scenceId = m_sceneId;
        PlayerStatus playerSta = new PlayerStatus();

        playerSta.x = m_myPlayer.position.x;
        playerSta.y = m_myPlayer.position.y;
        playerSta.z = m_myPlayer.position.z;
        playerSta.px = m_myPlayer.eulerAngles.x;
        playerSta.py = m_myPlayer.eulerAngles.y;
        playerSta.pz = m_myPlayer.eulerAngles.z;
        req.playerStatus = playerSta;
        RoomSocket.Instance.SendMsgProto(MsgIdDefine.ReqSyncStreetMessage, req);
        m_posLast = m_myPlayer.transform.position;
    }
    void PickOnePlayer()
    {
        Ray ray = m_myCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == DataMgr.m_tagRemote)
            {
                Debug.Log("点击到了远程");
                //hit.collider.gameObject.GetComponent<BoxCtrl>().OnPick();
                Player playerInfo = hit.collider.gameObject.GetComponent<RemotePlayerCtrl>().m_playerInfo;

                UIManager.Instance.PushPanel(UIPanelName.userinfopanel, false, true, (paragm) => { paragm.GetComponent<userinfopanel>().InitPlayer(playerInfo.accountId); });

            }
            //else if (hit.collider.gameObject.tag == DataMgr.m_tagFakeRemote)
            //{
            //    Player playerInfo = hit.collider.gameObject.GetComponent<RemotePlayerCtrl>().m_playerInfo;
            //    UIManager.Instance.PushPanel(UIPanelName.userinfopanel, false, true, (paragm) => 
            //    { paragm.GetComponent<userinfopanel>().InfoInit(EnUserInfoType.FakeRemote,playerInfo.name); });
            //}
            else if (hit.collider.gameObject.tag == DataMgr.m_tagNpc)
            {
                Debug.Log("点击了掌柜");

                ZhangGuiInfo info = hit.collider.gameObject.GetComponent<ZhangGuiInfo>();
                PosAndAngle posAA = JsonConvert.DeserializeObject<PosAndAngle>(info.m_info.cameraPos);
                Vector3 pos = new Vector3(posAA.x, posAA.y, posAA.z);
                Vector3 angle = new Vector3(posAA.dirX, posAA.dirY, posAA.dirZ);
                if (info.m_info.isInOut == 0)
                {
                    m_camZhangGui.transform.localPosition = pos;
                    m_camZhangGui.transform.localEulerAngles = angle;
                    m_camZhangGui.SetActive(true);
                   // MoveCamZhangGui(info.m_info.);
                    UIManager.Instance.PushPanel(Vc.AbName.shoppingmunepanel, false, false, (param) =>
                        {
                            shoppingmunepanel chat = param.GetComponent<shoppingmunepanel>();
                            chat.ReqGGLM(info.m_info.businessId);
                        }
                    );
                }
                else if(info.m_info.isInOut==1)
                {
                    UIManager.Instance.PushPanel(Vc.AbName.jiajushangchengpanel, false, false, (param) =>
                    {
                        jiajushangchengpanel jiaju = param.GetComponent<jiajushangchengpanel>();
                        jiaju.set_type((int)EnJjscType.Diamond);
                    });
                }
               
            }
        }
    }


    private void Update()
    {
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
                            PickOnePlayer();
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
                        PickOnePlayer();
                    }
                }
            }
        }
    }

    void CreateNpc()
    {
        List<int> listName = new List<int>();

        for (int i = 0; i < 2; i++)
        {
            int modelIdx = Random.Range(0, DataMgr.m_listRoleProperties.Count );
            string modelName = DataMgr.m_listRoleProperties[modelIdx].ModelDate;
            int nameIdx = Random.Range(0, VcData.m_listFakeName.Count);
            bool isContains = listName.Contains(nameIdx);
            while (isContains == true)
            {
                nameIdx = Random.Range(0, VcData.m_listFakeName.Count);
                isContains = listName.Contains(nameIdx);
                if (isContains == false)
                {
                    break;
                }
            }
            listName.Add(nameIdx);
          

            string npcName = VcData.m_listFakeName[nameIdx].name;

            float posX = Random.Range(15, 30);
            float posY = Random.Range(-15, -30);
            //Vector3 pos = PublicFunc.GetRandomPos(new Vector3(10, 0, -10));
            AssetMgr.Instance.CreateObj(modelName, modelName, m_npcPar, new Vector3(posX, 0, -10), Vector3.zero, Vector3.one, (param) => {
                //param.AddComponent<Vc.RandomWalk>();
                param.GetComponent<NavMeshAgent>().enabled = true;
                param.AddComponent<RemotePlayerCtrl>().m_playerInfo.accountId = VcData.m_listFakeName[nameIdx].accountId;
                param.tag = DataMgr.m_tagRemote;
                hudpanel.m_instance.CreateOneHeadUi(VcData.m_listFakeName[nameIdx].accountId, npcName, param.transform);
                ModelVisible mv = param.AddComponent<ModelVisible>();
                mv.m_id = VcData.m_listFakeName[nameIdx].accountId.ToString();
                mv.m_cam = m_myCamera;
            }
            );
        }
        
    }
    
}
