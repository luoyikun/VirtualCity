using System.Collections;
using System.Collections.Generic;
using ProtoDefine;
using UnityEngine;
using UnityEngine.UI;

public class ChatType : MonoBehaviour
{
    public ChatGroup TareChatGroup;
    public ChatUser TargetChatUser;
    public enum TypeTag
    {
        SystemNews,
        OwnGroup,
        InGroup,
        WorldChatNews,
        Proxy,
        PersonChat,
        OfflinePersonChat,
        SearchObj
    }
    public TypeTag m_tag;

    public static void UpdateInfo()
    {

        Debug.Log("更新了某一个社交信息");
    }

    public void UpdateInfo(ChatGroup m_ChatGroup)
    {

        TareChatGroup = m_ChatGroup;
        transform.Find("Name").GetComponent<Text>().text = m_ChatGroup.Name;
    }

    public void UpdateInfo(ChatUser m_ChatUser)
    {
        TargetChatUser = m_ChatUser;
        //transform.Find("Name").GetComponent<Text>().text = m_ChatUser.userName;
    }
}
