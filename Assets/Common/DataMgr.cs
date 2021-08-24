using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ProtoDefine;


public enum EnBuildMode
{
    Display,
    Build,
}
public class DataMgr : MonoBehaviour {

    public static string m_testUI = Vc.AbName.homepanel;

    static public int m_layerNoRecvLight = 13;
    static public string m_tagDrag = "Drag";
    static public string m_tagHover = "Hover";
    static public string m_tagSmallBtn = "SmallBtn";
    static public string m_tagUi = "Ui";
    static public string m_tagHighLight = "HighLight";
    static public string m_tagUnuse = "Unuse";
    static public string m_tagDrawLine = "DrawLine";
    static public string m_shaTwoSideAlpha = "Custom/TwoSideAlpha";
    static public string m_textOnly = "textOnly";
    static public string m_textAndSpr = "textAndSpr";
    static public string m_gotoScene = "GotoScene";
    static public string m_reScene = "MainFirst";
    static public string m_tagPenMove = "PenMove";//针对ui 平移
	static public string m_tagPenDrag = "PenDrag";//针对ui 滚动层的拖拽
    static public string m_tagPackBox = "m_tagPackBox";
    static public string m_tagPenLimitMove = "PenLimitMove"; //笔限制方向移动
    static public string m_tagUntagged = "Untagged";
    static public string m_tagChildPos = "ChildPos";
    public const string m_tagRemote = "Remote"; //远程玩家
    public const string m_tagFakeRemote = "FakeRemote"; //远程玩家
    public const string m_tagNpc = "Npc";//商店中的npc，点击弹出购买商品界面
    static public int m_layerPackBox = 10;
    static public int m_layerPenDrag = 9;
    static public int m_layerPenMove = 8;
    static public int m_layerPenLimitMove = 11;//限制物体的移动方向，xyz取一个方向
    static public GameObject m_curShowObj = null;//当前场景展示的总的父物体


    public static Dictionary<GameObject, Vector3> m_dicOriPos = new Dictionary<GameObject, Vector3>();
    public static Dictionary<GameObject, Quaternion> m_dicOriQua = new Dictionary<GameObject, Quaternion>();
    public static Dictionary<GameObject, Vector3> m_dicOriEuler = new Dictionary<GameObject, Vector3>();
    public static Dictionary<GameObject, Vector3> m_dicOriScale = new Dictionary<GameObject, Vector3>();

    public static Dictionary<GameObject, Vector3> m_dicOriLocalPos = new Dictionary<GameObject, Vector3>();
    public static Dictionary<GameObject, Quaternion> m_dicOriLocalQua = new Dictionary<GameObject, Quaternion>();
    public static Dictionary<GameObject, Vector3> m_dicOriLocalScale = new Dictionary<GameObject, Vector3>();
    public static Dictionary<GameObject, Vector3> m_dicOriLocalEuler = new Dictionary<GameObject, Vector3>();
    public static List<GameObject> m_listHideObj = new List<GameObject>();
    public static List<Canvas> m_listLabelCanvas = new List<Canvas>();
    static public GameObject m_curSelectObj = null;//当前笔拖动的物体
    static public GameObject m_curAdsorbObj = null;//当前吸附提示物体
    static public GameObject m_curHitObj = null;//当前笔碰撞到的物体
    static public bool m_isPenMidPress = false;
    static public bool m_isPenBigMove = true;
    static public bool m_isPenMidMove = false;//中间是放大功能
    static public bool m_isHighLight = true;
    static public Vector3 m_oriScale = Vector3.zero;
    static public GameObject m_penRightObj = null;
    static public Vector3 m_penEndPos = Vector3.zero;
    static public Vector3 m_penEndAngle = Vector3.zero;

    //static public Vector3 m_abPos = Vector3.zero;
    static public GameObject m_moveObj = null;
    static public GameObject m_moveToObj = null;

    static public Shader m_shader;

