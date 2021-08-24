using System.Collections;
using System.Collections.Generic;
using Framework.Event;
using Framework.UI;
using ProtoDefine;
using UnityEngine;

public class MoneyTreeCtrl : MonoBehaviour
{
    private GameObject CurUI;
    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    public void UpdateButton(int state)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        switch (state)
        {
            case 0:
                CurUI = transform.GetChild(0).gameObject;
                CurUI.SetActive(true);
                ClickListener.Get(CurUI).onClick = clickGetProxy;
                break;
            case 1:
                CurUI = transform.GetChild(2).gameObject;
                CurUI.SetActive(true);
                ClickListener.Get(CurUI).onClick = clickDebugHasProxy;
                break;
            case 2:
                CurUI=transform.GetChild(1).gameObject;
                CurUI.SetActive(true);
                ClickListener.Get(CurUI).onClick = clickGetMoney;
                break;
            case 3:
                CurUI = transform.GetChild(3).gameObject;
                CurUI.SetActive(true);
                break;
        }
    }
    void clickGetProxy(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.dailiquanpanel, false, true, paragrm => { paragrm.GetComponent<dailiquanpanel>().OpenPanelWindows(0); });
    }

    void clickDebugHasProxy(GameObject obj)
    {
        Hint.LoadTips("请耐心等待三天哦", Color.white);
    }

    void clickGetMoney(GameObject obj)
    {
        if (DataMgr.m_myOther == EnMyOhter.My)
        {
            ReqRewardMoneyTreeMessage reqRMTM = new ReqRewardMoneyTreeMessage();
            reqRMTM.accountId = DataMgr.m_accountId;
            GameSocket.Instance.SendMsgProto(MsgIdDefine.ReqRewardMoneyTreeMessage, reqRMTM);
            EventManager.Instance.DispatchEvent(Common.EventStr.UpdateMoneyTree, new EventDataEx<int>(-1));
        }
    }
}
