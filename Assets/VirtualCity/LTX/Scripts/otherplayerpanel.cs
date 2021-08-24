using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class otherplayerpanel : UGUIPanel
{
    public GameObject back_but;
    public GameObject iocn_tx;
    public Text[] num_arr;


    // Use this for initialization
    void Start () {
        ClickListener.Get(back_but).onClick = back_;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void OnOpen() {}
    public override void OnClose(){}


    void division_str(double num,Text m_text)
    {
        Debug.Log(num.ToString());
        string[] ac = num.ToString().Split('.');
        if (ac.Length > 1)
        {
            Debug.Log(ac[1]);
            m_text.text = string.Format(string.Format("{0:N0}", num)+"."+ ac[1]);
        }
        else
        {
            Debug.Log(num);
            m_text.text = string.Format("{0:N0}", num);
        }
    }

    void back_(GameObject obj)
    {
        UIManager.Instance.PopSelf();
    }
}
