using Framework.UI;
using ProtoDefine;
using SGF.Codec;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class htspeeduppanel : UGUIPanel
{

    public Devlopments m_deveInfo;
    public Transform m_hasSpeedUpPar;
    public GameObject m_tmpSpeedUp;
    public GameObject m_friendIcon;
    public GameObject m_btnOk;
    public GameObject m_btnClose;
    List<KeyValuePair<string, Recode>> m_listHelp = new List<KeyValuePair<string, Recode>>();
    Dictionary<long, ChatUser> m_dicChatUser = new Dictionary<long, ChatUser>();
    int m_idx;
    bool m_isMySpeedUp = false;

    public Text m_textCost;
    public Transform m_bgSpeedUpPar;
    public Transform m_bgTmp;
    // Use this for initialization
    void Start()
    {
        ClickListener.Get(m_btnOk).onClick = OnBtnOk;
        ClickListener.Get(m_btnClose).onClick = OnBtnClose;
        ClickListener.Get(m_friendIcon).onClick = OnBtnFriendIcon;

        for (int i = 0; i < DataMgr.m_slefHPTimes; i++)
        {
            PublicFunc.CreateTmp(m_bgTmp.gameObject, m_bgSpeedUpPar);
        }
    }

    void OnBtnFriendIcon(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.userinfopanel, false, true,  (param) => {
            param.GetComponent<userinfopanel>().Init(m_dicChatUser[long.Parse(m_listHelp[m_idx].Key)], false);
            //param.GetComponent<userinfopanel>().Init(m_dicChatUser[long.Parse((long)m_listHelp[m_idx].Key)],false);
        });
    }

    void OnBtnClose(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
    void OnBtnOk(GameObject obj)
    {
        buildhometown.m_instance.SendReqSpeedUpMessage(m_deveInfo.id);
        UIManager.Instance.PopSelf();
    }

    public override void OnOpen()
    {
        for (int i = 0; i < m_hasSpeedUpPar.childCount; i++)
        {
            Destroy(m_hasSpeedUpPar.GetChild(i).gameObject);
        }
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetPlayerByIdMessage, OnGetPlayerByIdMessage);
       
    }

    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetPlayerByIdMessage, OnGetPlayerByIdMessage);
    }

    public void SetInfo(Devlopments deve)
    {
        m_deveInfo = deve;
        SortHelpRecod();
        SetSpeedUp();
        if (DataMgr.m_myOther == EnMyOhter.My)
        {
            DevlopmentProperties pro = DataMgr.m_dicDevlopmentProperties[(long)m_deveInfo.modelId];
            m_textCost.text = "立即加速，消耗" + pro.oncespeedupCost.ToString() + "钻石";
        }
        else {
            bool isTaSpeed = IsMySpeedUp();

            if (isTaSpeed == true)
            {
                m_textCost.text = "帮好友加速，现在加速可以获取钻石奖励";
            }
            else {
                m_textCost.text = "帮好友加速,得到金币奖励";
            }
            
        }
    }


    bool IsMySpeedUp()
    {
        if (m_deveInfo.speedUpTimes == m_deveInfo.friendHelp)
        {
            return false;
            //m_isMySpeedUp = false;
        }
        else if (m_deveInfo.speedUpTimes > m_deveInfo.friendHelp)
        {
            return true;
            //m_isMySpeedUp = true;
        }
        return false;
    }
    void SetSpeedUp()
    {
        if (m_deveInfo.speedUpTimes != null)
        {
            for (int i = 0; i < m_deveInfo.speedUpTimes; i++)
            {
                GameObject obj = PublicFunc.CreateTmp(m_tmpSpeedUp, m_hasSpeedUpPar, Vector3.zero, Vector3.zero, Vector3.one);
                obj.name = i.ToString();
                ClickListener.Get(obj).onClick = OnBtnClickSpeedUp;
            }
        }

        //for (int i = 0; i < 3; i++)
        //{
        //    GameObject obj = PublicFunc.CreateTmp(m_tmpSpeedUp, m_hasSpeedUpPar, Vector3.zero, Vector3.zero, Vector3.one);
        //    obj.name = i.ToString();
        //    ClickListener.Get(obj).onClick = OnBtnClickSpeedUp;
        //}
    }

    void OnBtnClickSpeedUp(GameObject obj)
    {
        m_idx = int.Parse(obj.name);
        if (m_idx >= m_listHelp.Count)
        {
            m_friendIcon.SetActive(false);
            return;
        }
        m_friendIcon.SetActive(true);
        Vector3 pos = obj.transform.position;
        pos.y += 50;
        m_friendIcon.transform.position = pos;

        if (m_idx < m_listHelp.Count)
        {
            //todo 创建杨用户的头像
            long modelId = m_dicChatUser[long.Parse(m_listHelp[m_idx].Key)].modelId;
            string modelIcon = DataMgr.m_dicRoleProperties[modelId].Icon;
            AssetMgr.Instance.CreateSpr(modelIcon, "charactericon", (param) =>
            {
                m_friendIcon.transform.Find("icon").GetComponent<Image>().sprite = param;
            });

           
            //m_friendIcon.
        }
    }

    void SortHelpRecod()
    {
        if (m_deveInfo.helpRecod != null)
        {
            m_listHelp = new List<KeyValuePair<string, Recode>>(m_deveInfo.helpRecod);

            m_listHelp.Sort(delegate (KeyValuePair<string, Recode> s1, KeyValuePair<string, Recode> s2)
            {
                long s1time = SyncTime.Server2Stamp(s1.Value.time);
                long s2time = SyncTime.Server2Stamp(s2.Value.time);
                if (s1time < s2time)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }

            });


            ReqGetPlayerByIdMessage ReqGPBIM = new ReqGetPlayerByIdMessage();
            ReqGPBIM.accountIds = new List<long?>();
            for (int i = 0; i < m_listHelp.Count; i++)
            {
                ReqGPBIM.accountIds.Add(long.Parse(m_listHelp[i].Key));
            }

            ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetPlayerByIdMessage, ReqGPBIM, EnSocket.Chat);

        }
    }

    void OnGetPlayerByIdMessage(byte[] buf)
    {
        RspGetPlayerByIdMessage RspGPBIM = PBSerializer.NDeserialize<RspGetPlayerByIdMessage>(buf);
        for (int i = 0; i < RspGPBIM.users.Count; i++)
        {
            ChatUser chat = RspGPBIM.users[i];
            m_dicChatUser[chat.accountId] = chat;
        }
    }
}
