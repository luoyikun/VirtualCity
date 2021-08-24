using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using Framework.UI;
using UnityEngine;
using UnityEngine.UI;
using Framework.Tools;
using Newtonsoft.Json;

public class InviteMessage
{
    public string ServerIP;
    public long AccountID;
    public long ModelID;
    public string UserName;
}
public class chat : MonoBehaviour
{
    public GameObject ChatPar;
    public GameObject OtherTmp;
    public GameObject OtherInviteTmp;
    public GameObject SelfTmp;
    public GameObject SelfInviteTmp;
    public GameObject DownPar;
    public InputField IF;
    public int State;
    public ChatUser targetChatUser;
    public ChatGroup targetChatGroup;
    public GameObject PersentChat;
    public Text Sendtxt;
    public Text InviteText;
    public Image Sendimg;
    public static int m_worldLeftTime = 30;
    public static int m_worldInviteTime = 30;
    Button m_btnSend;

    // static bool ioSendBtn=true;
    //  static float time;
    public long ID;


    public void Init()
    {
        if (ChatPar.transform.childCount != 0)
        {
            for (int i = 0; i < ChatPar.transform.childCount; i++)
            {
                PublicFunc.Destroy(ChatPar.transform.GetChild(i).gameObject);
            }
        }
        switch (State)
        {
            case SocialityDataPool.WORLD:
                DownPar.transform.Find("TiShiText").GetComponent<Text>().text = "";
                DownPar.transform.Find("TiShiText").gameObject.SetActive(true);
                DownPar.transform.Find("MoreBtn").gameObject.SetActive(false);
                DownPar.transform.Find("QunSheZhiBtn").gameObject.SetActive(false);
                break;
            case SocialityDataPool.FRIEND:
                DownPar.transform.Find("MoreBtn").gameObject.SetActive(true);
                DownPar.transform.Find("TiShiText").gameObject.SetActive(false);
                DownPar.transform.Find("QunSheZhiBtn").gameObject.SetActive(false);
                break;
            case SocialityDataPool.GROUP:
                DownPar.transform.Find("QunSheZhiBtn").gameObject.SetActive(true);
                DownPar.transform.Find("TiShiText").gameObject.SetActive(false);
                DownPar.transform.Find("MoreBtn").gameObject.SetActive(false);
                break;
        }
        //for (int i = 0; i < TotalCount; i++)
        //{
        //    GameObject obj = PublicFunc.CreateTmp(OtherTmp, ChatPar.transform);
        //    obj.transform.Find("ChatInfo").GetChild(0).GetComponent<Text>().text = i.ToString();
        //    if (IsWorldChat == true)
        //    {
        //        obj.transform.Find("HeadImage").GetComponent<Button>().enabled = true;
        //        ClickListener.Get(obj.transform.Find("HeadImage").gameObject).onClick = clickHeadImage;
        //    }
        //}
        ClickListener.Get(DownPar.transform.Find("SendBtn").gameObject).onClick = clickSendBtn;
        m_btnSend = DownPar.transform.Find("SendBtn").GetComponent<Button>();
        ClickListener.Get(DownPar.transform.Find("MoreBtn").gameObject).onClick = clickMoreBtn;
        ClickListener.Get(DownPar.transform.Find("DaLaBaBtn").gameObject).onClick = clickDaLaBaBtn;
        ClickListener.Get(DownPar.transform.Find("QunSheZhiBtn").gameObject).onClick = clickQunSheZhiBtn;
        ClickListener.Get(DownPar.transform.Find("InviteBtn").gameObject).onClick = clickInviteBtn;
    }

    public void UpdateOtherChatMessage(string ChatText)
    {
        if (ChatPar.transform.childCount >= 50)
        {
            DestroyImmediate(ChatPar.transform.GetChild(0).gameObject);
        }
        GameObject obj = PublicFunc.CreateTmp(OtherTmp, ChatPar.transform);
        obj.name = targetChatUser.accountId.ToString();
        obj.transform.Find("HeadImage").GetComponent<Button>().enabled = true;
        PublicFunc.CreateHeadImg(obj.transform.Find("HeadImage").GetComponent<Image>(), targetChatUser.modelId);
        ClickListener.Get(obj.transform.Find("HeadImage").gameObject).onClick = clickHeadImage;
        obj.transform.Find("ChatInfo").GetChild(0).GetComponent<Text>().text = ChatText;
        obj.transform.Find("UserName").GetComponent<Text>().text = targetChatUser.userName;
        chatpanel.cp.UpdateDownChatText(targetChatUser.userName, ChatText);
        JumpToDown();
    }

