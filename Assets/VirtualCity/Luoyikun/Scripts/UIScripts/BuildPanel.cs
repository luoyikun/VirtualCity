using BitBenderGames;
using Framework.Event;
using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPanel : UGUIPanel {

	// Use this for initialization
	void Start () {
		
	}

    public void OnBtnBuildMode()
    {
        DataMgr.m_buildMode = EnBuildMode.Build;
        EventManager.Instance.DispatchEvent(Common.EventStr.BuildMode,new EventDataEx<bool>(true));
    }

    public void OnBtnDisplayMode()
    {
        DataMgr.m_buildMode = EnBuildMode.Display;
        EventManager.Instance.DispatchEvent(Common.EventStr.BuildMode, new EventDataEx<bool>(false));
    }
}
