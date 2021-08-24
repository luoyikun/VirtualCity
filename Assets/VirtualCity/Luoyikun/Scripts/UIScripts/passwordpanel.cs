using Framework.UI;
using Net;
using ProtoDefine;
using SGF.Codec;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using System.Text;

public class NewPasswordJson
{
    public string newPassword;
}
//6-15位英文，数字组成密码
public class passwordpanel : UGUIPanel {

    public InputField m_textUp;
    public InputField m_textDown;
    public GameObject m_btnOK;
    public static bool io=true;
    public static int type=0;
    private void Start()
    {
        ClickListener.Get(m_btnOK).onClick = OnBtnOk;
    }

    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspUpdateUserInfoMessage, OnEvNetUpdateUserInfoRsp);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, Receive_data);

        //新用户第一次注册生成秘钥发送给服务器
        /*
        if (DataMgr.m_account.password == null)
        {
            RSAEncryption.register();
            RSAEncryption.star_Encryption();
        }
        */
    }

    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspUpdateUserInfoMessage, OnEvNetUpdateUserInfoRsp);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, Receive_data);
    }
    public void Receive_data(byte[] buf)
    {
        RSAEncryption.Receive_data(buf);
    }
    void OnEvNetUpdateUserInfoRsp(byte[] buf)
    {
        RspCodeTip pro = PBSerializer.NDeserialize<RspCodeTip>(buf);
        if (pro.code == 0)
        {
            Hint.LoadTips(pro.tip, Color.white);
        }
        else {
            //Hint.LoadTips(pro.tip, Color.white);
            UIManager.Instance.PopSelf(false);
        }
    }
    public void OnBtnOk(GameObject obj)
    {
        if (m_textUp.text.Length >= 6 && m_textUp.text.Length <= 10)
        {
            if (m_textUp.text == m_textDown.text)
            {
                if (PublicFunc.IsTiXianOk(m_textUp.text) == false)
                {
                    Hint.LoadTips("密码格式不对", Color.white);
                }
                else
                {
                    /*
                    if (!File.Exists(AppConst.LocalPath + "/Rsa.txt"))
                    {
                        if (DataMgr.m_account.password == null)
                        { 
                            RSAEncryption.register();
                            RSAEncryption.star_Encryption();
                        }
                    }
                    else
                    {
                        RSAEncryption.register();
                        RSAEncryption.Password_Send(m_textUp.text,null, true);
                    }
                    */
                    //  RSAEncryption.register();

                    if (io)
                    {
                        io = false;
                        switch (type)
                        {
                            case 0:
                                RSAEncryption.star_Encryption(m_textUp.text);
                                break;
                            case 1:
                                Debug.Log("忘记密码设置");
                                RSAEncryption.Password_Send_Land(m_textUp.text);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else
            {
                Hint.LoadTips("两次密码不匹配", Color.white);
            }
        }
        else {
            Hint.LoadTips("密码长度大于6位小于10位", Color.white);
        }
    }
}
