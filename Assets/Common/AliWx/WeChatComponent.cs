using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WeChatComponent : MonoSingleton<WeChatComponent>
{

    public string WXAppID = "wx09f530f1e33f2cb0";
    public string WXAppSecret = "9c789be0809b1353f2fdd08e6fbaae95";

    public bool isRegisterToWechat = false;

    AndroidJavaClass javaClass;
    AndroidJavaObject javaActive;
    public string javaClassStr ="com.unity3d.player.UnityPlayer";
    public string javaActiveStr = "currentActivity";

    public string WeChatCallObjName = "WeChatComponent";

    //事件原型
    public delegate void WeChatLogonCallback(WeChatUserData userData);//微信登录回调 userData:null表示登陆了失败 否则表示成功,携带用户公开的信息
    public delegate void WeChatShareCallback(string result);//分享结果 result:ERR_OK(由于业务调整,现在始终返回分享成功)
    public delegate void WeChatPayCallback(int state);//微信充值回调  result 0:成功 -1失败 -2取消

    public WeChatLogonCallback weChatLogonCallback;
    public WeChatShareCallback weChatShareTextCallback, weChatShareImageCallback, weChatShareWebPageCallback;
    public WeChatPayCallback weChatPayCallback;

   
#if UNITY_EDITOR
#elif UNITY_ANDROID
#elif UNITY_IPHONE

    
    //注册微信IOS
    [DllImport("__Internal")]
    static extern void IOS_RegisteredWeChat(string appId);

        //是否安装了微信
    [DllImport("__Internal")]
    static extern bool IOS_IsInstallWechat();
    
    //是否支持API调度的微信版本
     [DllImport("__Internal")]
    static extern bool IOS_IsWeChatSupportApi();

    //微信登录
    [DllImport("__Internal")]
    static extern void IOS_WeChatLogon(string state);

    //微信支付
    [DllImport("__Internal")]
    static extern void IOS_WechatPay(string appId, string partnerId, string prepayId, string nonceStr, int timeStamp, string packageValue, string sign);
    
    //分享场景:0 = 好友列表 1 = 朋友圈 2 = 收藏  图片 大小 缩略图 缩略图大小

    //分享文字
     [DllImport("__Internal")]
    static extern void IOS_WeChatShareText(int scene, string content);
    
    //分享图片 
    [DllImport("__Internal")]
    static extern void IOS_WeChatShareImage(int scene, byte[] ptr, int size, byte[] ptrThumb, int sizeThumb);
    
   
    //分享网页 场景 url 标题 内容 缩略图字节 字节长度
    [DllImport("__Internal")]
    static extern void IOS_WeChatShareWebPage(int scene, string url, string title, string content,byte[] ptrThumb, int sizeThumb);
    
    
#endif




    void Start () {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        //初始化 获得项目对应的MainActivity
        javaClass = new AndroidJavaClass(javaClassStr);
        javaActive = javaClass.GetStatic<AndroidJavaObject>(javaActiveStr);
#elif UNITY_IPHONE
#endif
        RegisterAppWechat();
    }

    /// <summary> 微信初始化:注册ID </summary>
    public void RegisterAppWechat()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        //安卓端已经在java层做了 这里忽略 
        if (!isRegisterToWechat)
        {
            javaActive.Call("WechatInit", WXAppID);
        }
        isRegisterToWechat=true;
#elif UNITY_IPHONE
         if (!isRegisterToWechat)
        {
            IOS_RegisteredWeChat(WXAppID);
            isRegisterToWechat = true;
        }
#else
       return; 
#endif
    }

    /// <summary> 是否安装了微信 </summary>
    public bool IsWechatInstalled()
    {

#if UNITY_EDITOR
        return false;
#elif UNITY_ANDROID
        return javaActive.Call<bool>("IsWechatInstalled");
#elif UNITY_IPHONE
        return IOS_IsInstallWechat()&&IOS_IsWeChatSupportApi();
#else 
        return false;
#endif
    }

    /// <summary>微信登录 </summary>
    public void WeChatLogon( string state)
    {

#if UNITY_EDITOR
#elif UNITY_ANDROID
        object[] objs = new object[] { WXAppID, state, WeChatCallObjName, "LogonCallback" };
        javaActive.Call("LoginWechat", objs);
#elif UNITY_IPHONE
        IOS_WeChatLogon(state);
#endif
    }

    /// <summary>微信充值</summary>
    public void WeChatPay(string appid, string mchid, string prepayid, string noncestr, string timestamp, string sign)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        //将服务器返回的参数 封装到object数组里 分别是:会话ID,随机字符串,时间戳,签名,支付结果通知回调的物体,物体上的某个回调函数名称
        object[] objs = new object[] { appid, mchid, prepayid, noncestr, timestamp, sign, WeChatCallObjName, "WechatPayCallback" };
        //调用安卓层的WeiChatPayReq方法 进行支付
        javaActive.Call("WeChatPayReq", objs);
#elif UNITY_IPHONE
         IOS_WechatPay(appid, mchid, prepayid, noncestr, int.Parse(timestamp), "Sign=WXPay", sign);
#endif
    }


    /// <summary>微信分享网页 </summary>
    public void WeChatShare_WebPage(int scene, string url, string title, string content, byte[] thumb)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        // ShareWebPage(scene, @"http://www.baidu.com/", "这个参数是标题", "这个参数是身体部分", getThumb());
        object[] objs = new object[] { scene, url, title,content, thumb,WeChatCallObjName,"ShareWebPage_Callback"};
        javaActive.Call("WXShareWebPage", objs);
#elif UNITY_IPHONE
        IOS_WeChatShareWebPage(scene,url,title,content,thumb,thumb.Length);
