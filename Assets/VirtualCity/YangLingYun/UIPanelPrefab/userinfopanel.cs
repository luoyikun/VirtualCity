using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Framework.UI;
using ProtoDefine;
using Net;
using SGF.Codec;

public enum EnUserInfoType
{
    Friend,
    Stranger,
    FakeRemote
}
public class userinfopanel : UGUIPanel {
    public GameObject DeleteBtn;
    public GameObject FriendPar;
    public GameObject StrangerPar;
    public GameObject ZheZhaoBtn;
    public ChatUser Target_ChatUser;
    public bool IsFriend = false;

    public Image m_headImg;
    long m_strangerId;
    public static userinfopanel uip;
    EnUserInfoType m_enUserType = EnUserInfoType.Stranger;
    public override void OnOpen()
    {
        uip = this;
        FriendPar.SetActive(false);
        StrangerPar.SetActive(false);
        //NetEventManager.Instance.AddEventListener(MsgIdDefine.UpdateUserInfoRsp, OnEvNetDeleteFriend);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetPlayerByIdMessage, OnNetRspGetPlayerByIdMessage);
    }
    public override void OnClose()
    {
        //NetEventManager.Instance.RemoveEventListener(MsgIdDefine.UpdateUserInfoRsp, OnEvNetDeleteFriend);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetPlayerByIdMessage, OnNetRspGetPlayerByIdMessage);
    }

    void OnNetRspGetPlayerByIdMessage(byte[] buf)
    {

        RspGetPlayerByIdMessage rsp = PBSerializer.NDeserialize<RspGetPlayerByIdMessage>(buf);
        if (rsp.users.Count == 1)
        {
            ChatUser chat = rsp.users[0];
            Target_ChatUser = chat;
            Init(chat);

        }

    }
    public void OnEvNetDeleteFriend(byte[] buf)
    {
        //RspUpdateUserInfoMessage RspUUIM = PBSerializer.NDeserialize<RspUpdateUserInfoMessage>(buf);
        //if (RspCM.rspcmd == 509)
        //{
        //    if (RspCM.code == 1)
        //    {
        //        UIManager.Instance.PopSelf();
        //        chatpanel.cp.Init();
        //    }
        //}
    }

    private void Start()
    {
        ClickListener.Get(ZheZhaoBtn).onClick = clickZheZhao;
        ClickListener.Get(DeleteBtn).onClick = clickDeleteBtn;

        for (int i = 0; i < FriendPar.transform.childCount; i++)
        {
            ClickListener.Get(FriendPar.transform.GetChild(i).gameObject).onClick = clickMenuBtn;
        }
        for (int i = 0; i < StrangerPar.transform.childCount; i++)
        {
            ClickListener.Get(StrangerPar.transform.GetChild(i).gameObject).onClick = clickMenuBtn;
        }

    }
    public void InitPlayer(long accountId)
    {
        ReqGetPlayerByIdMessage req = new ReqGetPlayerByIdMessage();
        req.accountIds = new List<long?>();
        req.accountIds.Add(accountId);

        ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetPlayerByIdMessage, req);


        


    }
    public void Init(ChatUser Target_ChatUserTmp,bool IsHasDeleteFriend = true)
    {
        Target_ChatUser = Target_ChatUserTmp;
        PublicFunc.CreateHeadImg(m_headImg, Target_ChatUserTmp.modelId);
        IsFriend = false;
        for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.friendList.Count; i++)
        {
            if (DataMgr.m_RspGetSocialityInfoMessage.friendList[i].accountId == Target_ChatUser.accountId)
            {
                IsFriend = true;
            }
        }
        if (IsFriend == true)
        {
            if (IsHasDeleteFriend == true)
            {
                DeleteBtn.gameObject.SetActive(true);
            }
            FriendPar.SetActive(true);
            transform.Find("Content/Userinfo").GetComponent<Text>().text = "资产" + double.Parse(((int)Target_ChatUser.income).ToString());
        }
        else if (IsFriend == false)
        {
            m_strangerId = Target_ChatUser.accountId;
            DeleteBtn.gameObject.SetActive(false);
            StrangerPar.SetActive(true);
            transform.Find("Content/Userinfo").GetComponent<Text>().text = "资产" + double.Parse(((int)Target_ChatUser.income).ToString());
        }
        transform.Find("Content/UserName").GetComponent<Text>().text = Target_ChatUser.userName;
    }
    void clickZheZhao(GameObject obj)
    {
        UIManager.Instance.PopSelf(true);
    }
    void clickDeleteBtn(GameObject obj)
    {
        ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, false);
        ispanel.SetContent("提示", "是否要删除此好友？");
        ispanel.m_ok = () =>
        {
            ReqDeleteFrindMessage ReqDFM = new ReqDeleteFrindMessage();
            ReqDFM.friendId = Target_ChatUser.accountId;
            ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqDeleteFriendMessage, ReqDFM);
        };
    }

    public void InfoInit(EnUserInfoType enUserInfoType,string name = "")
    {
        m_enUserType = enUserInfoType;
        switch (m_enUserType)
        {
            case EnUserInfoType.Friend:
                break;
            case EnUserInfoType.Stranger:
                break;
            case EnUserInfoType.FakeRemote:
                transform.Find("Content/UserName").GetComponent<Text>().text = name;
                FriendPar.SetActive(false);
                DeleteBtn.gameObject.SetActive(false);
                StrangerPar.SetActive(true);
                transform.Find("Content/Userinfo").GetComponent<Text>().text = "资产" + double.Parse(((int)7370).ToString());
                break;
            default:
                break;
        }
    }
    void clickMenuBtn(GameObject obj)
    {
        switch (obj.name)
        {
            case "InvitationBtn":
                Debug.Log("发送邀请");

                break;
            case "GoToBtn":
                Debug.Log("前往家园");
                VirtualCityMgr.GotoHometown(EnMyOhter.Other, Target_ChatUser);
                //chatpanel.cp.CloseChatWindws();
                break;
            case "SendBtn":
                Debug.Log("发送信息");

                break;
            case "TianJiaBtn":
                SystemNotifyMessage SNM = new SystemNotifyMessage();
                SNM.systemNotify = new SystemNotify();
                SNM.systemNotify.AccountId = m_strangerId;
                SNM.systemNotify.Type = 1;
                SNM.systemNotify.NotifyFrom = DataMgr.m_account.id;
                SNM.systemNotify.HasHandle = 0;
                SNM.systemNotify.HandleRes = "0";
                SNM.systemNotify.Title = "";
                SNM.systemNotify.Content = DataMgr.m_account.userName+"请求添加你为好友";
                ChatSocket.Instance.SendMsgProto(MsgIdDefine.SystemNotifyMessage, SNM);
                UIManager.Instance.PopSelf(true);
                Hint.LoadTips("发送添加好友", Color.white);
                break;
        }
    }


}
