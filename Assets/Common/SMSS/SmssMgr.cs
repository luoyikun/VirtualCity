using cn.SMSSDK.Unity;
using Framework.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SmssMgr : SingletonMono<SmssMgr>, SMSSDKHandler
{
    public SMSSDK m_smssdk;
    //please add your phone number
    private string zone = "86";
    private string tempCode = "1319972";
    private string code = "";
    private string result = null;
    string m_phone;

    public UnityAction m_act;
    public UnityAction m_failureAct;
    //public static SmssMgr m_instance;
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
            Debug.Log("手机验证码OK");

            if (m_act != null)
            {
                m_act();
            }
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
        ActionType act = (ActionType)action;
        Debug.Log("Smss error:" + act.ToString());
        if (m_failureAct != null)
        {
            m_failureAct();
        }
        switch (act)
        {
            case ActionType.GetCode:
                break;
            case ActionType.CommitCode:

                break;
            case ActionType.GetSupportedCountries:
                break;
            case ActionType.SubmitUserInfo:
                break;
            case ActionType.GetFriends:
                break;
            case ActionType.GetVersion:
                break;
            case ActionType.ShowRegisterView:
                break;
            case ActionType.ShowContractFriendsView:
                break;
            default:
                break;
        }
    }


    public void StartUp()
    {
        SMSSDK smss = gameObject.AddComponent<SMSSDK>();
        m_smssdk = smss;
        m_smssdk.init("moba6b6c6d6", "b89d2427a3bc7ad1aea1e1e8c1d36bf3", true);
        m_smssdk.setHandler(this);
    }
    public void GetCode(string phone)
    {
        m_smssdk.getCode(CodeType.TextCode,phone, zone, tempCode);
        m_phone = phone;
    }

    public void CommitCode(string commitCode)
    {
        m_smssdk.commitCode(m_phone, zone, commitCode);
    }
}
