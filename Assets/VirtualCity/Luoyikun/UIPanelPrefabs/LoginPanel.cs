using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Framework.Event;
using SGF.Codec;
using ProtoDefine;
using Net;
using UnityEngine.UI;
using System;
using Framework.UI;
using System.IO;
using UnityEditor;
using Newtonsoft.Json;

[System.Serializable]
public class LocalLoginInfo
{
    public long uid;
    public string deviceId;
}

public class LoginPanel : UGUIPanel {

    public GameObject m_btnKefu;

    public Image m_imgDian;
    public Text m_textState;
    public Text m_textServerName;

    Color m_colorBaoMan;
    Color m_colorFanMang;
    Color m_colorLiuChang;
    Color m_colorWeiHu;

    public GameObject m_loginPart;
    public GameObject m_btnLogin;
    public static long m_id;

    public GameObject m_btnNotice;
    bool m_isNew = false;

    public static ReqLoginMessage m_login = new ReqLoginMessage();
    public GameObject m_btnSa;
    public GameObject m_loadingPar;
    // Use this for initialization
    void Start () {

        m_colorBaoMan = PublicFunc.StringToColor("ff3219");
        m_colorFanMang = PublicFunc.StringToColor("f0a42b");
        m_colorLiuChang = PublicFunc.StringToColor("3f9d12");
        m_colorWeiHu = PublicFunc.StringToColor("8c8c8c");

        


        //ReqLogin login = new ReqLogin();
        //login.accountId = 0;
        //login.hadLogin = 0;
        //login.phone = "17770813514";

        //NetManager.Instance.SendMsgProte("101,1", login);


        ClickListener.Get(m_btnKefu).onClick = OnBtnKeFu;
        ClickListener.Get(m_btnLogin).onClick = OnBtnLogin;
        ClickListener.Get(m_btnNotice).onClick = OnBtnNotice;
        ClickListener.Get(m_btnSa).onClick = OnBtnSa;
    }

