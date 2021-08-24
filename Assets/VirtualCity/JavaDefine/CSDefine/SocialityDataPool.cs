using System.Collections.Generic;
namespace ProtoDefine {

public class SocialityDataPool {
    //cmd请求协议枚举
    /** 请求－登录聊天服务器 */
    public const  short REQ_LOGIN = 1;
    /** 请求－退出聊天服务器*/
    public const  short REQ_LOGOUT = 2;
    /** 聊天消息*/
    public const  short CHAT = 3;
    /** 请求-搜索用户*/
    public const  short REQ_SEARCHUSER = 4;
    /**请求 - 获取好友信息*/
    public const  short REQ_GETFRIENDINFO=5;
    /**请求 - 创建群*/
    public const  short REQ_CREATEGROUP=6;
    /**请求 - 删除群*/
    public const  short REQ_DELETEGROUP=7;
    /**请求 - 踢出好友*/
    public const  short REQ_DELETEPARAMATE=8;
    /**请求 - 删除好友*/
    public const  short REQ_DELETEFRIEND=9;
    /**请求 - 修改群名称*/
    public const  short REQ_UPDATEGROUP=10;
    /**请求- 获取在线用户展示信息*/
    public const  short REQ_GETPLAYER=11;


    //cmd响应协议枚举
    /** 响应－登录聊天服务器 */
    public const  short RSP_LOGIN = 501;
    /** 响应－退出聊天服务器*/
    public const  short RSP_LOGOUT = 502;
    /** 系统消息*/
    public const  short SYSINFO = 503;
    /** 响应-搜索用户*/
    public const  short RSP_SEARCHUSER = 504;
    /**请求 - 响应好友信息*/
    public const  short RSP_GETFRIENDINFO=505;
    /**响应 - 创建群*/
    public const  short RSP_CREATEGROUP=506;
    /**响应 - 删除群*/
    public const  short RSP_DELETEGROUP=507;
    /**响应 - 踢出好友*/
    public const  short RSP_DELETEPARAMATE=508;
    /**响应 - 删除好友*/
    public const  short RSP_DELETEFRIEND=509;
    /**响应 - 修改群名称*/
    public const  short RSP_UPDATEGROUP=510;
    /**响应- 获取在线用户展示信息*/
    public const  short RSP_GETPLAYER=511;

    /**系统通知类型--加好友*/
    public const  int ADDFRIEND = 1;
    /**系统通知类型--加群*/
    public const  int ADDGROUP = 2;
    /**系统通知类型--其他通知*/
    public const  int OTHERS = 3;


    /** 失败标识 */
    public const  short FAIL = 0;
    /** 成功标识 */
    public const  short SUCC = 1;

    /**消息类型 -- 好友消息*/
    public const  short FRIEND = 1;
    /**消息类型 -- 群消息*/
    public const  short GROUP = 2;
    /**消息类型 -- 世界消息*/
    public const  short WORLD = 3;

}
}