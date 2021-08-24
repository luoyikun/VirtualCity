using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnWxShareType
{
    WXSceneSession = 0,
    WXSceneTimeline = 1,
    WXSceneFavorite = 2
}
public class AndroidFunc {
    public static void CallPhone(string phone)
    {
        AndroidJavaObject pluginObject = new AndroidJavaClass("com.luoyikun.call.MyCall").CallStatic<AndroidJavaObject>("GetInstance");
        pluginObject.Call("CallPhone", phone);
    }


    public static void CopyToB(string phone)
    {
        AndroidJavaObject pluginObject = new AndroidJavaClass("com.luoyikun.clipboard.MyClipboard").CallStatic<AndroidJavaObject>("GetInstance");
        pluginObject.Call("CopyToClipboard", phone);
    }

    public static void InstallApk(string path)
    {
        AndroidJavaObject pluginObject = new AndroidJavaClass("com.luoyikun.installapk.InstallApk").CallStatic<AndroidJavaObject>("GetInstance");
        pluginObject.Call("installApp", path);
    }

    public static void AliPay(string info,string gameObjectName = "", string funcName = "")
    {
        AndroidJavaObject pluginObject = new AndroidJavaClass("com.luoyikun.myalipay.MyAliPay").CallStatic<AndroidJavaObject>("GetInstance");
        pluginObject.Call("UnityFunc", gameObjectName, funcName);
        pluginObject.Call("PayING", info);
    }

    public static void WxShareText(string text,EnWxShareType type)
    {
        AndroidJavaObject pluginObject = new AndroidJavaClass("com.luoyikun.mywxshare.MyWxShare").CallStatic<AndroidJavaObject>("GetInstance");
        //pluginObject.Call("regToWx");
        pluginObject.Call("shareText", text,(int)type);
    }

    public static void WxShareWebpage(string url,string title,string descr,string img, EnWxShareType type = EnWxShareType.WXSceneSession)
    {
        AndroidJavaObject pluginObject = new AndroidJavaClass("com.luoyikun.mywxshare.MyWxShare").CallStatic<AndroidJavaObject>("GetInstance");
        //pluginObject.Call("regToWx");
        pluginObject.Call("shareWebpage", url,title,descr,img, (int)type);
    }

    public static void WxShareWebpageCross(string url)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidFunc.WxShareWebpage(url, "睡鹰科技：S界",  DataMgr.m_account.userName  + ":\r\n" + "邀请您一起来助力S界", "");
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            byte[] buf = new byte[0];
            WeChatComponent.Instance.WeChatShare_WebPage(0, url, "睡鹰科技：S界",  DataMgr.m_account.userName + ":\r\n" + "邀请您一起来助力S界", buf);
        }
    }
}