    public void UpdateOtherChatMessage(string ChatText, long TargetAccountID, string TargetUserName, long TargetModelID)
    {
        if (ChatPar.transform.childCount >= 50)
        {
            DestroyImmediate(ChatPar.transform.GetChild(0).gameObject);
        }
        GameObject obj = PublicFunc.CreateTmp(OtherTmp, ChatPar.transform);
        obj.name = TargetAccountID.ToString();
        obj.transform.Find("HeadImage").GetComponent<Button>().enabled = true;
        PublicFunc.CreateHeadImg(obj.transform.Find("HeadImage").GetComponent<Image>(), TargetModelID);
        ClickListener.Get(obj.transform.Find("HeadImage").gameObject).onClick = clickHeadImage;
        obj.transform.Find("ChatInfo").GetChild(0).GetComponent<Text>().text = ChatText;
        obj.transform.Find("UserName").GetComponent<Text>().text = TargetUserName;
        JumpToDown();
    }
    public void UpdateOtherInviteMessage(string InviteText)
    {
        if (ChatPar.transform.childCount >= 50)
        {
            DestroyImmediate(ChatPar.transform.GetChild(0).gameObject);
        }

        GameObject obj = PublicFunc.CreateTmp(OtherInviteTmp, ChatPar.transform);
        obj.GetComponent<Invite>().TargetInviteMessage = JsonConvert.DeserializeObject<InviteMessage>(InviteText);
        obj.transform.Find("UserName").GetComponent<Text>().text = targetChatUser.userName;
        PublicFunc.CreateHeadImg(obj.transform.Find("HeadImage").GetComponent<Image>(), targetChatUser.modelId);
        ClickListener.Get(obj.transform.Find("HeadImage").gameObject).onClick = clickHeadImage;
        ClickListener.Get(obj.transform.Find("ChatInfo").gameObject).onClick = clickOtherInviteBtn;
        JumpToDown();
    }
    public void UpdateOtherInviteMessage(string InviteText,long accountID,string TargetUserName,long TargetModelID)
    {
        if (ChatPar.transform.childCount >= 50)
        {
            DestroyImmediate(ChatPar.transform.GetChild(0).gameObject);
        }

        GameObject obj = PublicFunc.CreateTmp(OtherInviteTmp, ChatPar.transform);
        obj.name = accountID.ToString();
        obj.GetComponent<Invite>().TargetInviteMessage  = JsonConvert.DeserializeObject<InviteMessage>(InviteText);
        obj.transform.Find("UserName").GetComponent<Text>().text = TargetUserName;
        PublicFunc.CreateHeadImg(obj.transform.Find("HeadImage").GetComponent<Image>(), TargetModelID);
        ClickListener.Get(obj.transform.Find("ChatInfo").gameObject).onClick = clickOtherInviteBtn;
        JumpToDown();
    }

