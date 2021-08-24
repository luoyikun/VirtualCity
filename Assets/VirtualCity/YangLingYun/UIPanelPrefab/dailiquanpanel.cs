using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using UnityEngine.UI;
using DG.Tweening;
using ProtoDefine;
using SGF.Codec;
using Framework.Event;

public class dailiquanpanel : UGUIPanel {
    public GameObject Content;
    Text TuiJianRenAccountText;
    int PayType = -1;
    public void Init()
    {
         
    }
    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetProxyMessage, OnNetRspGPM);
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetProxyPayMessage,OnNetRspGPPM);
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            AliComponent.Instance.aliPayCallBack += AliPayCallback;
        }
    }
    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetProxyMessage, OnNetRspGPM);
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetProxyPayMessage, OnNetRspGPPM);
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            AliComponent.Instance.aliPayCallBack -= AliPayCallback;
        }
    }

    void OnNetRspGPPM(byte[] buf)
    {
        RspGetProxyPayMessage rspGetProxyPay = PBSerializer.NDeserialize<RspGetProxyPayMessage>(buf);
        if (rspGetProxyPay.code == 0)
        {
            Hint.LoadTips(rspGetProxyPay.tips, Color.white);
            return;
        }
        PayResult("1");
    }
    private void AliPayCallback(string result)
    {
        if (result == "true")
        {
            Debug.Log("支付宝支付成功");
            PayResult("1");
        }
        else
        {
            Debug.Log("支付宝支付失败");
            PayResult("0");

        }
        
    }


    void OnNetRspGPM(byte[] buf)
    {
        RspGetProxyMessage RspGPM = PBSerializer.NDeserialize<RspGetProxyMessage>(buf);
        if (RspGPM.code != 0)
        {
            if (PayType == 0)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    AndroidFunc.AliPay(RspGPM.payInfo, gameObject.name, "PayResult");
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    AliComponent.Instance.AliPay(RspGPM.payInfo);
                }
            }
            else if (PayType == 1)
            {
                Hint.LoadTips("微信支付暂未开通,请选择支付宝支付", Color.white);
            }
        }
        else if (RspGPM.code == 0)
        {
            Hint.LoadTips(RspGPM.tip, Color.white);
        }
    }
    public void PayResult(string str)
    {
        uiloadpanel.Instance.Close();
        int state = int.Parse(str);
        if (state == 0)
        {
            DataMgr.m_account.hadProxy = 0;
        }
        else if (state == 1)
        {
            OpenPanelWindows(2);
        }
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
    void clickTuiJianRenBtn(GameObject obj)
    {
        OpenPanelWindows(1);
    }
    public void OpenPanelWindows(int idx)
    {
        for (int i = 0; i < Content.transform.childCount; i++)
        {
            Content.transform.GetChild(i).gameObject.SetActive(false);
            if (i == Content.transform.childCount - 1)
            {
                Content.transform.GetChild(i).localScale = Vector3.zero;
            }
        }
        GameObject OpenWindow = Content.transform.GetChild(idx).gameObject;
        OpenWindow.SetActive(true);
        switch (idx)
        {
            case 0:
                OpenWindow.transform.Find("InputField").GetComponent<InputField>().text = "";
                TuiJianRenAccountText = OpenWindow.transform.Find("InputField").Find("Text").GetComponent<Text>();
                ClickListener.Get(OpenWindow.transform.Find("BG").Find("BackBtn").gameObject).onClick = clickBackBtn;
                ClickListener.Get(OpenWindow.transform.Find("Button").gameObject).onClick = clickTuiJianRenBtn;
                break;
            case 1:
                ClickListener.Get(OpenWindow.transform.Find("SelectPay").Find("AliPay").Find("SelectBtn").gameObject).onClick = clickSelectPayBtn;
                OpenWindow.transform.Find("SelectPay").Find("AliPay").Find("SelectBtn").GetChild(1).gameObject.SetActive(false);
                ClickListener.Get(OpenWindow.transform.Find("SelectPay").Find("WXPay").Find("SelectBtn").gameObject).onClick = clickSelectPayBtn;
                OpenWindow.transform.Find("SelectPay").Find("WXPay").Find("SelectBtn").GetChild(1).gameObject.SetActive(false);
                ClickListener.Get(OpenWindow.transform.Find("BG").Find("BackBtn").gameObject).onClick = clickBackBtn;
                ClickListener.Get(OpenWindow.transform.Find("Button").gameObject).onClick = clickGetProxy;
                clickSelectPayBtn(OpenWindow.transform.Find("SelectPay").Find("AliPay").Find("SelectBtn").gameObject);
                break;
            case 2:
                ClickListener.Get(OpenWindow).onClick -= clickBackBtn;
                DataMgr.m_account.hadProxy = 1;
                Tween move = DOTween.To(() => OpenWindow.GetComponent<Transform>().localScale, r => OpenWindow.GetComponent<Transform>().localScale = r,Vector3.one,0.5f);
                EventManager.Instance.DispatchEvent(Common.EventStr.CloseTiXian);
                move.OnComplete(()=> { GetClick(OpenWindow); });
                EventManager.Instance.DispatchEvent(Common.EventStr.UpdateMoneyTree, new EventDataEx<int>(1));
                //move.OnComplete(GetClick);
                break;
        }
    }
    void GetClick(GameObject obj)
    {
        ClickListener.Get(obj).onClick = clickBackBtn;
    }
    void clickSelectPayBtn(GameObject obj)
    {
        if (obj.transform.parent.name == "AliPay")
        {
            PayType = 0;
            obj.transform.GetChild(1).gameObject.SetActive(true);
            obj.transform.parent.parent.Find("WXPay").Find("SelectBtn").GetChild(1).gameObject.SetActive(false);
        }
        else if (obj.transform.parent.name == "WXPay")
        {
            PayType = 1;
            obj.transform.GetChild(1).gameObject.SetActive(true);
            obj.transform.parent.parent.Find("AliPay").Find("SelectBtn").GetChild(1).gameObject.SetActive(false);
        }
    }
    void clickGetProxy(GameObject obj)
    {
        ReqGetProxyMessage m_ReqGPM = new ReqGetProxyMessage();
        m_ReqGPM.code = TuiJianRenAccountText.text;
        m_ReqGPM.payType = PayType;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetProxyMessage, m_ReqGPM);
    }
}
