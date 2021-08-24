using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using UnityEngine.UI;
using LitJson;
using DG.Tweening;
using UnityEngine.EventSystems;
using ProtoDefine;
using Net;
using Newtonsoft.Json;
using SGF.Codec;

public class createcharacterpanel : UGUIPanel {
    public GameObject ModelShowPar;
    public GameObject SelectManBtn;
    public GameObject SelectWomanBtn;
    public GameObject DragModelObj;
    GameObject SelectHeadImageObj;
    public Text CharacterNameText;
    public string CharacterName;
    public string ModelName;
    LoopVerticalScrollRect lvsr;
    public GameObject HeadImage;
    public GameObject ManHeadImagePar;
    public GameObject WoManHeadImagePar;
    public List<Character> characterl = new List<Character>();
    public static createcharacterpanel ccp;
    float m_xangles = 0.0f;
    int BtnSex=-1;
    bool IsInit = false;
    public InputField IF;
    // Use this for initialization
    private void Awake()
    {
        
        Inittext();
        EventTriggerListener.Get(DragModelObj).onDrag = dragmodel;
        IF.onEndEdit.AddListener(delegate { EndInput(IF); });
    }

    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspUpdateUserInfoMessage, OnNetEvUpdateUserInfoRsp);
    }

    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspUpdateUserInfoMessage, OnNetEvUpdateUserInfoRsp);
    }

    void OnNetEvUpdateUserInfoRsp(byte[] buf)
    {
        RspUpdateUserInfoMessage ret = PBSerializer.NDeserialize<RspUpdateUserInfoMessage>(buf);
        if (ret.code == 0)
        {
            if (ret.tips !=null)
            {
                Hint.LoadTips(ret.tips, Color.white);
            }
        }
        else if (ret.code == 1)
        {
            Debug.Log("创建成功");
            DataMgr.m_isLoginOk = true;
            //todo 服务器传回更改的字段
            if (ret.userInfoMap != null)
            {
                for (int i = 0; i < ret.userInfoMap.Count; i++)
                {
                    var info = ret.userInfoMap[i];
                    switch ((short)info.infoType)
                    {
                        case SystemDataPool.USERNAME:
                            DataMgr.m_account.userName = info.info;
                            break;
                        case SystemDataPool.MOUDLEID:
                            DataMgr.m_account.modleId = long.Parse(info.info);
                            break;
                        case SystemDataPool.SEX:
                            DataMgr.m_account.sex = int.Parse(info.info);
                            break;
                        default:
                            break;
                    }
                }
            }
            //VirtualCityMgr.m_instance.CameraSetShyBox(true);

            ReqChatLoginMessage chatLogin = new ReqChatLoginMessage();
            chatLogin.chatUser = new ChatUser();
            chatLogin.chatUser.accountId = (long)DataMgr.m_account.id ;
            chatLogin.chatUser.modelId = (long)DataMgr.m_account.modleId;
            chatLogin.chatUser.userName = DataMgr.m_account.userName;
            chatLogin.chatUser.income = (double)DataMgr.m_account.wallet.mIncome;
            chatLogin.chatUser.serverIp = DataMgr.m_account.serverIp;
         
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

            ChatSocket.Instance.Connect(AppConst.m_newChatIp, AppConst.m_newChatPort);
            DataMgr.m_ReqChatLoginMessage = chatLogin;
            //ReqGetPorxyUserMessage m_ReqGPUM = new ReqGetPorxyUserMessage();
            //m_ReqGPUM.accountId = (long)DataMgr.m_account.id;
            ChatSocket.Instance.m_onConnectOk = () =>
            {
               // ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetPorxyUserMessage, m_ReqGPUM);
                ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqChatLoginMessage, DataMgr.m_ReqChatLoginMessage);
            };

            //UIManager.Instance.PushPanelDeleteSelf(UIPanelName.homepanel);       
            VirtualCityMgr.GotoHometown(EnMyOhter.My);
            //VirtualCityMgr.GotoShanYeJie();
        }
    }
    private void Inittext()
    {
        // CL = JsonMapper.ToObject<CharacterList>(text);
         ccp = this;
        SelectSex(1);
    }
    void EndInput(InputField IF)
    {
        CharacterName = CharacterNameText.text;
    }
    void init()
    {
        foreach (var sex in DataMgr.m_dicRoleProperties)
        {
            if (sex.Value.Sex == 0)
            {
                GameObject obj = PublicFunc.CreateTmp(HeadImage, WoManHeadImagePar.transform);
                obj.name = sex.Value.Id.ToString();
                Tween toscale = DOTween.To(() => obj.transform.localScale, r => obj.transform.localScale = r, Vector3.one, 0.5f);
                AssetMgr.Instance.CreateSpr(sex.Value.Icon, "charactericon", (sprite) => { obj.transform.GetChild(2).GetComponent<Image>().sprite = sprite; });
                obj.tag = "Woman";
                ClickListener.Get(obj).onClick = selectHeadImage;
            }
            else if (sex.Value.Sex == 1)
            {
                GameObject obj = PublicFunc.CreateTmp(HeadImage, ManHeadImagePar.transform);
                obj.name = sex.Value.Id.ToString();
                Tween toscale = DOTween.To(() => obj.transform.localScale, r => obj.transform.localScale = r, Vector3.one, 0.5f);
                AssetMgr.Instance.CreateSpr(sex.Value.Icon, "charactericon", (sprite) => { obj.transform.GetChild(2).GetComponent<Image>().sprite = sprite; });
                obj.tag = "Man";
                ClickListener.Get(obj).onClick = selectHeadImage;
            }
        }
        //for (int i = 0; i < CL.dic.Count; i++)
        //{
        //    if (CL.dic[i].Sex == 0)
        //    {
        //        GameObject obj = PublicFunc.CreateTmp(HeadImage, WoManHeadImagePar.transform);
        //        obj.name = i.ToString();
        //        Tween toscale = DOTween.To(() => obj.transform.localScale, r => obj.transform.localScale = r, Vector3.one, 0.5f);
        //        AssetMgr.Instance.CreateSpr(CL.dic[i].Name, "charactericon", (sprite) => { obj.transform.GetChild(2).GetComponent<Image>().sprite = sprite; });
        //        obj.tag = "Woman";
        //        ClickListener.Get(obj).onClick = selectHeadImage;
        //    }
        //    else if (CL.dic[i].Sex == 1)
        //    {
        //        GameObject obj = PublicFunc.CreateTmp(HeadImage, ManHeadImagePar.transform);
        //        obj.name = i.ToString();
        //        Tween toscale = DOTween.To(() => obj.transform.localScale, r => obj.transform.localScale = r, Vector3.one, 0.5f);
        //        AssetMgr.Instance.CreateSpr(CL.dic[i].Name, "charactericon", (sprite) => { obj.transform.GetChild(2).GetComponent<Image>().sprite = sprite; });
        //        obj.tag = "Man";
        //        ClickListener.Get(obj).onClick = selectHeadImage;
        //    }
        //}
        IsInit = true;
    }
    public void SelectSex(int sex)
    {
        if (BtnSex != sex)
        {
            if (IsInit == false)
            {
                init();
            }
            if (sex == 0)
            {
                SelectManBtn.GetComponent<Image>().enabled = false;
                SelectWomanBtn.GetComponent<Image>().enabled = true;
                WoManHeadImagePar.transform.parent.parent.gameObject.SetActive(true);
                ManHeadImagePar.transform.parent.parent.gameObject.SetActive(false);
                selectHeadImage(WoManHeadImagePar.transform.GetChild(0).gameObject);
            }
            else if (sex == 1)
            {
                SelectWomanBtn.GetComponent<Image>().enabled = false;
                SelectManBtn.GetComponent<Image>().enabled = true;
                WoManHeadImagePar.transform.parent.parent.gameObject.SetActive(false);
                ManHeadImagePar.transform.parent.parent.gameObject.SetActive(true);
                selectHeadImage(ManHeadImagePar.transform.GetChild(0).gameObject);
            }
            BtnSex = sex;
        }
    }

    float m_Rotate = 10.0f;
    public void dragmodel(GameObject obj)
    {

        //if (Input.touchCount == 1)
        //{
        //    if (Input.GetTouch(0).phase == TouchPhase.Moved)
        //    {
        //        m_xangles = Input.GetAxis("Mouse X") * m_Rotate;
        //        ModelShowPar.transform.GetChild(0).Rotate(0, -m_xangles*2, 0);
        //    }
        //}
        if (Input.GetMouseButton(0))
        {
            m_xangles = Input.GetAxis("Mouse X") * m_Rotate;
            ModelShowPar.transform.GetChild(0).Rotate(0, -m_xangles, 0);
        }
    }
    public void CreateCharacter()
    {
        if (CharacterNameText.text == "")
        {
            Hint.LoadTips("名字不能为空", Color.white);
            return;
        }

        UserInfoMap item = new UserInfoMap();
        item.infoType = SystemDataPool.USERNAME;
        PublicFunc.GetLegalString(CharacterNameText.text);
        item.info = CharacterNameText.text;
        UpdateUserInfoMessage list = new UpdateUserInfoMessage();
        list.info.Add(item);
        UserInfoMap item1 = new UserInfoMap();
        item1.infoType = SystemDataPool.SEX;
        item1.info = BtnSex.ToString();
        list.info.Add(item1);
        UserInfoMap item2 = new UserInfoMap();
        item2.infoType = SystemDataPool.MOUDLEID;
        item2.info = ModelName;
        list.info.Add(item2);

        list.accountId = (long)DataMgr.m_account.id;
        GameSocket.Instance.SendMsgProto(MsgIdDefine.UpdateUserInfoReq, list);
    }
    public void selectHeadImage(GameObject obj)
    {
        uiloadpanel.Instance.Open();
        PublicFunc.RemoveFromChild(ModelShowPar.transform);
        SelectHeadImageObj = obj;
        GameObject HeadImageTmp = obj.transform.parent.gameObject;
        CharacterName = CharacterNameText.text;
        for (int i = 0; i < HeadImageTmp.transform.childCount; i++)
        {
            obj.transform.parent.GetChild(i).Find("BianKuang").gameObject.SetActive(false);
            if (HeadImageTmp.transform.GetChild(i).name == obj.name)
            {
                ModelName = obj.name;
            }
        }
        obj.transform.Find("BianKuang").gameObject.SetActive(true);

        string modeName = DataMgr.m_dicRoleProperties[long.Parse(ModelName)].ModelDate;

        AssetMgr.Instance.CreateObj(modeName, modeName, ModelShowPar.transform, new Vector3(0,-433,0), new Vector3(0,180,0), new Vector3(400, 400, 400), (charactermodel) => {
            foreach (var mesh in charactermodel.transform.GetComponentsInChildren<Renderer>())
            {
                mesh.gameObject.layer = 9;
                uiloadpanel.Instance.Close();
            }
                
                });
        //foreach (var id in DataMgr.m_dicRoleProperties)
        //{
        //    if (id.Value.Id == long.Parse(ModelName))
        //    {
                
        //    }
        //}
        //AssetMgr.Instance.CreateObj(ModelName, ModelName, ModelShowPar.transform, Vector3.zero, Vector3.zero, new Vector3(100, 100, 100), (charactermodel) => { charactermodel.layer = 9; });
    }
}
