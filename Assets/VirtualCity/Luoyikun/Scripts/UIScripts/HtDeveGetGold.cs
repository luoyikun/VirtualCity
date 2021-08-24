using Framework.Event;
using Framework.UI;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HtDeveGetGold : MonoBehaviour {
    public Devlopments m_deveInfo;
    public GameObject m_btnClick;
    // Use this for initialization
    void Start () {
        ClickListener.Get(m_btnClick).onClick = OnBtnClick;
	}


    void OnBtnClick(GameObject obj)
    {
        if (PublicFunc.IsHomeTownMyOrFriend())
        {
            buildhometown.m_instance.SendReqGetRewardMessage(m_deveInfo.id);
            EventManager.Instance.DispatchEvent(Common.EventStr.PlayGetGold,new EventDataEx<int>(1));
            //NewGuideMgr.Instance.StartOneNewGuide();
        }
        else {
            Hint.LoadTips("您还不是TA的好友", Color.white);
        }
        
    }

    public void SetInfo(string id)
    {

        m_deveInfo = buildhometown.m_instance.m_dicDevlopment[id];

    }
	
}
