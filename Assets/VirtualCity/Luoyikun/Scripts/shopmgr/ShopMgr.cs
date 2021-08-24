using Framework.Event;
using Framework.Tools;
using Framework.UI;
using Newtonsoft.Json;
using ProtoDefine;
using SGF.Codec;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class ShopMgr : MonoBehaviour {

    public static ShopMgr m_instance;
    public Transform m_shopPar;
    public Transform m_playerPar;
    public Transform m_npcPar;


    MouseOrbitImproved m_camera;

    public Dictionary<long, GameObject> m_dicPlayerObj = new Dictionary<long, GameObject>();//创建的player


    string m_sceneId;
    public long m_shopId;
    public Transform m_myPlayer;
    public Dictionary<long?, Player> m_dicPlayerInfo = new Dictionary<long?, Player>();

    Vector3 m_posLast;


    private bool bInTouch = false;
    private float ClickAfter = 0.0f;
    private Vector3 mousePosLast = Vector3.zero;
    private bool Dragged = false;

    private bool bTemporarySelect = false;

    Camera m_myCamera;

    
    //NavMeshAgent m_myAgent;
    //Animator m_myAni;

    MoveController m_myMoveCtrl;

    public GameObject m_camZhangGui;

    public Transform m_zhangGuiPar;

    hudpanel m_hud;

    ShopsProperties m_shopInfo;
    private void Awake()
    {
        m_instance = this;

    }
    // Use this for initialization
    void Start () {
        

    }

    private void OnDestroy()
    {
        
    }

    void OnEvCloseCamZhangGui(EventData data)
    {
        m_camZhangGui.SetActive(false);
    }

    private void OnEnable()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspRunInShopMessage, OnNetRspRunInShopMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspSyncShopMessage, OnNetRspSyncShopMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspRunInScenceMessage, OnNetRspRunInScenceMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspRunOutScenceMessage, OnNetRspRunOutScenceMessage);
        EventManager.Instance.AddEventListener(Common.EventStr.ChatWithShopkeeper, OnEvChatWithShopkeeper);
        EventManager.Instance.AddEventListener(Common.EventStr.CloseCamZhangGui, OnEvCloseCamZhangGui);
    }


    private void OnDisable()
    {
        //RenderSettings.fog = false;
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspRunInShopMessage, OnNetRspRunInShopMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspSyncShopMessage, OnNetRspSyncShopMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspRunInScenceMessage, OnNetRspRunInScenceMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspRunOutScenceMessage, OnNetRspRunOutScenceMessage);
        TimeManager.Instance.RemoveTask(SendReqSyncStreetMessage);
        RecycleZhangGui();
        EventManager.Instance.RemoveEventListener(Common.EventStr.ChatWithShopkeeper, OnEvChatWithShopkeeper);
        EventManager.Instance.RemoveEventListener(Common.EventStr.CloseCamZhangGui, OnEvCloseCamZhangGui);
    }

    void OnEvChatWithShopkeeper(EventData data)
    {
        var exdata = data as EventDataEx<bool>;
        bool isOpen = exdata.GetData();
        m_camZhangGui.SetActive(isOpen);
    }
    void OnNetRspRunInShopMessage(byte[] buf)
    {
        // 得到在场玩家的信息
        RspRunInShopMessage rsp = PBSerializer.NDeserialize<RspRunInShopMessage>(buf);

        if (rsp.players == null)
        {
            m_dicPlayerInfo = new Dictionary<long?, Player>();
        }
        else
        {
            m_dicPlayerInfo = rsp.players;
        }
        //m_sceneId = rsp.scenceId;

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

    void OnNetRspSyncShopMessage(byte[] buf)
    {
        RspSyncShopMessage rsp = PBSerializer.NDeserialize<RspSyncShopMessage>(buf);
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

            Debug.Log("当前需要同步的人" + item.Key);
            if (m_dicPlayerObj.ContainsKey((long)item.Key) == true)
            {

                Vector3 pos = new Vector3(item.Value.x, item.Value.y, item.Value.z);
                Vector3 angles = new Vector3(item.Value.px, item.Value.py, item.Value.pz);
                NavMeshAgent Agent = m_dicPlayerObj[(long)item.Key].GetComponent<NavMeshAgent>();
                Agent.destination = pos;

            }

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

                //playerheadpanel.m_instance.CreateOneHeadUi(rsp.player.accountId, rsp.player.name, param.transform.Find("LookAt"));
                Transform paramPar = param.transform.Find("LookAt");
                hudpanel.m_instance.CreateOneHeadUi(rsp.player.accountId, rsp.player.name, param.transform);
                //PublicFunc.GetFirstRenderer(param).AddComponent<ModelVisible>().m_id = rsp.player.accountId;
                ModelVisible mv = param.AddComponent<ModelVisible>();
                mv.m_id = rsp.player.accountId.ToString();
                mv.m_cam = m_myCamera;
            }
            );


        }
    }

    void OnNetRspRunOutScenceMessage(byte[] buf)
    {
        RspRunOutScenceMessage rsp = PBSerializer.NDeserialize<RspRunOutScenceMessage>(buf);
        if (m_dicPlayerInfo.ContainsKey(rsp.playerId))
        {
            m_dicPlayerInfo.Remove(rsp.playerId);
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
    // Update is called once per frame
    void Update () {
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


    void PickOnePlayer()
    {
        if (m_myCamera != null)
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
                else if (hit.collider.gameObject.tag == DataMgr.m_tagNpc)
                {
                    Debug.Log("点击了掌柜");

                    ZhangGuiInfo info = hit.collider.gameObject.GetComponent<ZhangGuiInfo>();

                    //m_camZhangGui.SetActive(true);

                    if (info.m_info.isInOut == 1)
                    {
                        UIManager.Instance.PushPanel(Vc.AbName.jiajushangchengpanel, false, false, (param) =>
                        {
                            jiajushangchengpanel jiaju = param.GetComponent<jiajushangchengpanel>();
                            jiaju.set_type((int)EnJjscType.Diamond);

                        });
                    }
                    else if (info.m_info.isInOut == 0)
                    {
                        UIManager.Instance.PushPanel(Vc.AbName.shoppingmunepanel, false, false, (param) =>
                        {
                            shoppingmunepanel chat = param.GetComponent<shoppingmunepanel>();
                            ShopsProperties shopPro = DataMgr.m_dicShopsProperties[m_shopId];
                            chat.ReqGGLM(shopPro.businessId);
                        }
                        );
                    }
                }
            }
        }
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

        m_dicPlayerInfo.Clear();

        for (int i = m_shopPar.childCount-1; i >= 0; i--)
        {
            PoolMgr.Instance.RecycleObj(m_shopPar.GetChild(i).gameObject);
        }


    }

    public void SetJoystick(VariableJoystick joyTmp,long shopId)
    {
        DataInit();
        m_shopId = shopId;
        m_sceneId = SYJMgr.m_instance.m_sceneId;
        //UIManager.Instance.PushPanel(UIPanelName.playerheadpanel, false, false, (param) => { CreateMySelf(joyTmp); }, true);
        UIManager.Instance.PushPanel(Vc.AbName.hudpanel, false, false, (param) =>
        {
            m_hud = param.GetComponent<hudpanel>();
            //hud.SetCamera(m_myCamera);
            m_hud.ClearAll();
            CreateMySelf(joyTmp);

            CreateZhangGui(shopId);
            MoveCamZhangGui(shopId);
        }, true);
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
    void CreateZhangGui(long shopId)
    {
        ShopsProperties info = DataMgr.m_dicShopsProperties[shopId];
        PosAndAngle posAndAn = JsonConvert.DeserializeObject<PosAndAngle>(info.npcPos);
        Vector3 pos = new Vector3(posAndAn.x, posAndAn.y, posAndAn.z);
        Vector3 angle = new Vector3(posAndAn.dirX, posAndAn.dirY, posAndAn.dirZ);
        AssetMgr.Instance.CreateObj(info.npcModel, info.npcModel, m_zhangGuiPar, pos, angle, Vector3.one, (param) =>
        {
            ZhangGuiInfo infoZhangGui = param.AddComponent<ZhangGuiInfo>();
            infoZhangGui.m_info = info;

            hudpanel.m_instance.CreateOneHeadUi(m_shopId, "掌柜", param.transform);
            ModelVisible mv = param.AddComponent<ModelVisible>();
            mv.m_id = m_shopId.ToString();
            mv.m_cam = m_myCamera;
           
        });
    }


    void RecycleZhangGui()
    {
        for (int i = m_zhangGuiPar.childCount-1; i >= 0; i--)
        {
            PoolMgr.Instance.RecycleObj(m_zhangGuiPar.GetChild(i).gameObject);
        }
    }
    void CreateMySelf(VariableJoystick joyTmp)
    {
        string shopName = DataMgr.m_dicShopsProperties[m_shopId].moduleId;

        //if (shopName == Vc.AbName.shengling)
        //{
            
        //    RenderSettings.fogMode = FogMode.Linear;
        //    RenderSettings.fogStartDistance = 15.9f;
        //    RenderSettings.fogEndDistance = 39.82f;
        //    RenderSettings.fogColor = new Color(38.0f, 48.0f, 94.0f,255);
        //    //RenderSettings.fogColor = new Color(38.0f, 48.0f, 94.0f, 255);
        //    RenderSettings.fog = true;
        //}
        //else {
        //    RenderSettings.fog = false;
        //}
        PosAndAngle posAA = JsonConvert.DeserializeObject<PosAndAngle>(DataMgr.m_dicShopsProperties[m_shopId].bornPos);
        Vector3 posBorn = new Vector3(posAA.x,posAA.y,posAA.z);
        Vector3 angleBorn = new Vector3(posAA.dirX, posAA.dirY, posAA.dirZ);
        //string skybox = "skybox" + shopName;
        //AssetMgr.Instance.CreateMat(skybox, skybox, (mat) => {
        //    m_skyBox.material = mat;
        //});
        AssetMgr.Instance.CreateObj(shopName, shopName, m_shopPar, Vector3.zero, Vector3.zero, Vector3.one, (scene) =>
        {
            string modelName = PublicFunc.GetUserModelName(DataMgr.m_account);

            Transform transCam = scene.transform.Find("PlayerCamera");
            m_camera = transCam.GetComponent<MouseOrbitImproved>();

            m_myCamera = transCam.GetComponent<Camera>();
            m_hud.SetCamera(m_myCamera);
            AssetMgr.Instance.CreateObj(modelName, modelName, m_playerPar, posBorn, angleBorn, Vector3.one, (parma) =>
            {
                m_camera.target = parma.transform.Find("LookAt");
                MoveController moveCtrl = parma.AddComponent<MoveController>();
                moveCtrl.m_camera = m_camera.transform;
                moveCtrl.m_joy = joyTmp;
                m_myMoveCtrl = moveCtrl;
                parma.tag = "Player";
                parma.GetComponent<NavMeshAgent>().enabled = true;
                m_dicPlayerObj[(long)DataMgr.m_account.id] = parma;
                m_myPlayer = parma.transform;

                ReqRunInShopMessage req = new ReqRunInShopMessage();
                PlayerStatus playerSta = new PlayerStatus();
                playerSta.x = parma.transform.position.x;
                playerSta.y = parma.transform.position.y;
                playerSta.z = parma.transform.position.z;

                playerSta.px = parma.transform.eulerAngles.x;
                playerSta.py = parma.transform.eulerAngles.y;
                playerSta.pz = parma.transform.eulerAngles.z;
                req.playerStatus = playerSta;
                req.shopId = m_shopId;
                req.scenceId = m_sceneId;
                //PublicFunc.ToGameServer(MsgIdDefine.ReqRunInShopMessage, req);
                RoomSocket.Instance.SendMsgProto(MsgIdDefine.ReqRunInShopMessage, req);
                m_posLast = parma.transform.position;
                Transform paramPar = parma.transform.Find("LookAt");
                hudpanel.m_instance.CreateOneHeadUi((long)DataMgr.m_account.id, DataMgr.m_account.userName, parma.transform);
                //playerheadpanel.m_instance.CreateOneHeadUi((long)DataMgr.m_account.id, DataMgr.m_account.userName, parma.transform.Find("LookAt"));

                //scene.SetActive(false);
                //scene.SetActive(true);

            });

        });
        
    }

    void SendReqSyncStreetMessage()
    {
        if ((m_posLast - m_myPlayer.transform.position).sqrMagnitude <= 0.1f)
        {
            return;
        }

        ReqSyncShopMessage req = new ReqSyncShopMessage();
        req.streetScenceId = m_sceneId;
        req.shopId = m_shopId;
        PlayerStatus playerSta = new PlayerStatus();

        playerSta.x = m_myPlayer.position.x;
        playerSta.y = m_myPlayer.position.y;
        playerSta.z = m_myPlayer.position.z;
        playerSta.px = m_myPlayer.eulerAngles.x;
        playerSta.py = m_myPlayer.eulerAngles.y;
        playerSta.pz = m_myPlayer.eulerAngles.z;
        req.playerStatus = playerSta;
        //PublicFunc.ToGameServer(MsgIdDefine.ReqSyncShopMessage, req);
        RoomSocket.Instance.SendMsgProto(MsgIdDefine.ReqSyncShopMessage, req);
        m_posLast = m_myPlayer.transform.position;
    }

    public void SendReqRunOutShopMessage(bool isToStreet = true)
    {
        ReqRunOutShopMessage req = new ReqRunOutShopMessage();
        if (isToStreet == true)
        {
            req.playerStatus = new PlayerStatus();
            req.playerStatus.x = 10;
            req.playerStatus.y = 0;
            req.playerStatus.z = -10;
            req.playerStatus.px = 0;
            req.playerStatus.py = 180;
            req.playerStatus.pz = 0;
        }
        else
        {
            req.playerStatus = null;
        }
        req.shopId = m_shopId;
        req.streetScenceId = m_sceneId;
        RoomSocket.Instance.SendMsgProto(MsgIdDefine.ReqRunOutShopMessage, req);

    }
}
