using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaLaBa : MonoBehaviour {
    public GameObject BackBtn;
    public GameObject SendBtn;
	// Use this for initialization
	void Start () {
        ClickListener.Get(BackBtn).onClick = clickBackBtn;
        ClickListener.Get(SendBtn).onClick = clickSendBtn;
    }
    void clickBackBtn(GameObject obj)
    {
        chatwindowspanel.cwp.clickBackBtn(obj);
    }
    void clickSendBtn(GameObject obj)
    {
        chatwindowspanel.cwp.clickBackBtn(obj);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
