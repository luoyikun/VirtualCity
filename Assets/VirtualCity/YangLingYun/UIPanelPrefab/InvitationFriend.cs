using Framework.UI;
using Net;
using ProtoDefine;
using SGF.Codec;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvitationFriend : MonoBehaviour {
    public GameObject BackBtn;
    public GameObject FriendTmp;
    public GameObject FriendPar;
    public GameObject QueDingBtn;
    public List<long> SelectList = new List<long>();
    // Use this for initialization
    private void OnEnable()
    {
        //Init();
        //NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, OnEvNetCommentMessage);
    }
    public void InitMember()
    {
        if (FriendPar.transform.childCount != 0)
        {
            for (int i = FriendPar.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(FriendPar.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.friendList.Count; i++)
        {
            bool IsFriend=false;
            for (int j = 0; j < chatwindowspanel.Target_ChatUserList.Count;j++)
            {
                if (chatwindowspanel.Target_ChatUserList[j].accountId ==
                    DataMgr.m_RspGetSocialityInfoMessage.friendList[i].accountId)
                {
                    IsFriend = true;
                    break;
                }
            }

            if (IsFriend == true)
            {
                continue;
            }
            GameObject obj = PublicFunc.CreateTmp(FriendTmp, FriendPar.transform);
            obj.name = DataMgr.m_RspGetSocialityInfoMessage.friendList[i].accountId.ToString();
            obj.transform.Find("MemberName").GetComponent<Text>().text = DataMgr.m_RspGetSocialityInfoMessage.friendList[i].userName;
            PublicFunc.CreateHeadImg(obj.transform.Find("MemberImage").GetComponent<Image>(), DataMgr.m_RspGetSocialityInfoMessage.friendList[i].modelId);
            obj.transform.Find("MemberImage").GetChild(0).gameObject.SetActive(false);
            ClickListener.Get(obj.transform.Find("SelectBtn").gameObject).onClick = clickSelectBtn;
        }
        UpdateSelectList();
    }
    void UpdateSelectList()
    {
        for (int i = 0; i < FriendPar.transform.childCount; i++)
        {
            FriendPar.transform.GetChild(i).Find("SelectBtn").GetChild(1).gameObject.SetActive(false);
        }
        if (SelectList.Count != 0)
        {
            for (int i = 0; i < SelectList.Count; i++)
            {
                if (FriendPar.transform.Find(SelectList[i].ToString()) == null)
                {
                    SelectList.Remove(SelectList[i]);
                    continue;
                }
                FriendPar.transform.Find(SelectList[i].ToString()).Find("SelectBtn").GetChild(1).gameObject.SetActive(true);
            }
        }
    }
    private void Start()
    {
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        ClickListener.Get(QueDingBtn).onClick = clickQueDing;
    }
    private void OnDisable()
    {
        //NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, OnEvNetCommentMessage);
    }
    public void Init()
    {
        //if (chatpanel.m_RspGSIM.onlineFriend != null)
        //{
        //    for (int i = 0; i < chatpanel.m_RspGSIM.onlineFriend.Count; i++)
        //    {
        //        if (chatwindowspanel.Target_ChatGroup.AccountIdList != null)
        //        {
        //            if (chatwindowspanel.Target_ChatGroup.AccountIdList.Contains(chatpanel.m_RspGSIM.onlineFriend[i].accountId.ToString()))
        //            {
        //                continue;
        //            }
        //        }
        //        GameObject obj = PublicFunc.CreateTmp(FriendTmp, FriendPar.transform);
        //        obj.name = chatpanel.m_RspGSIM.onlineFriend[i].accountId.ToString();
        //        obj.transform.Find("MemberName").GetComponent<Text>().text = chatpanel.m_RspGSIM.onlineFriend[i].userName;
        //        obj.transform.Find("MemberImage").GetChild(0).gameObject.SetActive(false);
        //        ClickListener.Get(obj.transform.Find("SelectBtn").gameObject).onClick = clickSelectBtn;
        //    }
        //}
        //if (chatpanel.m_RspGSIM.offlineFriend != null)
        //{
        //    for (int i = 0; i < chatpanel.m_RspGSIM.offlineFriend.Count; i++)
        //    {
        //        if (chatwindowspanel.Target_ChatGroup.AccountIdList != null)
        //        {
        //            if (chatwindowspanel.Target_ChatGroup.AccountIdList.Contains(chatpanel.m_RspGSIM.offlineFriend[i].accountId.ToString()))
        //            {
        //                continue;
        //            }
        //        }
        //        GameObject obj = PublicFunc.CreateTmp(FriendTmp, FriendPar.transform);
        //        obj.name = chatpanel.m_RspGSIM.offlineFriend[i].accountId.ToString();
        //        obj.transform.Find("MemberName").GetComponent<Text>().text = chatpanel.m_RspGSIM.offlineFriend[i].userName;
        //        obj.transform.Find("MemberImage").GetChild(0).gameObject.SetActive(false);
        //        ClickListener.Get(obj.transform.Find("SelectBtn").gameObject).onClick = clickSelectBtn;
        //    }
        //}
    }
    void clickBackBtn(GameObject obj)
    {
        chatwindowspanel.cwp.clickBackBtn(obj);
    }
    void clickSelectBtn(GameObject obj)
    {
        //if (obj.transform.GetChild(1).gameObject.activeSelf == true)
        //{
        //    for (int i = 0; i < SelectObjList.Count; i++)
        //    {
        //        if (SelectObjList[i].gameObject == obj.transform.parent.gameObject)
        //        {
        //            SelectObjList.Remove(obj.transform.parent.gameObject);
        //            obj.transform.GetChild(1).gameObject.SetActive(false);
        //        }
        //    }
        //}
        //else if (obj.transform.GetChild(1).gameObject.activeSelf == false)
        //{
        //    SelectObjList.Add(obj.transform.parent.gameObject);
        //    obj.transform.GetChild(1).gameObject.SetActive(true);
        //}
        if (SelectList.Contains(long.Parse(obj.transform.parent.name)))
        {
            SelectList.Remove(long.Parse(obj.transform.parent.name));
        }
        else
        {
            SelectList.Add(long.Parse(obj.transform.parent.name));
        }
        UpdateSelectList();
    }
    void clickQueDing(GameObject obj)
    {
        for (int i = 0; i < SelectList.Count; i++)
        {
            SystemNotifyMessage SNM = new SystemNotifyMessage();
            SNM.systemNotify = new SystemNotify();
            SNM.systemNotify.AccountId = SelectList[i];
            SNM.systemNotify.Type = (int)2;
            SNM.systemNotify.HasHandle = (int)0;
            SNM.systemNotify.HandleRes = "0";
            SNM.systemNotify.Title = "";
            SNM.systemNotify.Createtime = Time.time.ToString();
            SNM.systemNotify.NotifyFrom = chatwindowspanel.Target_ChatGroup.Id;
            SNM.systemNotify.Content = DataMgr.m_account.userName+"邀请你进入群："+chatwindowspanel.Target_ChatGroup.Name;
            ChatSocket.Instance.SendMsgProto(MsgIdDefine.SystemNotifyMessage, SNM, EnSocket.Chat);
            //SelectObjList[i].transform.Find("SelectBtn").GetChild(1).gameObject.SetActive(false);
        }
        Hint.LoadTips("已发送邀请", Color.white);
        SelectList.Clear();
        UpdateSelectList();
        //chatwindowspanel.cwp.clickBackBtn(obj);
    }
}
