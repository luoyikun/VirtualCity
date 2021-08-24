using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HttpMgr : MonoSingleton<HttpMgr> {

    public void Httppost(string url, UnityAction<string> act,WWWForm data = null )
    {
        Debug.Log("Http:" + url);
        StartCoroutine(YieldPost(url, act, data));
    }

    IEnumerator YieldPost(string url, UnityAction<string> act, WWWForm data = null)
    {
        WWW www;
        if (data != null)
        {
            www = new WWW(url, data);
            yield return www;
        }
        else
        {
            www = new WWW(url);
            yield return www;
        }

        if (www.error == null)
        {
            act(www.text);
        }
        else
        {
            Hint.LoadTips(www.text, Color.white);
        }
    }
}
