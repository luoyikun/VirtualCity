using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SysNotifyMassage : MonoBehaviour {
    public MassageType m_MassgeType;
    public enum MassageType
    {
        SysNotification=0,
        AddFriend=1,
        Invatation=2
    }
    public SystemNotify m_systemnotify;

    public void Init()
    {

        ClickListener.Get(transform.Find("ChaKan/ChaKanBtn").gameObject).onClick = chatpanel.cp .clickChaKan;

        ClickListener.Get(transform.Find("HuLueBtn").gameObject).onClick = chatpanel.cp.clickHuLue;

        transform.Find("InfoText").GetComponent<Text>().text = m_systemnotify.Content;

        transform.Find("InfoTime").GetComponent<Text>().text = m_systemnotify.Createtime;

        transform.name = m_systemnotify.Id.ToString();

        if (m_systemnotify.Type != null)
        {
            switch (m_systemnotify.Type)
            {
                case SocialityDataPool.OTHERS:
                    m_MassgeType = SysNotifyMassage.MassageType.SysNotification;

                    transform.Find("ChaKan/ChaKanBtn").GetChild(0).GetComponent<Text>().text = "忽略";

                    transform.Find("ChaKan/ChaKanText").GetComponent<Text>().text = "已忽略";

                    transform.Find("HuLueBtn").gameObject.SetActive(false);
                    break;
                case SocialityDataPool.ADDFRIEND:
                    m_MassgeType = SysNotifyMassage.MassageType.AddFriend;

                    transform.Find("ChaKan/ChaKanBtn").GetChild(0).GetComponent<Text>().text = "同意";

                    transform.Find("ChaKan/ChaKanText").GetComponent<Text>().text = "已同意";
                    break;
                case SocialityDataPool.ADDGROUP:
                    m_MassgeType = SysNotifyMassage.MassageType.Invatation;

                    transform.Find("ChaKan/ChaKanBtn").GetChild(0).GetComponent<Text>().text = "同意";

                    transform.Find("ChaKan/ChaKanText").GetComponent<Text>().text = "已同意";
                    break;
            }
        }
    }
}
