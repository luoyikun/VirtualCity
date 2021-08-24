using Framework.Event;
using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSysCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //AssetMgr.Instance.Init(Init);
        DataMgr.m_buildMode = EnBuildMode.Build;
        EventManager.Instance.DispatchEvent(Common.EventStr.BuildMode, new EventDataEx<bool>(true));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Init()
    {
        //UIManager.Instance.PushPanel(UIPanelName.buildpanel, UIManager.CanvasType.Screen);
        //UIManager.Instance.PushPanel(UIPanelName.buildpanel);
    }
}
