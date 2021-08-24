using Framework.UI;
using ProtoDefine;
using SGF.Codec;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cn.SMSSDK.Unity;

public class EditTelephone : MonoBehaviour
{
    public InputField OriTelephone;
    public InputField NewTelephone;
    public InputField VerificationCode;
    public InputField Password;
    public GameObject ConfrimBtn;
    public GameObject GetVerificationCodeBtn;
    int JiShi = 60;
    private string result = null;
    // Use this for initialization
    void Start() {
        ClickListener.Get(ConfrimBtn).onClick = clickConfrimBtn;
        ClickListener.Get(GetVerificationCodeBtn).onClick = clickGetVC;
        SmssMgr.Instance.m_act = () => { ConfrimTelephone(OriTelephone.text, NewTelephone.text, Password.text); };
        SmssMgr.Instance.m_failureAct = () => {Hint.LoadTips("验证码错误",Color.white); };
    }
    void clickGetVC(GameObject obj)
    {
        SmssMgr.Instance.GetCode(DataMgr.m_account.phone);
        //obj.transform.
        JiShi = 60;
        InvokeRepeating("DaoJiShi",0,1);
        obj.GetComponent<Button>().enabled=false;
    }
    void DaoJiShi()
    {
        JiShi--;
        GetVerificationCodeBtn.transform.Find("Text").GetComponent<Text>().text = JiShi.ToString()+"s";
        if (JiShi == 0)
        {
            GetVerificationCodeBtn.transform.Find("Text").GetComponent<Text>().text = "获取验证码";
            CancelInvoke("DaoJiShi");
            GetVerificationCodeBtn.GetComponent<Button>().enabled = true;
        }
    }
    void clickConfrimBtn(GameObject obj)
    {
#if UNITY_EDITOR
        ConfrimTelephone(OriTelephone.text, NewTelephone.text, Password.text);
        Debug.Log("验证短信");
#elif UNITY_ANDROID
        SmssMgr.Instance.CommitCode(VerificationCode.text);
#endif
    }
    void ConfrimTelephone(string OriTelephone,string NewTelephone,string Password)
    {
        if (OriTelephone == DataMgr.m_account.phone)
        {
                ReqUpdateUserInfoMessage ReqUUIM = new ReqUpdateUserInfoMessage();
                ReqUUIM.accountId = DataMgr.m_account.id;
                ReqUUIM.info = new List<UserInfoMap>();
                UserInfoMap m_UserInfoMap = new UserInfoMap();
                m_UserInfoMap.infoType = 603;
                m_UserInfoMap.info = NewTelephone;
                ReqUUIM.info.Add(m_UserInfoMap);
                HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdateUserInfoMessage, ReqUUIM);
        }
    }
    private void OnEnable()
    {
        Init();

        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspUpdateUserInfoMessage, OnEvNetUpdateInfo);
    }
    private void OnDisable()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspUpdateUserInfoMessage, OnEvNetUpdateInfo);
    }
    void OnEvNetUpdateInfo(byte[] buf)
    {
        RspUpdateUserInfoMessage RspUUIM = PBSerializer.NDeserialize<RspUpdateUserInfoMessage>(buf);
        if (RspUUIM.code != 0)
        {
            DataMgr.m_account.phone = RspUUIM.userInfoMap[0].info;
            Init();
            Hint.LoadTips("修改成功",Color.white);
         }
        else if (RspUUIM.code == 0)
        {
            Hint.LoadTips(RspUUIM.tips,Color.white);
            //Debug.Log(RspUUIM.tips);

        }
    }
    public void Init()
    {
        OriTelephone.text = "";
        NewTelephone.text = "";
        VerificationCode.text = "";
        Password.text = "";
        GetVerificationCodeBtn.transform.Find("Text").GetComponent<Text>().text = "获取验证码";
        CancelInvoke("DaoJiShi");
        GetVerificationCodeBtn.GetComponent<Button>().enabled = true;
    }
    // Update is called once per frame
}
