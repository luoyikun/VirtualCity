using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using ProtoDefine;
using SGF.Codec;
using UnityEditor;

public class VCUITest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //AssetMgr.Instance.Init(Init);
       // MessageCenter.Instance.StartUp();
        Init();
    }
    private void OnEnable()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspCommentMessage, OnEvNetCommentMessage);
    }
    private void OnDisable()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspCommentMessage, OnEvNetCommentMessage);
    }
    void OnEvNetCommentMessage(byte[] buf)
    {
        RspCommentMessage RspCM = PBSerializer.NDeserialize<RspCommentMessage>(buf);
        switch (RspCM.rspcmd)
        {
            case 505:
                ispanel IsPanel= (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, true);
                //IsPanel.m_ok = () => Application.Quit();
#if UNITY_EDITOR
                IsPanel.m_ok = () => EditorApplication.isPlaying = false;
                IsPanel.m_cancel = () => EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
    }
    void Init()
    {
       //UIManager.Instance.PushPanel(UIPanelName.loginpanel);
    }
    void OnApplicationQuit()
    {
        GameSocket.Instance.Close();
        ChatSocket.Instance.Close();
        HallSocket.Instance.Close();
    }
    // Update is called once per frame
    void Update () {
		
	}
}