#endif
    }

    /// <summary>微信分享图片 场景:0 = 好友列表 1 = 朋友圈 2 = 收藏  图片 大小 </summary>
    public void WeChatShare_Image(int scene, byte[] imgData, byte[] thumbData)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        object[] objs = new object[] { scene, imgData, thumbData, WeChatCallObjName, "ShareImage_Callback" };
        javaActive.Call("WXShareImage", objs);
#elif UNITY_IPHONE
        IOS_WeChatShareImage(scene,  imgData, imgData.Length,thumbData, thumbData.Length);
#endif
    }

    //--------------------------回调接口---------------------------------//

    /// <summary> 登录回调 </summary>
    public void LogonCallback(string str)
    {
        if (str != "用户拒绝授权" && str != "用户取消授权")
        {
            Debug.Log("微信登录,用户已授权:" + str);
            StartCoroutine(GetWeChatUserData(WXAppID, WXAppSecret, str));
        }

        if (str == "用户拒绝授权" || str == "用户取消授权")
        {
            Debug.Log("微信登录," + str);
            weChatLogonCallback(null);
        }
    }

    //微信SDK的业务调整 现在的分享是无法获取分享成功还是失败:
    //现在的分享业务始终返回success(成功)状态
    //公告链接:https://open.weixin.qq.com/cgi-bin/announce?action=getannouncement&key=11534138374cE6li&version=&lang=zh_CN&token=


    /// <summary> 分享图片回调 </summary>
    public void ShareImage_Callback(string code)
    {
        //ERR_OK = 0(用户同意) ERR_AUTH_DENIED = -4（用户拒绝授权） ERR_USER_CANCEL = -2（用户取消）
        switch (code)
        {
            case "ERR_OK":
                Debug.Log("分享成功");
                break;
            case "ERR_AUTH_DENIED":
            case "ERR_USER_CANCEL":
                Debug.Log("用户取消分享");
                break;
            default:
                break;
        }
        weChatShareImageCallback(code);

    }

    public void ShareWebPage_Callback(string code)
    {
        switch (code)
        {
            case "ERR_OK":
                Debug.Log("wx:网页分享成功");
                break;
            case "ERR_AUTH_DENIED":
            case "ERR_USER_CANCEL":
                Debug.Log("wx:用户取消分享网页");
                break;
            default:
                break;
        }
        weChatShareWebPageCallback(code);
    }



    /// <summary> 微信支付回调 </summary>
    public void WechatPayCallback(string retCode)
    {
        int state = int.Parse(retCode); 
        switch (state)
        {
            case -2:
                Debug.Log("支付取消");

                break;
            case -1:
                Debug.Log("支付失败");
             
                break;
            case 0:
                Debug.Log("支付成功");
          
                break;
        }
        weChatPayCallback(state);
    }

    /// <summary>
    /// 请求微信用户公开的信息
    /// </summary>
    /// <param name="appid">应用id</param>
    /// <param name="secret">应用秘密指令</param>
    /// <param name="code">微信客户端返回的code</param>
    /// <returns></returns>
    IEnumerator GetWeChatUserData(string appid, string secret, string code)
    {
        //第一步:通过appid、secret、code去获取请求令牌以及openid;
        //code是用户在微信登录界面授权后返回的
        string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" 
            + appid + "&secret=" + secret + "&code=" + code + "&grant_type=authorization_code";

        WWW www = new WWW(url);
        yield return www;

        if (www.error != null)
        {
            Debug.Log("微信登录请求令牌失败:" + www.error);
        }
        else
        {
            Debug.Log("微信登录请求令牌成功:" + www.text);
            WeChatData weChatData = JsonUtility.FromJson<WeChatData>(www.text);
            if (weChatData == null)
            {
                yield break;
            }
            else
            {
                //第二步:请求个人微信公开的信息
                string getuserurl = "https://api.weixin.qq.com/sns/userinfo?access_token=" 
                    + weChatData.access_token + "&openid=" + weChatData.openid;
                WWW getuser = new WWW(getuserurl);
                yield return getuser;
                if (getuser.error != null)
                {
                    Debug.Log("向微信请求用户公开的信息,出现异常:" + getuser.error);
                }
                else
                {
                    //从json格式的数据中反序列化获取
                    WeChatUserData weChatUserData = JsonUtility.FromJson<WeChatUserData>(getuser.text);
                    if (weChatUserData == null)
                    {
                        Debug.Log("error:"+"用户信息反序列化异常");
                        yield break;
                    }
                    else
                    {
                        Debug.Log("用户信息获取成功:"+getuser.text);
                        Debug.Log("openid:" + weChatUserData.openid + ";nickname:" + weChatUserData.nickname);
                 
                        //获取到微信的openid与昵称
                        string wxOpenID = weChatUserData.openid;
                        string wxNickname = weChatUserData.nickname;
                        int sex = weChatUserData.sex;//0-女 1-男  //sex	普通用户性别，1为男性，2为女性
                        
                        //微信登录 外部要处理的事件
                        weChatLogonCallback(weChatUserData);
                        
                    }
                }
            }
        }
    }
}

/// <summary> 用户授权后 微信返回的数据  </summary>
[System.Serializable]
public class WeChatData
{
    //使用token进一步获取个人公开的资料
    public string access_token;
    public string expires_in;
    public string refresh_token;
    public string openid;
    public string scope;
}

[System.Serializable]
public class WeChatUserData
{
    public string openid;//用户唯一ID
    public string nickname;//昵称
    public int sex;//性别
    public string province;//省份
    public string city;//城市
    public string country;//县级
    public string headimgurl;//头像Url
    public string[] privilege;//用户特权
    public string unionid;//会员
}
