using Framework.UI;
using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectserverpanel : UGUIPanel {

    public LeftScroll m_leftScroll;
    public LoopVerticalScrollRect m_loop;
    // Use this for initialization
    void Awake () {
        CreateLeft();
        //m_loop.totalCount = 20;
       // m_loop.RefillCells();
    }

    override public void  OnOpen()
    {
        //请求服务器列表
        //NetManager.Instance.SendMsg

        //CreateLeft();
    }

    public override void OnClose()
    {
        
    }


    // Update is called once per frame
    void Update () {
		
	}

    void CreateLeft()
    {
        m_leftScroll.Create(20);
        m_leftScroll.SetSelect(2);
    }
}