    static public int m_ctrlMode = 0;//当0为ppt模式，当1为模型模式
    static public float m_radius = 0.1f;
    static public float m_changeRate = 3.0f;

    static public string m_leftChapterName = "";
    static public bool m_isEnableToPpt = true;
    static public bool m_isEnableDrag = true;
    static public bool m_isEnableRightBigMenu = true;

    static public string m_path = "C:/Ardez/Data";
    static public string m_imgFolderName = "PptImg";


	static public string m_LastScene = "";
	static public string m_LastLastScene = "";
	
    static public bool m_isPreLoad = true;
    static public bool m_isClosePreLoadCnt = false;
    static public bool m_isAutoClickPreLoad = false; //是否自动点击加载界面
    static public bool m_isJianXiu = false;
    static public int m_jianXiuIdx = 0;

    //热更新相关
    static public bool m_isUpdate = false;
    static public bool m_isTotalDown = true;
    static public long m_downTotal = 0;
    static public long m_downCur = 0;
    //右键按钮相关
    static public bool m_isShowPingAndScale = true;
    static public bool m_isShowExplode = true;
    static public bool m_isShowReset = true;
    //string加密相关
    static public string m_rsaPublicKey = "<RSAKeyValue><Modulus>sz9WgkPGw/AD9wCGAFAOyynex8huYDjd7IVWhPEBBwQSvv9wP5u4hnrouxXWaaA72Yth/RnKgFsobnY15bJ4w6e2eGqsmj7idPYVWHi7XnnuJQG84O+7ctUWk06QDzY50Neb+3DSfgSQ0HinK2xBdhk1RqydQXyFBk6sa9tNcQ0=</Modulus><Exponent>EQ==</Exponent></RSAKeyValue>";
    static public string m_rsaPrivateKey = "<RSAKeyValue><Modulus>sz9WgkPGw/AD9wCGAFAOyynex8huYDjd7IVWhPEBBwQSvv9wP5u4hnrouxXWaaA72Yth/RnKgFsobnY15bJ4w6e2eGqsmj7idPYVWHi7XnnuJQG84O+7ctUWk06QDzY50Neb+3DSfgSQ0HinK2xBdhk1RqydQXyFBk6sa9tNcQ0=</Modulus><Exponent>EQ==</Exponent><P>4tL5JePui5WGRq/+kFe+CisKyX/UgGgESatNk/VbernDIdyB1KUik1miXMlNok84PSr6zFa2PL1YTHiBNwr5HQ==</P><Q>yk25nNd5bcpsQD13YYi+aGqm8vfEImA/MGoiQUonf8FnFwVMmgkbEtBLPiuNBoXXXwpcsb/SQ12a6SGKamHEsQ==</Q><DP>UA450SNFIjTF+tS0MvHKmi1PGfDhlrtMzrTuNDh6o8kXsZkew4WTu4kMIL+Ez9+5fwAcSB6arAaXooTiT6mFGQ==</DP><DQ>L5nRUhSVCsY3lqUNB+PwkQoJKhwuJjTDkuvL0yCBw7UJMpfVyeQGXseZO84DEJf2cLck3od8xI5+zXFNvq2XsQ==</DQ><InverseQ>Yw0k1G6k1hR3Fsw0YYhUj+BojITedgoT9aNb/q5PJK58cb+1SwY6cq9/TrS+vWF4zrBAJwzETMndZocF5/cTGw==</InverseQ><D>aXCNPZFHvo0vgjyLDz4m0dxk7f1uGnvN1myNPyRa9RF0cFoFyxBObTk9mzoFp4tuf/ejDVp3HlO9bidq/5Yo6pNnKPlgtCjjW8jqSQQISLkG2mT7k/EHhUSXQgsgUn1/NNXW7UyNy4LTHAnwa74/w9EPBSptXcGnzv+wdNbAaXE=</D></RSAKeyValue>";
    static public bool m_isHaveNet = true; //是否有网

    public static Byte m_abKey = 157;               //ab加密
    public static int m_yzmDjs = 60;

