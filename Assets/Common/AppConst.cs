using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public enum EnTestServer
{
    InTest,//内网测试
    OutTest,//外网测试
    Out//外网正式
}
public class AppConst {
    //打包用切换内外网
    public const EnTestServer m_enTestServer = EnTestServer.OutTest;

    public const string m_shareUrl = "http://shuiyingkeji.com:8081/game/index.html?code=";
    public const bool m_testForUiNoNet = true;
    public const string shangyejie = "shangyejie";
    public const string buildhometown = "buildhometown";
    public const string homemgr = "homemgr";

    // 切换内外网编辑器用
    public static EnTestServer m_enEditorServer = EnTestServer.InTest;
    //public static int m_inOrOutInEditor = -1;
    //public const string m_switchMenuInOrOut = "SwitchTool/IsOut";
    public const string m_switchMenuIn = "切换服务器/内网";
    public const string m_switchMenuOut = "切换服务器/外网";
    public const string m_switchMenuOutTest = "切换服务器/外网测试";

    public static string m_newHallIp = "";
    public static int m_newHallPort = 0;

    public static string m_newChatIp = "";
    public static int m_newChatPort = 0;

    public static bool m_isDebug = false;

#if UNITY_EDITOR
    public static EnTestServer EditorServer
    {
        get
        {
            if (EditorPrefs.GetBool(m_switchMenuIn,false))
            {
                m_enEditorServer = EnTestServer.InTest;
            }

            if (EditorPrefs.GetBool(m_switchMenuOut,false))
            {
                m_enEditorServer = EnTestServer.Out;
            }

            if (EditorPrefs.GetBool(m_switchMenuOutTest,false))
            {
                m_enEditorServer = EnTestServer.OutTest;
            }
           

            return m_enEditorServer;
        }
        set
        {
            m_enEditorServer = value;
            //int newValue = value ? 1 : 0;
            //if (newValue != AppConst.m_inOrOutInEditor)
            //{
            //    AppConst.m_inOrOutInEditor = newValue;
            //    EditorPrefs.SetBool(m_switchMenuInOrOut, value);
            //}
        }
    }
#endif

    public const string ImageHeadUrl = "http://virtualcity.shuiyingkeji.com/virtualCity/picture/";

    public string m_ipGame;
    public string m_portGame;

    public const bool m_isUpdateRes = true; // 是否从服务器更新资源
  
    
    public const bool DebugMode = true;                       //调试模式-用于内部测试，执行热更新用
    /// <summary>
    /// 如果想删掉框架自带的例子，那这个例子模式必须要
    /// 关闭，否则会出现一些错误。
    /// </summary>
    public const bool ExampleMode = true;                       //例子模式 

    /// <summary>
    /// 如果开启更新模式，前提必须启动框架自带服务器端。
    /// 否则就需要自己将StreamingAssets里面的所有内容
    /// 复制到自己的Webserver上面，并修改下面的WebUrl。
    /// </summary>
    public const bool UpdateMode = true;                       //更新模式-默认关闭 
    public const bool AutoWrapMode = true;                      //自动添加Wrap模式

    public const int TimerInterval = 1;
    public const int GameFrameRate = 30;                       //游戏帧频

    public const bool UsePbc = true;                           //PBC
    public const bool UseLpeg = true;                          //LPEG
    public const bool UsePbLua = true;                         //Protobuff-lua-gen
    public const bool UseCJson = true;                         //CJson
    public const bool UseSproto = true;                        //Sproto
    public const bool LuaEncode = false;                        //使用LUA编码

    public const string AppName = "CloudJiaJu";           //应用程序名称
    public const string AppPrefix = AppName + "_";             //应用程序前缀
    public const string ExtName = ".assetbundle";              //素材扩展名
    public const string AssetDirname = "StreamingAssets";      //素材目录 
    //public const string WebUrl = "http://localhost:6688/";      //测试更新地址
    public static string WebUrlOri = "http://www.ardezparklive.com/zspace/KeJunAR";
    public static string WebUrlVideo = "http://www.ardezparklive.com/zspace/KeJunAR/Video";

    public static string UserId = string.Empty;                 //用户ID
    public static int SocketPort = 0;                           //Socket服务器端口
    public static string SocketAddress = string.Empty;          //Socket服务器地址
	public static bool IsCloud = true;
    public static  Byte m_abKey = 157;               //ab加密

    public const string m_outDownUri = "http://virtualcity.shuiyingkeji.com/virtualCity/download/gamedate";

    public const string m_inDownUri = "http://192.168.0.154:4050";

    //public const string m_outTestUri = "http://121.199.79.6:8099/?cmd=1071&params={}";
    //public const string m_outUri =      "http://121.199.79.6:8099/?cmd=1071&params={}";
    //public const string m_inTestUri = "http://121.199.79.6:8099/?cmd=1071&params={}";

