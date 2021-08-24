using cn.SMSSDK.Unity;
using Framework.Event;
using Framework.UI;
using ProtoDefine;
using SGF.Codec;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class RegisterPart : UGUIPanel
{
    //public SMSSDK smssdk;
    public GameObject m_btnZhuCe;
    public InputField m_input;
    public GameObject m_btnClear;
    public GameObject m_phonePart;
    public GameObject m_yzmPart;
    
    public GameObject m_daoJiShi;
    public GameObject m_btnResend;
    public GameObject m_replace;
    internal byte[] RsaEncrypt_(string v, string privatekey)
    {
        throw new NotImplementedException();
    }

    public InputField m_inputYzm;
    public Transform m_linePar;
    int m_daoJiShiCnt = 60;
    public Text m_textDaoJiShi;

    //please add your phone number
    private string phone = "";
    private string zone = "86";
    private string tempCode = "1319972";
    private string code = "";
    private string result = null;

    public GameObject m_btnReturn;
    public Text m_textPhone;
    //public GameObject m_btnCommit;
    // Use this for initialization
    void Start () {
        //smssdk = gameObject.GetComponent<SMSSDK>();
        //smssdk.init("moba6b6c6d6", "b89d2427a3bc7ad1aea1e1e8c1d36bf3", true);
        //smssdk.setHandler(this);

        ClickListener.Get(m_btnZhuCe).onClick = OnBtnZhuCu;
        ClickListener.Get(m_btnClear).onClick = OnBtnClear;
        ClickListener.Get(m_btnResend).onClick = OnBtnReSend;
        ClickListener.Get(m_btnReturn).onClick = OnBtnReturn;
        ClickListener.Get(m_replace).onClick = OnBtnreplace;

        //ClickListener.Get(m_btnCommit).onClick = CommitCode;
        m_inputYzm.caretWidth = 0;
        m_inputYzm.onValueChanged.AddListener((param)=> { OnInputYzmChange(param); });
        Debug.Log("smssdk OK");
        SmssMgr.Instance.m_act = () => { OKGoToNext(phone); };
    }


    public override void OnOpen()
    {
        UiInit();
        //smssdk.init("moba6b6c6d6", "b89d2427a3bc7ad1aea1e1e8c1d36bf3", true);
        //smssdk.setHandler(this);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspLoginMessage, OnEvNetLogin);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, OnNetRspCommentMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspLoadPlayerMessage, OnEvNetLoadPlayer);
    }

    void UiInit()
    {
        m_phonePart.SetActive(true);
        m_yzmPart.SetActive(false);
        OnWhenYzmAtice();
    }
    void OnEvNetLoadPlayer(byte[] buf)
    {
        RspLoadPlayerMessage ret = PBSerializer.NDeserialize<RspLoadPlayerMessage>(buf);
       
        if (ret.code == 0)
        {
            Hint.LoadTips(ret.tip, Color.white);
        }
        else {

            VirtualCityMgr.OnEvNetLoadPlayer(ret);

            Debug.Log("通过验证");
            inputonepasswordpanel.Type = 1;
            if (ret.account.password == null)
            {
                UIManager.Instance.PushPanelDeleteSelf(Vc.AbName.passwordpanel, true);
            }
            else if (DataMgr.m_account.password == "*" && !File.Exists(AppConst.LocalPath + "/Rsa.txt"))
            {
                UIManager.Instance.PushPanelDeleteSelf(Vc.AbName.inputonepasswordpanel, true);
            }
            else if (DataMgr.m_account.password == "*" && File.Exists(AppConst.LocalPath + "/Rsa.txt"))
            {
                EventManager.Instance.DispatchEvent(Common.EventStr.OpenLoginPart);
                UIManager.Instance.PopSelf(false);
            }
        }
    }
    void OnEvNetLogin(byte[] buf)
    {
        RspLoginMessage pro = PBSerializer.NDeserialize<RspLoginMessage>(buf);
        if (pro.code == 0)
        {
            Hint.LoadTips(pro.tips, Color.white);
            //return;
        }

        if (pro.isOnline == 1)
        {
            HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqLoginMessage, LoginPanel.m_login);
            return;
        }
        VirtualCityMgr.OnLoginHallOk(pro);
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, OnNetRspCommentMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspLoginMessage, OnEvNetLogin);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspLoadPlayerMessage, OnEvNetLoadPlayer);
    }

    void OnNetRspCommentMessage(byte[] buf)
    {
        //RspCommentMessage RspCM = PBSerializer.NDeserialize<RspCommentMessage>(buf);
        //if (RspCM.code == LoginDataPool.LOGIN_FAIL)
        //{
        //    HallSocket.Instance.SendMsgProto(MsgIdDefine.LoginReq, m_login);
        //}
    }

    void OnBtnReturn(GameObject obj)
    {
        CancelInvoke("OnDaoJiShi");
        //m_daoJiShi.SetActive(false);
        //m_btnResend.SetActive(true);
        m_phonePart.SetActive(true);
        m_yzmPart.SetActive(false);
    }
    void GetCode()
    {
        if (Application.isMobilePlatform)
            SmssMgr.Instance.GetCode(phone);
        //smssdk.getCode (CodeType.TextCode, phone, zone, tempCode);
    }

    void CommitCode(string commitCode)
    {
        Debug.Log("提交验证码:" + phone + ":" + zone + ":" + commitCode);
        //smssdk.commitCode(phone, zone, commitCode);
        SmssMgr.Instance.CommitCode(commitCode);

    }

    void OnBtnReSend(GameObject obj)
    {
        Hint.LoadTips("已发送验证短信", Color.white);
        GetCode();
        m_daoJiShi.SetActive(true);
        m_btnResend.SetActive(false);

        m_daoJiShiCnt = 60;
        InvokeRepeating("OnDaoJiShi", 0, 1);
    }

    void OnBtnreplace(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.kefupanel, false, false, (param) => { });
    }


    void OnBtnZhuCu(GameObject obj)
    {
        bool isPhone = true;
        if (Application.isMobilePlatform && AppConst.m_enTestServer == EnTestServer.Out)
        {
            isPhone = PublicFunc.IsPhoneOK(m_input.text);
        }

        if (AppConst.m_isDebug == true)
        {
            isPhone = true;
        }
        //判断手机号是否违法
        if (isPhone)
        { 
            phone = m_input.text;
            GetCode();
            Debug.Log("手机号正确");
            Hint.LoadTips("已发送验证短信", Color.white);
            //todo 发送验证码

            //进入第二个输入激活码界面
            m_phonePart.SetActive(false);
            m_yzmPart.SetActive(true);
            OnWhenYzmAtice();
        }
        else {
            m_input.text = "";
            Hint.LoadTips("手机号不正确", Color.white);
        }
    }

    void OnWhenYzmAtice()
    {
        m_textPhone.text = "发送至" + phone;
        m_inputYzm.text = "";
        for (int i = 0; i < m_linePar.childCount; i++)
        {
            m_linePar.GetChild(i).gameObject.SetActive(true);
        }

        m_daoJiShi.SetActive(true);
        m_btnResend.SetActive(false);

        //if (DataMgr.m_yzmDjs > 0)
        //{
        //    m_daoJiShi.SetActive(true);
        //    m_btnResend.SetActive(false);
        //}
        //else if (DataMgr.m_yzmDjs <= 0)
        //{
        //    m_daoJiShi.SetActive(true);
        //    m_btnResend.SetActive(false);
        //}
        m_daoJiShiCnt = 60;
        InvokeRepeating("OnDaoJiShi", 0, 1);
    }

    void OnDaoJiShi()
    {
        m_daoJiShiCnt--;
        m_textDaoJiShi.text = m_daoJiShiCnt.ToString() + "s";
        if (m_daoJiShiCnt == 0)
        {
            m_daoJiShi.SetActive(false);
            m_btnResend.SetActive(true);
            CancelInvoke("OnDaoJiShi");
        }
        
    }
    void OnBtnClear(GameObject obj)
    {
        m_input.text = "";
    }

    void OnInputYzmChange(string text)
    {

        for (int i = 0; i < 4; i++)
        {
            if (i < text.Length)
            {
                m_linePar.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                m_linePar.GetChild(i).gameObject.SetActive(true);
            }
        }

        if (text.Length == 4)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            {
                OKGoToNext(phone);
            }
            else {
                if (AppConst.m_isDebug == true)
                {
                    OKGoToNext(phone);
                }
                else
                {
                    if (AppConst.m_enTestServer == EnTestServer.Out)
                    {
                        CommitCode(text);
                    }
                    else {
                        OKGoToNext(phone);
                    }
                }
            }
        }
    }


    void OKGoToNext(string phone)
    {
        //NetManager.Instance.SendConnect(AppConst.m_ip, AppConst.m_port);
        ReqLoginMessage login = new ReqLoginMessage();
        login.accountId = null;
        login.phone = phone;


        LoginPanel.m_login.accountId = null;
        LoginPanel.m_login.phone = phone;
        DataMgr.m_isNewHaveLoginInfo = true;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqLoginMessage, login,EnSocket.Hall);

       
    }
    public void onComplete(int action, object resp)
    {
        Debug.Log("得到回调");
        ActionType act = (ActionType)action;
        if (resp != null)
        {
            result = resp.ToString();
        }
        if (act == ActionType.GetCode)
        {
            string responseString = (string)resp;
            Debug.Log("isSmart :" + responseString);
        }
        else if (act == ActionType.GetVersion)
        {
            string version = (string)resp;
            Debug.Log("version :" + version);
            print("Demo*version*********" + version);

        }
        else if (act == ActionType.GetSupportedCountries)
        {

            string responseString = (string)resp;
            Debug.Log("zoneString :" + responseString);

        }
        else if (act == ActionType.GetFriends)
        {
            string responseString = (string)resp;
            Debug.Log("friendsString :" + responseString);

        }
        else if (act == ActionType.CommitCode)
        {

            string responseString = (string)resp;
            Debug.Log("commitCodeString :" + responseString);
            Debug.Log("手机验证码OK,向服务器请求");

            OKGoToNext(phone);

        }
        else if (act == ActionType.SubmitUserInfo)
        {

            string responseString = (string)resp;
            Debug.Log("submitString :" + responseString);

        }
        else if (act == ActionType.ShowRegisterView)
        {

            string responseString = (string)resp;
            Debug.Log("showRegisterView :" + responseString);

        }
        else if (act == ActionType.ShowContractFriendsView)
        {

            string responseString = (string)resp;
            Debug.Log("showContractFriendsView :" + responseString);
        }
    }

    public void onError(int action, object resp)
    {
        Debug.Log("Error :" + resp);
        result = resp.ToString();
        print("OnError ******resp" + resp);
        Hint.LoadTips("手机验证码错误", Color.white);
    }
    
}
