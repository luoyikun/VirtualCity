using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using UnityEngine.UI;
using ProtoDefine;
using SGF.Codec;
using Newtonsoft.Json;

public class editaccountpanel : UGUIPanel
{
    public GameObject Fond;
    public GameObject DownPar;
    public GameObject BackBtn;
    public InputField NameInput;
    public InputField TelephoneInput;
    //public string AccountString;
    //public string RealNameString;
    public bool IsALiPay = false;
    public bool m_IsEdit;
    public static editaccountpanel eap;
    public GameObject Target_Object;
    public List<PayAccount> m_ListPayAccount;
    public PayAccount m_PayAccount;

    public PayAccount Target_PayAccount;
    // Use this for initialization
    private void Awake()
    {
        eap = this;
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        ClickListener.Get(DownPar.transform.GetChild(0).GetChild(0).gameObject).onClick = clickSaveBtn;
        ClickListener.Get(DownPar.transform.GetChild(0).GetChild(1).gameObject).onClick = clickDeleteBtn;
        ClickListener.Get(DownPar.transform.GetChild(1).gameObject).onClick = clickSaveBtn;
    }
    private void OnEnable()
    {
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
            Debug.Log("保存了支付宝账号成功");
            DataMgr.m_account.userPayAccount = RspUUIM.userInfoMap[0].info;
            // accountTest.AT.InitAccountList(m_LPA);
            UIManager.Instance.PopSelf();
        }
        else if (RspUUIM.code == 0)
        {
            for (int i = 0; i < m_ListPayAccount.Count; i++)
            {
                if (m_ListPayAccount[i] == Target_PayAccount)
                {
                    m_ListPayAccount.RemoveAt(i);
                    break;
                }
            }
            string m_StringListPayAccount = JsonConvert.SerializeObject(m_ListPayAccount);
            DataMgr.m_account.userPayAccount = m_StringListPayAccount;
            Hint.LoadTips(RspUUIM.tips, Color.white);
        }
    }
    void clickBackBtn(GameObject obj)
    {
        if (m_isOpen == true)
        UIManager.Instance.PopSelf(true);
    }
    void clickSaveBtn(GameObject obj)
    {
        ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, true);
        ispanel.SetContent("提示", "奖励账号绑定之后将无法修改,如果奖励账号有误，奖励金额将无法追回，请确定要绑定这个账号吗？");
        ispanel.m_ok = () =>
        {
            ReqUpdateUserInfoMessage ReqUUIM = new ReqUpdateUserInfoMessage();
            ReqUUIM.accountId = DataMgr.m_account.id;
            ReqUUIM.info = new List<UserInfoMap>();
            UserInfoMap m_UserInfoMap = new UserInfoMap();
            m_UserInfoMap.infoType = 601;
            List<PayAccount> ListPayAccount = new List<PayAccount>();
            ListPayAccount = m_ListPayAccount;
            // M_ListPayAccount = accountsecuritypanel.asp.m_LPA;
            PayAccount PA = new PayAccount();
            PA.account = TelephoneInput.transform.GetComponent<InputField>().text;
            PA.realName = NameInput.transform.GetComponent<InputField>().text;
            if (IsALiPay == true)
            {
                PA.payType = 0;
            }
            else
            {
                PA.payType = 1;
            }
            Target_PayAccount = PA;
            if (m_IsEdit == true)
            {
                for (int i = 0; i < ListPayAccount.Count; i++)
                {
                    if (ListPayAccount[i].account == m_PayAccount.account && ListPayAccount[i].realName == m_PayAccount.realName)
                    {
                        ListPayAccount[i] = PA;
                    }
                }
            }
            else if (m_IsEdit == false)
            {
                ListPayAccount.Add(PA);
            }

            string m_StringListPayAccount = JsonConvert.SerializeObject(ListPayAccount);
            m_UserInfoMap.info = m_StringListPayAccount;
            DataMgr.m_account.userPayAccount = m_StringListPayAccount;
            ReqUUIM.info.Add(m_UserInfoMap);
            HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdateUserInfoMessage, ReqUUIM);
        };

    }
    void clickDeleteBtn(GameObject obj)
    {
        ReqUpdateUserInfoMessage ReqUUIM = new ReqUpdateUserInfoMessage();
        ReqUUIM.accountId = DataMgr.m_account.id;
        ReqUUIM.info = new List<UserInfoMap>();
        UserInfoMap m_UserInfoMap = new UserInfoMap();
        m_UserInfoMap.infoType = 601;
        List<PayAccount> ListPayAccount = new List<PayAccount>();
        ListPayAccount = m_ListPayAccount;
        for (int i = 0; i < ListPayAccount.Count; i++)
        {
            if (ListPayAccount[i].account == m_PayAccount.account && ListPayAccount[i].realName == m_PayAccount.realName)
            {
                ListPayAccount.RemoveAt(i);
                //DestroyImmediate(Target_Object);
                break;
            }
        }
        string m_StringListPayAccount = JsonConvert.SerializeObject(ListPayAccount);
        m_UserInfoMap.info = m_StringListPayAccount;
        DataMgr.m_account.userPayAccount = m_StringListPayAccount;
        ReqUUIM.info.Add(m_UserInfoMap);
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdateUserInfoMessage, ReqUUIM);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Stateinit(bool IsEdit)
    {
        if (IsEdit == true)
        {
            NameInput.GetComponent<InputField>().text = m_PayAccount.realName;
            TelephoneInput.GetComponent<InputField>().text = m_PayAccount.account;
            DownPar.transform.GetChild(0).gameObject.SetActive(true);
            DownPar.transform.GetChild(1).gameObject.SetActive(false);
            Fond.transform.GetChild(0).gameObject.SetActive(true);
            Fond.transform.GetChild(1).gameObject.SetActive(false);

        }
        else if (IsEdit == false)
        {
            NameInput.GetComponent<InputField>().text = "";
            TelephoneInput.GetComponent<InputField>().text = "";
            DownPar.transform.GetChild(0).gameObject.SetActive(false);
            DownPar.transform.GetChild(1).gameObject.SetActive(true);
            Fond.transform.GetChild(0).gameObject.SetActive(false);
            Fond.transform.GetChild(1).gameObject.SetActive(true);
        }
        m_IsEdit = IsEdit;
    }
}
