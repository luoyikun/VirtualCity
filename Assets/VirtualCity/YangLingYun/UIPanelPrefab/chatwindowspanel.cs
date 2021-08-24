using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using ICSharpCode.SharpZipLib.Core;
using ProtoDefine;
using SGF.Codec;
using Newtonsoft.Json;

public class chatwindowspanel : UGUIPanel
{
    public GameObject MainPar;
    public static chatwindowspanel cwp;
    public static ChatGroup Target_ChatGroup;
    public static List<ChatUser> Target_ChatUserList;

    public bool IsSearch = false;
    public static List<ChatUser> Target_SearchChatUser;
    // Use this for initialization
    // bool IsInit = false;
    // bool IsGroupSetting = false;
    string m_WindowsName;
    public bool IsManager = false;
    private int pagenumber;
    private int createGourpCount;
    private string targetChatGroupName;
    private void Awake()
    {
        cwp = this;
        MainPar = this.transform.GetChild(1).gameObject;
    }
    private void OnEnable()
    {
        //NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetSocialityInfoMessage, OnNetEvRspGetSocialityInfoMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspSearchUserMessage, OnNetSearchUserMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspUpdateUserInfoMessage, OnEvNetUpdateUserInfo);
        //NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetPlayerByIdMessage, OnEvNetGetPlayerByIdMessage);
        //NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetSocialityInfoMessage, OnNetEvRspGetSocialityInfoMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, OnEvNetCommentMessage);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCreateGroupMessage, OnEvNetCreateGourpMessage);
    }
    private void OnDisable()
    {
        //NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetSocialityInfoMessage, OnNetEvRspGetSocialityInfoMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspSearchUserMessage, OnNetSearchUserMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspUpdateUserInfoMessage, OnEvNetUpdateUserInfo);
        //NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetPlayerByIdMessage, OnEvNetGetPlayerByIdMessage);
        //NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetSocialityInfoMessage, OnNetEvRspGetSocialityInfoMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, OnEvNetCommentMessage);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCreateGroupMessage, OnEvNetCreateGourpMessage);
    }
    void OnEvNetCreateGourpMessage(byte[] buf)
    {
        RspCreateGroupMessage rspCreateGourpMessage = PBSerializer.NDeserialize<RspCreateGroupMessage>(buf);

        chatpanel.cp.leftMenuPar.transform.Find("CreateGroup").gameObject.SetActive(false);
        DestroyImmediate(chatpanel.cp.leftMenuPar.transform.Find("CreateGroup").gameObject);

        chatpanel.cp.UpdateGourpInfo(rspCreateGourpMessage.group);
        
       clickBackBtn(new GameObject());
    }
    void OnNetSearchUserMessage(byte[] buf)
    {
        RspSearchUserMessage RspSUM = PBSerializer.NDeserialize<RspSearchUserMessage>(buf);
        if (IsSearch == false)
        {
            Target_ChatUserList = RspSUM.userName;
            InitMember(m_WindowsName);
        }
        else if (IsSearch == true)
        {
            MainPar.transform.Find("GroupManage").GetComponent<GroupManage>().InitMember(RspSUM.userName);
        }
        //if (m_WindowsName == "GroupManage")
        //{
        //    MainPar.transform.Find("GroupManage").GetComponent<GroupManage>().InitMember(Target_ChatUserList);
        //    return;
        //}
        
        //InitMember(m_WindowsName);

        //Debug.Log("1");
    }
    void OnEvNetCommentMessage(byte[] buf)
    {
        RspCommentMessage RspCM = PBSerializer.NDeserialize<RspCommentMessage>(buf);
        switch (RspCM.rspcmd)
        {
            case 506:
                //创建群成功
                if (RspCM.code == 0)
                {
                    Hint.LoadTips(RspCM.tip, Color.white);
                }
                else
                {
                    Debug.LogError(RspCM.rspcmd + "接收到了创建群成功的Comment");
                    
                }

                break;
            case 510:
                //群名称更新
                if (RspCM.code == 0)
                {
                    //IsEditOver = false;
                    Hint.LoadTips(RspCM.tip, Color.white);
                }
                else if (RspCM.code == 1)
                {
                    Target_ChatGroup.Name = targetChatGroupName;
                    chatpanel.cp.UpdateGourpInfo(Target_ChatGroup);


                    //PublicFunc.Destroy(chatLeftBtn.clb.MainPar.transform.Find(Target_ChatGroup.Id.ToString()).gameObject);

                    //IsCreateAdd = false;

                    //Init();

                    //clickBackBtn(BackBtn);
                }
                break;
            case 507:
                if (RspCM.code == 0)
                {

                }
                else if (RspCM.code == 1)
                {//解散群之后，所有群成员会收到这条信息

                    if (chatpanel.cp.MainPar.transform.Find(Target_ChatGroup.Id.ToString()) != null)
                    {
                        PublicFunc.Destroy(chatpanel.cp.MainPar.transform.Find(Target_ChatGroup.Id.ToString()).gameObject);
                        PublicFunc.Destroy(chatpanel.cp.leftMenuPar.transform.Find(Target_ChatGroup.Id.ToString()).gameObject);

                        //IsCreateAdd = false;

                        //Init();

                        //clickBackBtn(BackBtn);
                    }
                }
                break;
            case 508:
                if (RspCM.code == 0)
                {

                }
                else if (RspCM.code == 1)
                {//自己退出群之后,或者被踢出群之后，会收到这条通知

                    PublicFunc.Destroy(chatpanel.cp.MainPar.transform.Find(Target_ChatGroup.Id.ToString()).gameObject);

                    //PublicFunc.Destroy(chatpanel.cp.LeftMenuPar.transform.Find(Target_ChatGroup.Id.ToString()).gameObject);
                    clickBackBtn(this.gameObject);
                    //IsCreateAdd = false;
                    //Init();
                    //clickBackBtn(BackBtn);
                }
                else if (RspCM.code == 2)
                {//别人退出群之后自己收到这条通知
                    //PublicFunc.Destroy(chatLeftBtn.clb.MainPar.transform.Find(Target_ChatGroup.Id.ToString()).gameObject);
                    //PublicFunc.Destroy(chatpanel.cp.LeftMenuPar.transform.Find(Target_ChatGroup.Id.ToString()).gameObject);
                    //IsCreateAdd = false;
                    //Init();
                    //clickBackBtn(BackBtn);

                }
                break;
        }
        //chatpanel.cp.ReqGetSociality();
        // InitMember();
    }
    void OnEvNetUpdateUserInfo(byte[] buf)
    {

    }
    public void Init(string WindowsName)
    {
        cwp = this;
        m_WindowsName = WindowsName;
        MainPar = this.transform.GetChild(1).gameObject;
        pagenumber = 0;
        Target_ChatUserList=new List<ChatUser>();
        targetChatGroupName = "";
        IsSearch = false;
        if (Target_ChatGroup.AccountId == DataMgr.m_account.id)
        {//如果目标群聊的管理ID等于这个账号的ID，则为管理员
            IsManager = true;
        }
        else
        {
            IsManager = false;
        }
        for (int i = 0; i < MainPar.transform.childCount; i++)
        {
            MainPar.transform.GetChild(i).gameObject.SetActive(false);
        }
        GetGroupSettingGourpPlayer();
        //GetAllGroupMember();
        //chatpanel.cp.ReqGetSociality();//向服务器索取最新的社交信息
        //for(int i=0;i<)
    }
    void GetGroupSettingGourpPlayer()
    {

        ReqSearchUserMessage reqSearchUserMessage = new ReqSearchUserMessage();
        reqSearchUserMessage.groupId = Target_ChatGroup.Id;
        reqSearchUserMessage.pageIndex = pagenumber;
        //    reqSearchUserMessage.accountIds = Target_AccountIdList;
        ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqSearchUserMessage, reqSearchUserMessage);
    }
    public void InitMember(string WindowsName)
    {//初始化所有窗口的成员
        switch (WindowsName)
        {
            case "GroupSetting":
                MainPar.transform.Find(WindowsName).GetComponent<GroupSetting>().InitMember(Target_ChatUserList);
                break;
            case "GroupManage":
                MainPar.transform.Find(WindowsName).GetComponent<GroupManage>().InitMember(Target_ChatUserList);
                //ReqSUM(null);
                break;
            case "InvitationFriend":
                MainPar.transform.Find(WindowsName).GetComponent<InvitationFriend>().InitMember();
                //Debug.Log("11");
                break;
        }
        openWindwos(WindowsName);//成员初始化之后打开窗口
        //Init();
    }
    void ReqSUM(string UserName)
    {
        ReqSearchUserMessage ReqSUM = new ReqSearchUserMessage();
        ReqSUM.groupId = Target_ChatGroup.Id;
        ReqSUM.userName = UserName;
        ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqSearchUserMessage, ReqSUM, EnSocket.Chat);
    }

    public void SendEditGroupName(string targetName)
    {
        targetChatGroupName = targetName;
        ReqUpdateGroupMessage ReqUGM = new ReqUpdateGroupMessage();
        ReqUGM.gourpId = Target_ChatGroup.Id;
        ReqUGM.groupName = targetChatGroupName;
        ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdateGroupMessage, ReqUGM, EnSocket.Chat);
    }
    public void openWindwos(string WindowsName)
    {//通过WindowsName打开一个子物体
     //Init();
        Transform tran;
        for (int i = 0; i < MainPar.transform.childCount; i++)
        {
            MainPar.transform.GetChild(i).gameObject.SetActive(false);
        }
        if (WindowsName == null)
        {
            return;
        }
        m_WindowsName = WindowsName;
        tran = MainPar.transform.Find(WindowsName);
        tran.gameObject.SetActive(true);
        //switch (WindowsName)
        //{//不同的WindwsName进行不同的操作
        //    case "GroupSetting":
        //        tran.GetComponent<GroupSetting>().Init();
        //        break;
        //    case "GroupManage":
        //        tran.GetComponent<GroupManage>().Init();
        //        break;`
        //    case "InvitationFriend":
        //        tran.GetComponent<InvitationFriend>().Init();
        //        break;
        //}
        //InitMember(WindowsName);
    }
    public void clickBackBtn(GameObject obj)
    {
        openWindwos(null);
        if (UIManager.Instance.IsTopPanel("chatwindowspanel"))
        {
            //chatpanel.cp.ReqGetSociality();
            chatpanel.cp.IsTopPanel = true;
            UIManager.Instance.PopSelf();
        }
    }
    public void clickHeadImage(GameObject obj)
    {
        for (int i = 0; i < Target_ChatUserList.Count; i++)
        {
            if (Target_ChatUserList[i].accountId == long.Parse(obj.name))
            {
                ChatUser TargetChatUser = Target_ChatUserList[i];
                UIManager.Instance.PushPanel(UIPanelName.userinfopanel, false, true, paragrm => { paragrm.GetComponent<userinfopanel>().Init(TargetChatUser); });
            }
        }
    }
}
