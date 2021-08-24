using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;

public class dailipanel : UGUIPanel {
    public GameObject BG;
	// Use this for initialization
	void Start () {
        ClickListener.Get(BG).onClick = clickBG ;
	}
    void clickBG(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
