using System.Collections;
using System.Collections.Generic;
using Framework.UI;
using UnityEngine;

public class tishengedupanel : UGUIPanel
{
    public GameObject InviteBtn;
    public GameObject DaiLiQuanBtn;
    public GameObject BackBtn;

    void Start()
    {
        ClickListener.Get(InviteBtn).onClick = clickInviteBtn;
        ClickListener.Get(DaiLiQuanBtn).onClick = clickDaiLiQuanBtn;
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
    }

    public override void OnOpen()
    {
        //Hint.LoadTips("您的可提现额度不足");
    }

    public override void OnClose()
    {

    }

    public void Init(string text)
    {
        Hint.LoadTips(text);
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }

    void clickInviteBtn(GameObject obj)
    {
        string url = AppConst.m_shareUrl + DataMgr.m_account.commendCode;
        AndroidFunc.WxShareWebpageCross(url);
    }

    void clickDaiLiQuanBtn(GameObject obj)
    {
        if (DataMgr.m_account.hadProxy == 0)
        {
            UIManager.Instance.PushPanel(UIPanelName.dailiquanpanel, false, false,
                paragrm => { paragrm.GetComponent<dailiquanpanel>().OpenPanelWindows(0); });
        }
    }
}
