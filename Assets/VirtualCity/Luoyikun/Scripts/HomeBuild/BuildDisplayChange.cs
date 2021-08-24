using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDisplayChange : MonoBehaviour {

    public GameObject m_btnBuild;
    public GameObject m_btnDisplay;
	// Use this for initialization
	void Start () {
        ClickListener.Get(m_btnBuild).onClick = OnBtnBuild;
        ClickListener.Get(m_btnDisplay).onClick = OnBtnDisplay;

    }

    void OnBtnBuild(GameObject obj)
    {
        HomeMgr.m_instance.ChangeToBuild();
    }

    void OnBtnDisplay(GameObject obj)
    {
        HomeMgr.m_instance.ChangeToDisplay();
    }
}
