using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class webdisplaypanel : UGUIPanel
{
    GameObject obj;
    UniWebView webView;
    public static int Type=0;
    public GameObject forum;
    public GameObject game;
    public override void OnOpen()
    {
        switch (Type)
        {
            case 0:
                forum.SetActive(true);
                game.SetActive(false);
                open(DataMgr.m_forum_url);
                break;
            case 1:
                forum.SetActive(false);
                game.SetActive(true);
                open(@"http://47.99.99.79:8082/game/game.html");
                break;
        }
    }
    public override void OnClose() { }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void open(string str)
    {
        obj = new GameObject();
        obj.name = "UniWebView";
        obj.transform.parent = transform;
        webView=obj.AddComponent<UniWebView>();
        Debug.Log(Screen.width.ToString()+ ":"+Screen.height.ToString());
        webView.Show();
        webView.Load(str);

        switch (Type)
        {
            case 0:
                webView.Frame = new Rect(new Vector2(0, 0), new Vector2(Screen.width / DataMgr.m_resolution, Screen.height / DataMgr.m_resolution - 100));
                break;
            case 1:
                Vector2 size = game.GetComponent<RectTransform>().sizeDelta * PublicFunc.GetHeightFactor();
                webView.Frame = new Rect(new Vector2(0,size.y), new Vector2(Screen.width /  DataMgr.m_resolution, Screen.height / DataMgr.m_resolution - size.y));
                Debug.Log(DataMgr.m_designHeight.ToString() + PublicFunc.GetWidthFactor().ToString() + PublicFunc.GetHeightFactor().ToString());
                break;
        }
        Debug.Log(webView.gameObject.transform.position);
        Debug.Log("分辨率缩放比例是"+ DataMgr.m_resolution.ToString());
    }

    public void back()
    {
        webView.GoBack();
    }

    public void Forward()
    {
        webView.GoForward();
    }

    public void webRefresh()
    {
        webView.Reload();
    }

    public void share()
    {
        AndroidFunc.WxShareWebpageCross(DataMgr.m_forum_url);
        Debug.Log("分享");
    }
    public void back_()
    {
        webView.CleanCache();
        UniWebView.ClearCookies();
        Destroy(obj);
        UIManager.Instance.PopSelf(false);
        homepanel.m_instance.m_timeio = false;
        homepanel.m_instance.m_btnTiXian.SetActive(true);
    }
}