    public static string m_id = ""; //当前登录的电话号
    public static int m_selectItem = 0;

    public static EnBuildMode m_buildMode = EnBuildMode.Display;

    public static Account m_account;

    public static UserOtherData m_userOtherData;
    //public static AccountWallet m_accountWallet;


    //public static List<ProxyUser> m_proxyUser;

    //public static bool m_isAbTest = false;

    /**
* 房屋配置数据
*/
    public static List<HomeProperties> homeProperties;

    public static Dictionary<long, HomeProperties> m_dicHomeProperties = new Dictionary<long, HomeProperties>();
    /**
     * 家具配置数据
     */

    public static List<PartProperties> partProperties;

    public static Dictionary<long, PartProperties> m_dicPartProperties = new Dictionary<long, PartProperties>();
    /**
     * 建筑配置数据
     */
    public static List<DevlopmentProperties> devlopmentProperties;
    public static Dictionary<long, DevlopmentProperties> m_dicDevlopmentProperties = new Dictionary<long, DevlopmentProperties>();
    /**
     * 商业模式配置数据
     */
    public static List<BusinessModelProperties> businessModelProperties;
    public static Dictionary<long, BusinessModelProperties> m_dicBusinessModelProperties = new Dictionary<long, BusinessModelProperties>();


    //当前客户端版本号
    public static string clientVersion = "1.1";


    
    //全部地块信息
    //public static Dictionary<int, Land> m_dicLand = new Dictionary<int, Land>();


    //每块地的开垦花费
    public static Dictionary<string, OpenAreaCast> m_dicOpenAreaCast = new Dictionary<string, OpenAreaCast>();

    //用户角色表
    public static List<RoleProperties> m_listRoleProperties = new List<RoleProperties>();
    public static Dictionary<long, RoleProperties> m_dicRoleProperties = new Dictionary<long, RoleProperties>();

    //商业街商铺表
    public static Dictionary<long, ShopsProperties> m_dicShopsProperties = new Dictionary<long, ShopsProperties>();

    //室外homeunit表
    public static Dictionary<string, string> m_dicHomeUnitOutDoorPage = new Dictionary<string, string>();

    //室内homeunit表
    public static Dictionary<string, string> m_dicHomeUnitInDoorPage = new Dictionary<string, string>();

    

    //当前所在场景
    public static EnCurScene m_curScene = EnCurScene.Hometown;

    public static GameObject m_curSceneObj;

    //当前所在服务器
    public static EnServer m_curServer = EnServer.MyServer;

    //我的还是他的
    public static EnMyOhter m_myOther = EnMyOhter.My;

    //当前所在的他的
    public static ChatUser m_taInfo;

    //单位时间
    public static int m_deveGetGoldUnitTime = 0;

    static public int m_WorldChatConsumeGold = 0;

    //论坛路径
    public static string m_forum_url;

    public static ReqChatLoginMessage m_ReqChatLoginMessage;
    //public static List<ChatUser> m_FriendList;
    //public static List<ChatGroup> m_ChatGroupList;
    public static RspGetSocialityInfoMessage m_RspGetSocialityInfoMessage;
    public static long m_accountId;
    public static List<ProxyUser> m_ProxyUserList;
    public static bool m_isLoginOk = false;
    public static float m_resolution = 0.75f;

    public static int m_friendHPTimes;
    public static int m_slefHPTimes;
    public static float m_reBuildCast;

    public static int m_ShouHuoShiJian;
    public static bool m_isNewHaveLoginInfo = false;
    public static long m_zan;
    public static MoneyTree m_MoneyTree;
    public static bool m_isNewGuide = true;
    public static Dictionary<long, List<string>> m_dicZan = new Dictionary<long, List<string>>();

    public static bool m_isOpenWelcome = false;

    public static string m_newBoxId = "2";

    public static float m_designWidth = 1920;
    public static float m_designHeight = 1080;
}
