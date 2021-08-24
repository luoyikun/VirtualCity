using BE;
using Framework.Event;
using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCtrlPanel : UGUIPanel {

    Transform m_trans;
	// Use this for initialization
	void Start () {
        m_trans = this.transform;

    }
	
	// Update is called once per frame
	void Update () {

        if (SceneTown.buildingSelected != null)
        {
            gameObject.SetActive(true);
            Vector2 player2DPosition = Camera.main.WorldToScreenPoint(SceneTown.buildingSelected.transform.position);
            m_trans.position = player2DPosition;
        }
        else {
            gameObject.SetActive(false);
        }
        

    }

    public void OnBtnClickOk()
    {
        gameObject.SetActive(false);
        EventManager.Instance.DispatchEvent(Common.EventStr.ModelPlaceOK);
    }

    public void OnBtnClickRotate()
    {
        EventManager.Instance.DispatchEvent(Common.EventStr.ModelRotate);
    }
}
