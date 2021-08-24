using System.Collections.Generic;
namespace ProtoDefine {

public class SystemDataPool {

	//cmd请求协议枚举
	/** 请求－修改角色信息 */
	public const  short REQ_UPDATEUSERINFO = 1;
	/**请求-获取代理权*/
	public const  short REQ_GETPORXY= 2;
	/**请求-校验推荐码*/
	public const  short REQ_CKCODE= 4;
	/**请求--跨服取数据*/
	public const  short REQ_GETUSERINFO = 5;
	/**请求--获取排行榜*/
	public const  short REQ_GETRANK = 6;
	/**请求--获取钱包数据*/
	public const  short REQ_GETWALLETDATE = 7;
	/**请求 -- 修改公钥*/
	public const  short REQ_UPDATEPUBKEY = 8;
	/**请求 -- 分配场景服务器*/
	public const  short REQ_ALLOCATIONSCENCE = 9;
	/**请求--修改提现密码*/
	public const  short REQ_UPDATEPASSWORD = 10;
	/**请求--修改提现密码*/
	public const  short REQ_CREATEPASSWORD = 11;
	/**请求--获取注册推荐码*/
	public const  short REQ_REGISTERCODE = 12;
    /**请求--修改用户其他数据*/
    public const  short REQ_OTHERDATA = 13;
	/**请求--通过短信验证码修改密码*/
	public const  short REQ_UPBYMESSAGE = 14;


	/**请求--链接其他服务器*/
	public const  short REQ_CONNECT = 98;
	/**响应--链接其他服务器*/
	public const  short RSP_CONNECT = 99;
	/**请求 -- 停服*/
	public const  short REQ_STOPSERVER = 100;



	//cmd响应协议枚举
	/** 响应－修改角色信息 */
	public const  short RSP_UPDATEUSERINFO = 501;
	/**响应-获取代理权*/
	public const  short RSP_GETPORXY= 502;
	/**响应-校验推荐码*/
	public const  short RSP_CKCODE= 504;
	/**响应-通用响应*/
	public const  short RSP_COMMENT= 505;
	/**响应--跨服取数据*/
	public const  short RSP_GETUSERINFO = 506;
	/**响应--获取排行榜*/
	public const  short RSP_GETRANK = 507;
	/**响应--获取钱包数据*/
	public const  short RSP_GETWALLETDATE = 508;
	/**响应 -- 修改公钥*/
	public const  short RSP_UPDATEPUBKEY = 509;
	/**响应 -- 分配场景服务器*/
	public const  short RSP_ALLOCATIONSCENCE = 510;
	/**响应 -- 检验密码*/
	public const  short RSP_CHECKPASSWORD = 511;
	/**响应--修改提现密码*/
	public const  short RSP_UPDATEPASSWORD = 512;
	/**响应--更新代理关系*/
	public const  short RSP_UPDATEPROXY = 513;
	/**响应 -- 给用户发停服消息*/
	public const  short RSP_STOPSERVER = 514;
	/**响应--创建提现密码*/
	public const  short RSP_CREATEPASSWORD = 515;
	/**响应--获取代理权成功*/
	public const  short RSP_GETPROXYYPAY = 516;
	/**响应--获取注册推荐码*/
	public const  short RSP_REGISTERCODE = 517;
	/**响应--收取红包*/
	public const  short RSP_GETREDPACKET = 518;
	/**请求--修改用户其他数据*/
	public const  short RSP_OTHERDATA = 519;
	/**请求--通过短信验证码修改密码*/
	public const  short RSP_UPBYMESSAGE = 520;

	/** 失败标识 */
	public const  short FAIL = 0;
	/** 成功标识 */
	public const  short SUCC = 1;

    /**角色信息标识--支付信息*/
    public const  short USER_PAY_INFO = 601;
    /**角色信息标识--手机号码*/
    public const  short PHONE = 603;
	/**角色信息标识--角色名*/
	public const  short USERNAME = 604;
	/**角色信息标识--模型id*/
	public const  short MOUDLEID = 605;
	/**角色信息标识--收货信息*/
	public const  short ADDRESS = 606;
	/**角色信息标识--性别*/
    public const  short SEX = 609;
	/**角色信息标识--获取代理权*/
	public const  short GETPROXY = 610;


	/**排行榜标识--全服*/
	public const  short SERVER = 1;
	/**排行榜标识--好友*/
	public const  short FRIENDS = 2;

	/**排行榜标识--资产*/
	public const  short ASSET = 802;
	/**排行榜标识--慈善*/
	public const  short DCOST = 803;
	/**排行榜标识--土豪*/
	public const  short INCOME = 804;
	/**排行榜标识--点赞*/
	public const  short ZAN = 805;


}
}