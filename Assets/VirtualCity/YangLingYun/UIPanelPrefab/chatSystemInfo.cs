using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chatSystemInfo : MonoBehaviour {
    public GameObject InfoPar;
    public GameObject InfoTmp;
	// Use this for initialization
	void Start () {
        InfoPar = this.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        InfoTmp = this.transform.GetChild(0).gameObject;
        for (int i = 0; i < 30; i++)
        {
            GameObject obj = PublicFunc.CreateTmp(InfoTmp, InfoPar.transform);
            ClickListener.Get(obj.transform.Find("ChaKan/ChaKanBtn").gameObject).onClick = clickChaKanBtn;
        }
	}
    void clickChaKanBtn(GameObject obj)
    {
        obj.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
