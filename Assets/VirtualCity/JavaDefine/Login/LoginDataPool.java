package com.kingston.jforgame.server.game.login;

public class LoginDataPool {

	//cmd请求协议枚举
	/** 请求－登录 */
	public static final short REQ_LOGIN = 1;
	/** 响应-心跳检测*/
	public  static final  short REQ_HEARTBATE = 2;
	/** 请求-加载角色 */
	public static final short REQ_LOAD_PLAYER = 3;
	/**请求-策划数据*/
	public static final short REQ_LOAD_DATE = 4;
	/**请求-异地登录,强制下线*/
	public static final short REQ_LOGINOUT=5;
	/**请求-游戏服务器建立连接*/
	public static final short REQ_CONNECTION=6;

	//cmd响应协议枚举
	/** 响应－登录 */
	public static final short RSP_LOGIN = 501;
	/** 响应-心跳检测*/
	public  static final  short RSP_HEARTBATE = 502;
	/** 响应－加载角色 */
	public static final short RSP_LOAD_PLAYER = 503;
	/**响应-策划数据*/
	public static final short RSP_LOAD_DATE = 504;
	/**响应-异地登录,强制下线*/
	public static final short RSP_LOGINOUT = 505;
	/**响应-游戏服务器建立连接*/
	public static final short RSP_CONNECTION=506;

	/** 登录失败标识 */
	public static final short LOGIN_FAIL = 0;
	/** 登录成功标识 */
	public static final short LOGIN_SUCC = 1;

}
