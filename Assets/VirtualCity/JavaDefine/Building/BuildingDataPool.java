package com.kingston.jforgame.server.game.building;

public class BuildingDataPool {
    //cmd请求协议枚举
    /** 请求－建房屋 */
    public static final short REQ_BUILTEHOUSE = 1;
    /** 请求- 拆房屋*/
    public static final short REQ_REMOVEHOUSE= 2;
    /** 请求－开发建筑 */
    public static final short REQ_BUILTEDEVLOPMENT = 3;
    /** 请求－拆除建筑 */
    public static final short REQ_REMOVEDEVLOPMENT = 4;
    /** 请求-开垦土地*/
    public static final short REQ_OPENAREA = 5;
    /** 请求-获取建筑收益状态*/
    public static final short REQ_GETREWARD = 6;



    //cmd响应协议枚举
    /** 响应－建造 */
    public static final short RSP_BUILTEHOUSE = 501;



    /** 失败标识 */
    public static final short FAIL = 0;
    /** 成功标识 */
    public static final short SUCC = 1;

    /**角色信息标识--房屋*/
    public static final short HOUSE = 611;
    /**角色信息标识--土地*/
    public static final short LAND = 612;
    /**角色信息标识--建筑*/
    public static final short DEVLOPMENT = 613;
    /**角色信息标识--家具*/
    public static final short PART = 614;

    /**好友操作标识--帮忙*/
    public static final short HELP = 0;
    /**角色信息标识--收取*/
    public static final short STONE = 1;

    /**建筑状态标识--开发中*/
    public static final short BULIDING = 0;
    /**建筑状态标识--正在收益*/
    public static final short REWARDING = 1;
    /**建筑状态标识--待翻新*/
    public static final short REBUILDING = 3;


}