    public void clickOtherInviteBtn(GameObject obj)
    {
       InviteMessage m_Invite= obj.transform.parent.GetComponent<Invite>().TargetInviteMessage;
       ChatUser targetChatUser =new ChatUser();
        targetChatUser.accountId = m_Invite.AccountID;
        targetChatUser.serverIp = m_Invite.ServerIP;
        targetChatUser.modelId = m_Invite.ModelID;
        targetChatUser.userName = m_Invite.UserName;
        VirtualCityMgr.GotoHometown(EnMyOhter.Other, targetChatUser);
        Debug.Log("去他家园");
    }
    void JumpToDown()
    {
        if (ChatPar.transform.childCount > 5)
        {
            ChatPar.transform.parent.parent.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Unrestricted;
            float ItemHight = ChatPar.GetComponent<GridLayoutGroup>().cellSize.y;
            float ItemNumber = ChatPar.transform.childCount;
            //float OriginPosY = ChatPar.GetComponent<RectTransform>().anchoredPosition.y;
            float TargetY = ItemHight * ItemNumber;
            ChatPar.GetComponent<RectTransform>().sizeDelta = new Vector2(ChatPar.GetComponent<RectTransform>().sizeDelta.x, TargetY);
            ChatPar.GetComponent<RectTransform>().anchoredPosition = new Vector2(ChatPar.GetComponent<RectTransform>().anchoredPosition.x, ChatPar.GetComponent<RectTransform>().sizeDelta.y);
            //Debug.Log(ChatPar.GetComponent<RectTransform>().anchoredPosition.y);
            ChatPar.transform.parent.parent.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Elastic;
            //Debug.Log(ChatPar.GetComponent<RectTransform>().anchoredPosition.y + (TargetY - 100));
        }
    }
    void clickQunSheZhiBtn(GameObject obj)
    {
        chatpanel.cp.clickQunSheZhiBtn(obj);
    }
    void clickDaLaBaBtn(GameObject obj)
    {
        chatpanel.cp.clickDaLaBaBtn(obj);
    }
    void clickMoreBtn(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.userinfopanel, false, true, paragrm => { paragrm.GetComponent<userinfopanel>().Init(targetChatUser); });
    }
    public void clickInviteBtn(GameObject obj)
    {

        createSelfInvite();
        chatpanel.cp.clickInviteBtn(obj.transform.parent.parent.gameObject);
        if (State == SocialityDataPool.WORLD)
        {
            m_worldInviteTime = 30;
            OnTimeInviteWorld();
            TimeManager.Instance.RemoveTask(OnTimeInviteWorld);
            TimeManager.Instance.AddTask(1.0f, true, OnTimeInviteWorld);
            //m_btnSend.enabled = false;
            DownPar.transform.Find("InviteBtn").GetComponent<Button>().interactable = false;
            DownPar.transform.Find("InviteBtn").GetComponent<Image>().raycastTarget = false;
        }
    }
    void clickSendBtn(GameObject obj)
    {
        if (IF.text == "")
        {
            return;
        }
        //if (State==SocialityDataPool.WORLD)
        //{
        //    //if (chatpanel.ioSendBtn)
        //    //{
        //    //    Sendimg.color = new Color(1, 1, 1, 0.5f); ;
        //    //    chatpanel.ioSendBtn = false;
        //    //}
        //    //else
        //    //{
        //    //    return;
        //    //}

        //    if (chatpanel.m_worldLeftTime > 0)
        //    {
        //        return;
        //    }
        //}
        chatpanel.cp.clickSendBtn(obj.transform.parent.parent.gameObject);
        createSelfChat(IF.text);
        IF.text = "";

        if (State == SocialityDataPool.WORLD)
        {
            m_worldLeftTime = 30;
            OnTimeChatWorld();
            TimeManager.Instance.RemoveTask(OnTimeChatWorld);
            TimeManager.Instance.AddTask(1.0f, true, OnTimeChatWorld);
            //m_btnSend.enabled = false;
            m_btnSend.interactable = false;
            m_btnSend.GetComponent<Image>().raycastTarget = false;
        }
    }

    void OnTimeChatWorld()
    {
        Sendtxt.text = m_worldLeftTime.ToString();
        m_worldLeftTime--;

        if (m_worldLeftTime <= 0)
        {
            Sendtxt.text = "发送";
            TimeManager.Instance.RemoveTask(OnTimeChatWorld);
            m_btnSend.GetComponent<Image>().raycastTarget = true;
            m_btnSend.interactable = true;
        }
    }

    void OnTimeInviteWorld()
    {
        InviteText.text = m_worldInviteTime.ToString();
        m_worldInviteTime--;
        if (m_worldInviteTime <= 0)
        {
            InviteText.text = "邀请来我家园";
            TimeManager.Instance.RemoveTask(OnTimeInviteWorld);
            DownPar.transform.Find("InviteBtn").GetComponent<Image>().raycastTarget = true;
            DownPar.transform.Find("InviteBtn").GetComponent<Button>().interactable = true;
        }
    }

    public void createSelfInvite()
    {
        GameObject InviteObj;
        if (ChatPar.transform.childCount >= 50)
        {
            DestroyImmediate(ChatPar.transform.GetChild(0).gameObject);
        }

        InviteObj = PublicFunc.CreateTmp(SelfInviteTmp, ChatPar.transform);
        PublicFunc.CreateHeadImg(InviteObj.transform.Find("HeadImage").GetComponent<Image>(),DataMgr.m_account.modleId);
        JumpToDown();
    }
    public void createSelfChat(string ChatMessage)
    {
        GameObject ChatObj;
        if (IF.text != "" && IF.text != null)
        {
            if (ChatPar.transform.childCount >= 50)
            {
                DestroyImmediate(ChatPar.transform.GetChild(0).gameObject);
            }
            ChatObj = PublicFunc.CreateTmp(SelfTmp, ChatPar.transform);
            PublicFunc.CreateHeadImg(ChatObj.transform.Find("HeadImage").GetComponent<Image>(), (long)DataMgr.m_account.modleId);
            ChatObj.transform.Find("ChatInfo").GetChild(0).GetComponent<Text>().text = ChatMessage;
            chatpanel.cp.UpdateDownChatText("我", ChatMessage);
        }
        JumpToDown();
        //VirtualCityMgr.UpdateWallet(- DataMgr.m_WorldChatConsumeGold, 0, 0, 0);
    }
    void clickHeadImage(GameObject obj)
    {
        chatpanel.cp.SendGetPlayerByIdMessage(long.Parse(obj.transform.parent.name));
    }
}
