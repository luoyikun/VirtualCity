using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using LitJson;
using SGF.Codec;
using ProtoDefine;
using SuperScrollView;

public class billflowpanel : UGUIPanel
{
    public GameObject backBtn;
    //public int State;
    public static billflowpanel BFP;
    int m_State;
    public string m_LastDate = null;
    public LoopListView2 ScrollView;
    private bool IsInitScrollView;
    public override void OnOpen()
    {
        BFP = this;
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspQueryBillMessage, OnNetRspQBM);
    }

    public override void OnClose()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspQueryBillMessage, OnNetRspQBM);
    }

    void OnNetRspQBM(byte[] buf)
    {
        RspQueryBillMessage RspQBM = PBSerializer.NDeserialize<RspQueryBillMessage>(buf);
        if (RspQBM.code != 0)
        {
            if (RspQBM.bills != null)
            {
                allbillscroll.ABS.Init(RspQBM);
            }
            else
            {
                Debug.Log("bills为null");
                //allbillscroll.ABS.MonthsJianShao();
                //SendReqQBM(m_State);
                //Hint.LoadTips("本月无账单", Color.white);
            }
        }
        else if (RspQBM.code == 0) 
        {
            allbillscroll.ABS.MonthsJianShao();
            //SendReqQBM(m_State);
            //Hint.LoadTips("本月无账单", Color.white);
            //Hint.LoadTips(RspQBM.tip, Color.white);
        }
        //m_RQBM = RspQBM;
    }

    void UpdateScrollView()
    {

    }

    void UpdateTitalTime()
    {

    }

    public void SendReqQBM(int State)
    {
        ReqQueryBillMessage ReqQBM = new ReqQueryBillMessage();
        m_State = State;
        ReqQBM.billType = State;
        ReqQBM.lastDate = m_LastDate;
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqQueryBillMessage, ReqQBM);
    }
    // Use this for initialization
    void Start()
    {
        ClickListener.Get(backBtn).onClick = clickBackBtn;
    }
    void clickBackBtn(GameObject obj)
    {
        UIManager.Instance.PopSelf(true);
    }

    public void Init()
    {

    }
}
