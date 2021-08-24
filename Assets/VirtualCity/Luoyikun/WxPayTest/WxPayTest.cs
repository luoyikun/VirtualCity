using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WxPayTest : MonoBehaviour {
    
	// Use this for initialization
	

    IEnumerator Start()
    {


        WWW www = new WWW("https://wxpay.wxutil.com/pub_v2/app/app_pay.php");
        //WWW www = new WWW("https://www.baidu.com");
        yield return www;

        if (www.isDone)
        {
            Debug.Log(www.text);
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