    public static string m_outTestUri
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return JsonMgr.GetJsonString(Application.streamingAssetsPath + "/ServerUri.txt");
            }
            return "http://121.199.79.6:8099/?cmd=1071&params={}";
        }
    }
    public static string m_outUri
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return JsonMgr.GetJsonString(Application.streamingAssetsPath + "/ServerUri.txt");
            }
            return "http://121.199.79.6:8099/?cmd=1071&params={}";
        }
    }
    public static string m_inTestUri
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return JsonMgr.GetJsonString(Application.streamingAssetsPath + "/ServerUri.txt");
            }
            return "http://121.199.79.6:8099/?cmd=1071&params={}";
        }
    }

    public static string LuaBasePath {
        get { return Application.dataPath + "/uLua/Source/"; }
    }

    public static string GateHttp
    {
        get {
#if UNITY_EDITOR
            switch (EditorServer)
            {
                case EnTestServer.Out:
                    return m_outUri;
                    break;
                case EnTestServer.InTest:
                    return m_inTestUri;
                    break;
                case EnTestServer.OutTest:
                    return m_outTestUri;
                    break;
                default:
                    break;
            }






#else
            switch (m_enTestServer)
            {
                case EnTestServer.InTest:
                    return m_inTestUri;
                    break;
                case EnTestServer.OutTest:
                    return m_outTestUri;
                    break;
                case EnTestServer.Out:
                    return m_outUri;
                    break;
                default:
                    break;
            }
#endif
            return m_inTestUri;
        }
    }
    public static string LuaWrapPath {
        get { return LuaBasePath + "LuaWrap/"; }
    }

    public static string LocalPath
    {
        get {
            string ret = "";

#if UNITY_EDITOR
            //ret = Application.persistentDataPath;
            if (Application.streamingAssetsPath == "F:/Project/trunk/Client/VirtualCity/Assets/StreamingAssets")
            {
                ret = Application.persistentDataPath;
            }
            else if (Application.streamingAssetsPath == "F:/cloneProject/Assets/StreamingAssets")
            {
                ret = "F:/test";
            }
            else
            {
                ret = Application.persistentDataPath;
            }
#else
            ret = Application.persistentDataPath;
#endif
            return ret;
        }
    }
    public static string DataPath
    {
        get
        {
            string contentPath = "";
#if UNITY_EDITOR
            contentPath = Application.streamingAssetsPath ;
#else
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    contentPath = Application.persistentDataPath;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    contentPath = Application.persistentDataPath;
                    break;
                default:
                    contentPath = Application.streamingAssetsPath + "/Windows";
                    break;
            }
#endif
            return contentPath;

        }
    }

    public static string BigVerionSavePath
    {
        get {
            string ret = "";
#if UNITY_EDITOR
            ret = Application.streamingAssetsPath + "/BigVersion";
#else
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    ret = Application.persistentDataPath + "/BigVersion";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    ret = Application.persistentDataPath + "/BigVersion";
                    break;
                default:
                    ret = Application.streamingAssetsPath + "/BigVersion";
                    break;
            }

#endif
            return ret;
        }
    }

    //更新整个apk路径
    public static string BigVersionDownUrl
    {
        get {
            if (m_enTestServer == EnTestServer.InTest)
            {
                return m_inDownUri + "/BigVersion";
            }
            else if (m_enTestServer == EnTestServer.Out)
            {
                return m_outDownUri + "/BigVersion";
            }
            else if (m_enTestServer == EnTestServer.OutTest)
            {
                return m_outDownUri + "/test/BigVersion";
            }
        }

    }
    public static string AbDataPath
    {
        get
        {
            string contentPath = "";
#if UNITY_EDITOR
            //contentPath = Application.streamingAssetsPath + "/Android";
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    contentPath = Application.persistentDataPath;
                    break;
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    contentPath = "D:/Android";
                    break;
                
                default:
                    break;
            }
            
#else
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    contentPath = Application.persistentDataPath + "/Android";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    contentPath = Application.persistentDataPath + "/iOS";
                    break;
                default:
                    contentPath = Application.streamingAssetsPath + "/Windows";
                    break;
            }
#endif
            return contentPath;

        }
    }

    /// <summary>
    /// 应用程序内容路径
    /// </summary>
    public static string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw";
                break;
            default:
                path = Application.dataPath + "/" + AppConst.AssetDirname ;
                break;
        }
        return path;
    }

    public static string DownUri
    {
        get
        {
            string uri = "";

#if UNITY_EDITOR
            switch (EditorServer)
            {
                case EnTestServer.InTest:
                    uri = m_inDownUri;
                    break;
                case EnTestServer.OutTest:
                    uri = m_outDownUri + "/test";
                    break;
                case EnTestServer.Out:
                    uri = m_outDownUri;
                    break;
                default:
                    break;
            }
           

#else
             if (m_enTestServer == EnTestServer.InTest)
            {
                uri = m_inDownUri;
            }
            else if (m_enTestServer == EnTestServer.Out)
            {
                uri = m_outDownUri;
            }
            else if (m_enTestServer == EnTestServer.OutTest)
            {
                uri = m_outDownUri + "/test";
            }

           

            
#endif

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    uri += "/Android";
                    break;
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    uri += "/iOS";
                    break;
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    uri += "/Android";
                    break;
            }
            return uri;
        }

    }


    //沙盘路径,从沙盘路径放到data路径
    public static string StreamingAssetsPath
    {
        get
        {
            string contentPath = "";

#if UNITY_EDITOR
            string sAsset = Application.dataPath;
            sAsset = sAsset.Replace("Assets", "");
            contentPath = sAsset + "Android";
#else
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    contentPath = "jar:file://" + Application.dataPath + "!/assets/Android";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    contentPath = Application.dataPath + "/Raw/iOS";
                    break;
                default:
                    contentPath = Application.dataPath + "/AbTest";
                    //contentPath = Application.streamingAssetsPath + "/Windows";
                    break;
            }
#endif
            return contentPath;
        }
    }
}
