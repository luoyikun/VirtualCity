using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProtoDefine;
using Net;
using SGF.Codec;
using Framework.UI;

public class CreateGroup : MonoBehaviour {
    public GameObject BackBtn;
    public GameObject CreateBtn;
    public InputField IF;
    bool IsReq = false;
    bool IsRspOver = false;
    // Use this for initialization
    private void OnEnable()
    {
        IF.text = "";
        //NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, OnEvNetGroupMessage);
        //NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetSocialityInfoMessage, OnEvNetGetSocialityInfoMessage);
    }
    void Start () {
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        ClickListener.Get(CreateBtn).onClick = clickCreateBtn;
    }
    void clickBackBtn(GameObject obj)
    {
        chatwindowspanel.cwp.clickBackBtn(obj);
    }
    void clickCreateBtn(GameObject obj)
    {
        if (IF.text == "" || IF.text == null)
        {
            Hint.LoadTips("名称不能为空",Color.white);
        }
        ReqCreateGroupMessage ReqCGM = new ReqCreateGroupMessage();
        ReqCGM.groupName = IF.transform.Find("Text").GetComponent<Text>().text;
        ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqCreateGroupMessage, ReqCGM, EnSocket.Chat);
    }
    void OnEvNetGetSocialityInfoMessage(byte[] buf)
    {
        //chatpanel.cp.clickChatLeftBtn(chatpanel.cp.LeftMenuPar.transform.GetChild(1).gameObject);
       // chatwindowspanel.cwp.clickBackBtn(CreateBtn);
    }
    void ReqRsp()
    {
        if (IsRspOver == true)
        {
            //chatpanel.cp.ReqGetSociality();
            chatwindowspanel.cwp.clickBackBtn(BackBtn);
            //chatpanel.cp.init
        }
    }
    private void OnDisable()
    {
        //NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, OnEvNetGroupMessage);
        //NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetSocialityInfoMessage, OnEvNetGetSocialityInfoMessage);
    }

    void OnEvNetGroupMessage(byte[] buf)
    {
        RspCommentMessage pro = PBSerializer.NDeserialize<RspCommentMessage>(buf);
        if (pro.rspcmd == 506)
        {
            if (pro.code == 0)
            {
                IsRspOver = false;
                Hint.LoadTips(pro.tip, Color.white);
                Debug.Log("false");
            }
            else if (pro.code == 1)
            {
                IsRspOver = true;
                ReqRsp();
                Debug.Log("创建群成功");
            }
        }
    }
}
