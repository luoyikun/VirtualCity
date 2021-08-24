using System.Collections;
using System.Collections.Generic;
using Framework.UI;
using ProtoDefine;
using SGF.Codec;
using UnityEngine;
using UnityEngine.UI;

public class tuijianpanel : UGUIPanel
{
    public InputField TuiJianMaIF;
    public GameObject QueRenBtn;
    public GameObject CancelBtn;
    public void Start()
    {
        ClickListener.Get(QueRenBtn).onClick = clickQueRenBtn;
        ClickListener.Get(CancelBtn).onClick = clickCancelBtn;
    }

    void clickCancelBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf(false);
    }
    public override void OnOpen()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspRegisterCodeMessage,OnNetRspRCM);
    }

    public override void OnClose()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspRegisterCodeMessage, OnNetRspRCM);
    }

    void OnNetRspRCM(byte[] buf)
    {
        RspRegisterCodeMessage rspRegisterCode = PBSerializer.NDeserialize<RspRegisterCodeMessage>(buf);
        if (rspRegisterCode.code == 0)
        {
            Hint.LoadTips(rspRegisterCode.tips, Color.white);
            return;
        }
        UIManager.Instance.PopSelf(false);
        
    }
    void clickQueRenBtn(GameObject obj)
    {
        if (TuiJianMaIF.text == "")
        {
            Hint.LoadTips("请输入正确的邀请码", Color.white);
            //UIManager.Instance.PopSelf(false);
            return;
        }
        ReqRegisterCodeMessage reqRegisterCode=new ReqRegisterCodeMessage();
        reqRegisterCode.code = TuiJianMaIF.text;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqRegisterCodeMessage,reqRegisterCode);
    }
}
