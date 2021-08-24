using Framework.UI;
using Newtonsoft.Json;
using ProtoDefine;
using SGF.Codec;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditPassword : MonoBehaviour {
    public InputField OriPassword;
    public InputField NewPassword;
    public InputField CfmPassword;
    public GameObject ConfrimBtn;
    public GameObject KeFuBtn;
    public GameObject ResetBtn;
    // Use this for initialization
    void Start () {
        ClickListener.Get(ConfrimBtn).onClick = clickConfrimBtn;
        ClickListener.Get(KeFuBtn).onClick = clickKeFuBtn;
        ClickListener.Get(ResetBtn).onClick = clickResetBtn;
    }
    public void clickKeFuBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.kefupanel);
    }

    public void clickResetBtn(GameObject obj)
    {
        Hint.LoadTips("重新登录!验证手机号重置密码", Color.white);
    }

    public void clickConfrimBtn(GameObject obj)
    {
            if (NewPassword.text == CfmPassword.text)
            {
          //  ReqUpdatePasswordMessage pak = new ReqUpdatePasswordMessage();
            ReqUpdateUserInfoMessage ReqUUIM = new ReqUpdateUserInfoMessage();
                ReqUUIM.accountId = DataMgr.m_account.id;
                ReqUUIM.info = new List<UserInfoMap>();
                UserInfoMap m_UserInfoMap = new UserInfoMap();
                m_UserInfoMap.infoType = 602;
                Dictionary<string, string> PasswordDic = new Dictionary<string, string>();
                PasswordDic.Add(NewPassword.text, OriPassword.text);
                m_UserInfoMap.info = JsonConvert.SerializeObject(PasswordDic);
                ReqUUIM.info.Add(m_UserInfoMap);

               // RSAEncryption.register();
                 RSAEncryption.Password_Send(NewPassword.text, OriPassword.text, false);
                //HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdateUserInfoMessage, ReqUUIM);
            }
            else if (NewPassword.text != CfmPassword.text)
            {
                Hint.LoadTips("确认密码错误，请确认", Color.white);
            }
    }
    public void Init()
    {
        OriPassword.text = "";
        NewPassword.text = "";
        CfmPassword.text = "";
    }
    private void OnEnable()
    {
        Init();
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspUpdateUserInfoMessage, OnEvNetUpdateInfo);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, Receive_data);
    }
    private void OnDisable()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspUpdateUserInfoMessage, OnEvNetUpdateInfo);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, Receive_data);
    }


    public void Receive_data(byte[] buf)
    {
        RSAEncryption.Receive_data(buf);
    }

    void OnEvNetUpdateInfo(byte[] buf)
    {
        RspUpdateUserInfoMessage RspUUIM = PBSerializer.NDeserialize<RspUpdateUserInfoMessage>(buf);
        if (RspUUIM.code != 0)
        {
            Hint.LoadTips("修改密码成功", Color.white);
            //DataMgr.m_account.password = RspUUIM.userInfoMap[0].info;
            Init();
        }
        else if (RspUUIM.code == 0)
        {
            Hint.LoadTips(RspUUIM.tips, Color.white);
        }
    }
}