    void OnBtnSa(GameObject obj)
    {
        VirtualCityMgr.SwitchAccount();
    }
    void OnBtnNotice(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.noticepanel, false, false,(param)=>
        {
            NoticePanel notice = param.GetComponent<NoticePanel>();
            notice.ShowNotice();
        },true
        );
    }

    void OnBtnLogin(GameObject obj)
    {
        //VirtualCityMgr.EnterGame();  
        if (DataMgr.m_account == null)
        {
            //uiloadpanel.Instance.Open();
            //Hint.LoadTips("本地登陆信息有误，请尝试切换账号");
            return;
        }

        if (DataMgr.m_account.userName == null)
        {
            UIManager.Instance.PushPanelDeleteSelf(UIPanelName.createcharacterpanel);
        }
        else
        {
            DataMgr.m_isLoginOk = true;
            VirtualCityMgr.GotoHometown(EnMyOhter.My);
            //VirtualCityMgr.GotoShanYeJie();
        }

    }


    void OnNetRspLogoutHallMessage(byte[] buf)
    {
        //if (m_isNew == true)
        //{
        //    return;
        //}
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqLoginMessage, m_login);
    }

    public override void OnOpen()
    {
        DataMgr.m_isLoginOk = false;
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspLogoutHallMessage, OnNetRspLogoutHallMessage);
        //NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, OnNetRspCommentMessage);

        EventManager.Instance.AddEventListener(Common.EventStr.OpenLoginPart, OnEvOpenLoginPart);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspLoginMessage, OnEvNetLogin);

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspLoadPlayerMessage, OnEvNetLoadPlayer);

        //NetEventManager.Instance.AddEventListener(MsgIdDefine.UpdateUserInfoRsp, )
        bool isExist = PublicFunc.IsFileExist(AppConst.LocalPath + "/login.mi");
        //return;
        if (isExist == true)
        {
            //UIManager.Instance.PushPanel(UIPanelName.uiloadpanel,false,false,null,true);
            //uiloadpanel.Instance.Open();
            m_loadingPar.SetActive(true);
            m_isNew = false;
            m_loginPart.SetActive(true);
            string sContent = JsonMgr.GetJsonString(AppConst.LocalPath + "/login.mi");
            LocalLoginInfo info = JsonUtility.FromJson<LocalLoginInfo>(sContent);
            
            //LocalLoginInfo info = JsonMapper.ToObject<LocalLoginInfo>(sContent);
            if (info.deviceId == SystemInfo.deviceUniqueIdentifier)
            {
                //DataMgr.m_phone = info.phone;
                m_id = info.uid;

               

                m_login.accountId = m_id;
                m_login.phone = "0";

                if (File.Exists(AppConst.LocalPath + "/Rsa.txt") == false)
                {
                    //Hint.LoadTips("客户端已损坏，请卸载重装", Color.white);
                    ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, false);
                    ispanel.SetContent("提示", "客户端已损坏，请先卸载再重装",false);
                    ispanel.m_ok = () =>
                    {
                        Debug.Log("取消");
#if UNITY_EDITOR
                        EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                    };


                        return;
                }
                else
                {
                    string rsaKey = JsonMgr.GetJsonString(AppConst.LocalPath + "/Rsa.txt");
                    byte[] buf = RSAEncryption.RsaEncrypt_(m_login.accountId.ToString(), rsaKey);
                    m_login.content = buf;
                }

                HttpMgr.Instance.Httppost(AppConst.GateHttp, OnHttpHavaUid);




            }
        }
        else {
            HttpMgr.Instance.Httppost(AppConst.GateHttp, OnHttpNewUser);


        }
    }

    void OnHttpNewUser(string text)
    {
        Debug.Log(text);
        HttpJson hj = JsonConvert.DeserializeObject<HttpJson>(text);

        if (hj.code == "1")
        {

            HttpServerInfo data = JsonConvert.DeserializeObject<HttpServerInfo>(hj.message);
            if (data.hallServerInfo != null)
            {
                string[] hall = data.hallServerInfo.Split(':');
                HallSocket.Instance.Connect(hall[0], int.Parse(hall[1]));
                HallSocket.Instance.m_onConnectOk = () =>
                {

                    if (DataMgr.m_isNewHaveLoginInfo == true)
                    {
                        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqLoginMessage, m_login);
                        //GameSocket.Instance.m_isBreakLineReconnection = false;
                    }
                };

                m_isNew = true;
                UIManager.Instance.PushPanel(UIPanelName.registerpanel, false, true, null, true);
            }

            if (data.chatServerInfo != null)
            {
                string[] chat = data.chatServerInfo.Split(':');

                AppConst.m_newChatIp = chat[0];
                AppConst.m_newChatPort = int.Parse(chat[1]);
            }

            
        }
    }


    void OnHttpHavaUid(string text)
    {
        HttpJson hj = JsonConvert.DeserializeObject<HttpJson>(text);

        if (hj.code == "1")
        {
            HttpServerInfo data = JsonConvert.DeserializeObject<HttpServerInfo>(hj.message);
            string[] hall = data.hallServerInfo.Split(':');
            string[] chat = data.chatServerInfo.Split(':');

            AppConst.m_newChatIp = chat[0];
            AppConst.m_newChatPort = int.Parse(chat[1]);

            HallSocket.Instance.Connect(hall[0], int.Parse(hall[1]));

            HallSocket.Instance.m_onConnectOk = () =>
            {
                HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqLoginMessage, m_login);
            };
        }
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspLogoutHallMessage, OnNetRspLogoutHallMessage);

        EventManager.Instance.RemoveEventListener(Common.EventStr.OpenLoginPart, OnEvOpenLoginPart);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspLoginMessage, OnEvNetLogin);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspLoadPlayerMessage, OnEvNetLoadPlayer);
    }
    void OnBtnKeFu(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.kefupanel, false, true,null,true);
    }

    void OnEvOpenLoginPart(EventData data)
    {
        m_loginPart.SetActive(true);
    }


    void OnEvNetUpdateUsrInfo(byte[] buf)
    {
        Debug.Log("OnEvNetUpdateUsrInfo");


    }
    void OnEvNetLogin(byte[] buf)
    {
        //未登录过
        if (m_isNew == true)
        {
            Debug.Log("m_isNew == true");
            return;
        }
        
        RspLoginMessage pro = PBSerializer.NDeserialize<RspLoginMessage>(buf);

        if (pro.code == 0)
        {
            Debug.Log("pro.code == 0");
            Hint.LoadTips(pro.tips, Color.white);

            //return;

            if (pro.isOnline == 1)
            {
                HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqLoginMessage, LoginPanel.m_login);
                return;
            }

            if (pro.tips == "密钥错误")
            {
                ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, false);
                ispanel.SetContent("提示", "此账号在其他设备登陆过，请重新登陆", false);
                ispanel.m_ok = () =>
                {
                    VirtualCityMgr.SwitchAccount();
                };

                return;
            }
        }



        Debug.Log("得到登陆回应");
        VirtualCityMgr.OnLoginHallOk(pro);
    }

    void OnEvNetLoadPlayer(byte[] buf)
    {
        //未登录过
        if (m_isNew == true)
        {
            return;
        }



        RspLoadPlayerMessage ret = PBSerializer.NDeserialize<RspLoadPlayerMessage>(buf);
        if (ret.code == 0)
        {
            Debug.Log("LoadPlayer 失败:" + ret.tip);
            Hint.LoadTips(ret.tip, Color.white);
        }
        else
        {

            VirtualCityMgr.OnEvNetLoadPlayer(ret,false);

        }

        StartCoroutine(YieldCloseUiLoadPanel());
    }

    IEnumerator YieldCloseUiLoadPanel()
    {
        while (!(VirtualCityMgr.m_isGetGameData == true && VirtualCityMgr.m_isGetChatData == true))
        {
            //uiloadpanel.Instance.Open();
            yield return null;
        }

        //if (UIManager.Instance.IsTopPanel(UIPanelName.uiloadpanel))
        //{
        //    UIManager.Instance.PopSelf(false);
        //}
        //uiloadpanel.Instance.Close();
        m_loadingPar.SetActive(false);
    }


}
